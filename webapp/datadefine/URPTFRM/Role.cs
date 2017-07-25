using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;

namespace UserRightObj
{ 
 /// <summary>
    /// 系统角色
    /// </summary>
    public class Roles
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public string Id="";  
		
		/// <summary>
        /// 角色名称
        /// </summary>
        public string Name="";
		
		/// <summary>
        /// 所属项目名称
        /// </summary>
        public string AppName = "";
		
		/// <summary>
        /// 描述
        /// </summary>
        public string Desc="";


        /// <summary>
        /// 关联第三方项目
        /// </summary>
        public string OutApp = "";
		
		/// <summary>
        /// 首页地址
        /// </summary>
        public string IndexPage="";

		
		public List<string> SetDataTableInfo()
        {
            List<string> rt = new List<string>();
            rt.Add("Role,1,1,系统角色,1");
            rt.Add("Id,1,1,角色ID,1");
            rt.Add("Name,2,1,角色名称,1");
            rt.Add("AppName,3,1,所属项目名称,1");
            rt.Add("Desc,4,1,描述,1");
            rt.Add("OutApp,5,0,关联第三方项目,1");
            rt.Add("IndexPage,6,1,首页地址,1");
            return rt;
        }		
	}
		
}