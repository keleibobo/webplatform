using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;

namespace UserRightObj
{ 
 /// <summary>
    /// 项目信息
    /// </summary>
    public class AppinfoNode
    {
	/// <summary>
        /// 项目编号
        /// </summary>
        public string id="";  
        /// <summary>
        /// 项目名称
        /// </summary>
        public string name="";  

		public List<string> SetDataTableInfo()
        {
            List<string> rt = new List<string>();
            rt.Add("AppinfoNode,1,1,项目信息,1");
            rt.Add("id,1,1,项目编号,1");
			 rt.Add("name,2,1,项目名称,1");
            return rt;
        }		
	}
		
}