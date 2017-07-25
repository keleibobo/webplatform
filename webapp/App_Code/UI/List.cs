using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using System.Text;
using UTDtBaseSvr;

namespace AppCode
{
    /// <summary>
    ///List 的摘要说明
    /// </summary>
    public class List
    {
        public List()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            // 

        }

        /// <summary>
        /// 分页总页树
        /// </summary>
        //public static string pagesum="";
        public static Dictionary<string, string> pagesum = new Dictionary<string, string>();


        /// <summary>
        /// 表格组件与分页组件的关联
        /// </summary>
        public static Dictionary<string, string> tablepage = new Dictionary<string, string>();

        /// <summary>
        /// 带分页组件的表格每页显示的页数
        /// </summary>
        public static Dictionary<string, int> pagesizes = new Dictionary<string, int>();

        public static string pagesize = ReadConfig.TheReadConfig["pagesize"]; //默认

        public static void addPageSum(String targetId, string sum)
        {
            if (pagesum.ContainsKey(targetId))
            {
                pagesum[targetId] = sum;
            }
            else
            {
                pagesum.Add(targetId, sum);
            }
        }



        public static string GetHtmlData(string ComponentID, UTDtBusiness.BsDataTable BsdtOrc, BusinessCall bscall,long total,long pagenum)
        {
            String clickfunc = getClickFunc(ComponentID, bscall.bcEventList);

            string TableComponentID = ComponentID;
            DataTable dtOrc = BsdtOrc.dt;
            Hashtable htExternInfo = new Hashtable();
            Hashtable htDataTableInfo = new Hashtable();
            foreach (UTDtBusiness.ExternInfo ei in BsdtOrc.ltExternInfo)
            {
                htExternInfo.Add(ei.x.ToString() + "-" + ei.y.ToString(), ei);
            }

            foreach (UTDtCnvrtTable.DataTableInfo dti in BsdtOrc.ltColumnInfo)
            {
                htDataTableInfo.Add(dti.ColumnIndex, dti);
            }
            int ColumnIndex = 0;
            int RowIndex = 0;
            //   string componentidsourceid = ComponentID.Replace("component_", "");
            string componentidsourceid = ComponentID.Split('_')[1];

            Dictionary<string, object> extendparam = GetTablextend(componentidsourceid, bscall);
            StringBuilder sHtml = new StringBuilder();
            if (dtOrc != null)
            {
                if (tablepage.ContainsKey(ComponentID))
                {
                    string tpage = "";

                    tpage = String.Format("<input id='tablepagesum{0}' type='hidden' value='{1}' /> ", tablepage[ComponentID], pagenum);
                    tpage += String.Format("<input id='table{0}total' type='hidden' value='{1}' /> ",ComponentID, total);
                    sHtml.Append(tpage);
                }

                string displayvalue = "";
                if (extendparam.ContainsKey("visibletablecolumnname"))
                {
                    if (extendparam["visibletablecolumnname"].ToString().Equals("0"))
                    {
                        displayvalue = "display: none;";
                    }
                }
                sHtml.Append("<table style='width:100%;border:1px' border='0' cellpadding='0' cellspacing='0'>");
                sHtml.Append(String.Format("<tr height='20' style='background:url(../images/{0}/line04.gif); {1} '>", ReadConfig.TheReadConfig["style"], displayvalue));
                sHtml.Append(addExtendTitle(extendparam, ""));
                for (int x = 0; x < dtOrc.Columns.Count; x++)
                {
                    ColumnIndex = x + 1;
                    UTDtCnvrtTable.DataTableInfo DataTableInfo = (UTDtCnvrtTable.DataTableInfo)htDataTableInfo[ColumnIndex];
                    sHtml.Append("<td nowrap sqlId='" + DataTableInfo.FieldName + "' " + GetStyle(0, ColumnIndex, htExternInfo, htDataTableInfo) + " class=\"tdhead\"> ");
                    sHtml.Append(dtOrc.Columns[x].ColumnName);
                    sHtml.Append("</td>");
                }
                sHtml.Append(addExtendTitle(extendparam, "-"));
                sHtml.Append("</tr>");

                string bgColor = "";
                string std = "";



                int count = dtOrc.Rows.Count;
                int size = getPagesize(componentidsourceid);

                //for (int i = count; i < size; i++)
                //{
                //    DataRow row = dtOrc.NewRow();
                //    dtOrc.Rows.Add(row);
                //}


                string SessionParam = "";
                for (int y = 0; y < dtOrc.Rows.Count; y++)
                {
                    if (BsdtOrc.ltExternInfo.Count == 0)
                    {
                        if (y % 2 == 0)
                        {
                            bgColor = "class='t0'";
                            std = "class='td0'";
                        }
                        else
                        {
                            bgColor = "class='t1'";
                            std = "class='td1'";
                        }
                    }
                    DataRow dr1 = dtOrc.Rows[y];
                    string dbparam = "";
                    foreach (UTDtCnvrtTable.DataTableInfo dti in BsdtOrc.ltColumnInfo)
                    {
                        if (dti.ReturnFlag && dti.ColumnIndex!=0)
                        {
                            string svalue = "";
                            if (dtOrc.Rows[y][dti.ColumnIndex - 1] != null)
                            {
                                svalue = dtOrc.Rows[y][dti.ColumnIndex - 1].ToString().Trim();
                            }
                            if (BsdtOrc.ltColumnInfo[0].ReturnFlag == false)
                            {
                                dbparam += "&" + HttpUtility.UrlEncode(dti.FieldName) + "=" + HttpUtility.UrlEncode(svalue);
                                SessionParam += "&" + dti.FieldName + "=" + svalue;
                            }
                            else
                            {
                                dbparam += "&" + dti.ClassName + "." + HttpUtility.UrlEncode(dti.FieldName) + "=" + HttpUtility.UrlEncode(svalue);
                            }
                        }
                    }
                    SessionParam += ";";
                    string StrChoose = "onclick='f_choosetr(this,\"" + TableComponentID + "\")' reportparam='"+dbparam+"'  onmouseout='removeTableCursor(this)' onmouseover='addTableCursor(this)'";
                    string listtrid = "listtr" + ComponentID + "_" + y.ToString();
                    foreach (BusinessComponentEventCall BusinessComponentEventCall1 in bscall.bcEventList)
                    {
                        if (BusinessComponentEventCall1.source_id == componentidsourceid)
                        {
                            StrChoose = "{4}='{3}(\"{0}\",\"{2}\",\"{1}\",\"";
                            string targetid = LayoutUI.GetDestId(BusinessComponentEventCall1.source_id, bscall.bcEventList, bscall.bComponentList);
                            StrChoose = String.Format(StrChoose, targetid, listtrid, ComponentID, clickfunc, BusinessComponentEventCall1.eventtype);

                            foreach (UTDtCnvrtTable.DataTableInfo dti in BsdtOrc.ltColumnInfo)
                            {
                                if (dti.ReturnFlag && dti.ColumnIndex != 0)
                                {
                                    string svalue = "";
                                    if (dtOrc.Rows[y][dti.ColumnIndex - 1] != null)
                                    {
                                        svalue = dtOrc.Rows[y][dti.ColumnIndex - 1].ToString().Trim();
                                    }
                                    if (BsdtOrc.ltColumnInfo[0].ReturnFlag == false)
                                    {
                                        StrChoose += "&" + HttpUtility.UrlEncode(dti.FieldName) + "=" + HttpUtility.UrlEncode(svalue);
                                    }
                                    else
                                    {
                                        StrChoose += ";" + dti.ClassName + "." + HttpUtility.UrlEncode(dti.FieldName) + "=" + HttpUtility.UrlEncode(svalue);
                                    }
                                   
                                }
                            }
                            StrChoose += ";\",this )' ";
                            break;
                        }
                    }
                    sHtml.Append("<tr " + bgColor + " id='" + listtrid + "' " + StrChoose + "  style='cursor:hand'>");
                    bool flag = y < count;

                    sHtml.Append(addExtend(extendparam, std, "", flag));

                    for (int x = 0; x < dtOrc.Columns.Count; x++)
                    {
                        RowIndex = y + 1;
                        ColumnIndex = x + 1;
                        sHtml.Append("<td  " + std + " " + GetStyle(RowIndex, ColumnIndex, htExternInfo, htDataTableInfo) + " >");
                        sHtml.Append(dr1[x].ToString());//System.Web.HttpUtility.UrlEncode(
                        sHtml.Append("</td>");
                    }
                    if (y < count)
                        sHtml.Append(addExtend(extendparam, std, "-", flag));
                    sHtml.Append("</tr>");
                }

                if (SessionParam.Length > 1) //报表参数缓存
                {
                    SessionParam = SessionParam.Substring(0, SessionParam.Length - 1);
                    HttpContext.Current.Session[TableComponentID] = SessionParam;      
                }

                sHtml.Append("</table> <input id='" + "listoldrow" + ComponentID + "' value='' type='hidden' /><input type='hidden' id='rowcount' value='" + dtOrc.Rows.Count + "'/>");
                if (extendparam.ContainsKey("excelnav"))
                {
                    sHtml.Append("<input id='" + "" + ComponentID+"_excelnav" + "' value='" + extendparam["excelnav"] + "' type='hidden' />");
                }


            }
            return sHtml.ToString();
        }

        public static String getClickFunc(String ComponentID, List<BusinessComponentEventCall> bceList)
        {
            String rs = "f_chooserow"; //默认
            String cID = ComponentID.Split('_')[1];
            foreach (BusinessComponentEventCall beCall in bceList)
            {
                if (beCall.source_id.Equals(cID))
                {
                    rs = beCall.eventfuncname;
                    break;
                }

            }
            return rs;
        }

        public static Dictionary<string, object> GetTablextend(string Id, BusinessCall bscall)
        {
            Dictionary<string, object> extendMap = new Dictionary<string, object>();
            string ctype = LayoutUI.GetComponentType(bscall, Id);
            foreach (BusinessComponentCall bc in bscall.bComponentList)
            {
                if (bc.id.Equals(Id))
                {
                    if (ctype.Equals(HtmlComponetType.querydatatable.ToString()) || ctype.Equals(HtmlComponetType.editdatatable.ToString()))
                    {
                        extendMap = LayoutUI.getParam(bc.extendparam, ';', '=');
                    }
                    break;
                }
            }
            return extendMap;
        }

        /// <summary>
        /// 表格 列头/尾 加字段
        /// 头 0,1...
        /// 尾 -0，-1
        /// </summary>
        /// <param name="extendparam"></param>
        /// <returns></returns>
        private static string addExtendTitle(Dictionary<string, object> extendparam, String index)
        {
            StringBuilder sb = new StringBuilder();
            string[] columns = new string[extendparam.Count];

            foreach (KeyValuePair<string, object> kvp in extendparam)
            {
                if (index.Equals("-"))
                {

                }
                else
                {
                    if (kvp.Key.Equals("checkbox"))
                    {
                        int id = 0;
                        try
                        {
                            id = Convert.ToInt32(kvp.Value);
                        }
                        catch (Exception e)
                        {
                        }
                        columns[id] = "<td class='tdhead'></td>";
                    }
                }
            }

            foreach (string str in columns)
            {
                if (str != null)
                {
                    sb.Append(str);
                }

            }

            return sb.ToString();
        }

        /// <summary>
        /// 表格 数据头/尾 加单元格（checkbox button等）
        /// 头 0,1...
        /// 尾 -0，-1
        /// </summary>
        /// <param name="extendparam"></param>
        /// <returns></returns>
        private static string addExtend(Dictionary<string, object> extendparam, string attr, String index, bool flag)
        {
            StringBuilder sb = new StringBuilder();
            string[] columns = new string[extendparam.Count];

            foreach (KeyValuePair<string, object> kvp in extendparam)
            {
                if (index.Equals("-"))
                {

                }
                else
                {


                    int id = 0;
                    try
                    {
                        if (flag&&kvp.Key.Equals("checkbox")) //仅checkbox
                        {
                            id = Convert.ToInt32(kvp.Value);
                            columns[id] = String.Format("<td {1} ><input type='{0}' /></td>", kvp.Key, attr);
                        }
                        else
                        {
                           // columns[id] = String.Format("<td {0} ></td>", attr);
                        }

                    }
                    catch (Exception e)
                    {
                    }


                }
            }

            foreach (string str in columns)
            {
                if (str != null)
                    sb.Append(str);
            }

            return sb.ToString();
        }

        public static void setPagesize(String Id)
        {
            if (AppCode.List.pagesizes.ContainsKey(Id)) return;
            BusinessCall bcCall = (BusinessCall)HttpContext.Current.Session["bcCall"];

            foreach (BusinessComponentLayoutCall bclc in bcCall.bcLayoutList)
            {
                if (bclc.componentid == Id && bclc.componentlayout != null)
                {
                    string rs = bclc.componentlayout;
                    Dictionary<string, object> obj = (Dictionary<string, object>)FormatUtil.fromJSON(bclc.componentlayout);
                    if (obj != null && obj.ContainsKey("pagesize"))
                    {
                        AppCode.List.pagesizes.Add(Id, (int)obj["pagesize"]);
                    }
                    else
                    {
                        AppCode.List.pagesizes.Add(Id, Convert.ToInt32(ReadConfig.TheReadConfig["pagesize"]));
                        ;
                    }
                }
            }
        }

        private static int getPagesize(string componentid)
        {
            int size = 0;
            try
            {
                if (pagesizes.ContainsKey(componentid))
                {
                    size = Convert.ToInt32(pagesizes[componentid]);
                }
                else
                    size = Convert.ToInt32(pagesize);
            }
            catch (Exception e) { }



            return size;
        }

        public static string GetStyle(int rowindex, int columnindex, Hashtable htExternInfo, Hashtable htDataTableInfo)
        {
            string sRt = " style ='padding-left:8px;";
            UTDtBusiness.ExternInfo ei = null;
            string skey = rowindex.ToString() + "-" + columnindex.ToString();
            if (htExternInfo.ContainsKey(skey))
            {
                ei = (UTDtBusiness.ExternInfo)htExternInfo[skey];
                if (ei.Param != "")
                {
                   // Dictionary<string, object> DC = LayoutUI.getParam(ei.Param, ';', '=');
                    sRt += ei.Param;
                }
            }
            UTDtCnvrtTable.DataTableInfo dti = null;
            if (htDataTableInfo.ContainsKey(columnindex))
            {
                dti = (UTDtCnvrtTable.DataTableInfo)htDataTableInfo[columnindex];
                if (!dti.ShowFlag)
                {
                    sRt += "display: none;";
                }
            }

            sRt += ";'";
            return sRt;
        }



    }



}