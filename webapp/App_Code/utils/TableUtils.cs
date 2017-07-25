using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace AppCode
{
    public class TableUtils
    {
        String NO = "-1"; //无序号

   //     public string table;
  //      DataTable dt;

       private static TableUtils instance = new TableUtils();

        private TableUtils() {
           
            }

        public void initTest()
        {
           DataTable dt = new DataTable();
            dt.Columns.Add("类型");
            dt.Columns.Add("缺口");
            dt.Columns.Add("方案");
            dt.Columns.Add("周一");
            dt.Columns.Add("周二");

            DataRow dr = dt.NewRow();
            dr["类型"] = "高耗能";
            dr["缺口"] = "10以内";
            dr["方案"] = "开六停一";
            dr["周一"] = "停";
            dr["周二"] = "正常";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["类型"] = "高耗能";
            dr["缺口"] = "10以内";
            dr["方案"] = "错峰负荷";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["类型"] = "高耗能";
            dr["缺口"] = "10-20";
            dr["方案"] = "开五停二";
      
            dr["周二"] = "停";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["类型"] = "高耗能";
            dr["缺口"] = "10-20";
            dr["方案"] = "错峰负荷";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["类型"]="合计";
            dr["方案"] = "合计";
            dt.Rows.Add(dr);

            string header = getTH(dt);
            List<String> mInfo = load("F:/1.csv");
            toTable(dt, mInfo);
      

        }
        /// <summary>
        /// 或者表头默认占两行datarow?
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private string getTH(DataTable dt)
        {
            string split = "-UT_";
            StringBuilder rt = new StringBuilder();
            rt.Append("<tr>");
            List<String> colName = new List<String>();
            foreach (DataColumn dc in dt.Columns)
            {
              //  colName.Add(dc.ColumnName);
                rt.Append(String.Format("<th>{0}</th>",dc.ColumnName));//简单 ，未处理多级表头
            }

            rt.Append("</tr>");
            return rt.ToString();
        }

      


      

        public List<String> load(string strpath)
        {
            StreamReader mysr = new StreamReader(strpath, System.Text.Encoding.Default);
            string strline = "";
            List<String> rt = new List<string>();


            while ((strline = mysr.ReadLine()) != null)
            {
                rt.Add(strline);
            }

            return rt;
        }

        public static String toTable(DataTable dtbody)
        {
            return toTable(dtbody, null);
        }

        public static String toTable(DataTable dtbody, List<String> aMergeInfo)
        {
            return instance.formatTable(dtbody,aMergeInfo);
        }

        private string formatTable(DataTable dtbody)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@" <table  style='width:100%;'   cellpadding='0' cellspacing='0'  >");

            //     foreach (DataRow dr in dtbody.Rows) { 

            //      }

            ///表头
            string header = getTH(dtbody);
            sb.Append(header);

            for (int i = 0; i < dtbody.Rows.Count; i++)
            {
                sb.Append("<tr>");
                for (int j = 0; j < dtbody.Columns.Count; j++)
                {
                    if (dtbody.Rows[i][j].ToString().Equals(""))
                    {//空值
                        continue;
                    }
                    sb.Append(dtbody.Rows[i][j]);
                }
                sb.Append("</tr>");
            }

            sb.Append("</table>");
            //  table = sb.ToString();
            return sb.ToString();
        }


        private String formatTable(DataTable dtbody, List<String> aMergeInfo)
        {

            String rt = "";
            DataTable dt = new DataTable();
            String[] head = getHead(dtbody);
            foreach (string h in head)
            {
                dt.Columns.Add(h);
            }


            for (int i = 0; i < dtbody.Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < dtbody.Columns.Count; j++)
                {
                    ///空值错位
                    //     if (dtbody.Rows[i][j].Equals("")) continue;
                    //    dtbody.Rows[i][j] = String.Format("<td>&nbsp;{0}&nbsp;</td>", dtbody.Rows[i][j]); //&nbsp; 空值时显示单元格
                    dr[j] = String.Format("<td>&nbsp;{0}&nbsp;</td>", dtbody.Rows[i][j]);
                }

                dt.Rows.Add(dr);

            }

            if (aMergeInfo != null)
            {

                formatMerge(dt, aMergeInfo);
            }

            rt = formatTable(dt);

            return rt;
        }

        private string[] getHead(DataTable dt)
        {
            int size = dt.Columns.Count;
            String[] head = new String[size];
            for (int i = 0; i < size; i++)
            {
                head[i] = dt.Columns[i].ColumnName;
            }

            return head;
        }



        /// <summary>
        /// 处理表格行列的合并
        /// </summary>
        /// <param name="dtbody"></param>
        /// <param name="aMergeInfo"></param>
        private void formatMerge(DataTable dtbody, List<String> aMergeInfo)
        {
            for (int i = 0; i < aMergeInfo.Count; i++)
            {
                string[] split = aMergeInfo[i].Split(',');
                string[] span = split[0].Split('-');
                string pan = "";
                int ir, ic, ie;
                if (span.Length == 2)
                {
                    ir = Convert.ToInt32(span[0]);
                    ic = Convert.ToInt32(split[1]);
                    ie = Convert.ToInt32(span[1]);

                    for (int j = ir; j < ie; j++) //空值
                    {
                        dtbody.Rows[j][ic + NO] = ""; //-1 无序号
                    }
                    pan = String.Format(" rowspan='{0}' ", ie - ir + 1);
                }
                else
                {
                    span = split[1].Split('-');
                    ir = Convert.ToInt32(split[0]);
                    ic = Convert.ToInt32(span[0]);
                    ie = Convert.ToInt32(span[1]);
                    for (int j = ic; j < ie; j++) //空值 无序号
                    {
                        dtbody.Rows[ir - 1][j] = "";
                    }

                    pan = String.Format(" colspan='{0}' ", ie - ic + 1);

                }

                //多<td></td>
                string td = dtbody.Rows[ir - 1][ic + NO].ToString(); //ic -1 无序号
                td = td.Substring(0, 3) + pan + td.Substring(3); //<td
                dtbody.Rows[ir - 1][ic + NO] = td; // ic -1无序号

            }
        }
    }
}
