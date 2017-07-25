using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Data;
using System.Web.Script.Serialization;
using System.Xml;
using System.Reflection;
using UTDtCnvrt;
namespace AppCode
{
    public static class FormatUtil
    {
        public static String toJSON(object obj)
        {

            if (obj == null) return "[]";
                JavaScriptSerializer js = new JavaScriptSerializer();
            
                return js.Serialize(obj);
           
        }

        public static Object fromJSON(String json)
        {

            return fromJSON(json,null);
        }

        public static Object fromJSON(String json, Type type)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            object rs = null;
            try
            {

               rs= js.Deserialize(json, type);
            }
            catch (Exception e)
            {
            }
            return rs;
        }

        public static String toXML(Object obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            StringWriter sw = new StringWriter();
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            //     ns.Add("", "");

            serializer.Serialize(sw, obj, ns);
            return sw.ToString();
        }

        public static Object fromXML(String xml, String typeName)
        {
            Type type = Type.GetType(typeName);
            XmlSerializer xs = new XmlSerializer(type);
            StreamReader sm = new StreamReader(new MemoryStream(System.Text.Encoding.Default.GetBytes(xml)), System.Text.Encoding.Default);

            return  xs.Deserialize(sm);
    
        }

        public static Object fromXML(String xml, Type type)
        {
    
            XmlSerializer xs = new XmlSerializer(type);
            StreamReader sm = new StreamReader(new MemoryStream(System.Text.Encoding.Default.GetBytes(xml)), System.Text.Encoding.Default);

            return xs.Deserialize(sm);

        }

        public static DataTable toView(List<object> list) 
        {
            DataTable dt = new DataTable();
            FieldInfo[] fInfos = null;
            if (list.Count > 0)
            {
                Type type = list[0].GetType();
                fInfos = type.GetFields();
                foreach (FieldInfo f in fInfos)
                {
                    string name = f.Name;
                    object value = f.GetValue(list[0]);
                    Type t = value.GetType();
                    if (t == typeof(string) || t.IsPrimitive)//只处理简单类型
                    {

                        dt.Columns.Add(name);

                    }  
                   
                }
            }

          
            foreach(object obj in  list){
                DataRow dr = dt.NewRow();

                foreach(FieldInfo f in fInfos){
                    string name = f.Name;
                object value = f.GetValue(obj);
                Type t = value.GetType();
                if (t == typeof(string) || t.IsPrimitive)//只处理简单类型
                {

                    dr[name] = value;

                }  
                }

                dt.Rows.Add(dr);

            }
            return dt;
        }

        public static List<MRDDataAll> toMDA(string xml)
        {
            XmlDocument xmldoc = new XmlDocument();
            if (xml.Equals("")) return null;
            xmldoc.LoadXml(xml);
            XmlNodeList nodelist = xmldoc.GetElementsByTagName("MRDDataAll");


            List<MRDDataAll> mList = new List<MRDDataAll>();
            foreach (XmlElement element in nodelist)
            {
                MRDDataAll mda = new MRDDataAll();

                mda.st = element["st"].InnerText;
                mda.v = element["v"].InnerText;

                mList.Add(mda);
            }

            return mList;
        }
    }


}
