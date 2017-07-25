<%@ WebHandler Language="C#" Class="HandlerDialog" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Text;

public class HandlerDialog : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        

        HttpRequest request = context.Request;
        HttpResponse response = context.Response;
        context.Response.ContentType = "text/plain";

        string script = request.Params["script"];
        string sid = request.Params["sid"];
     //   string args = request.QueryString.ToString().Substring(request.QueryString.ToString().IndexOf("&args=") + "&args=".Length);

        // string sPara = request.QueryString.ToString();
        string rs = handler(script,sid);
        context.Response.Write(rs);
    }

    public string handler(String script,string sid)
    {
        StringBuilder rs = new StringBuilder();
        String style = ReadConfig.TheReadConfig["style"];
      rs.Append(String.Format(@"<table width='160' height='210'  border='0' cellpadding='0' cellspacing='0'  bgcolor='#FFFFFF'>
      <tr> <td align='left' valign='top'><table width='100%' height='240'  border='1' cellspacing='0' cellpadding='1' bordercolorlight='#97B9D7' bordercolordark='#ffffff' bgcolor='#FFFFFF' class='TextBorder01'>
    <tr align='center'> <td height='32' align='left' class='t2'>
     <table width='100%' height='26' border='0' cellpadding='0' cellspacing='0'> <tr> <td width='80%' class='tdtitle15'>&nbsp;&nbsp;选项</td><td width='5%'>&nbsp;</td><td width='15%' align='center' onclick='fnCloseWindows();'><img src='../images/{0}/关闭.gif' alt='#' width='24' height='24'></td></tr> </table>
      </td></tr><tr> <td align='center' valign='top'>",style));
    string[] ps = script.Split('￥');
    int it = ps.Length;
    while (it-- > 0) {
       string p = ps[it];
        string[] q = p.Split('=');
        string p1 = q[1];
        string p2 = q[0];


        if (p != "") {

            String click = String.Format(" onclick=fnCallByDlg('{0}','{1}') ",sid,p2);
                
             rs.Append(String.Format(@"   <table width='100%' height='30' border='0' cellpadding='0' cellspacing='0'>
            <tr> <td width='10%'></td><td width='85'>
            <table width='68%' height='22' border='0' cellpadding='0' cellspacing='0'> <tr><td width='10%' align='right'><img src='../images/{2}/button3_l.png' width='8' height='24'></td><td width='80%' align='center' background='../images/{2}/button3_m.png' {0}>{1}</td><td width='10%'><img src='../images/{2}/button3_r.png' width='8' height='24'></td></tr></table>
             </td> <td width='5%'></td></tr></table>",click,p1,style));

        }
    }


        return rs.ToString();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}