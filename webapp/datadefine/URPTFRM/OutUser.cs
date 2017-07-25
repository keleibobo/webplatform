using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;

namespace UserRightObj
{ 
 /// <summary>
    /// 第三方接入用户
    /// </summary>
    public class OutUser
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId="";  
		
		/// <summary>
        /// 用户名
        /// </summary>
        public string UserName="";
		
		/// <summary>
        /// 角色
        /// </summary>
        public string Role="";
		
		/// <summary>
        /// 映射用户ID
        /// </summary>
        public string MatchId="";
		
		/// <summary>
        /// 所属项目
        /// </summary>
        public string AppName="";
		
		public List<string> SetDataTableInfo()
        {
            List<string> rt = new List<string>();
			rt.Add("OutUser,1,1,第三方接入用户");
            rt.Add("UserId,1,1,用户ID");
			rt.Add("UserName,2,1,用户名");
			rt.Add("Role,3,1,角色");
			rt.Add("MatchId,4,1,映射用户ID");
			rt.Add("AppName,5,1,所属项目");
            return rt;
        }		
	}
		
}