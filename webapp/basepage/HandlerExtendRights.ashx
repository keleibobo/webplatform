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
        string type = request.Params["type"];
        USWebService us = new USWebService();
        if (type == "businessright")//获取该业务的所有权限
        {
            int index = 1;
            string appname = request.Params["appname"];
            string businessname = request.Params["businessname"];
            List<NodeRightData> ls = us.GetImportUserRight(appname, businessname);
            Dictionary<string, string> ParentNameList = new Dictionary<string, string>();
            string json = "{\"total\":"+ls.Count+",\"rows\":[";
            bool first = true;
            foreach (NodeRightData st in ls)
            {
                if (!ParentNameList.ContainsKey(st.StationName))
                {
                    ParentNameList.Add(st.StationName, st.StationName);
                }
            }
            foreach(string pn in ParentNameList.Values)
            {
                if (first)
                {
                    first = false;
                    json += "{\"id\":" + index + ",\"name\":\"" + pn + "\",\"state\":\"closed\",\"view\":\"false\",\"control\":\"false\"}";
                }
                else
                {
                    json += ",{\"id\":" + index + ",\"name\":\"" + pn + "\",\"state\":\"closed\",\"view\":\"false\",\"control\":\"false\"}";
                }
                int parentid = index;
                index++;
                foreach (NodeRightData st in ls)
                {
                    if (st.StationName == pn)
                    {
                        json += ",{\"id\":" + index + ",\"name\":\"" + st.NodeName + "\",\"_parentId\":"+parentid+",\"parname\":\""+pn+"\"}";
                        index++;
                    }
                }
            }
            json += "]}";
            context.Response.Write(json);
        }
        else if (type == "addright")//给用户分配权限
        {
            string appname = request.Params["appname"];
            string username = request.Params["username"];
            string businessname = request.Params["businessname"];
            string rights = request.Params["rights"];
            ResultData rd = us.AddUserRight(appname, username, businessname, rights);
            context.Response.Write(rd.result);
        }
        else if (type == "getuserright")//获取用户已分配的权限
        {
            string appname = request.Params["appname"];
            string username = request.Params["username"];
            string businessname = request.Params["businessname"];
            string result = us.GetUserRight(appname, username, businessname);
            if (result == "")
                result = "";
            context.Response.Write(result);
        }
        else if (type == "deleteright")//删除已分配给用户的权限
        {
            string appname = request.Params["appname"];
            string username = request.Params["username"];
            string businessname = request.Params["businessname"];
            ResultData rd = us.DeleteUserRight(appname, username, businessname);
            context.Response.Write(rd.result);
        }
        else if (type == "updateright")//修改已分配给用户的权限
        {
            string appname = request.Params["appname"];
            string username = request.Params["username"];
            string businessname = request.Params["businessname"];
            string rights = request.Params["rights"];
            ResultData rd = us.UpdateUserRight(appname, username, businessname, rights);
            context.Response.Write(rd.result);
        }
        else
        {
            string json = "";
            context.Response.Write(json);
        }
    }

    //public string GetSonStr(List<string> ct,ref int index)
    //{
    //    string str = "";
    //    if (ct != null && ct.Count > 0)
    //    {
    //        bool first = true;
    //        str = ",\"children\":[";
    //        foreach (string s in ct)
    //        {
    //            if (first)
    //            {
    //                str += "{\"id\":"+index+",\"name\":\""+s+"\"}";
    //                first = false;
    //            }
    //            else
    //            {
    //                str += ",{\"id\":" + index + ",\"name\":\"" + s + "\"}";
    //            }
    //            index++;
    //        }
    //        str += "]";
    //    }
    //    return str;
    //}
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}