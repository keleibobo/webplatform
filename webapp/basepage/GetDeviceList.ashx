<%@ WebHandler Language="C#" Class="GetDeviceList" %>

using System;
using System.Web;
using AppCode;
using System.Collections.Generic;
using System.Web.SessionState;

public class GetDeviceList : IHttpHandler, IRequiresSessionState{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        ServiceInfo bsi = ServiceConfig.GetInfo(UTDtCnvrt.EnumServiceFlag.businessservice);
        string appname = ReadConfig.TheReadConfig["appname"].ToLower();
        string password = HttpContext.Current.Session["password"].ToString();
        string username = HttpContext.Current.Session["username"].ToString();
        string url = string.Format("http://{0}/GetRDDataJson?a={1}&b={2}&c=bsextern&d=appname={3};BusinessName=DataThrough;ComponentName=StreamInfo;", bsi.url,username,password, appname);
        string msg = "";
        List<UTDtCnvrt.MRDDataAll> ltData = (List<UTDtCnvrt.MRDDataAll>)UTUtil.WebServiceUtil.GetObjectFromWebService(url, typeof(List<UTDtCnvrt.MRDDataAll>), out msg);
        if (ltData!=null && ltData.Count > 1)
        {
            context.Response.Write(ltData[1].v);
        }
        else
        {
            context.Response.Write("[]");
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}