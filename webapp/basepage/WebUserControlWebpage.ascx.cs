using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UTDtBaseSvr;
using AppCode;
using System.Text;

public partial class UI_WebUserControlWebpage : System.Web.UI.UserControl
{
  //  string sHtml = "";
    string title = "";
    string type = "";
    string src = "";
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

          

            if (bc != null)
            {
                //if (bc.sourcemethod.IndexOf(".htm") > -1) //静态html
                //    sHtml = getHTML(bc.sourcemethod); // or 
                if (bc.sourcetype != "" && bc.sourcetype != "webservice")
                {
                    src = bc.sourcemethod;
                    src = String.Format("apppage/{0}/{1}/{2}",bcCall.AppName,bcCall.BussinessName,bc.sourcemethod);
                }
              
               
            }
        }
    }

    //public string getHTML(string link)
    //{
    //    string path = String.Format("{0}apppage\\{1}\\{2}", System.AppDomain.CurrentDomain.BaseDirectory, LoginUserModel.AppName,link);
    //    string rs = ""; 
    //    try
    //    {
    //        System.IO.StreamReader sr = new System.IO.StreamReader(path);
    //        rs = sr.ReadToEnd() ;
    //    }catch(Exception e){
    //    }

        
    //    return rs;
    //}


    protected override void Render(HtmlTextWriter output)
    {
        //EasyUILayoutCSS css = new EasyUILayoutCSS();
        //css.layout = "center";

       

        string rs = @"<div class='layout-split-south' id='component_{0}_{1}' style='width:100%;height:100%;overflow-x: hidden; overflow-y: hidden;margin-top:5px;margin:0 auto;'>
        {3}
        <iframe id='iframe_{0}' style='width:100%;height:100%;text-align:center;margin:0 auto;border-bottomt: 1px solid #008A8A;'  src='../basepage/iframe.html' marginwidth=0 framespacing=0 marginheight=0 frameborder=0 ></iframe>
        {5}
        </div>
        <script type='text/javascript' >setLayout('component_{0}_{1}','{2}');{4}</script>";
        string bottom = "";
        string script = "";
        if (src != null && !src.Equals("") && src.IndexOf(".htm") > -1)
        {
            script = string.Format("f_loadiframe('iframe_{0}','../{1}')",Id,src);
        }
        else
        {
            title =string.Format( " <div style='width:100%;background: url(../images/line04.gif);height: 24px  ; padding-left:8px;vertical-align:middle;line-height:24px;' >{0}</div>",title);
            bottom="<div style='bottom: 32px; position: absolute;width:100%;height:2px;border-bottom:1px solid #008A8A'></div>";
        }
        string css = "[]";
       
        if (layout!=null)
        {
            css=FormatUtil.toJSON(layout["css"]);
        }
        output.Write(String.Format(rs, Id, type,css,title,script,bottom));//sHtml
    }
}