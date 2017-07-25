using System;
using System.Collections; 
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class images_Pic : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        //PowerAnalysisModel bm=new PowerAnalysisModel();
        //DataView dv1= bm.ExecuteSql("select pic from",null);
        if (Request["imgsrc"] == null)
        {
            return;
        }
        string s = Server.MapPath("~/").Replace(@"\", @"/") + HttpUtility.UrlDecode(Request["imgsrc"]);  
        byte[] bImg=null;
        try
        {
            AppCode.MyImage.SmallPic(s, out  bImg, 40, 40);
            Image1.Page.Response.Clear();
            Image1.Page.Response.ContentType = "image/jpeg";
            Image1.Page.Response.BinaryWrite(bImg);
        }
        catch (System.Exception ex)
        {
        	
        }
    }
}
