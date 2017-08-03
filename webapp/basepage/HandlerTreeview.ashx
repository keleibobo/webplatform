<%@ WebHandler Language="C#" Class="HandlerTreeview" %>

using System;
using System.Web;
using UTDtBaseSvr;
using AppCode;
using System.Collections;
using System.Collections.Generic;
using UTDtCnvrt;
using System.Linq;
using System.Web.SessionState;
using System.Text;

public class HandlerTreeview : IHttpHandler, IRequiresSessionState
{

    String action = "";
    string appname = "";
    string businesstype = "";
    string nodepath = "";
    Dictionary<string, object> extendparam;
    public void ProcessRequest(HttpContext context)
    {
        HttpRequest request = context.Request;
        HttpResponse response = context.Response;
        context.Response.ContentType = "text/plain";

        action = request.Params["action"];
        appname = HttpContext.Current.Session["appname"].ToString();
        businesstype = HttpContext.Current.Session["type"].ToString();
        nodepath = request.Params["nodepath"];
        response.AddHeader("Content-type", "text/json; charset=utf-8");
        String rs = handle();
        response.Write(rs);
        response.End();
    }

    /// <summary>
    /// 根据当前节点类型获取下一个节点类型
    /// </summary>
    /// <param name="currentObjecttype">当前节点类型</param>
    /// <param name="navigatetype">当前导航树类型</param>
    /// <param name="bccCall">业务对象数据</param>
    /// <returns>下一个节点类型，如果是--，表示是没有下一个节点，是叶子</returns>
    private string GetNextObjectType(string currentObjecttype, string navigatetype, BusinessCall bccCall)
    {
        string rt = "--";
        foreach (ClassNaviagteCall cnCall in bccCall.bcNavigateList)
        {
            if (cnCall.name.Equals(navigatetype))
            {
                for (int i = 0; i < cnCall.naviagteobjectList.Count; i++)
                {
                    ClassObjectCall coCall = cnCall.naviagteobjectList[i];
                    if (coCall.name.Equals(currentObjecttype, StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (i == cnCall.naviagteobjectList.Count - 1)
                        {
                            rt = "--";
                        }
                        else
                        {
                            rt = cnCall.naviagteobjectList[i + 1].name;
                        }
                        break;
                    }
                }
                break;
            }
        }
        return rt;
    }

    /// <summary>
    /// 获取导航树类型
    /// </summary>
    /// <param name="bccCall"></param>
    /// <returns></returns>
    private string GetNavigateType(BusinessCall bccCall)
    {
        string rt = "";
        foreach (BusinessComponentCall bcc in bccCall.bComponentList)
        {
            if (bcc.type.Equals(HtmlComponetType.treeview.ToString()))
            {
                rt = bcc.name;
                break;
            }
        }
        return rt;
    }

    /// <summary>
    /// 获取默认的导航树类型
    /// </summary>
    /// <param name="bccCall"></param>
    /// <returns></returns>
    private string GetFirstNavigateType(BusinessCall bccCall)
    {
        string rt = "";
        if (bccCall.bcNavigateList.Count > 0)
        {
            rt = bccCall.bcNavigateList[0].name;
        }
        return rt;
    }

    /// <summary>
    /// 获取导航树类型第一个节点数据类型
    /// </summary>
    /// <param name="bccCall"></param>
    /// <param name="navigatetype"></param>
    /// <returns></returns>
    private string CreateFirstObjectType(BusinessCall bccCall, string navigatetype)
    {
        string rt = "";
        foreach (ClassNaviagteCall cnCall in bccCall.bcNavigateList)
        {
            if (cnCall.name.Equals(navigatetype))
            {
                if (cnCall.naviagteobjectList.Count > 0)
                {
                    ClassObjectCall coCall = cnCall.naviagteobjectList[0];
                    rt = coCall.name;
                }
                break;
            }
        }
        return rt;
    }

    public List<NavigateData> GetSortData(List<NavigateData> ltData)
    {
        List<NavigateData> rt = new List<NavigateData>();
        if (ltData != null && ltData.Count > 0)
        {
            if (ltData[0].ascorder)
            {
                var query = ltData.OrderBy(a => a.showorder);
                rt.AddRange(query);
            }
            else
            {
                var query = ltData.OrderByDescending(a => a.showorder);
                rt.AddRange(query);
            }
        }
        return rt;
    }
    /// <summary>
    /// 分层处理节点数据
    /// </summary>
    /// <returns></returns>
    public string handle()
    {
        BusinessCall bccCall = InitModel.GetBusinessCall(appname, businesstype);
        extendparam = LayoutUI.getParam(bccCall, HtmlComponetType.treeview.ToString());

        string navigatetype = GetFirstNavigateType(bccCall);
        List<EasyUITreeNode> rs = new List<EasyUITreeNode>();
        EasyUITreeNode root = null;
        List<string> pname = new List<string>();
        string nodepathtype = "";
        if (nodepath == null || nodepath.Length == 0)//第一个
        {
            nodepathtype = CreateFirstObjectType(bccCall, navigatetype);
            //if(nodepathtype=="jsys")
            //{
            //    root=null;
            //}
            //else
            //{
            nodepath = appname + "|appname|" + nodepathtype;
            root = new EasyUITreeNode("", LoginUserModel.Title, nodepath);
            root.state = "open";
            string func = LayoutUI.getTreeViewClick(nodepathtype);
            root.addAttribute("clickjs", func);
            //root.text = "";
            // }

        }

        string[] aNodePath = nodepath.Split('|');
        if (aNodePath.Length < 3)
        {
            return "{}";
        }

        string password = HttpContext.Current.Session["password"].ToString();
        string username = HttpContext.Current.Session["username"].ToString();
        string lastnodepath = aNodePath[aNodePath.Length - 1];
        String url = @"http://{0}/SetRDDataJson?a={6}&b={7}&c={1}&d=AppName={4};BusinessName={5};navigatetype={3};nodepath={2}";
        url = String.Format(url, ServiceConfig.GetInfo(UTDtCnvrt.EnumServiceFlag.businessservice).url, "bsnavdat", GetValuePath(nodepath), navigatetype, appname, businesstype, username, password);
        List<object> rt1 = WSutils.getListFromPostWS(url, typeof(UTDtCnvrt.NavigateData));

        List<NavigateData> LND = new List<NavigateData>();
        if (rt1 != null)
        {
            foreach (UTDtCnvrt.NavigateData oTmp in rt1)
            {
                LND.Add(oTmp);
            }

        }

        if (LND != null && LND.Count > 0)
        {
            List<NavigateData> query = GetSortData(LND);
            bool bHasChild = false;
            foreach (UTDtCnvrt.NavigateData oTmp in query)
            {
                string nodeid = oTmp.name;
                string nodetext = oTmp.description;
                string tempath = "";
                bHasChild = oTmp.haschild;
                if (oTmp.haschild)
                {
                    tempath = nodeid + "|" + lastnodepath + "|" + lastnodepath;
                }
                else
                {
                    string nextobjecttype = GetNextObjectType(lastnodepath, navigatetype, bccCall);
                    if (nodepath.IndexOf("FZPTSvr.syspowerdatatype/13") > 0)
                    {
                        tempath = nodeid + "|" + lastnodepath + "|--";
                    }
                    else if (nextobjecttype.Equals("--"))
                    {
                        tempath = nodeid + "|" + lastnodepath + "|--";
                    }
                    else if (oTmp.description == "WEBPTFRM")
                    {
                        tempath = nodeid + "|" + lastnodepath + "|--";
                    }
                    else
                    {
                        bHasChild = true;
                        tempath = nodeid + "|" + lastnodepath + "|" + nextobjecttype + "";
                    }
                }
                EasyUITreeNode node = new EasyUITreeNode(nodeid, nodetext, nodepath + "/" + tempath);
                if (bHasChild)
                {
                    node.state = "closed";
                }
                if (oTmp.canquerycontent)
                {
                    LayoutUI.getTreeViewClick(aNodePath[aNodePath.Length - 1]);
                    string func = LayoutUI.getTreeViewClick(aNodePath[aNodePath.Length - 1]); //getFunc(lastnodepath);// 
                    node.addAttribute("clickjs", func);
                    //额外信息
                    addExtend(node, lastnodepath);
                }
                //额外信息
                // addExtend(node, lastnodepath);
                rs.Add(node);
            }
        }
        bool showRootNode = true;
        //处理父子节点
        foreach (ClassNaviagteCall cnc in bccCall.bcNavigateList)
        {
            if (cnc.name.Equals(nodepathtype, StringComparison.CurrentCultureIgnoreCase)
                && cnc.getdataflag.Equals("returnalldata", StringComparison.CurrentCultureIgnoreCase))
            {
                Bynode(rs, rt1);
                if (cnc.name == "jsys")
                {
                    showRootNode = false;
                }
            }
        }
        if (root != null && showRootNode)
        {
            root.children = rs;
            rs = new List<EasyUITreeNode>();


            rs.Add(root);


        }
        string temp = FormatUtil.toJSON(rs);
        return temp;
    }

    /// <summary>
    /// 数据全部过来的处理
    /// </summary>
    /// <param name="rs"></param>
    /// <param name="lists"></param>
    private void Bynode(List<EasyUITreeNode> rs, List<Object> lists)
    {
        if (lists == null || rs == null)
        {
            return;
        }
        Dictionary<string, EasyUITreeNode> Nodes = new Dictionary<string, EasyUITreeNode>();
        foreach (EasyUITreeNode uinode in rs)
        {
            Nodes.Add(uinode.id, uinode);
        }
        List<NavigateData> LND = new List<NavigateData>();
        foreach (UTDtCnvrt.NavigateData oTmp in lists)
        {
            LND.Add(oTmp);
        }
        List<NavigateData> query = GetSortData(LND);
        foreach (UTDtCnvrt.NavigateData oTmp in query)
        {
            if (Nodes.ContainsKey(oTmp.parent_name))
            {
                Nodes[oTmp.parent_name].addChild(Nodes[oTmp.name]);
                rs.Remove(Nodes[oTmp.name]);
            }
            else if (!oTmp.parent_name.Equals("") && !oTmp.parent_name.Equals(appname)) //根
            {
                EasyUITreeNode node = null;
                if (Nodes.ContainsKey(oTmp.parent_name))
                {
                    node = Nodes[oTmp.parent_name];
                }
                else
                {
                    node = new EasyUITreeNode(oTmp.parent_name);
                    Nodes.Add(oTmp.parent_name, node);
                }
                if (Nodes.ContainsKey(oTmp.name))
                {
                    node.addChild(Nodes[oTmp.name]);
                    rs.Remove(Nodes[oTmp.name]);
                    rs.Add(node);
                }
            }
        }
    }

    protected string GetValuePath(string strValuePath)
    {
        return GetValuePath(strValuePath, "\\");
    }

    protected string GetValuePath(string strValuePath, string strSeparator)
    {
        string strRt = "";
        string[] aStr1 = strValuePath.Split('/');
        for (int i = 0; i < aStr1.Length; i++)
        {
            string s1 = aStr1[i];
            if (s1.Length > 0)
            {
                string[] aStr2 = s1.Split('|');
                if (aStr2.Length > 2)
                {
                    if (strRt.Length != 0)
                    {
                        strRt += strSeparator;
                    }
                    strRt += aStr2[0] + "(" + aStr2[1] + ")";
                    if (i == aStr1.Length - 1)
                    {
                        if (!aStr2[1].Equals(aStr2[2]))
                        {
                            strRt = strRt + strSeparator + "(" + aStr2[2] + ")";
                        }
                    }
                }
            }
        }
        return strRt;
    }

    private string getFunc(String lastnodepath)
    {
        string rs = ""; //改用addExtend
                        //string ctype = "onclicktype";
                        //if (extendparam.ContainsKey(ctype) && extendparam[ctype].Equals(lastnodepath))
                        //{
                        //    ctype = "onClick";
                        //    if(extendparam.ContainsKey(ctype))
                        //    return extendparam[ctype].ToString();
                        //}
        return rs;
    }

    private void addExtend(EasyUITreeNode node, string lastnodepath)
    {
        //不区分click类型
        // if (extendparam!=null&&extendparam.ContainsKey("onclicktype") && extendparam["onclicktype"].Equals(lastnodepath))
        {
            foreach (KeyValuePair<string, object> kvp in extendparam)
            {
                node.addAttribute(kvp.Key, kvp.Value);
            }
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}

