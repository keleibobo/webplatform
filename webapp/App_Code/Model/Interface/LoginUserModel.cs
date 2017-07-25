using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Collections;
using System.Data.OleDb;
using System.Data;

/// <summary>
///LoginUser 的摘要说明
/// </summary>
namespace AppCode
{

    public static class LoginUserModel
    {
        //public LoginUserModel()
        //{
        //    DatabaseBussiness = enumDatabaseBussiness.MEMBERSHIP;
        //}

        /// <summary>
        /// 获得项目名称
        /// </summary>
        /// <returns></returns>
        private static string _title = "";
        public static string Title
        {
            get
            {
                if (_title.Length == 0)
                {
                    _title = AppParaModel.GetDataAppDesc(AppName);
                }
                return _title;
            }
        }

        /// <summary>
        /// 取得或设置当前的登录用户名,不能在构造函数中调用
        /// </summary>
        public static string UserName
        {
            get
            {
                object o = "";
                try
                {
                    o = HttpContext.Current.Session["username"];
                }
                catch { }

                return o == null ? "" : o.ToString();
            }
            set
            {
                HttpContext.Current.Session["username"] = value;
            }
        }

        private static string _strRoles = null;



        public static string Roles
        {
            get
            {
                Object roles = "";
                if (_strRoles == null)
                {

                    if (HttpContext.Current.Session == null) roles = "user";
                    else
                        roles = HttpContext.Current.Session["RoleType"];
                    if (roles == null)
                    {
                        roles = "user";
                    }
                    _strRoles = roles.ToString();
                }

                return _strRoles;
            }

            set
            {
                _strRoles = value;
            }


        }


        private static string _AppName = "";
        public static string AppName
        {
            get
            {
                if (_AppName.Length == 0)
                {
                    _AppName = ReadConfig.TheReadConfig["appname"].Trim().ToLower();
                }
                return _AppName;
            }
            set
            {
                _AppName = value.Trim().ToLower();
            }
        }

        private static string _Pwd = "ee11cbb19052e40b07aac0ca060c23ee";
        public static string Pwd
        {
            get
            {
                return _Pwd;
            }
            set
            {
                _Pwd = value;
            }
        }

        //private static string _treePage="";
        //public static string TreePage
        //{
        //    get
        //    {
        //        return _treePage;
        //    }
        //    set
        //    {
        //        _treePage = value;
        //    }
        //}
    }
}