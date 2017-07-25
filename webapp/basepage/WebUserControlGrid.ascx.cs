using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UTDtBaseSvr;
using AppCode;

public partial class UI_WebUserControlGrid : System.Web.UI.UserControl
{
    public string Id
    {
        set;
        get;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        List.setPagesize(Id);
    }

   


    protected override void Render(HtmlTextWriter output)
    {
        string css = getcss().Trim();
        string rs = "";
        string func = "";
        BusinessCall bcCall = (BusinessCall)HttpContext.Current.Session["bcCall"];
        rs = @"<div id='component_{0}_{1}'></div>
             <script type='text/javascript' >{3};setLayout('component_{0}_{1}','{2}');</script>";//f_loadpage('component_{0}_{1}',1,'')

        Dictionary<string, object> extendMap = List.GetTablextend(Id,bcCall);
        if (extendMap.Count > 0 && extendMap.ContainsKey("checkbox"))
        {
        }
        else
        {
            func = string.Format("setTimeout(function(){{f_loadpage('component_{0}_{1}',1,'');}},500)", Id, HtmlComponetType.querydatatable);
        }
        rs = String.Format(rs,Id,HtmlComponetType.querydatatable,css,func);
        output.Write(rs);
    }

   



    public string getcss()
    {
       // EasyUILayoutCSS css = new EasyUILayoutCSS();
        //表格默认




        string rs = @"{""divcss"":{""overflow"": ""scroll""},""height"":""100%""}";
        BusinessCall bcCall = (BusinessCall)HttpContext.Current.Session["bcCall"];

        foreach(BusinessComponentLayoutCall bclc in bcCall.bcLayoutList){
            if (bclc.componentid==Id&&bclc.componentlayout != null)
            {
                //rs = bclc.componentlayout;
                Dictionary<string, object> obj = (Dictionary<string, object>)FormatUtil.fromJSON(bclc.componentlayout);
                if (obj != null && obj.ContainsKey("css"))
                {
                    //Dictionary<string, object> divcss = (Dictionary<string, object>)obj["css"];
                    //if (divcss.ContainsKey("divcss"))
                    //{
                    //    css.divcss = FormatUtil.toJSON(divcss["divcss"]);
                    //}
                    rs = FormatUtil.toJSON(obj["css"]);
                    break;
                }
              
            }
        }

       
        
        return rs;
    }
}