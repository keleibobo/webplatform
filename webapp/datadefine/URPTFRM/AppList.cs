using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;

namespace UserRightObj
{ 
 /// <summary>
    /// 项目列表
    /// </summary>
    public class AppInfo
    {
        /// <summary>
        /// 项目代码
        /// </summary>
        public string AppName = "";

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Desc = "";

        public List<string> SetDataTableInfo()
        {
            List<string> rt = new List<string>();
            rt.Add("AppInfo,1,1,项目信息,1");
            rt.Add("AppName,1,1,项目代码,1");
            rt.Add("Desc,2,1,项目名称,1");
            return rt;
        }		
	}
		
}