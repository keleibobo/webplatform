using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Services;
using System.Web.Services;
using AppCode;
using UserRightObj;
using UTUtil;
using System.Configuration;
using Microsoft.AspNet.SignalR;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;


//using System.Runtime.Serialization;
//using System.ServiceModel;
//using System.Reflection;
//using System.IO;
//using System.Text;
/// <summary>
///WebService 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.ComponentModel.ToolboxItem(false)]
//若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。 
[System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService
{
    public WebService()
    {
        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World, 2012";    
    }

    public String EncryptCode(String message)
    {
        string cl = message;
        string pwd = "";
        MD5 md5 = MD5.Create();//实例化一个md5对像  
        // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　  
        byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
        // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得  
        for (int i = 0; i < s.Length; i++)
        {
            // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符   
            pwd = pwd + s[i].ToString("X");
        }
        return pwd;
    }

    [WebMethod]
    public void pushdata(string a, string b, string c, string d)
    {
        d = HttpUtility.UrlDecode(d);
        if (c == "pushevent")
        {
            IHubContext IHEvent = GlobalHost.ConnectionManager.GetHubContext<EventHub>();
            IHEvent.Clients.All.pushEvent(d);
           // IHubConnectionContext _clients = GlobalHost.ConnectionManager.GetHubContext<EventHub>().Clients;
           // _clients.All.pushEvent(d);
        }
        else if (c == "updatesvg")
        {
            Dictionary<String, String> config = GetConfig(d);
          //  IHubConnectionContext _clients = GlobalHost.ConnectionManager.GetHubContext<SvgHub>().Clients;
            IHubContext IHSvg = GlobalHost.ConnectionManager.GetHubContext<SvgHub>();
            IHSvg.Clients.All.UpdateStatus(config["svgid"], config["shapeguid"], config["svgshapename"], config["svalue"], config["mappingtabletype"]);

           // _clients.All.UpdateStatus(config["svgid"], config["shapeguid"], config["svgshapename"], config["svalue"], config["mappingtabletype"]);    
        }
        else if (c == "confirmevent")
        {
            IHubContext IHEvent = GlobalHost.ConnectionManager.GetHubContext<EventHub>();
            IHEvent.Clients.All.confirmEvent(d);
            //IHubConnectionContext _clients = GlobalHost.ConnectionManager.GetHubContext<EventHub>().Clients;
            //_clients.All.confirmEvent(d);
        }
    }

    public  Dictionary<String, String> GetConfig(string d)
    {
        Dictionary<String, String> rt = new Dictionary<String, String>();
        if (d != null && d.Length > 0)
        {
            string[] aParam = d.Split(';');
            foreach (string s in aParam)
            {

                if (s.Trim().Length > 0)
                {
                    string[] aS = s.Split('=');

                    if (aS[0].ToLower() == "appname")
                    {
                        rt.Add(aS[0].ToLower(CultureInfo.CurrentCulture), s.Replace(aS[0] + "=", "").ToLower());
                    }
                    else
                    {
                        rt.Add(aS[0].ToLower(CultureInfo.CurrentCulture), s.Replace(aS[0] + "=", ""));
                    }
                }
            }
        }
        return rt;
    }

    //[WebMethod]
    //public void PushEvent(string eventstr)
    //{
    //    IHubConnectionContext _clients = GlobalHost.ConnectionManager.GetHubContext<EventHub>().Clients;
    //    _clients.All.displayMessage(eventstr);
    //}

    [WebMethod]
    public string UpdateConfig(string a,string b,string c,string d)
    {
        d = HttpUtility.UrlDecode(d);
        string rt = "false";   
        if (c.Equals("checkuser"))
        {
            if (a != null && b != null)
            {
                string pwd = EncryptCode("ut2012");
                if (a.Equals("admin") && b.Equals(pwd))
                {
                    rt = "";
                }
                else
                {
                    rt = "用户名和密码错误";
                }
            }
        }
        else if (c.Equals("appname"))
        {
            rt = ReadConfig.TheReadConfig["appname"];       
        }
        else if (c.Equals("UpdateConfig",StringComparison.OrdinalIgnoreCase))
        {
            rt = "用户名和密码错误";
            if (a != null && b != null)
            {
                string pwd = EncryptCode("ut2012");
                if (a.Equals("admin") && b.Equals(pwd))
                {
                    Dictionary<string, string> dt = new Dictionary<string, string>();
                    UtilFunc.GetParaToDictory(d, dt, ";", true);
                    foreach(KeyValuePair<string ,string> kv in dt)
                    {
                        ReadConfig.TheReadConfig[ kv.Key]=kv.Value;                        
                    }
                    try
                    {
                        AppCode.ServiceConfig.InitialParam();
                        InitModel.Refresh();
                        rt = "";
                    }
                    catch (System.Exception ex)
                    {
                        rt = ex.Message;
                    }
                   
                }               
            }            
        }
        else if (c.Equals("refresh"))
        {
            try
            {
                Dictionary<string, string> dtconfig = new Dictionary<string, string>();
                UtilFunc.GetParaToDictory(d, dtconfig, ";", false);
                foreach (KeyValuePair<string, string> kv in dtconfig)
                {
                    ReadConfig.TheReadConfig[kv.Key] = kv.Value;
                }
                try
                {
                    AppCode.ServiceConfig.InitialParam();
                    if (dtconfig.ContainsKey("basesservice") || dtconfig.ContainsKey("userrightservice"))
                    {
                        Thread.Sleep(3000);
                        InitModel.Refresh();
                    }                    
                    rt = "";
                }
                catch (System.Exception ex)
                {
                    rt = ex.Message;
                }                
                rt = "";
            }
            catch (Exception ex)
            {
                rt = "初始化失败";
            }
        }
        else if (c.Equals("timeadjust", StringComparison.CurrentCultureIgnoreCase))
        {
            Dictionary<string, string> dtconfig = new Dictionary<string, string>();
            UtilFunc.GetParaToDictory(d, dtconfig, ";", false);
            string stime = "";
            if (dtconfig.TryGetValue("timevalue", out stime))
            {
                if (Regex.IsMatch(stime, @"^\d{17}$"))
                {
                    try
                    {
                        DateTime changetime = DateTime.ParseExact(stime, "yyyyMMddHHmmssfff", System.Globalization.CultureInfo.CurrentCulture);
                        SystemTime st = UTSetLocalTime.ConvertDatetime(changetime);
                        UTUtil.UTSetLocalTime.SetSysTime(st);
                    }
                    catch (System.Exception ex)
                    {
                    }
                }
            }
        }
        else if (c.Equals("heartbeat"))
        {
            rt = "";
        }
        return rt;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public string GetDayLoadData()
    {
        //return "Day Load Data, 2012";
        string strRt = "";
        //         AppCode.DayLoadAnayliseContentBusiness bcb = new AppCode.DayLoadAnayliseContentBusiness();
        //         System.Data.DataView dv = bcb.getDatas(null);
        //         strRt = AppCode.DataViewToJson.DataToJson(dv);
        return strRt;
    }

    [WebMethod]
    public void UploadFile(Stream stream)
    {

        byte[] NameLength = new byte[4];
        stream.Read(NameLength, 0, 4);

        int Length = BitConverter.ToInt32(NameLength, 0);
        byte[] NameByte = new byte[Length];
        stream.Read(NameByte, 0, Length);

        string paraminfo = System.Text.Encoding.Default.GetString(NameByte);
        string uploadFolder = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "uploadfiles");
        uploadFolder = Path.Combine(uploadFolder, System.DateTime.Now.ToString("yyyy-MM-dd"));
        Dictionary<string, string> dt = new Dictionary<string, string>();
        UtilFunc.GetParaToDictory(paraminfo, dt, ";", true);

        if (!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }
        
    }
}