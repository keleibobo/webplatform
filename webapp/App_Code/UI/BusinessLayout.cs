using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using UTDtBaseSvr;

/// <summary>
/// Summary description for BusinessLayout
/// </summary>
/// 
namespace AppCode
{
    public class BusinessLayout
    {
        string id;
        string region;
        bool iscollapsible;
        bool issplit;
        List<BusinessLayout> childLayout;
        public List<ComponentLayout> componentList;

        public bool showdiv = true;//是否返回 <div class='easyui-ui'> </div> ;root仅需返回其下子节点（保留原设计,query 页面已有class='easyui-ui'）
  
        public static String toLayout(List<BusinessInfoLayout> biList,List<BusinessComponentLayoutCall> bclcList)
        {
            if (biList.Count == 0) return "";
            Dictionary<string, List<ComponentLayout>> componentLayout = new Dictionary<string, List<ComponentLayout>>();

            foreach (BusinessComponentLayoutCall bclc in bclcList)
            {
                ComponentLayout layout = new ComponentLayout(bclc.placeregion, bclc.componentid,bclc.createorder);
                //  componentLayout.Add(layout);
                List<ComponentLayout> componentList = null;
                if (bclc.componentlayout != null)
                {
                   Dictionary<string,object> dcss=(Dictionary<string,object>)FormatUtil.fromJSON(bclc.componentlayout);
                   if (dcss!=null&&dcss.ContainsKey("css"))
                   {
                       Dictionary<string, object> ddiv = (Dictionary<string, object>)dcss["css"];
                       if (ddiv.ContainsKey("divcss"))
                       {
                           layout.styles = (Dictionary<string,object>)ddiv["divcss"];
                       }
                   }
                   
                }

                if (componentLayout.ContainsKey(bclc.businessinfo_layout_id))
                {
                    componentList = componentLayout[bclc.businessinfo_layout_id];
                }
                else
                {
                    componentList = new List<ComponentLayout>();
                    componentLayout.Add(bclc.businessinfo_layout_id, componentList);
                }
                componentList.Add(layout);
            }

            Dictionary<string, BusinessLayout> indxLayout = new Dictionary<string, BusinessLayout>();
            BusinessLayout root=null;
            foreach (BusinessInfoLayout bi in biList)
            {
                BusinessLayout layout = new BusinessLayout(bi.id,bi.placeregion,bi.issplit);
                if (componentLayout.ContainsKey(bi.id))
                {
                    layout.componentList=componentLayout[bi.id];
                }
                indxLayout.Add(bi.id,layout);
                if (bi.parent_id.Equals("0"))
                {
                    root = layout;
                }
            }

          //  if (root == null) return "";//无配置

            foreach (BusinessInfoLayout bi in biList)
            {
                if (indxLayout.ContainsKey(bi.parent_id))
                {
                    indxLayout[bi.parent_id].AddLayout(indxLayout[bi.id]);
                }
            }


            root.showdiv = false;
           // root.componentLayout = componentLayout;
            return root.toLayout();
        }
        public void AddLayout(BusinessLayout layout)
        {
            if (childLayout == null)
            {
                childLayout = new List<BusinessLayout>();
            }

            childLayout.Add(layout);
        }


        public BusinessLayout(string id,String region,bool issplit)
        {
            this.id = id;
            this.region = region;
            this.issplit = issplit;
        }


        string getDefaultStyle(String region)
        {
            String rs = "";
            switch (region)
            {
                case "west": rs = "width:270px;height:100%x"; break;
                case "north": rs = "width:100%;height:38px"; break;
                default: rs="width:100%;height:100%";break;


            }
            return rs;
        }

     

        public String toLayout() //待定
        {
            StringBuilder rs = new StringBuilder();
            if (childLayout != null && childLayout.Count > 0)
            {
                if(showdiv)
                rs.Append(String.Format("<div class='easyui-layout' id='BI_{0}' style='width:100%;height:100%;' data-options='fit:true'>",id));
                foreach (BusinessLayout blayout in childLayout)
                {
                    string style = getDefaultStyle(blayout.region);
                    String div = "<div {0} style='{2}'>{1}</div>"; //宽度/高度 需设置 否则高度为0 div将隐藏?
                    String layout=blayout.toLayout();


                    rs.Append(String.Format(div, blayout.getOptions(), layout,style)); //style 默认  + blayout.getRegionComponent(blayout.region)
                }

                rs.Append(getOtherComponent()); 
                if(showdiv)
                rs.Append("</div>");
            }

            else
            rs.Append(getAnotherComponent()); //bug

           
            return rs.ToString();
        }

        public string getOtherComponent()
        {
            StringBuilder rs = new StringBuilder();

            if (componentList != null)
            {
                Dictionary<string, List<ComponentLayout>> dclayout = new Dictionary<string, List<ComponentLayout>>();
                


                foreach (ComponentLayout clayout in componentList)
                {
                 //   if (region.Equals(clayout.region))
                    {

                    }
                 //   else
                    {
                        List<ComponentLayout> layoutlist = null;
                        if (dclayout.ContainsKey(clayout.region))
                        {
                            layoutlist = dclayout[clayout.region];
                        }
                        else
                        {
                            layoutlist = new List<ComponentLayout>();
                            dclayout.Add(clayout.region,layoutlist);
                        }
                        layoutlist.Add(clayout);
                    }
                  //  rs.Append(clayout.toLayout());

                }

                 foreach(KeyValuePair<string,List<ComponentLayout>> kvp in dclayout  ){
                     string style = " style='width:100%;heihgt:100%' ";
                    // style='{1}'
                    String div=String.Format(" <div  data-options=\"region:'{0}'\"  {1}>",kvp.Key,style);
                    rs.Append(div);
                    List<ComponentLayout> clayoutList = kvp.Value;
                    clayoutList.Sort(new ComponentLayoutCompare());
                     foreach(ComponentLayout layout in clayoutList){
                         rs.Append(layout.toLayout());
                     }

                    rs.Append("</div>");
                 }

            }

           

            return rs.ToString();
        }


        public string getAnotherComponent()
        {
            StringBuilder rs = new StringBuilder();

            if (componentList != null)
            {
                Dictionary<string, List<ComponentLayout>> dclayout = new Dictionary<string, List<ComponentLayout>>();



                foreach (ComponentLayout clayout in componentList)
                {
                    //   if (region.Equals(clayout.region))
                    //{

                    //}
                    //   else
                    {
                        List<ComponentLayout> layoutlist = null;
                        if (dclayout.ContainsKey(clayout.region))
                        {
                            layoutlist = dclayout[clayout.region];
                        }
                        else
                        {
                            layoutlist = new List<ComponentLayout>();
                            dclayout.Add(clayout.region, layoutlist);
                        }
                        layoutlist.Add(clayout);
                    }
                    //  rs.Append(clayout.toLayout());

                }

                rs.Append("<div class='easyui-layout' >");
                foreach (KeyValuePair<string, List<ComponentLayout>> kvp in dclayout)
                {
                    string style = " style='width:100%;heihgt:100%' ";
                    // style='{1}'
                    String div = String.Format("<div  data-options=\"region:'{0}'\"  {1}>", kvp.Key, style);
                    rs.Append(div);
                    List<ComponentLayout> clayoutList = kvp.Value;
                    clayoutList.Sort(new ComponentLayoutCompare());
                    foreach (ComponentLayout layout in clayoutList)
                    {
                        rs.Append(layout.toLayout());
                    }

                    rs.Append("</div>");
                }
                rs.Append("</div>");

            }



            return rs.ToString();
        }

        /// <summary>
        /// 获取同 region 的Component 组件信息
        /// </summary>
        /// <returns></returns>
        public string getRegionComponent(String region)
        {
            StringBuilder rs = new StringBuilder();

            if (componentList != null)
                foreach (ComponentLayout clayout in componentList)
                {
                    if (region.Equals(clayout.region))
                    {
                        rs.Append(clayout.toLayout());

                    }
                    // rs.Append(clayout.toLayout());

                }

            return rs.ToString();
        }

        private String getOptions()
        {
            String rs = " data-options=\"region:'{0}'{1}\" ";

         

            string isplit = "";
            if (issplit)
            {
                isplit = ",split:true";
            }

            return String.Format(rs, region, isplit);
        }

        class ComponentLayoutCompare : IComparer<ComponentLayout>
        {
            public int Compare(ComponentLayout x, ComponentLayout y)
            {
                return x.order - y.order;
            }

        }
        
        
        public class ComponentLayout
        {
            public String region;
            string componentid;
            public int order;
            public Dictionary<String,object> styles;

            public ComponentLayout(string region, string componentid,string order)
            {
                this.region = region;
                this.componentid = componentid;
                try
                {
                    this.order = Convert.ToInt32(order);
                }
                catch (Exception e) { }
            }

            public string toLayout()
            {

                StringBuilder style = new StringBuilder();

                if (styles != null)
                {
                    style.Append("style=\"");
                    foreach (KeyValuePair<string, object> kvp in styles)
                    {
                        style.Append(String.Format("{0}:{1},", kvp.Key, kvp.Value));
                    }

                    style.Replace(',', '"', style.Length - 2, 2); //最后的 ，--> "
                }
                else
                {
                    style.Append( "style=\"width:100%\"");
                }

                style = new StringBuilder();

                string rs = String.Format("<div id='{0}BaseSvr.BusinessComponentLayoutCall' {1}></div>", componentid,style);
                return rs;
            }

          

            
        }
    }

   
}