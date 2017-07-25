using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UTDtBaseSvr;
using AppCode;
using System.Web.Services;
using System.Text;
using System.IO;
using UserRightObj;
using UTDtCnvrt;
using System.Reflection;
using System.Data;
using System.Web.UI.HtmlControls;
using Microsoft.Office.Interop.Excel;
using UTDtCnvrtTable;

/// <summary>
/// 工作区主处理类
/// </summary>
public partial class UI_Default : System.Web.UI.Page
{
    public  string BusinessType="";
    public static string arg = "";//之前url的参赛，传递给table组件用于查询？
    static Dictionary<string, long> totalpage=new Dictionary<string,long>();
    static BusinessCall bcCall;

    public string appname = "";
    public string rolename = "";
    public string username = "";
    public string layoutcells;
    public string style = ReadConfig.TheReadConfig["style"];
    public string timeout = InitModel.GetLoginTimeOut(ReadConfig.TheReadConfig["appname"]);


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!ValidateSession.Check())
            Response.Redirect(ValidateSession.GetRedirectUrl(1));
        if (!IsPostBack)
        {
            BindCss("com.css");
            BindCss("ucList.css");
            BindCss("themes/default/easyui.css");
            BindCss("themes/icon.css");
            SiteAddress.InnerText = menutext.Value = Request["SelectNodeName"].Replace("?", " ");
            
            appname = HttpContext.Current.Session["appname"].ToString();
            stype.Value = BusinessType;
        }

        BusinessType = Request["type"] != null ? Request["type"] : "Main";
        HttpContext.Current.Session["type"] = BusinessType;
        stype.Value = BusinessType;
        bcCall = InitModel.GetBusinessCall(appname, HttpContext.Current.Session["type"].ToString());
        HttpContext.Current.Session["bcCall"] = bcCall;

        if (bcCall != null)
        {
            List<BusinessComponentCall> bccList = bcCall.bComponentList;
            List<BusinessComponentLayoutCall> bclcList = bcCall.bcLayoutList;
            List<BusinessInfoLayout> biList=bcCall.bILayoutList;
            SetWebLayout(bclcList);
            toModule(bccList, bclcList);
        }
    }

    private void SetWebLayout(List<BusinessComponentLayoutCall> LayoutList)
    {
        weblayout.InnerHtml = SetLayout.Layout(LayoutList).Replace("UTDt","") + TimerScript(bcCall.bComponentList);
    }

    public string TimerScript(List<BusinessComponentCall> ComponentList)
    {
        string Script = "";
        foreach (BusinessComponentCall BCC in ComponentList)
        {
            if (BCC.extendparam.ToLower().IndexOf("timer=1") > -1)
            {
                Dictionary<string, object> dc = LayoutUI.getParam(BCC.extendparam, ';', '=');
                if (dc.ContainsKey("timeinterval"))
                {
                    Script += "<script>startTimer('component_" + BCC.id + "_" + BCC.type + "'," +Convert.ToInt16(dc["timeinterval"])+ ");</script>";
                }
            }
        }
        return Script;
    }

    private void getargs()
    {
        string rs = "";
        string tag = "nodepath=";
        string temp = "";
        StringBuilder sb = new StringBuilder();
        int index = -1;
        if (Request.RawUrl.IndexOf(tag) != -1)
        {
            index = Request.RawUrl.IndexOf(tag) + tag.Length;
            arg = Request.RawUrl.Substring(index);

            int begin = Request.RawUrl.IndexOf("?");
            rs = Request.RawUrl.Substring(begin + 1, Request.RawUrl.IndexOf(tag) - begin - 1);

        }



        string[] split = arg.Split('|');

        tag = "\\\\";
        if (split.Length > 4)
        {
            for (int i = 0; i < split.Length; i = i + 4)
            {

                string node = "";
                if (split[i].IndexOf(tag) != -1)
                {
                    node = split[i].Substring(split[i].IndexOf(tag) + tag.Length);
                }
                else
                {
                    node = split[i];
                }
                if (i + 2 < split.Length)
                {
                    temp = String.Format("{0}({1})\\", node, split[i + 2]);
                    sb.Append(temp);
                }

            }
            arg = "nodepath=" + System.Web.HttpUtility.UrlEncode(sb.ToString().Substring(0, sb.Length - 1));

            ///增加导航树 选中节点值的传递
            string key = "", val = "";
            temp = Request.RawUrl.Substring(Request.RawUrl.IndexOf("TreeNodeText="));
            int end = temp.IndexOf(";");
            if (split[4].IndexOf(tag) > -1)
            {
                key = split[4].Substring(0, split[4].IndexOf(tag));
                val = temp.Substring("TreeNodeText=".Length, end - "TreeNodeText=".Length);
                arg += ";" + key + "=" + val;
            }

        }
        else
        {
            arg = "";
        }

    }
    /// <summary>
    /// 设置布局信息
    /// </summary>
    /// <param name="bccList">业务组件列表</param>
    private void setlayoutype(List<BusinessComponentCall> bccList)
    {
        List<Dictionary<string, string>> datas = new List<Dictionary<string, string>>();
        foreach (BusinessComponentCall bcc in bccList)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("type", bcc.type);
            data.Add("id",bcc.id);
            datas.Add(data);
        }

        layoutcells = FormatUtil.toJSON(datas).Replace("\"","'");
    }

    /// <summary>
    /// 解析业务组件中的布局信息
    /// </summary>
    /// <param name="bccList">业务组件列表</param>
    /// <param name="bclcList">业务组件布局列表</param>
    private void toModule(List<BusinessComponentCall> bccList, List<BusinessComponentLayoutCall> bclcList)
    {
        string path = "../basepage/";
        string ascx = path+"WebUserControlCondition.ascx";
        //Control control = Page.LoadControl(ascx);
        //PlaceHolderWebUserControl.Controls.Add(control);
        bool flag = true;

        foreach (BusinessComponentCall bcc in bccList)
        {
           // string type = bcc.type;

            //暂引入form
           // if (bcc.name.Equals("htmlform"))
            if ("business_condition".Equals(bcc.sourcetype)) //待处理
            {
                ascx = "WebUserControlForm.ascx"; //单个
                if ("business_condition".Equals(bcc.type.Trim())) //总
                {
                    ascx = "WebUserControlCondition.ascx";
                    flag = false;
                }
            //    ascx = "WebUserControlHVCondition.ascx";
            //    flag = false;
            //    continue;
            }
            else
            {
              //  ascx = getASCX(bcc.type);
                ComponentPage cp = InitModel.GetComponentPageByType(bcc.type);
              
                if(cp!=null)
                ascx = cp.page;
            }

           
            if (ascx==null||ascx.Equals("")) continue;

            Control control = Page.LoadControl(path + ascx);
             setValue(control, "Id", bcc.id);

            PlaceHolderWebUserControl.Controls.Add(control);
            ascx = "";
            
        }

        if (flag) //
        {
            ascx = path + "WebUserControlCondition.ascx";
            Control control = Page.LoadControl(ascx);
            PlaceHolderWebUserControl.Controls.Add(control);
        }

    }

  

    private void setValue(Control uc, string var, object vals) //User   : ;
    {

        PropertyInfo pi = uc.GetType().GetProperty(var, BindingFlags.Public | BindingFlags.Instance);
        pi.SetValue(uc, vals, null);

    }


    /// <summary>
    /// 根据svg节点编号获取
    /// </summary>
    /// <param name="nodeid">svg节点编号</param>
    /// <param name="mark"></param>
    /// <returns>返回SVGNode对象转换为json的字符串</returns>
    [WebMethod]
    public static string GetFilename(string nodeid, string mark)
    {
        string rt = "";
        string args = "";

        args = "nodeid=" + System.Web.HttpUtility.UrlEncode(nodeid);
        string password = HttpContext.Current.Session["password"].ToString();
        string username = HttpContext.Current.Session["username"].ToString();
        ServiceInfo sisvg = ServiceConfig.GetInfo(UTDtCnvrt.EnumServiceFlag.svgsvrservice);
        string appname = HttpContext.Current.Session["appname"].ToString();

        String url = String.Format("http://{0}/GetRDDataJson?a={2}&b={3}&c=getuserdefaultsvg&d=appname={1};", sisvg.url,
        appname,
        username, password);
        if (mark == "true")
        {
            Object[] vals = (Object[])FormatUtil.fromJSON(WSUtil.getURLResult(url));
            if (vals != null && vals.Length == 2)
            {
                Dictionary<string, Object> node = (Dictionary<string, Object>)vals[1];
                rt = node["v"].ToString();
                return rt;
            }
            else
            {
                url = String.Format("http://{0}/GetRDDataJson?a={3}&b={4}&c=getsvgnode&d=appname={1};{2};", sisvg.url,
                appname,
                args,
                username, password);
                 vals = (Object[])FormatUtil.fromJSON(WSUtil.getURLResult(url));
                if (vals != null && vals.Length == 2)
                {
                    Dictionary<string, Object> node = (Dictionary<string, Object>)vals[1];
                    rt = node["v"].ToString();
                    return rt;
                }
            }
        }
        else
        {
            url = String.Format("http://{0}/GetRDDataJson?a={3}&b={4}&c=getsvgnode&d=appname={1};{2};", sisvg.url,
           appname,
           args,
           username, password);
            Object[] vals = (Object[])FormatUtil.fromJSON(WSUtil.getURLResult(url));
            if (vals != null && vals.Length == 2)
            {
                Dictionary<string, Object> node = (Dictionary<string, Object>)vals[1];
                rt = node["v"].ToString();
            }
        }
        return rt;
    }

    [WebMethod]
    public static string GetLinkFilename(string nodeid)
    {
        string rt = "";
        string args = "";

        args = "fname=" + System.Web.HttpUtility.UrlEncode(nodeid);
        string password = HttpContext.Current.Session["password"].ToString();
        string username = HttpContext.Current.Session["username"].ToString();

        ServiceInfo sisvg = ServiceConfig.GetInfo(UTDtCnvrt.EnumServiceFlag.svgsvrservice);
        string appname = HttpContext.Current.Session["appname"].ToString();
        String url = String.Format("http://{0}/GetRDDataJson?a={3}&b={4}&c=getsvgnode&d=appname={1};{2};", sisvg.url,
          appname,
         args,
         username, password);


        Object[] vals = (Object[])FormatUtil.fromJSON(WSUtil.getURLResult(url));
        if (vals != null && vals.Length == 2)
        {
            Dictionary<string, Object> node = (Dictionary<string, Object>)vals[1];
            rt = node["v"].ToString();
        }

        return rt;
    }

    /// <summary>
    /// 从网络服务获取svg编号的文件
    /// </summary>
    /// <param name="sId">svg编号</param>
    /// <param name="loginID">项目代码</param>
    /// <param name="filesize">文件大小</param>
    /// <returns>返回svg对应的文件路径</returns>
    [WebMethod]
    public static string GetSvgFileName(string sId, string loginID, string filesize)
    {
        string sRt = "";
        ServiceInfo si = ServiceConfig.GetInfo(UTDtCnvrt.EnumServiceFlag.svgsvrservice);
        string password = HttpContext.Current.Session["password"].ToString();
        string username = HttpContext.Current.Session["username"].ToString();
      
        if (filesize.Equals("0")) return sRt;
         string strFileName = loginID + "_" + sId + "_" + filesize + ".svg";//    
        string strUrlFormat = "http://{0}/GetSvg?fid={1}&fname={2}&appname={3}&contenttype={4}&username={5}&pwd={6}";
        string strUrl = string.Format(strUrlFormat, si.url, sId, "", loginID, 0,username,password);

        sRt = AppDomain.CurrentDomain.BaseDirectory + "svg\\svgfiles\\" + strFileName;
        if (!Directory.Exists(Path.GetDirectoryName(sRt)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(sRt));
        }
        try
        {
            var fileInfo = new FileInfo(sRt);
            if (fileInfo.Length.ToString() == filesize)
            {
                return sRt;
            }
        }
        catch (System.Exception ex)
        {
            // ... 
        }

        // Debug @ 2013-07-02 
        //return sRt;
    
        byte[] MyData = AppCode.WSUtil.getURLResultStream(strUrl);
        FileStream fs = File.Create(sRt);
        try
        {
            fs.Write(MyData, 0, MyData.Length);
        }
        catch (System.Exception ex)
        {

        }
        fs.Close();

        //MetaData.SaveStreamToFile(stream, @"e:\\CSDN_logo.GIF");
        return sRt;
    }


    /// <summary>
    /// 根据条件代码获取下拉框条件数据的网页内容
    /// </summary>
    /// <param name="ConditionName">条件代码</param>
    /// <returns>条件下来数据网页内容</returns>
    [WebMethod]
    public static string getConditions(string ConditionName)
    {
        string rs = "";
        String appname = HttpContext.Current.Session["appname"].ToString();
        string roles = HttpContext.Current.Session["RoleType"].ToString();

        string password = HttpContext.Current.Session["password"].ToString();
        string username = HttpContext.Current.Session["username"].ToString();


        BusinessCall bcCall = (BusinessCall)HttpContext.Current.Session["bcCall"];
        string ptype = bcCall.BussinessName;
        ServiceInfo si = AppCode.ServiceConfig.GetInfo(UTDtCnvrt.EnumServiceFlag.businessservice);
        string url = String.Format("http://{0}/GetRDDataJson?a={5}&b={6}&c=bscdtdat&d=rolename={1};BusinessName={2};AppName={3};ConditionName={4}",
            si.url, roles, ptype, appname, ConditionName,username,password);
        List<object> bccList = AppCode.WSUtil.getListFromWS(url, typeof(BusinessConditionCall));
        if (bccList != null && bccList.Count > 0)
        {
            rs = HtmlCondition.GetControl_ComboxContent((BusinessConditionCall)bccList[0]);
        }
        ///获取下拉等数据 
        return rs;
    }

   

    private static string getPageRange(String ctrlIdDesc,String currentpage)
    {
        string rs = "";
        string sid = ctrlIdDesc.Split('_')[1];

        int ipagesize = Convert.ToInt32(ReadConfig.TheReadConfig["pagesize"]);
        long total = ipagesize;
        if (totalpage.ContainsKey(ctrlIdDesc))
        {
            total = totalpage[ctrlIdDesc];
        }
        //int ipagesize = iPageSize;
        if (List.pagesizes.ContainsKey(sid))
        {
            ipagesize = List.pagesizes[sid];
        }

        int iCurrentPage = Convert.ToInt32(currentpage);
        long ibegin = (iCurrentPage - 1) * ipagesize;
        long iend = iCurrentPage * ipagesize - 1;
        if (iend >= total && total >= ipagesize) iend = total - 1;


        rs += ";ibegin=" + ibegin;
        rs += ";iend=" + iend;


        return rs;
    }

    [WebMethod]
    public static void SetPageNum(String id, String pagesize)
    {
        id = id.Split('_')[1];
        if (List.pagesizes.ContainsKey(id))
        {
            List.pagesizes[id] = Convert.ToInt16(pagesize);
        }
    }


    /// <summary>
    /// 获取表格数据
    /// </summary>
    /// <param name="ctrlIdDesc">组件编号</param>
    /// <param name="currentpage">当前页号</param>
    /// <param name="sPara">参数</param>
    /// <returns>返回表格数据</returns>
    [WebMethod]
    public static String getTable(String ctrlIdDesc, String currentpage, String sPara,String totalnum)
    {
        string rs = "";
        string sid = ctrlIdDesc.Split('_')[1];
        int ipagesize = Convert.ToInt32(ReadConfig.TheReadConfig["pagesize"]);
        long total = ipagesize;
        if (totalnum!="")
        {
            total = Convert.ToInt32(totalnum);
        }
        if (List.pagesizes.ContainsKey(sid))
        {
            ipagesize = List.pagesizes[sid];
        }
        BusinessCall bcCal = (BusinessCall)HttpContext.Current.Session["bcCall"];
        string BusinessType = HttpContext.Current.Session["type"].ToString();
        string componenttype = "";
        if (bcCal == null) return rs;
        foreach (BusinessComponentCall bcc in bcCal.bComponentList)
        {
            if (bcCal.bComponentList.Count == 0)
            {
                return "componentlist count : 0";
            }
            if (bcc.id == sid)
            {
                componenttype = bcc.name;
                break;
            }
        }
        if (componenttype == "")
        {
            string comid = "";
            foreach (BusinessComponentCall bcc in bcCal.bComponentList)
            {
                comid += bcc.id+";";
            }
            return "componenttype is null;ComponentCount:" + bcCal.bComponentList.Count + ";sid:"+sid+";componentid:"+comid+";bccalid"+bcCal.BusinessID;
        }
        int iCurrentPage = Convert.ToInt32(currentpage);
        long ibegin = (iCurrentPage - 1) * ipagesize;
        long iend = iCurrentPage * ipagesize - 1;
        if (iend >= total && total >= ipagesize) iend = total - 1;

        String strPara = "";
        if (sPara != "")
        {
            strPara = sPara.Replace('&', ';').Trim();
            if (strPara[0].Equals(';'))
            {
                strPara = strPara.Substring(1);
            }
        }

        strPara += ";ibegin=" + ibegin;
        strPara += ";iend=" + iend;
        strPara += ";" + arg;
        arg = "";
        if (strPara.Equals(""))
            return rs;
        
        string appname = ((Loginresult)HttpContext.Current.Session["Session"]).AppName;
        object[] args = new object[] { "BusinessName=" + BusinessType, "ComponentName=" + componenttype, strPara, componenttype + ".appname=" + appname };
        UTDtBusiness.BsDataTable dtData = (UTDtBusiness.BsDataTable)AppCode.WSUtil.getTableFromPostXMLWS(EnumServiceFlag.businessservice, "bstabdat", appname, args);
        
        if (dtData != null)
        {
            {
                if ( ibegin == 0)
                {
                    total = dtData.total;
                    //if (totalpage.ContainsKey(ctrlIdDesc))
                    //{
                    //    totalpage[ctrlIdDesc] = total;
                    //}
                    //else
                    //{
                    //    totalpage.Add(ctrlIdDesc, total);
                    //}
                   // List.addPageSum(ctrlIdDesc,"" + ((total - 1) / ipagesize + 1));
                }
           }
            rs = List.GetHtmlData(ctrlIdDesc, dtData, bcCal, total, (total - 1) / ipagesize + 1);
        }
         return rs;
    }

    /// <summary>
    /// 导出excel表格
    /// </summary>
    /// <param name="ctrlIdDesc">组件编号</param>
    /// <param name="currentpage">当前页号</param>
    /// <param name="sPara">参数</param>
    /// <returns>返回表格数据</returns>
    [WebMethod]
    public static String ExportExcel(String ctrlIdDesc, String currentpage, String sPara,string nav)
    {
        string rs = "";
        string componentID = ctrlIdDesc.Split('_')[1];
        int ipagesize = Convert.ToInt32(ReadConfig.TheReadConfig["pagesize"]);
        long total = ipagesize;
        if (totalpage.ContainsKey(ctrlIdDesc))
        {
            total = totalpage[ctrlIdDesc];
        }
        if (List.pagesizes.ContainsKey(componentID))
        {
            ipagesize = List.pagesizes[componentID];
        }
        string BusinessType = HttpContext.Current.Session["type"].ToString();
        string componenttype = BusinessType;
        if (bcCall == null) return rs;
        foreach (BusinessComponentCall bcc in bcCall.bComponentList)
        {
            if (bcc.id.Equals(componentID))
            {
                componenttype = bcc.name;
                break;
            }
        }
        int iCurrentPage = Convert.ToInt32(currentpage);
        long ibegin = (iCurrentPage - 1) * ipagesize;
        long iend = iCurrentPage * ipagesize - 1;
        if (iend >= total && total >= ipagesize) iend = total - 1;
        String strPara = "";
        if (sPara != "")
        {
            strPara = sPara.Replace('&', ';').Trim();
            if (strPara[0].Equals(';'))
            {
                strPara = strPara.Substring(1);
            }
        }
        strPara += ";" + arg;
        strPara += ";ibegin=0";
        strPara += ";iend=" + total+";";
        arg = "";
        if (strPara.Equals(""))
            return rs;

        string appname = ((Loginresult)HttpContext.Current.Session["Session"]).AppName;
        string rolename = ((Loginresult)HttpContext.Current.Session["Session"]).RoleName;
        object[] args = new object[] { "BusinessName=" + BusinessType, "ComponentName=" + componenttype, strPara, componenttype + ".appname=" + appname };
        UTDtBusiness.BsDataTable dtData = (UTDtBusiness.BsDataTable)AppCode.WSUtil.getFromXMLWS(EnumServiceFlag.businessservice, "bstabdat", appname, args);
        if (dtData.dt.Rows.Count == 0)
        {
            return "no data";
        }
        List<string> HiddenColumn = new List<string>();
        foreach (DataTableInfo dti in  dtData.ltColumnInfo)
        {
            if (dti.ColumnIndex != 0)
            {
                if (dti.ShowFlag == false)
                {
                    dtData.dt.Columns.Remove(dti.Desc);
                }
                if (nav == dti.FieldName)
                {
                    nav = dti.Desc;
                }
            }
        }
        string path = HttpContext.Current.Server.MapPath("query.aspx");
        path = path.Replace("\\query.aspx","");
        string filePath = path + "\\tempfile";
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        string gid = Guid.NewGuid().ToString();
        filePath = filePath + "\\" + gid + ".xlsx";
        string download = gid + ".xlsx";
        SuperToExcel(dtData.dt,filePath , nav);
        return download;
    }

    public static bool SuperToExcel(System.Data.DataTable excelTable, string filePath,string nav)
    {
        Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.ApplicationClass();
        try
        {
            if (nav != "")
            {
                Dictionary<string, string> navList = new Dictionary<string, string>();
                foreach (DataRow drv in excelTable.Rows)
                {
                    if (!navList.ContainsKey(drv[nav].ToString()))
                    {
                        navList.Add(drv[nav].ToString(), drv[nav].ToString());
                    }
                }
                app.Visible = false;
                Workbook wBook = app.Workbooks.Add(true);
                List<Worksheet> lw = new List<Worksheet>();
                for (int x = 0; x < navList.Count; x++)
                {
                    Worksheet wSheet = wBook.Worksheets[x + 1] as Worksheet;
                    lw.Add(wSheet);
                    if (x != navList.Count - 1)
                    {
                        wSheet = (Worksheet)wBook.Worksheets.Add(Type.Missing, wSheet, 1, Type.Missing);
                    }
                }
                int j = 0;
                foreach (String nv in navList.Values)
                {
                    lw[j].Name = nv;
                    int colCount = excelTable.Columns.Count;
                    int rowCount = excelTable.Rows.Count;
                    //写标题  
                    int size = excelTable.Columns.Count;
                    for (int i = 0; i < size; i++)
                    {
                        lw[j].Cells[1, 1 + i] = excelTable.Columns[i].Caption;
                    }
                    int x = 2;
                    foreach (DataRow dr in excelTable.Rows)
                    {
                        if (lw[j].Name == dr[nav].ToString())
                        {
                            for (int i = 0; i < size; i++)
                            {
                                lw[j].Cells[x, 1 + i] = dr[excelTable.Columns[i].Caption].ToString();
                            }
                            x++;
                        }
                    }
                    lw[j].Columns.AutoFit();
                    j++;
                }
                //设置禁止弹出保存和覆盖的询问提示框   
                app.DisplayAlerts = false;
                app.AlertBeforeOverwriting = false;

                wBook.Saved = true;
                wBook.SaveCopyAs(filePath);

                app.Quit();
                app = null;
                GC.Collect();
            }
            else
            {
                if (excelTable == null)
                    return false;
                int rowNum = excelTable.Rows.Count;
                int columnNum = excelTable.Columns.Count;
                int rowIndex = 1;
                int columnIndex = 0;
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.ApplicationClass();
                xlApp.DefaultFilePath = "";
                xlApp.DisplayAlerts = true;
                xlApp.SheetsInNewWorkbook = 1;
                Workbook xlBook = xlApp.Workbooks.Add(true);
                //将DataTable的列名导入Excel表第一行
                foreach (DataColumn dc in excelTable.Columns)
                {
                    columnIndex++;
                    xlApp.Cells[rowIndex, columnIndex] = dc.ColumnName;
                    
                }
                //将DataTable中的数据导入Excel中
                for (int i = 0; i < rowNum; i++)
                {
                    rowIndex++;
                    columnIndex = 0;
                    for (int j = 0; j < columnNum; j++)
                    {
                        columnIndex++;
                        xlApp.Cells[rowIndex, columnIndex] = excelTable.Rows[i][j].ToString();
                    }
                }
                xlBook.SaveCopyAs(HttpUtility.UrlDecode(filePath, System.Text.Encoding.UTF8));
                app.DisplayAlerts = false;
                app.AlertBeforeOverwriting = false;

                xlBook.Saved = true;
                xlBook.SaveCopyAs(filePath);

                app.Quit();
                app = null;
                GC.Collect();
            }
            

            
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
        }
    }


    [WebMethod]
    public static void DestroySession()
    {
        if (HttpContext.Current.Session != null)
        {
            HttpContext.Current.Session.Abandon();
        }
    }

    /// <summary>
    /// 获取网页组件的内容
    /// </summary>
    /// <param name="ctrlIdDesc">组件编号</param>
    /// <param name="sPara">参数</param>
    /// <returns>返回网页内容</returns>
    [WebMethod]
    public static String getHtml(String ctrlIdDesc, String sPara)
    {
        string rs = "";

        string sid = ctrlIdDesc.Split('_')[1];

        String strPara = "";
        if (sPara != "")
        {
            strPara = sPara.Replace('&', ';').Trim();
            if (strPara[0].Equals(';'))
            {
                strPara = strPara.Substring(1);
            }
        }

        strPara += ";" + arg;
        arg = "";
        if (strPara.Equals(""))
            return null;
        string BusinessType = HttpContext.Current.Session["type"].ToString();
        string componenttype = BusinessType;
        if (bcCall == null) return null;
        foreach (BusinessComponentCall bcc in bcCall.bComponentList)
        {
            if (bcc.id.Equals(sid))
            {
                componenttype = bcc.name;
                break;
            }
        }
        string appname = ((Loginresult)HttpContext.Current.Session["Session"]).AppName;
        string rolename = ((Loginresult)HttpContext.Current.Session["Session"]).RoleName;
        object[] args = new object[] { "BusinessName=" + BusinessType, "ComponentName=" + componenttype, strPara, componenttype + ".appname=" + appname };

        Object dtData = AppCode.WSUtil.getFromWS(EnumServiceFlag.businessservice, "bstabdat", appname, rolename, args);
        if (dtData != null)
        {
            rs = dtData.ToString();
        }
        return rs;


    }

  

    /// <summary>
    /// 获取表格导航页数
    /// </summary>
    /// <param name="tableid">表格组件编号</param>
    /// <returns>返回导航页数</returns>
    [WebMethod]
    public static String getPageID(String tableid)
    {
        String rs = "";

        if (List.tablepage.ContainsKey(tableid))
        {
            rs = List.tablepage[tableid];
            //   rs += ":" + totolpage[tableid];
        }
        rs= LayoutUI.getId(rs, bcCall.bComponentList);
        return rs;

    }
    /// <summary>
    /// 获取svg图形对应的缩略图
    /// </summary>
    /// <param name="sId">svg编号</param>
    /// <param name="loginID">项目代码</param>
    /// <returns>返回缩略图的文件全路径</returns>
    [WebMethod]
    public static string GetSvgJPGFile(string sId, string loginID)
    {
        string sRt = "";
        ServiceInfo si = ServiceConfig.GetInfo(UTDtCnvrt.EnumServiceFlag.svgsvrservice);
        string password = HttpContext.Current.Session["password"].ToString();
        string username = HttpContext.Current.Session["username"].ToString();
        //string sData = "";
        string strFileName = loginID + "_" + sId + ".jpg";//    
        string strUrlFormat = "http://{0}/GetJPG?fid={1}&fname={2}&appname={3}&contenttype={4}&username={5}&pwd={6}";
        string strUrl = string.Format(strUrlFormat, si.url, sId, "", loginID, 0,username,password);

        sRt = AppDomain.CurrentDomain.BaseDirectory + "svg\\svgfiles\\" + strFileName;
        if (!Directory.Exists(Path.GetDirectoryName(sRt)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(sRt));
        }
        try
        {
            var fileInfo = new FileInfo(sRt);
        }
        catch (System.Exception ex)
        {
            // ... 
        }

        byte[] MyData = AppCode.WSUtil.getURLResultStream(strUrl);

        FileStream fs = File.Create(sRt);
        try
        {
            fs.Write(MyData, 0, MyData.Length);
        }
        catch (System.Exception ex)
        {

        }
        fs.Close();
        return sRt;
    }


    /// <summary>
    /// 保存记录
    /// </summary>
    /// <param name="TableName">业务名称</param>
    /// <param name="namelist">字段名称</param>
    /// <param name="valueArray">该字段数据</param>
    /// <param name="sFilename"></param>
    /// <returns></returns>
    [WebMethod]
    public static string GetData(string url, string method, string param)
    {
        string appname = ((Loginresult)HttpContext.Current.Session["Session"]).AppName;
        string rolename = ((Loginresult)HttpContext.Current.Session["Session"]).RoleName;
        List<string> Rt = AppCode.WSUtil.GetInputDataFromWS(url, "", param, method);
        string result = "";
        for (int i = 0; i < Rt.Count; i++)
        {
            result += Rt[i] + "|";
        }
        return result;
    }
    [WebMethod]
    public static string GetDWData(string url, string method, string param,string domname)
    {
        string appname = ((Loginresult)HttpContext.Current.Session["Session"]).AppName;
        string rolename = ((Loginresult)HttpContext.Current.Session["Session"]).RoleName;
        List<string> Rt = AppCode.WSUtil.GetInputDataFromWS(url, "", param, method);
        string result = domname+"|";
        for (int i = 0; i < Rt.Count; i++)
        {
            result += Rt[i] + "|";
        }
        return result;
    }
    /// <summary>
    /// sql注入关键字判断
    /// </summary>
    /// <param name="InText">输入参数</param>
    /// <returns></returns>
    public static bool SqlFilter2(string InText)
    {
        string word = "and|exec|insert|select|delete|update|chr|mid|master|or|truncate|char|declare|join"; if (InText == null) return false;
        foreach (string i in word.Split('|'))
        {
            if ((InText.ToLower().IndexOf(i + " ") > -1) || (InText.ToLower().IndexOf(";" + i) > -1))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 保存记录
    /// </summary>
    /// <param name="TableName">业务名称</param>
    /// <param name="namelist">字段名称</param>
    /// <param name="valueArray">该字段数据</param>
    /// <param name="sFilename"></param>
    /// <returns></returns>
    [WebMethod]
    public static long AddNewRecord(string TableName, List<string> namelist, List<string> valueArray, string sFilename)
    {
        ResultData rd = new ResultData();
        foreach (string s in valueArray)
        {
            if (SqlFilter2(s))
            {
                rd.result = 48;
                return rd.result;
            }
        }
        USWebService Us = new USWebService();
        BusinessCall bcCall = (BusinessCall)HttpContext.Current.Session["bcCall"];
        if (bcCall != null)
        {
            if (bcCall.bComponentList.Count > 0)
            {
                for (int i = 0; i < bcCall.bComponentList.Count; i++)
                {
                    if (bcCall.bComponentList[i].name == TableName)
                    {
                        string param = "";
                        string AppName = "appname=" + ReadConfig.TheReadConfig["appname"] + ";";
                        Loginresult lt = (Loginresult)HttpContext.Current.Session["Session"];
                        param += "sid=" + lt.Sessionid + ";pwd=" + HttpContext.Current.Session["password"].ToString() + ";" + AppName.ToLower();
                        for (int k = 0; k < namelist.Count; k++)
                        {
                            param += bcCall.bComponentList[i].name + "." + namelist[k] + "=" + valueArray[k] + ";";
                        }
                        if (bcCall.bComponentList[i].sourcetype.ToLower() == "webservice_base")
                        {
                            rd = Us.CommonMethod(bcCall.bComponentList[i].EditSourceMethodAdd, TableName, bcCall.bComponentList[i].name, param);
                            break;
                        }
                    }
                }
            }
        }
        return rd.result;
    }



    /// <summary>
    /// 更新记录
    /// </summary>
    /// <param name="TableName">业务名称</param>
    /// <param name="namelist">字段名称</param>
    /// <param name="valueArray">该字段数据</param>
    /// <param name="sFilename"></param>
    /// <returns></returns>
    [WebMethod]
    public static long SaveNewRecord(string TableName, List<string> namelist, List<string> valueArray, string sFilename)
    {
        ResultData rd = new ResultData();
        foreach (string s in valueArray)
        {
            if (SqlFilter2(s))
            {
                rd.result = 48;
                return rd.result;
            }
        }
        USWebService Us = new USWebService();
        BusinessCall bcCall = (BusinessCall)HttpContext.Current.Session["bcCall"];
        if (bcCall != null)
        {
            if (bcCall.bComponentList.Count > 0)
            {
                for (int i = 0; i < bcCall.bComponentList.Count; i++)
                {
                    if (bcCall.bComponentList[i].name == TableName)
                    {
                        string param = "";
                        string AppName = "appname=" + ReadConfig.TheReadConfig["appname"] + ";";
                        Loginresult lt = (Loginresult)HttpContext.Current.Session["Session"];
                        param += "sid=" + lt.Sessionid + ";pwd=" + HttpContext.Current.Session["password"].ToString() + ";" + AppName.ToLower();
                        for (int k = 0; k < namelist.Count; k++)
                        {
                            param += bcCall.bComponentList[i].name + "." + namelist[k] + "=" + valueArray[k] + ";";
                        }
                        if (bcCall.bComponentList[i].sourcetype.ToLower() == "webservice_base")
                        {
                            rd = Us.CommonMethod(bcCall.bComponentList[i].EditSourceMethodUpdate, TableName, bcCall.bComponentList[i].name, param);
                            break;
                        }
                    }
                }
            }
        }
        return rd.result;
    }

    /// <summary>
    /// 保存上传的数据到应用系统中
    /// </summary>
    /// <param name="TableName">表名</param>
    /// <param name="namelist">名称列表</param>
    /// <param name="valueArray">值列表</param>
    /// <param name="sFilename">文件名称，用于存在上传文件时候需要</param>
    /// <returns>返回保存结果值</returns>
    [WebMethod]
    public static long DelData(string TableName, List<string> namelist, List<string> valueArray)
    {
        ResultData rd = new ResultData();
        USWebService Us = new USWebService();
        BusinessCall bcCall = (BusinessCall)HttpContext.Current.Session["bcCall"];
        if (bcCall != null)
        {
            if (bcCall.bComponentList.Count > 0)
            {
                for (int i = 0; i < bcCall.bComponentList.Count; i++)
                {
                    if (bcCall.bComponentList[i].id == TableName.Split('_')[1])
                    {
                        string param = "";
                        string AppName = "appname=" + ReadConfig.TheReadConfig["appname"] + ";";
                        Loginresult lt = (Loginresult)HttpContext.Current.Session["Session"];
                        param += "sid=" + lt.Sessionid + ";pwd=" + HttpContext.Current.Session["password"].ToString() + ";" + AppName;
                        for (int k = 0; k < namelist.Count; k++)
                        {
                            param += bcCall.bComponentList[i].name + "." + namelist[k] + "=" + valueArray[k] + ";";
                        }
                        if (bcCall.bComponentList[i].sourcetype.ToLower() == "webservice_base")
                        {
                            rd = Us.CommonMethod(bcCall.bComponentList[i].EditSourceMethodDel, bcCall.BussinessName, bcCall.bComponentList[i].name, param);
                            break;
                        }
                    }
                }
            }
        }

        return rd.result;
    }

    [WebMethod]
    public static string FBData(string table, string sWhere, string id)
    {
        bool bDel = false;
        string sUpdatesql = string.Format("update {0} set pubtime=getdate() where {1}", table, sWhere);
        ////   WebDatabase dbProduct = new WebDatabase(enumDatabaseBussiness.PRODUCT);
        ///  bDel = dbProduct.ExecSQL(sUpdatesql);
        return bDel.ToString();
    }
    [WebMethod]
    public static string GetPOWERGAP(string strDate)
    {
        string strRt = "";
        ///      PowerAnalysisModel pam = new PowerAnalysisModel();
        ///      pam.sStartDate = strDate + "-01-01";
        ///      pam.sEndDate = pam.sStartDate;
        if (strDate == DateTime.Now.Year.ToString())
        {
            ///        pam.sStartDate = DateTime.Now.ToString();
            ///         pam.sEndDate = DateTime.Now.ToString();
        }
        ///     pam.bExplainFieldNameToChinese = false;
        ///    DataView dv1= pam.GetPOWERGAP();
        ///    if(dv1!=null&&dv1.Count>0)
        {
            ///         strRt = "当前负荷缺口：" + dv1[0]["C_SHIFTPEAK_VALUE"].ToString() + "MW&nbsp;&nbsp;&nbsp;方案:&nbsp;" + dv1[0]["PlanTypeName"].ToString() + "&nbsp;&nbsp;&nbsp;错峰目标：" + dv1[0]["DAY_VALUE"].ToString() + "MW";
        }
        return strRt;

    }

    private void BindCss(String css)
    {

        HtmlGenericControl objLink = new HtmlGenericControl("LINK");

        objLink.Attributes["rel"] = "stylesheet";

        objLink.Attributes["type"] = "text/css";

        objLink.Attributes["href"] = "../css/" + style + "/"+css;

        comcss.Controls.Add(objLink);

    }



   

   

}