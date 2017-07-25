using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using AppCode;
using UserRightObj;
using UTDtCnvrt;

public partial class Public_Admin_ModifyPwd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="oldPassword">老密码</param>
    /// <param name="newPassword">新密码</param>
    [WebMethod]
    public static string ModifyPsw(string oldPassword, string newPassword)
    {
        try
        {
            string pwd = HttpContext.Current.Session["password"].ToString();
            if (pwd != oldPassword)
            {
                return "原密码输入不正确";
            }
            else
            {
                USWebService us = new USWebService();
                Loginresult lt = (Loginresult)HttpContext.Current.Session["Session"];
                string AppName = lt.AppName;
                string username = HttpContext.Current.Session["username"].ToString();
                ResultData rt = us.ChangeSelfPwd(lt.Sessionid, oldPassword, newPassword, AppName,username);
                if (rt == null)
                {
                    return "与用户会话服务连接失败";
                }
                if (rt.result == 1)
                {
                    HttpContext.Current.Session["password"] = newPassword;
                    return "修改成功";
                }
                else if (rt.result == 3)
                {
                    return "会话超时,请重新登录。";
                }
            }
        }
        catch (Exception e)
        {
            return "修改失败,原因：" + e.ToString();
        }
        return "修改失败";
    }

    public string savebutton
    {
        get
        {
            return HtmlCondition.GetControl_Button("修改"); ;
        }
    }

}