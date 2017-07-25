using System;
using System.Collections; 
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web; 
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class GuaShi_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        AppCode.ValidateNumber vn1 = new AppCode.ValidateNumber();
        string strValidateNumber=vn1.CreateValidateNumber(4);
        vn1.CreateValidateGraphic(imgValidateNumber.Page, strValidateNumber);
        //Profile.ValidateNumber = strValidateNumber;
        Session["VldNum"] = null;
        Session["VldNum"] = strValidateNumber;
      
    }
}
