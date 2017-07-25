using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace svgsvr
{
    public class SVGNode
    {
        /// <summary>
        /// ͼ���ļ�ID
        /// </summary>
        public String id = "";
        /// <summary>
        /// ͼ���ļ�����
        /// </summary>
        public String name = "";
        /// <summary>
        /// ͼ���ļ�����
        /// </summary>
        public String desc = "";
        /// <summary>
        /// ͼ���ļ����
        /// </summary>
        public String width = "";
        /// <summary>
        /// ͼ���ļ��߶�
        /// </summary>
        public String height = "";
        /// <summary>
        /// ͼ���ļ����ڵ���
        /// </summary>
        public String parent_id = "";
        /// <summary>
        /// ���ڵ�·��
        /// </summary>
        public String parent_fullpath = "";
        /// <summary>
        /// �ļ���С
        /// </summary>
        public String filesize = "";
        /// <summary>
        /// �޸�ʱ��,17λ��ʽ��yyyymmddhhmmssfff
        /// </summary>
        public String modifydate = "";	
        /// <summary>
        /// �Ƿ��к���
        /// </summary>
        public bool haschild = false;
        /// <summary>
        /// �������ͣ����ֿռ�.����
        /// </summary>
        public string classname = "svgsvr.SVGNode";
        /// <summary>
        /// �Ƿ��������������ݲ�ѯ
        /// </summary>
        public bool canquerycontent = true;		
		public List<string> SetDataTableInfo()
        {
            List<string> rt = new List<string>();
            rt.Add("svgnodedata,1,1,����ͼ,1");
            rt.Add("id,1,1,���,1");
			rt.Add("name,2,1,����,1");
			rt.Add("desc,3,1,����,1");
			rt.Add("width,4,1,���,1");
			rt.Add("height,5,1,�߶�,1");
			rt.Add("parent_id,6,1,�����,1");
			rt.Add("parent_fullpath,7,1,��ȫ·��,1");
			rt.Add("filesize,8,1,�ļ���С,1");			
			rt.Add("modifydate,9,1,�޸�ʱ��,1");
			rt.Add("haschild,10,1,�Ƿ��к���,1");
			rt.Add("classname,11,0,����,1");
			rt.Add("canquerycontent,12,0,�Ƿ��������������ݲ�ѯ,1");
            return rt;
        }			
    }
}
