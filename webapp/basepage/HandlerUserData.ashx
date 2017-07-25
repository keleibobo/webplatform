<%@ WebHandler Language="C#" Class="HandlerExtendRights" %>

using System;
using System.Collections.Generic;
using System.Web;
using UserRightObj;
using UTDtBaseSvr;
using UTDtCnvrt;
using System.Web.SessionState;
using AppCode;

public class HandlerExtendRights : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        HttpRequest request = context.Request;
        HttpResponse response = context.Response;
        context.Response.ContentType = "text/plain";
        string param = request.Params["param"];

        string[] value = param.Split(';');
        object[] args = new object[] { "BusinessName=" + value[0], "ComponentName=" + value[1], "EditColumnName=" + value[2], "appname=" + value[3], "param=RealTimeDLHJJK" };
        List<UTDtCnvrt.MRDDataAll> ListMRDD = AppCode.WSUtil.getGridDataFromWS(EnumServiceFlag.businessservice, "bsusrtab", "", "", args);
        List<object> ListTreeNode = DataReflect.FromContractData(ListMRDD, typeof(UserData));
        UserData userData = new UserData();
        if (ListTreeNode != null)
        {
            userData = (UserData) ListTreeNode[0];
        }
        string json = "{\"total\":" + userData.datalist.Count + ",\"rows\":[";
        bool first = true;
        for (int i = 1; i <= userData.datadesclist.Count; i++)
        {
            if (first)
            {
                json += "{\"id\":" + i + ",\"view\":\"false\",\"desc\":\"" + userData.datadesclist[i - 1] + "\",\"ExtendStr\":\"" + userData.datalist[i - 1] + "\"}";
                first = false;
            }
            else
            {
                json += ",{\"id\":" + i + ",\"view\":\"false\",\"desc\":\"" + userData.datadesclist[i - 1] + "\",\"ExtendStr\":\"" + userData.datalist[i - 1] + "\"}";
            }
            
        }
        json += "]}";
        context.Response.Write(json);
    }

    public class UserData
    {
        /// <summary>
        /// 项目名
        /// </summary>	
        public string appname = "";
        /// <summary>
        /// 用户名
        /// </summary>	
        public string username = "";
        /// <summary>
        /// 控制参数
        /// </summary>
        public string extendparam = "";
        /// <summary>
        /// 数据编号列表
        /// </summary>
        public List<string> datalist = new List<string>();
        /// <summary>
        /// 数据描述列表
        /// </summary>
        public List<string> datadesclist = new List<string>();
    }
    
    public bool IsReusable {
        get {
            return false;
        }
    }

}