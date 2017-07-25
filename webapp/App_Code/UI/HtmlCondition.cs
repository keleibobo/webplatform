using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using AppCode;
using System.Data;
using UTDtBaseSvr;
namespace AppCode
{

    /// <summary>
    /// Summary description for HtmlCondition
    /// </summary>
    public static class HtmlCondition
    {

        public static string style = ReadConfig.TheReadConfig["style"];
        private static string src = "../images/" + style;

        #region
        public static string GetControl_Base(string columnName, string columnDes, string columnType, string queryValue, string swhere)
        {
            return string.Format(@"<td >{0}<input style=' overflow:visible; height:20;width:70' where='{4}' value='' name='{1}' value=""{2}"" class='TextBorder02' columntype='{3}' /></td>",
                            columnDes,
                            columnName,
                            queryValue,
                            columnType,
                            swhere
                            );
        }
    
        public static string GetControl_Number(string columnName, string columnDes, string columnType, string queryValue,string swhere)
        {
            return GetControl_Base(columnName, columnDes, columnType, queryValue,swhere);
        }

        public static string GetControl_Int(string columnName, string columnDes, string columnType, string queryValue,string swhere)
        {
            return  GetControl_Base(columnName,columnDes,columnType,queryValue,swhere);
        }

        public static string GetControl_VarChar(string columnName, string columnDes, string columnType, string queryValue, string swhere, string scolumnsql)
        {
            string rt = string.Format(@"<td >{0}<input style=' overflow:visible; height:20;width:70' where='{4}' value='' name='{1}' value=""{2}"" class='TextBorder02' columntype='{3}' fk='{5}' /></td>",
                            columnDes,
                            columnName,
                            queryValue,
                            columnType,
                            swhere,
                            scolumnsql
                            );
            return rt;
        }

        public static string GetControl_Datetime_Start(string columnName, string columnDes, string columnType, string queryValue,string swhere)
        {
            string rt = string.Format(@"<td>{0}从<input size='9' where='{4}' dataname='时间从' datatype='date' name='{1}' value='{2}' columntype='{3}' class='TextBorder02' />
                 <img onmouseout=""this.src='{5}/日历.png'"" onclick=""utf_calendar('{1}',1)"" 
                 style='cursor:hand' onmouseover=""this.src='{5}/日历.png'"" 
                 src='{5}/日历.png' align='center' valign='middle' /></td>",
                            columnDes,
                            columnName,
                            queryValue,
                            columnType,
                            swhere,
                            src
                            );
            return rt;
        }

        public static string GetControl_Datetime_End(string columnName, string columnType,string swhere)
        {
            string rt = string.Format(@"<td >到<input size='9' where='{2}' dataname='时间从' datatype='date' name='{0}9' columntype='{1}' value='' class='TextBorder02' />
                 <img onmouseout=""this.src='{3}/日历.png'"" onclick=""utf_calendar('{0}9',1)"" 
                 style='cursor:hand' onmouseover=""this.src='{3}/日历.png'"" 
                 src='{3}/日历.png' align='center' valign='middle' /></td>",
                            columnName, columnType,swhere,src);
            return rt;
        }

        public static string GetControl_Datetime_End(string columnName, string columnDes, string columnType, string queryValue)
        {
            string rt = string.Format(@"<td >到<input size='9' where='<=' dataname='时间从' datatype='date' name='{0}9' columntype='{1}' value='' class='TextBorder02' />
                 <img onmouseout=""this.src='{2}/日历.png'"" onclick=""utf_calendar('{0}9',1)"" 
                 style='cursor:hand' onmouseover=""this.src='{2}/日历.png'"" 
                 src='{2}/日历.png' align='center' valign='middle' /></td>",
                            columnName, columnType,src);
            return rt;
        }

        

        public static string GetControl_Combox(string columnName, string columnDes, string columnType, string queryValue,
           DataTable dtList, bool bImg,string swhere)
        {
            StringBuilder sbtemp = new StringBuilder();
            sbtemp.Append(string.Format("<td valign='middle' nowrap id='td{1}'>&nbsp;&nbsp;{0}&nbsp;&nbsp;</td>", columnDes, columnName));
            sbtemp.Append(string.Format(@"<td id='tdselect{0}'><select NAME='{0}' onchange=""f_changetype(this,'{0}')"" class='search1' where='{1}' >", columnName,swhere));

            string spath = "";
            if (dtList != null && dtList.Rows.Count > 0)
            {
                sbtemp.Append(string.Format("<option value='{0}' selected =selected>{1}</option>","",""));

                foreach (DataRow listrow in dtList.Rows)
                {
                    if (queryValue.Length > 0 && queryValue.ToLower() == listrow[0].ToString().ToLower())
                    {
                        spath = Convert.ToString(listrow[1]);
                        sbtemp.Append(string.Format("<option value='{0}' selected =selected>{1}</option>",
                            Convert.ToString(listrow[0]),
                            Convert.ToString(listrow[1])));
                    }
                    else
                    {
                        if (spath.Length == 0)
                        {
                            spath = Convert.ToString(listrow[1]);
                        }
                        sbtemp.Append(string.Format("<option value='{0}'>{1}</option>",
                            Convert.ToString(listrow[0]),
                            Convert.ToString(listrow[1])));
                    }
                }
            }

            sbtemp.Append("</select>");
            if (bImg)
            {
                string s = "./photo.aspx/?imgsrc=" + HttpUtility.UrlEncode(spath);
                sbtemp.Append(string.Format("<img id='imgselect{0}' src='{1}'/>", columnName, s));
            }
            sbtemp.Append("</td>");
            return sbtemp.ToString();
        }
        #endregion
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
        /// <summary>
        /// 基本控件
        /// </summary>
        /// <param name="cd"></param>
        /// <returns></returns>
        public static string GetControl_Base(BusinessConditionCall cd)
        {
            string rt = string.Format(@"<td >{0}<input style=' overflow:visible; height:20;width:70'  value='' name='ConditionName' id={1} value=""{2}"" class='TextBorder02' columntype='{3}' onkeydown='enterquery()' /></td>",
                             cd.DictionaryName,
                             cd.name,
                             cd.DisplaySelect.Count > 0 ? cd.DisplaySelect[0].ToString() : "",
                             ""
                             );
            return rt;
        }
        /// <summary>
        /// 浮点数输入控件
        /// </summary>
        /// <param name="cd"></param>
        /// <returns></returns>
        public static string GetControl_Number(BusinessConditionCall cd)
        {
            return GetControl_Base(cd);
        }
        /// <summary>
        /// 数字输入控件
        /// </summary>
        /// <param name="cd"></param>
        /// <returns></returns>
        public static string GetControl_Int(BusinessConditionCall cd)
        {
            return GetControl_Base(cd);
        }
        /// <summary>
        /// 字符串输入控件
        /// </summary>
        /// <param name="cd"></param>
        /// <returns></returns>
        public static string GetControl_VarChar(BusinessConditionCall cd)
        {
            string rt = string.Format(@"<td >{0}<input style=' overflow:visible; height:20;width:70'  value='' name='ConditionName' id={1} value=""{2}"" class='TextBorder02' columntype='{3}' /></td>",
                            cd.DictionaryName,
                            cd.name,
                            cd.DisplaySelect.Count>0?cd.DisplaySelect[0].ToString():"",
                            ""
                            );
            return rt;
        }
        /// <summary>
        /// 标准日期+时间控件
        /// </summary>
        /// <param name="cd"></param>
        /// <returns></returns>
        public static string GetControl_Datetime(BusinessConditionCall cd)
        {
            string time = "yyyy-MM-dd HH:mm:ss";
            if (cd.name.ToLower().IndexOf("start")>-1)
            {
                // time = "00:00:00";
              //  time = DateTime.Now.AddDays(Convert.ToDouble((0 - Convert.ToInt16(DateTime.Now.DayOfWeek))) - 7).ToString(time); ;
                time = DateTime.Now.AddDays(- 7).ToString(time); ;
                // time = DateTime.Now.ToString("yyyy-MM-dd " + time);// "00:00:00";
            }
            else
            {
                time = DateTime.Now.ToString(time);
            }
            string rt = string.Format(@"<td align='center' valign='middle' id='td1'>
                                                        &nbsp;&nbsp;{1}&nbsp;&nbsp;<input type='text' size='11' id='{0}' name='ConditionNameDatetime' style='width:150px;'
                                                            style='width: 110px;' value='{2}' valign='middle' class='TextBorder02' />
                                                    </td>
                                                    <td nowrap onclick=""utf_calendar('{0}',0)"" width='31' height='24' id='td2' >
                                                    <img onmouseout=""this.src='{3}/日历.png'"" style='cursor: hand' onmouseover=""this.src='{3}/日历.png'""
                                                        src='{3}/日历.png' align='center' valign='middle' /></td>",
                            cd.name, cd.DictionaryName,time,src);//  cd.alDisplaySelect.Count > 0 ? cd.alDisplaySelect[0].ToString() : "")
            return rt;
        }

        /// <summary>
        /// 选择日期控件
        /// </summary>
        /// <param name="cd"></param>
        /// <returns></returns>
        public static string GetControl_Date(BusinessConditionCall cd)
        {

            string timeformat = "yyyy-MM-dd";
           // string time = getTime(cd.para,timeformat);
            string time = "";
            if (cd.name.ToLower().IndexOf("start")>-1&&cd.para!=null&&!cd.para.Equals(""))
            {
                // time = "00:00:00";
                string[] split= cd.para.Split(',');
                if (split[1].ToLower().IndexOf("day") > -1)
                {
                    int day = Convert.ToInt32(split[0]);
                    time = DateTime.Now.AddDays(day).ToString(timeformat); 
                }
                // time = DateTime.Now.ToString("yyyy-MM-dd " + time);// "00:00:00";
            }
            else
            {
                time = DateTime.Now.ToString(timeformat);
            }

            
            string rt = string.Format(@"<td align='center' valign='middle' id='td1'>
                                                        &nbsp;&nbsp;{1}&nbsp;&nbsp;<input type='text' size='11' id='{0}' name='ConditionNameDate'
                                                            style='width: 110px;' value='{2}' valign='middle' class='TextBorder02' />
                                                    </td>
                                                    <td nowrap onclick=""utf_date('{0}')"" width='31' height='24' id='td2' />
                                                    <img onmouseout=""this.src='{3}/日历.png'"" style='cursor: hand' onmouseover=""this.src='{3}/日历.png'""
                                                        src='{3}/日历.png' align='center' valign='middle' />",
                            cd.name, cd.DictionaryName,time ,src);//cd.alDisplaySelect.Count > 0 ? cd.alDisplaySelect[0].ToString() : ""
            return rt;
        }


        public static string GetControl_UIDate(BusinessConditionCall cd)
        {

            string timeformat = "yyyy-MM-dd";
            // string time = getTime(cd.para,timeformat);
            string time = "";
            if (cd.name.ToLower().IndexOf("start") > -1 && cd.para != null && !cd.para.Equals(""))
            {
                // time = "00:00:00";
                string[] split = cd.para.Split(',');
                if (split[1].ToLower().IndexOf("day") > -1)
                {
                    int day = Convert.ToInt32(split[0]);
                    time = DateTime.Now.AddDays(day).ToString(timeformat);
                }
                
                // time = DateTime.Now.ToString("yyyy-MM-dd " + time);// "00:00:00";
            }
            else
            {
                time = DateTime.Now.ToString(timeformat);
            }
            string param = "";
            if (cd.para.IndexOf("type=month")>-1)
            {
                param = "{dateFmt:'yyyy-MM'}";
                time = DateTime.Now.ToString("yyyy-MM");
            }

            string rt = string.Format(@"<td align='center' valign='middle' >
                        &nbsp;&nbsp;{1}&nbsp;&nbsp;<input name='ConditionNameDate_{0}' id='{0}' style='width:105px;' class='Wdate'  value='{2}' type='text' onclick=""WdatePicker({3})"" /></td>",
                            cd.name, cd.DictionaryName,time,param);
            return rt;
        }


        public static string GetControl_UIDatetime(BusinessConditionCall cd)
        {

            string time = "yyyy-MM-dd HH:mm:00";
            string format = "dateFmt:'yyyy-MM-dd HH:mm:ss'"; 
            if (cd.para.ToLower().IndexOf("type=zero", System.StringComparison.Ordinal) > -1)
            {
                time = "00:00:00";
                format = "dateFmt:'HH:mm:ss'";
            }
            else if (cd.name.ToLower().IndexOf("start", System.StringComparison.Ordinal) > -1)
            {
                time = DateTime.Now.AddDays(-1).ToString(time);
            }
            else
            {
                time = DateTime.Now.ToString(time);
            }

            string rt = string.Format(@"<td align='center' valign='middle' >
                        &nbsp;&nbsp;{1}&nbsp;&nbsp;<input name='ConditionNameDatetime_{0}' id='{0}' style='width:150px;' class='Wdate'  type='text'  value='{2}' onfocus=""WdatePicker({{{3}}})"" /></td>",
                            cd.name, cd.DictionaryName, time,format);
            return rt;
        }


        private static string getTime(string timeflag,string timeformat){
            string rs=DateTime.Now.ToString(timeformat);
            if (timeflag == null || timeflag.Equals("")) return rs;
            {
                string[] split=timeflag.Split(',');
                double timeNo=Convert.ToDouble(split[0]);
                string timetype=split[1];
                switch(timetype){
                    case "Day": rs = DateTime.Now.AddDays(timeNo).ToString(timeformat); break;
                    default:break;

                }
            }
            return rs ;
        }

        public static string GetControl_ComboxContent(BusinessConditionCall cd)
        {
             StringBuilder sbtemp = new StringBuilder();
                foreach (UTDtBaseSvr.Pair p in cd.AllItemName)
                {
                    UTDtBaseSvr.Single bs = new UTDtBaseSvr.Single();
                    bs.Value = p.Key;
                    if (cd.DisplayEnable.Contains(bs))
                    {
                        if (cd.DisplaySelect.Contains(bs))
                            sbtemp.Append(string.Format("<option value='{0}' selected =selected>{1}</option>", p.Key, p.Value));
                        else
                            sbtemp.Append(string.Format("<option value='{0}'>{1}</option>", p.Key, p.Value));
                    }
                }
            return sbtemp.ToString();
        }
        /// <summary>
        /// 普通下拉列表控件
        /// </summary>
        /// <param name="cd"></param>
        /// <returns></returns>
        public static string GetControl_Combox(BusinessConditionCall cd)
        {
            StringBuilder sbtemp = new StringBuilder();
            Dictionary<string,string> paramDictionary = new Dictionary<string, string>();
            UTUtil.UtilFunc.GetParaToDictory(cd.para,paramDictionary,";",true);
            string width = paramDictionary.ContainsKey("width") ? paramDictionary["width"] : "150px";

            sbtemp.Append(string.Format("<td valign='middle' nowrap id='td{1}' width='60px'>&nbsp;&nbsp;{0}&nbsp;&nbsp;</td>", cd.DictionaryName, cd.name));
            sbtemp.Append(string.Format(@"<td id='tdselect{0}'><input class='easyui-combobox' style='width:{1}' align='left'
			name='ConditionName' id='{0}' sattr='combobox_{0}' vid='{0}' data-options=""valueField:'id',textField:'text'"" />", cd.name,width));
            sbtemp.Append("</td> ");            
            return sbtemp.ToString();
        }
        public static string GetControl_ChartCombox(BusinessConditionCall cd)
        {
            StringBuilder sbtemp = new StringBuilder();

            sbtemp.Append(string.Format("<td valign='middle' nowrap id='td{1}'>&nbsp;&nbsp;{0}&nbsp;&nbsp;</td>", cd.DictionaryName, cd.name));


            sbtemp.Append(string.Format(@"<td id='tdselect{0}'><input class='easyui-combobox' 
			name='ConditionName' id='{0}' sattr='combobox_{0}' vid='{0}' data-options=""valueField:'id',textField:'text'"" />", cd.name));
            sbtemp.Append("</td> ");
            Dictionary<string, object> extendMap = LayoutUI.getParam(cd.para, ';', '=');
            string func = "onchange";
            if (extendMap.ContainsKey(func))
            {
                sbtemp.Append(string.Format("<script type='text/javascript' >AutoLoadChartType('{0}')</script>", cd.name));

            }
            return sbtemp.ToString();
        }
        public static string GetControl_Combox(BusinessConditionCall cd,string sourceid,string destid)
        {
            StringBuilder sbtemp = new StringBuilder();

            sbtemp.Append(string.Format("<td valign='middle' nowrap id='td{1}'>&nbsp;&nbsp;{0}&nbsp;&nbsp;</td>", cd.DictionaryName, cd.name));
            //sbtemp.Append(string.Format("<td id='tdselect{0}'><select  NAME='ConditionName' id='{0}' onclick='changeselected(\"{0}\")'  ", cd.name));//style=' position:absolute;clip:   rect(2   100%   90%   201)'   onchange=\"document.getElementById('txtPlace').value=this.value\" ><option   value=''>   </option>

            //if (cd.AllItemName.Count > 0)
            //{
            //    sbtemp.Append(GetControl_ComboxContent(cd));
            //}
            //else
            //{
            //    sbtemp.Append("<option value=''>所有</option>");
            //}

            //sbtemp.Append("</select> ");//<input   id='txtPlace'   type='text'   style=' size:16;font-size:12px;   border:   none;   background-color:transparent;' />  

            //string script = String.Format("<script type='text/javascript' >changeselected(\"{0}\")</script>",cd.name);
            //sbtemp.Append(script);

            sbtemp.Append(string.Format(@"<td id='tdselect{0}'><input class='easyui-combobox' sourceid='{1}' destid='{2}'
			name='ConditionName' id='{0}' vid='{0}' data-options=""valueField:'id',textField:'text'"" />", cd.name,sourceid,destid));
            sbtemp.Append("</td> ");
          //  sbtemp.Append(string.Format("<script type='text/javascript' >f_loadcombobox('{0}')</script>", cd.name));
            return sbtemp.ToString();
        }
        /// <summary>
        /// 时间控件，只有小时分钟秒
        /// </summary>
        /// <param name="cd"></param>
        /// <returns></returns>
        public static string GetControl_Time(BusinessConditionCall cd)
        {
            StringBuilder sbtemp = new StringBuilder();

            sbtemp.Append(string.Format("<td valign='middle' nowrap id='td{1}'>&nbsp;&nbsp;{0}&nbsp;&nbsp;</td>", cd.DictionaryName, cd.name));
            sbtemp.Append(string.Format(@"<td id='tdselect{0}'>", cd.name));

            string st = string.Format(@"<input  type='text' sqlId='{0}' id='{0}' NAME='ConditionNameTime' onkeyDown='validate(this,2)'    enableEmpty='{1}' stralert='{2}' value='{3}' defineName='RecordPageType'  class='TextBorder04' size='18' />",
                           cd.name,
                           "False",
                           "",
                           DateTime.Now.ToString("HH:mm:ss"));
            sbtemp.Append(st);

            sbtemp.Append("</td>");

            return sbtemp.ToString();
        }
        /// <summary>
        /// 下拉年控件
        /// </summary>
        /// <param name="cd"></param>
        /// <returns></returns>
        public static string GetControl_Year(BusinessConditionCall cd)
        {
            StringBuilder sbtemp = new StringBuilder();

            sbtemp.Append(string.Format("<td valign='middle' nowrap id='td{1}'>&nbsp;&nbsp;{0}&nbsp;&nbsp;</td>", cd.DictionaryName, cd.name));
            sbtemp.Append(string.Format(@"<td id='tdselect{0}'><select NAME='ConditionNameYear' id='{0}' onchange=""f_changetype(this,'{0}')"" class='search1' where='{1}' >", cd.name, ""));
            int y = DateTime.Now.Year;
            if (cd.AllItemName.Count > 0)
            {
                //sbtemp.Append(string.Format("<option value='{0}' selected =selected>{1}</option>", "", ""));

                for (int i = y - 6; i <= y + 6; i++)//int i = 1; i <= 12; i++
                {
                        if (y==i)
                            sbtemp.Append(string.Format("<option value='{0}' selected =selected>{1}</option>", i, i)); 
                        else
                            sbtemp.Append(string.Format("<option value='{0}'>{1}</option>", i, i)); // i,i
                }
            }

            sbtemp.Append("</select>");
            sbtemp.Append("</td>");

            return sbtemp.ToString();
        }
        /// <summary>
        /// 选择某年某月控件
        /// </summary>
        /// <param name="cd"></param>
        /// <returns></returns>
        public static string GetControl_Month(BusinessConditionCall cd)
        {
            StringBuilder sbtemp = new StringBuilder();

            sbtemp.Append(string.Format("<td valign='middle' nowrap id='td{1}'>&nbsp;&nbsp;{0}&nbsp;&nbsp;</td>", "年", cd.name + "year"));
            sbtemp.Append(string.Format(@"<td id='tdselect{0}'><select NAME='ConditionNameYearMonth' id='{0}' onchange=""f_changetype(this,'{0}')"" class='search1' where='{1}' >", cd.name + "year", ""));
            int y = DateTime.Now.Year;
            if (cd.AllItemName.Count > 0)
            {
                for (int i = y-10; i <y+10; i++)
                {
                    if ( DateTime.Now.Year==i)
                            sbtemp.Append(string.Format("<option value='{0}' selected =selected>{1}</option>", i, i));
                        else
                            sbtemp.Append(string.Format("<option value='{0}'>{1}</option>", i, i));
                }
            }

            sbtemp.Append("</select>");
            sbtemp.Append("</td>");

            sbtemp.Append(string.Format("<td valign='middle' nowrap id='td{1}'>&nbsp;&nbsp;{0}&nbsp;&nbsp;</td>", "月", cd.name+"month"));
            sbtemp.Append(string.Format("<td id='tdselect{0}'><select NAME='ConditionNameMonth' id='{0}' >", cd.name + "month", ""));


            if (cd.AllItemName.Count > 0)
            {
                //sbtemp.Append(string.Format("<option value='{0}' selected =selected>{1}</option>", "", ""));

                for (int i = 1; i <= 12; i++)
                {
                    if ( DateTime.Now.Month==i)
                            sbtemp.Append(string.Format("<option value='{0}' selected =selected>{1}</option>", i, i));
                        else
                            sbtemp.Append(string.Format("<option value='{0}'>{1}</option>", i, i));
                }
            }

            sbtemp.Append("</select>");
            sbtemp.Append("</td>");

            return sbtemp.ToString();
        }
        /// <summary>
        /// 唯一选择Radio类控件
        /// </summary>
        /// <param name="cd"></param>
        /// <returns></returns>
        public static string GetControl_Radio(BusinessConditionCall cd)
        {
            StringBuilder sbtemp = new StringBuilder();

            sbtemp.Append(string.Format("<td valign='middle' nowrap id='td{1}'>&nbsp;&nbsp;{0}&nbsp;&nbsp;</td>", cd.DictionaryName, cd.name));
            sbtemp.Append(string.Format(@"<td id='tdselect{0}' name='ConditionNameRadio'>", cd.name));

            if (cd.AllItemName.Count > 0)
            {
                //sbtemp.Append(string.Format("<option value='{0}' selected =selected>{1}</option>", "", ""));

                foreach (UTDtBaseSvr.Pair p in cd.AllItemName)
                {
                    UTDtBaseSvr.Single bs = new UTDtBaseSvr.Single();
                    bs.Value = p.Key;
                    if (cd.DisplayEnable.Contains(bs))
                    {
                        if (cd.DisplaySelect.Contains(bs))
                            sbtemp.Append(string.Format(" {1}<input type='Radio'  value='{0}' name='ConditionNameRadio{2}' checked /> ", p.Key, p.Value, cd.name));
                        else
                            sbtemp.Append(string.Format(" {1}<input type='Radio' value='{0}' name='ConditionNameRadio{2}' /> ", p.Key, p.Value, cd.name));
                    }
                }
            }

            sbtemp.Append("</td>");

            return sbtemp.ToString();
        }
        /// <summary>
        /// 多选控件
        /// </summary>
        /// <param name="cd"></param>
        /// <returns></returns>
        public static string GetControl_Checkbox(BusinessConditionCall cd)
        {
            StringBuilder sbtemp = new StringBuilder();

            sbtemp.Append(string.Format("<td valign='middle' nowrap id='td{1}'>&nbsp;&nbsp;{0}&nbsp;&nbsp;</td>", cd.DictionaryName, cd.name));
            sbtemp.Append(string.Format(@"<td id='tdselect{0}' name='ConditionNameCheckbox'>", cd.name));

            if (cd.AllItemName.Count > 0)
            {
                //sbtemp.Append(string.Format("<option value='{0}' selected =selected>{1}</option>", "", ""));

                foreach (UTDtBaseSvr.Pair p in cd.AllItemName)
                {
                    UTDtBaseSvr.Single bs = new UTDtBaseSvr.Single();
                    bs.Value = p.Key;
                    if (cd.DisplayEnable.Contains(bs))
                    {
                        if (cd.DisplaySelect.Contains(bs))
                            sbtemp.Append(string.Format("{1}<input type='checkbox' id='{0}' value='{0}' name='ConditionNameCheckbox{2}' checked /> ", p.Key, p.Value, cd.name));
                        else
                            sbtemp.Append(string.Format("{1}<input type='checkbox' id='{0}' value='{0}' name='ConditionNameCheckbox{2}' /> ", p.Key, p.Value, cd.name));
                    }
                }
            }

            sbtemp.Append("</td>");

            return sbtemp.ToString();
        }



        public static string GetControl_Button(string name)
        {
            string rs = "";

            rs = @"<table width='80' height='24' border='0' cellpadding='0' cellspacing='0'>
          <tbody><tr>
            <td width='10' align='right'><img src='{1}/button3_l.png' width='8' height='24' /></td>
            <td width='30' align='center' background='{1}/button3_m.png'><img src='{1}/{0}.png' width='20' height='20' /></td>
            <td width='43' align='left' background='{1}/button3_m.png'><table width='100%' height='24' border='0' cellpadding='0' cellspacing='0'>
                <tbody><tr>
                  <td height='3'></td>
                </tr>
                <tr>
                  <td width='34' height='21' align='center'>{0}</td>
                </tr>
            </tbody></table></td>
            <td width='8' align='left'><img src='{1}/button3_r.png' width='8' height='24' /></td>
          </tr>
        </tbody></table>";

            rs = String.Format(rs, name,src);
            return rs;
        }
    }
}