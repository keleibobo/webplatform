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
    public class RoleNode
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public string Id="";  
		
		/// <summary>
        /// 角色名称
        /// </summary>
        public string Name="";

		public List<string> SetDataTableInfo()
        {
            List<string> rt = new List<string>();
            rt.Add("Role,1,1,系统角色,1");
            rt.Add("RoleNode,1,1,角色ID,1");
            rt.Add("Name,2,1,角色名称,1");            
            return rt;
        }		
	}
		
}