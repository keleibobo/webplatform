using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
 
using System.Web.UI;
using System.Web.UI.WebControls;
using AppCode;

public partial class user_ShowSvg : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sRt = "";
        //UtdrawModel utDM = new UtdrawModel();
        //utDM.strLoginID = LoginUserModel.PowerUser;
        //sRt = utDM.GetSVGBySiteID(Request.QueryString["id"]);
        Response.Clear();
        Response.ContentType = "image/svg+xml";
        //Response.ContentType = "application/xhtml+xml";
        Response.Write(sRt);
        Response.Flush();
        Response.Close();
    }
}