<%@ WebHandler Language="C#" Class="GetStreamUserData" %>

using System;
using System.Web;
using AppCode;
using System.Collections.Generic;
using System.Web.SessionState;
using UserRightObj;
using System.Web.Script.Serialization;

public class GetStreamUserData : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        ServiceInfo Usi = ServiceConfig.GetInfo(UTDtCnvrt.EnumServiceFlag.userrightservice);
        string appname = ReadConfig.TheReadConfig["appname"].ToLower();
        string password = HttpContext.Current.Session["password"].ToString();
        string username = HttpContext.Current.Session["username"].ToString();
        string url = string.Format("http://{0}/GetRDDataJson?a={1}&b={2}&c=getuserbusinessparam&d=appname={3};BusinessName=SPRealTimeJK;username={1};", Usi.url, username, password, appname);
        string msg = "";
        List<UTDtCnvrt.MRDDataAll> ltData = (List<UTDtCnvrt.MRDDataAll>)UTUtil.WebServiceUtil.GetObjectFromWebService(url, typeof(List<UTDtCnvrt.MRDDataAll>), out msg);
        List<BusinessUserParam> rd = new List<BusinessUserParam>();
        if (ltData != null && ltData.Count > 0)
        {
            List<Object> ol = UTDtCnvrt.DataReflect.FromContractData(ltData, typeof(BusinessUserParam));
            for (int i = 0; i < ol.Count; i++)
            {
                rd.Add((BusinessUserParam)ol[i]);
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            context.Response.Write(js.Serialize(rd));
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