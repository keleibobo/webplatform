using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;

namespace UserRightObj
{ 
 /// <summary>
    /// 用户信息
    /// </summary>
    public class User
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string Id="";  
		
		/// <summary>
        /// 用户名
        /// </summary>
        public string Name="";
		
		/// <summary>
        /// 项目名称
        /// </summary>
        public string AppName = "";
		
		/// <summary>
        /// 角色ID
        /// </summary>
        public string Roleid="";
		
		/// <summary>
        /// 所属角色
        /// </summary>
        public string RoleName="";
		
		/// <summary>
        /// 加密字符
        /// </summary>
        public string PasswordSalt="";
		
		/// <summary>
        /// 移动端序列号
        /// </summary>
        public string MobliePin="";
		
		/// <summary>
        /// 电子邮件
        /// </summary>
        public string Email="";
		
		/// <summary>
        /// 备注
        /// </summary>
        public string Remark="";
		
		/// <summary>
        /// 密码
        /// </summary>
        public string Password="";
		
		/// <summary>
        /// 超时时间
        /// </summary>
        public int Timeout=0;
		
		/// <summary>
        /// 密码安全问题
        /// </summary>
        public string PasswordQuestion="";
		
		/// <summary>
        /// 密码安全答案
        /// </summary>
        public string PasswordAnswer="";
		
		/// <summary>
        /// 移动端别名
        /// </summary>
        public string MobileAlias="";
		
		/// <summary>
        /// 特有属性值
        /// </summary>
        public string proValues="";
		
		/// <summary>
        /// 激活状态
        /// </summary>
        public string IsActive="";
		
		/// <summary>
        /// 锁定状态
        /// </summary>
        public string IsLockedOut="";
		
		public List<string> SetDataTableInfo()
        {
            List<string> rt = new List<string>();
			rt.Add("User,1,1,用户信息,1");
            rt.Add("Id,1,0,用户ID,1");
			rt.Add("Name,2,1,用户名,1");
            rt.Add("AppName,3,1,项目名称,1");
			rt.Add("Roleid,4,0,角色ID,1");
            rt.Add("RoleName,5,1,角色,1");
            rt.Add("PasswordSalt,6,1,加密字符,1");
            rt.Add("MobliePin,7,1,移动端序列号,1");
            rt.Add("Email,8,1,电子邮件,1");
            rt.Add("Remark,9,0,备注,1");
            rt.Add("Password,10,0,密码,1");
            rt.Add("Timeout,11,0,超时时间,1");
            rt.Add("PasswordQuestion,12,0,密码提示问题,1");
            rt.Add("PasswordAnswer,13,0,密码提示答案,1");
            rt.Add("MobileAlias,14,0,移动端别名,1");
            rt.Add("proValues,15,0,特有属性值,1");
            rt.Add("IsActive,16,1,激活状态,1");
            rt.Add("IsLockedOut,17,1,锁定状态,1");
            return rt;
        }		
	}
		
}