using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UTDtBaseSvr;
using UserRightObj;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using AppCode;
using UTDtCnvrt;
using System.Web.UI.HtmlControls;
//using BaseSrvMothed;

/// <summary>
/// 主页面处理类
/// </summary>
public partial class Public_Homepage : System.Web.UI.Page
{
    /// <summary>
    /// 主菜单
    /// </summary>

    protected string strrolename = "";
    protected string Notice = "";
    
    private string _homepage = "";
    public string title;
    public string Strappname = "";
    public string Strusername = "";
    public AppCode.UserMenuModel DbUm = null;

    public string style = ReadConfig.TheReadConfig["style"];
    public string Appname = ReadConfig.TheReadConfig["appname"];
    public string eventdisplaysize = InitModel.GetEventDisplaySize(ReadConfig.TheReadConfig["appname"]);
    public string showeventwindow = InitModel.GetShowEventWindow(ReadConfig.TheReadConfig["appname"]);

    private System.Timers.Timer timer;

    /// <summary>
    /// 加载页面时
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Appname != "")
        {
            Appname = "/" + Appname + "/";
        }
        if (!ValidateSession.Check())
            Response.Redirect(ValidateSession.GetRedirectUrl(1));
        Strappname = HttpContext.Current.Session["appname"].ToString();
        strrolename = HttpContext.Current.Session["RoleType"].ToString();
        HttpContext.Current.Session["starttime"] = DateTime.Now;
        Strusername = ((Loginresult)HttpContext.Current.Session["Session"]).NameAlias;
        DbUm = new AppCode.UserMenuModel(Strappname);
        RoleName.Value = Strusername;
        if (Strappname == "urptfrm")
        {
            Strusername = "管理员 " + Strusername;
        }
        else
        {
            Strusername = "用户 " + Strusername;
        }
        UserInfo.Text = Strusername;

        //第一次加载

        if (!IsPostBack)
        {
           // BindCss();

            BindCss("com.css");
            BindCss("ucList.css");
            BindCss("themes/default/easyui.css");
            BindCss("themes/icon.css");

            
            Init();
            myframe.Attributes.Add("src", _homepage);//"../" + roles + "/main.aspx"


            if (string.IsNullOrEmpty(Notice)) PopFrame.Attributes.Add("src", "");
            else
            {
                PopFrame.Attributes.Add("src", "../basepage/WinPOP.aspx?Notice=" + Notice);
            }

            title = LoginUserModel.Title;
        }


    }

    /// <summary>
    /// 获得主界面菜单
    /// </summary>
    private new void Init()
    {
        DbUm.roles = strrolename;
        UserMenuInfo hp = DbUm.GetHomePage(Strappname, strrolename);
        if (hp != null)
        {
            if (!hp.link.Equals(""))
            {
                _homepage = hp.link + "?type=" + hp.name + "&SelectNodeName= > " + hp.desc + "&nbsp;>&nbsp;" + System.Web.HttpUtility.UrlEncode(hp.desc);//hp.link
            }
        }
    }

    protected void LoginOut_Click(object sender, EventArgs e)
    {
        var lt = (Loginresult)HttpContext.Current.Session["Session"];
        var us = new USWebService();
        try
        {
            string pwd = HttpContext.Current.Session["password"].ToString();
            if (Convert.ToInt32(ReadConfig.TheReadConfig["sessionmode"]) == 1)
            {
                if (!string.IsNullOrEmpty(lt.Sessionid))
                {
                    ResultData rd = us.LoginOutUser(lt.Sessionid, pwd);
                    if (rd.result == 1 || rd.result == 0)
                    {
                        HttpContext.Current.Session["Session"] = null;
                        HttpContext.Current.Session["password"] = "";
                        HttpContext.Current.Session["username"] = "";
                        HttpContext.Current.Session["RoleType"] = "";
                        HttpContext.Current.Session["appname"] = "";
                        Response.Redirect(string.Format("../public/login/login.aspx?logintype={0}", strrolename.ToLower()));
                    }
                }
            }
            else
            {
                HttpContext.Current.Session["Session"] = null;
                HttpContext.Current.Session["password"] = "";
                HttpContext.Current.Session["username"] = "";
                HttpContext.Current.Session["RoleType"] = "";
                HttpContext.Current.Session["appname"] = "";
                Response.Redirect(string.Format("../public/login/login.aspx?logintype={0}", strrolename.ToLower()));
            }
           
        }
        catch (Exception ex)
        {
            Response.Redirect(string.Format("../public/login/login.aspx?logintype={0}", strrolename.ToLower()));
        }
    }

    public string trAddress
    {
        get
        {
            return @" 
            <tr id='traddress' runat='server'>
    

            </tr>
           
            ";

        }
    }

    private void BindCss()
    {

        HtmlGenericControl objLink = new HtmlGenericControl("LINK");

        objLink.Attributes["rel"] = "stylesheet";

        objLink.Attributes["type"] = "text/css";

        objLink.Attributes["href"] = "../css/" + style + "/com.css";

        comcss.Controls.Add(objLink);

    }

    private void BindCss(String css)
    {

        HtmlGenericControl objLink = new HtmlGenericControl("LINK");

        objLink.Attributes["rel"] = "stylesheet";

        objLink.Attributes["type"] = "text/css";

        objLink.Attributes["href"] = "../css/" + style + "/" + css;

        comcss.Controls.Add(objLink);

    }
}