using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
using AppCode;
using UserRightObj;
using UTDtBaseSvr;
using System.Collections;

public partial class UI_WebAddItemControl : System.Web.UI.UserControl
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
        string SPERATOR = ReadConfig.TheReadConfig["SPERATOR"];
        if (Request["type"] != "")
        {
            ComponentName = Request["type"];
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
            
            Table tab = new Table();
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            
            

            tab.Style.Add("width", "100%");
            tab.Style.Add("height", "100%");
            tab.Style.Add("overflow", "hidden");
            tab.BorderWidth = 0;
            tab.CellPadding = 0;
            tab.CellSpacing = 0;
            tab.CssClass = "TextBorder01";
            tab.BackColor = System.Drawing.Color.White;
            sb.Append("<div><table style='width: 100%;height:100%;overflow:hidden' border='0' cellpadding='0' cellspacing='0' bgcolor='#FFFFFF'  class='TextBorder01'>");
            foreach (TableInfo row in TbInfoList)  //根据tableinfo业务字段生成输入表格
            {
                if (ReadConfig.TheReadConfig["URAppName"] == ReadConfig.TheReadConfig["appname"])
                {
                    row.appname = ReadConfig.TheReadConfig["URAppName"];
                }
                string paramstr = businessname + ";" + componentname + ";" + row.ColumnName+";"+row.appname;
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

                tr = new TableRow();
                tr.VerticalAlign = VerticalAlign.Middle;
                tr.Height = 100 / (TbInfoList.Count + 1);
                tr.ID = "tr" + columnName;
                string temp = Convert.ToBoolean(row.ColumnEnableEmpty.ToString() == "True" ? "true" : "false") ? "" : "(*)";
                if (row.ColumnIsHidden != true)
                {        
                    sb.Append(string.Format("<tr valign='middle' height='{0}%' id='tr{1}' >", 100 / (TbInfoList.Count + 1), columnName));
                }
                else
                {
                    tr.Style.Add("display","none");
                    sb.Append(string.Format("<tr valign='middle' height='{0}%' id='tr{1}' style='display:none;'>", 100 / (TbInfoList.Count + 1), columnName));
                }
                tc.Width = 300;
                tc.CssClass = "tdtitle5";
                sb.Append(string.Format("<span id='span{0}'><td align=right width='30%' class='tdtitle5'>", columnName));
                if (row.ColumnDes != "")
                {
                    tc.Text = Convert.ToString(row.ColumnDes).Replace("[", "").Replace("]", "") + temp + ":&nbsp; ";
                    sb.Append(Convert.ToString(row.ColumnDes).Replace("[", "").Replace("]", "") + temp + ":&nbsp; ");
                }
                sb.Append("</td>");
                tr.Cells.Add(tc);
                tc = new TableCell();
                tc.VerticalAlign = VerticalAlign.Middle;
                tc.CssClass = "tdtitle5";
                tc.ID = "td" + columnName;
                
                sb.Append(string.Format("<td align=left valign='middle'  class='tdtitle5' id='td{0}' >", columnName));
                string st = "";
                bool EnableEmpty = row.ColumnEnableEmpty.ToString() == "True" ? true : false;
                USWebService us = new USWebService();
                string ExentdName = "";
                switch (Convert.ToString(row.ColumnType))
                {
                    case "double":
                    case "int":
                        st = HtmlControl.GetControl_Int(columnName, EnableEmpty, columnDes, "", false);
                        sb.Append(st);
                        tc.Text = st;
                        break;
                    case "multiselect":
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
                            columnName ="extendpro" + columnName;
                        }
                        
                            if (row.datasource != null && row.datasource != "" && !IScontract && !HasSrc)
                            {
                                st = HtmlControl.GetControl_DW(columnName, paramstr);
                            }
                            else if (row.datasource != null && row.datasource != "" && IScontract && HasSrc)
                            {
                                string destcolumnname = "";
                                string destdatasource = "";
                                string selected = "";
                                string srcparam = "";
                                string destparam = "";
                                string srccolumnname = "";
                                foreach (TableinfoEvent te in TbInfoEvList)
                                {
                                    if (te.dest == row.ColumnName)
                                    {
                                        foreach (TableInfo ti in TbInfoList)
                                        {
                                            if (ti.ColumnName == te.src)
                                            {
                                                srccolumnname = ti.ColumnName;
                                                paramstr = businessname + ";" + componentname + ";" + ti.ColumnName + ";" + row.appname;
                                                selected = us.GetFirstItem(ti.ColumnName, paramstr);
                                                selected = Regex.Split(selected, SPERATOR, RegexOptions.IgnoreCase)[0];
                                                paramstr = businessname + ";" + componentname + ";" + row.ColumnName + ";" + row.appname;
                                                srcparam = ti.ColumnName;
                                                break;
                                            }
                                        }
                                    }
                                    else if (te.src == row.ColumnName)
                                    {
                                        foreach (TableInfo ti in TbInfoList)
                                        {
                                            if (ti.ColumnName == te.dest)
                                            {
                                                destparam = businessname + ";" + componentname + ";" + ti.ColumnName + ";" + row.appname;
                                                destcolumnname = ti.ColumnName;
                                                destdatasource = ti.datasource;
                                                break;
                                            }
                                        }
                                    }
                                    if (destcolumnname != "" && destdatasource != "" && selected != "")
                                    {
                                        if (te.src.ToLower().Equals("appname"))
                                        {
                                            st = HtmlControl.GetControl_WebServicesSelect(columnName, row.datasource, row.sourcemethod, ComponentName + "." + srcparam + "=" + selected, destdatasource, destcolumnname, paramstr, destparam, columnName + ";" + srccolumnname);

                                        }
                                        else
                                        {
                                            st = HtmlControl.GetControl_WebServicesSelect(columnName, row.datasource, row.sourcemethod, srcparam + "=" + selected, destdatasource, destcolumnname, paramstr,destparam,columnName+";"+srccolumnname);

                                        }
                                        break;
                                    }
                                }
                            }
                            else if (row.datasource != null && row.datasource != "" && HasSrc && !IScontract)
                            {
                                foreach (TableinfoEvent te in TbInfoEvList)
                                {
                                    if (te.dest == row.ColumnName)
                                    {
                                        foreach (TableInfo ti in TbInfoList)
                                        {
                                            if (ti.ColumnName == te.src)
                                            {
                                                string selected = "";
                                                bool SrcSrc = false;
                                                string srccolumnname = "";
                                                foreach (TableinfoEvent tie in TbInfoEvList)
                                                {
                                                    if (tie.dest == ti.ColumnName)
                                                    {
                                                        SrcSrc = true;
                                                        srccolumnname = tie.src;
                                                    }
                                                }
                                                if (SrcSrc == false)
                                                {
                                                    paramstr = businessname + ";" + componentname + ";" + ti.ColumnName + ";" + row.appname;
                                                    selected = us.GetFirstItem(ti.ColumnName, paramstr);
                                                    selected = Regex.Split(selected, SPERATOR, RegexOptions.IgnoreCase)[0];
                                                    paramstr = businessname + ";" + componentname + ";" + row.ColumnName + ";" + row.appname;
                                                }
                                                else
                                                {
                                                    paramstr = businessname + ";" + componentname + ";" + srccolumnname + ";" + row.appname;
                                                    selected = us.GetFirstItem(srccolumnname, paramstr);
                                                    paramstr = businessname + ";" + componentname + ";" + ti.ColumnName + ";" + row.appname + ";" + srccolumnname + "=" + selected;
                                                    string parentparam = srccolumnname + "=" + selected;
                                                    selected = us.GetFirstItemWithSrc(ti.ColumnName, paramstr);
                                                    paramstr = businessname + ";" + componentname + ";" + row.ColumnName + ";" + row.appname + ";" + parentparam;
                                                }
                                                if (SrcSrc)
                                                {
                                                    if (te.src.ToLower().Equals("appname"))
                                                    {
                                                        st = HtmlControl.GetControl_WebServicesSelectSrc(columnName, row.datasource, row.sourcemethod, ComponentName + "." + te.src + "=" + selected, paramstr);
                                                    }
                                                    else
                                                    {
                                                        st = HtmlControl.GetControl_WebServicesSelectSrc(columnName, row.datasource, row.sourcemethod, te.src + "=" + selected, paramstr);
                                                    }
                                                }
                                                else
                                                {
                                                    if (te.src.ToLower().Equals("appname"))
                                                    {
                                                        st = HtmlControl.GetControl_WebServicesSelect(columnName, row.datasource, row.sourcemethod, ComponentName + "." + te.src + "=" + selected, paramstr);
                                                    }
                                                    else
                                                    {
                                                        st = HtmlControl.GetControl_WebServicesSelect(columnName, row.datasource, row.sourcemethod, te.src + "=" + selected, paramstr);
                                                    }
                                                }
                                                
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            else if (row.datasource != null && row.datasource != "" && IScontract && !HasSrc)
                            {
                                int srccount = 0;
                                List<string> al = new List<string>();
                                foreach (TableinfoEvent te in TbInfoEvList)
                                {
                                    if (te.src == row.ColumnName)
                                    {
                                        srccount++;
                                        al.Add(te.dest);
                                    }
                                }
                                if (srccount == 1)
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
                                                            st = HtmlControl.GetControl_DWBindEvent(columnName, row.datasource, destcolumnName, paramstr);
                                                            break;
                                                        }
                                                    }

                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (TableinfoEvent te in TbInfoEvList)
                                    {
                                        if (te.src == row.ColumnName)
                                        {
                                            st = HtmlControl.GetControl_DWBindMutipleEvent(columnName, row.datasource, paramstr,al);
                                            break;

                                        }
                                    }
                                }
                            }
                            else
                            {
                                st = HtmlControl.GetControl_Varchar(columnName, EnableEmpty, columnDes, "", false);
                            }
                        if(st.IndexOf("<select")>=0)
                            st = st.Insert(st.IndexOf("<select") + 7, " class='easyui-combobox' data-options='multiple:true' editable='false' ");
                        st = st.Replace("width:50%", "width:270px");
                        sb.Append(st);tc.Text = st;
                        break;
                    case "datetime":
                        st = HtmlControl.GetControl_DateTime(columnName, EnableEmpty, columnDes, "", false);
                        sb.Append(st);tc.Text = st;
                        break;
                    case "time":
                        st = HtmlControl.GetControl_Time(columnName, EnableEmpty, columnDes, "00:00:00", false);
                        sb.Append(st);tc.Text = st;
                        break;
                    case "varchar":
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
                            columnName ="extendpro" + columnName;
                        }
                        
                            if (row.datasource != null && row.datasource != "" && !IScontract && !HasSrc)
                            {
                                st = HtmlControl.GetControl_DW(columnName, paramstr);
                            }
                            else if (row.datasource != null && row.datasource != "" && IScontract && HasSrc)
                            {
                                string destcolumnname = "";
                                string destdatasource = "";
                                string selected = "";
                                string srcparam = "";
                                string destparam = "";
                                string srccolumnname = "";
                                foreach (TableinfoEvent te in TbInfoEvList)
                                {
                                    if (te.dest == row.ColumnName)
                                    {
                                        foreach (TableInfo ti in TbInfoList)
                                        {
                                            if (ti.ColumnName == te.src)
                                            {
                                                srccolumnname = ti.ColumnName;
                                                paramstr = businessname + ";" + componentname + ";" + ti.ColumnName + ";" + row.appname;
                                                selected = us.GetFirstItem(ti.ColumnName, paramstr);
                                                selected = Regex.Split(selected, SPERATOR, RegexOptions.IgnoreCase)[0];
                                                paramstr = businessname + ";" + componentname + ";" + row.ColumnName + ";" + row.appname;
                                                srcparam = ti.ColumnName;
                                                break;
                                            }
                                        }
                                    }
                                    else if (te.src == row.ColumnName)
                                    {
                                        foreach (TableInfo ti in TbInfoList)
                                        {
                                            if (ti.ColumnName == te.dest)
                                            {
                                                destparam = businessname + ";" + componentname + ";" + ti.ColumnName + ";" + row.appname;
                                                destcolumnname = ti.ColumnName;
                                                destdatasource = ti.datasource;
                                                break;
                                            }
                                        }
                                    }
                                    if (destcolumnname != "" && destdatasource != "" && selected != "")
                                    {
                                        if (te.src.ToLower().Equals("appname"))
                                        {
                                            st = HtmlControl.GetControl_WebServicesSelect(columnName, row.datasource, row.sourcemethod, ComponentName + "." + srcparam + "=" + selected, destdatasource, destcolumnname, paramstr, destparam, columnName + ";" + srccolumnname);

                                        }
                                        else
                                        {
                                            st = HtmlControl.GetControl_WebServicesSelect(columnName, row.datasource, row.sourcemethod, srcparam + "=" + selected, destdatasource, destcolumnname, paramstr,destparam,columnName+";"+srccolumnname);

                                        }
                                        break;
                                    }
                                }
                            }
                            else if (row.datasource != null && row.datasource != "" && HasSrc && !IScontract)
                            {
                                foreach (TableinfoEvent te in TbInfoEvList)
                                {
                                    if (te.dest == row.ColumnName)
                                    {
                                        foreach (TableInfo ti in TbInfoList)
                                        {
                                            if (ti.ColumnName == te.src)
                                            {
                                                string selected = "";
                                                bool SrcSrc = false;
                                                string srccolumnname = "";
                                                foreach (TableinfoEvent tie in TbInfoEvList)
                                                {
                                                    if (tie.dest == ti.ColumnName)
                                                    {
                                                        SrcSrc = true;
                                                        srccolumnname = tie.src;
                                                    }
                                                }
                                                if (SrcSrc == false)
                                                {
                                                    paramstr = businessname + ";" + componentname + ";" + ti.ColumnName + ";" + row.appname;
                                                    selected = us.GetFirstItem(ti.ColumnName, paramstr);
                                                    selected = Regex.Split(selected, SPERATOR, RegexOptions.IgnoreCase)[0];
                                                    paramstr = businessname + ";" + componentname + ";" + row.ColumnName + ";" + row.appname;
                                                }
                                                else
                                                {
                                                    paramstr = businessname + ";" + componentname + ";" + srccolumnname + ";" + row.appname;
                                                    selected = us.GetFirstItem(srccolumnname, paramstr);
                                                    paramstr = businessname + ";" + componentname + ";" + ti.ColumnName + ";" + row.appname + ";" + srccolumnname + "=" + selected;
                                                    string parentparam = srccolumnname + "=" + selected;
                                                    selected = us.GetFirstItemWithSrc(ti.ColumnName, paramstr);
                                                    paramstr = businessname + ";" + componentname + ";" + row.ColumnName + ";" + row.appname + ";" + parentparam;
                                                }
                                                if (SrcSrc)
                                                {
                                                    if (te.src.ToLower().Equals("appname"))
                                                    {
                                                        st = HtmlControl.GetControl_WebServicesSelectSrc(columnName, row.datasource, row.sourcemethod, ComponentName + "." + te.src + "=" + selected, paramstr);
                                                    }
                                                    else
                                                    {
                                                        st = HtmlControl.GetControl_WebServicesSelectSrc(columnName, row.datasource, row.sourcemethod, te.src + "=" + selected, paramstr);
                                                    }
                                                }
                                                else
                                                {
                                                    if (te.src.ToLower().Equals("appname"))
                                                    {
                                                        st = HtmlControl.GetControl_WebServicesSelect(columnName, row.datasource, row.sourcemethod, ComponentName + "." + te.src + "=" + selected, paramstr);
                                                    }
                                                    else
                                                    {
                                                        st = HtmlControl.GetControl_WebServicesSelect(columnName, row.datasource, row.sourcemethod, te.src + "=" + selected, paramstr);
                                                    }
                                                }
                                                
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            else if (row.datasource != null && row.datasource != "" && IScontract && !HasSrc)
                            {
                                int srccount = 0;
                                List<string> al = new List<string>();
                                foreach (TableinfoEvent te in TbInfoEvList)
                                {
                                    if (te.src == row.ColumnName)
                                    {
                                        srccount++;
                                        al.Add(te.dest);
                                    }
                                }
                                if (srccount == 1)
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
                                                            st = HtmlControl.GetControl_DWBindEvent(columnName, row.datasource, destcolumnName, paramstr);
                                                            break;
                                                        }
                                                    }

                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (TableinfoEvent te in TbInfoEvList)
                                    {
                                        if (te.src == row.ColumnName)
                                        {
                                            st = HtmlControl.GetControl_DWBindMutipleEvent(columnName, row.datasource, paramstr,al);
                                            break;

                                        }
                                    }
                                }
                            }
                            else
                            {
                                st = HtmlControl.GetControl_Varchar(columnName, EnableEmpty, columnDes, "", false);
                            }
                        sb.Append(st);tc.Text = st;
                        break;
                    case "treeview":
                        st = "<div runat=\"server\" style=\"background-color:red;width:170px;height:150px;overflow:auto\" id=\"treeview\"><input name='TreeNodeText' id='TreeNodeText' value='0' type='hidden' runat='server'/><ul id='ul_64' ></ul></div>";
                        sb.Append(st);tc.Text = st;
                        break;
                    case "checktable":
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
                            st = HtmlControl.GetDataGridUI(columnName, paramstr);
                        }
                        sb.Append(st);tc.Text = st;
                        break;
                    case "float":
                    case "numeric":
                        st = HtmlControl.GetControl_Numeric(columnName, EnableEmpty, columnDes, "", false);
                        sb.Append(st);tc.Text = st;
                        break;
                    case "img":
                        st = HtmlControl.GetControl_UploadFile();
                        sb.Append(st);
                        break;
                    case "password":
                        st = HtmlControl.GetControl_NewPwd(columnName, EnableEmpty, columnDes, 50 / (TbInfoList.Count + 1));
                        sb.Append(st);tc.Text = st;
                        break;
                    case "checkbox":
                        st = HtmlControl.GetControl_CheckBox(columnName);
                        sb.Append(st); tc.Text = st;
                        break;
                    default:
                        break;
                }
                sb.Append("</td></span>");
                sb.Append("</tr>");
                tr.Cells.Add(tc);
                tab.Rows.Add(tr);
            }
            tr = new TableRow();

            tr.VerticalAlign = VerticalAlign.Middle;
            tr.Cells.Add(new TableCell());
            sb.Append("<tr valign='middle'>");
         //   sb.Append("<td align=right height='35px'></td>");
            tc = new TableCell();
            tc.Text = "<img align=left src='../images/save.gif' id='btnsave' onmouseout='handlesaveout()' onmouseover='handlesaveover()' width='80' height='24' border='0' onclick='AddData()'/>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<img src='../images/cancel.gif' id='btncancel'  onmouseout='handlecancelout()' onmouseover='handlecancelover()' width='80' height='24' border='0' onclick='window.close()'/>";
            sb.Append("<td colspan=2><div style='margin:0 auto;width:200px'>" 
             //   +"<img align=left src='../images/save.gif' id='btnsave' onmouseout='handlesaveout()' onmouseover='handlesaveover()' width='80' height='24' border='0' onclick='AddData()'/>" +
             //   "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<img src='../images/cancel.gif' id='btncancel'  onmouseout='handlecancelout()' onmouseover='handlecancelover()' width='80' height='24' border='0' onclick='window.close()'/></td>"

             + String.Format("<div style='cursor:pointer;width:80px;float:left' id='btnsave' onclick='AddData()' onmouseout='changeoutimg(this)' onmouseover='changeonimg(this)'>{0}</div>",HtmlCondition.GetControl_Button("保存"))
             + String.Format("<div style='cursor:pointer;width:80px;float:right' id='btncancel' onclick='window.close()' onmouseout='changeoutimg(this)' onmouseover='changeonimg(this)'>{0}</div>", HtmlCondition.GetControl_Button("取消"))
             );
            sb.Append("</div></td></tr>");
            tr.Cells.Add(tc);
            tab.Rows.Add(tr);
            sb.Append("</table></div>");
            sb.Append("<input name='CurrentId' id='HiddenField1' value='" + ComponentName + "' type='hidden' />");
            main.InnerHtml = sb.ToString();

        }
        

    }
    public void setValue(Control uc, string var, object vals) 
    {

        PropertyInfo pi = uc.GetType().GetProperty(var, BindingFlags.Public | BindingFlags.Instance);
        pi.SetValue(uc, vals, null);

    }
}