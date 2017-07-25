using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UTDtBaseSvr;

public partial class UI_WebUserControlTreeViewNo : System.Web.UI.UserControl
{
    string BusinessType = "";
   // BusinessCall bcCall;
    public string appname = "";
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


    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            BusinessType = Request["stype"] != null ? Request["stype"] : "Main";
            appname = HttpContext.Current.Session["appname"].ToString();//ReadConfig.TheReadConfig["appname"].ToLower();

         //   setID();
        }

        //InitModel.init();
        //bcCall = InitModel.GetBusinessCall(appname, BusinessType);
        //HttpContext.Current.Session["bcCall"] = bcCall;
    }



    protected override void Render(HtmlTextWriter output)
    {
        String rs = "";

        rs = @" <input name='TreeNodeText' id='TreeNodeText' value='0' type='hidden' runat='server'/>
                <div width='100%' id='component_{2}_{3}'><ul id='ul_{2}'></ul></div>
                <script type='text/javascript' >f_loadtree('{0}','{1}','ul_{2}');setLayout('ul_{2}','west')</script>";
        rs = String.Format(rs, appname, BusinessType, Id, HtmlComponetType.treeview.ToString());
        output.Write(rs);
    }

    //public void setID()
    //{
       
    //    BusinessCall bcCall = (BusinessCall)HttpContext.Current.Session["bcCall"];
    //    List<BusinessComponentCall> bcList = bcCall.bComponentList;
    //    foreach (BusinessComponentCall bc in bcList)
    //    {
    //        if (bc.type.Equals(HtmlComponetType.treeview.ToString()))
    //        {
    //            treeviewDefine.ID = bc.id;
    //            break;
    //        }
    //    }

    //}
}