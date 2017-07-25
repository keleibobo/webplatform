using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
using AppCode;
using UTDtBaseSvr;
using UserRightObj;

public partial class UI_WebUpdateItemControl : System.Web.UI.UserControl
{
    public string componentname = "";
    public string ComponentName
    {
        set
        {
            componentname = value;
        }
        get
        {
            return componentname;
        }
    }

    public string businessname = "";
    public string BusinessName
    {
        set
        {
            businessname = value;
        }
        get
        {
            return businessname;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected override void Render(HtmlTextWriter output)
    {
        if (Request["type"] != "")
        {
            ComponentName = Request["type"];
        }

        string NameStr = Request.QueryString["name"];
        string ValueStr = Request.QueryString["value"];
        string[] NameList = Regex.Split(NameStr, "-ut_", RegexOptions.IgnoreCase);
        string[] ValueList = Regex.Split(ValueStr, "-ut_", RegexOptions.IgnoreCase);
        Dictionary<string, string> VD = new Dictionary<string, string>();

        for (int i = 0; i < NameList.Length; i++)
        {
            if (NameList[i] != "")
            {
                VD.Add(NameList[i].ToLower(), ValueList[i]);
            }
        }
   
        List<TableInfo> TbInfoList = new List<TableInfo>();
        List<TableinfoEvent> TbInfoEvList = new List<TableinfoEvent>();
        BusinessCall bcCall = (BusinessCall)HttpContext.Current.Session["bcCall"];
        if (bcCall != null)
        {
            businessname = bcCall.BussinessName;
            if (bcCall.bComponentList.Count > 0)
            {
                for (int k = 0; k < bcCall.bComponentList.Count; k++)
                {
                    if (bcCall.bComponentList[k].name == ComponentName)
                    {
                        TbInfoList = bcCall.bComponentList[k].TableInfoList;
                        TbInfoEvList = bcCall.bComponentList[k].TableInfoEventList;
                    }
                }
            }
        }

        List<BusinessInfo> BI = InitModel.businessinfoes;
        List<extraparaminfo> EP = InitModel.ExtendParamList;
        Dictionary<string, extraparaminfo> DEP = new Dictionary<string, extraparaminfo>();
        foreach (extraparaminfo p in EP)
        {
            DEP.Add(p.name.ToLower(), p);
        }
        
        if (TbInfoList.Count > 0)
        {

            string SQLStr = string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.Append("<div><table style='width: 100%;height:100%;overflow:hidden' border='0' cellpadding='0' cellspacing='0' bgcolor='#FFFFFF'  class='TextBorder01'>");
            foreach (TableInfo row in TbInfoList)
            {
                if (ReadConfig.TheReadConfig["URAppName"] == ReadConfig.TheReadConfig["appname"])
                {
                    row.appname = ReadConfig.TheReadConfig["URAppName"];
                }
                string paramstr = businessname + ";" + componentname + ";" + row.ColumnName + ";" + row.appname;
                string columnName = row.ColumnName;
                string columnDes = row.ColumnDes;
                bool IScontract = false;
                foreach (TableinfoEvent te in TbInfoEvList)
                {
                    if (te.src == columnName)
                    {
                        IScontract = true;
                    }
                }
                bool HasSrc = false;
                foreach (TableinfoEvent te in TbInfoEvList)
                {
                    if (te.dest == columnName)
                    {
                        HasSrc = true;
                    }
                }

                string temp = Convert.ToBoolean(row.ColumnEnableEmpty.ToString() == "True" ? "true" : "false") ? "" : "(*)";
                if ( row.ColumnIsHidden !=true )
                {
                    sb.Append(string.Format("<tr valign='middle' height='{0}%' id='tr{1}' >", 100 / (TbInfoList.Count + 1), columnName));
                }
                else
                {
                    sb.Append(string.Format("<tr valign='middle' height='{0}%' id='tr{1}' style='display:none;'>", 100 / (TbInfoList.Count + 1), columnName));
                }
                sb.Append(string.Format("<span id='span{0}'><td align=right width='30%' class='tdtitle5'>", columnName));
                if (row.ColumnDes != "")
                {
                    sb.Append(Convert.ToString(row.ColumnDes).Replace("[", "").Replace("]", "") + temp + ":&nbsp; ");
                }
                sb.Append("</td>");
                sb.Append(string.Format("<td align=left valign='middle'  class='tdtitle5' id='td{0}' >", columnName));
                string st = "";
                bool EnableEmpty = row.ColumnEnableEmpty.ToString() == "True" ? true : false;

                switch (Convert.ToString(row.ColumnType))
                {
                    case "double":
                    case "int":
                        st = HtmlControl.GetControl_Int(columnName, EnableEmpty, columnDes, VD[columnName.ToLower()], false);
                        sb.Append(st);
                        break;
                    case "multiselect":
                        StringBuilder htmlStr = new StringBuilder();
                        sb.Append(htmlStr);
                        break;
                    case "datetime":
                        st = HtmlControl.GetControl_DateTime(columnName, EnableEmpty, columnDes, VD[columnName.ToLower()], false);
                        sb.Append(st);
                        break;
                    case "time":
                        st = HtmlControl.GetControl_Time(columnName, EnableEmpty, columnDes, VD[columnName.ToLower()], false);
                        sb.Append(st);
                        break;
                    case "varchar":
                        bool CanEdit = Convert.ToBoolean(row.ColumnIsPrimary);
                        string ExentdName = "";
                        if (row.extendpara.Split('=').Length > 1)
                        {
                            ExentdName = row.extendpara.Split('=')[1];
                        }
                        if (DEP.ContainsKey(ExentdName.ToLower()))
                        {
                            if (DEP[columnName.ToLower()].datasource != "" && DEP[columnName.ToLower()].sourcemethod != "")
                            {
                                row.datasource = DEP[columnName.ToLower()].datasource;
                                row.sourcemethod = DEP[columnName.ToLower()].sourcemethod;
                            }
                        }
                        if (DEP.ContainsKey(ExentdName.ToLower()))
                        {
                            columnName = "extendpro" + columnName;
                        }
                        if (row.datasource != null && row.datasource != "" && !IScontract && !HasSrc)
                        {
                            if (VD.ContainsKey(row.ColumnName.ToLower()))
                            {
                                st = HtmlControl.GetControl_DWbyValue(columnName, VD[row.ColumnName.ToLower()], paramstr);
                            }
                            else
                            {
                                st = HtmlControl.GetControl_DWbyValue(columnName, "", paramstr);
                            }
                        }

                        else if (row.datasource != null && row.datasource != "" && IScontract)
                        {
                            foreach (TableinfoEvent te in TbInfoEvList)
                            {
                                if (te.src == row.ColumnName)
                                {
                                    foreach (TableInfo ti in TbInfoList)
                                    {
                                        if (ti.ColumnName == te.src)
                                        {
                                            foreach (TableInfo dest in TbInfoList)
                                            {
                                                if (dest.ColumnName == te.dest)
                                                {
                                                    string destcolumnName = dest.ColumnName;
                                                    if (columnName.Contains("extendpro"))
                                                    {
                                                        destcolumnName = "extendpro" + dest.ColumnName;
                                                    }
                                                    if (VD.ContainsKey(row.ColumnName.ToLower()))
                                                    {
                                                        st = HtmlControl.GetControl_DWbyValueBind(columnName, row.datasource,dest.sourcemethod, destcolumnName, VD[row.ColumnName.ToLower()], paramstr);
                                                    }
                                                    else
                                                    {
                                                        st = HtmlControl.GetControl_DWbyValueBind(columnName, row.datasource,dest.sourcemethod, destcolumnName, "", paramstr);                                                    
                                                    }
                                                    break;
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }
                        else if (row.datasource != null && row.datasource != "" && HasSrc)
                        {
                            foreach (TableinfoEvent te in TbInfoEvList)
                            {
                                if (te.dest == row.ColumnName)
                                {
                                    foreach (TableInfo ti in TbInfoList)
                                    {
                                        if (ti.ColumnName == te.src)
                                        {
                                            USWebService us = new USWebService();
                                            if (VD.ContainsKey(ti.ColumnName.ToLower()) && VD.ContainsKey(row.ColumnName.ToLower()))
                                            {
                                                st = HtmlControl.GetControl_WebServicesSelect(columnName, row.datasource, row.sourcemethod, te.src + "=" + VD[ti.ColumnName.ToLower()], VD[row.ColumnName.ToLower()], paramstr);
                                            }
                                            else
                                            {
                                                st = HtmlControl.GetControl_WebServicesSelect(columnName, row.datasource, row.sourcemethod, te.src + "=","", paramstr);
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (VD.ContainsKey(row.ColumnName.ToLower()))
                            {
                                st = HtmlControl.GetControl_Varchar(columnName, EnableEmpty, columnDes, VD[row.ColumnName.ToLower()], CanEdit);
                            }
                            else
                            {
                                st = HtmlControl.GetControl_Varchar(columnName, EnableEmpty, columnDes, "", CanEdit);
                            }
                        }
                        sb.Append(st);
                        break;
                    case "checktable":
                        ExentdName = "";
                        if (row.extendpara.Split('=').Length > 1)
                        {
                            ExentdName = row.extendpara.Split('=')[1];
                        }
                        if (DEP.ContainsKey(ExentdName.ToLower()))
                        {
                            if (DEP[ExentdName.ToLower()].datasource != "" && DEP[ExentdName.ToLower()].sourcemethod != "")
                            {
                                row.datasource = DEP[ExentdName.ToLower()].datasource;
                                row.sourcemethod = DEP[ExentdName.ToLower()].sourcemethod;
                            }
                        }
                        if (DEP.ContainsKey(ExentdName.ToLower()))
                        {
                            columnName = "extendpro" + columnName;
                        }
                        if (row.datasource != null && row.datasource != "" && !IScontract && !HasSrc)
                        {
                            st = HtmlControl.GetDataGridUI(columnName, paramstr, VD[row.ColumnName.ToLower()]);
                        }
                        sb.Append(st);
                        break;
                    case "float":
                    case "numeric":
                        st = HtmlControl.GetControl_Numeric(columnName, EnableEmpty, columnDes, VD[columnName.ToLower()], false);
                        sb.Append(st);
                        break;
                    case "img":
                        st = HtmlControl.GetControl_UploadFile();
                        sb.Append(st);
                        break;
                    case "password":
                        st = HtmlControl.GetControl_NewPwd(columnName, EnableEmpty, columnDes, 50 / (TbInfoList.Count + 1));
                        sb.Append(st);
                        break;
                    default:
                        break;
                }
                sb.Append("</td></span>");
                sb.Append("</tr>");
            }
            sb.Append("<tr valign='middle'>");
          //  sb.Append("<td align=right></td>");
            sb.Append("<td colspan=2><div style='margin:0 auto;width:200px'>" 
           //     "<img align=left src='../images/save.gif' id='btnsave' onmouseout='handlesaveout()' onmouseover='handlesaveover()' width='80' height='24' border='0' onclick='SaveData()'/>" +
           //     "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<img src='../images/cancel.gif' id='btncancel'  onmouseout='handlecancelout()' onmouseover='handlecancelover()' width='80' height='24' border='0' onclick='window.close()'/></td>"
            + String.Format("<div style='cursor:pointer;width:80px;float:left' id='btnsave' onclick='SaveData()' onmouseout='changeoutimg(this)' onmouseover='changeonimg(this)'>{0}</div>", HtmlCondition.GetControl_Button("保存"))
             + String.Format("<div style='cursor:pointer;width:80px;float:right' id='btncancel' onclick='window.close()' onmouseout='changeoutimg(this)' onmouseover='changeonimg(this)'>{0}</div>", HtmlCondition.GetControl_Button("取消"))
           
           );
           
            sb.Append("</tr>");
            sb.Append("</table></div>");
            sb.Append("<input name='CurrentId' id='HiddenField1' value='" + ComponentName + "' type='hidden' />");
            output.Write(sb.ToString());
        }
    }
}