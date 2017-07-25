using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; 
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Security;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!(IsPostBack || IsAsync))
        {
            //string returnUrl = Request.QueryString["ReturnUrl"];
            //if (returnUrl != null)
            //    if (returnUrl == "/")
            //        Response.Redirect("default.aspx");

            if ( User.IsInRole("jmj") || User.IsInRole("Administrator"))
                Response.Redirect("../Main.aspx");
            if(User.IsInRole("user"))
                Response.Redirect("../../user/Homepage.aspx");
            else if(Request.QueryString.Count>0) 
                Response.Redirect("Default.aspx");   
        }
    }
    protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = (Session["VldNum"].ToString() == args.Value);
    }
    protected void Login1_LoginError(object sender, EventArgs e)
    {
       
    }

    // 测试 2011-12-29
    protected void LoginButton_Click(object sender, EventArgs e)
    {

    }
}