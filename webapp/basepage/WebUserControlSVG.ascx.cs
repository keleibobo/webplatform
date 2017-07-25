using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using UTDtBaseSvr;
using System.Text;
using System.Web.Services;
using System.Xml;
using AppCode;

public partial class UI_WebUserControlSVG : System.Web.UI.UserControl
{



    public SVGControlDefine _svgDefine;
    string appname;
    public string Id
    {
        set;
        get;
    }

    public SVGControlDefine svgDefine
    {
        set
        {
            _svgDefine = value;
        }
        get
        {
            if (_svgDefine == null)
            {
                // string appname=
                _svgDefine = new SVGControlDefine("");
            }
            return _svgDefine;
        }
    }



    protected void Page_Load(object sender, EventArgs e)
    {



        if (!(IsPostBack))
        {
         //   setID();
            appname = HttpContext.Current.Session["appname"].ToString();
        }
    }

    //public void setID()
    //{
    //    svgDefine.appname = LoginUserModel.AppName;
    //    BusinessCall bcCall = (BusinessCall)HttpContext.Current.Session["bcCall"];
    //    String BusinessType = bcCall.BussinessName;
    //    //  InitModel.GetComponentPageByType(kvp.Value);
    //    List<BusinessComponentCall> bcList = bcCall.bComponentList;
    //    foreach (BusinessComponentCall bc in bcList)
    //    {
    //        if (bc.type.Equals(HtmlComponetType.svg.ToString()))
    //        {
    //        //    svgDefine.ID = bc.id;
    //            break;
    //        }
    //    }

    
    //}


    protected override void Render(HtmlTextWriter output)
    {
        EasyUILayoutCSS css = new EasyUILayoutCSS();
        css.layout = "center";
        css.divcss = "{'width':'100%'}";//,'position':'absolute' ,'height':'100%'
       // css.layoutcss = "{'height':'100%'}";
        string rs = @"
    <input id='nodeid' name='nodeid' type='hidden' runat='server' value='1' />
    <div id='component_{0}_{2}' >
    <div id='svg' style='z-index:9;height:100px;width:100%;'  onmousedown='md(event)' onmouseup='mu()' onmousemove='mm(event,this)'  ></div>
   <div id='svgbutton' ></div>
            <div id='SvgNavigation' >
                <img id='svgpic' src='../images/point.png' onclick='Navigation(event)' style='visibility:hidden;' />
                <div id='NavigationArea' style=' border:2px solid gray; position:relative;'></div>
    </div>
    </div>
    
    <script type='text/javascript' >f_loadsvg('{1}','{3}');setLayout('component_{0}_{2}','{4}');resizesvg();</script>
     ";
        rs = String.Format(rs, Id, appname, HtmlComponetType.svg.ToString(),svgDefine.refData,FormatUtil.toJSON(css));
        output.Write(rs);
    }


}