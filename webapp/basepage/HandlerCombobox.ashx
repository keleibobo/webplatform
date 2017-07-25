<%@ WebHandler Language="C#" Class="HandlerCombobox" %>

using System;
using System.Web;
using UTDtBaseSvr;
using AppCode;
using System.Collections;
using System.Collections.Generic;
using System.Web.SessionState;
using System.Text;

public class HandlerCombobox : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
       

        HttpRequest request = context.Request;
        HttpResponse response = context.Response;
        context.Response.ContentType = "text/plain";

        string ConditionName = request.Params["ConditionName"];

        string rs = getConditions(ConditionName, request.QueryString.ToString());
        response.Write(rs);
        response.End();
    }


    public  string getConditions(string ConditionName,String args)
    {
        string rs = "[]";
        String param = args.Replace("&",";");
        String appname = HttpContext.Current.Session["appname"].ToString();
        string roles = HttpContext.Current.Session["RoleType"].ToString();
        string pwd = HttpContext.Current.Session["password"].ToString();
        string username = HttpContext.Current.Session["username"].ToString();
        BusinessCall bcCall = (BusinessCall)HttpContext.Current.Session["bcCall"];
        string ptype = bcCall.BussinessName;
        ServiceInfo si = AppCode.ServiceConfig.GetInfo(UTDtCnvrt.EnumServiceFlag.businessservice);
        
        //string url = String.Format("http://{0}/GetRDDataJson?a=1&b=1&c=bscdtdat&d=rolename={1};BusinessName={2};AppName={3};ConditionName={4}",
        //    si.url, roles, ptype, appname, ConditionName);

        string url = String.Format("http://{0}/SetRDDataJson?a={5}&b={6}&c=bscdtdat&d=rolename={1};BusinessName={2};AppName={3};{4}",
           si.url, roles, ptype, appname, param,username,pwd);

        List<object> bccList = AppCode.WSUtil.getListFromPostWS(url, typeof(BusinessConditionCall));
        if (bccList != null && bccList.Count > 0)
        {
            rs = GetControl_ComboxContent((BusinessConditionCall)bccList[0]);
        }
        ///获取下拉等数据 
        return rs;
    }

    public string GetControl_ComboxContent(BusinessConditionCall cd)
    {
        StringBuilder sbtemp = new StringBuilder();
        sbtemp.Append("[");
        int index = 0;
        foreach (UTDtBaseSvr.Pair p in cd.AllItemName)
        {
            UTDtBaseSvr.Single bs = new UTDtBaseSvr.Single();
            bs.Value = p.Key;
            if (cd.DisplayEnable.Contains(bs))
            {
                string select="";
                if (cd.DisplaySelect.Contains(bs)||index==0)
                {
                   // sbtemp.Append(string.Format("<option value='{0}' selected =selected>{1}</option>", p.Key, p.Value));
                    select = ",\"selected\":true"; //默认选中项 未测
                    index++;
                }
                else
                {
                  //    sbtemp.Append(string.Format("{'id'='{0}'>{1}</option>", p.Key, p.Value));
                }
              
                sbtemp.Append(string.Format("{{\"id\":\"{0}\",\"text\":\"{1}\"{2}}},", p.Key, p.Value,select));
              
            }
        }

        sbtemp.Remove(sbtemp.Length-1,1);
        sbtemp.Append("]");
        return sbtemp.ToString();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}