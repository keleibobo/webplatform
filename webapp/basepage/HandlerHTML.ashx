<%@ WebHandler Language="C#" Class="HandlerHTML" %>

using System;
using System.Web;
using AppCode;
using UserRightObj;
using UTDtBaseSvr;
using UTDtCnvrt;
using System.Web.SessionState;

public class HandlerHTML : IHttpHandler, IRequiresSessionState
{
    string appname = "";
    string businesstype = "";
    string cid = "";
    string componentid = "";
    int paramlength;
    BusinessCall bcCall;
    
    public void ProcessRequest (HttpContext context) {
        HttpRequest request = context.Request;
        HttpResponse response = context.Response;

        context.Response.ContentType = "text/html";

      //  appname = request.Params["appname"];
      //  businesstype = request.Params["businesstype"];
        appname = HttpContext.Current.Session["appname"].ToString();
        businesstype = HttpContext.Current.Session["type"].ToString();

        cid = request.Params["cid"];
        componentid = request.Params["componentid"];
        paramlength = HttpContext.Current.Session[componentid].ToString().Split(';').Length;

      //  string listId = request.Params["listId"];
      //  string currentId = request.Params["HISTORYID"].Split(';')[0];
        string Index = System.Web.HttpUtility.UrlDecode(request.QueryString.ToString().Substring(request.QueryString.ToString().IndexOf("&rowindex=") + "&rowindex=".Length));
        
        bcCall = InitModel.GetBusinessCall(appname, businesstype);

        string rs = getHtml(Convert.ToInt32(Index));
        int b = rs.IndexOf("--&gt;\n</style>");

        string scroll = @" html
        {
            scrollbar-shadow-color: #ffffff;

scrollbar-highlight-color: #ffffff;

scrollbar-3dlight-color: #3EC4CC;

scrollbar-darkshadow-color: #3EC4CC;

scrollbar-track-color: #79CDD9;

scrollbar-arrow-color: #008989;

   scrollbar-base-color: #BBE1EA;} 
 table{margin:0 auto;}";
    //    rs.Insert(b,scroll);
        if(b>-1)
        rs = rs.Substring(0, b) + scroll + rs.Substring(b);

        string doctype = "<!DOCTYPE html PUBLIC \" -//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";
        if (rs.IndexOf("<!DOCTYPE") == -1)
        {
            rs = doctype + rs;
        }
        rs += "<script type=\"text/javascript\" src=\"report.js\"></script>";
        if (cid != null)
        {
            rs += "<script>setTimeout(function(){$(\"body\").append(\"<input type='hidden' id='cId' value='" + cid + "' /> \");},1);</script>";
        }
        
        rs += "<script>setTimeout(function(){$(\"body\").append(\"<input type='hidden' id='listParam' value='" + paramlength + "' /> \");},1);</script>";
        
        
        rs += "<script>setTimeout(function(){$(\"body\").append(\"<input type='hidden' id='currentId' value='" + Index + "' /> \");},1);</script>";
        rs += "<script>setTimeout(function(){$(\"body\").append(\"<input type='hidden' id='componentId' value='" + componentid + "' /> \");},1);</script>";
        rs += "<script>setTimeout(function(){$(\"#tpagecount\").text(pageCount);},1);</script>";
        
        context.Response.Write(rs);
        response.End();
    }

    public String getHtml(int Index)//String ctrlIdDesc, 
    {
        string rs = "";
        string sPara = "";

        if (paramlength >= Index)
        {
            sPara = HttpContext.Current.Session[componentid].ToString().Split(';')[Index - 1];
        }
        String strPara = "";
        if (sPara != ""&&sPara!=null)
        {
            strPara = sPara.Replace('&', ';').Trim();
            if (strPara[0].Equals(';'))
            {
                strPara = strPara.Substring(1);
            }
        }

        string componenttype = "";
        if (bcCall == null) return null;

        string type="";
        foreach (BusinessComponentCall bcc in bcCall.bComponentList)
        {
            if (bcc.id.Equals(cid))
            {
                type = bcc.type;
                break;
            }
        }

        foreach (BusinessComponentCall bcc in bcCall.bComponentList)
        {
            if (bcc.type.Equals(type))
            {
                componenttype = bcc.name;
                break;
            }
        }
        string rolename = ((Loginresult)HttpContext.Current.Session["Session"]).RoleName;
        
        object[] args = new object[] { "BusinessName=" + businesstype, "ComponentName=" + componenttype,HttpUtility.UrlEncode(strPara), componenttype + ".appname=" + appname };

        Object dtData = AppCode.WSUtil.getFromWS(EnumServiceFlag.businessservice, "bstabdat", appname, rolename, args);
        if (dtData != null)
        {
            rs = dtData.ToString();
        }
        return rs;


    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}