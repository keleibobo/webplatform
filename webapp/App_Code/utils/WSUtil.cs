using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Web.Services;
using UTDtBaseSvr;
using UTDtCnvrt;
using UTUtil;
using UTDtBusiness;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Data;

/// <summary>
/// Summary description for WSUtil
/// </summary>
/// 

namespace AppCode
{
    public static class WSUtil
    {
     //   private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        private static string msg = "";
        /// <summary>
        /// 以流形式返回webservice的结果
        /// </summary>
        /// <param name="strUrl"></param>
        /// <returns></returns>
        public static byte[] getURLResultStream(string strUrl)
        {
            return UTUtil.WebServiceUtil.GetByteFromWebService(strUrl, out msg);
        }

        /// <summary>
        /// 以字符串形式返回webservice的结果
        /// </summary>
        /// <param name="strUrl">webservice路径</param>
        /// <returns></returns>
        public static string getURLResult(string strUrl)
        {
            return UTUtil.WebServiceUtil.GetStringFromWebService(strUrl, out msg);
        }

        public static object getInstanceFromWS(String url)
        {
            List<UTDtCnvrt.MRDDataAll> ltData = (List<UTDtCnvrt.MRDDataAll>)UTUtil.WebServiceUtil.GetObjectFromWebService(url, typeof(List<UTDtCnvrt.MRDDataAll>), out msg);
            if (ltData != null && ltData.Count > 0)
            {
                string typename = DataReflect.GetTypeName(ltData);
                List<Object> rt = DataReflect.FromContractData(ltData, typeof(webcomm_Event));
                if (rt != null && rt.Count > 0)
                {
                    return rt[0];
                }
            }
            return null;
        }

        public static List<object> getloInstanceFromWS(String url)
        {
            List<UTDtCnvrt.MRDDataAll> ltData = (List<UTDtCnvrt.MRDDataAll>)UTUtil.WebServiceUtil.GetObjectFromWebService(url, typeof(List<UTDtCnvrt.MRDDataAll>), out msg);
            if (ltData != null && ltData.Count > 0)
            {
                string typename = DataReflect.GetTypeName(ltData);

                List<Object> rt = DataReflect.FromContractData(ltData, typeof(webcomm_Event));
                if (rt != null && rt.Count > 0)
                {
                   // object obj = rt[0];
                    return rt;
                }
            }
            return null;
        }

        public static object getInstanceFromWS(String url, Type type)
        {
            List<UTDtCnvrt.MRDDataAll> ltData = (List<UTDtCnvrt.MRDDataAll>)UTUtil.WebServiceUtil.GetObjectFromWebService(url, typeof(List<UTDtCnvrt.MRDDataAll>), out msg);
            if (ltData != null && ltData.Count > 0)
            {
                // string typename = DataOperate.GetTypeName(ltData);

                List<Object> rt = DataReflect.FromContractData(ltData, type);
                // bcCall = (BusinessCall)rt[0];
                if (rt != null && rt.Count > 0)
                    return rt[0];
            }
            return null;
        }

        public static UTDtBusiness.BsDataTable getInstanceFromXMLWS(String url)
        {
            List<UTDtCnvrt.MRDDataAll> ltData = (List<UTDtCnvrt.MRDDataAll>)UTUtil.WebServiceUtil.GetObjectFromWebService(url, typeof(List<UTDtCnvrt.MRDDataAll>), out msg);
            if (ltData != null && ltData.Count > 0)
            {
                String typename = DataReflect.GetTypeName(ltData);
                if (typename.Equals(typeof(UTDtBusiness.BsDataTable).FullName, StringComparison.OrdinalIgnoreCase))
                {
                    return (UTDtBusiness.BsDataTable)FormatUtil.fromXML(ltData[1].v, typeof(UTDtBusiness.BsDataTable));
                }
            }
            return null;
        }
        public static UTDtBusiness.BsDataTable getInstanceFromPostXMLWS(String url)
        {
            string errorMessage = "";
            StringBuilder sb = new StringBuilder();
            if (url.IndexOf("?") > 0)
            {
                sb.Append(url.Split('?')[1]);
                Stream stream = UTUtil.MyHttpPost.PostMessageToWebService(url.Split('?')[0], sb, out errorMessage);
                List<MRDDataAll> ltData = MyHttpResponseDataOperator.StreamToContractDataFromJson(stream, out errorMessage);
                if (ltData != null && ltData.Count > 0)
                {
                    String typename = DataReflect.GetTypeName(ltData);
                    if (typename.Equals(typeof(UTDtBusiness.BsDataTable).FullName, StringComparison.OrdinalIgnoreCase))
                    {
                        return (UTDtBusiness.BsDataTable)FormatUtil.fromXML(ltData[1].v, typeof(UTDtBusiness.BsDataTable));
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 有误
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static object getObjectFromXMLWS(String url)
        {
            List<UTDtCnvrt.MRDDataAll> ltData = (List<UTDtCnvrt.MRDDataAll>)UTUtil.WebServiceUtil.GetObjectFromWebService(url, typeof(List<UTDtCnvrt.MRDDataAll>), out msg);
            if (ltData != null && ltData.Count > 0)
            {
                string typename = DataReflect.GetTypeName(ltData);
                Object rt = FormatUtil.fromXML(ltData[1].v, typename);
                // bcCall = (BusinessCall)rt[0];
                if (rt != null)
                    return rt;
            }
            return null;
        }

        /// <summary>
        /// 通用的根据方法名及参数 获取数据
        /// </summary>
        /// <returns></returns>
        public static object getFromWS(string action, params object[] args)//BusinessName=
        {
            string host = "BaseSrvHost"; //默认
            return getFromWS(host, action, args);
        }

        public static object getFromWS(EnumServiceFlag serviceflag, string action, string appname, string rolename, params object[] args)//BusinessName=
        {
            StringBuilder sb = new StringBuilder();
            foreach (object o in args)
            {
                sb.Append(o).Append(";");
            }

            string password = HttpContext.Current.Session["password"].ToString();
            string username = HttpContext.Current.Session["username"].ToString();
      

            ServiceInfo si = ServiceConfig.GetInfo(serviceflag);
            string url = String.Format("http://{0}/GetRDDataJson?a={5}&b={6}&c={1}&d=rolename={2};AppName={3};{4}", si.url, action, rolename, appname, sb,username,password);

            return getObjectFromXMLWS(url);
        }

        public static List<object> getloFromWS(EnumServiceFlag serviceflag, string action, string appname, string rolename, params object[] args)//BusinessName=
        {
            StringBuilder sb = new StringBuilder();
            foreach (object o in args)
            {
                sb.Append(o).Append(";");
            }

            string password = HttpContext.Current.Session["password"].ToString();
            string username = HttpContext.Current.Session["username"].ToString();
      
            ServiceInfo si = ServiceConfig.GetInfo(serviceflag);
            string url = String.Format("http://{0}/GetRDDataJson?a={5}&b={6}&c={1}&d=rolename={2};AppName={3};{4}", si.url, action, rolename, appname, sb,username,password);

            return getloInstanceFromWS(url);
        }

        public static List<object> getListFromWS(String url, Type type)
        {
            List<UTDtCnvrt.MRDDataAll> ltData = (List<UTDtCnvrt.MRDDataAll>)UTUtil.WebServiceUtil.GetObjectFromWebService(url, typeof(List<UTDtCnvrt.MRDDataAll>), out msg);
            if (ltData != null && ltData.Count > 0)
            {
                //string typename = DataReflect.GetTypeName(ltData);
                //Type type = Type.GetType(typename);
                List<Object> rt = DataReflect.FromContractData(ltData, type);
                // bcCall = (BusinessCall)rt[0];
                if (rt != null && rt.Count > 0)
                    return rt;
            }
            return null;
        }

        public static List<object> getListFromPostWS(String url, Type type)
        {
            List<object> rt = new List<object>();
            string errorMessage = "";
            StringBuilder sb = new StringBuilder();
            if (url.IndexOf("?") > 0)
            {
                sb.Append(url.Split('?')[1]);
                Stream stream = UTUtil.MyHttpPost.PostMessageToWebService(url.Split('?')[0], sb, out errorMessage);
                List<MRDDataAll> ltData = MyHttpResponseDataOperator.StreamToContractDataFromJson(stream, out errorMessage);
                if (ltData != null && ltData.Count > 0)
                {
                    rt = DataReflect.FromContractData(ltData, type);
                }
            }
            return rt;
        }

        public static List<UTDtCnvrt.MRDDataAll> getGridDataFromWS(String url, Type type)
        {
            List<MRDDataAll> rt = new List<MRDDataAll>();
            string errorMessage = "";
            StringBuilder sb = new StringBuilder();
            if (url.IndexOf("?") > 0)
            {
                sb.Append(url.Split('?')[1]);
                Stream stream = UTUtil.MyHttpPost.PostMessageToWebService(url.Split('?')[0], sb, out errorMessage);
                if (stream != null )
                {
                    rt = MyHttpResponseDataOperator.StreamToContractDataFromJson(stream, out errorMessage);
                }
            }
            return rt;
        }

        public static UTDtBusiness.BsDataTable getFromXMLWS(EnumServiceFlag serviceflag, string action,string appname, params object[] args)//BusinessName=
        {
            StringBuilder sb = new StringBuilder();
            foreach (object o in args)
            {
                sb.Append(o).Append(";");
            }
            sb.Append("appname=" + appname + ";");
            ServiceInfo si = ServiceConfig.GetInfo(serviceflag);
            string ip=si.url;
            string password = HttpContext.Current.Session["password"].ToString();
            string username = HttpContext.Current.Session["username"].ToString();

            string url = String.Format("http://{0}/SetRDDataJson?a={1}&b={2}&c={3}&d={4};", ip, username, password, action, HttpUtility.UrlEncode(sb.ToString()));
            url = url.Replace(";;", ";");
            return getInstanceFromPostXMLWS(url);
        
        }

        public static UTDtBusiness.BsDataTable getTableFromPostXMLWS(EnumServiceFlag serviceflag, string action, string appname,  params object[] args)
        {
            StringBuilder sb = new StringBuilder();
            foreach (object o in args)
            {
                sb.Append(o).Append(";");
            }
            sb.Append("appname=" + appname + ";");
            ServiceInfo si = ServiceConfig.GetInfo(serviceflag);
            string ip = si.url;
            string password = HttpContext.Current.Session["password"].ToString();
            string username = HttpContext.Current.Session["username"].ToString();

            string url = String.Format("http://{0}/SetRDDataJson?a={1}&b={2}&c={3}&d={4};random={5};", ip, username, password, action, HttpUtility.UrlEncode(sb.ToString()),DateTime.Now.Ticks);
            url = url.Replace(";;", ";");
            return getInstanceFromPostXMLWS(url);

        }

        public static object getFromWSByRole(string action, string roles, string appname, params object[] args)
        {
            StringBuilder sb = new StringBuilder();
            foreach (object o in args)
            {
                sb.Append(o);
            }

            string password = HttpContext.Current.Session["password"].ToString();
            string username = HttpContext.Current.Session["username"].ToString();
      
            ServiceInfo si = ServiceConfig.GetInfo(EnumServiceFlag.basesservice);
            string url = String.Format("http://{0}:{1}/GetRDDataJson?a={6}&b={7}&c={2}&d=rolename={3};AppName={4};{5}", si.ip, si.port, action, roles, appname, sb,username,password);
            return getInstanceFromWS(url);
        }

        public static DataTable GetDataTableFromWS(EnumServiceFlag serviceflag, string tablename)
        {
            string action = "gettable";
            ServiceInfo si = ServiceConfig.GetInfo(serviceflag);
            object[] args = new object[] { "tablename=t_tableinfo;name=" + tablename };
            object rt = WSUtil.getFromWS(action, args);
            DataTable dt = DataTableSerializer.DESerialize(rt.ToString());
            return dt;
        }

        public static List<String> GetInputDataFromWS(string param,string appname,string rolename)
        {
            List<String> rd = new List<string>();
            string[] value = param.Split(';');
            object[] args = new object[] { "BusinessName=" + value[0], "ComponentName=" + value[1], "EditColumnName=" + value[2], "appname=" + value[3] };
            List<Object> ol = getFromWSColumnInfo(EnumServiceFlag.businessservice, "bsusrtab",appname,rolename, args);
            if (ol != null)
            for (int i = 0; i < ol.Count; i++)
            {
                rd.Add((String)ol[i]);
            }
            return rd;
        }

        public static List<String> GetInputDataFromWSwithSRC(string param, string appname, string rolename)
        {
            List<String> rd = new List<string>();
            string[] value = param.Split(';');
            object[] args = new object[] { "BusinessName=" + value[0], "ComponentName=" + value[1], "EditColumnName=" + value[2], "appname=" + value[3],value[4] };
            List<Object> ol = getFromWSColumnInfo(EnumServiceFlag.businessservice, "bsusrtab", appname, rolename, args);
            if (ol != null)
                for (int i = 0; i < ol.Count; i++)
                {
                    rd.Add((String)ol[i]);
                }
            return rd;
        }

        public static List<String> GetInputDataFromWSN(object[] args,string appname,string rolename)
        {
            List<String> rd = new List<string>();
            List<Object> ol = getFromWSColumnInfo(EnumServiceFlag.businessservice, "bsusrtab",appname,rolename, args);
            if(ol!=null)
            for (int i = 0; i < ol.Count; i++)
            {
                rd.Add((String)ol[i]);
            }
            return rd;
        }

        public static List<object> getFromWSColumnInfo(EnumServiceFlag serviceflag, string action,string appname,string rolename, params object[] args)//BusinessName=
        {

            StringBuilder sb = new StringBuilder();
            foreach (object o in args)
            {
                sb.Append(o).Append(";");
            }
            string password = HttpContext.Current.Session["password"].ToString();
            string username = HttpContext.Current.Session["username"].ToString();
      
            ServiceInfo si = ServiceConfig.GetInfo(serviceflag);
            string url = String.Format("http://{0}/SetRDDataJson?a={5}&b={6}&c={1}&d=rolename={2};{3};{4};random={7};", si.url, action, rolename,appname, sb,username,password,DateTime.Now.Ticks);

            return getListFromPostWS(url,null);
        }

        public static List<UTDtCnvrt.MRDDataAll> getGridDataFromWS(EnumServiceFlag serviceflag, string action, string appname, string rolename, params object[] args)//BusinessName=
        {

            StringBuilder sb = new StringBuilder();
            foreach (object o in args)
            {
                sb.Append(o).Append(";");
            }
            string password = HttpContext.Current.Session["password"].ToString();
            string username = HttpContext.Current.Session["username"].ToString();

            ServiceInfo si = ServiceConfig.GetInfo(serviceflag);
            string url = String.Format("http://{0}/SetRDDataJson?a={5}&b={6}&c={1}&d=rolename={2};{3};{4}", si.url, action, rolename, appname, sb, username, password);

            return getGridDataFromWS(url, null);
        }

        public static List<String> GetInputDataFromWS(string URL, string Method, string param, string paramstr,string appname,string rolename)
        {
            List<String> rd = new List<String>();

            string[] value = paramstr.Split(';');

            if (param.ToLower().IndexOf("appname") == 0)
            {
                param = value[1] + "." + param;
            }

            object[] args = new object[] { "BusinessName=" + value[0], "ComponentName=" + value[1], "EditColumnName=" + value[2], "appname=" + value[3],param };
            List<String> ol = GetInputDataFromWSN(args,"", rolename);
            if (ol != null)
            for (int i = 0; i < ol.Count; i++)
            {
                rd.Add((String)ol[i]);
            }
            return rd;
        }

        public static List<String> GetInputDataFromWSWithSRC(string URL, string Method, string param, string paramstr, string appname, string rolename)
        {
            List<String> rd = new List<String>();

            string[] value = paramstr.Split(';');

            if (param.ToLower().IndexOf("appname") == 0)
            {
                param = value[1] + "." + param;
            }

            object[] args = new object[] { "BusinessName=" + value[0], "ComponentName=" + value[1], "EditColumnName=" + value[2], "appname=" + value[3],value[4], param };
            List<String> ol = GetInputDataFromWSN(args, "", rolename);
            if (ol != null)
                for (int i = 0; i < ol.Count; i++)
                {
                    rd.Add((String)ol[i]);
                }
            return rd;
        }

        public static List<String> GetInputDataFromWS(string WebUrl, string Method, string param, string paramstr)
        {
           
            List<String> rd = new List<String>();

            string[] value = paramstr.Split(';');

            if (param.ToLower().IndexOf("appname") == 0)
            {
                param = value[1] + "." + param;
            }
            if (value[2].IndexOf("extendpro") == 0)
            {
                value[2] = value[2].Replace("extendpro", "");
            }
            if (param.IndexOf("extendpro") == 0)
            {
                param = param.Replace("extendpro", "");
            }
            object[] args = new object[] { "BusinessName=" + value[0], "ComponentName=" + value[1], "EditColumnName=" + value[2], "appname=" + value[3], param };
            List<String> ol = GetInputDataFromWSN(args,"", WebUrl);
            if (ol != null)
            for (int i = 0; i < ol.Count; i++)
            {
                rd.Add((String)ol[i]);
            }
            return rd;
        }

        public static List<String> GetInputDataFromWSN(string URL, string Method, string param, string paramstr,string appname,string rolename)
        {
            List<String> rd = new List<String>();
            string[] value = paramstr.Split(';');
            object[] args = new object[] { "BusinessName=" + value[0], "ComponentName=" + value[1], "EditColumnName=" + value[2], "appname=" + value[3], param };
            List<String> ol = GetInputDataFromWSN(args,"", rolename);
            if (ol != null)
            for (int i = 0; i < ol.Count; i++)
            {
                rd.Add((String)ol[i]);
            }
            return rd;
        }
    }
}