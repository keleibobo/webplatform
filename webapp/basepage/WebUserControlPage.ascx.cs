using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using AppCode;
using UTDtCnvrt;
using UTDtBaseSvr;

public partial class UI_WebUserControlPage : System.Web.UI.UserControl
{
    public string Id
    {
        set;
        get;
    }

    string componentid = "component_1_";//默认格式

    string sizetype = "";
    string style = ReadConfig.TheReadConfig["style"];

    protected void Page_Load(object sender, EventArgs e)
    {
        BusinessCall bcCall = (BusinessCall)HttpContext.Current.Session["bcCall"];
        String BusinessType = bcCall.BussinessName;

        List<BusinessComponentCall> bcList = bcCall.bComponentList;
        String tableType = getTableType(bcList);
        foreach (BusinessComponentCall bc in bcList)
        {
           // if (bc.name.Equals(BusinessType))
            if (tableType.Equals(bc.name)) //bc.type.Equals( HtmlComponetType.querydatatable.ToString())&&
            {
              //  pageDefine.componentid = "component_" + bc.id+"_"+bc.type;
                string type = GetComponenType( bc,bcCall.bccList);
                componentid = "component_" + bc.id + "_" + type;
                if (!List.tablepage.ContainsKey(componentid))
                {
                    List.tablepage.Add(componentid, Id);
                }

                break;
               
            }
          
        }

         sizetype=getHtml(bcList);

       


    }

    /// <summary>
    /// 获取分页组件关联的组件类型
    /// </summary>
    /// <param name="bc"></param>
    /// <returns></returns>
    public string GetComponenType(BusinessComponentCall bc,List<BusinessConditionCall> bccList)
    {
        string rs=bc.type;;
        if ("business_condition".Equals(bc.sourcetype))
        {
            rs = "";// 关联的是条件，条件组件对应的component类型
            foreach (BusinessConditionCall bcCall in bccList)
            {
                if (bcCall.name.Equals(bc.name))
                {
                    rs = bcCall.ComponetType;
                }
            }
        }
       
        return rs;
    }

    public string getHtml(List<BusinessComponentCall> bcList)
    {
        string rs = "";

        foreach (BusinessComponentCall bc in bcList)
        {
            // if (bc.name.Equals(BusinessType))
            if (bc.id.Equals(Id)&&bc.type.Equals(HtmlComponetType.datatablepagenavigate.ToString()) )
            {
                Dictionary<string, object> extendMap = LayoutUI.getParam(bc.extendparam, ';', '=');
                if (extendMap != null && extendMap.ContainsKey("type"))
                {
                    rs = extendMap["type"].ToString();

                    break;
                }

            }

        }


        return rs;
    }

    private String getTableType(List<BusinessComponentCall> bcList)
    {
        foreach (BusinessComponentCall bc in bcList)
        {
            // if (bc.name.Equals(BusinessType))
            if (bc.type.Equals(HtmlComponetType.datatablepagenavigate.ToString())&&bc.id.Equals(Id))
            {
                return bc.datasource;
            }


        }

        return "";
    }

    public string getsubstring(string temp, string tag)
    {
        String rs = "";
        if (temp == null || tag == null) return rs;
        int index = temp.IndexOf(tag);
        if (index > -1)
        {
            rs = temp.Substring(index+tag.Length);
        }
        return rs;
    }

   // private PageControlDefine _pageDefine;

    //public PageControlDefine pageDefine //PageDefine
    //{
    //    set
    //    {
    //        _pageDefine = value;
    //    }
    //    get
    //    {
    //        if (_pageDefine ==null)
    //        {
    //            _pageDefine = new PageControlDefine();

    //        }
    //        return _pageDefine;
    //    }
    //}


    /// <summary>
    /// 将此控件呈现给指定的输出参数
    /// </summary>
    /// <param name="output"></param>
    protected override void Render(HtmlTextWriter output)
    {
        //:&nbsp;&nbsp;<span id='recordno' >0</span>&nbsp;&nbsp;条记录


        string sHtml = "";
        if (sizetype.ToLower().Equals("mini"))
        {
            sHtml = Mini;
        }
        else
        {
            sHtml = Max;
        }

        sHtml = string.Format("<div id='component_{0}_{1}' >{2}</div><script type='text/javascript' >setLayout('component_{0}_{1}','{3}');</script>", Id, HtmlComponetType.datatablepagenavigate, sHtml, getcss());//lastdiv('component_{0}_{1}','{4}')
        output.Write(sHtml);
    }

    public string GetSelectHtml(string id)
    {
        string[] pageList = ReadConfig.TheReadConfig["pagesizelist"].ToString().Split(';');
        string rt =string.Format("<select id='{0}_pagesize' onchange='SetPageNum(this)' >",id);
        
        id = id.Split('_')[1];
        int pagesize = 0;
        if (List.pagesizes.ContainsKey(id))
        {
             pagesize = List.pagesizes[id];
        }
        foreach (string s in pageList)
        {
            if (pagesize.ToString() != s)
            {
                rt += "<option>"+s.ToString()+"</option>";
            }
            else
            {
                rt += "<option selected='true'>" + s.ToString() + "</option>";
            }
        }
        rt += "</select>&nbsp;&nbsp;行/页&nbsp;&nbsp;";
        return rt;
    }

    public string getcss()
    {
        string rs = "";
        BusinessCall bcCall = (BusinessCall)HttpContext.Current.Session["bcCall"];
        Dictionary<string, object> layout= LayoutUI.getLayout(bcCall.bcLayoutList, Id);
        if (layout != null )
        {
            if (layout.ContainsKey("css"))
            rs = FormatUtil.toJSON(layout["css"]);
            else if(layout.ContainsKey("bottomcss")){ ///兼容原设计
                rs="{\"divcss\":"+FormatUtil.toJSON(layout["bottomcss"])+"}";
            }
        }
        return rs;
    }

    string Max{
        get
        {
            string sHtml = @"     <input id='style' runat='server'  type='hidden' value='{3}' />
                    <table width='100%' class='TextBorder006' height='25' border='0' cellpadding='0' cellspacing='0'>
                    <tr>
                        <td height='1' colspan='2' bgcolor='#FFFFFF'>
                        </td>
                    </tr>
                    <tr>
                        <td width='35%' style='padding-left: 20px' height='24' valign='middle' class='tdtitle03'>

                            总&nbsp;&nbsp;<span id='spansumpage{1}'>{0}</span>&nbsp;&nbsp;页&nbsp;&nbsp;-&nbsp;&nbsp;第&nbsp;&nbsp;<span id='spanpagenum{1}'>1</span>&nbsp;&nbsp;页&nbsp;&nbsp;
                        </td>
                        <td align='right'>
                            <table width='100%' height='24' border='0' cellpadding='0' cellspacing='0'>
                                <tr>
                                    <td style='width: 15%'>
                                    </td>
                                    <td width='100' align='center' valign='bottom' onclick='f_page(1,{2})' id='firsttd{1}'
                                        class='page1' style='cursor: pointer;text-align:left'>
                                        <img src='../images/{3}/首页_gray.gif' align='left' id='firstimg{1}' style='vertical-align: middle'/>&nbsp;&nbsp;首页&nbsp;&nbsp;
                                    </td>
                                    <td width='100' align='center' valign='bottom' onclick='f_page(2,{2})' id='pretd{1}'
                                        class='page1' style='cursor: pointer;text-align:left'>
                                        <img src='../images/{3}/上一页_gray.gif' align='left' id='preimg{1}' style='vertical-align: middle'/>&nbsp;&nbsp;上一页&nbsp;&nbsp;
                                    </td>
                                    <td width='100' align='center' valign='bottom' onclick='f_page(3,{2})' id='nexttd{1}'
                                        class='page1' style='cursor: pointer;text-align:right'>
                                        &nbsp;&nbsp;下一页&nbsp;&nbsp;<img src='../images/{3}/下一页_gray.gif' align='right' id='nextimg{1}' style='vertical-align: middle'/>
                                    </td>
                                    <td width='100' align='center' valign='bottom' onclick='f_page(4,{2})' id='endtd{1}'
                                        class='page1' style='cursor: pointer;text-align:right'>
                                        &nbsp;&nbsp;末页&nbsp;&nbsp;<img src='../images/{3}/末页_gray.gif' align='right' id='endimg{1}' style='vertical-align: middle'/>
                                    </td>
                                    <td width='2%'>
                                        <img src='../images/{3}/none.gif' width='20' height='10'/>
                                    </td>
                                    <td width='150' class='tdtitle03' align='right' style='padding-right: 20px'>
                                        跳转至&nbsp;&nbsp;<input name='currentpage' id='currentpage{1}' onkeydown='if(event.keyCode==13){{f_page(0,{2});return false;}}' size='3'
                                            value='1' type='text' class='TextBorder02' />页
                                    </td>
                                    <td style='width:100px;'>{4}</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>    <input name='SumPage' id='SumPage{1}' value='{0}' type='hidden' />";


            // sHtml = "<div id='component_{4}_{5}' >" + sHtml + "</div> <script type='text/javascript' >setLayout('component_" + Id + "','{3}');resizepage('','{1}')</script>";//PageDefine.id


            sHtml = String.Format(sHtml, 1, componentid, "\"" + componentid + "\"", style,GetSelectHtml(componentid));

            return sHtml;
        }
   }

    string Mini
    {
        get
        {
            return String.Format(@" <table width='100%' height='24' border='0' cellpadding='0' cellspacing='0'  class='TextBorder006'>
                                <tr>
                                    <td style='width: 15%'>
                                    </td>
                                    <td width='20%' style='padding-left: 10px' height='24' valign='middle' class='tdtitle03' nowrap>
                                     总<span id='spansumpage{1}'>{0}</span>/<span id='spanpagenum{1}'>1</span>
                                    </td>

                                    <td style='width:100%' align='center'><table><tr>

                                    <td width='20' align='center' valign='bottom' onclick='f_page(1,{2})' id='firsttd{1}'
                                        class='page1' style='cursor: pointer;text-align:left'>
                                        <img src='../images/{3}/首页_gray.gif' align='left' id='firstimg{1}' style='vertical-align: middle'/>
                                    </td>
                                    <td width='20' align='center' valign='bottom' onclick='f_page(2,{2})' id='pretd{1}'
                                        class='page1' style='cursor: pointer;text-align:left'>
                                        <img src='../images/{3}/上一页_gray.gif' align='left' id='preimg{1}' style='vertical-align: middle'/>
                                    </td>
                                    <td width='20' align='center' valign='bottom' onclick='f_page(3,{2})' id='nexttd{1}'
                                        class='page1' style='cursor: pointer;text-align:right'>
                                       <img src='../images/{3}/下一页_gray.gif' align='right' id='nextimg{1}' style='vertical-align: middle'/>
                                    </td>
                                    <td width='20' align='center' valign='bottom' onclick='f_page(4,{2})' id='endtd{1}'
                                        class='page1' style='cursor: pointer;text-align:right'>
                                        <img src='../images/{3}/末页_gray.gif' align='right' id='endimg{1}' style='vertical-align: middle'/>
                                    </td>

                                    </tr></table></td>  
                                    
                                    <td width='20%' class='tdtitle03' align='right' style='padding-right: 10px' nowrap>
                                        至&nbsp;&nbsp;<input name='currentpage' id='currentpage{1}' onkeydown='if(event.keyCode==13){{f_page(0,{2});return false;}}' size='3'
                                            value='1' type='text' class='TextBorder02' />
                                    </td>

                                    <td style='width: 15%;height:100%'>
                                    </td>
                                </tr>
                            </table><input name='SumPage' id='SumPage{1}' value='{0}' type='hidden' />", 1, componentid, "\"" + componentid + "\"", style);
        }
    }

   
}

