<%@ WebHandler Language="C#" Class="RealTimeData" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UTDtBaseSvr;
using AppCode;
using System.Web.Services;
using System.Text;
using System.IO;
using UserRightObj;
using UTDtCnvrt;
using System.Reflection;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;

public class RealTimeData : IHttpHandler, IRequiresSessionState   //获取未确认的告警事件
{
    public string BusinessType = "";
    public static string arg = "";//之前url的参赛，传递给table组件用于查询？
    static Dictionary<string, long> totalpage = new Dictionary<string, long>();
    static BusinessCall bcCall;

    public string appname = "";
    public string rolename = "";
    public string username = "";
    public string layoutcells;
    public string style = ReadConfig.TheReadConfig["style"];
    public void ProcessRequest(HttpContext context)
    {
        HttpRequest request = context.Request;
        HttpResponse response = context.Response;
        context.Response.ContentType = "text/plain";
        bcCall = (BusinessCall)HttpContext.Current.Session["bcCall"];
        string ctrlIdDesc = request.Params["ctrlIdDesc"];
        string currentpage = request.Params["currentpage"];
        string sPara = request.Params["sPara"];
        string totalnum = request.Params["totalnum"];
        string rs = getTable(ctrlIdDesc, currentpage, sPara, totalnum);
        context.Response.Write(rs);
    }

    public String getTable(String ctrlIdDesc, String currentpage, String sPara, String totalnum)
    {
        string rs = "";
        string sid = ctrlIdDesc.Split('_')[1];
        int ipagesize = Convert.ToInt32(ReadConfig.TheReadConfig["pagesize"]);
        long total = ipagesize;
        if (totalnum!="" && totalnum!="undefined")
        {
            total = Convert.ToInt32(totalnum);
        }
        if (List.pagesizes.ContainsKey(sid))
        {
            ipagesize = List.pagesizes[sid];
        }
        string BusinessType = HttpContext.Current.Session["type"].ToString();
        string componenttype = BusinessType;
        if (bcCall == null) return rs;
        foreach (BusinessComponentCall bcc in bcCall.bComponentList)
        {
            if (bcc.id.Equals(sid))
            {
                componenttype = bcc.name;
                break;
            }
        }
        int iCurrentPage = Convert.ToInt32(currentpage);
        long ibegin = (iCurrentPage - 1) * ipagesize;
        long iend = iCurrentPage * ipagesize - 1;
        if (iend >= total && total >= ipagesize) iend = total - 1;
        String strPara = "";
        if (sPara != "")
        {
            strPara = sPara.Replace('&', ';').Trim();
            if (strPara[0].Equals(';'))
            {
                strPara = strPara.Substring(1);
            }


        }
        strPara += ";ibegin=" + ibegin;
        strPara += ";iend=" + iend;
        strPara += ";" + arg;
        arg = "";
        if (strPara.Equals(""))
            return rs;
        appname = ((Loginresult)HttpContext.Current.Session["Session"]).AppName;
        object[] args = new object[] { "BusinessName=" + BusinessType, "ComponentName=" + componenttype, strPara, componenttype + ".appname=" + appname };
        UTDtBusiness.BsDataTable dtData = (UTDtBusiness.BsDataTable)AppCode.WSUtil.getFromXMLWS(EnumServiceFlag.businessservice, "bstabdat", appname, args);
        if (dtData != null)
        {
                if (ibegin == 0)
                {
                    total = dtData.total;
                }
                rs = GetJson(dtData.dt);
        }
        return rs;
    }
    
    public string GetJson(DataTable dt)
    {
        string result = "[";
        bool first = true;
        foreach( DataRow dr in dt.Rows )
        {            
            if(first)
            {
                if(dt.Columns.Contains("状态"))
                {
                    result += "{\"pointname\":\"" + dr["描述"].ToString() + "\",\"value\":\"" + dr["值"].ToString() + "\",\"status\":\"" + dr["状态"].ToString() + "\""; 
                }
                else
                {
                    result += "{\"pointname\":\"" + dr["描述"].ToString() + "\",\"value\":\"" + dr["值"].ToString() + "\"";
                }
                if (dt.Columns.Contains("人工置数"))
                {
                    result += ",\"manual\":\"" + dr["人工置数"].ToString() + "\"";
                }
                if (dt.Columns.Contains("刷新"))
                {
                    result += ",\"refresh\":\"" + dr["刷新"].ToString() + "\"";
                }
                result += "}";
                first = false; 
            }
            else
            {
                if (dt.Columns.Contains("状态"))
                {
                    result += ",{\"pointname\":\"" + dr["描述"].ToString() + "\",\"value\":\"" + dr["值"].ToString() + "\",\"status\":\"" + dr["状态"].ToString() + "\"";
                }
                else
                {
                    result += ",{\"pointname\":\"" + dr["描述"].ToString() + "\",\"value\":\"" + dr["值"].ToString() +
                                  "\"";
                }
                if (dt.Columns.Contains("人工置数"))
                {
                    result += ",\"manual\":\"" + dr["人工置数"].ToString() + "\"";
                }
                if (dt.Columns.Contains("刷新"))
                {
                    result += ",\"refresh\":\"" + dr["刷新"].ToString() + "\"";
                }
                result += "}";
            }
        }
        result += "]";
        return result;
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}

