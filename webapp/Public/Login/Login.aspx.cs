using System;
using System.Web;
using System.Web.Services;
using AppCode;
using UserRightObj;
using System.Web.UI.HtmlControls;

/// <summary>
/// 登录处理类
/// </summary>
public partial class user_Login : System.Web.UI.Page
{
    public string title = "";
    public string userhostip;
    public string userhostname = "";
    public string LogoTitle = ReadConfig.TheReadConfig[ReadConfig.TheReadConfig["appname"]+"logotitle"];
    public string style = ReadConfig.TheReadConfig["style"];
    public string RememberUser="";
    //获取客户端IP地址   
    public string getIP()
    {
        string result = String.Empty;
        result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (null == result || result == String.Empty)
        {
            result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }
        if (null == result || result == String.Empty)
        {
            result = HttpContext.Current.Request.UserHostAddress;
        }
        if (null == result || result == String.Empty)
        {
            return "0.0.0.0";
        }
        return result;
    }   

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            BindCss();
        }
        userhostip = getIP();        
        userhostname = Page.Request.UserHostName;
        title = LoginUserModel.Title.Trim();
        //初始化缓存
        InitModel.reinit();
        RememberUser = InitModel.GetRememberuser(ReadConfig.TheReadConfig["appname"]);
    }


    /// <summary>
    /// 验证登录用户是否合法
    /// </summary>
    /// <param name="UserName">用户名</param>
    /// <param name="Psw">密码</param>
    /// <param name="VldNum">验证码</param>
    /// <param name="RoleType">角色代码</param>
    /// <param name="appname">项目代码</param>
    /// <returns>如果长度为空，则正确，否则返回错误信息</returns>
    [WebMethod]
    public static string VerifyLoginUser(string UserName, string Psw, string VldNum, string RoleType, string appname,string ip,string hostname)
    {
        //如果用户没有选择项目，则取配置中的默认项目代码
        if (appname.Length == 0)
        {
            appname = ReadConfig.TheReadConfig["appname"].ToLower();
        }
        string ipmac = ip + "-UT_" + hostname;       
        int sessionmode = Convert.ToInt32(ReadConfig.TheReadConfig["sessionmode"]);

        string showVldNum = ReadConfig.TheReadConfig["ShowVldNum"];
        Boolean vld = false;
        try
        {
            vld = Convert.ToBoolean(showVldNum);
        }
        catch (Exception ex)
        {
            vld = true;
        }
        if (showVldNum != "" && vld)
        {
            if (Convert.ToString(HttpContext.Current.Session["VldNum"]) != VldNum)
            {
                return "不能登录;验证码错误";
            }
        }
        RoleType = RoleType.ToLower();
        USWebService us = new USWebService();
        Loginresult lt = us.LoginUser(appname, UserName, Psw, RoleType, sessionmode, ipmac);

        if (lt.Resultid == -3)
        {
            return "不能登录:连接用户会话服务失败";
        }

        if (lt.Resultid == 0)
        {
            return string.Format("你输入的用户{0}不存在,不能登录", UserName);
        }
        else if (lt.Resultid == 2)
        {
            return "不能登录:输入密码错误";
        }
        else if (lt.Resultid == -1)
        {
            return "不能登录:用户未激活，请联系管理员";
        }
        else if (lt.Resultid == -2)
        {
            return "不能登录:用户被锁定，请联系管理员";
        }
        else if (lt.Resultid == 13)
        {
            return "不能登录:用户名或者密码错误";
        }
        else if (lt.Resultid == 14)
        {
            return "不能登录:与用户验证服务连接失败";
        }
        else
        {
            LoginUserModel.AppName = lt.AppName;
            LoginUserModel.Roles = lt.RoleName;
            HttpContext.Current.Session["Session"] = lt;
            HttpContext.Current.Session["password"] = Psw;
            HttpContext.Current.Session["username"] = UserName;
            HttpContext.Current.Session["RoleType"] = lt.RoleName;
            HttpContext.Current.Session["appname"] = appname;
            if (lt.HomePage.Equals(""))
            {
                //有问题
                return "../../public/Homepage.aspx";
            }
            return "../../" + lt.HomePage;
        }
    }

    /// <summary>
    /// 获取用户的角色代码
    /// </summary>
    /// <param name="UserName">用户名</param>
    /// <returns>返回角色代码</returns>
    [WebMethod]
    public static string GetLoginUserRole(string UserName)
    {
        return "../Homepage.aspx";
    }

    private void BindCss()
    {
        HtmlGenericControl objLink = new HtmlGenericControl("LINK");
        objLink.Attributes["rel"] = "stylesheet";
        objLink.Attributes["type"] = "text/css";
        objLink.Attributes["href"] = "../../css/" +style+ "/com.css";
        comcss.Controls.Add(objLink);
    }
}

