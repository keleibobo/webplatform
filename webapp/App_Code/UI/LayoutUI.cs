using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UTDtBaseSvr;
using System.Text;
using System.Collections;


/// <summary>
/// Summary description for LayoutUI
/// </summary>
/// 
namespace AppCode
{
    public class LayoutUI
    {
        static Dictionary<string, string> componentids = new Dictionary<string, string>();

        //public  static void getComponentids()
        //{

        //    foreach (KeyValuePair<String, Dictionary<string, BusinessCall>> kv in InitModel.appbusinesscall)
        //    {
        //        foreach (KeyValuePair<string, BusinessCall> kvp in kv.Value)
        //        {
        //            BusinessCall bc = kvp.Value;
        //          //  if(componentids.ContainsKey(bc.i))
        //        }
        //    }
        //}

        public static string getId(string id, List<BusinessComponentCall> bccList)
        {
            string rs = "component_";
            foreach (BusinessComponentCall bcc in bccList)
            {
                if (bcc.id.Equals(id))
                {
                    return rs + id + "_" + bcc.type;
                }
            }
            return rs;
        }

        public static string GetDestId(string id, List<BusinessComponentEventCall> Belist,List<BusinessComponentCall> BccList)
        {
            string rs = "";
            foreach (BusinessComponentEventCall Bce in Belist)
            {
                if (Bce.source_id.Equals(id))
                {
                    foreach(BusinessComponentCall Bcc in BccList)
                    {
                        if(Bcc.id.Equals(Bce.dest_id))
                        if (rs != "")
                        {
                            rs += ";component_" + Bce.dest_id + "_" + Bcc.type;
                        }
                        else
                        {
                            rs += "component_" + Bce.dest_id + "_" + Bcc.type;
                        }
                    }
                }
            }
            return rs;
        }

        public static string getId(string id, BusinessCall bscall)
        {
            string rs = "component_";
            
              string      type = GetComponentType(bscall, id);
                    return rs + id + "_" + type;
        
          
        }


        /// <summary>
        /// 自定义解析字符串 
        /// 仅处理简单的多个 key value 定义
        /// </summary>
        /// <param name="param"></param>
        public static Dictionary<string, Object> getParam(String param, Char first, Char second)
        {
            
            Dictionary<string, Object> extendparam = new Dictionary<string, object>();
            if (param == null) return extendparam;
            string[] split = param.Split(first);
          
            foreach (string temp in split)
            {
                string[] pm = temp.Split(second);
                if (pm.Length == 2)
                {
                    extendparam.Add(pm[0], pm[1]);
                }
            }

            return extendparam;
        }

        /// <summary>
        /// 根据组件类型返回扩展参数
        /// </summary>
        /// <param name="bcCall"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Dictionary<string, Object> getParam(BusinessCall bcCall,string type)
        {

            List<BusinessComponentCall> bcList = bcCall.bComponentList;
            foreach (BusinessComponentCall bc in bcList)
            {
                if (bc.type.Equals(type))
                {
                   return LayoutUI.getParam(bc.extendparam, ';', '=');            
                }
            }

            return null;
        }

        public static string GetComponentType(BusinessCall bscall,string Id)
        {
            string rs = "";

            foreach (BusinessComponentCall bc in bscall.bComponentList)
            {
                // if (bc.name.Equals(BusinessType))
                if (bc.id.Equals(Id))
                {
                    if ("business_condition".Equals(bc.sourcetype))
                    {
                        foreach (BusinessConditionCall bcc in bscall.bccList)
                        {
                            if (bcc.name.Equals(bc.name))
                            {
                                rs = bcc.ComponetType;
                                break;
                            }
                        }
                    }
                    else
                    {
                        rs = bc.type;
                    }


                    break;
                }

            }

            return rs;
        }

       
        
        public LayoutUI()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static Dictionary<string, object> getLayout(List<BusinessComponentLayoutCall> layoutList, string Id)
        {
            Dictionary<string, object> rs=null;
            foreach (BusinessComponentLayoutCall bclc in layoutList)
            {
                if (bclc.componentid == Id && bclc.componentlayout != null)
                {
             
                   rs = (Dictionary<string, object>)FormatUtil.fromJSON(bclc.componentlayout);
                   break;
                }
            }

            return rs;
        }

        public static string GetComponentLayout(List<BusinessComponentLayoutCall> layoutList, string Id)
        {
           string rs = "";
            foreach (BusinessComponentLayoutCall bclc in layoutList)
            {
                if (bclc.componentid == Id && bclc.componentlayout != null)
                {

                    rs = bclc.componentlayout;
                    break;
                }
            }

            return rs;
        }

        
       static Dictionary<string, string> clickfuns {
           get
           {
               Dictionary<string, string> map= new Dictionary<string, string>();
               map.Add("svgsvr.SVGNode", String.Format("f_loadsvg('{0}','{1}')", ReadConfig.TheReadConfig["appname"], ReadConfig.TheReadConfig["refreshData"]));
             
               return map;
           }
       }
        public static string getTreeViewClick(string navtype)
        {
            if (clickfuns.ContainsKey(navtype))
            {
                return clickfuns[navtype];
            }
            else
            {
                return "f_loadlayout()";
            }
        }

        /// <summary>
        /// 获取父子结构
        /// </summary>
        /// <param name="menuList"></param>
        /// <returns></returns>
        public static Dictionary<String, List<object>> getNav(List<object> menuList)
        {

            Dictionary<String, List<object>> childreninfos = new Dictionary<string, List<object>>();
            foreach (UserMenuInfo umi in menuList)
            {
                ///父子
                string pid = umi.parentname;
                if (childreninfos.ContainsKey(pid))
                {
                    List<object> infoList = childreninfos[pid];
                    infoList.Add(umi);
                }
                else
                {
                    List<object> infoList = new List<object>();
                    infoList.Add(umi);
                    childreninfos.Add(pid, infoList);

                }
            }

            return childreninfos;
        }


        public static string getTable(UTDtBusiness.BsDataTable Bsdt, BusinessCall bsCall)
        {
            String rs = "";

            Hashtable htDataTableInfo = new Hashtable();
         
            foreach (UTDtCnvrtTable.DataTableInfo dti in Bsdt.ltColumnInfo)
            {
                htDataTableInfo.Add(dti.ColumnIndex, dti);
            }
            return rs;
        }

        /// <summary>
        /// 手风琴 侧边菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public static string getAccordion(EasyUIAccordion menu)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("<div title='{0}' icon='{1}'><ul> ", menu.title, menu.icon));
          //  List<String> aList = new List<String>();
            foreach (Dictionary<String, string> kv in menu.ul)
            {
                string a=String.Format("<a href='{0}' {1} ><span class='icon-nav icon'>{2}</span></a> ", kv["href"], kv["target"], kv["text"]);
                sb.Append("<li>"+a+"</li>");
            }

            //foreach (string astr in aList)
            //{
            //}
            sb.Append("</ul></div>");
            return sb.ToString();
        }
    }

    public class EasyUIAccordion
    {
        public string title = "";
        public string icon = "icon-sys";
        public List<Dictionary<string, string>> ul = new List<Dictionary<string, string>>();
        ///href target text
    }

    public class EasyUILayoutCSS
    {
      public  string layout;//west,east,north,sourth,center
      public  string divcss;// " ":'',
      public  string layoutcss;
      public string layoutclass;
      public string divclass;

    }

    public class EasyUIGridData
    {
        public List<Column> columns = new List<Column>();
        public int total=0;
        public List<Dictionary<string, Object>> rows = new List<Dictionary<string, object>>(); 
        //DataTable 不能直接转json
       // DataRow

        public class Column
        {
           public string field;
           public string title;
            int width=100;
            string align = "center";
        }

        public void addColumn(string field, string title)
        {
            Column col = new Column();
            col.field = field;
            col.title = title;
            columns.Add(col);
        }



    }

    public class EasyUITreeNode
    {
        public string id;
        public string text;
        public string state;
        public Dictionary<string, object> attributes = new Dictionary<string, object>();
        public List<EasyUITreeNode> children; //无用

        public EasyUITreeNode(string text)
        {

            this.text = text;
        }

        public EasyUITreeNode(string id, string text, string value)
        {
            this.id = id;
            this.text = text;
            attributes.Add("value", value);
        }

        public void addAttribute(string key, Object value)
        {
            if (attributes.ContainsKey(key))
            {
                attributes[key] = value;
            }
            else
            {
                attributes.Add(key, value);
            }
        }

        public void addChild(EasyUITreeNode node)
        {
            if (children==null)
            {
                children = new List<EasyUITreeNode>();
            }
            children.Add(node);
        }
    }
}