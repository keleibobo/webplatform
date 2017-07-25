using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UTDtBaseSvr;

namespace AppCode
{
    /// <summary>
    /// Summary description for InitModel
    /// </summary>
    public static class AppParaModel
    {
        public static Dictionary<string, AppPara> datas = new Dictionary<string, AppPara>();
        static AppParaModel()
        {
            init();
        }

        public static void init()
        {
            GetData();
        }

        private static void GetData()
        {
            datas.Clear();
            ServiceInfo si = AppCode.ServiceConfig.GetInfo(UTDtCnvrt.EnumServiceFlag.basesservice);
            string url = String.Format("http://{0}/GetRDDataJson?a=1&b=1&c=appinfo&d=appname={1}", si.url, "");
            List<Object> bcCallList = (List<Object>)AppCode.WSUtil.getListFromWS(url, typeof(AppPara));

            if (bcCallList != null)
                foreach (object obj in bcCallList)
                {
                    AppPara ap = (AppPara)obj;
                    datas.Add(ap.appname, ap);
                }
        }

        public static AppPara GetDataByAppName(string appname)
        {
            AppPara rt = null;
            if (datas.ContainsKey(appname))
            {
                rt = datas[appname];
            }
            return rt;
        }

        public static string GetDataAppDesc(string appname)
        {
            string rt = "";
            if (datas.ContainsKey(appname))
            {
                rt = datas[appname].appdes;
            }
            return rt;
        }
    }
}
