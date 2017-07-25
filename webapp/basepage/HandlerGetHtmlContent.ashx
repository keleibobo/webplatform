<%@ WebHandler Language="C#" Class="HandlerGetHtmlContent" %>

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using UTDtBaseSvr;
using UTDtCnvrt;

public class HandlerGetHtmlContent : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        string param = context.Request.Params["params"];
        string businessName = HttpContext.Current.Session["type"].ToString();
        string sid = context.Request.Params["sid"];
        string appname = ReadConfig.TheReadConfig["appname"];
        BusinessCall bcCal = (BusinessCall)HttpContext.Current.Session["bcCall"];
        string componentName = "";
        foreach (BusinessComponentCall bcc in bcCal.bComponentList)
        {
            if (bcc.id == sid)
            {
                componentName = bcc.name;
                break;
            }
        }
        object[] args = new object[] { "BusinessName=" + businessName, "ComponentName=" + componentName, param ,"appname="+appname};
        List<UTDtCnvrt.MRDDataAll> ListMRDD = AppCode.WSUtil.getGridDataFromWS(EnumServiceFlag.businessservice, "bsextern", "", "", args);
        if (ListMRDD != null && ListMRDD.Count > 1)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            htmlreport htmlReport = (htmlreport)jsSerializer.Deserialize(ListMRDD[1].v, typeof(htmlreport));
            if (string.IsNullOrEmpty(htmlReport.content))
            {
                context.Response.Write("<HTML><HEAD><TITLE></TITLE></HEAD><BODY style='text-align:center;'><h1 style='margin-top:20px;'>未找到该报表</h1></BODY></HTML>");
            }
            else
            {
                context.Response.Write(htmlReport.content); 
            }
            
        }
        else
        {
            context.Response.Write("<HTML><HEAD><TITLE></TITLE></HEAD><BODY style='text-align:center;'><h1 style='margin-top:20px;'>未找到该报表</h1></BODY></HTML>");
        }
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    public class htmlreport
    {
        public string url;

        public string content;
    }
}