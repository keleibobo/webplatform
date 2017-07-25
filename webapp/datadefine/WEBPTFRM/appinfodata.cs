using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace svgsvr
{
    public class appinfodata
    {
        /// <summary>
        /// 项目代码
        /// </summary>
        public string id = "";
        /// <summary>
        /// 项目名称
        /// </summary>
        public string name = "";
        /// <summary>
        /// 父亲项目代码
        /// </summary>
        public string parent_id = "";
		/// <summary>
        /// 是否有孩子
        /// </summary>
		public bool haschild= false;
		/// <summary>
        /// 类名：名字空间.类名
        /// </summary>
        public string classname = "svgsvr.appinfodata";
        /// <summary>
        /// 是否允许工作区的内容查询
        /// </summary>
        public bool canquerycontent = false;		
		public List<string> SetDataTableInfo()
        {
            List<string> rt = new List<string>();
            rt.Add("svgsvr.appinfodata,1,1,项目信息,1");
            rt.Add("id,1,1,编号,1");
			rt.Add("name,2,1,名称,1");					
			rt.Add("parent_id,3,1,父编号,1");			
			rt.Add("haschild,4,1,是否有孩子,1");
            rt.Add("classname,5,0,类名,1");
			rt.Add("canquerycontent,6,0,是否允许工作区的内容查询,1");			
            return rt;
        }	
    }
}