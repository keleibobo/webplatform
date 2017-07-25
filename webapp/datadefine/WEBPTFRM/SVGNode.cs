using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace svgsvr
{
    public class SVGNode
    {
        /// <summary>
        /// 图形文件ID
        /// </summary>
        public String id = "";
        /// <summary>
        /// 图形文件名称
        /// </summary>
        public String name = "";
        /// <summary>
        /// 图形文件描述
        /// </summary>
        public String desc = "";
        /// <summary>
        /// 图形文件宽度
        /// </summary>
        public String width = "";
        /// <summary>
        /// 图形文件高度
        /// </summary>
        public String height = "";
        /// <summary>
        /// 图形文件父节点编号
        /// </summary>
        public String parent_id = "";
        /// <summary>
        /// 父节点路径
        /// </summary>
        public String parent_fullpath = "";
        /// <summary>
        /// 文件大小
        /// </summary>
        public String filesize = "";
        /// <summary>
        /// 修改时间,17位格式：yyyymmddhhmmssfff
        /// </summary>
        public String modifydate = "";	
        /// <summary>
        /// 是否有孩子
        /// </summary>
        public bool haschild = false;
        /// <summary>
        /// 对象类型：名字空间.类名
        /// </summary>
        public string classname = "svgsvr.SVGNode";
        /// <summary>
        /// 是否允许工作区的内容查询
        /// </summary>
        public bool canquerycontent = true;		
		public List<string> SetDataTableInfo()
        {
            List<string> rt = new List<string>();
            rt.Add("svgnodedata,1,1,接线图,1");
            rt.Add("id,1,1,编号,1");
			rt.Add("name,2,1,名称,1");
			rt.Add("desc,3,1,描述,1");
			rt.Add("width,4,1,宽度,1");
			rt.Add("height,5,1,高度,1");
			rt.Add("parent_id,6,1,父编号,1");
			rt.Add("parent_fullpath,7,1,父全路径,1");
			rt.Add("filesize,8,1,文件大小,1");			
			rt.Add("modifydate,9,1,修改时间,1");
			rt.Add("haschild,10,1,是否有孩子,1");
			rt.Add("classname,11,0,类名,1");
			rt.Add("canquerycontent,12,0,是否允许工作区的内容查询,1");
            return rt;
        }			
    }
}
