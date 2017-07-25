using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using AppCode;
using System.Text;
using System.Web.Services;
using System.Collections;
using System.Collections.Specialized;
using UTDtBaseSvr;
using UTCmpl;
using UserRightObj;
using UTDtCnvrt;

public partial class Public_AddRecord : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        string tablename = Request["type"];
        //ScriptManager scpt = new ScriptManager();
        //scpt.ID = "ScriptManager1";
        //scpt.EnablePageMethods = true;
        //form1.Controls.Add(scpt);
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
    /// 获取下拉列表数据
    /// </summary>
    /// <param name="url">数据url</param>
    /// <param name="method">获取数据的方法</param>
    /// <param name="param">获取参数</param>
    /// <returns></returns>
    [WebMethod]
    public static string GetDWData(string url,string method, string param)
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
    public static string GetOneDWData(string url, string method, string param,string destname)
    {
        string appname = ((Loginresult)HttpContext.Current.Session["Session"]).AppName;
        string rolename = ((Loginresult)HttpContext.Current.Session["Session"]).RoleName;
        List<string> Rt = AppCode.WSUtil.GetInputDataFromWS(url, "", param, method);
        string result = "";
        result += destname + "|";
        for (int i = 0; i < Rt.Count; i++)
        {
            result += Rt[i] + "|";
        }
        return result;
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
                for(int i=0;i<bcCall.bComponentList.Count;i++)
                {
                    if (bcCall.bComponentList[i].name == TableName)
                    {
                        string param = "";
                        string AppName ="appname="+ ReadConfig.TheReadConfig["appname"]+";";
                        Loginresult lt = (Loginresult)HttpContext.Current.Session["Session"];
                        param += "sid=" + lt.Sessionid + ";pwd=" + HttpContext.Current.Session["password"].ToString() + ";" + AppName.ToLower();
                        for (int k = 0; k < namelist.Count; k++)
                        {
                            param +=bcCall.bComponentList[i].name+"."+ namelist[k] + "=" + valueArray[k] + ";";
                        }
                        if (bcCall.bComponentList[i].sourcetype.ToLower() == "webservice")
                        {
                            rd = Us.CommonMethod( bcCall.bComponentList[i].EditSourceMethodAdd, TableName, bcCall.bComponentList[i].name, param);
                            break;
                        }
                    }
                }
            }
        }
        return rd.result;
    }

}