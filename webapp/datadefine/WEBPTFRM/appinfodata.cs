using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace svgsvr
{
    public class appinfodata
    {
        /// <summary>
        /// ��Ŀ����
        /// </summary>
        public string id = "";
        /// <summary>
        /// ��Ŀ����
        /// </summary>
        public string name = "";
        /// <summary>
        /// ������Ŀ����
        /// </summary>
        public string parent_id = "";
		/// <summary>
        /// �Ƿ��к���
        /// </summary>
		public bool haschild= false;
		/// <summary>
        /// ���������ֿռ�.����
        /// </summary>
        public string classname = "svgsvr.appinfodata";
        /// <summary>
        /// �Ƿ��������������ݲ�ѯ
        /// </summary>
        public bool canquerycontent = false;		
		public List<string> SetDataTableInfo()
        {
            List<string> rt = new List<string>();
            rt.Add("svgsvr.appinfodata,1,1,��Ŀ��Ϣ,1");
            rt.Add("id,1,1,���,1");
			rt.Add("name,2,1,����,1");					
			rt.Add("parent_id,3,1,�����,1");			
			rt.Add("haschild,4,1,�Ƿ��к���,1");
            rt.Add("classname,5,0,����,1");
			rt.Add("canquerycontent,6,0,�Ƿ��������������ݲ�ѯ,1");			
            return rt;
        }	
    }
}