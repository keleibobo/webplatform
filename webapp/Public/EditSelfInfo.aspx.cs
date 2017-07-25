using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using AppCode;
using UserRightObj;
using UTDtCnvrt;

public partial class Public_EditSelfInfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!ValidateSession.Check())
            Response.Redirect(ValidateSession.GetRedirectUrl(2));
        if (!IsPostBack)
        {
            USWebService us = new USWebService();
            Loginresult lt = (Loginresult)HttpContext.Current.Session["Session"];
            User user = us.GetUserInfoBySid(lt.Sessionid);
            email.Text = user.Email;
            MobileAlias.Text = user.MobileAlias;
            MobilePIN.Text = user.MobliePin;
            PasswordQuestion.Text = user.PasswordQuestion;
            PasswordAnswer.Text = user.PasswordAnswer;
        }
    }
    [WebMethod]
    public static string UpdateSelfInfo(string Email,string PIN,string Question,string answer,string Alias)
    {
        USWebService us = new USWebService();
        Loginresult lt = (Loginresult)HttpContext.Current.Session["Session"];
        string pwd = HttpContext.Current.Session["password"].ToString();
        string AppName = lt.AppName;
        ResultData rt = us.UpdateSelf(lt.Sessionid, pwd, AppName, Email, PIN, Question, answer, Alias);
        if (rt == null)
        {
            return "与用户会话服务连接失败";
        }
        if (rt.result == 1)
            return "修改成功";
        else if (rt.result == -2)
            return "会话超时";
        else
            return "修改失败";
    }
}