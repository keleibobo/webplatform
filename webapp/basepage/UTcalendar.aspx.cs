using System;
using System.Data;
using System.Configuration;
using System.Collections; 
using System.Web; 
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class UTLib_UTcalendar : System.Web.UI.Page
{
    public string style = ReadConfig.TheReadConfig["style"];
    protected void Page_Load(object sender, EventArgs e)
    {
        mode.Value = Request["mode"];
        cdvalue.Value = Request["cdvalue"];
    }
}
