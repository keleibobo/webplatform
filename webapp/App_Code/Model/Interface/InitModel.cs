using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UTDtBaseSvr;
using System.Xml;

namespace AppCode
{
    /// <summary>
    /// 从框架基础服务、用户权限服务获取需要的数据类
    /// </summary>
    public static class InitModel
    {

        private static ServiceInfo sibase = null;
        private static ServiceInfo siur = null;
        private static ServiceInfo sisvg = null;
        /// <summary>
        /// string为ComponentPage的type
        /// </summary>
        private static Dictionary<string, ComponentPage> componentpages = new Dictionary<string, ComponentPage>();
        /// <summary>
        /// object类型为ButtonInfo
        /// </summary>
        private static Dictionary<string, ButtonInfo> buttoninfos = new Dictionary<string, ButtonInfo>();
        /// <summary>
        /// 项目业务字典
        /// </summary>
        public static Dictionary<string, Dictionary<string, BusinessCall>> appbusinesscall = new Dictionary<string, Dictionary<string, BusinessCall>>();
        /// <summary>
        /// 项目菜单字典，object类型为UserMenuInfo
        /// </summary>
        public static Dictionary<string, Dictionary<string, List<Object>>> appusermenu = new Dictionary<string, Dictionary<string, List<Object>>>();
        /// <summary>
        /// object类型为string
        /// </summary>
        private static Dictionary<string, List<object>> appusermenuroles = new Dictionary<string, List<object>>();
        /// <summary>
        /// int:mappingtableid,object:DataMappingTable
        /// </summary>

        //public static Dictionary<string,Dictionary<string,Object>> svgInfoes=new Dictionary<string,Dictionary<string,object>>();

        /// <summary>
        /// 项目扩展参数表
        /// </summary>
        private static Dictionary<string, object> mappingparam = new Dictionary<string, object>();

        private static bool bintial = false;
        /// <summary>
        /// 用户权限对应的业务信息列表
        /// </summary>
        public static List<UserRightObj.BusinessInfo> businessinfoes = new List<UserRightObj.BusinessInfo>();
        /// <summary>
        /// 扩展参数列表
        /// </summary>
        public static List<extraparaminfo> ExtendParamList = new List<extraparaminfo>();

        /// <summary>
        /// 登录超时
        /// </summary>

        /// <summary>
        /// 初始化变量
        /// </summary>
        public static void init()
        {
            if (!bintial)
            {
                Refresh();
                bintial = true;
            }
        }
        /// <summary>
        /// 从网络服务重新获取数据
        /// </summary>
        public static void Refresh()
        {
            sibase = ServiceConfig.GetInfo(UTDtCnvrt.EnumServiceFlag.basesservice);
            siur = ServiceConfig.GetInfo(UTDtCnvrt.EnumServiceFlag.userrightservice);
            sisvg = ServiceConfig.GetInfo(UTDtCnvrt.EnumServiceFlag.svgsvrservice);

            AppParaModel.init();
            GetComponentPageFromBaseSvr();
            GetButtonInfoFromBaseSvr();
            GetAppBusinessCallFromBaseSvr();

            GetAppUserMenuRolesFromUserRightSvr();
            GetAppUserMenuFromUserRightSvr();

            GetBusinessInfoesFromUserRightSvr();

            getExtendParamFromBaseSvr();
            GetParamInfoFromBaseSvr();

            GetEncryptionFromUserRightSvr();
            reinit();
        }

        private static void GetParamInfoFromBaseSvr()
        {

            String url = String.Format("http://{0}/GetRDDataJSON?a=1&b=1&c=getparaminfo&d=appname={1}", sibase.url, ReadConfig.TheReadConfig["appname"]);

            List<object> paramlist = (List<object>)AppCode.WSUtil.getListFromWS(url, typeof(ParamInfo));
            mappingparam.Clear();
            if (paramlist != null)
            {

                foreach (ParamInfo pinfo in paramlist)
                {
                    mappingparam.Add(pinfo.appname + "_" + pinfo.paramname, pinfo.paramvalue);
                }
            }

        }

        public static string GetLoginTimeOut(string appname)
        {
            int result = 0;
            string key = appname + "_logintimeout";
            if (mappingparam.ContainsKey(key))
            {
                result =Convert.ToInt32(mappingparam[key]);
                result = result == 0 ? int.MaxValue : result;
            }
            return result.ToString();
        }

        public static string GetEventDisplaySize(string appname)
        {
            int result = 0;
            string key = appname + "_eventdisplaysize";
            if (mappingparam.ContainsKey(key))
            {
                result = Convert.ToInt32(mappingparam[key]);
                result = result == 0 ? int.MaxValue : result;
            }
            return result.ToString();
        }

        public static string GetShowEventWindow(string appname)
        {
            string result = "false";
            string key = appname + "_showeventwindow";
            if (mappingparam.ContainsKey(key))
            {
                result = mappingparam[key].ToString().ToLower();
            }
            return result;
        }

        public static string GetRememberuser(string appname)
        {
            string result = "false";
            string key = appname + "_rememberuser";
            if (mappingparam.ContainsKey(key))
            {
                result = mappingparam[key].ToString().ToLower();
            }
            return result;
        }

        private static void GetBusinessInfoesFromUserRightSvr()
        {
            USWebService service = new USWebService();
            string appname = ReadConfig.TheReadConfig["appname"];
            businessinfoes = service.GetBusiness(appname);
        }

        private static void getExtendParamFromBaseSvr()
        {
            string appname = ReadConfig.TheReadConfig["appname"];
            ExtendParamList.Clear();
            string url = String.Format("http://{0}/GetRDDataJson?a=1&b=1&c=extraparaminfo&d=appname={1}", sibase.url, appname);

            List<Object> bcCallList = (List<Object>)AppCode.WSUtil.getListFromWS(url, typeof(extraparaminfo));
            if (bcCallList != null)
            {
                foreach (object obj in bcCallList)
                {
                    extraparaminfo bi = (extraparaminfo)obj;
                    ExtendParamList.Add(bi);
                }
            }

        }
        static int time = 0;
        /// <summary>
        /// 数据异常时重新初始化
        /// </summary>
        public static void reinit()
        {
            if (time > 3)
            {
                time = 0;
                return;
            }
            if (appusermenu.Count == 0)
            {
                time++;
                Refresh();

            }
            bintial = true;
        }

        private static void GetComponentPageFromBaseSvr()
        {
            componentpages.Clear();
            string url = String.Format("http://{0}/GetRDDataJson?a=1&b=1&c=componentpage&d=", sibase.url);

            List<Object> bcCallList = (List<Object>)AppCode.WSUtil.getListFromWS(url, typeof(ComponentPage));
            if (bcCallList != null)
            {
                foreach (object obj in bcCallList)
                {
                    ComponentPage bi = (ComponentPage)obj;
                    componentpages.Add(bi.type.ToLower(), bi);
                }
            }
        }

        private static void GetButtonInfoFromBaseSvr()
        {
            buttoninfos.Clear();
            string url = String.Format("http://{0}/GetRDDataJson?a=1&b=1&c=buttoninfo&d=", sibase.url);

            List<Object> bcCallList = (List<Object>)AppCode.WSUtil.getListFromWS(url, typeof(ButtonInfo));
            if (bcCallList != null)
            {
                foreach (object obj in bcCallList)
                {
                    ButtonInfo bi = (ButtonInfo)obj;
                    buttoninfos.Add(bi.id.ToLower(), bi);
                }
            }
        }
        private static void GetAppBusinessCallFromBaseSvr()
        {
            appbusinesscall.Clear();
            string url = "";
            foreach (KeyValuePair<String, AppPara> kv in AppParaModel.datas)
            {
                url = String.Format("http://{0}/GetRDDataJson?a=1&b=1&c=businessCallall&d=AppName={1}", sibase.url, kv.Key);

                List<Object> bcCallList = (List<Object>)AppCode.WSUtil.getListFromWS(url, typeof(BusinessCall));
                if (bcCallList != null)
                {
                    Dictionary<string, BusinessCall> data = new Dictionary<string, BusinessCall>();
                    foreach (BusinessCall bcCall in bcCallList)
                    {
                        data.Add(bcCall.BussinessName, bcCall);
                    }
                    appbusinesscall.Add(kv.Key, data);
                }
            }
        }

        /// <summary>
        /// 获取各个项目中角色对应的用户菜单
        /// </summary>
        //private static void GetAppUserMenuFromBaseSvr()
        //{
        //    appusermenubasesvr.Clear();
        //    ServiceInfo si = ServiceConfig.GetInfo("BaseSrvHost");
        //    string url = "";
        //    foreach (KeyValuePair<String, List<object>> kv in appusermenuroles)
        //    {
        //        Dictionary<string, List<Object>> role = new Dictionary<string, List<Object>>();
        //        foreach (object obj in kv.Value)
        //        {
        //            url = String.Format("http://{0}:{1}/GetRDDataJson?a=1&b=1&c=getallusersbyroles&d=appname={2};rolename={3}", si.ip, si.port, kv.Key, obj.ToString());

        //            List<object> lt = null;
        //            List<object> bcCallList = (List<object>)AppCode.WSUtil.getListFromWS(url, typeof(UserMenuInfo));
        //            if (bcCallList != null)
        //            {
        //                lt = bcCallList;
        //            }
        //            else
        //            {
        //                lt = new List<object>();
        //            }
        //            role.Add((string)obj, lt);
        //        }
        //        appusermenubasesvr.Add(kv.Key, role);
        //    }
        //}


        /// <summary>
        /// 获取各个项目中角色对应的用户菜单
        /// </summary>
        private static void GetAppUserMenuFromUserRightSvr()
        {
            appusermenu.Clear();
            string url = "";
            foreach (KeyValuePair<String, List<object>> kv in appusermenuroles)
            {
                foreach (object obj in kv.Value)
                {
                    url = String.Format("http://{0}/GetRDDataJson?a=1&b=1&c=getuserrightinfo&d=appname={1};rolename={2}", siur.url, kv.Key, obj.ToString());

                    List<object> bcCallList = (List<object>)AppCode.WSUtil.getListFromWS(url, typeof(UserRihgtInfo));
                    if (bcCallList != null)
                    {
                        List<object> dt1 = null;
                        if (appusermenu.ContainsKey(kv.Key))
                        {
                            Dictionary<string, List<Object>> dt = appusermenu[kv.Key];
                            if (dt.ContainsKey(obj.ToString()))
                            {
                                dt1 = dt[obj.ToString()];
                            }
                            else
                            {
                                dt1 = new List<object>();
                                dt.Add(obj.ToString(), dt1);
                            }
                        }
                        else
                        {
                            Dictionary<string, List<object>> dttemp = new Dictionary<string, List<object>>();
                            dt1 = new List<object>();
                            dttemp.Add(obj.ToString(), dt1);
                            appusermenu.Add(kv.Key, dttemp);
                        }

                        foreach (object obj1 in bcCallList)
                        {
                            UserRihgtInfo ui = (UserRihgtInfo)obj1;
                            UserMenuInfo uri = GetUserMenuInfoByMenuName(kv.Key, obj.ToString(), ui.name);
                            if (uri != null)
                            {
                                if (ui.parentname.Length > 0)
                                {
                                    UserMenuInfo uri1 = GetUserMenuInfoByMenuName(kv.Key, obj.ToString(), ui.parentname);
                                    if (uri1 != null)
                                    {
                                        uri.parentid = uri1.id;
                                    }
                                    else
                                    {
                                        uri.parentid = "0";
                                    }
                                }
                                uri.parentname = ui.parentname;
                                uri.pathlevel = ui.pathlevel;
                                uri.rolecanusefunction = ui.rolecanusefunction;
                                uri.showorder = ui.showorder;
                                uri.usercannotusefunction = ui.usercannotusefunction;
                                uri.desc = ui.desc;
                                uri.customuserfunction = ui.customuserfunction;
                            }
                            else
                            {
                                uri = new UserMenuInfo();
                                uri.name = ui.name;
                                uri.parentname = ui.parentname;
                                uri.pathlevel = ui.pathlevel;
                                uri.rolecanusefunction = ui.rolecanusefunction;
                                uri.showorder = ui.showorder;
                                uri.usercannotusefunction = ui.usercannotusefunction;
                                uri.desc = ui.desc;
                                uri.customuserfunction = ui.customuserfunction;
                                BusinessCall bc = GetBusinessCall(kv.Key, ui.name);



                                if (bc != null)
                                {

                                    uri.link = bc.link;
                                    uri.functions = bc.functions;
                                    uri.functionsdes = bc.functionsdes;
                                    uri.id = bc.BusinessID;
                                }
                                dt1.Add(uri);
                            }
                        }
                    }
                }
            }
        }

        private static void GetAppUserMenuRolesFromUserRightSvr()
        {
            appusermenuroles.Clear();
            string url = "";
            foreach (KeyValuePair<String, AppPara> kv in AppParaModel.datas)
            {
                url = String.Format("http://{0}/GetRDDataJson?a=1&b=1&c=getrolename&d=appname={1}", siur.url, kv.Key);

                List<Object> bcCallList = (List<Object>)AppCode.WSUtil.getListFromWS(url, typeof(string));
                if (bcCallList != null)
                {
                    appusermenuroles.Add(kv.Key, bcCallList);
                }
            }
        }


        static Dictionary<string, List<Object>> encryptionMap = new Dictionary<string, List<Object>>();
        private static void GetEncryptionFromUserRightSvr()
        {
            encryptionMap.Clear();
            string url = "";
            foreach (KeyValuePair<String, AppPara> kv in AppParaModel.datas)
            {
                url = String.Format("http://{0}/GetRDDataJson?a=1&b=1&c=getencryption&d=AppName={1}", siur.url, kv.Key);

                List<Object> enList = (List<Object>)AppCode.WSUtil.getListFromWS(url, typeof(UserRightObj.Encryption));
                if (enList != null)
                {
                    encryptionMap.Add(kv.Key, enList);
                }
            }
        }


        //public static void GetSVGNodeInfoesFrom()
        //{
        //    svgInfoes.Clear();      

        //    String url = string.Format(@"http://{0}/{1}?appname={2}", sisvg.url, "GetSVGNodesJSON", LoginUserModel.AppName);
        //    String rt = AppCode.WSUtil.getURLResult(url);
        //    Object[] vals = (Object[])FormatUtil.fromJSON(rt);
        //    if(vals!=null&&vals.Length>0)
        //    foreach (Object o in vals)
        //    {
        //        Dictionary<string, Object> node = (Dictionary<string, Object>)o;
        //        string name = node["name"].ToString();

        //        //files.Add(name, String.Format("{0}_{1}_{2}.svg", appname, id, filesize));
        //        svgInfoes.Add(name, node);
        //    }
        //}

        /// <summary>
        /// 根据项目代码和业务代码获取对应的业务数据
        /// </summary>
        /// <param name="appname">项目代码</param>
        /// <param name="name">业务代码</param>
        /// <returns>业务代码对应的业务数据对象</returns>
        public static BusinessCall GetBusinessCall(string appname, string name)
        {
            BusinessCall bc = null;
            if (appbusinesscall.ContainsKey(appname))
            {
                Dictionary<string, BusinessCall> dt = appbusinesscall[appname];
                if (name != null && dt.ContainsKey(name))
                {
                    bc = dt[name];
                }
            }
            return bc;
        }

        private static UserMenuInfo GetUserMenuInfoByMenuName(string appname, string rolename, string menuname)
        {
            UserMenuInfo rt = null;
            List<Object> bt = GetUserMenuByRoleID(appname, rolename);
            foreach (object obj in bt)
            {
                if (((UserMenuInfo)obj).name.Equals(menuname))
                {
                    rt = (UserMenuInfo)obj;
                    break;
                }
            }

            return rt;
        }
        /// <summary>
        /// 根据项目代码和角色代码以及菜单编号获取对应的菜单数据
        /// </summary>
        /// <param name="appname">项目代码</param>
        /// <param name="rolename">角色代码</param>
        /// <param name="menuid">菜单编号</param>
        /// <returns>返回对应的菜单数据</returns>
        public static UserMenuInfo GetUserMenuInfoByMenuID(string appname, string rolename, string menuid)
        {
            UserMenuInfo rt = null;
            //if (appusermenu.ContainsKey(appname))
            //{
            //    Dictionary<string, List<Object>> dt = appusermenu[appname];
            //    if (dt.ContainsKey(rolename))
            //    {
            //        List<Object> bt = dt[rolename];
            //        foreach (object obj in bt)
            //        {
            //            if (((UserMenuInfo)obj).id.Equals(menuid))
            //            {
            //                rt = (UserMenuInfo)obj;
            //                break;
            //            }
            //        }
            //    }
            //}

            List<Object> bt = GetUserMenuByRoleID(appname, rolename);
            if (bt != null)
                foreach (object obj in bt)
                {
                    if (((UserMenuInfo)obj).id.Equals(menuid))
                    {
                        rt = (UserMenuInfo)obj;
                        break;
                    }
                }

            return rt;
        }
        /// <summary>
        /// 获取项目代码和角色代码对应的所有菜单数据
        /// </summary>
        /// <param name="appname">项目代码</param>
        /// <param name="rolename">角色代码</param>
        /// <returns>角色代码所有的菜单数据列表</returns>
        public static List<Object> GetUserMenuByRoleID(string appname, string rolename)
        {
            List<Object> rt = null;
            if (appusermenu != null && appusermenu.Count == 0)
            {
                Refresh();
            }
            if (appusermenu.ContainsKey(appname))
            {
                Dictionary<string, List<Object>> dt = appusermenu[appname];
                if (dt.ContainsKey(rolename))
                {
                    rt = dt[rolename];

                }
            }
            return rt;
        }


        private static string GetFunctionsByMenuID(string appname, string rolename, string menuid)
        {
            string rt = "";
            UserMenuInfo umi = GetUserMenuInfoByMenuID(appname, rolename, menuid);
            if (umi != null)
            {
                rt = umi.usercannotusefunction;
            }
            return rt;
        }
        /// <summary>
        /// 根据按钮代码获取对应的按钮数据
        /// </summary>
        /// <param name="id">按钮代码</param>
        /// <returns>返回按钮数据</returns>
        public static ButtonInfo GetButtonInfoByID(string id)
        {
            if (buttoninfos.ContainsKey(id))
            {
                return buttoninfos[id];
            }
            return null;
        }
        /// <summary>
        /// 根据组件类型获取对应的处理网页地址
        /// </summary>
        /// <param name="type">组件类型</param>
        /// <returns>返回对应的网页地址</returns>
        public static ComponentPage GetComponentPageByType(string type)
        {
            if (componentpages.ContainsKey(type.ToLower()))
            {
                return componentpages[type.ToLower()];
            }
            return null;
        }

        /// <summary>
        /// 根据项目获取ParamInfo,项目名读取配置文件
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, object> getParamInfo()
        {
            return mappingparam;
        }

        public static List<object> GetEncryption(string appname)
        {
            if (encryptionMap.ContainsKey(appname))
            {
                return encryptionMap[appname];
            }

            return null;
        }

        public static BusinessCall getBusinessCallByType(String appname, String type)
        {
            //foreach (KeyValuePair<string, Dictionary<string, BusinessCall>> kvp in appbusinesscall)
            //{
            //    if (kvp.Key.Equals(""))
            //    {
            //    }
            //}

            if (appbusinesscall.ContainsKey(appname))
            {

                foreach (KeyValuePair<String, BusinessCall> kvp in appbusinesscall[appname])
                {
                    BusinessCall bcall = kvp.Value;
                    if (bcall.businesstype.Equals(type))
                        return bcall;
                }
            }

            return null;
        }
    }
}
