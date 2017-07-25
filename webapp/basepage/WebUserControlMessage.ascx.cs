using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using UTDtBaseSvr;
using AppCode;

public partial class UI_MessagePopUpControl : System.Web.UI.UserControl
{
    public DataTable dt = new DataTable();
    public string title = "告警事件";
    public string msg = "";

    public string Id
    {
        set;
        get;
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected override void Render(HtmlTextWriter output)
    {

        output.Write(@"<div  id='component_{0}_{1}'>{2}</div>

                      <script type='text/javascript' >f_loadmsg('component_{0}_{1}')</script>" //setTimeout(function(){{startshow('component_{0}_{1}')}},1000);
            , Id,HtmlComponetType.alarmevent,"");
    }
}