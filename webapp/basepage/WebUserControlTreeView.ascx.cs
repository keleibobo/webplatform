using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UTDtBaseSvr;
using AppCode;
using System.Text;
using UTUtil;

public partial class UI_WebUserControlTreeView : System.Web.UI.UserControl
{
    string BusinessType = "";
   // BusinessCall bcCall;
    public string appname = "";
    public string datatype = "";
    private string navtitle;
    Dictionary<string, object> extendparam;
  //  Dictionary<string, object> layout;
    public string Id
    {
        set;
        get;
    }

    public TreeViewControlDefine _treeviewDefine;

    public TreeViewControlDefine treeviewDefine
    {
        set
        {
            _treeviewDefine = value;
        }
        get
        {
            if (_treeviewDefine == null)
            {
                _treeviewDefine = new TreeViewControlDefine();
            }
            return _treeviewDefine;
        }
    }
    public BusinessCall bscall = null;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            BusinessType = Request["type"] != null ? Request["type"] : "Main";
            appname = HttpContext.Current.Session["appname"].ToString();//ReadConfig.TheReadConfig["appname"].ToLower();

         //   setID();
        }
        BusinessCall bcCall = (BusinessCall)HttpContext.Current.Session["bcCall"];
        bscall = bcCall;
        List<BusinessComponentCall> bcList = bcCall.bComponentList;
        foreach (BusinessComponentCall bc in bcList)
        {
            if (bc.type.Equals(HtmlComponetType.treeview.ToString()))
            {
                foreach(ClassNaviagteCall cnc in bcCall.bcNavigateList){
                    if (cnc.name.Equals(bc.name))
                    {
                      navtitle = cnc.desc;
                        break;
                    }
                }

                extendparam = LayoutUI.getParam(bc.extendparam, ';', '=');

                break;
            }
        }

      //   layout= LayoutUI.getLayout(bcCall.bcLayoutList, Id);
        //InitModel.init();
        //bcCall = InitModel.GetBusinessCall(appname, BusinessType);
        //HttpContext.Current.Session["bcCall"] = bcCall;

       //  extendparam = LayoutUI.getParam(bcCall,',',':');
    }



    protected override void Render(HtmlTextWriter output)
    {
        string source_id = "";
        string dest_id = "";
        string onclickfunc = "";
        string dbclickfunc = "";
        string param = "";
        foreach (BusinessComponentEventCall bcec in bscall.bcEventList)
        {
            if (Id == bcec.dest_id)
            {
                if (source_id != "")
                {
                    source_id += "," + bcec.source_id;
                }
                else
                {
                    source_id = bcec.source_id;
                }
                
            }
            if (Id == bcec.source_id)
            {
                if (param != "")
                {
                    param += "," + bcec.extendparam;
                }
                else
                {
                    param = bcec.extendparam;
                }

                if (dest_id != "")
                {
                    dest_id += "," + bcec.dest_id;
                }
                else
                {
                    dest_id = bcec.dest_id;
                }
                if (bcec.eventtype == "onclick")
                {
                    if (onclickfunc == "")
                        onclickfunc = bcec.eventfuncname;
                    else
                    {
                        onclickfunc += "," + bcec.eventfuncname;
                    }
                }
                if (bcec.eventtype == "dbclick")
                    dbclickfunc = bcec.eventfuncname;
            }
        }
        string checkbox = getCheckbox();
        string check = "";//

       // getFunc();
        String rs = "";
        //if returnchilddata
        rs = @" <input name='TreeNodeText' id='TreeNodeText' value='' type='hidden' runat='server'/>
                <input id='SelectNodeType' runat='server'  type='hidden' />
                <input id='SelectNode' name='SelectNode' type='hidden' runat='server' />
                <input id='SelectType' name='SelectType' type='hidden' runat='server' /> 
                <input id='SelectNodeName' name='SelectNodeName' type='hidden' runat='server' />
                <input id='nodepath' name='nodepath' type='hidden' runat='server' />
                <div  id='component_{2}_{3}' style='height:100%;width:100%'> 
                <div >{5}</div>
                <div class='treeback' style='height:100%;width:100%'><ul id='ul_{2}' sourceid='{6}' destid='{7}' oneclick='{8}' twoclick='{9}' param='{10}'  style='overflow:auto' class='easyui-tree' {0}></ul></div>
                </div>
                <script type='text/javascript' >f_loadtree('ul_{2}'); setLayout('component_{2}_{3}','{4}');{1};</script>"; //setTitle({6})
        rs = String.Format(rs,checkbox,check, Id, HtmlComponetType.treeview.ToString(), GetCss(), PanelTitle, source_id, dest_id,onclickfunc,dbclickfunc,param); //, "\"component_" + Id + "_" + HtmlComponetType.treeview.ToString() + "\""

        output.Write(rs);
    }


    private string GetCss()
    {
        BusinessCall bcCall = (BusinessCall)HttpContext.Current.Session["bcCall"];
        String rs = "";

        Dictionary<string, object> layout = LayoutUI.getLayout(bcCall.bcLayoutList, Id);

        if (layout != null&&layout.ContainsKey("css"))
        {
          
                rs = FormatUtil.toJSON(layout["css"]);
           
        }

        if (rs == null || rs.Equals("")) //默认
        {
            EasyUILayoutCSS css = new EasyUILayoutCSS();

            css.layoutcss = "{'width':'270','height':'100%'}";
            css.divcss = "{'height':'100%'}";

            rs = FormatUtil.toJSON(css);

        }


            return rs;
    }

  

    public String  getFunc()
    {
        //returnalldata  returnchilddata
        return "f_treeresize('"+Id+"')";

    }

    public String getCheckbox(){
        string rs = "";
        if (extendparam.ContainsKey("checkbox") && extendparam["checkbox"].ToString()=="1")
        {
            rs = "data-options=\"animate:true,checkbox:true\"";
        }

        return rs;
    }





    string style = ReadConfig.TheReadConfig["style"];
    public string PanelTitle
    {
        get
        {
            string title = "";
            BusinessCall bcCall = (BusinessCall)HttpContext.Current.Session["bcCall"];
            foreach (BusinessComponentLayoutCall businessComponentLayoutCall in bcCall.bcLayoutList)
            {
                if (businessComponentLayoutCall.columncount == "1" && businessComponentLayoutCall.rowcount == "1")
                {
                    Dictionary<string, string> dtconfig = new Dictionary<string, string>();
                    UtilFunc.GetParaToDictory(businessComponentLayoutCall.extendparam, dtconfig, ";", false);
                    if (dtconfig.ContainsKey("autohide"))
                    {
                        title = dtconfig["autohide"];
                    }
                }
            }
            string rs = "";
            if (string.IsNullOrEmpty(title))
            {
                rs = @"<div style='width:100%;'><table width='100%' height='24' border='0' cellpadding='0' cellspacing='0' class='TextBorder009' style='background:url(../images/{2}/line03.gif)' >
                  <tbody><tr>
                    <td width='6%'></td>
                    <td width='74%' height='23' valign='bottom' class='tdtitle05'><table width='100%' height='23' border='0' cellpadding='0' cellspacing='0'>
                      <tbody><tr>
                        <td height='6'></td>
                      </tr>
                      <tr>
                        <td height='15'>{1}</td>
                      </tr>
                      <tr>
                        <td height='2'></td>
                      </tr>
                    </tbody></table></td>
                    <td width='51' align='center' style='display:none' ><nobr><img src='../images/展开.gif' width='17' height='16'><img src='../images/收起.gif' width='17' height='16'><img src='../images/关闭_s.gif' width='17' height='16' onclick='closetree({0})' style='cursor:pointer'></nobr></td>
                  </tr>
                </tbody></table></div>";
                rs = String.Format(rs, "\"component_" + Id + "_" + HtmlComponetType.treeview.ToString() + "\"", navtitle, ReadConfig.TheReadConfig["style"]); 
            }
            return rs;
        }
    }
}