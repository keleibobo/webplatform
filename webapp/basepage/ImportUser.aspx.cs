using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Security.Cryptography;
using UserRightObj;
using AppCode;
using System.Web.Services;
using System.Text.RegularExpressions;
using System.IO;

public partial class basepage_UploadUser : System.Web.UI.Page
{
    string SysApp = ReadConfig.TheReadConfig["URAppName"].ToLower();
    public static string SPERATOR = ReadConfig.TheReadConfig["SPERATOR"];
    public string ClientFilePath = "";
    public string ServerFilePath = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        Loginresult lt = (Loginresult)HttpContext.Current.Session["Session"];
        USWebService Us = new USWebService();
        if (!IsPostBack)
        {
            string appname = "";
            List<String> AppList = Us.GetAllAppName();
            if (lt.AppName == SysApp)
            {
                if (AppList != null && AppList.Count > 0)
                {
                    for (int i = 0; i < AppList.Count; i = i + 2)
                    {
                        ListItem li = new ListItem();
                        li.Text = AppList[i + 1];
                        li.Value = AppList[i];
                        lbTheAppName.Items.Add(li);
                    }
                }
                appname = lbTheAppName.SelectedItem.Value;
            }
            else
            {
                for (int i = 0; i < AppList.Count; i = i + 2)
                {
                    ListItem li = new ListItem();
                    li.Text = AppList[i + 1];
                    li.Value = AppList[i];
                    if (li.Value == lt.AppName)
                    {
                        lbTheAppName.Items.Add(li);
                        appname = li.Value;
                        break;
                    }
                }
            }
        }
    }
    protected void Import_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            string filename = FileUpload1.PostedFile.FileName;
            if (filename.IndexOf(".xlsx") > 0)
            {
                if (!Directory.Exists(ServerFilePath + "\\temfile"))
                    {
                        Directory.CreateDirectory(ServerFilePath + "\\temfile");
                    }
                if (Directory.Exists(ServerFilePath + "\\temfile"))
                {
                    ServerFilePath = ServerFilePath + "\\" + Guid.NewGuid() + FileUpload1.FileName;
                    FileUpload1.SaveAs(ServerFilePath);
                    DataSet ds = ExcelxlsxToDataSet(ServerFilePath);
                    ImportToUR(ds.Tables[0].DefaultView);
                }
            }
            else if (filename.IndexOf(".xls") > 0)
            {
                 if (!Directory.Exists(ServerFilePath + "\\temfile"))
                    {
                        Directory.CreateDirectory(ServerFilePath + "\\temfile");
                    }
                 if (Directory.Exists(ServerFilePath + "\\temfile"))
                 {
                     ServerFilePath = ServerFilePath + "\\" + Guid.NewGuid() + FileUpload1.FileName;
                     FileUpload1.SaveAs(ServerFilePath);
                     DataSet ds = ExcelxlsToDataSet(ServerFilePath);
                     ImportToUR(ds.Tables[0].DefaultView);
                 }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "alert('请选择正确的EXCEL模板文件!')", true);
            }
        }
        else
        {
            if (hpath.Value != "")
            {
                if (hpath.Value.IndexOf(".xlsx") > 0)
                {
                    if (!Directory.Exists(ServerFilePath + "\\temfile"))
                    {
                        Directory.CreateDirectory(ServerFilePath + "\\temfile");
                    }
                    if (Directory.Exists(ServerFilePath + "\\temfile"))
                    {
                        DataSet ds = ExcelxlsxToDataSet(hpath.Value);
                        ImportToUR(ds.Tables[0].DefaultView);
                    }
                }
                else if (hpath.Value.IndexOf(".xls") > 0)
                {
                    if (!Directory.Exists(ServerFilePath + "\\temfile"))
                    {
                        Directory.CreateDirectory(ServerFilePath + "\\temfile");
                    }
                    if (Directory.Exists(ServerFilePath + "\\temfile"))
                    {
                        DataSet ds = ExcelxlsToDataSet(hpath.Value);
                        ImportToUR(ds.Tables[0].DefaultView);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "alert('请选择正确的EXCEL模板文件!')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "alert('该文件不存在!')", true);
            }
        }
    }

    public void ImportToUR(DataView dv)
    {
        string result = "";
        int scount = 0;
        int fcount = 0;
        USWebService Us = new USWebService();
        Loginresult lt = (Loginresult)HttpContext.Current.Session["Session"];
        string appname = lbTheAppName.SelectedValue;
        string SPERATOR = ReadConfig.TheReadConfig["SPERATOR"].ToLower();
        string adminpwd = HttpContext.Current.Session["password"].ToString();
        try
        {
            if (dv.Table.Columns.Contains("用户名") && dv.Table.Columns.Contains("密码"))
            {
                foreach (DataRowView drv in dv)
                {
                    if (drv["用户名"].ToString() == "" || drv["密码"].ToString() == "")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "alert('EXCEL文件账号密码不完整，请补充完整再导入!')", true);
                        return;
                    }
                }
                string rolename = "";
                string extendparam = "";
                foreach (DataRowView drv in dv)
                {
                    rolename = "";
                    extendparam = "";
                    if (dv.Table.Columns.Contains("角色") && drv["角色"].ToString() != "")
                    {
                        rolename = drv["角色"].ToString();
                    }
                    string pwd = Encrypt(drv["密码"].ToString());
                    foreach (DataColumn dc in dv.Table.Columns)
                    {
                        if (dc.ColumnName != "用户名" && dc.ColumnName != "密码" && dc.ColumnName != "角色")
                        {
                            extendparam += dc.ColumnName + SPERATOR + drv[dc.ColumnName].ToString()+"-UTTT_";
                        }
                    }
                    if (Us.ImportUser(lt.Sessionid, adminpwd, appname, rolename, drv["用户名"].ToString(), pwd, extendparam).result == 1)
                    {
                        scount++;
                    }
                    else
                    {
                        fcount++;
                        result += drv["用户名"].ToString() + "\\n";
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "alert('EXCEL模板文件格式有误，请检查后再导入!')", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "alert("+ex.Message+")", true);         
        }
        if (fcount > 0)
        {
            result = "成功导入：" + scount + "个用户，失败：" + fcount + "个。\\n导入失败的用户名：\\n" + result;
        }
        else
        {
            result = "成功导入：" + scount + "个用户。";
        }
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "alert('"+result+"')", true);
        listtable.InnerHtml = "";
    }

    public static DataSet ExcelxlsToDataSet(string filename)
    {
        DataSet ds;
        string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                        "Extended Properties=Excel 8.0;" +
                        "data source=" + filename;
        OleDbConnection myConn = new OleDbConnection(strCon);
        string strCom = " SELECT * FROM [Sheet1$]";
        myConn.Open();
        OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, myConn);
        ds = new DataSet();
        myCommand.Fill(ds);
        myConn.Close();
        return ds;
    }

    public static DataSet ExcelxlsxToDataSet(string filename)
    {
        DataSet ds;
        string strCon = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                        "Extended Properties=Excel 8.0;" +
                        "data source=" + filename;
        OleDbConnection myConn = new OleDbConnection(strCon);
        string strCom = " SELECT * FROM [Sheet1$]";
        myConn.Open();
        OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, myConn);
        ds = new DataSet();
        myCommand.Fill(ds);
        myConn.Close();
        return ds;
    }

    public string Encrypt(string strPwd)
    {
        return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strPwd, "MD5").ToLower();
   }

    public static string DataViewToHtmlTable(DataView dv, string appname)
    {
        USWebService us = new USWebService();
        string htmlstr = "<table style='width:100%;border:1px' border='0' cellpadding='0' cellspacing='0'";
        htmlstr +="<tr height='20' class='trHead'>";
        dv.Table.Columns.Add("该用户是否已存在");
        Dictionary<String, String> UserName = us.GetAppUserName(appname, "");
        foreach (DataColumn dc in dv.Table.Columns)
        {
            htmlstr += "<td class='tdhead'>" + dc.ColumnName + "</td>";
        }
        htmlstr += "</tr>";
        foreach (DataRowView drv in dv)
        {
            htmlstr += "<tr>";
            foreach (DataColumn dc in dv.Table.Columns)
            {
                if (dc.ColumnName != "该用户是否已存在")
                {
                    htmlstr += "<td class='t0'>" + drv[dc.ColumnName].ToString() + "</td>";
                }
                else
                {
                    if (UserName.ContainsKey(drv["用户名"].ToString()))
                    {
                        htmlstr += "<td class='t0'>已存在，不能导入</td>";
                    }
                    else
                    {
                        htmlstr += "<td class='t0'>不存在，可以导入</td>";
                    }
                }
            }
            htmlstr += "</tr>";
        }
        htmlstr += "</table>";
        return htmlstr;
    }
    [WebMethod]
    public static string getTable(string filename,string appname)
    {
        string html = "";
        
            if (filename.IndexOf(".xlsx") > 0)
            {
                DataSet ds = ExcelxlsxToDataSet(filename);
                html = DataViewToHtmlTable(ds.Tables[0].DefaultView, appname);
            }
            else if (filename.IndexOf(".xls") > 0)
            {
                DataSet ds = ExcelxlsToDataSet(filename);
                html = DataViewToHtmlTable(ds.Tables[0].DefaultView, appname);
            }
            
        return html;
    }
    protected void Preview_Click(object sender, EventArgs e)
    {
        PreviewHtml();
    }
    public void PreviewHtml()
    {
        ServerFilePath = HttpContext.Current.Server.MapPath("");
        if (FileUpload1.HasFile)
        {
            string filename = FileUpload1.PostedFile.FileName;
            if (filename.IndexOf(".xlsx") > 0)
            {
                if (!Directory.Exists(ServerFilePath + "\\temfile"))
                {
                    Directory.CreateDirectory(ServerFilePath + "\\temfile");
                }
                if (Directory.Exists(ServerFilePath + "\\temfile"))
                {
                    ServerFilePath = ServerFilePath + "\\temfile\\" + Guid.NewGuid() + FileUpload1.FileName;
                    hpath.Value = ServerFilePath;
                    FileUpload1.SaveAs(ServerFilePath);
                    DataSet ds = ExcelxlsxToDataSet(ServerFilePath);
                    listtable.InnerHtml = DataViewToHtmlTable(ds.Tables[0].DefaultView, lbTheAppName.SelectedValue);
                }
            }
            else if (filename.IndexOf(".xls") > 0)
            {
                if (!Directory.Exists(ServerFilePath + "\\temfile"))
                {
                    Directory.CreateDirectory(ServerFilePath + "\\temfile");
                }
                if (Directory.Exists(ServerFilePath + "\\temfile"))
                {
                    ServerFilePath = ServerFilePath + "\\temfile\\" + Guid.NewGuid() + FileUpload1.FileName;
                    hpath.Value = ServerFilePath;
                    FileUpload1.SaveAs(ServerFilePath);
                    DataSet ds = ExcelxlsToDataSet(ServerFilePath);
                    listtable.InnerHtml = DataViewToHtmlTable(ds.Tables[0].DefaultView, lbTheAppName.SelectedValue);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "alert('请选择正确的EXCEL模板文件!')", true);
            }
        }
        else
        {
            if (hpath.Value != "")
            {
                if (hpath.Value.IndexOf(".xlsx") > 0)
                {
                    DataSet ds = ExcelxlsxToDataSet(hpath.Value);
                    listtable.InnerHtml = DataViewToHtmlTable(ds.Tables[0].DefaultView, lbTheAppName.SelectedValue);
                }
                else if (hpath.Value.IndexOf(".xls") > 0)
                {
                    DataSet ds = ExcelxlsToDataSet(hpath.Value);
                    listtable.InnerHtml = DataViewToHtmlTable(ds.Tables[0].DefaultView, lbTheAppName.SelectedValue);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "alert('请选择正确的EXCEL模板文件!')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "alert('该文件不存在!')", true);
            }
           
        }
    }

    protected void lbTheAppName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (FileUpload1.PostedFile.FileName != "" && hpath.Value == "")
        {
            if (!Directory.Exists(ServerFilePath + "\\temfile"))
                {
                    Directory.CreateDirectory(ServerFilePath + "\\temfile");
                }
            if (Directory.Exists(ServerFilePath + "\\temfile"))
            {
                ServerFilePath = ServerFilePath + "\\" + Guid.NewGuid() + FileUpload1.FileName;
                hpath.Value = ServerFilePath;
                FileUpload1.SaveAs(ServerFilePath);
            }
        }
        if (listtable.InnerHtml != "" && hpath.Value!="")
        {
            if (hpath.Value.IndexOf(".xlsx") > 0)
            {
                DataSet ds = ExcelxlsxToDataSet(hpath.Value);
                listtable.InnerHtml = DataViewToHtmlTable(ds.Tables[0].DefaultView, lbTheAppName.SelectedValue);
            }
            else if (hpath.Value.IndexOf(".xls") > 0)
            {
                DataSet ds = ExcelxlsToDataSet(hpath.Value);
                listtable.InnerHtml = DataViewToHtmlTable(ds.Tables[0].DefaultView, lbTheAppName.SelectedValue);
            }
        }
    }
}