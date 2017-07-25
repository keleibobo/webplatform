using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using UTDtCnvrt;
using UTUtil;
using System.IO;
using System.Text;


/// <summary>
/// Summary description for WSutils
/// </summary>
/// 
namespace AppCode
{
    public class WSutils
    {
        private static string msg = "";
        public WSutils()
        {
            //
            // TODO: Add constructor logic here
            //
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
                if (ltData != null)
                {
                    rt = DataReflect.FromContractData(ltData, type);
                }
            }
            return rt;
        }
    }

    
}