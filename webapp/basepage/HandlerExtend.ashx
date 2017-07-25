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
        object[] args = new object[] { "BusinessName=" + value[0], "ComponentName=" + value[1], "EditColumnName=" + value[2], "appname=" + value[3] };
        List<UTDtCnvrt.MRDDataAll> ListMRDD = AppCode.WSUtil.getGridDataFromWS(EnumServiceFlag.businessservice, "bsusrtab", "", "", args);

        List<object> ListTreeNode = DataReflect.FromContractData(ListMRDD, typeof(StationTree));
        string json = "{\"total\":" + (ListMRDD.Count - 1) + ",\"rows\":[";
        if (ListTreeNode.Count > 0)
        {
            if (((StationTree)ListTreeNode[0]).ShortName == "所有")
            {
                ListTreeNode.Remove(ListTreeNode[0]);
            }
        }
        bool first = true;
        for (int i = 1; i <= ListTreeNode.Count; i++)
        {
            StationTree Td = (StationTree)ListTreeNode[i-1];
            if (first)
            {
                json += "{\"id\":" + i + ",\"view\":\"false\",\"desc\":\"" + Td.FullName + "\",\"ExtendStr\":\"" + Td.ShortName + "\"}";
                first = false;
            }
            else
            {
                json += ",{\"id\":" + i + ",\"view\":\"false\",\"desc\":\"" + Td.FullName + "\",\"ExtendStr\":\"" + Td.ShortName + "\"}";
            }
            
        }
        json += "]}";
        context.Response.Write(json);
    }

    public class StationTree
    {
        /// <summary>
        /// 厂站短名称
        /// </summary>
        public string ShortName;
        /// <summary>
        /// 厂站描述（长名称）
        /// </summary>
        public string FullName;
        /// <summary>
        /// 厂站父节点短名称
        /// </summary>
    }
    
    public bool IsReusable {
        get {
            return false;
        }
    }

}