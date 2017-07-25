using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Text;
using System.Data;

namespace UTHtmlTable
{
    class HtmlTable
    {
        private int colLevel = 1;
        private Dictionary<string, HtmlColumn> colHash = new Dictionary<string, HtmlColumn>();

        private StringBuilder tableHtmlString = new StringBuilder();
        private string spliter;
        private DataTable source;
        private TableStyle tableStyle;

        public HtmlTable(DataTable source, string spliter, TableStyle tableStyle)
        {
            this.source = source;
            this.spliter = spliter;
            this.tableStyle = tableStyle;
        }

        public string GetTableHtmlString()
        {
            if (tableHtmlString.Length > 0)
            {
                return tableHtmlString.ToString();
            }

            BuildColumn();

            tableHtmlString.Append(tableStyle.GetTableBeginLabel());

            tableHtmlString.Append(GetHtmlColString());
            tableHtmlString.Append(GetHtmlDataString());

            tableHtmlString.Append(TableStyle.TBEndLabel);

            return tableHtmlString.ToString();
        }

        private string GetHtmlColString()
        {
            StringBuilder[] rows = new StringBuilder[colLevel];
            for (int index = 0; index < colLevel; index++)
            {
                rows[index] = new StringBuilder();
            }

            foreach (HtmlColumn col in colHash.Values)
            {
                BuildHtmlColumnString(rows, col);
            }

            StringBuilder htmlColString = new StringBuilder();
            for (int index = 0; index < colLevel; index++)
            {
                htmlColString.Append(tableStyle.GetCTrBeginLabel()).Append(rows[index].ToString()).Append(TableStyle.TrEndLabel);
            }
            return htmlColString.ToString();
        }

        private string GetHtmlDataString()
        {
            StringBuilder htmlDataString = new StringBuilder();

            List<string> origColNameList = new List<string>();
            foreach (HtmlColumn col in colHash.Values)
            {
                foreach (HtmlColumn leafCol in col.LeafColList)
                {
                    origColNameList.Add(leafCol.ColNameMapping);
                }
            }

            int rowIndex = 0;
            foreach (DataRow row in source.Rows)
            {
                htmlDataString.Append(tableStyle.GetRTrBeginLabel());
                foreach (string origColName in origColNameList)
                {
                    htmlDataString.Append(tableStyle.GetRTdLabel(1, 1, row[origColName].ToString(), rowIndex));
                }
                htmlDataString.Append(TableStyle.TrEndLabel);
                rowIndex++;
            }

            return htmlDataString.ToString();
        }

        private void BuildColumn()
        {
            HtmlColumn.Spliter = spliter;
            foreach (DataColumn dc in source.Columns)
            {
                string origColName = dc.ColumnName;
                string[] colNames = origColName.Split(new string[] { spliter }, StringSplitOptions.RemoveEmptyEntries);
                if (colNames.Length == 0)
                {
                    continue;
                }

                HtmlColumn col = null;
                bool bCreate = false;
                if (colHash.ContainsKey(colNames[0]))
                {
                    col = colHash[colNames[0]];
                    HtmlColumn.UnionCol(col, origColName);
                }
                else
                {
                    col = HtmlColumn.CreateCol(origColName);
                    bCreate = true;
                }

                if (col != null)
                {
                    if (bCreate == true)
                    {
                        colHash.Add(col.Name, col);
                    }
                    if (colLevel < col.MaxLevel)
                    {
                        colLevel = col.MaxLevel;
                    }
                }
            }
        }

        private void BuildHtmlColumnString(StringBuilder[] rowsStore, HtmlColumn col)
        {
            int rowspan = 1;
            string colName = col.Name;

            if (col.ChildCols.Count == 0)
            {
                rowspan = colLevel - col.Level + 1;

                // 为了提高性能，只能在这里处理
                col.AddLeafCol(col);
            }
            else if (col.ChildCols.Count == 1)
            {
                HtmlColumn childCol = col;
                do
                {
                    childCol = childCol.ChildCols.Values.ElementAt(0);
                    childCol.ActualLevel = childCol.ParentCol.ActualLevel;
                    colName += childCol.Name;
                    rowspan++;
                } while (childCol.ChildCols.Count == 1);
                col = childCol;
                if (col.ChildCols.Count == 0)
                {
                    rowspan += colLevel - col.Level;
                    // 为了提高性能，只能在这里处理
                    col.AddLeafCol(col);
                }
            }

            rowsStore[col.ActualLevel - 1].Append(tableStyle.GetCTdLabel(col.ColSpan, rowspan, colName));
            foreach (HtmlColumn childCol in col.ChildCols.Values)
            {
                BuildHtmlColumnString(rowsStore, childCol);
            }
        }
    }

    class HtmlColumn
    {
        public static string Spliter = "_UT_";

        public int Level = 1;
        public int ColSpan = 1;
        public int MaxLevel = 1;
        public int ActualLevel = 1;

        public string ColNameMapping;

        public HtmlColumn RootCol;
        public HtmlColumn ParentCol;
        public List<HtmlColumn> LeafColList;

        public string Name;
        public Dictionary<string, HtmlColumn> ChildCols = new Dictionary<string, HtmlColumn>();

        public static HtmlColumn CreateCol(string origColName)
        {
            HtmlColumn parentCol = new HtmlColumn();
            parentCol.RootCol = parentCol;

            UnionCol(parentCol, origColName);

            return parentCol;
        }

        public static void UnionCol(HtmlColumn srcCol, string origColName)
        {
            string[] colNames = origColName.Split(new string[] { Spliter }, StringSplitOptions.RemoveEmptyEntries);
            if (colNames.Length == 0)
            {
                return;
            }

            srcCol.Name = colNames[0];
            if (srcCol.MaxLevel < colNames.Length)
            {
                srcCol.MaxLevel = colNames.Length;
            }

            HtmlColumn childCol = srcCol;
            for (int i = 1; i < colNames.Length; i++)
            {
                childCol = childCol.AddChildCol(childCol, colNames[i]);
            }

            childCol.ColNameMapping = origColName;
            //childCol.AddLeafCol(childCol);
        }

        private HtmlColumn AddChildCol(HtmlColumn parentCol, string childColName)
        {
            if (parentCol.ChildCols.ContainsKey(childColName))
            {
                return parentCol.ChildCols[childColName];
            }

            HtmlColumn childCol = new HtmlColumn();
            childCol.Name = childColName;
            childCol.RootCol = parentCol.RootCol ?? parentCol;
            childCol.ParentCol = parentCol;
            childCol.Level = parentCol.Level + 1;
            childCol.ActualLevel = childCol.Level;
            childCol.MaxLevel = parentCol.MaxLevel;

            parentCol.ChildCols.Add(childCol.Name, childCol);

            UpdateParentCol(childCol);
            return childCol;
        }

        private void UpdateParentCol(HtmlColumn childCol)
        {
            HtmlColumn parentColTmp = childCol.ParentCol;
            HtmlColumn childColTmp = childCol;
            while (parentColTmp != null)
            {
                if (parentColTmp.ChildCols.Count > 1 || childColTmp.ColSpan > 1)
                {
                    int colSpan = 0;
                    foreach (HtmlColumn col in parentColTmp.ChildCols.Values)
                    {
                        colSpan += col.ColSpan;
                    }

                    parentColTmp.ColSpan = colSpan;
                    childColTmp = parentColTmp;
                    parentColTmp = parentColTmp.ParentCol;
                }
                else
                {
                    break;
                }
            }
        }

        public void AddLeafCol(HtmlColumn leafCol)
        {
            HtmlColumn rootCol = leafCol.RootCol;
            if (rootCol.LeafColList == null)
            {
                rootCol.LeafColList = new List<HtmlColumn>();
            }
            rootCol.LeafColList.Add(leafCol);
        }
    }

    class TableStyle
    {
        public struct StytleAttri
        {
            // 是否需要加入序号
            public bool AutoAequence;
            public string Style;
            public int Border;
            public int CellSpacing;
            public int CellPadding;
            public string OnMouseDown;
            public string Class1;
            public string Class2;
            public string Align;
            public int Height;
            public bool NoWrap;
        }

        private const string TableBeginLabelFormat = "<Table cellspacing='0' cellpadding='0' {0}>";
        public const string TBEndLabel = "</Table>";

        private const string TrBeginLabelFormat = "<Tr {0}>";
        public const string TrEndLabel = "</Tr>";

        private const string TdFormat = "<Td {0} colspan='{1}' rowspan='{2}, id='td{3}'>{3}</Td>";

        public StytleAttri TableStytleAttri;
        public StytleAttri ColumnStytleAttri;
        public StytleAttri ColumnCellStytleAttri;
        public StytleAttri RowStytleAttri;
        public StytleAttri RowCellStytleAttri;

        private string tableBeginLabel = "";
        private string ctrBeginLabel = "";
        private string ctdLabelAttri = "";
        private string rtrBeginLabel = "";
        private string rtdLabel1Attri = "";
        private string rtdLabel2Attri = "";

        public string GetTableBeginLabel()
        {
            if (!string.IsNullOrEmpty(tableBeginLabel))
            {
                return tableBeginLabel;
            }

            tableBeginLabel = string.Format(TableBeginLabelFormat, CombineStyleAttri(TableStytleAttri));
            return tableBeginLabel;
        }

        public string GetCTrBeginLabel()
        {
            if (!string.IsNullOrEmpty(ctrBeginLabel))
            {
                return ctrBeginLabel;
            }

            ctrBeginLabel = string.Format(TrBeginLabelFormat, CombineStyleAttri(ColumnStytleAttri));
            return ctrBeginLabel;
        }

        public string GetCTdLabel(int colspan, int rowspan, string content)
        {
            if (string.IsNullOrEmpty(ctdLabelAttri))
            {
                ctdLabelAttri = CombineStyleAttri(ColumnCellStytleAttri);
            }

            return string.Format(TdFormat, ctdLabelAttri, colspan, rowspan, content);
        }

        public string GetRTrBeginLabel()
        {
            if (!string.IsNullOrEmpty(rtrBeginLabel))
            {
                return rtrBeginLabel;
            }

            ctrBeginLabel = string.Format(TrBeginLabelFormat, CombineStyleAttri(RowStytleAttri));
            return ctrBeginLabel;
        }

        public string GetRTdLabel(int colspan, int rowspan, string content, int rowIndex)
        {
            if (!string.IsNullOrEmpty(RowCellStytleAttri.Class2))
            {
                if (rowIndex % 2 == 0)
                {
                    return GetRTdLabel1(colspan, rowspan, content);
                }
                else
                {
                    return GetRTdLabel2(colspan, rowspan, content);
                }
            }
            else
            {
                return GetRTdLabel1(colspan, rowspan, content);
            }
        }

        private string GetRTdLabel1(int colspan, int rowspan, string content)
        {
            if (string.IsNullOrEmpty(rtdLabel1Attri))
            {
                rtdLabel1Attri = CombineStyleAttri(RowCellStytleAttri);
            }

            return string.Format(TdFormat, rtdLabel1Attri, colspan, rowspan, content); ;
        }

        private string GetRTdLabel2(int colspan, int rowspan, string content)
        {
            if (string.IsNullOrEmpty(rtdLabel2Attri))
            {
                StytleAttri saTmp = RowCellStytleAttri;
                saTmp.Class1 = RowCellStytleAttri.Class2;
                rtdLabel2Attri = CombineStyleAttri(saTmp);
            }

            return string.Format(TdFormat, rtdLabel2Attri, colspan, rowspan, content); ;
        }

        private string CombineStyleAttri(StytleAttri sa)
        {
            StringBuilder str = new StringBuilder();
            if (!string.IsNullOrEmpty(sa.Style))
            {
                str.Append(string.Format("style='{0}' ", sa.Style));
            }

            if (sa.Border > 0)
            {
                str.Append(string.Format("border='{0}' ", sa.Border));
            }

            if (sa.CellPadding > 0)
            {
                str.Append(string.Format("cellspacing='{0}' ", sa.CellPadding));
            }

            if (sa.CellSpacing > 0)
            {
                str.Append(string.Format("cellpadding='{0}' ", sa.CellSpacing));
            }

            if (!string.IsNullOrEmpty(sa.Class1))
            {
                str.Append(string.Format("class='{0}' ", sa.Class1));
            }

            if (!string.IsNullOrEmpty(sa.Align))
            {
                str.Append(string.Format("align='{0}' ", sa.Align));
            }

            if (sa.Height > 0)
            {
                str.Append(string.Format("height='{0}' ", sa.Height));
            }

            if (sa.NoWrap)
            {
                str.Append("nowrap ");
            }

            return str.ToString();
        }
    }
}