<%@ WebHandler Language="C#" Class="HandlerSVG" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Text;
using System.Net;
using System.IO;
using System.Web.Services;
using System.Xml;
using AppCode;
using System.Security.Cryptography;
using System.Reflection;
using System.Data;
using UTDtCnvrt;
using UTDtBaseSvr;
using System.Collections.Generic;
using System.Linq;
using UTUtil;
using UserRightObj;
using System.Text.RegularExpressions;

public class HandlerSVG : IHttpHandler, IRequiresSessionState
{
    private ServiceInfo si = AppCode.ServiceConfig.GetInfo(UTDtCnvrt.EnumServiceFlag.svgsvrservice);
    public void ProcessRequest (HttpContext context) {
        HttpRequest request = context.Request;
        HttpResponse response = context.Response;
        context.Response.ContentType = "text/plain";


        string[] par =request.QueryString.AllKeys;

        string host = "";
        if (request.QueryString["type"] != null && request.QueryString["type"] == "control")
        {

            host = setControlUrl(par, request);
        }
        else if (request.QueryString["type"] != null && request.QueryString["type"] == "download")//下载
        {
             download(request); //未启用
             host = setUrl(par, request);
        }
        else
        {
           // update(request);
            host = setUrl(par, request);
        }
        string result = "";
        if (host != "")
        {
            result = getURLResult(host);
        }

       
    
        context.Response.Write(result);
    }

    public String download(HttpRequest request)
    {
        String rs = "";
        ServiceInfo bsi = AppCode.ServiceConfig.GetInfo(UTDtCnvrt.EnumServiceFlag.svgsvrservice);
        string password = HttpContext.Current.Session["password"].ToString();
        string username = HttpContext.Current.Session["username"].ToString();
        string appname = HttpContext.Current.Session["appname"].ToString();
        
        // fid={1}&fname={2}&appname={3}&contenttype=
        String fid = request["fid"];
        string fname=request["fname"];
        String ctype = request["contenttype"];

        string host = string.Format("http://{0}/GetRDDataJson?a={1}&b={2}&c=getsvgnode&d=appname={3};nodeid={4};fname={5};contenttype={6}", bsi.url, username, password, appname, fid, fname,ctype);

        return rs;
    }

    
    /// <summary>
    /// 未启用
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public String update(HttpRequest request)
    {
        ServiceInfo bsi = AppCode.ServiceConfig.GetInfo(UTDtCnvrt.EnumServiceFlag.svgsvrservice);
        string password = HttpContext.Current.Session["password"].ToString();
        string username = HttpContext.Current.Session["username"].ToString();
        string appname = HttpContext.Current.Session["appname"].ToString();
        String nodeid = request["nodeid"];
        String lasttime=request["lasttime"];

        string host = string.Format("http://{0}/GetRDDataJson?a={1}&b={2}&c=GetDatas&d=appname={3};nodeid={4};lasttime={5}", bsi.url, username, password, appname,nodeid,lasttime);

        return host;
    
    }

    public string setUrl(string[] par, HttpRequest req)
    {
        string password = HttpContext.Current.Session["password"].ToString();
        string username = HttpContext.Current.Session["username"].ToString();
        string appname = HttpContext.Current.Session["appname"].ToString();
        string host = string.Format("http://{0}/" + req.QueryString[par[0]] + "?username={1}&pwd={2}&appname={3}&", si.url, username, password,appname);


        for (int k = 0; k < par.Length; k++)
        {
            if (par[k] == "_")
                par = Remove(par, k);
        }

        for (int i = 1; i < par.Length; i++)
        {
            if (par[i] != "_")
                if (i != par.Length - 1)
                {
                    host += par[i] + "=" + req.QueryString[par[i]] + "&";
                }
                else
                {
                    host += par[i] + "=" + req.QueryString[par[i]];
                }
        }

        return host;
    }

    public string handler()
    {
        String rs = "";
        
        return rs;
    }


    public string setControlUrl(string[] par, HttpRequest req)
    {
        
        ServiceInfo bsi = AppCode.ServiceConfig.GetInfo(UTDtCnvrt.EnumServiceFlag.svgsvrservice);
        string password = HttpContext.Current.Session["password"].ToString();
        string username = HttpContext.Current.Session["username"].ToString();
        string appname = HttpContext.Current.Session["appname"].ToString();
        string host = string.Format("http://{0}/GetRDDataJson?a={1}&b={2}&c=getdatacontrolinfo&d=appname={3};", bsi.url, username, password,appname);
        for (int k = 0; k < par.Length; k++)
        {
            if (par[k] == "_")
                par = Remove(par, k);
        }

        for (int i = 1; i < par.Length; i++)
        {
            if (par[i] != "_")
            {
                string sdata = req.QueryString[par[i]];
                if(par[i]=="controldata")
                {
                    string business = HttpContext.Current.Session["type"].ToString();                    
                    BusinessCall bcall = InitModel.GetBusinessCall(appname, business);
                    String componentname = "";
                    foreach (BusinessComponentCall bcc in bcall.bComponentList)
                    {
                        if (bcc.type.Equals("svg"))
                        {
                            componentname = bcc.name;
                            break;
                        }
                    }
                    List<object> el = InitModel.GetEncryption(appname);
                    Encryption current = new Encryption();
                    foreach(object obj in el )
                    {
                        Encryption ep = (Encryption)obj;
                        if(ep.businessname == business)
                        {
                            current = ep;
                            break;
                        }
                    }
                    if(current!=null && current.crypttype.Length>0 && current.crypttype.ToLower()=="des")                    
                    {
                        string desparam = UTUtil.DesEncrypt.Encrypt(sdata, current.cryptkey);
                        host += par[i] + "=" + System.Web.HttpUtility.UrlEncode(string.Format("{0:d4}", desparam.Length) + desparam) + ";";
                    }
                    else
                    {
                        host += par[i] + "=" + System.Web.HttpUtility.UrlEncode(sdata) + ";";
                    }
                }
                else
                {
                    if (i != par.Length - 1)
                    {
                        host += par[i] + "=" + sdata + ";";
                    }
                    else
                    {
                        host += par[i] + "=" + sdata;
                    }
                }
            }
        }
        USWebService us = new USWebService();
        host = Regex.Split(host, "d=", RegexOptions.IgnoreCase)[0] +"d="+ System.Web.HttpUtility.UrlEncode(Regex.Split(host, "d=", RegexOptions.IgnoreCase)[1]);
        List<MRDDataAll> tmpltData = (List<MRDDataAll>)us.GetDataFromWebService(host, typeof(List<MRDDataAll>));
        List<object> ltret = DataReflect.FromContractData(tmpltData, typeof(System.String));
        string strResult = "";
        if (ltret != null && ltret.Count > 0)
        {
            strResult = ltret[0].ToString();
        }
        if (strResult.Length>0) //
        {
            bsi = AppCode.ServiceConfig.GetInfo(UTDtCnvrt.EnumServiceFlag.businessservice);
            string business = HttpContext.Current.Session["type"].ToString();
           
           // BusinessCall bcall = InitModel.getBusinessCall(appname, EnumBusinessType.transferdatatype.ToString());
            BusinessCall bcall = InitModel.GetBusinessCall(appname,business);
            String componentname = "";
            foreach (BusinessComponentCall bcc in bcall.bComponentList)
            {
                if (bcc.type.Equals("svg"))
                {
                    componentname = bcc.name;
                    break;
                }
            }
            
            host = string.Format("http://{0}/GetRDDataJson?a={1}&b={2}&c=bssvgctrl&d={3}businessname={4};componentname={5};appname={6}",
                bsi.url, System.Web.HttpUtility.UrlEncode(username), System.Web.HttpUtility.UrlEncode(password),
                System.Web.HttpUtility.UrlEncode(strResult), System.Web.HttpUtility.UrlEncode(business),
                System.Web.HttpUtility.UrlEncode(componentname), System.Web.HttpUtility.UrlEncode(appname));
        }
        else
        {
            host = "";
        }

        return host;
    }

    private static string[] Remove(string[] array, int index)
    {
        int length = array.Length;
        string[] result = new string[length - 1];
        Array.Copy(array, result, index);
        Array.Copy(array, index + 1, result, index, length - index - 1);
        return result;
    }
    public static DataTable ListToDataTable(List<int> entitys)
    {
        //检查实体集合不能为空
        if (entitys == null || entitys.Count < 1)
        {
            throw new Exception("需转换的集合为空");
        }
        //取出第一个实体的所有Propertie
        Type entityType = entitys[0].GetType();
        PropertyInfo[] entityProperties = entityType.GetProperties();

        //生成DataTable的structure
        //生产代码中，应将生成的DataTable结构Cache起来，此处略
        DataTable dt = new DataTable();
        for (int i = 0; i < entityProperties.Length; i++)
        {
            //dt.Columns.Add(entityProperties[i].Name, entityProperties[i].PropertyType);
            dt.Columns.Add(entityProperties[i].Name);
        }
        //将所有entity添加到DataTable中
        foreach (object entity in entitys)
        {
            //检查所有的的实体都为同一类型
            if (entity.GetType() != entityType)
            {
                throw new Exception("要转换的集合元素类型不一致");
            }
            object[] entityValues = new object[entityProperties.Length];
            for (int i = 0; i < entityProperties.Length; i++)
            {
                entityValues[i] = entityProperties[i].GetValue(entity, null);
            }
            dt.Rows.Add(entityValues);
        }
        return dt;
    }
    public string getURLResult(string strUrl)
    {
        WebResponse result;
        MemoryStream recvStream = new MemoryStream();
        WebRequest webRequest = WebRequest.Create(strUrl);

        Stream stream;
        string sData = "";
        try
        {
            //查询并返回结果
            result = webRequest.GetResponse();

            stream = result.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            sData = reader.ReadToEnd();
            reader.Close();
        }
        catch (Exception e)
        {

            return "";
        }

        return sData;

    }
    
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}