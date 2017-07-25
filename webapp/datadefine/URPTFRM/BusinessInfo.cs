using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;

namespace UserRightObj
{ 
 /// <summary>
    /// 业务信息
    /// </summary>
    public class BusinessInfo
    {
        /// <summary>
        /// 业务ID
        /// </summary>
        public string ID="";  
		
		/// <summary>
        /// 所属项目名称
        /// </summary>
        public string AppName = "";
		
		/// <summary>
        /// 业务类型
        /// </summary>
        public string BusinessType="";
		
		/// <summary>
        /// 业务名称
        /// </summary>
        public string BusinessName="";
		
		/// <summary>
        /// 功能名称
        /// </summary>
        public string FuncName="";
		
		/// <summary>
        /// 拓展参数
        /// </summary>
        public string ExtendParam="";
		
		/// <summary>
        /// 业务描述
        /// </summary>
        public string BusinessDesc="";
		
		/// <summary>
        /// 功能描述
        /// </summary>
        public string funcDesc="";
		
		/// <summary>
        /// 父节点业务
        /// </summary>
        public string Parent="";
		
		public List<string> SetDataTableInfo()
        {
            List<string> rt = new List<string>();
            rt.Add("BusinessInfo,1,1,业务信息,1");
            rt.Add("ID,1,1,业务ID,1");
            rt.Add("AppName,2,1,所属项目名称,1");
            rt.Add("BusinessType,3,1,业务类型,1");
            rt.Add("BusinessName,4,1,业务名称,1");
            rt.Add("FuncName,5,1,功能名称,1");
            rt.Add("ExtendParam,6,1,拓展参数,1");
            rt.Add("BusinessDesc,7,1,业务描述,1");
            rt.Add("funcDesc,8,1,功能描述,1");
            rt.Add("Parent,9,1,父节点业务,1");
            return rt;
        }		
	}
		
}