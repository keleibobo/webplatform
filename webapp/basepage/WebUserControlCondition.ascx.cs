using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using UTDtBaseSvr;
using System.Data;
using AppCode;

public partial class UI_WebUserControlCondition : System.Web.UI.UserControl
{
    public string appname = "";
    public string rolename ;
    public string style = ReadConfig.TheReadConfig["style"];
   

    public string username ;

    public bool isDataType = false;
    private ConditionControlDefine _conditionDefine;

    public string Id
    {
        set;
        get;
    }

    public ConditionControlDefine ConditionDefine
    {
        set
        {
            _conditionDefine = value;
        }
        get
        {
            if (_conditionDefine == null)
                _conditionDefine = new ConditionControlDefine();
            return _conditionDefine;
        }
    }

    public BusinessCall bscall = null;

    public bool relation = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        appname = HttpContext.Current.Session["appname"].ToString();
        rolename = HttpContext.Current.Session["RoleType"].ToString();
        username = HttpContext.Current.Session["username"].ToString();

        string ptype = Request["type"];
      //  ptype = "Main"; //test HistoryTaskLogItem
        ConditionDefine.stable = ptype;
        BusinessCall bccCall = InitModel.GetBusinessCall(appname, ptype);
        bscall = bccCall;
        if (bccCall != null)
        {
            ConditionDefine.bccList = bccCall.bccList;
            ConditionDefine.menuid = bccCall.BusinessID;
        }
        foreach (BusinessComponentEventCall bce in bccCall.bcEventList)
        {
            foreach (BusinessComponentCall bcc in bccCall.bComponentList)
            {
                if (bce.dest_id == bcc.id && bcc.type == "business_condition")
                {
                    relation = true;
                }
            }
        }
       

    }


  

    protected string getUserFuns()
    {
        string rt = "";
        String menuid = Request["id"];
        if (menuid == null || menuid.Equals(""))
        {
            menuid = ConditionDefine.menuid;
        }

        UserMenuInfo mui = InitModel.GetUserMenuInfoByMenuID(appname, rolename, menuid);
        if (mui == null)
            return rt;
        bool bCustomFun = mui.customuserfunction.Equals("1");
        string rolefunc = mui.rolecanusefunction;
        if (!bCustomFun)
        {
            rt = rolefunc;
        }
        else
        {
            string alluserfunc = mui.usercannotusefunction;
            string sfunc = mui.usercannotusefunction;// dbUM.GetUserFunctionsByMenuID(alluserfunc, AppCode.LoginUserModel.UserName);
            string[] atemp = sfunc.Split('|');
            bool userin = true;
            foreach (string uc in atemp)
            {
                string[] temp = uc.Split(',');
                if (username == temp[0])
                {
                    userin = false;
                    if (temp.Length > 1)
                    {
                        string[] afuns = rolefunc.Split(',');
                        for (int i = 0; i < afuns.Length; i++)
                        {
                            bool mark = true;
                            for (int j = 1; j < temp.Length; j++)
                            {
                                if (afuns[i] == temp[j])
                                {
                                    mark = false;
                                }
                            }
                            if (mark)
                            {
                                rt += afuns[i] + ",";
                            }
                        }
                    }
                    else
                    {
                        rt = mui.rolecanusefunction;
                    }
                    break;
                }
            }
            if (userin)
            {
                rt = rolefunc;
            }
        }
        return rt;
    }

    protected string addButton(string[] aFun)
    {
        string sbutton = "";
        foreach (string s in aFun)
        {
            ButtonInfo bi = InitModel.GetButtonInfoByID(s);
           // if (s.Equals("btnquery")) continue; //查询按钮特殊处理
            if (bi != null)
            {
                if (sbutton.Length == 0)
                {
                    sbutton = bi.actionscript;
                }
                else
                {
                    sbutton += bi.actionscript;
                }
            }
        }

        sbutton = sbutton.Replace("../images","../images/"+style);
        return sbutton;
    }

    /// <summary>
    /// 将此控件呈现给指定的输出参数
    /// </summary>
    /// <param name="output"></param>
    protected override void Render(HtmlTextWriter output)
    {
        if (Id == null) Id = "0";
        output.Write(String.Format("<div id='component_{0}_condition' ><table width='100%' border='0' cellpadding='0' cellspacing='0'>",Id));//<table width='100%' border='0' cellpadding='0' cellspacing='0' class='TextBorder005' id='listable'>
      //  output.Write(trAddress);
        Table table = new Table();
        table.Attributes.Add("height", "36px");

        TableRow tableRow = new TableRow();
        tableRow.ID = "tbCondition";
        string sUserfuns = getUserFuns();
        string[] aFun = sUserfuns.Split(',');
        string sbutton = addButton(aFun);
        if (isDataType)
        {
            output.Write(string.Format("<input name='dtStart' id='dtStart' type='hidden' value='{0}-01' />", HttpContext.Current.Request["YearMonth"]));
        }
        else if (ConditionDefine == null)//bc  改动
        {
            TableCell tableCell = new TableCell();
            if (ConditionDefine.stable.ToLower() == "t_systeminfo")
            {
                jsHiddenAdd();
                return;
            }
            string sQuery = IsQuery();
            tableCell.Text = sbutton + sQuery;
            tableRow.Cells.Add(tableCell);
            if (sQuery != "")
            {
                tableRow.Cells.Add(QueryButton());
            }
        }
        else
        {

            TableCell tableCell = null;
            foreach (BusinessConditionCall cd in ConditionDefine.bccList)
            {
                tableCell = new TableCell();

                Dictionary<string, Object> extendparam = LayoutUI.getParam(cd.para, ';', '=');

                if (extendparam.ContainsKey("show") && extendparam["show"].Equals("layout")) //扩展，默认由条件组件管理，增加对单个条件的布局管理（不出现在组件管理而是布局管理）
                {
                    continue;
                }

                if (cd.ComponetType.ToLower() == "combox" && cd.PresentType == "presenttypecombox")
                {
                    string text = AppCode.HtmlCondition.GetControl_Combox(cd);;

                    Dictionary<string, object> extendMap = LayoutUI.getParam(cd.para, ';', '=');
                    string func = "onchange";
                    if (extendMap.ContainsKey(func) && extendMap.ContainsKey("param") && ConditionLayout.getTargetId(bscall, Id)=="")
                    {
                        text += string.Format("<script type='text/javascript' >{1}('{0}','{2}')</script>", cd.name, extendMap[func], extendMap["param"]);
                    }
                    else if (extendMap.ContainsKey(func))
                    {
                        text += string.Format("<script type='text/javascript' >{1}('{0}','{2}')</script>", cd.name, extendMap[func], ConditionLayout.getTargetId(bscall, Id));

                    }

                  //  if ("".Equals(dest_id)) //自动加载
                    if(!relation)
                    {

                        text += string.Format("<script type='text/javascript' >f_loadcombobox('{0}');</script>", cd.name);
                 
                    }
                    else
                    {
                        text += string.Format("<script type='text/javascript' >comboboxInit('{0}');</script>", cd.name);
                    }
                    
                    tableCell.Text = text;
                    
                }
                else if (cd.ComponetType.ToLower() == "combox" && cd.PresentType == "presenttyperadio")
                {
                    tableCell.Text = AppCode.HtmlCondition.GetControl_Radio(cd);
                    
                }
                else if (cd.ComponetType.ToLower() == "combox" && cd.PresentType == "presenttypecheckbox")
                {
                    tableCell.Text = AppCode.HtmlCondition.GetControl_Checkbox(cd);
                }
                else if (cd.ComponetType.ToLower() == "int")
                {
                    tableCell.Text = AppCode.HtmlCondition.GetControl_Int(cd);
                }
                else if (cd.ComponetType.ToLower() == "varchar")
                {
                    tableCell.Text = AppCode.HtmlCondition.GetControl_VarChar(cd);
                }
                else if (cd.ComponetType.ToLower() == "number")
                {
                    tableCell.Text = AppCode.HtmlCondition.GetControl_Number(cd);
                }
                else if (cd.ComponetType.ToLower() == "datetime")//
                {
                    tableCell.Text = AppCode.HtmlCondition.GetControl_UIDatetime(cd);
                }
                else if (cd.ComponetType.ToLower() == "date")
                {
                    tableCell.Text = AppCode.HtmlCondition.GetControl_UIDate(cd);
                }
                else if (cd.ComponetType.ToLower() == "year")
                {
                    tableCell.Text = AppCode.HtmlCondition.GetControl_Year(cd);
                }
                else if (cd.ComponetType.ToLower() == "month")
                {
                    tableCell.Text = AppCode.HtmlCondition.GetControl_Month(cd);
                }
                else if (cd.ComponetType.ToLower() == "time")
                {
                    tableCell.Text = AppCode.HtmlCondition.GetControl_Time(cd);
                }
                else
                {
                    tableCell.Text = AppCode.HtmlCondition.GetControl_Base(cd);
                }
                tableRow.Cells.Add(tableCell);
            }

            if (ConditionDefine.bccList.Count == 0)
            {
                table.Attributes.Add("width", "100%");
                foreach (String btn in aFun)
                {
                    if (btn.Equals("btnquery"))
                    {
                        tableRow.Cells.Add(addButton(InitModel.GetButtonInfoByID("btnquery").actionscript));
                        break;
                    }
                }
            }
            else
            {
                foreach (String btn in aFun)
                {
                    if (btn != "")
                    {
                        tableRow.Cells.Add(addButton(InitModel.GetButtonInfoByID(btn).actionscript));
                    }
                }
            }
        }

        output.Write("<tr   id='filtertr' runat='server'><td height='38' valign='top'>");

        if (tableRow.Cells.Count > 0)
        {
            table.Rows.Add(tableRow);
            output.Write(@" <table width='100%' height='38' border='0' cellpadding='0' cellspacing='0' class='TextBorder004'>
                        <tr>
                            <td width='95%' align='left' valign='middle'>");
            table.RenderControl(output);
            output.Write(@" 
                            </td>
                        </tr>
                    </table>");
        }
        else
        {
            if (!sbutton.Equals(""))
            {
                output.Write("<table width='100%' height='38' border='0' cellpadding='0' cellspacing='0' class='TextBorder004'><tr><td width='100%' align='right'>"+sbutton+"</td></tr></table>");
            }else
          
            jsHiddenAdd();
        }
        output.Write("</td></tr></table></div>");
        output.Write("<script type='text/javascript' >setLayout('component_"+Id+"_condition','{\"layout\":\"center\"}');</script>");//$(document.getElementById('SiteAddress')).html(document.getElementById('menutext').value)
    }

    /// <summary>
    /// 增加界面显示查询条件
    /// </summary>
    public string IsQuery()
    {
        StringBuilder swsb = new StringBuilder();
        string sqlQuery = string.Format(@"SELECT Name,ColumnName,ColumnDes,ColumnType,ColumnLenght,ColumnSql,ColumnEnableEmpty,isQueryFilter as isQuery 
             FROM T_TableInfo WHERE Name='{0}' AND isQueryFilter=1 ", ConditionDefine.stable);
        DataTable dtQuery = new DataTable();
        ///   WebDatabase dbMembership = new WebDatabase(enumDatabaseBussiness.MEMBERSHIP);
        ///   dbMembership.ExecSqlQuery(sqlQuery, out dtQuery);
        //HttpContext.Current.Response.Write(sqlQuery + "|" + dtQuery.Rows.Count.ToString());
        string sFieldChName = "";
        string sFieldName = "";
        string FpsQueryValue = "";
        string ColumnType = "";
        foreach (DataRow dr in dtQuery.Rows)
        {
            sFieldChName = dr["ColumnDes"].ToString().ToString();
            sFieldName = dr["ColumnName"].ToString().ToString();
            ColumnType = dr["ColumnType"].ToString().ToString().ToLower();
            if (ColumnType == "datetime")
            {
                swsb.Append(string.Format(@"<td >{0}从<input size='9' where='>=' dataname='时间从'
                 datatype='date' name='{1}' value='{2}' class='TextBorder02'>
                 <img  onmouseout=""this.src='../images/日历.gif'""  onclick=""utf_calendar('{1}',1)"" 
                 style='cursor:hand' onmouseover=""this.src='../images/日历_on.gif'"" 
                src='../images/日历.gif' align='center' valign='middle' columntype='{3}' />
             </td>", sFieldChName, sFieldName, FpsQueryValue, ColumnType));
                swsb.Append(string.Format(@"<td >到<input   size='9'  where='<='  dataname='时间从'
                 datatype='date' name='{0}9' value='' class='TextBorder02' >
                 <img  onmouseout=""this.src='../images/日历.gif'""  onclick=""utf_calendar('{0}9',1)"" 
                 style='cursor:hand' onmouseover=""this.src='../images/日历_on.gif'"" 
                src='../images/日历.gif' align='center' valign='middle' />
             </td>", sFieldName));
                //   swsb.Append(" <td>到<input size='9'   where='<=' name='" + sFieldName + "9' dataname='时间到' datatype='date'   class=wxinput><img class='wxinput' align='absMiddle' border='0' onclick=\"utf_calendar('" + sFieldName + "9')\"  height='18' src='../images/calendar.bmp' title='选择日期' width='18' style='CURSOR:hand'></td>");

            }
            if (ColumnType == "strdatetime")
            {
                swsb.Append(string.Format(@"<td >{0}从<input size='9' where='>=' dataname='时间从'
                 datatype='date' name='{1}' value='{2}' class='TextBorder02'>
                 <img  onmouseout=""this.src='../images/日历.gif'""  onclick=""utf_calendar('{1}',1)"" 
                 style='cursor:hand' onmouseover=""this.src='../images/日历_on.gif'"" 
                src='../images/日历.gif' align='center' valign='middle' columntype='{3}' />
             </td>", sFieldChName, sFieldName, FpsQueryValue, ColumnType));
                swsb.Append(string.Format(@"<td >到<input   size='9'  where='<='  dataname='时间从'
                 datatype='date' name='{0}9' value='' class='TextBorder02' >
                 <img  onmouseout=""this.src='../images/日历.gif'""  onclick=""utf_calendar('{0}9',1)"" 
                 style='cursor:hand' onmouseover=""this.src='../images/日历_on.gif'"" 
                src='../images/日历.gif' align='center' valign='middle'  columntype='{1}' />
             </td>", sFieldName, ColumnType));
                //   swsb.Append(" <td>到<input size='9'   where='<=' name='" + sFieldName + "9' dataname='时间到' datatype='date'   class=wxinput><img class='wxinput' align='absMiddle' border='0' onclick=\"utf_calendar('" + sFieldName + "9')\"  height='18' src='../images/calendar.bmp' title='选择日期' width='18' style='CURSOR:hand'></td>");

            }
            if (ColumnType == "varchar")
            {
                ///      WebDatabase dbtemp = new WebDatabase(enumDatabaseBussiness.PRODUCT);
                ///       if (dbtemp.getDatabaseType() == enumDatabaseType.SQLSERVER)
                {
                    swsb.Append(string.Format(@"<td >{0}<input style=' overflow:visible; height:20;width:70' where='charindex' value=''   name='{1}' value=""{2}""  class='TextBorder02' columntype='{3}' ></td>",
                        sFieldChName, sFieldName, FpsQueryValue, ColumnType));
                }
                ///       else if (dbtemp.getDatabaseType() == enumDatabaseType.MYSQL)
                {
                    ///           swsb.Append(string.Format(@"<td >{0}<input style=' overflow:visible; height:20;width:70' where='position' value=''   name='{1}' value=""{2}""  class='TextBorder02'  columntype='{3}' ></td>",
                    ///               sFieldChName, sFieldName, FpsQueryValue, ColumnType));
                }

            }
            if (ColumnType == "int" || ColumnType == "numeric")
            {
                swsb.Append(string.Format(@"<td >{0}<input  style=' overflow:visible; height:20;width:70' where='>=' value=''   name='{1}' value=""{2}""  class='TextBorder02'  columntype='{3}' ></td>",
                    sFieldChName, sFieldName, FpsQueryValue, ColumnType));
            }
        }
        return swsb.ToString();
    }

    private void jsHiddenAdd()
    {
        HttpContext.Current.Response.Write(string.Format(@"<script type=""text/jscript"">document.getElementById('filtertr').style.display='none'</script>"));// 
    }

    /// <summary>
    /// 创建查询按钮
    /// </summary>
    /// <returns></returns>
    protected TableCell QueryButton()
    {
        TableCell tableCell = new TableCell();
        tableCell.Text = string.Format(@"<td name='btnQuery' onclick='f_query()' id='btnQuery' width='63' align='center' valign='middle'><img align='center' valign='middle' onmouseout=""this.src='../images/查询.gif'"" style='cursor:hand' onmouseover=""this.src='../images/查询_on.gif'"" src='../images/查询.gif' /></td>");
        //   tableCell.Attributes.Add("float","right");
        return tableCell;
    }

    public TableCell addButton(string td)
    {
        TableCell tableCell = new TableCell();
        StringBuilder sb = new StringBuilder();
         td =td.Replace("../images", "../images/" + style);
      
        sb.Append("<td><table width='100%' style='border-spacing:0px'><tr style='float:right'>" + td + "</tr></table></td>");
        tableCell.Text = sb.ToString();

        //  tableCell.Text = td;
        return tableCell;
    }

    private List<string> getDisplay()
    {
        List<string> list = new List<String>();

        

        string conditionid = "";
        foreach (BusinessComponentCall bccall in bscall.bComponentList)
        {
            if ("business_condition".Equals(bccall.sourcetype) && bccall.type!=null&&bccall.type.Trim().Equals(""))//
            {
                conditionid = bccall.id;
            }
        }

        if(!conditionid.Equals(""))
        foreach (BusinessComponentEventCall bcec in bscall.bcEventList)
        {
            if (bcec.dest_id.Equals(conditionid))
            {
                foreach(BusinessConditionCall bcondition in bscall.bccList){
                    if (bcondition.ComponetType.Equals("combox"))
                    {
                        list.Add(bcondition.id);
                    }
                }
            }
        }
        return list;
    }

   
}