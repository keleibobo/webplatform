using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using UTDtBaseSvr;
using UTDtCnvrt;
using AppCode;

public partial class Public_TestImage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

      
  
        
      //  fromws("点名","值");
      //  WebUserControlChart.chartData = lt;
        InitModel.init();
        string appname = "joyoaffzpt";
        string BusinessType = "YCRealTime";
        BusinessCall bcCall = InitModel.GetBusinessCall(appname, BusinessType);
        HttpContext.Current.Session["bcCall"] = bcCall;
    //    WebUserControlAccordion.ID = "100";
    }


    public void fromws(string cid,string cval)
    {
        string BusinessType = "JSDDHistory";
        string componenttype = "webcomm_PulseCalcHistory";
        string strPara = ";webcomm_StatistCycle=;webcomm_StatistType=;afStartdate=201310280000000000;afEnddate=201310280000000000;ibegin=0;iend=19;";
        string appname = "joyoaffzpt";
        string rolename = "user";
        object[] args = new object[] { "BusinessName=" + BusinessType, "ComponentName=" + componenttype, strPara, componenttype + ".appname=" + appname };

        BusinessCall bcCall = InitModel.GetBusinessCall(appname, BusinessType);
        HttpContext.Current.Session["bcCall"] = bcCall;


        UTDtBusiness.BsDataTable dtData = (UTDtBusiness.BsDataTable)AppCode.WSUtil.getFromXMLWS(EnumServiceFlag.businessservice, "bstabdat", appname, rolename, args);


        string id = "";

        string rs = "";

      //  List<object> tempData = new List<object>();
        Dictionary<string, List<object>> tempData = new Dictionary<string, List<object>>();
        foreach (DataRow dr in dtData.dt.Rows) 

        {
            string chartid = dr[cid].ToString();
            if (tempData.ContainsKey(chartid))
            {
                tempData[chartid].Add(Convert.ToDouble(dr[cval]) / 1.0);
            }
            else
            {
                
                List<object> tempVal = new List<object>();
                tempVal.Add(Convert.ToDouble(dr[cval])/1.0);
                tempData.Add(chartid, tempVal);
            }
          

        }

       // WebUserControlChart.chartData = tempData;
        rs = FormatUtil.toJSON(tempData);
        
    }

    [WebMethod]
    public static string GetImageDatas(int m, int n)
    {
        string s = string.Empty;

        s = "0101010101";

        return s;
    }

   

}