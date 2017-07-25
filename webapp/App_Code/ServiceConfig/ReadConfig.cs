using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.InteropServices;
using System.Text;


/// <summary>
///ReadConfig 的摘要说明
/// </summary>
public class ReadConfig
{
    [DllImport("kernel32")]
    public static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
    [DllImport("kernel32")]
    public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
    [DllImport("kernel32.dll")]
    public static extern int GetPrivateProfileInt(string lpAppName, string lpKeyName, int nDefault, string lpFileName);
	public ReadConfig()
	{
        System.Web.HttpServerUtility server = System.Web.HttpContext.Current.Server;
        tmpRootDir = server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath.ToString());//获取程序根目录
	}
    string tmpRootDir = "";
    public static ReadConfig  TheReadConfig = new ReadConfig();
    public  string  this[string key]
    {
        get
        {
            StringBuilder strBuilder1 = new StringBuilder("", 100);
            GetPrivateProfileString("appconfig", key, "", strBuilder1, 100, tmpRootDir + "\\config.ini");
            return strBuilder1.ToString();
        }
        set
        {
            WritePrivateProfileString("appconfig", key, value, tmpRootDir + "\\config.ini");
        }
    } 
}