using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UTDtBaseSvr;
using System.Text;

/// <summary>
/// Summary description for Condition
/// </summary>
/// 
namespace AppCode
{
    public class ConditionLayout
    {
        public ConditionLayout()
        {
            //
            // TODO: Add constructor logic here
            //
        }

      


        /// <summary>
        /// 原表格布局 返回td
        /// </summary>
        /// <param name="bccList"></param>
        /// <returns></returns>
        public static String getTD(BusinessConditionCall cd,string id,BusinessCall bcCall)
        {
            String rs = "";
       //     foreach (BaseSrv.BusinessConditionCall cd in bccList)
       //     {


                //Dictionary<string, Object> extendparam = LayoutUI.getParam(cd.para, ';', '=');

                //if (extendparam.ContainsKey("show") && extendparam["show"].Equals("layout")) //扩展，默认由条件组件管理，增加对单个条件的布局管理（不出现在组件管理而是布局管理）
                //{
                   
                //}

            if (cd == null)
            {
                return "<div id='component_0_condition'></div>";
            }


                if (cd.ComponetType.ToLower() == "combox" && cd.PresentType == "presenttypecombox")
                {
                    rs= AppCode.HtmlCondition.GetControl_Combox(cd);
                 //   rs+=ConditionLayout.getJS(bcCall, id);

                    //Dictionary<string, object> extendMap = LayoutUI.getParam(cd.para, ';', '=');
                    //string func = "onchange";
                    //if (extendMap.ContainsKey(func))
                    //{
                    //  //  rs += string.Format("<script type='text/javascript' >{1}('{0}','{2}')</script>", cd.name, extendMap[func], getTargetId(bcCall, id));
                    //    rs+= getJS(bcCall, id);
                    //}

                  

                    rs += getJS(bcCall,id);
                }
                else if (cd.ComponetType.ToLower() == "combox" && cd.PresentType == "presenttyperadio")
                {
                    rs= AppCode.HtmlCondition.GetControl_Radio(cd);
                    //   rs= AppCode.HtmlCondition.GetControl_Combox(cd);
                    //   rs= AppCode.HtmlCondition.GetControl_Month(cd);
                }
                else if (cd.ComponetType.ToLower() == "combox" && cd.PresentType == "presenttypecheckbox")
                {
                    rs= AppCode.HtmlCondition.GetControl_Checkbox(cd);
                }
                else if (cd.ComponetType.ToLower() == "int")
                {
                    rs= AppCode.HtmlCondition.GetControl_Int(cd);
                }
                else if (cd.ComponetType.ToLower() == "varchar")
                {
                    rs= AppCode.HtmlCondition.GetControl_VarChar(cd);
                }
                else if (cd.ComponetType.ToLower() == "number")
                {
                    rs= AppCode.HtmlCondition.GetControl_Number(cd);
                }
                else if (cd.ComponetType.ToLower() == "datetime")//
                {
                    //   rs= "<input class='easyui-datebox'></input>";// AppCode.HtmlCondition.GetControl_Datetime(cd);
                    rs= AppCode.HtmlCondition.GetControl_Datetime(cd);
                }
                else if (cd.ComponetType.ToLower() == "date")
                {
                    rs= AppCode.HtmlCondition.GetControl_Date(cd);
                }
                else if (cd.ComponetType.ToLower() == "year")
                {
                    rs= AppCode.HtmlCondition.GetControl_Year(cd);
                }
                else if (cd.ComponetType.ToLower() == "month")
                {
                    rs= AppCode.HtmlCondition.GetControl_Month(cd);
                }
                else if (cd.ComponetType.ToLower() == "time")
                {
                    rs= AppCode.HtmlCondition.GetControl_Time(cd);
                }
                else if (cd.ComponetType.ToLower() == "pagrid") //在easyui增加分页表格
                {
                    rs = string.Format("<table class='easyui-datagrid' id='{0}' cname='{1}' data-options='pagination:true' ></table>",id,cd.name);
                }
                else if (cd.ComponetType.ToLower() == "querydatatable") //在easyui增加分页表格
                {
                    rs = string.Format("<table  cname='{1}' ></table>", id, cd.name);

                    List.setPagesize(id);
                    return rs;
                }
                else
                {
                    rs= AppCode.HtmlCondition.GetControl_Base(cd);
                }
               
           // }
                rs = string.Format("<table  class='TextBorder004' style='width:100%'><tr>{0}</tr></table>",rs);
               
               
            return rs;
        }

        public static string getTargetId(BusinessCall bcCall,string Id)
        {
            string rs = "";
           StringBuilder func = new StringBuilder();
            foreach (BusinessComponentEventCall bcecall in bcCall.bcEventList)
            {
                if (bcecall.source_id.Equals(Id) )//&& bcecall.eventtype.Equals("onselect")
                {
                    rs = LayoutUI.getId(bcecall.dest_id, bcCall);
                    
       }
            }
               
            return rs;
        }


        public static string getJS(BusinessCall bcCall, string Id)
        {
            string rs = "";
            StringBuilder func = new StringBuilder();
            foreach (BusinessComponentEventCall bcecall in bcCall.bcEventList)
            {
                if (bcecall.source_id.Equals(Id) && bcecall.eventtype.Equals("onselect"))
                {
                    string target = LayoutUI.getId(bcecall.dest_id, bcCall);
                    string source = getConditonId(bcecall.source_id, bcCall);


                    func.Append(String.Format("{0}('{1}','{2}','{3}');", bcecall.eventfuncname, source, target, bcecall.eventtype));
                }
            }
            if (func.ToString().Length > 0)
                rs = String.Format("<script type='text/javascript' >{0}</script>", func.ToString());
            return rs;
        }

        private static string getConditonId(string Id,BusinessCall bscall)
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
                                rs = bcc.name;
                                break;
                            }
                        }
                    }
                    

                    break;
                }

            }
            return rs;
        }


        /// <summary>
        /// 返回单个条件组件 到布局
        /// </summary>
        /// <param name="cd"></param>
        /// <returns></returns>
        //public static String getHTML(BusinessConditionCall cd,String id)
        //{
        //    String rs = "";
        //    Dictionary<string, Object> extendparam = LayoutUI.getParam(cd.para, ';', '=');

        ////    if (extendparam.ContainsKey("show") && extendparam["show"].Equals("layout")) //扩展，默认由条件组件管理，增加对单个条件的布局管理（不出现在组件管理而是布局管理）
        //    {
        //        rs = "<table  class='TextBorder004' ><tr>{0}</tr></table>";
        //        rs = String.Format(rs, getTD(cd,id)); //pagrid
        

        //    }
        //       return rs;
        //}

        public static String  getPagrid(List<UTDtCnvrtTable.DataTableInfo> tableinfolist,String id,String name,String rows)
        {
            String rs = "<table class='easyui-datagrid' data-options='pagination:true,singleSelect:true' id='{1}' name='ConditionNameGrid' ><thead><tr>{0}</tr></thead>{2}</table>";

            StringBuilder tr = new StringBuilder();

            foreach (UTDtCnvrtTable.DataTableInfo tableinfo in tableinfolist)
            {
                string option = "";
                if (!tableinfo.ShowFlag)
                {
                    option = ",hidden:true";
                 }
                tr.Append(String.Format("<th  data-options=\"field:'{0}'{2}\">{1}</th>",tableinfo.FieldName, tableinfo.Desc,option));
               
            }

            rs = String.Format(rs,tr,id,rows);
            return rs;
        }


        


    }
}