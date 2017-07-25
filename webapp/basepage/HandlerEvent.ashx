<%@ WebHandler Language="C#" Class="HandlerEvent" %>

using System;
using System.Web;
using UserRightObj;
using UTDtBaseSvr;
using UTDtCnvrt;
using AppCode;
using System.Collections.Generic;
using System.Web.SessionState;

public class HandlerEvent : IHttpHandler, IRequiresSessionState   //获取未确认的告警事件
{
    
    public void ProcessRequest (HttpContext context) {
        HttpRequest request = context.Request;
        HttpResponse response = context.Response;
        context.Response.ContentType = "text/plain";

     //   string sid = request.Params["componentid"];

        string rs = getHtml();
        context.Response.Write(rs);
    }

    public String getHtml()//String ctrlIdDesc, 
    {
        string rs = "";
        // BusinessCall bccCall = (BusinessCall)HttpContext.Current.Session["bcCall"];
        string componenttype = "";
        string businesstype = "";

        string appname = ((Loginresult)HttpContext.Current.Session["Session"]).AppName;
        string rolename = ((Loginresult)HttpContext.Current.Session["Session"]).RoleName;
        ;
        BusinessCall bccCall = InitModel.getBusinessCallByType(appname, EnumBusinessType.alarmeventpagetype.ToString());
      //  businesstype = bccCall.BussinessName;
        if (bccCall != null)
        {
            //  string componenttype = "";
            businesstype = bccCall.BussinessName;

            foreach (BusinessComponentCall bcc in bccCall.bComponentList)
            {
                if (bcc.type.Equals(HtmlComponetType.alarmevent.ToString()))
                {
                    componenttype = bcc.name;
                    break;
                }
            }

        }
        //else
        //{
        //    return "";
        //}
     
       
       

        object[] args = new object[] { "BusinessName=" + businesstype, "ComponentName=" + componenttype, componenttype + ".appname=" + appname };
        ///参数不对
        List<object> dtData = AppCode.WSUtil.getloFromWS(EnumServiceFlag.businessservice, "bstabdat", appname, rolename, args);
        List<webcomm_Event> evlist = new List<webcomm_Event>();
        if (dtData == null) return "";
        string json = "{\"total\":\"" + dtData.Count + "\",\"event\":[";
        for(int i=0;i<dtData.Count;i++)
        {
            if(i!=dtData.Count-1)
            {
                json += "{\"dt\":\"" + ((webcomm_Event)dtData[i]).dt + "\",\"alarmtype\":\"" + ((webcomm_Event)dtData[i]).alarmtype + "\",\"eventtype\":\"" + ((webcomm_Event)dtData[i]).eventtype + "\",\"alarmlevel\":\"" + ((webcomm_Event)dtData[i]).alarmlevel + "\",\"desc\":\"" + ((webcomm_Event)dtData[i]).desc + "\"},";
            }
            else
            {
                json += "{\"dt\":\"" + ((webcomm_Event)dtData[i]).dt + "\",\"alarmtype\":\"" + ((webcomm_Event)dtData[i]).alarmtype + "\",\"eventtype\":\"" + ((webcomm_Event)dtData[i]).eventtype + "\",\"alarmlevel\":\"" + ((webcomm_Event)dtData[i]).alarmlevel + "\",\"desc\":\"" + ((webcomm_Event)dtData[i]).desc + "\"}";
            }
        }
        json += "]}";
        json = json.Replace("\\", "\\\\");
        return json;


    }


    public bool IsReusable
    {
        get
        {
            return false;
        }
    }


}