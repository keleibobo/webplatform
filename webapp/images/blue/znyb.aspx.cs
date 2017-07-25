using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; 
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Data;
//using System.Text;
//using AppCode;

public partial class Public_dnydt : System.Web.UI.Page
{
    //public DataView dv=null;

    
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!(IsPostBack || IsAsync))
        //{
        //    TreeView1.Nodes.Clear();
        //    TreeNode tnRoot = new TreeNode("列表", "-1");
        //    TreeView1.Nodes.Add(tnRoot);
        //    tnRoot.Expanded = true;
        //    UtdrawModel utDM = new UtdrawModel();
        //    utDM.strLoginID = LoginUserModel.PowerUser;
        //    dv = utDM.GetDatas();
        //    TreeNode tn1; //
        //    foreach (DataRowView drv in dv)
        //    {
        //        tn1 = new TreeNode(drv["name"].ToString(), drv["id"].ToString()); //
        //        tnRoot.ChildNodes.Add(tn1);
        //    }
        //    AutoRefreshTime.Value = ReadConfig.TheReadConfig["AutoRefreshTime"];
        //}
        Response.ContentType = "image/svg+xml";
        Response.ContentType = "application/xhtml+xml";
    }
}