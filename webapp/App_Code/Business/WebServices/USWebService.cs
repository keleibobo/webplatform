using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Configuration;
using UTDtCnvrt;
using UserRightObj;
using System.Xml.Serialization;

namespace AppCode
{
    public class USWebService
    {
        string UserRightHost;
        string UserRightPort;
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        public USWebService()
        {
            ServiceInfo si = ServiceConfig.GetInfo(UTDtCnvrt.EnumServiceFlag.userrightservice);
            this.UserRightHost = si.ip;
            this.UserRightPort = si.port;
        }

        /// <summary>
        /// 从wcf服务获取数据转化成指定type
        /// </summary>
        /// <param name="surl">数据地址</param>
        /// <param name="type">数据类型</param>
        /// <returns></returns>
        public Object GetDataFromWebService(string surl, Type type)
        {
            Object rt = null;
            try
            {
                WebRequest webRequest = WebRequest.Create(surl);
                WebResponse result = webRequest.GetResponse();
                Stream stream = result.GetResponseStream();
                DataContractJsonSerializer dataJson = new DataContractJsonSerializer(type);
                rt = (Object)dataJson.ReadObject(stream);
            }
            catch (System.Exception ex)
            {

            }
            return rt;
        }

        public Object GetboolFromWebService(string surl, Type type)
        {

            bool rt = false;
            MemoryStream recvStream = new MemoryStream();
            try
            {
                WebRequest webRequest = WebRequest.Create(surl);
                Stream stream;
                WebResponse result = webRequest.GetResponse();
                stream = result.GetResponseStream();
                DataContractJsonSerializer dataJson = new DataContractJsonSerializer(type);
                rt = (bool)dataJson.ReadObject(stream);
            }
            catch (System.Exception ex)
            {

            }
            return rt;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="AppName">项目代码</param>
        /// <param name="uid">用户id</param>
        /// <param name="p">密码</param>
        /// <param name="roletype">角色</param>
        /// <param name="s">登录类型</param>
        /// <param name="ipmac">ip和mac地址</param>
        /// <returns></returns>
        public Loginresult LoginUser(string AppName, string uid, string p, string roletype, int s, string ipmac)
        {
            string strUrlFormat = "http://{0}:{1}/GetRDDataJson?a={3}&b={4}&c=LoginJson&d=AppName={2};roletype={5};s={6};ipmac={7}";
            string strUrl = string.Format(strUrlFormat,
                UserRightHost,
                UserRightPort,
                System.Web.HttpUtility.UrlEncode(AppName.ToLower()),
                System.Web.HttpUtility.UrlEncode(uid),
                System.Web.HttpUtility.UrlEncode(p),
                System.Web.HttpUtility.UrlEncode(roletype),
                s,
                System.Web.HttpUtility.UrlEncode(ipmac)
                );
            List<MRDDataAll> ld = (List<MRDDataAll>)GetDataFromWebService(strUrl, typeof(List<MRDDataAll>));
            
            Loginresult rd = new Loginresult();
            rd.Resultid = -3;
            if (ld != null && ld.Count > 0)
            {
                List<Object> ol = DataReflect.FromContractData(ld, rd.GetType());
                rd = (Loginresult)ol[0];
            }
            return rd;
        }

        /// <summary>
        /// 注销用户
        /// </summary>
        /// <param name="sid">会话id</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public ResultData LoginOutUser(string sid, string pwd)
        {
            string strUrlFormat = "http://{0}:{1}/GetRDDataJson?a=1&b=1&c=LoginOutUserJson&d=sid={2};p={3}";
            string strUrl = string.Format(strUrlFormat,
                UserRightHost,
                UserRightPort,
                System.Web.HttpUtility.UrlEncode(sid),
                System.Web.HttpUtility.UrlEncode(pwd)
                );
            List<MRDDataAll> ld = (List<MRDDataAll>)GetDataFromWebService(strUrl, typeof(List<MRDDataAll>));
            ResultData rd = new ResultData();
            if (ld != null && ld.Count > 0)
            {
                List<Object> ol = DataReflect.FromContractData(ld, rd.GetType());
                rd = (ResultData)ol[0];
            }
            return rd;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="sid">会话id</param>
        /// <param name="pwd">密码</param>
        /// <param name="newpwd">新密码</param>
        /// <param name="appname">项目代码</param>
        /// <returns></returns>
        public ResultData ChangeSelfPwd(string sid, string pwd, string newpwd, string appname, string username)
        {
            string strUrlFormat = "http://{0}:{1}/GetRDDataJson?a={6}&b={3}&c=ChangePwdJson&d=sid={2};newpwd={4};AppName={5};";
            string strUrl = string.Format(strUrlFormat,
                UserRightHost,
                UserRightPort,
                System.Web.HttpUtility.UrlEncode(sid),
                System.Web.HttpUtility.UrlEncode(pwd),
                System.Web.HttpUtility.UrlEncode(newpwd),
                System.Web.HttpUtility.UrlEncode(appname.ToLower()),
                System.Web.HttpUtility.UrlEncode(username)
                );
            List<MRDDataAll> ld = (List<MRDDataAll>)GetDataFromWebService(strUrl, typeof(List<MRDDataAll>));
            ResultData rd = new ResultData();
            if (ld != null && ld.Count > 0)
            {
                List<Object> ol = DataReflect.FromContractData(ld, rd.GetType());
                rd = (ResultData)ol[0];
            }
            return rd;
        }

        /// <summary>
        /// 检查会话有效性
        /// </summary>
        /// <param name="sid">会话id</param>
        /// <returns></returns>
        public bool CheckExpriedt(string sid)
        {
            string strUrlFormat = "http://{0}:{1}/CheckExpriedtJson?sid={2}";
            string strUrl = string.Format(strUrlFormat,
                UserRightHost,
                UserRightPort,
                System.Web.HttpUtility.UrlEncode(sid)
                );
            bool rt = (bool)GetDataFromWebService(strUrl, typeof(bool));
            return rt;
        }

        public User GetUserInfoBySid(string sid)
        {
            string strUrlFormat = "http://{0}:{1}/GetRDDataJson?a=1&b=1&c=getuserinfobysidjson&d=sid={2}";
            string strUrl = string.Format(strUrlFormat,
                UserRightHost,
                UserRightPort,
                System.Web.HttpUtility.UrlEncode(sid)
                );
            List<MRDDataAll> ld = (List<MRDDataAll>)GetDataFromWebService(strUrl, typeof(List<MRDDataAll>));
            User rd = new User();
            if (ld != null && ld.Count > 0)
            {
                List<Object> ol = DataReflect.FromContractData(ld, rd.GetType());
                rd = (User)ol[0];
            }
            return rd;
        }

        /// <summary>
        /// 修改个人信息
        /// </summary>
        /// <param name="sid">会话id</param>
        /// <param name="pwd">密码</param>
        /// <param name="appName">项目代码</param>
        /// <param name="email">邮箱</param>
        /// <param name="mobliepin">移动编码</param>
        /// <param name="pwdquetion">密码找回问题</param>
        /// <param name="pwdanswer">密码找回答案</param>
        /// <param name="mobilealias">移动别名</param>
        /// <returns></returns>
        public ResultData UpdateSelf(string sid, string pwd, string appName, string email, string mobliepin, string pwdquetion, string pwdanswer, string mobilealias)
        {
            string strUrlFormat = "http://{0}:{1}/GetRDDataJson?a=1&b=1&c=updateselfjson&d=sid={2};pwd={3};AppName={4};email={5};mobliepin={6};pwdquetion={7};pwdanswer={8};mobilealias={9}";
            string strUrl = string.Format(strUrlFormat,
                UserRightHost,
                UserRightPort,
                System.Web.HttpUtility.UrlEncode(sid),
                System.Web.HttpUtility.UrlEncode(pwd),
                System.Web.HttpUtility.UrlEncode(appName.ToLower()),
                System.Web.HttpUtility.UrlEncode(email),
                System.Web.HttpUtility.UrlEncode(mobliepin),
                System.Web.HttpUtility.UrlEncode(pwdquetion),
                System.Web.HttpUtility.UrlEncode(pwdanswer),
                System.Web.HttpUtility.UrlEncode(mobilealias)
                );
            List<MRDDataAll> ld = (List<MRDDataAll>)GetDataFromWebService(strUrl, typeof(List<MRDDataAll>));
            ResultData rd = new ResultData();
            if (ld != null && ld.Count > 0)
            {
                List<Object> ol = DataReflect.FromContractData(ld, rd.GetType());
                rd = (ResultData)ol[0];
            }
            return rd;
        }

        /// <summary>
        /// 导入用户
        /// </summary>
        /// <param name="sid">会话id</param>
        /// <param name="pwd">密码</param>
        /// <param name="appName">项目代码</param>
        /// <param name="RoleName">角色</param>
        /// <param name="UserName">用户名</param>
        /// <param name="urpwd">会话id密码</param>
        /// <param name="extendparam">拓展参数</param>
        /// <returns></returns>
        public ResultData ImportUser(string sid, string pwd, string appName, string RoleName, string UserName, string urpwd, string extendparam)
        {
            extendparam = extendparam.Replace(";","");
            extendparam = extendparam.Replace("=", "");
            string strUrlFormat = "http://{0}:{1}/GetRDDataJson?a=1&b=1&c=importuser&d=sid={2};pwd={3};AppName={4};UserName={5};urpwd={6};RoleName={7};extendparam={8}";
            string strUrl = string.Format(strUrlFormat,
                 UserRightHost,
                 UserRightPort,
                 System.Web.HttpUtility.UrlEncode(sid),
                 System.Web.HttpUtility.UrlEncode(pwd),
                 System.Web.HttpUtility.UrlEncode(appName.ToLower()),
                 System.Web.HttpUtility.UrlEncode(UserName),
                 System.Web.HttpUtility.UrlEncode(urpwd),
                 System.Web.HttpUtility.UrlEncode(RoleName),
                 System.Web.HttpUtility.UrlEncode(extendparam)
                 );

            List<MRDDataAll> ld = (List<MRDDataAll>)GetDataFromWebService(strUrl, typeof(List<MRDDataAll>));
            ResultData rd = new ResultData();
            if (ld != null && ld.Count > 0)
            {
                List<Object> ol = DataReflect.FromContractData(ld, rd.GetType());
                rd = (ResultData)ol[0];
            }
            return rd;
        }

        /// <summary>
        /// /// 获取项目中指定角色的用户名
        /// </summary>
        /// <param name="appName">项目代码</param>
        /// <param name="RoleName">角色</param>
        /// <returns></returns>
        public List<User> GetMatchUserList(string appName,string RoleName)
        {
            string strUrlFormat = "http://{0}:{1}/GetRDDataJson?a=1&b=1&c=GetMatchUsersJson&d=AppName={2};RoleName={3}";
            string strUrl = string.Format(strUrlFormat,
                UserRightHost,
                UserRightPort,
                System.Web.HttpUtility.UrlEncode(appName.ToLower()),
                System.Web.HttpUtility.UrlEncode(RoleName.ToLower())
                );
            List<MRDDataAll> ld = (List<MRDDataAll>)GetDataFromWebService(strUrl, typeof(List<MRDDataAll>));
            List<User> rd = new List<User>();
            if (ld != null && ld.Count > 0)
            {
                List<Object> ol = DataReflect.FromContractData(ld, typeof(User));
                for (int i = 0; i < ol.Count; i++)
                {
                    rd.Add((User)ol[i]);
                }
            }
            return rd;
        }

        /// <summary>
        /// 获取项目中指定角色的用户名
        /// </summary>
        /// <param name="appName">项目代码</param>
        /// <param name="RoleName">角色</param>
        /// <returns></returns>
        public Dictionary<String, String> GetAppUserName(string appName, string RoleName)
        {
            string strUrlFormat = "http://{0}:{1}/GetRDDataJson?a=1&b=1&c=getusername&d=AppName={2};RoleName={3}";
            string strUrl = string.Format(strUrlFormat,
                UserRightHost,
                UserRightPort,
                System.Web.HttpUtility.UrlEncode(appName.ToLower()),
                System.Web.HttpUtility.UrlEncode(RoleName.ToLower())
                );
            List<MRDDataAll> ld = (List<MRDDataAll>)GetDataFromWebService(strUrl, typeof(List<MRDDataAll>));
            Dictionary<String, String> rd = new Dictionary<String, String>();
            if (ld != null && ld.Count > 0)
            {
                List<Object> ol = DataReflect.FromContractData(ld, typeof(String));
                for (int i = 0; i < ol.Count; i++)
                {
                    rd.Add((String)ol[i], (String)ol[i]);
                }
            }
            return rd;
        }

        /// <summary>
        /// 获取该项目的所有角色名称
        /// </summary>
        /// <param name="appName">项目代码</param>
        /// <returns></returns>
        public List<string> GetRoleSelect(string appName)
        {
            string strUrlFormat = "http://{0}:{1}/GetRDDataJson?a=1&b=1&c=getroleselect&d=U.AppName={2}";
            string strUrl = string.Format(strUrlFormat,
                UserRightHost,
                UserRightPort,
                System.Web.HttpUtility.UrlEncode(appName.ToLower())
                );
            List<MRDDataAll> ld = (List<MRDDataAll>)GetDataFromWebService(strUrl, typeof(List<MRDDataAll>));
            List<string> rd = new List<string>();
            if (ld != null && ld.Count > 0)
            {
                List<object> ol = DataReflect.FromContractData(ld, typeof(string));
                for (int i = 0; i < ol.Count; i++)
                {
                    rd.Add((string)ol[i]);
                }
            }
            return rd;
        }

        public ResultData UpdateActive(string sid, string pwd, string appname, string uid, string status)
        {
            string strUrlFormat = "http://{0}:{1}/GetRDDataJson?a=1&b=1&c=updateactive&d=sid={2};pwd={3};AppName={4};uid={5};status={6}";
            string strUrl = string.Format(strUrlFormat,
                UserRightHost,
                UserRightPort,
                System.Web.HttpUtility.UrlEncode(sid),
                System.Web.HttpUtility.UrlEncode(pwd),
                System.Web.HttpUtility.UrlEncode(appname.ToLower()),
                System.Web.HttpUtility.UrlEncode(uid),
                System.Web.HttpUtility.UrlEncode(status)
                );
            List<MRDDataAll> ld = (List<MRDDataAll>)GetDataFromWebService(strUrl, typeof(List<MRDDataAll>));
            ResultData rd = new ResultData();
            List<Object> ol = DataReflect.FromContractData(ld, rd.GetType());
            rd = (ResultData)ol[0];
            return rd;
        }

        public ResultData UpdateLocked(string sid, string pwd, string appname, string uid, string status)
        {
            string strUrlFormat = "http://{0}:{1}/GetRDDataJson?a=1&b=1&c=updatelocked&d=sid={2};pwd={3};AppName={4};uid={5};status={6}";
            string strUrl = string.Format(strUrlFormat,
                UserRightHost,
                UserRightPort,
                System.Web.HttpUtility.UrlEncode(sid),
                System.Web.HttpUtility.UrlEncode(pwd),
                System.Web.HttpUtility.UrlEncode(appname.ToLower()),
                System.Web.HttpUtility.UrlEncode(uid),
                System.Web.HttpUtility.UrlEncode(status)
                );
            List<MRDDataAll> ld = (List<MRDDataAll>)GetDataFromWebService(strUrl, typeof(List<MRDDataAll>));
            ResultData rd = new ResultData();
            List<Object> ol = DataReflect.FromContractData(ld, rd.GetType());
            rd = (ResultData)ol[0];
            return rd;
        }
       
        /// <summary>
        /// 获取所有项目名称
        /// </summary>
        /// <returns></returns>
        public List<String> GetAllAppName()
        {
            List<String> AppList = new List<String>();
            string strUrlFormat = "http://{0}:{1}/GetAllAppNameJson";
            string strUrl = string.Format(strUrlFormat,
                UserRightHost,
                UserRightPort
                );
            AppList = (List<String>)GetDataFromWebService(strUrl, typeof(List<String>));

            return AppList;
        }

        /// <summary>
        /// 获取项目业务数据
        /// </summary>
        /// <param name="appname">项目代码</param>
        /// <returns></returns>
        public List<BusinessInfo> GetBusiness(string appname)
        {
            string strUrlFormat = "http://{0}:{1}/GetRDDataJson?a=1&b=1&c=businesslist&d=businesslist.AppName={2}";
            string strUrl = string.Format(strUrlFormat,
                UserRightHost,
                UserRightPort,
                System.Web.HttpUtility.UrlEncode(appname.ToLower())
                );
            List<MRDDataAll> ld = (List<MRDDataAll>)GetDataFromWebService(strUrl, typeof(List<MRDDataAll>));
            List<BusinessInfo> rd = new List<BusinessInfo>();
            if (ld != null)
            {
                List<Object> ol = DataReflect.FromContractData(ld, typeof(BusinessInfo));
                if (ol != null)
                    for (int i = 0; i < ol.Count; i++)
                    {
                        rd.Add((BusinessInfo)ol[i]);
                    }
            }
            return rd;
        }

        /// <summary>
        /// 添加业务
        /// </summary>
        /// <param name="KeyID">ID</param>
        /// <param name="Desc">业务描述</param>
        /// <param name="Role">角色</param>
        /// <param name="rcufunction">角色功能点</param>
        /// <param name="ucnfunction">用户功能点</param>
        /// <param name="cmufunction">过滤功能点</param>
        /// <param name="appname">项目代码</param>
        /// <param name="businessname">业务名称</param>
        /// <param name="showorder">显示序号</param>
        /// <returns></returns>
        public ResultData AddRoleBusiness(string KeyID, string Desc, string Role, string rcufunction, string ucnfunction, string cmufunction, string appname, string parent,string businessname,int showorder,string userextendparam)
        {
            string strUrlFormat = "http://{0}:{1}/GetRDDataJson?a=1&b=1&c=addrolebusiness&d=keyid={2};desc={3};role={4};rcufunction={5};ucnfunction={6};cmufunction={7};AppName={8};parent={9};businessname={10};showorder={11};userextendparam={12}";
            string strUrl = string.Format(strUrlFormat,
                UserRightHost,
                UserRightPort,
                System.Web.HttpUtility.UrlEncode(KeyID),
                System.Web.HttpUtility.UrlEncode(Desc),
                System.Web.HttpUtility.UrlEncode(Role),
                System.Web.HttpUtility.UrlEncode(rcufunction),
                System.Web.HttpUtility.UrlEncode(ucnfunction),
                 System.Web.HttpUtility.UrlEncode(cmufunction),
                 System.Web.HttpUtility.UrlEncode(appname.ToLower()),
                 System.Web.HttpUtility.UrlEncode(parent),
                  System.Web.HttpUtility.UrlEncode(businessname),
                  System.Web.HttpUtility.UrlEncode(showorder.ToString()),
                 System.Web.HttpUtility.UrlEncode(userextendparam) 
                );
            List<MRDDataAll> ld = (List<MRDDataAll>)GetDataFromWebService(strUrl, typeof(List<MRDDataAll>));
            ResultData rd = new ResultData();
            List<Object> ol = DataReflect.FromContractData(ld, rd.GetType());
            rd = (ResultData)ol[0];
            return rd;
        }

        /// <summary>
        /// 删除业务
        /// </summary>
        /// <param name="desc">业务描述</param>
        /// <param name="usertpye">角色</param>
        /// <param name="appname">项目代码</param>
        /// <param name="businessname">业务名称</param>
        /// <returns></returns>
        public ResultData DeleteRoleBusiness(string desc, string usertpye, string appname,string businessname)
        {
            string strUrlFormat = "http://{0}:{1}/GetRDDataJson?a=1&b=1&c=deleterolebusiness&d=des={2};usertype={3};AppName={4};businessname={5}";
            string strUrl = string.Format(strUrlFormat,
                UserRightHost,
                UserRightPort,
                System.Web.HttpUtility.UrlEncode(desc),
                System.Web.HttpUtility.UrlEncode(usertpye),
                System.Web.HttpUtility.UrlEncode(appname.ToLower()),
                System.Web.HttpUtility.UrlEncode(businessname)
                );
            List<MRDDataAll> ld = (List<MRDDataAll>)GetDataFromWebService(strUrl, typeof(List<MRDDataAll>));
            ResultData rd = new ResultData();
            List<Object> ol = DataReflect.FromContractData(ld, rd.GetType());
            rd = (ResultData)ol[0];
            return rd;
        }

        /// <summary>
        /// 修改业务
        /// </summary>
        /// <param name="keyid">id</param>
        /// <param name="desc">业务描述</param>
        /// <param name="olddesc">久的业务描述</param>
        /// <param name="usertpye">角色</param>
        /// <param name="rcufunction">角色功能点</param>
        /// <param name="ucnfunction">用户功能点</param>
        /// <param name="cmufunction">过滤功能点</param>
        /// <param name="appname">项目代码</param>
        /// <param name="businessname">业务名称</param>
        /// <param name="showorder">显示序号</param>
        /// <param name="parentNode">父菜单</param>
        /// <returns></returns>
        public ResultData UpdateRoleBusiness(string keyid, string desc, string olddesc, string usertpye, string rcufunction, string ucnfunction, string cmufunction, string appname, string businessname, int showorder, string parentNode,string userextendparam)
        {
            string strUrlFormat = "http://{0}:{1}/GetRDDataJson?a=1&b=1&c=updaterolebusiness&d=keyid={2};desc={3};olddesc={4};usertype={5};rcufunction={6};ucnfunction={7};cmufunction={8};AppName={9};businessname={10};showorder={11};parentname={12};userextendparam={13}";
            string strUrl = string.Format(strUrlFormat,
                UserRightHost,
                UserRightPort,
                System.Web.HttpUtility.UrlEncode(keyid),
                System.Web.HttpUtility.UrlEncode(desc),
                System.Web.HttpUtility.UrlEncode(olddesc),
                System.Web.HttpUtility.UrlEncode(usertpye),
                System.Web.HttpUtility.UrlEncode(rcufunction),
                System.Web.HttpUtility.UrlEncode(ucnfunction),
                System.Web.HttpUtility.UrlEncode(cmufunction),
                System.Web.HttpUtility.UrlEncode(appname.ToLower()),
                System.Web.HttpUtility.UrlEncode(businessname),
                System.Web.HttpUtility.UrlEncode(showorder.ToString()),
                System.Web.HttpUtility.UrlEncode(parentNode),
                System.Web.HttpUtility.UrlEncode(userextendparam)
                );
            List<MRDDataAll> ld = (List<MRDDataAll>)GetDataFromWebService(strUrl, typeof(List<MRDDataAll>));
            ResultData rd = new ResultData();
            List<Object> ol = DataReflect.FromContractData(ld, rd.GetType());
            rd = (ResultData)ol[0];
            return rd;
        }

        public ResultData AddBlankMenu(string menuName,string showOrder,string appName,string userType)
        {
            string strUrlFormat = "http://{0}:{1}/GetRDDataJson?a=1&b=1&c=addblankmenu&d=menuname={2};showorder={3};appname={4};usertype={5}";
            string strUrl = string.Format(strUrlFormat,
                UserRightHost,
                UserRightPort,
                System.Web.HttpUtility.UrlEncode(menuName),
                System.Web.HttpUtility.UrlEncode(showOrder),
                System.Web.HttpUtility.UrlEncode(appName),
                System.Web.HttpUtility.UrlEncode(userType)
                );
            List<MRDDataAll> ld = (List<MRDDataAll>)GetDataFromWebService(strUrl, typeof(List<MRDDataAll>));
            ResultData rd = new ResultData();
            List<Object> ol = DataReflect.FromContractData(ld, rd.GetType());
            rd = (ResultData)ol[0];
            return rd;
        }

        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="appname">项目代码</param>
        /// <param name="role">角色名称</param>
        /// <returns></returns>
        public List<RoleBInfo> GetRoleBInfo(string appname, string role)
        {
            string strUrlFormat = "http://{0}:{1}/GetRDDataJson?a=1&b=1&c=rolebinfolist&d=rolebinfolist.AppName={2};role={3}";
            string strUrl = string.Format(strUrlFormat,
                UserRightHost,
                UserRightPort,
                System.Web.HttpUtility.UrlEncode(appname.ToLower()),
                System.Web.HttpUtility.UrlEncode(role)
                );
            List<MRDDataAll> ld = (List<MRDDataAll>)GetDataFromWebService(strUrl, typeof(List<MRDDataAll>));
            List<RoleBInfo> rd = new List<RoleBInfo>();
            List<Object> ol = DataReflect.FromContractData(ld, typeof(RoleBInfo));
            for (int i = 0; i < ol.Count; i++)
            {
                rd.Add((RoleBInfo)ol[i]);
            }
            return rd;
        }

        /// <summary>
        /// 获取下拉列表的第一个数据
        /// </summary>
        /// <param name="columnname">字段名称</param>
        /// <param name="url">获取数据url</param>
        /// <param name="method">方法</param>
        /// <param name="paramstr">参数</param>
        /// <returns></returns>
        public string GetFirstItem(string columnname,string paramstr)
        {
            string appname = ((Loginresult)HttpContext.Current.Session["Session"]).AppName;
            string rolename = ((Loginresult)HttpContext.Current.Session["Session"]).RoleName;
            string URappname = ReadConfig.TheReadConfig["URAppName"].ToLower();
            List<String> ld = WSUtil.GetInputDataFromWS(paramstr,appname,rolename);
            List<String> rd = new List<String>();
            if (columnname.ToLower() != "appname")
            {
                for (int i = 0; i < ld.Count; i++)
                {
                    rd.Add(ld[i]);
                }
            }
            else
            {
                if (appname == URappname)
                {
                    for (int i = 0; i < ld.Count; i++)
                    {
                        rd.Add(ld[i]);
                    }
                }
                else
                {
                    rd.Add(appname);
                }
            }
            if(rd.Count>0)
                return rd[0];
            else
                return "";
        }


        public string GetFirstItemWithSrc(string columnname, string paramstr)
        {
            string appname = ((Loginresult)HttpContext.Current.Session["Session"]).AppName;
            string rolename = ((Loginresult)HttpContext.Current.Session["Session"]).RoleName;
            string URappname = ReadConfig.TheReadConfig["URAppName"].ToLower();
            List<String> ld = WSUtil.GetInputDataFromWSwithSRC(paramstr, appname, rolename);
            List<String> rd = new List<String>();
            if (columnname.ToLower() != "appname")
            {
                for (int i = 0; i < ld.Count; i++)
                {
                    rd.Add(ld[i]);
                }
            }
            else
            {
                if (appname == URappname)
                {
                    for (int i = 0; i < ld.Count; i++)
                    {
                        rd.Add(ld[i]);
                    }
                }
                else
                {
                    rd.Add(appname);
                }
            }
            if (rd.Count > 0)
                return rd[0];
            else
                return "";
        }
        /// <summary>
        /// 通用操作数据
        /// </summary>
        /// <param name="Method">操作方法名称</param>
        /// <param name="BusinessName">业务名称</param>
        /// <param name="ComponentName">组件名称</param>
        /// <param name="Param">参数</param>
        /// <returns></returns>
        public ResultData CommonMethod(string Method,string BusinessName,string ComponentName, string Param)
        {
            ServiceInfo si = ServiceConfig.GetInfo(UTDtCnvrt.EnumServiceFlag.businessservice);            
            string strUrlFormat = "http://{0}:{1}/GetRDDataJson?a=1&b=1&c=bsusrctrl&d=SourceMethod={2};BusinessName={3};ComponentName={4};{5}";
            string strUrl = string.Format(strUrlFormat,
                si.ip,
                si.port,
                Method,
                BusinessName,
                ComponentName,
                Param
                );
            List<MRDDataAll> ld = (List<MRDDataAll>)GetDataFromWebService(strUrl, typeof(List<MRDDataAll>));
            ResultData rd = new ResultData();
            if (ld != null)
            {
                List<Object> ol = DataReflect.FromContractData(ld, rd.GetType());
                rd = (ResultData)ol[0];
            }
            return rd;
        }

       /// <summary>
       /// 获取项目的用户名
       /// </summary>
       /// <param name="appname">项目代码</param>
       /// <returns></returns>
        public List<String> GetProjectUser(string appname)
        {
            string strUrlFormat = "http://{0}:{1}/GetRDDataJson?a=1&b=1&c=getallappname&d=";
            ServiceInfo si = ServiceConfig.GetInfo(UTDtCnvrt.EnumServiceFlag.businessservice);
            string strUrl = string.Format(strUrlFormat,
                si.ip,
                si.port
                );
            List<MRDDataAll> ld = (List<MRDDataAll>)GetDataFromWebService(strUrl, typeof(List<MRDDataAll>));
            List<String> rd = new List<String>();
            List<Object> ol = DataReflect.FromContractData(ld, typeof(String));
            for (int i = 0; i < 23; i++)
            {
                rd.Add("username=user" + i + ";pwd=pwd" + i + ";rolename=user;remark=comment;exist=否;");
            }
            return rd;
        }

        /// <summary>
        /// 获取指定项目的导入用户名
        /// </summary>
        /// <param name="appname">项目代码</param>
        /// <returns></returns>
        public List<String> GetImportUser(string appname)
        {
            string strUrlFormat = "http://{0}:{1}/GetRDDataJson?a=1&b=1&c=getimportuser&d=appname={2}";
            ServiceInfo si = ServiceConfig.GetInfo(UTDtCnvrt.EnumServiceFlag.userrightservice);
            string strUrl = string.Format(strUrlFormat,
                si.ip,
                si.port,
                appname
                );
            List<MRDDataAll> ld = (List<MRDDataAll>)GetDataFromWebService(strUrl, typeof(List<MRDDataAll>));
            List<String> rd = new List<String>();
            List<Object> ol = DataReflect.FromContractData(ld, typeof(String));
            if(ol!=null)
            for (int i = 0; i < ol.Count; i++)
            {
                rd.Add((String)ol[i]);
            }
            return rd;
        }
       
        /// <summary>
        /// 通知服务刷新
        /// </summary>
        public static void refeshService()
        {
            ServiceInfo si = ServiceConfig.GetInfo(UTDtCnvrt.EnumServiceFlag.userrightservice);
            string strUrlFormat = "http://{0}:{1}/GetRDDataJson?a=1&b=1&c=refresh&d={2}";
            string strUrl = string.Format(strUrlFormat,
                si.ip,
                si.port,
                EnumServiceFlag.netservice.ToString()
                );
            List<MRDDataAll> ltdata = null;
            string errorMessage = "";
            ltdata = (List<MRDDataAll>)UTUtil.WebServiceUtil.GetObjectFromWebService(strUrl, typeof(List<MRDDataAll>), out errorMessage);

            si = ServiceConfig.GetInfo(UTDtCnvrt.EnumServiceFlag.basesservice);
            strUrlFormat = "http://{0}:{1}/GetRDDataJson?a=1&b=1&c=refresh&d={2}";
            strUrl = string.Format(strUrlFormat,
                si.ip,
                si.port,
                EnumServiceFlag.netservice.ToString()
                );
            ltdata = (List<MRDDataAll>)UTUtil.WebServiceUtil.GetObjectFromWebService(strUrl, typeof(List<MRDDataAll>), out errorMessage);

            si = ServiceConfig.GetInfo(UTDtCnvrt.EnumServiceFlag.businessservice);
            strUrlFormat = "http://{0}:{1}/GetRDDataJson?a=1&b=1&c=refresh&d={2}";
            strUrl = string.Format(strUrlFormat,
                si.ip,
                si.port,
                EnumServiceFlag.netservice.ToString()
                );
            ltdata = (List<MRDDataAll>)UTUtil.WebServiceUtil.GetObjectFromWebService(strUrl, typeof(List<MRDDataAll>), out errorMessage);
        }

        /// <summary>
        /// 检查会话状态的有效性
        /// </summary>
        /// <param name="SessionId">会话id</param>
        /// <returns></returns>
        public bool CheckSessionState(string SessionId)
        {
            string strUrlFormat = "http://{0}:{1}/GetRDDataJson?a=1&b=1&c=checksessionstate&d=sessionid={2}";
            ServiceInfo si = ServiceConfig.GetInfo(UTDtCnvrt.EnumServiceFlag.userrightservice);
            string strUrl = string.Format(strUrlFormat,
                si.ip,
                si.port,
                SessionId
                );
            List<MRDDataAll> ld = (List<MRDDataAll>)GetDataFromWebService(strUrl, typeof(List<MRDDataAll>));
            ResultData rd = new ResultData();
            List<Object> ol = DataReflect.FromContractData(ld, typeof(ResultData));
            if (ol != null)
            {
                rd = (ResultData)ol[0];
            }

            if (rd.result == 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获取所有相关联的导入项目描述
        /// </summary>
        /// <param name="appname">appname</param>
        /// <returns></returns>
        public List<String> GetImportApp(string appname)
        {
            string strUrlFormat = "http://{0}:{1}/GetRDDataJson?a=1&b=1&c=getimportapplist&d=appname={2}";
            ServiceInfo si = ServiceConfig.GetInfo(UTDtCnvrt.EnumServiceFlag.userrightservice);
            string strUrl = string.Format(strUrlFormat,
                si.ip,
                si.port,
                appname
                );
            List<MRDDataAll> ld = (List<MRDDataAll>)GetDataFromWebService(strUrl, typeof(List<MRDDataAll>));
            List<String> rd = new List<String>();
            List<Object> ol = DataReflect.FromContractData(ld, typeof(String));
            if (ol != null)
                for (int i = 0; i < ol.Count; i++)
                {
                    rd.Add((String)ol[i]);
                }
            return rd;
        }

        public List<NodeRightData> GetImportUserRight(string appname,string businessname)
        {
            string strUrlFormat = "http://{0}:{1}/GetRDDataJson?a=1&b=1&c=businessright&d=appname={2};businessname={3}";
            ServiceInfo si = ServiceConfig.GetInfo(UTDtCnvrt.EnumServiceFlag.userrightservice);
            string strUrl = string.Format(strUrlFormat,
                si.ip,
                si.port,
                appname,
                businessname
                );
            List<MRDDataAll> ld = (List<MRDDataAll>)GetDataFromWebService(strUrl, typeof(List<MRDDataAll>));
            List<NodeRightData> rd = new List<NodeRightData>();
            List<Object> ol = DataReflect.FromContractData(ld, typeof(NodeRightData));
            if (ol != null)
                for (int i = 0; i < ol.Count; i++)
                {
                    rd.Add((NodeRightData)ol[i]);
                }
            return rd;
        }

        public ResultData AddUserRight(string appname, string username, string businessname,string rights)
        {
            string strUrlFormat = "http://{0}:{1}/GetRDDataJson?a=1&b=1&c=adduserright&d=appname={2};username={3};businessname={4};rights={5}";
            ServiceInfo si = ServiceConfig.GetInfo(UTDtCnvrt.EnumServiceFlag.userrightservice);
            string strUrl = string.Format(strUrlFormat,
                si.ip,
                si.port,
                appname,
                username,
                businessname,
                rights
                );
            List<MRDDataAll> ld = (List<MRDDataAll>)GetDataFromWebService(strUrl, typeof(List<MRDDataAll>));
            ResultData rd = new ResultData();
            if (ld != null)
            {
                List<Object> ol = DataReflect.FromContractData(ld, rd.GetType());
                rd = (ResultData)ol[0];
            }
            return rd;
        }

        public string GetUserRight(string appname, string username, string businessname)
        {
            string strUrlFormat = "http://{0}:{1}/GetRDDataJson?a=1&b=1&c=getuserright&d=appname={2};username={3};businessname={4}";
            ServiceInfo si = ServiceConfig.GetInfo(UTDtCnvrt.EnumServiceFlag.userrightservice);
            string strUrl = string.Format(strUrlFormat,
                si.ip,
                si.port,
                appname,
                username,
                businessname
                );
            List<MRDDataAll> ld = (List<MRDDataAll>)GetDataFromWebService(strUrl, typeof(List<MRDDataAll>));
            string rd = "";
            if (ld != null)
            {
                List<Object> ol = DataReflect.FromContractData(ld, rd.GetType());
                rd = (string)ol[0];
            }
            return rd;
        }


        public ResultData DeleteUserRight(string appname, string username, string businessname)
        {
            string strUrlFormat = "http://{0}:{1}/GetRDDataJson?a=1&b=1&c=deleteuserright&d=appname={2};username={3};businessname={4}";
            ServiceInfo si = ServiceConfig.GetInfo(UTDtCnvrt.EnumServiceFlag.userrightservice);
            string strUrl = string.Format(strUrlFormat,
                si.ip,
                si.port,
                appname,
                username,
                businessname
                );
            List<MRDDataAll> ld = (List<MRDDataAll>)GetDataFromWebService(strUrl, typeof(List<MRDDataAll>));
            ResultData rd = new ResultData();
            if (ld != null)
            {
                List<Object> ol = DataReflect.FromContractData(ld, rd.GetType());
                rd = (ResultData)ol[0];
            }
            return rd;
        }

        public ResultData UpdateUserRight(string appname, string username, string businessname, string rights)
        {
            string strUrlFormat = "http://{0}:{1}/GetRDDataJson?a=1&b=1&c=updateuserright&d=appname={2};username={3};businessname={4};rights={5}";
            ServiceInfo si = ServiceConfig.GetInfo(UTDtCnvrt.EnumServiceFlag.userrightservice);
            string strUrl = string.Format(strUrlFormat,
                si.ip,
                si.port,
                appname,
                username,
                businessname,
                rights
                );
            List<MRDDataAll> ld = (List<MRDDataAll>)GetDataFromWebService(strUrl, typeof(List<MRDDataAll>));
            ResultData rd = new ResultData();
            if (ld != null)
            {
                List<Object> ol = DataReflect.FromContractData(ld, rd.GetType());
                rd = (ResultData)ol[0];
            }
            return rd;
        }
    }
}