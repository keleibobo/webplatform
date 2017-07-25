using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;

namespace UserRightObj
{ 
 /// <summary>
    /// 用户映射信息
    /// </summary>
    public class MatchInfo
    {
        /// <summary>
        /// 第三方用户ID
        /// </summary>
        public string OutUserId="";  
		
		/// <summary>
        /// 系统用户ID
        /// </summary>
        public string UserId="";
		
		/// <summary>
        /// 第三方用户名
        /// </summary>
        public string OutUserName="";
		
		/// <summary>
        /// 系统用户名
        /// </summary>
        public string UserName="";
		
		/// <summary>
        /// 系统项目名称
        /// </summary>
        public string AppName = "";
		
		/// <summary>
        /// 第三方项目名称
        /// </summary>
        public string OutAppName="";
		
		public List<string> SetDataTableInfo()
        {
            List<string> rt = new List<string>();
			rt.Add("MatchInfo,1,1,用户映射信息");
            rt.Add("OutUserId,1,1,第三方用户ID");
			rt.Add("UserId,2,1,系统用户ID");
			rt.Add("OutUserName,3,1,第三方用户名");
			rt.Add("UserName,4,1,系统用户名");
            rt.Add("AppName,5,1,系统项目名称");
			rt.Add("OutAppName,6,1,第三方项目名称");
            return rt;
        }		
	}
		
}