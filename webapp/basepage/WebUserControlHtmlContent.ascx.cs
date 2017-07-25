using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UTDtBaseSvr;
using AppCode;
using System.Text;

public partial class UI_WebUserControlHtmlContent : System.Web.UI.UserControl
{
    string title = "";
    string type = "";
    string src = "";
    private string css = "";
    Dictionary<string, object> layout;
    public string Id
    {
        set;
        get;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!(IsPostBack))
        {
            BusinessCall bcCall = (BusinessCall)HttpContext.Current.Session["bcCall"];
            layout = LayoutUI.getLayout(bcCall.bcLayoutList, Id);
            String BusinessType = bcCall.BussinessName;
            BusinessComponentCall bc=null;
            foreach (BusinessComponentCall bcc in bcCall.bComponentList)
            {
                if (Id.Equals(bcc.id))
                {
                    bc = bcc;
                    title =bcc.title;
                    type = bcc.type;
                    break;
                }
            }

            foreach (BusinessComponentLayoutCall bcc in bcCall.bcLayoutList)
            {
                if (Id.Equals(bcc.componentid))
                {
                    css = bcc.componentlayout;
                    break;
                }
            }
        }
    }

    protected override void Render(HtmlTextWriter output)
    {

        string rs = @"<div id='component_{0}_{1}' style='width:100%;height:100%;overflow-x: hidden; overflow-y:
        hidden;margin-top:5px;margin:0 auto;'>
        <iframe id='iframe_{0}' style='width:100%;height:100%;text-align:center;margin:0 auto;border-bottomt: 1px solid #008A8A;'
        src='../basepage/iframe.html' marginwidth=0 framespacing=0 marginheight=0 frameborder=0 ></iframe>
        </div><script>setLayout('component_{0}_{1}', '{2}');</script>";
        output.Write(String.Format(rs, Id, type,css));
    }
}