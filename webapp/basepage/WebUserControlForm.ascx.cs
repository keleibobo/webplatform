using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UTDtBaseSvr;
using AppCode;
using System.Text;

public partial class basepage_WebUserControlInput : System.Web.UI.UserControl
{

    Dictionary<string, object> layout;
    string type = "";// button/text 等
    BusinessComponentCall bcomponentCall;
    BusinessConditionCall bconditionCall;

 
     // select?ul
    public string Id
    {
        set;
        get;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!(IsPostBack))
        {
         //     String BusinessType = bcCall.BussinessName;
           
        }

        

    //    type = "ul";//test
    }


    protected override void Render(HtmlTextWriter output)
    {
        BusinessCall bcCall = (BusinessCall)HttpContext.Current.Session["bcCall"];
        String script = "";
        layout = LayoutUI.getLayout(bcCall.bcLayoutList, Id);
         getComponent(bcCall);
         string  rs = "";
        // rs = String.Format("<div id='component_{0}_{1}' style='float:left'>{2}</div><script type='text/javascript'>setLayout('component_{0}_{1}','[]')</script>", Id, "condition", rs);

         if (bconditionCall == null)
         {
        //     script = String.Format("<script type='text/javascript'>f_initcondition('{0}');</script>", Id);
         }
         else
         {
             string cid = String.Format("component_{0}_{1}", Id, bconditionCall.ComponetType);
             rs=ConditionLayout.getTD(bconditionCall,Id,bcCall);
         
             rs = String.Format("<div id='{0}' ctype='condition'>{1}</div>", cid, rs);

             Dictionary<string, Object> extendparam = LayoutUI.getParam(bconditionCall.para, ';', '=');

             string func = "";
             if (extendparam.ContainsKey("func"))
             {
                 func = String.Format("{0}('{1}')",extendparam["func"].ToString(),cid);
             }

            

             //if (bconditionCall.ComponetType.Equals("pagrid"))
             //{
             //    func = String.Format("f_loadpage('component_{0}_{1}',1,'')", Id, "condition");
             //}

             String css = "";
             if (layout != null)
             {
                 css = FormatUtil.toJSON(layout["css"]);
             }

              script = String.Format("<script type='text/javascript'>setLayout('{0}','{1}');{2}</script>", cid, css,func);
           //    script+=
           //  script+=ConditionLayout.getJS(bcCall,Id);

         }

                output.Write(rs+script);
    }

    private void getComponent(BusinessCall bCall)
    {


      
            foreach (BusinessComponentCall bcCall in bCall.bComponentList)
            {
                if (bcCall.id.Equals(Id))
                {
                    bcomponentCall = bcCall;
                    break;
                }
            }
        

         bconditionCall = null;
        if (bcomponentCall != null)
        {
            foreach (BusinessConditionCall bcCall in bCall.bccList)
            {
                if (bcCall.name.Equals(bcomponentCall.name)) //&& bcCall.para != null
                {
                    bconditionCall = bcCall;
                    break;
                }
            }
        }

    //  return  ConditionLayout.getHTML(bconditionCall);

       

    }

    
}

