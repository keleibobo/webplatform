using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using UTDtBaseSvr;
using System.Collections;
using System.Data;
using System.Text;

/// <summary>
///UserMenuModel用于菜单生成
/// </summary>
namespace AppCode
{
    public class UserMenuModel
    {
        public string menuTable = "";
        public int mainCount = 0;

        //UserMenuInfo
        Dictionary<String, List<object>> menuinfos = new Dictionary<string, List<object>>();
        public string _appname = "";
        public string _roles;
        public string roles
        {
            set
            {
                _roles = value;
                init(_roles);

            }
            get { return _roles; }

        }

        private string _msgTable = "<td width='100px' id='tdmsg'></td>";

        public string msgTable
        {
            set
            {
                _msgTable = value;
            }

            get
            {
                if (true)//
                {
                    _msgTable = setMSG();
                    ///f_loadmsg(cid)
                }
                return _msgTable;
            }
        }

        public UserMenuModel(string appname)
        {
            this._appname = appname;
        }

        private void init(String roles)
        {
            TableNav tableNav = new TableNav(_appname, roles);
            menuTable = tableNav.nav;
            //    ULNav ulNav = new ULNav(_appname, roles);
            //  menuTable ="<div align='center' width='80%'>"+ulNav.nav+"</div>";
        }


        public String setMSG()
        {
            String msg = "";

            Dictionary<string, object> mappingparam = InitModel.getParamInfo();
            String e = _appname + "_" + "alarmevent";
            if (mappingparam == null || !mappingparam.ContainsKey(e) || !mappingparam[e].Equals("true"))
            {
                return msg;
            }

            //             msg = @"<td width='3%' align='center' id='tdmsg'>
            //                                <table width='50' height='32' border='0' cellpadding='0' cellspacing='0'>
            //                                    <tr>
            //                                        <td width='55%' align='center' valign='middle'></td>
            //                                        <td></td>
            //                                        <td height='4'></td>
            //                                    </tr>
            //                                    <tr>
            //                                        <td width='55%' align='center' valign='middle'><a href='#' onmouseout='MM_swapImgRestore()'
            //                                            onmouseover=""MM_swapImage('Image14','','../images/信息.gif',1)"">
            //                                            <img src='../images/信息_gray.gif' name='Image14' width='20'  height='20' border='0' alt='提示信息' >
            //                                               </a></td>
            //                                        <td width='5%' height='17' align='center'>&nbsp;</td>
            //                                        <td width='40%' height='17' align='left' valign='middle'><a href='#' class='list5'>0</a></td>
            //                                    </tr>
            //                                    <tr>
            //                                        <td width='55%' align='center' valign='middle'></td>
            //                                        <td height='4'></td>
            //                                        <td height='9'></td>
            //                                    </tr>
            //                                </table>
            //                            </td>
            //                            <td width='1%' align='center'>
            //                                <img src='../images/none.gif' width='10' height='10'></td>";

            msg = @"<td width='3%' align='center' id='tdmsg'>
                                <table width='50' height='32' border='0' cellpadding='0' cellspacing='0'>
                                    <tr>
                                        <td width='55%' align='center' valign='middle'></td>
                                        <td></td>
                                        <td height='4'></td>
                                    </tr>
                                    <tr>
                                        <td width='55%' align='center' valign='middle'></td>
                                        <td width='5%' height='17' align='center'>&nbsp;</td>
                                        <td width='40%' height='17' align='left' valign='middle'></td>
                                    </tr>
                                    <tr>
                                        <td width='55%' align='center' valign='middle'></td>
                                        <td height='4'></td>
                                        <td height='9'></td>
                                    </tr>
                                </table>
                            </td>
                            <td width='1%' align='center'>
                                <img src='../images/none.gif' width='10' height='10' /></td>";


            msg += @" <td width='3%' align='center' id='tdalarm'>
                                <table width='50' height='32' border='0' cellpadding='0' cellspacing='0'>
                                    <tr>
                                        <td width='55%' align='center' valign='middle'></td>
                                        <td></td>
                                        <td height='4'></td>
                                    </tr>
                                    <tr>
                                        <td width='55%' align='center' valign='middle'><a href='#' onmouseout='MM_swapImgRestore()'
                                            onmouseover=""MM_swapImage('Image13','','../images/jsd_警示.gif',1)"">
                                            <img src='../images/jsd_未警示.gif' name='Image13' width='20' height='20' border='0' alt='警示信息' onclick='showEventlist()' />
                                                </a></td>
                                        <td width='5%' height='17' align='center'>&nbsp;</td>
                                        <td width='40%' height='17' align='left' valign='middle'><a href='#' id='EventTimes' class='list5'>0</a></td>
                                    </tr>
                                    <tr>
                                        <td width='55%' align='center' valign='middle'></td>
                                        <td height='4'></td>
                                        <td height='9'></td>
                                    </tr>
                                </table>

                            </td>
                            <td width='1%' align='center'>
                                <img src='../images/none.gif' width='10' height='10' /></td>";

            msg = msg.Replace("../images", "../images/" + ReadConfig.TheReadConfig["style"]);


            ////  BusinessCall bcCall = (BusinessCall)HttpContext.Current.Session["bcCall"];
            //  EnumServiceFlag serviceflag=EnumServiceFlag.basesservice;
            //  ServiceInfo si = ServiceConfig.GetInfo(serviceflag);

            //  String url = String.Format("http://{0}/GetRDDataJSON?a=1&b=1&c=getparaminfo&d=appname={1}", si.url,ReadConfig.TheReadConfig["appname"]);

            //   List<object> paramlist = (List<object>)AppCode.WSUtil.getListFromWS(url, typeof(ParamInfo));

            //  if(paramlist!=null)
            //   foreach (ParamInfo pinfo in paramlist)
            //   {
            //       if (pinfo.paramname.Equals("alarmevent")&&pinfo.paramvalue.Equals("true"))
            //       {
            //           ///
            //           msg += "<script type='text/javascript' >startshow('')</script>";
            //       }
            //   }

            msg += "<script type='text/javascript' >startshow('" + ReadConfig.TheReadConfig["appname"] + "')</script>";
            return msg;
        }

        /// <summary>
        /// 获得角色权限菜单列表
        /// </summary>
        /// <returns></returns>
        public DataView GetUserMenu(string strRole, string appname)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("Name");
            dt.Columns.Add("Des");
            dt.Columns.Add("Link");
            dt.Columns.Add("pageType");
            dt.Columns.Add("KeyId");
            ValidateUserModel vm = new ValidateUserModel();
            DataTable UserMenu = vm.getUserMenu(appname, strRole);
            toDataTable(UserMenu, dt);
            return dt.DefaultView;
        }

        public DataView GetUserMenuCopy()
        {
            DataView dvRt = null;
            //string action = "gettable";
            //object[] args = new object[] { "tablename=UserMenuRight" };
            //object uc = WSUtil.getFromWS(action, args);
            //DataTable dt=DataTableSerializer.DESerialize(uc.ToString());
            // dvRt= WSdatatableModel.getDataViewFromWS("UserMenuRight");

            List<Object> menuList = new List<object>();
            Dictionary<string, Dictionary<string, List<Object>>> appusermenus = InitModel.appusermenu;

            foreach (KeyValuePair<string, Dictionary<string, List<Object>>> kvp in appusermenus)
            {
                Dictionary<string, List<Object>> rolemenus = kvp.Value;
                foreach (KeyValuePair<string, List<Object>> kv in rolemenus)
                {
                    foreach (Object obj in kv.Value)
                    {
                        menuList.Add(obj);
                    }
                }
            }

            //  dvRt = toDataTable(menuList).DefaultView;
            return dvRt;
        }

        public DataTable GetAllData()
        {
            /*   string sql = "select a.id,b.name,a.des,a.userType from usermenu a,UserMenuRight b where a.KeyID=b.KeyID";
               return GetDataTable(sql);
             * */
            return null;
        }

        /// <summary>
        /// 从dt中取部分列信息到新的DataTable
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="result"></param>
        private void toDataTable(DataTable dt, DataTable result)
        {
            foreach (DataRow dtr in dt.Rows)
            {
                DataRow dr = result.NewRow();

                foreach (DataColumn dc in dt.Columns)
                {
                    string name = dc.ColumnName;
                    try
                    {
                        dr[name] = dtr[name];
                    }
                    catch //若列名错误不处理
                    {
                    }
                }
                result.Rows.Add(dr);
            }
        }

        public UserMenuInfo GetHomePage(string appname, string roles)
        {
            UserMenuInfo mainMenuTable = null;
            List<object> menuList = InitModel.GetUserMenuByRoleID(appname, roles);


            Dictionary<String, List<object>> menuinfos = new Dictionary<string, List<object>>();

            if (menuList != null)
                foreach (UserMenuInfo umi in menuList)
                {
                    ///层级
                    string pid = umi.pathlevel;
                    if (menuinfos.ContainsKey(pid))
                    {
                        List<object> infoList = menuinfos[pid];
                        infoList.Add(umi);
                    }
                    else
                    {
                        List<object> infoList = new List<object>();
                        infoList.Add(umi);
                        menuinfos.Add(pid, infoList);
                    }
                }

            if (menuinfos.ContainsKey("1"))
            {
                List<object> menu = menuinfos["1"];

                if (menu != null && menu.Count > 0)
                {
                    menu.Sort(new MenuInfoComparer());

                    mainMenuTable = ((UserMenuInfo)menu[0]);
                }
                if (mainMenuTable.link.Equals(""))
                {
                    string pname = mainMenuTable.name;
                    menu = menuinfos["2"];
                    menu.Sort(new MenuInfoComparer());

                    //     mainMenuTable = ((UserMenuInfo)menu[0]);
                    foreach (UserMenuInfo umi in menu)
                    {
                        if (umi.parentname.Equals(pname))
                        {
                            mainMenuTable = umi;
                            break;
                        }
                    }

                }
            }
            return mainMenuTable;
        }


        public DataTable GetPageType(string type)
        {
            /*        string sql = "";
                    switch (((ConnectionPool)htConnPool[eDatabaseBussiness]).DatabaseType)
                    {
                        case enumDatabaseType.SQLSERVER:
                            sql = "select top 1 b.pagetype From UserMenu a ,UserMenuRight b where a.KeyID=b.KeyID and b.name='" + type + "'";
                            break;
                        case enumDatabaseType.MYSQL:
                            sql = "select b.pagetype From  UserMenu a ,UserMenuRight b where a.KeyID=b.KeyID and b.name='" + type + "' limit 1";
                            break;
                    }
                    return GetDataTable(sql);
             */
            return null;
        }



        /// <summary>
        /// 根据类型和角色模糊获取菜单
        /// </summary>
        /// <param name="type"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public DataTable GetMenuByTypeAndRole(string type, string roles, bool bFuzzySearch)
        {
            string sql = "";

            /*     switch (((ConnectionPool)htConnPool[eDatabaseBussiness]).DatabaseType)
                 {
                     case enumDatabaseType.SQLSERVER:
                         if (bFuzzySearch)
                         {
                             sql = string.Format("select top 1 b.pagetype,b.ViewName,b.link From UserMenu a,UserMenuRight b where a.KeyID=b.KeyID and b.name like '{0}%' and isnull(b.link,'')<>'' and (a.usertype='{1}' or a.usertype='month' )", type, roles);
                         }
                         else
                         {
                             sql = string.Format("select top 1 b.pagetype,b.ViewName,b.link From UserMenu a,UserMenuRight b where a.KeyID=b.KeyID and b.name='{0}' and isnull(b.link,'')<>'' and (a.usertype='{1}' or a.usertype='month' )", type, roles);
                         }
                         break;
                     case enumDatabaseType.MYSQL:
                         if (bFuzzySearch)
                         {
                             sql = string.Format("select b.pagetype,b.ViewName,b.link From UserMenu a,UserMenuRight b where a.KeyID=b.KeyID and b.name like '{0}%' and IFNULL(b.link,'')<>'' and (a.usertype='{1}' or a.usertype='month') limit 1", type, roles);
                         }
                         else
                         {
                             sql = string.Format("select b.pagetype,b.ViewName,b.link From UserMenu a,UserMenuRight b where a.KeyID=b.KeyID and b.name='{0}' and IFNULL(b.link,'')<>'' and (a.usertype='{1}' or a.usertype='month' ) limit 1", type, roles);
                         }
                         break;
                 }
                 return GetDataTable(sql);
             * */
            return null;
        }

        /// <summary>
        /// 根据ID获取菜单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bFuzzySearch"></param>
        /// <returns></returns>
        public DataTable GetMonthRptMenu()
        {
            string sql = "";
            /*      switch (((ConnectionPool)htConnPool[eDatabaseBussiness]).DatabaseType)
                  {
                      case enumDatabaseType.SQLSERVER:
                          sql = "select len(a.id) as ilen,ROW_NUMBER() Over( order by a.id  ) as rowNum,a.* ,b.Name,b.PageType,b.link,b.viewname from UserMenu a ,UserMenuRight b where a.KeyID=b.KeyID and  a.id like '2207%'";
                          break;
                      case enumDatabaseType.MYSQL:
                          sql = "set @mycnt = 0;select LENGTH(a.id) as ilen,(@mycnt := @mycnt + 1) as rowNum,a.*,b.Name,b.PageType,b.link,b.viewname from UserMenu a ,UserMenuRight b where a.KeyID=b.KeyID and  a.id like '2207%' order by id";
                          break;
                  }
                  return GetDataTable(sql);
             */
            return null;
        }

        public DataTable GetAdminMenu()
        {
            string sql = "";
            /*      switch (((ConnectionPool)htConnPool[eDatabaseBussiness]).DatabaseType)
                  {
                      case enumDatabaseType.SQLSERVER:
                          sql = string.Format(@"select len(a.id) as ilen, a.* ,b.Name,b.PageType,b.link,b.viewname from UserMenu a ,UserMenuRight b where a.KeyID=b.KeyID and  a.usertype='{0}' order by a.ID Asc", "admin");
                          break;
                      case enumDatabaseType.MYSQL:
                          sql = string.Format(@"select LENGTH(a.id) as ilen, a.*,b.Name,b.PageType,b.link,b.viewname from UserMenu a ,UserMenuRight b where a.KeyID=b.KeyID and  a.usertype='{0}' order by a.ID Asc", "admin");
                          break;
                  }
                  return GetDataTable(sql);
             * */
            return null;
        }

        public string GetFunctions(string keyid)
        {
            string rt = "";
            /*      strSql = "select functions from  usermenuright where keyid=" + keyid;
                  Object obj = ExecuteSqlScalar(strSql, null);
                  if (obj != null)
                  {
                      rt = obj.ToString();
                  }
             * */
            return rt;
        }

        public string GetFunctionsDesc(string keyid)
        {
            string rt = "";
            /*      strSql = "select functionsdes from  usermenuright where keyid=" + keyid;
                  Object obj = ExecuteSqlScalar(strSql, null);
                  if (obj != null)
                  {
                      rt = obj.ToString();
                  }
             * */
            return rt;
        }

        public string GetFunctionsByMenuID(string appname, string roles, string menuid)
        {
            string rt = "";
            //string action = "GetFunctionsByMenuID";

            //object[] args = new object[] { "menuid=" + menuid };
            //object o = WSUtil.getFromWS(action, args);
            //if (o != null)
            //{
            //    rt = o.ToString();
            //}
            UserMenuInfo mui = InitModel.GetUserMenuInfoByMenuID(appname, roles, menuid);

            rt = mui.rolecanusefunction;


            return rt;
        }

        public string GetUserFunctionsByMenuID(string appname, string roles, string menuid)
        {
            string rt = "";
            ///*    strSql = "select usercannotusefunction from usermenu where id=" + menuid;
            //    Object obj = ExecuteSqlScalar(strSql, null);
            //    if (obj != null)
            //    {
            //        rt = obj.ToString();
            //    }
            // * */
            //string action = "GetUserFunctionsByMenuID";

            //object[] args = new object[] { "menuid=" + menuid };

            //rt = WSUtil.getFromWS(action, args).ToString();
            UserMenuInfo mui = InitModel.GetUserMenuInfoByMenuID(appname, roles, menuid);

            rt = mui.usercannotusefunction;

            return rt;
        }

        public string GetUserFunctionsByMenuID(string funs, string user)
        {
            string rt = "";
            string[] atemp = funs.Split(';');
            foreach (string s in atemp)
            {
                string[] a1 = s.Split(',');
                if (a1[0] == user)
                {
                    rt = s.Substring(user.Length + 1, s.Length - user.Length - 1);
                }
            }
            return rt;
        }
        public bool IsCustomUserFunct(string menuid)
        {
            bool rt = false;
            /*    strSql = "select customuserfunction from usermenu where id=" + menuid;
                Object obj = ExecuteSqlScalar(strSql, null);
                if (obj != null)
                {
                    rt = obj.ToString() == "1" ? true : false;
                }
             * */
            return rt;
        }
        public DataView GetDataUsermenuright()
        {
            DataView dvRt = null;
            /*    strSql = "select * from usermenuright";
                dvRt = ExecuteSql(strSql);*/
            return dvRt;
        }
        public string GetMaxId(int iLevel, string strParent)
        {
            string strID = "";
            int iID = 1;
            /*     if (iLevel == 1)
                     strSql = "select max(id)+1 from UserMenu where LEN(id)=2";
                 else if (iLevel == 2)
                     strSql = "select max(SUBSTRING(id,3,2))+1 from UserMenu where LEN(id)=4 and SUBSTRING(id,1,2)='" + strParent + "'";
                 else if (iLevel == 3)
                     strSql = "select max(SUBSTRING(id,5,2))+1 from UserMenu where LEN(id)=6 and SUBSTRING(id,1,4)='" + strParent + "'";
                 else if (iLevel == 4)
                     strSql = "select max(SUBSTRING(id,7,2))+1 from UserMenu where LEN(id)=8 and SUBSTRING(id,1,6)='" + strParent + "'";
                 try
                 {
                     iID = Convert.ToInt32(ExecuteSqlScalar(strSql, null));
                 }
                 catch { }

                 if (iLevel == 1)
                 {
                     strID = iID.ToString("D2");
                 }
                 else
                 {
                     strID = strParent + iID.ToString("D2");
                 }

                 */
            return strID;
        }


    }
}

public class MenuInfoComparer : IComparer<object>
{
    public int Compare(Object x, Object y)
    {
        int rs = 0;

        UserMenuInfo ix = (UserMenuInfo)x;// as UserMenuInfo;
        UserMenuInfo iy = (UserMenuInfo)y;//as UserMenuInfo;

        rs = ix.pathlevel.CompareTo(iy.pathlevel);

        if (rs == 0)
        {
            //   rs = ix.showorder.CompareTo(iy.showorder);
            try
            {
                rs = Convert.ToInt32(ix.showorder) - Convert.ToInt32(iy.showorder);
            }
            catch (Exception e) { }
        }

        return rs;
    }
}