<%@ WebHandler Language="C#" Class="HandlerChart" %>

using System;
using System.Web;
using UTDtBaseSvr;
using AppCode;
using System.Collections;
using System.Collections.Generic;
using System.Web.SessionState;
using System.Text;
using UserRightObj;
using UTDtCnvrt;
using System.Data;
using System.Web.Script.Serialization;
using System.Globalization; 

public class HandlerChart : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {

        HttpRequest request = context.Request;
        HttpResponse response = context.Response;
        context.Response.ContentType = "text/plain";
        string rendername = "";
        rendername = request.Params["rendername"];
        string sid = request.Params["sid"];
        string charttype = "";
        if (request.Params["charttype"]!=null)
        {
            charttype = request.Params["charttype"];
        }
        
        string sPara = request.QueryString.ToString();
        string rs = getChartData(sid, sPara, charttype, rendername);
        context.Response.Write(rs);
    }

    public string getChartData(string sid,String sPara,string charttype,string rendername)
    {
        string rs = "[]";
 
       
        BusinessCall bcCall = (BusinessCall)HttpContext.Current.Session["bcCall"];
        Dictionary<string, object>  options = LayoutUI.getLayout(bcCall.bcLayoutList, sid);
        string componenttype = bcCall.businesstype;
        string chartype = "";
        ChartProperty CP = new ChartProperty();
        Dictionary<string, Object> extendparam = new Dictionary<string, object>();
        if (bcCall == null) return null;
        foreach (BusinessComponentCall bcc in bcCall.bComponentList)
        {
            if (bcc.id.Equals(sid))
            {
                componenttype = bcc.name;
                chartype= bcc.type;
                extendparam = LayoutUI.getParam(bcc.extendparam, ';', '=');
                if (extendparam.ContainsKey("chartproperty"))
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    CP = (ChartProperty)js.Deserialize(extendparam["chartproperty"].ToString(), CP.GetType());
                    if (CP != null)
                    {
                        
                    }
                    break;
                }
                break;
            }
        }
        UTDtBusiness.BsDataTable dtData = getDataTable(bcCall.BussinessName, componenttype, sPara, "");//;ibegin=0;iend=16;
        if (dtData == null||dtData.dt.Rows.Count==0)
            return rs;

        switch ((HtmlComponetType)Enum.Parse(typeof(HtmlComponetType), chartype, true))
        {
           case HtmlComponetType.curvedlinechart:
                if (charttype == "" || charttype == "line")
                {
                    rs = Highcharts.getOptions(dtData.dt, options, new AppCode.SplineSeries(CP, dtData.dt.Rows[0], dtData.ltColumnInfo,rendername), CP, dtData.ltColumnInfo);
                }
                else if (charttype == "column")
                {
                    rs = Highcharts.getColumnOptions(dtData.dt);
                }
                else if (charttype == "pie")
                {
                    rs = Highcharts.getPieOptions(dtData.dt);
                }
               break;
            default:break;
        }
    

        return rs;
    
    }
    
    private  UTDtBusiness.BsDataTable getDataTable(string businesstype, string componenttype, string sPara, string extPara)
    {
        String strPara = "";
        if (sPara != "")
        {
            strPara = sPara.Replace('&', ';').Trim();
            if (strPara[0].Equals(';'))
            {
                strPara = strPara.Substring(1);
            }
        }

        strPara += extPara;

        string appname = ((Loginresult)HttpContext.Current.Session["Session"]).AppName;
        string rolename = ((Loginresult)HttpContext.Current.Session["Session"]).RoleName;

       
        
        object[] args = new object[] { "BusinessName=" + businesstype, "ComponentName=" + componenttype, strPara, componenttype + ".appname=" + appname };

        UTDtBusiness.BsDataTable dtData = (UTDtBusiness.BsDataTable)AppCode.WSUtil.getFromXMLWS(EnumServiceFlag.businessservice, "bstabdat", appname, args);


        return dtData;
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}