using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using System.Text;
using AppCode;
using svgsvr;
using UserRightObj;
using System.Text.RegularExpressions;
using UTDtCnvrt;

namespace AppCode
{
    /// <summary>
    /// Summary description for HtmlControl
    /// </summary>
    public static class HtmlControl
    {       
        public static string appname = ((Loginresult)HttpContext.Current.Session["Session"]).AppName;
        public static string URappname = ReadConfig.TheReadConfig["URAppName"];
        public static string SPERATOR = ReadConfig.TheReadConfig["SPERATOR"];
        /// <summary>
        /// 创建日期控件t,输入接日期，字段类型为日期控件
        /// </summary>
        /// <param name="columnName">字段名称</param>
        /// <param name="EnableEmpty">是否为空</param>
        /// <param name="columnDes">字段中文解释</param>
        /// <param name="svalue">字段值</param>
        /// <returns></returns>
        public static string GetControl_DateTime(string columnName, bool EnableEmpty, string columnDes, string svalue, bool bEnableEdit)
        {
            string rt = string.Format(@"<input  type='text' sqlId='{0}' id='{0}' isdate='true'  enableEmpty='{1}' stralert='{2}' value='{3}' defineName='RecordPageType' style='width:50%' class='TextBorder04' {4}>&nbsp;&nbsp;<img  onmouseout=""this.src='../images/日历.gif'"" style='cursor:hand;width:33;height:24' onmouseover=""this.src='../images/日历_on.gif'""  src='../images/日历.gif'  onclick=""utf_calendar('{0}','1')"" />",
                         columnName,
                         EnableEmpty ? "True" : "False",
                         columnDes.Replace("[", "").Replace("]", "") + "不能为空",
                         svalue,
                         bEnableEdit ? "readonly='True'" : "");
            return rt;
        }
        /// <summary>
        /// 创建日期控件t,输入接时间，字段类型为时间控件
        /// </summary>
        /// <param name="columnName">字段名称</param>
        /// <param name="EnableEmpty">是否为空</param>
        /// <param name="columnDes">字段中文解释</param>
        /// <param name="svalue">字段值</param>
        /// <returns></returns>
        public static string GetControl_Time(string columnName, bool EnableEmpty, string columnDes, string svalue, bool bEnableEdit)
        {
            string rt = string.Format(@"<input  type='text' sqlId='{0}' id='{0}' enableEmpty='{1}' onkeyDown='validate(this,2)' stralert='{2}' value='{3}' defineName='RecordPageType' style='width:50%' class='TextBorder04' {4}>",
                        columnName,
                        EnableEmpty ? "True" : "False",
                        columnDes.Replace("[", "").Replace("]", "") + "不能为空",
                        svalue,
                        bEnableEdit ? "readonly='True'" : "");
            return rt;
        }

        /// <summary>
        /// 创建类型为text,输入接受数字，可以带小数点的输入框，字段类型为整数的控件
        /// </summary>
        /// <param name="columnName">字段名称</param>
        /// <param name="EnableEmpty">是否为空</param>
        /// <param name="columnDes">字段中文解释</param>
        /// <param name="svalue">字段值</param>
        /// <returns></returns>
        public static string GetControl_Number(string columnName, bool EnableEmpty, string columnDes, string svalue, bool bEnableEdit)
        {
            string rt = string.Format("<input type='text' sqlId='{0}' id='{0}' enableEmpty='{1}' stralert='{2}' value='{3}' defineName='RecordPageType' style='width:90%' class='TextBorder04' onkeypress='InputNum()' {4}>",
                            columnName,
                            EnableEmpty ? "True" : "False",
                            columnDes.Replace("[", "").Replace("]", "") + "不能为空",
                            svalue,
                            bEnableEdit ? "readonly='True'" : "");
            return rt;
        }

        public static string GetControl_Numeric(string columnName, bool EnableEmpty, string columnDes, string svalue, bool bEnableEdit)
        {
            string rt = string.Format("<input type='text' sqlId='{0}' id='{0}' enableEmpty='{1}' stralert='{2}' value='{3}' defineName='RecordPageType' style='width:50%' class='TextBorder04' onkeypress='Inputfloat()' {4}>",
                            columnName,
                            EnableEmpty ? "True" : "False",
                            columnDes.Replace("[", "").Replace("]", "") + "不能为空",
                            svalue,
                            bEnableEdit ? "readonly='True'" : "");
            return rt;
        }
        /// <summary>
        ///  创建类型为text,输入接受整数的输入框，字段类型为整数的控件
        /// </summary>
        /// <param name="columnName">字段名称</param>
        /// <param name="EnableEmpty">是否为空</param>
        /// <param name="columnDes">字段中文解释</param>
        /// <param name="svalue">字段值</param>
        /// <returns></returns>
        public static string GetControl_Int(string columnName, bool EnableEmpty, string columnDes, string svalue, bool bEnableEdit)
        {
            string rt = string.Format("<input type='text' sqlId='{0}' id='{0}' enableEmpty='{1}' stralert='{2}' value='{3}' defineName='RecordPageType' style='width:90%'  class='TextBorder04' onkeypress='InputNum()' maxlength=12  {4} />",
                            columnName,
                            EnableEmpty ? "True" : "False",
                            columnDes.Replace("[", "").Replace("]", "") + "不能为空",
                            svalue,
                            bEnableEdit ? "readonly='True'" : "");
            return rt;
        }

        /// <summary>
        /// 创建类型为text，输入接受字符串的输入框,字段类型为字符串的控件
        /// </summary>
        /// <param name="columnName">字段名称</param>
        /// <param name="EnableEmpty">是否为空</param>
        /// <param name="columnDes">字段中文解释</param>
        /// <param name="svalue">字段值</param>
        /// <returns></returns>
        public static string GetControl_Varchar(string columnName, bool EnableEmpty, string columnDes, string svalue, bool bEnableEdit)
        {
            string rt = string.Format("<input type='text' sqlId='{0}' maxlength='50' id='{0}' defineName='RecordPageType' enableEmpty='{1}' stralert='{2}' value='{3}' style='width:50%' class='TextBorder04' {4} />",
                             columnName,
                             EnableEmpty ? "True" : "False",
                             columnDes.Replace("[", "").Replace("]", "") + "不能为空",
                             svalue,
                             bEnableEdit ? "disabled='disabled'" : "");
            return rt;
        }

        /// <summary>
        /// 创建类型为text，输入接受字符串的输入框,字段类型为字符串的扩展参数控件
        /// </summary>
        /// <param name="columnName">字段名称</param>
        /// <param name="EnableEmpty">是否为空</param>
        /// <param name="columnDes">字段中文解释</param>
        /// <param name="svalue">字段值</param>
        /// <param name="bEnableEdit">是否可编辑</param>
        /// <returns></returns>
        public static string GetControl_ProVarchar(string columnName, bool EnableEmpty, string columnDes, string svalue, bool bEnableEdit)
        {
            string rt = string.Format("<input type='text' sqlId='{0}' maxlength='50' id='pro{0}' defineName='RecordPageType' enableEmpty='{1}' stralert='{2}' value='{3}' style='width:50%' class='TextBorder04' {4} />",
                             columnName,
                             EnableEmpty ? "True" : "False",
                             columnDes.Replace("[", "").Replace("]", "") + "不能为空",
                             svalue,
                             bEnableEdit ? "readonly='True'" : "");
            return rt;
        }

        public static string GetControl_RadioButton(string columnName, bool EnableEmpty, string columnDes, string url, string method, string paramstr)
        {
            string rt = "";
           // List<String> listrow = WSUtil.GetInputDataFromWS(url, method);
            //foreach (string s in listrow)
            //{
            //    string[] temp = s.Split(',');
            //    rt += string.Format("<input name='{0}'  type='radio' value='{1}' />{2}&nbsp&nbsp",
            //                 columnName,
            //                 s[1],
            //                 s[0]);
            //}
            return rt;
        }

        /// <summary>
        /// 创建旧密码控件
        /// </summary>
        /// <param name="columnName">字段名称</param>
        /// <param name="EnableEmpty">是否为空</param>
        /// <param name="columnDes">字段描述</param>
        /// <returns></returns>
        public static string GetControl_OldPwd(string columnName, bool EnableEmpty, string columnDes)
        {
            string rt = string.Format("<input type='password' sqlId='{0}' id='old{0}'  defineName='RecordPageType' enableEmpty='{1}' stralert='{2}'  style='width:50%' class='TextBorder04' />",
                             columnName,
                             EnableEmpty ? "True" : "False",
                             columnDes.Replace("[", "").Replace("]", "") + "不能为空");
            return rt;
        }

        /// <summary>
        /// 创建新密码控件
        /// </summary>
        /// <param name="columnName">字段名称</param>
        /// <param name="EnableEmpty">是否为空</param>
        /// <param name="columnDes">字段描述</param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static string GetControl_NewPwd(string columnName, bool EnableEmpty, string columnDes,int height)
        {
            string rt = string.Format("<input type='password' sqlId='{0}' id='new{0}'  defineName='RecordPageType' enableEmpty='{1}' stralert='{2}'  style='width:50%' class='TextBorder04' />",
                             columnName,
                             EnableEmpty ? "True" : "False",
                             columnDes.Replace("[", "").Replace("]", "") + "不能为空");
            StringBuilder sb = new StringBuilder();
            sb.Append("</td></span>");
            sb.Append("</tr>");
            sb.Append(string.Format("<tr valign='middle' height='{0}%' id='tr{1}' >", height, columnName));
            sb.Append(string.Format("<span id='span{0}'><td align=right width='30%' class='tdtitle5'>", columnName));
            sb.Append("请再次输入:&nbsp; ");
            sb.Append("</td></span>");
            sb.Append("<td>");
            rt += sb.ToString();
            rt += string.Format("<input type='password' sqlId='{0}' id='repeat{0}'  defineName='RecordPageType' enableEmpty='{1}' stralert='{2}'  style='width:50%' class='TextBorder04' />",
                            columnName,
                            EnableEmpty ? "True" : "False",
                            columnDes.Replace("[", "").Replace("]", "") + "不能为空");
            return rt;
        }

        /// <summary>
        /// 创建新密码控件
        /// </summary>
        /// <param name="columnName">字段名称</param>
        /// <param name="EnableEmpty">是否为空</param>
        /// <param name="columnDes">字段描述</param>
        /// <returns></returns>
        public static string GetControl_NewPwd(string columnName, bool EnableEmpty, string columnDes)
        {
            string rt = string.Format("<input type='password' sqlId='{0}' id='new{0}'  defineName='RecordPageType' enableEmpty='{1}' stralert='{2}'  style='width:50%' class='TextBorder04' />",
                             columnName,
                             EnableEmpty ? "True" : "False",
                             columnDes.Replace("[", "").Replace("]", "") + "不能为空");
            return rt;
        }

        /// <summary>
        /// 创建密码确认控件
        /// </summary>
        /// <param name="columnName">字段名称</param>
        /// <param name="EnableEmpty">是否为空</param>
        /// <param name="columnDes">字段描述</param>
        /// <returns></returns>
        public static string GetControl_RepeatPwd(string columnName, bool EnableEmpty, string columnDes)
        {
            string rt = string.Format("<input type='password' sqlId='{0}' id='repeat{0}'  defineName='RecordPageType' enableEmpty='{1}' stralert='{2}'  style='width:50%' class='TextBorder04' />",
                             columnName,
                             EnableEmpty ? "True" : "False",
                             columnDes.Replace("[", "").Replace("]", "") + "不能为空");
            return rt;
        }

        /// <summary>
        /// 创建上传文件控件
        /// </summary>
        /// <returns></returns>
        public static string GetControl_UploadFile()
        {
            string rt = "<iframe id='frmPic' src='UploadPic.aspx' style=' width:400px; height:50px;' frameborder='0'></iframe>";
            return rt;
        }

        public static string GetControl_CheckBox(string ColumnName)
        {
            string rt = string.Format(@"<input type='checkbox' defineName='RecordPageType' sqlId='{0}' id='ck{0}' />", ColumnName);
            
            return rt;
        }

        /// <summary>
        /// 创建类型为整数的下列列表,值为编号，dtList表的第一个字段为编号，第二个字段为名称
        /// </summary>
        /// <param name="columnName">字段名称</param>
        /// <param name="EnableEmpty">是否为空</param>
        /// <param name="columnDes">字段中文解释</param>
        /// <param name="dtValue">字段值</param>
        /// <returns></returns>
        public static string GetControl_Select(string columnName, bool EnableEmpty, string columnDes, string svalue,
            string stableName, DataTable dtList, bool bEnableEdit,bool bImg)
        {
            StringBuilder sbtemp = new StringBuilder();
            sbtemp.Append(string.Format(@"<select defineName='RecordPageType' sqlId='{0}' style='width:90%' id='select{0}' isimg={2} onchange=""f_changeselect('{0}','{1}',this)"">",
                columnName,
                stableName,
                bImg));

            bool bfound = false;
            string spath = "",sid="",sname="",sfilepath="";
            if (dtList != null && dtList.Rows.Count > 0)
            {
                if (svalue.Length > 0)
                {
                    DataRow[] drs = dtList.Select("id=" + svalue);
                    if (drs.Length == 0)
                    {
                        if (bImg)
                        {
                            sfilepath = Convert.ToString(dtList.Rows[0][2]);
                        }
                    }
                    else
                    {
                        if (bImg)
                        {
                            sfilepath = Convert.ToString(drs[0][2]);
                        }
                        bfound = true;
                    }
                }

                for (int i=0;i<dtList.Rows.Count;i++)
                {
                    DataRow listrow = dtList.Rows[i];
                    sid = Convert.ToString(listrow[0]);
                    sname=Convert.ToString(listrow[1]);
                    spath = "";
                    if (bImg)
                    {
                        spath = Convert.ToString(listrow[2]);
                    }
                    if (bfound)
                    {
                        if (svalue.Length > 0 && svalue.ToLower() == listrow[0].ToString().ToLower())
                        {
                            sbtemp.Append(string.Format("<option value='{0}' selected ='selected' fp='{2}'>{1}</option>",
                                sid, sname, spath));
                        }
                        else
                        {
                            sbtemp.Append(string.Format("<option value='{0}' fp='{2}'>{1}</option>",
                               sid, sname, spath));
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            sbtemp.Append(string.Format("<option value='{0}' selected ='selected' fp='{2}'>{1}</option>",
                               sid, sname, spath));
                        }
                        else
                        {
                            sbtemp.Append(string.Format("<option value='{0}' fp='{2}'>{1}</option>", sid, sname, spath));
                        }
                    }
                }
            }
            
            sbtemp.Append("</select>");
            if (bImg)
            {
                string s = "./photo.aspx/?imgsrc=" + HttpUtility.UrlEncode(sfilepath);
                sbtemp.Append(string.Format("<img id='imgselect{0}' src='{1}'/>", columnName, s));
            }
            return sbtemp.ToString();
        }

        /// <summary>
        /// 创建类型为多选择的下列列表,值为编号，dtList表的第一个字段为编号，第二个字段为名称
        /// </summary>
        /// <param name="columnName">字段名称</param>
        /// <param name="EnableEmpty">是否为空</param>
        /// <param name="columnDes">字段中文解释</param>
        /// <param name="dtValue">字段值</param>
        /// <returns></returns>
        public static string GetControl_CheckSelect(string columnName, bool EnableEmpty, string columnDes,
            string sValueID, string sValueName, string stableName, DataTable dtList, bool bEnableEdit)
        {
            string rt = "";
            string sName="";
            StringBuilder sbTemp = new StringBuilder();
            StringBuilder sbDest = new StringBuilder();
            //sbTemp.Append("<option value='*'>所有数据</option>");
            string[] asValueID = sValueID.Split(',');

            if (dtList != null && dtList.Rows.Count > 0)
            {
                foreach (DataRow rowsub in dtList.Rows)
                {
                    sbTemp.Append(string.Format("<option value='{0}'>{1}</option>",
                        Convert.ToString(rowsub[0]),
                        Convert.ToString(rowsub[1])));
                    foreach (string s in asValueID)
                    {
                        if (s == rowsub[0].ToString())
                        {
                            if (sName.Length == 0)
                            {
                                sName = rowsub[1].ToString();
                            }
                            else
                            {
                                sName = sName + "," + rowsub[1].ToString();
                            }
                            break;
                        }
                    }
                }
            }

            string[] aID = sValueID.Split(',');
            //string[] aName = sValueName.Split(',');
            string[] aName = sName.Split(',');
            for (int i = 0; i < aID.Length; i++)
            {
                if(aID[i].Length>0)
                    sbDest.Append(string.Format("<option value='{0}'>{1}</option>", aID[i], aName[i]));
            }

            rt = string.Format(@"
                             <table class='wxbody' cellpadding='0' cellspacing='0' border='0'>                                   
                                    <tr>
                                        <td>
                                            <select size='20' id='source' style='width: 270' name='source' ondblclick='f_choose()'
                                                 class='wxinput'>
                                            {0}
                                            </select>
                                        </td>
                                        <td>
                                            <input value='>>>' onclick='f_chooseall()' type='button' title='全部加入' /><br />
                                            <br />
                                            <input value='>> ' onclick='f_choose()' type='button' title='单个加入' /><br />
                                            <br />
                                            <input value=' <<' onclick='f_unchoose()' type='button' title='单个删除' /><br />
                                            <br />
                                            <br />
                                            <input value='<<<' onclick='f_unallchoose()' type='button' title='全部删除' /><br />
                                        </td>
                                        <td >
                                            <select size='20' name='dest' multiple id='dest' runat='server'
                                                style='width: 280' class='wxinput' sqlId='{2}'>
                                                {1}
                                            </select>
                                        </td>
                                    </tr>
                                </table>", sbTemp.ToString(), sbDest.ToString(),columnName);
            return rt;
        }
        /// <summary>
        /// 创建类型为text，输入接受字符串的输入框,字段类型为字符串的控件
        /// </summary>
        /// <param name="columnName">字段名称</param>
        /// <param name="EnableEmpty">是否为空</param>
        /// <param name="columnDes">字段中文解释</param>
        /// <param name="svalue">字段值</param>
        /// <returns></returns>
        public static string GetControl_Image(string columnName, bool EnableEmpty, string columnDes, string svalue, bool bEnableEdit,
            string columnType, string fileext, string tablename, string respath, string sID)
        {
            string rt = string.Format("<input type='text' sqlId='{0}' defineName='RecordPageType' enableEmpty='{1}' stralert='{2}' value='{3}' style='width:90%' class='TextBorder04' id='imgpath' ctype='{4}' {5}><input type='text' id='imgpathtemp' Visible='False'>"
                + "<iframe id='frmPic' src='UploadPic.aspx?t={6}&d={7}&i={8}{9}' style='width:90%;height:150px;' frameborder='0'></iframe>",
                             columnName,
                             EnableEmpty ? "True" : "False",
                             columnDes.Replace("[", "").Replace("]", "") + "不能为空",
                             svalue,
                             columnType,
                             bEnableEdit ? "readonly='True'" : "",
                             tablename,
                             respath,
                             sID,
                             fileext.Length > 0 ? "&ext=" + fileext : "");
            return rt;
        }

        /// <summary>
        /// 创建类型为text，输入接受字符串的输入框,字段类型为字符串的控件
        /// </summary>
        /// <param name="columnName">字段名称</param>
        /// <param name="EnableEmpty">是否为空</param>
        /// <param name="columnDes">字段中文解释</param>
        /// <param name="svalue">字段值</param>
        /// <returns></returns>
        public static string GetControl_File(string columnName, bool EnableEmpty, string columnDes, string svalue, bool bEnableEdit, 
            string columnType, string fileext, string tablename, string respath,string sID)
        {
            string rt = string.Format("<input type='text' sqlId='{0}' defineName='RecordPageType' enableEmpty='{1}' stralert='{2}' value='{3}' style='width:90%' class='TextBorder04' id='imgpath' ctype='{4}' {5}><input type='text' id='imgpathtemp' Visible='False'>"
                + "<iframe id='frmPic' src='UploadOnlyFile.aspx?t={6}&d={7}&i={8}{9}' style='width:90%;height:60px;' frameborder='0'></iframe>",
                             columnName,
                             EnableEmpty ? "True" : "False",
                             columnDes.Replace("[", "").Replace("]", "") + "不能为空",
                             svalue,
                             "file",
                             bEnableEdit ? "readonly='True'" : "",
                             tablename,
                             respath,
                             sID,
                             fileext.Length>0?"&ext="+fileext:"");
            return rt;
        }

        /// <summary>
        /// 创建从webservice获取数据的下拉控件
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="WebUrl">数据源地址</param>
        /// <param name="Method">获取数据的方法名</param>
        /// <param name="paramstr">参数</param>
        /// <returns></returns>
        public static string GetControl_DW(string columnName, string paramstr)
        {
            string rt = "";
            appname = ((Loginresult)HttpContext.Current.Session["Session"]).AppName;
            string rolename = ((Loginresult)HttpContext.Current.Session["Session"]).RoleName;
            DataTable dt = GetSelectDTfromService(paramstr, rolename, columnName);
            rt=GetControl_WebSelect(columnName, dt);
            return rt;
        }


        public static string GetDataGridUI(string columnName, string paramstr)
        {
            string rt = "";
            appname = ((Loginresult)HttpContext.Current.Session["Session"]).AppName;
            string rolename = ((Loginresult)HttpContext.Current.Session["Session"]).RoleName;
            rt = "<table id='" + columnName + "' class='easyui-treegrid' title='可浏览厂站' style='width:265px;height:180px' data-options=\"singleSelect:false,idField: 'id',treeField: 'desc',animate: true,collapsible: true,fitColumns: true,url:'../basepage/HandlerExtend.ashx?param=" + paramstr + "'\">";
            rt += "<thead><tr><th field='view' width='80' formatter='viewcheck' ><input type='checkbox' value='1' id='ck" + columnName + "'  /> </th><th data-options=\"field:'id',hidden:true\">厂站ID</th><th data-options=\"field:'desc',width:150\">厂站名称</th><th data-options=\"field:'ExtendStr',hidden:true\">Name</th></thead></table>";
            rt += "<script>setTimeout(function(){$('#ck" + columnName + "').unbind();$('#ck" + columnName + "').bind('click',treegridSelectAll('ck" + columnName + "','" + columnName + "'))},500);</script>";
            return rt;
        }

        public static string GetDataGridUI(string columnName, string paramstr,string provalue)
        {
            string rt = "";
            appname = ((Loginresult)HttpContext.Current.Session["Session"]).AppName;
            string rolename = ((Loginresult)HttpContext.Current.Session["Session"]).RoleName;
            rt = "<table id='" + columnName + "' class='easyui-treegrid' title='可浏览厂站' style='width:265px;height:180px' provalue='" + provalue + "' data-options=\"singleSelect:false,idField: 'id',treeField: 'desc',animate: true,collapsible: true,fitColumns: true,url:'../basepage/HandlerExtend.ashx?param=" + paramstr + "'\">";
            rt += "<thead><tr><th field='view' width='80' formatter='viewcheck' ><input type='checkbox' value='1' id='ck" + columnName + "'  /> </th><th data-options=\"field:'id',hidden:true\">厂站ID</th><th data-options=\"field:'desc',width:150\">厂站名称</th><th data-options=\"field:'ExtendStr',hidden:true\">Name</th></thead></table>";
            rt += "<script>setTimeout(function(){$('#ck" + columnName + "').unbind();$('#ck" + columnName + "').bind('click',treegridSelectAll('ck" + columnName + "','" + columnName + "'))},500);</script>";
            return rt;
        }

        public static string GetDataGridHtml(string desc,string columnName, string paramstr)
        {
            string rt = "";
            string ajaxFileName = "";
            appname = ((Loginresult)HttpContext.Current.Session["Session"]).AppName;
            string rolename = ((Loginresult)HttpContext.Current.Session["Session"]).RoleName;
            if (columnName.IndexOf("ReportType", StringComparison.Ordinal) > -1)
            {
                ajaxFileName = "HandlerReportQueryUserData.ashx";
            }
            else
            {
                ajaxFileName = "HandlerUserData.ashx";
            }
            rt = "<table id='" + columnName + "' class='easyui-treegrid' title='" + desc +"' style='width:265px;height:180px' data-options=\"singleSelect:false,idField: 'id',treeField: 'desc',animate: true,collapsible: true,fitColumns: true,url:'../basepage/" + ajaxFileName + "?param=" + paramstr + "'\">";
            rt += "<thead><tr><th field='view' width='80' formatter='viewcheck' ><input type='checkbox' value='1' id='ck" + columnName + "'  /> </th><th data-options=\"field:'id',hidden:true\">ID</th><th data-options=\"field:'desc',width:150\">名称</th><th data-options=\"field:'ExtendStr',hidden:true\">Name</th></thead></table>";
            rt += "<script>setTimeout(function(){$('#ck" + columnName + "').unbind();$('#ck" + columnName + "').bind('click',treegridSelectAll('ck" + columnName + "','" + columnName + "'))},500);</script>";
            return rt;
        }

        public static string GetDataGridHtml(string desc, string columnName, string paramstr, string provalue)
        {
            string rt = "";
            string ajaxFileName = "";
            appname = ((Loginresult)HttpContext.Current.Session["Session"]).AppName;
            string rolename = ((Loginresult)HttpContext.Current.Session["Session"]).RoleName;
            if (columnName.IndexOf("ReportType", StringComparison.Ordinal) > -1)
            {
                ajaxFileName = "HandlerReportQueryUserData.ashx";
            }
            else
            {
                ajaxFileName = "HandlerUserData.ashx";
            }
            rt = "<table id='" + columnName + "' class='easyui-treegrid' title='" + desc + "' style='width:265px;height:180px' provalue='" + provalue + "' data-options=\"singleSelect:false,idField: 'id',treeField: 'desc',animate: true,collapsible: true,fitColumns: true,url:'../basepage/"+ ajaxFileName + "?param=" + paramstr + "'\">";
            rt += "<thead><tr><th field='view' width='80' formatter='viewcheck' ><input type='checkbox' value='1' id='ck" + columnName + "'  /> </th><th data-options=\"field:'id',hidden:true\"></th><th data-options=\"field:'desc',width:150\">名称</th><th data-options=\"field:'ExtendStr',hidden:true\">Name</th></thead></table>";
            rt += "<script>setTimeout(function(){$('#ck" + columnName + "').unbind();$('#ck" + columnName + "').bind('click',treegridSelectAll('ck" + columnName + "','" + columnName + "'))},500);</script>";
            return rt;
        }
        /// <summary>
        /// 从数据源获取数据然后转成DataTable
        /// </summary>
        /// <param name="paramstr">参数</param>
        /// <param name="rolename">角色名</param>
        /// <param name="columnName">字段名称</param>
        /// <returns></returns>
        public static DataTable GetSelectDTfromService(string paramstr, string rolename , string columnName )
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("value");
            List<String> listrow = WSUtil.GetInputDataFromWS(paramstr, appname, rolename);
            if (listrow != null && listrow.Count > 0)
            {
                if (!(columnName.ToLower().Contains("appname") ||columnName.ToLower().Contains("appcode")))
                {
                    for (int i = 0; i < listrow.Count; i = i + 2)
                    {
                        DataRow dr = dt.NewRow();
                        dr["id"] = listrow[i];
                        dr["value"] = listrow[i + 1];
                        dt.Rows.Add(dr);
                    }
                }
                else
                {
                    if (appname == URappname.ToLower())
                    {
                        for (int i = 0; i < listrow.Count; i = i + 2)
                        {
                            DataRow dr = dt.NewRow();
                            dr["id"] = listrow[i];
                            dr["value"] = listrow[i + 1];
                            dt.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < listrow.Count; i = i + 2)
                        {
                            if (listrow[i] == appname)
                            {
                                DataRow dr = dt.NewRow();
                                dr["id"] = listrow[i];
                                dr["value"] = listrow[i + 1];
                                dt.Rows.Add(dr);
                                break;
                            }
                        }
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// 生成下拉列表控件
        /// </summary>
        /// <param name="ColumnName">字段名称</param>
        /// <param name="dt">DataTable数据源</param>
        /// <returns></returns>
        public static string GetControl_WebSelect(string ColumnName,DataTable dt)
        {
            string rt = string.Format(@"<select defineName='RecordPageType' sqlId='{0}' style='width:50%' id='select{0}' >", ColumnName);
            foreach (DataRow dr in dt.Rows)
            {
                rt += string.Format("<option value='{0}'>{1}</option>", dr["id"].ToString(), dr["value"].ToString());
            }
            rt += "</select>";
            return rt;
        }

        /// <summary>
        /// 生成下拉列表控件
        /// </summary>
        /// <param name="ColumnName">字段名称</param>
        /// <param name="dt">DataTable数据源</param>
        /// <param name="svalue">默认选择的selectItem的值</param>
        /// <returns></returns>
        public static string GetControl_WebSelect(string ColumnName, DataTable dt,string svalue)
        {
            string rt="";
            if (ColumnName.ToLower() != "appname")
            {
                rt = string.Format(@"<select defineName='RecordPageType' sqlId='{0}' style='width:50%' id='select{0}' >", ColumnName);
            }
            else
            {
                rt = string.Format(@"<select defineName='RecordPageType' disabled='disabled' sqlId='{0}' style='width:50%' id='select{0}' >", ColumnName);
            }
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["id"].ToString() == svalue)
                {
                    rt += string.Format("<option selected='selected' value='{0}'>{1}</option>", dr["id"].ToString(), dr["value"].ToString());
                }
                else
                {
                    rt += string.Format("<option value='{0}'>{1}</option>", dr["id"].ToString(), dr["value"].ToString());
                }
            }
            rt += "</select>";
            return rt;
        }

        /// <summary>
        /// 从数据源获取父级下拉联动的默认值
        /// </summary>
        /// <param name="ColumnName">字段名称</param>
        /// <param name="WebUrl">数据源地址</param>
        /// <param name="Method">获取数据方法</param>
        /// <param name="svalue">默认选择的selectItem的值</param>
        /// <param name="paramstr">参数</param>
        /// <returns></returns>
        public static string GetControl_DWbyValue(string ColumnName, string svalue, string paramstr)
        {
            string rt = "";
            appname = ((Loginresult)HttpContext.Current.Session["Session"]).AppName;
            string rolename = ((Loginresult)HttpContext.Current.Session["Session"]).RoleName;
            DataTable dt = GetSelectDTfromService(paramstr, rolename, ColumnName);
            rt = GetControl_WebSelect(ColumnName, dt,svalue);
            return rt;
        }

        /// <summary>
        /// 生成具有联动功能的下拉列表
        /// </summary>
        /// <param name="ColumnName">字段名称</param>
        /// <param name="datasource">数据源</param>
        /// <param name="param">参数</param>
        /// <param name="dest">联动目的字段名称</param>
        /// <param name="dt">下拉列表数据</param>
        /// <returns></returns>
        public static string GetControl_WebSelectBindEvent(string ColumnName, string datasource, string param, string dest, DataTable dt)
        {
            string rt = string.Format(@"<select defineName='RecordPageType' sqlId='{0}' style='width:50%' id='select{0}' onchange=""f_changeselect(this,'{1}','{2}','{0}','{3}')"">", ColumnName, datasource, param, "select" + dest);
            foreach (DataRow dr in dt.Rows)
            {
                rt += string.Format("<option value='{0}'>{1}</option>", dr["id"].ToString(), dr["value"].ToString());
            }
            rt += "</select>";
            return rt;
        }

        public static string GetControl_WebSelectBindMutipleEvent(string ColumnName, string datasource, string param, string dest, DataTable dt)
        {
            string rt = string.Format(@"<select defineName='RecordPageType' sqlId='{0}' style='width:50%' id='select{0}' onchange=""f_changeselect(this,'{1}','{2}','{0}','{3}')"">", ColumnName, datasource, param , dest);
            foreach (DataRow dr in dt.Rows)
            {
                rt += string.Format("<option value='{0}'>{1}</option>", dr["id"].ToString(), dr["value"].ToString());
            }
            rt += "</select>";
            return rt;
        }


        public static string GetControl_WebSelectBindEventSrc(string ColumnName, string datasource, string param, string dest, DataTable dt,string srccloumnname)
        {
            string rt = string.Format(@"<select defineName='RecordPageType' sqlId='{0}' style='width:50%' id='select{0}' onchange=""f_changeselect(this,'{1}','{2}','{4}','{3}')"">", ColumnName, datasource, param, "select" + dest, srccloumnname);
            foreach (DataRow dr in dt.Rows)
            {
                rt += string.Format("<option value='{0}'>{1}</option>", dr["id"].ToString(), dr["value"].ToString());
            }
            rt += "</select>";
            return rt;
        }
        /// <summary>
        /// 生成具有联动功能的默认值下拉列表
        /// </summary>
        /// <param name="ColumnName">字段名称</param>
        /// <param name="datasource">数据源</param>
        /// <param name="param">参数</param>
        /// <param name="dest">联动目的字段名称</param>
        /// <param name="dt">下拉列表数据</param>
        /// <param name="svalue">默认选项的value</param>
        /// <returns></returns>
        public static string GetControl_WebSelectBindEvent(string ColumnName, string datasource, string param, string dest, DataTable dt, string svalue)
        {
            string rt = "";
            if (ColumnName.ToLower() != "appname")
            {
                rt = string.Format(@"<select defineName='RecordPageType' sqlId='{0}' style='width:50%' id='select{0}' onchange=""f_changeselect(this,'{1}','{2}','{0}','{3}')"">", ColumnName, datasource, param, "select" + dest);
            }
            else
            {
                rt = string.Format(@"<select defineName='RecordPageType' disabled='disabled' sqlId='{0}' style='width:50%' id='select{0}' onchange=""f_changeselect(this,'{1}','{2}','{0}','{3}')"">", ColumnName, datasource, param, "select" + dest);
            }
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["id"].ToString() == svalue)
                {
                    rt += string.Format("<option selected='selected' value='{0}'>{1}</option>", dr["id"].ToString(), dr["value"].ToString());
                }
                else
                {
                    rt += string.Format("<option value='{0}'>{1}</option>", dr["id"].ToString(), dr["value"].ToString());
                }
            }
            rt += "</select>";
            return rt;
        }



        /// <summary>
        /// 获取添加页面联动目标字段的数据
        /// </summary>
        /// <param name="datasource">数据源</param>
        /// <param name="Method">获取数据的方法</param>
        /// <param name="param">参数</param>
        /// <param name="paramstr">参数</param>
        /// <param name="rolename">角色名</param>
        /// <returns></returns>
        private static DataTable GetAddDestSelectOption(string datasource,string Method,string param,string paramstr,string rolename)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("value");
            List<String> listrow = WSUtil.GetInputDataFromWS(datasource, Method.ToString().Split('?')[0], param, paramstr, appname, rolename);
            if (listrow != null && listrow.Count > 0)
            {
                for (int i = 0; i < listrow.Count; i = i + 2)
                {
                    DataRow dr = dt.NewRow();
                    dr["id"] = listrow[i];
                    dr["value"] = listrow[i + 1];
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        private static DataTable GetAddDestSelectOptionWithSrc(string datasource, string Method, string param, string paramstr, string rolename)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("value");
            List<String> listrow = WSUtil.GetInputDataFromWSWithSRC(datasource, Method.ToString().Split('?')[0], param, paramstr, appname, rolename);
            if (listrow != null && listrow.Count > 0)
            {
                for (int i = 0; i < listrow.Count; i = i + 2)
                {
                    DataRow dr = dt.NewRow();
                    dr["id"] = listrow[i];
                    dr["value"] = listrow[i + 1];
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        /// <summary>
        /// 生成具有联动功能的下拉列表
        /// </summary>
        /// <param name="ColumnName">字段名称</param>
        /// <param name="WebUrl">数据源地址</param>
        /// <param name="datasource">数据源</param>
        /// <param name="destmethod">获取数据的方法</param>
        /// <param name="dest">联动字段名称</param>
        /// <param name="svalue">默认选项的value</param>
        /// <param name="paramstr">参数</param>
        /// <returns></returns>
        public static string GetControl_DWbyValueBind(string ColumnName, string WebUrl, string datasource, string dest, string svalue, string paramstr)
        {
            appname = ((Loginresult)HttpContext.Current.Session["Session"]).AppName;
            string rolename = ((Loginresult)HttpContext.Current.Session["Session"]).RoleName;
            string[] par = paramstr.Split(';');
            if (par.Length > 3 && par[2] != null)
            {
                par[2] = dest;
            }
            string temp = par[0] + ";" + par[1] + ";" + par[2] + ";" + par[3];
            string rt = "";
            DataTable dt = GetSelectDTfromService(paramstr, rolename, ColumnName);
            rt = GetControl_WebSelectBindEvent(ColumnName, datasource, temp, dest, dt,svalue);
            return rt;
        }
        public static string GetControl_WebServicesSelectSrc(string ColumnName, string datasource, string Method, string param, string paramstr)
        {
            appname = ((Loginresult)HttpContext.Current.Session["Session"]).AppName;
            string rolename = ((Loginresult)HttpContext.Current.Session["Session"]).RoleName;
            string rt = "";
            DataTable dt = GetAddDestSelectOptionWithSrc(datasource, Method, param, paramstr, rolename);
            rt += GetControl_WebSelect(ColumnName, dt);
            return rt;
        }
        public static string GetControl_WebServicesSelect(string ColumnName, string datasource, string Method, string param, string paramstr)
        {
            appname = ((Loginresult)HttpContext.Current.Session["Session"]).AppName;
            string rolename = ((Loginresult)HttpContext.Current.Session["Session"]).RoleName;
            string rt = "";
            DataTable dt = GetAddDestSelectOption(datasource, Method, param, paramstr, rolename);
            rt += GetControl_WebSelect(ColumnName,dt);
            return rt;
        }

        public static string GetControl_WebServicesSelect(string ColumnName, string datasource, string Method, string param, string destdatasource, string destcolumnname, string paramstr, string destparam)
        {
            appname = ((Loginresult)HttpContext.Current.Session["Session"]).AppName;
            string rolename = ((Loginresult)HttpContext.Current.Session["Session"]).RoleName;
            string rt = "";
            DataTable dt = GetAddDestSelectOption(datasource, Method, param, paramstr, rolename);
            rt = GetControl_WebSelectBindEvent(ColumnName, datasource, destparam, destcolumnname, dt);
            return rt;
        }

        public static string GetControl_WebServicesSelect(string ColumnName, string datasource, string Method, string param, string destdatasource, string destcolumnname, string paramstr, string destparam,string srccloumnname)
        {
            appname = ((Loginresult)HttpContext.Current.Session["Session"]).AppName;
            string rolename = ((Loginresult)HttpContext.Current.Session["Session"]).RoleName;
            string rt = "";
            DataTable dt = GetAddDestSelectOption(datasource, Method, param, paramstr, rolename);
            rt = GetControl_WebSelectBindEventSrc(ColumnName, datasource, destparam, destcolumnname, dt,srccloumnname);
            return rt;
        }

        public static string GetControl_DefaultSvgSelect(string businessname, string extendColumnName, string columnName)
        {
            appname = ((Loginresult)HttpContext.Current.Session["Session"]).AppName;
            object[] args = new object[] { "BusinessName=" + businessname, "ComponentName=" + businessname, "EditColumnName=" + columnName, "appname=" + appname};
            List<UTDtCnvrt.MRDDataAll> listMrdd = AppCode.WSUtil.getGridDataFromWS(EnumServiceFlag.businessservice, "bsusrtab", "", "", args);
            List<object> listTreeNode = DataReflect.FromContractData(listMrdd, typeof(SVGNode));
            string rt = string.Format(@"<select defineName='RecordPageType' sqlId='{0}' style='width:50%' id='select{0}' >", extendColumnName);
            foreach (SVGNode svgNode in listTreeNode)
            {
                if(!string.IsNullOrEmpty(svgNode.filesize) && Convert.ToInt32(svgNode.filesize) > 0)
                rt += string.Format("<option value='{0}'>{1}</option>", svgNode.desc, svgNode.name);
            }
            rt += "</select>";
            return rt;
        }
        public static string GetControl_DefaultSvgSelect(string businessname, string extendColumnName, string columnName,string value)
        {
            appname = ((Loginresult)HttpContext.Current.Session["Session"]).AppName;
            object[] args = new object[] { "BusinessName=" + businessname, "ComponentName=" + businessname, "EditColumnName=" + columnName, "appname=" + appname };
            List<UTDtCnvrt.MRDDataAll> listMrdd = AppCode.WSUtil.getGridDataFromWS(EnumServiceFlag.businessservice, "bsusrtab", "", "", args);
            List<object> listTreeNode = DataReflect.FromContractData(listMrdd, typeof(SVGNode));
            string rt = string.Format(@"<select defineName='RecordPageType' sqlId='{0}' style='width:50%' id='select{0}' >", extendColumnName);
            foreach (SVGNode svgNode in listTreeNode)
            {
                if (!string.IsNullOrEmpty(svgNode.filesize) && Convert.ToInt32(svgNode.filesize) > 0)
                {
                    if (svgNode.name == value)
                    {
                        rt += string.Format("<option value='{0}' selected='selected'>{1}</option>", svgNode.desc, svgNode.name);
                    }
                    else
                    {
                        rt += string.Format("<option value='{0}'>{1}</option>", svgNode.desc, svgNode.name);
                    }
                }
                    
            }
            rt += "</select>";
            return rt;
        }
        /// <summary>
        /// 生成具有联动功能的下拉列表
        /// </summary>
        /// <param name="ColumnName">字段名称</param>
        /// <param name="datasource">数据源</param>
        /// <param name="Method">获取数据的方法</param>
        /// <param name="destmethod">联动字段数据源的方法</param>
        /// <param name="dest">联动字段名称</param>
        /// <param name="paramstr">参数</param>
        /// <returns></returns>
        public static string GetControl_DWBindEvent(string ColumnName, string datasource, string dest, string paramstr)
        {
            appname = ((Loginresult)HttpContext.Current.Session["Session"]).AppName;
            string rolename = ((Loginresult)HttpContext.Current.Session["Session"]).RoleName;
            string[] par = paramstr.Split(';');
            if (par.Length > 3 && par[2] != null)
            {
                par[2] = dest;
            }
            string temp = par[0] + ";" + par[1] + ";" + par[2] + ";" + par[3];
            string rt = "";
            DataTable dt = GetSelectDTfromService(paramstr, rolename, ColumnName);
            rt = GetControl_WebSelectBindEvent(ColumnName, datasource, temp, dest, dt);
            return rt;
        }

        public static string GetControl_DWBindMutipleEvent(string ColumnName, string datasource,string paramstr,List<string> ls)
        {
            appname = ((Loginresult)HttpContext.Current.Session["Session"]).AppName;
            string rolename = ((Loginresult)HttpContext.Current.Session["Session"]).RoleName;
            string[] par = paramstr.Split(';');
            string dest = "";
            string destcolumn = "";
            for (int i = 0; i < ls.Count; i++)
            {
                dest += "select" + ls[i] + ";";
                destcolumn += ls[i] + SPERATOR.ToLower();
            }
            if (par.Length > 3 && par[2] != null)
            {
                par[2] = destcolumn;
            }
            string temp = par[0] + ";" + par[1] + ";" + par[2] + ";" + par[3];
            string rt = "";
            DataTable dt = GetSelectDTfromService(paramstr, rolename, ColumnName);
            rt = GetControl_WebSelectBindMutipleEvent(ColumnName, datasource, temp, dest, dt);
            return rt;
        }

        public static string GetControl_WebServicesSelect(string ColumnName, string datasource, string Method, string param, string svalue, string paramstr)
        {
            appname = ((Loginresult)HttpContext.Current.Session["Session"]).AppName;
            string rolename = ((Loginresult)HttpContext.Current.Session["Session"]).RoleName;
            string rt = "";
            DataTable dt = GetAddDestSelectOption(datasource, Method, param, paramstr, rolename);
            rt += GetControl_WebSelect(ColumnName, dt,svalue);
            return rt;
        }
    }
}