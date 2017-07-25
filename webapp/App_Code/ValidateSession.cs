using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UserRightObj;

/// <summary>
/// Summary description for valudateSession
/// </summary>
public  class ValidateSession : System.Web.UI.Page
{
    public static string GetRedirectUrl(int level)
    {
        string Url = ReadConfig.TheReadConfig["RedirectPage"];
        string path = "";
        for (int i = 0; i < level; i++)
        {
            path += "../";
        }
        Url = path + Url;
        return Url;
    }
    public static bool Check()
    {
        bool rt = false;
        Loginresult lt = (Loginresult)HttpContext.Current.Session["Session"];
        if (lt != null)
        {
            rt = true;
        }
        else
        {
            return false;
        }
       

        return rt;
    }

    public static string FileName()
    {
        string url = HttpContext.Current.Request.Url.PathAndQuery.ToString();
        int tag = url.LastIndexOf("/") + 1;
        int mm = url.IndexOf(".aspx") - url.LastIndexOf("/") - 1;
        string urlName = url.Substring(tag, mm + 5);
        return urlName;
    }
}