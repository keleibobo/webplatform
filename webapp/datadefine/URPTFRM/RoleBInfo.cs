using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;

namespace UserRightObj
{ 
 /// <summary>
    /// 角色业务信息
    /// </summary>
    public class RoleBInfo
    {
        /// <summary>
        /// 业务ID
        /// </summary>
        public string KeyID="";  
		
		/// <summary>
        /// 描述
        /// </summary>
        public string Des="";
		
		/// <summary>
        /// 用户角色类型
        /// </summary>
        public string UserType="";
		
		/// <summary>
        /// 角色功能点
        /// </summary>
        public string Rolefunc="";
		
		/// <summary>
        /// 指定用户无效功能点
        /// </summary>
        public string UserCfunc="";
		
		/// <summary>
        /// 是否包含指定用户无效功能点
        /// </summary>
        public string Customerfunc="";
        /// <summary>
        /// 菜单排序索引
        /// </summary>
        public string showorder = "";
        /// <summary>
        /// 菜单层级
        /// </summary>
        public string pathlevel = "";
        /// <summary>
        /// 父节点名称
        /// </summary>
        public string parentname = "";
        /// <summary>
        /// 名称
        /// </summary>
        public string Name = "";

		
		public List<string> SetDataTableInfo()
        {
            List<string> rt = new List<string>();
			rt.Add("RoleBInfo,1,1,角色业务信息");
            rt.Add("KeyID,1,1,业务ID");
			rt.Add("Des,2,1,描述");
			rt.Add("UserType,3,1,用户角色类型");
			rt.Add("Rolefunc,4,1,角色功能点");
			rt.Add("UserCfunc,5,1,指定用户无效功能点");
			rt.Add("Customerfunc,6,1,是否包含指定用户无效功能点");
            rt.Add("Customerfunc,7,1,菜单排序索引");
            rt.Add("Customerfunc,8,1,菜单层级");
            rt.Add("Customerfunc,9,1,父节点名称");
            rt.Add("Customerfunc,10,1,名称");
            return rt;
        }		
	}
		
}