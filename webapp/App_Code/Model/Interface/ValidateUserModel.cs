using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Security.Cryptography;
using UTDtBaseSvr;
 
/// <summary>
///ValidateUserModel 的摘要说明
/// </summary>
namespace AppCode
{
    public class ValidateUserModel
    {
        public string[] getAllUsersByRoles(string roles)//无数据
        {
            string action = "getAllUsersByRoles";
    
            object[] args = new object[] { "roles="+roles};

            object result = WSUtil.getFromWS(action, args);
            if(result!=null)
            return result.ToString().Split(',');

            return null;
        }

        public DataView getAllRoles()
        {
            bool bRt = false;
         //   strSql = "select RoleName from aspnet_Roles order by rolename";
         //   return ExecuteSql(strSql);         

           // string action = "gettable";

           // object[] args = new object[] { "tablename=aspnet_Roles" };

           //object rt = WSUtil.getFromWS(action,args);

           //DataTable dt = DataTableSerializer.DESerialize(rt.ToString());
           // DataTable dt = null;
           // ServiceInfo si = ServiceConfig.GetInfo("UserRightHost");
           // String appname = LoginUserModel.AppName;
           // string url = String.Format("http://{0}:{1}/GetRDDataJson?a=1&b=1&c=getroles&d=rolename={2};BusinessName={3};AppName={4};ConditionName={5}", si.ip, si.port, roles, ptype, appname, ConditionName);
           // List<object> bccList = AppCode.WSUtil.getListFromWS(url, typeof(BusinessConditionCall));     

           //USWebService us = new USWebService();
      
           //List<Role> rolesList = us.GetRolesList(appname);

           return null;
        
        }

        public DataTable getUserMenu(string appname,string roles)
        {
            string action = "gettable";
            object[] args = new object[] { "tablename=UserMenu"};
            object rt = WSUtil.getFromWSByRole(action, appname, roles, args);
            DataTable dt = UTUtil.DataTableSerializer.DESerialize(rt.ToString());
            return dt;
        }

        public DataTable getTableInfo(string tablename,string role,string appname)
        {
            string action = "gettable";
            object[] args = new object[] { "tablename=" + tablename };
            object rt = WSUtil.getFromWSByRole(action, appname, role, args);
            DataTable dt = UTUtil.DataTableSerializer.DESerialize(rt.ToString());
            return dt;
        }
    }
}