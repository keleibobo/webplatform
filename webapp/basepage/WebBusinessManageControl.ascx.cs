using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AppCode;
using UserRightObj;
using UTDtCnvrt;

public partial class UI_WebBusinessManageControl : System.Web.UI.UserControl
{
    string SPER = "-UT_";
    string SysApp = ReadConfig.TheReadConfig["URAppName"].ToLower();
    public static string SPERATOR = ReadConfig.TheReadConfig["SPERATOR"];
    public Loginresult lt = new Loginresult();
    public List<BusinessInfo> BL = new List<BusinessInfo>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (SPERATOR.Length == 0)
        {
            SPERATOR = SPER;
        }
        if (!IsPostBack)
        {
            lt = (Loginresult)HttpContext.Current.Session["Session"];
            if (lt == null ||lt.Sessionid == null)
            {
                Response.Redirect(string.Format("../public/login/login.aspx"));
            }
            USWebService Us = new USWebService();

            if (!ValidateSession.Check())
                Response.Redirect(ValidateSession.GetRedirectUrl(1));

            ddlBusinessNames.Items.Add(new ListItem("", ""));
            string appname = "";
            List<String> AppList = Us.GetAllAppName();
            if (lt.AppName == SysApp)
            {
                if (AppList != null && AppList.Count > 0)
                {
                    for (int i = 0; i < AppList.Count;i=i+2 )
                    {
                        ListItem li = new ListItem();
                        li.Text = AppList[i + 1];
                        li.Value = AppList[i];
                        lbTheAppName.Items.Add(li);
                    }
                }
                if (lbTheAppName.Items.Count>0)
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
            BL = Us.GetBusiness(appname);
            if(BL!=null)
            foreach (BusinessInfo drv1 in BL)
            {
                if (drv1.ID.ToString().Length != 0 && drv1.AppName == appname)
                    ddlBusinessNames.Items.Add(new ListItem(drv1.BusinessDesc, drv1.ID + ":" + drv1.AppName + ":" + drv1.Parent + ":" + drv1.BusinessName));
            }
            if (lbTheAppName.Items.Count > 0)
            {
                string ImportAppName = lbTheAppName.SelectedItem.Value.ToLower();
                USWebService us = new USWebService();
                List<string> RL = Us.GetRoleSelect(lbTheAppName.SelectedItem.Value);
                if (RL != null)
                for (int i = 0; i < RL.Count; i = i + 2)
                {
                    ListItem li = new ListItem();
                    li.Text = RL[i + 1];
                    li.Value = RL[i];
                    lbTheRoles.Items.Add(li);
                }
                lbTheRoles.SelectedIndex = 0;
                lbTheRoles_SelectedIndexChanged(lbTheRoles, null);
            }
        }

    }

    private void InitBusinessDrodownList()
    {
        ddlBusinessNames.Items.Clear();
        ddlBusinessNames.Items.Add(new ListItem("", ""));
        string appname = lbTheAppName.SelectedItem.Value;
        USWebService usWebService = new USWebService();
        BL = usWebService.GetBusiness(appname);
        if (BL != null)
        {
            foreach (BusinessInfo drv1 in BL)
            {
                if (drv1.ID.ToString().Length != 0 && drv1.AppName == appname)
                    ddlBusinessNames.Items.Add(new ListItem(drv1.BusinessDesc, drv1.ID + ":" + drv1.AppName + ":" + drv1.Parent + ":" + drv1.BusinessName));
            }  
        }
    }
    /// <summary>
    /// 绑定导入用户到DropDownCheckList中
    /// </summary>
    /// <param name="tn"></param>

    private void FillTree(TreeNodeCollection tnc1)
    {
        TreeNode tn1 = null;
        USWebService Us = new USWebService();
        if (lbTheRoles.Items.Count == 0)
            return;
        List<RoleBInfo> RL = Us.GetRoleBInfo(lbTheAppName.SelectedItem.Value, lbTheRoles.SelectedItem.Value);
        for (int i = 0; i < RL.Count; i++)
        {
            if (RL[i].pathlevel == "1")
            {
                int sub = i;
                int index = Convert.ToInt32(RL[i].showorder);
                for (int j = i; j < RL.Count - 1; j++)
                {
                    int nextindex = Convert.ToInt32(RL[j + 1].showorder);
                    if (index > nextindex && RL[i].pathlevel == "1" && RL[j + 1].pathlevel == "1")
                    {
                        index = nextindex;
                        sub = j + 1;
                    }
                }
                if (RL[sub].pathlevel == "1")
                {
                    RoleBInfo rbi = RL[sub];
                    RL[sub] = RL[i];
                    RL[i] = rbi;
                }
            }
        }

        for (int i = 0; i < RL.Count; i++)
        {
            if (RL[i].pathlevel == "2")
            {
                int sub = i;
                int index = Convert.ToInt32(RL[i].showorder);
                for (int j = i; j < RL.Count - 1; j++)
                {
                    int nextindex = Convert.ToInt32(RL[j + 1].showorder);
                    if (index > nextindex && RL[i].pathlevel == "2" && RL[j + 1].pathlevel == "2" && RL[j + 1].parentname == RL[i].parentname)
                    {
                        index = nextindex;
                        sub = j + 1;
                    }
                }
                if (RL[sub].pathlevel == "2")
                {
                    RoleBInfo rbi = RL[sub];
                    RL[sub] = RL[i];
                    RL[i] = rbi;
                }
            }
        }

        foreach (RoleBInfo RI in RL)
        {
            if (RI.pathlevel == "1")
            {
                tn1 = new TreeNode(RI.Des, RI.UserType + SPER + RI.Rolefunc + SPER + RI.UserCfunc + SPER + RI.Customerfunc + SPER + RI.KeyID + SPER + RI.showorder + SPER + RI.pathlevel + SPER + RI.userextendparam + SPER + RI.parentname);
                foreach (RoleBInfo RIS in RL)
                {
                    if (RIS.parentname == RI.Name)
                    {
                        TreeNode tn2 = new TreeNode(RIS.Des, RIS.UserType + SPER + RIS.Rolefunc + SPER + RIS.UserCfunc + SPER + RIS.Customerfunc + SPER + RIS.KeyID + SPER + RIS.showorder + SPER + RIS.pathlevel + SPER + RIS.userextendparam + SPER + RIS.parentname);
                        tn1.ChildNodes.Add(tn2);
                    }
                }
                tnc1.Add(tn1);
            }
        }
        TreeView1.ExpandAll();
    }

    protected void CheckChanged(TreeNode tn1)
    {
        if (tn1 != null && tn1.Checked)
        {
            btnOk.Enabled = true;
            if (tn1.ChildNodes.Count == 0)
            {
                btnDelete.Enabled = true;
            }
            else
            {
                btnDelete.Enabled = false;
            }
            if (RadioOper.SelectedValue == "modify")
            {

            }
            else if (RadioOper.SelectedValue == "add")
            {
                foreach (ListItem li in ddlBusinessNames.Items)
                {
                    if (li.Text == tn1.Text)
                        li.Selected = true;
                }

            }

            tn1.Expand();
        }
        else
        {
            txtName.Text = "";
            ddlBusinessNames.SelectedIndex = 0;
          //  btnOk.Enabled = false;
            if (tn1 != null)
                tn1.Collapse();
        }
    }

    protected void TreeView1_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
    {
        if (e.Node != null && e.Node.Checked)
        {
            btnOk.Enabled = true;
            if (e.Node.ChildNodes.Count == 0)
            {
                btnDelete.Enabled = true;
            }
            else
            {
                btnDelete.Enabled = false;
            }
            string[] sv = Regex.Split(e.Node.Value, SPER, RegexOptions.IgnoreCase);
            if (sv[0] != "-1")
            {
                for (int i = 0; i < ddlBusinessNames.Items.Count; i++)
                {
                    if (ddlBusinessNames.Items[i].Value.Split(':')[0] == sv[4])
                    {
                        businessname.Value = ddlBusinessNames.Items[i].Value.Split(':')[3];
                        ddlBusinessNames.SelectedIndex = i;
                        ddlBusinessNames_SelectedIndexChanged(null, null);
                        txtName.Text = e.Node.Text;
                        ShowOrder.Text = sv[5];
                        string[] svalue = Regex.Split(e.Node.Value, SPER, RegexOptions.IgnoreCase);
                        string rolefunc = svalue[1];
                        foreach (ListItem ck in cblRolesFun.Items)
                        {
                            if (rolefunc.Contains(ck.Value))
                            {
                                ck.Selected = true;
                            }
                            else
                            {
                                ck.Selected = false;
                            }
                        }
                        if (svalue[3] == "1")
                        {
                            cblUserFun.Enabled = true;
                            cbSelf.Checked = true;
                            ddlUser.Visible = true;
                            cblUserFun.Visible = true;
                            string[] ucnfunc = svalue[2].Split('|');
                            foreach (ListItem li in ddlUser.Items)
                            {
                                if (svalue[2].Contains(li.Text))
                                {
                                    for (int k = 0; k < ucnfunc.Length; k++)
                                    {
                                        if (ucnfunc[k].Split(',')[0] == li.Text)
                                        {
                                            if (ucnfunc[k].IndexOf(",") > 0)
                                            {
                                                li.Value += ucnfunc[k].Substring(ucnfunc[k].IndexOf(','));
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            if (ddlUser.Items.Count > 0 && svalue[2].Contains(ddlUser.SelectedItem.Text))
                            {
                                for (int k = 0; k < ucnfunc.Length; k++)
                                {
                                    if (ucnfunc[k].Split(',')[0] == ddlUser.SelectedItem.Text)
                                    {
                                        foreach (ListItem li in cblUserFun.Items)
                                        {
                                            if (!ucnfunc[k].Contains(li.Value))
                                            {
                                                li.Selected = true;
                                            }
                                            else
                                            {
                                                li.Selected = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            cbSelf.Checked = false;
                            ddlUser.Visible = false;
                            cblUserFun.Visible = false;
                        }
                    }
                }
        }
            else
            {
                TreeNodeCollection tnc = TreeView1.Nodes;
                if (tnc[TreeView1.Nodes.Count - 1].ChildNodes.Count > 0 )
                {
                    string[] sv1 = Regex.Split(tnc[0].ChildNodes[tnc[0].ChildNodes.Count - 1].Value, SPER, RegexOptions.IgnoreCase);
                    ShowOrder.Text = (Convert.ToInt32(sv1[5]) + 1).ToString();
                }
            }

    }

        Check(TreeView1.Nodes, e.Node);
       // BindImportUser(e.Node);
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "<script>ImportChuange('"+businessname.Value+"');</script>", false);
    }

    private void Check(TreeNodeCollection tnc1, TreeNode tn1)
    {
        foreach (TreeNode tnTmp in tnc1)
        {
            if (tnTmp != tn1 && tnTmp.Checked)
            {
                tnTmp.Checked = false;
                if (!(tn1.Parent == tnTmp || (tn1.Parent != null && tn1.Parent.Parent == tnTmp)))
                    tnTmp.Collapse();
            }
            if (tnTmp.ChildNodes.Count > 0)
            {
                Check(tnTmp.ChildNodes, tn1);
            }
        }
    }

    private TreeNode GetChecked()
    {
        TreeNode tnRt = null;
        if (TreeView1.CheckedNodes.Count > 0)
            tnRt = TreeView1.CheckedNodes[0];
        return tnRt;
    }

    protected void lbTheRoles_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlUser.Visible = false;
        cblUserFun.Visible = false;
        cblRolesFun.Visible = false;
        txtName.Text = "";
        ShowOrder.Text = "";
        ddlBusinessNames.SelectedIndex = 0;
        TreeView1.Nodes.Clear();
        TreeNode rtNode = new TreeNode("业务列表", "-1");
        TreeView1.Nodes.Add(rtNode);
        FillTree(rtNode.ChildNodes);
        TreeView1.Attributes.Add("onclick", "postBackByObject()");

        GetRoleAllUsers();
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        TreeNode tnRt = GetChecked();
        
        if (lbTheRoles.Items.Count == 0)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "<script>alert('请先添加角色!')</script>", false);
            return;
        }
        if (txtName.Text.Equals(""))
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "<script>alert('请输入菜单名称!')</script>", false);
            return;
        }
        if (ShowOrder.Text.Equals(""))
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "<script>alert('请输入菜单排序索引!')</script>", false);
            return;
        }
        int showorder = 1;
        try
        {
            showorder = Convert.ToInt32(ShowOrder.Text.Trim());
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "<script>alert('菜单排序索引为整数!')</script>", false);
            return;
        }
        if (GetChecked() == null && RadioOper.SelectedValue == "modify")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "<script>alert('请先选择要修改的业务!')</script>", false);
            return;
        }
        if (GetChecked() == null && RadioOper.SelectedValue == "delete")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "<script>alert('请先选择要删除的业务!')</script>", false);
            return;
        }
        if (tnRt == null)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "<script>alert('请先选择父节点!')</script>", false);
            return;
        }
        if (ddlBusinessNames.SelectedIndex == 0)
        {
            if (tnRt.Depth != 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "<script>alert('请先选择要添加的业务!')</script>", false);
                return;
            }
        }

         USWebService Us = new USWebService();
        string RoleName = lbTheRoles.SelectedItem.Value;
        if (ddlBusinessNames.SelectedItem.Value == "")   //添加自定义空业务菜单
        {
            if (RadioOper.SelectedValue == "add")
            {
                ResultData rt = Us.AddBlankMenu(txtName.Text.Trim(), showorder.ToString(), lbTheAppName.SelectedItem.Value, RoleName);
                if (rt.result == 1)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "<script>alert('添加成功!');</script>", false);
                    InitModel.Refresh();
                    lbTheRoles_SelectedIndexChanged(lbTheRoles, null);
                }
                else if (rt.result == 17)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "<script>alert('添加的菜单名称已存在!');</script>", false);
                }
                else if (rt.result == 18)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "<script>alert('菜单索引重复!');</script>", false);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "<script>alert('添加失败!');</script>", false);
                }
            }
            else
            {
                
            }
            InitBusinessDrodownList();
            return;
        }
        string KeyId = ddlBusinessNames.SelectedItem.Value.Split(':')[0];
        string BusinessName = ddlBusinessNames.SelectedItem.Value.Split(':')[3];
        string Des = txtName.Text.Trim();
        string AppName = lbTheAppName.SelectedItem.Value;
        string sfunc = getSelectedRoleFunctions();
        string sUserfunc = "";
        string cmfunc = "";
        
        string userextendparam = "";
        if (cbSelf.Checked)
        {
            sUserfunc = getSelectedUserFunctions();
            cmfunc = "1";
        }
        else
        {
            sUserfunc = "";
            cmfunc = "0";
        }
        string parentNode = ddlBusinessNames.SelectedItem.Value.Split(':')[2];
        int bflag = cbSelf.Checked ? 1 : 0;
        if (RadioOper.SelectedValue == "modify")
        {

            TreeNode tn1 = GetChecked();
            if (tn1 != null && tn1.Value != "-1")
            {
                string OldDesc = tn1.Text;
                ResultData rt = Us.UpdateRoleBusiness(KeyId, Des, OldDesc, RoleName, sfunc, sUserfunc, cmfunc, AppName, BusinessName, showorder, parentNode,userextendparam);
                if (rt.result == 1)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "<script>alert('修改成功!');</script>", false);
                    InitModel.Refresh();
                    lbTheRoles_SelectedIndexChanged(lbTheRoles, null);
                }
                else if (rt.result == 17)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "<script>alert('修改的菜单名称已存在!');</script>", false);
                }
                else if (rt.result == 18)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "<script>alert('菜单索引重复!');</script>", false);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "<script>alert('修改失败!');</script>", false);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "<script>alert('该节点无法修改!');</script>", false);
            }
        }
        else if (RadioOper.SelectedValue == "add")
        {
            if (tnRt.Depth == 0)
            {
                parentNode = "";
            }
            else if (tnRt.Depth == 1)
            {
                string keyId = Regex.Split(tnRt.Value, SPER)[4];
                foreach (ListItem item in ddlBusinessNames.Items)
                {
                    if (item.Value.Split(':')[0] == keyId)
                    {
                        parentNode = item.Value.Split(':')[3];
                        break;
                    }
                }

            }
            else if (tnRt.Depth == 2)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "<script>alert('暂不支持二级以上的菜单!');</script>", false);
                return;
            }
            ResultData rt = Us.AddRoleBusiness(KeyId, Des, RoleName, sfunc, sUserfunc, cmfunc, AppName, parentNode, BusinessName, showorder,userextendparam);
            if (rt.result == 1)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "<script>alert('添加成功!');</script>", false);
                InitModel.Refresh();
                lbTheRoles_SelectedIndexChanged(lbTheRoles, null);
            }
            else if (rt.result == 16)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "<script>alert('添加的业务已存在!');</script>", false);
                return;
            }
            else if (rt.result == 17)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "<script>alert('添加的菜单名称已存在!');</script>", false);
            }
            else if (rt.result == 18)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "<script>alert('菜单索引重复!');</script>", false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "<script>alert('添加失败!');</script>", false);
            }
        }
        else if (RadioOper.SelectedValue == "delete")
        {
            btnDelete_Click(null, null);
        }
        InitBusinessDrodownList();
        CheckChanged(GetChecked());
    }

    protected void RadioOper_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RadioOper.SelectedValue == "modify")
        {
            if (GetChecked() == null)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "<script>alert('请先选择要修改的业务!');</script>", false);
                RadioOper.SelectedIndex = 0;
                return;
            }
            ddlBusinessNames.Enabled = false;
            lbRemark.Text = "";
        }
        else if (RadioOper.SelectedValue == "add")
        {
            btnOk.Enabled = true;
            ddlBusinessNames.Enabled = true;
            lbRemark.Text = "";
        }
        else if (RadioOper.SelectedValue == "delete")
        {
            if (GetChecked() == null)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "<script>alert('请先选择要删除的业务!');</script>", false);
                RadioOper.SelectedIndex = 0;
                return;
            }
            ddlBusinessNames.Enabled = false;
            lbRemark.Text = "";
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        TreeNode tn1 = GetChecked();
        if (tn1 != null && tn1.Value != "-1")
        {
            string Des = tn1.Text;
            string appname = lbTheAppName.SelectedItem.Value;
            string usertype = lbTheRoles.SelectedItem.Value;
            string BusinessName = ddlBusinessNames.SelectedItem.Value.Split(':')[3];
            USWebService Us = new USWebService();
            ResultData rt = Us.DeleteRoleBusiness(Des, usertype, appname, BusinessName);
            if (rt.result == 1)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "<script>alert('删除成功!');</script>", false);
                InitModel.Refresh();
                lbTheRoles_SelectedIndexChanged(lbTheRoles, null);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "<script>alert('删除失败!');</script>", false);
            }

        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), System.DateTime.Now.Ticks.ToString(), "<script>alert('请先选择要删除的业务!');</script>", false);
        }
        CheckChanged(GetChecked());
    }

    protected void ddlBusinessNames_SelectedIndexChanged(object sender, EventArgs e)
    {
        string keyid = ddlBusinessNames.SelectedValue;
        bool bHaveFunc = addBusinessFunction(cblRolesFun);
        cbSelf.Checked = false;
        ddlUser.Visible = false;
        cblUserFun.Visible = false;
        ShowOrder.Text = "";
        if (bHaveFunc)
        {
            cblRolesFun.Visible = true;
            cbSelf.Visible = true;
        }
        else
        {
            cbSelf.Visible = false;
            cblRolesFun.Visible = false;
        }
        txtName.Text = ddlBusinessNames.SelectedItem.Text;
        TreeNodeCollection tnc = TreeView1.Nodes;
        TreeNode tnRt = GetChecked();
        if (tnc[TreeView1.Nodes.Count - 1].ChildNodes.Count > 0 && keyid!="")
        {
            
            if (tnRt != null)
            {
                if (ddlBusinessNames.SelectedItem.Text != tnRt.Text && tnRt.Depth == 1)
                {
                    if (tnRt.ChildNodes.Count > 0)
                    {
                        string[] sv = Regex.Split(tnRt.ChildNodes[tnRt.ChildNodes.Count - 1].Value, SPER, RegexOptions.IgnoreCase);
                        ShowOrder.Text = (Convert.ToInt32(sv[5]) + 1).ToString();
                    }
                    else
                    {
                        ShowOrder.Text = "1";
                    }

                }
                else
                {
                    string[] sv = Regex.Split(tnc[0].ChildNodes[tnc[0].ChildNodes.Count - 1].Value, SPER, RegexOptions.IgnoreCase);
                    ShowOrder.Text = (Convert.ToInt32(sv[5]) + 1).ToString();
                }
            }
            else
            {
                GetOrder(tnc[0].ChildNodes, keyid.Split(':')[2]);
            }
        }
        else
        {
            ShowOrder.Text = "1";
        }
    }

    private void GetOrder(TreeNodeCollection tnc, string ParentName)
    {
        
            foreach (TreeNode tn in tnc)
            {
                string[] sv = Regex.Split(tn.Value, SPER, RegexOptions.IgnoreCase);
                if (sv.Length > 7)
                {
                    if (ParentName == sv[8])
                    {
                        sv = Regex.Split(tnc[tnc.Count - 1].Value, SPER, RegexOptions.IgnoreCase);
                        if (sv.Length > 7)
                        {
                            ShowOrder.Text = (Convert.ToInt32(sv[5]) + 1).ToString();
                            return;
                        }
                    }
                    if (tn.ChildNodes.Count > 0)
                    {
                        GetOrder(tn.ChildNodes, ParentName);
                    }
                }
            }
    }

    protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        changeUserFuns();
    }

    protected void cblUserFun_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cbSelf.Checked)
        {
            string sValue = ddlUser.SelectedItem.Text;
            foreach (ListItem li in cblUserFun.Items)
            {
                if (!li.Selected)
                {
                    sValue = sValue + "," + li.Value;
                }
            }
            ddlUser.SelectedItem.Value = sValue;
        }
    }

    protected void cbSelf_CheckedChanged(object sender, EventArgs e)
    {
        if (cbSelf.Checked)
        {
            
            if (ddlUser.SelectedIndex != -1)
            {
                ddlUser.Visible = true;
                ddlUser.SelectedIndex = 0;
                ddlUser_SelectedIndexChanged(null, null);
                cblUserFun.Visible = true;
            }
        }
        else
        {
            ddlUser.Visible = false;
            cblUserFun.Visible = false;
        }
    }

    private string getSelectedRoleFunctions()
    {
        string rt = "";
        foreach (ListItem li in cblRolesFun.Items)
        {
            if (li.Selected)
            {
                if (rt.Length == 0)
                {
                    rt = li.Value;
                }
                else
                {
                    rt = rt + "," + li.Value;
                }
            }
        }
        return rt;
    }

    private string getSelectedUserFunctions()
    {
        string rt = "";
        if (cbSelf.Checked)
        {
            foreach (ListItem li in ddlUser.Items)
            {
                if (rt.Length == 0)
                {
                    rt = li.Value;
                }
                else
                {
                    rt = rt + "|" + li.Value;
                }
            }
        }
        return rt;
    }

    /// <summary>
    /// 获取这个业务对应的功能点
    /// </summary>
    /// <param name="cblObj"></param>
    protected bool addBusinessFunction(CheckBoxList cblObj)
    {
        lt = (Loginresult)HttpContext.Current.Session["Session"];
        string UserRightName = ReadConfig.TheReadConfig["URAppName"].ToLower();
        bool rt = false;
        USWebService Us = new USWebService();
        string appname = "";
        appname = lt.AppName;
        BL = Us.GetBusiness(appname);
        foreach (BusinessInfo bi in BL)
        {
            if (bi.BusinessDesc == ddlBusinessNames.SelectedItem.Text)
            {
                string sAllFuns = "";
                if (bi.FuncName != "")
                {
                    sAllFuns = bi.funcDesc;
                    if (sAllFuns.Length == 0)
                    {
                        return rt;
                    }
                    string sAllFunsDesc = bi.FuncName;
                    string[] aAllFuns = sAllFuns.Split(',');
                    string[] aAllFunsDesc = sAllFunsDesc.Split(',');
                    cblObj.Items.Clear();
                    cblUserFun.Items.Clear();
                    for (int i = 0; i < aAllFuns.Length; i++)
                    {
                        ListItem li = new ListItem(aAllFunsDesc[i], aAllFuns[i]);
                        li.Selected = true;
                        cblObj.Items.Add(li);
                        li = new ListItem(aAllFunsDesc[i], aAllFuns[i]);
                        li.Selected = true;
                        cblUserFun.Items.Add(li);
                    }

                    rt = true;
                    break;
                }
            }
        }
        return rt;
    }

    /// <summary>
    /// 根据角色获取角色对应的所有用户
    /// </summary>
    private void GetRoleAllUsers()
    {
        string UserRightName = ReadConfig.TheReadConfig["URAppName"].ToLower();
        USWebService us = new USWebService();
        string appname = lbTheAppName.SelectedItem.Value;
        string rolename = "";
        if (lbTheRoles.SelectedIndex != -1)
        {
            rolename = lbTheRoles.SelectedItem.Value;
        }
        List<User> UL = us.GetMatchUserList(appname, rolename);
        ddlUser.Items.Clear();
        MatchUser.Items.Clear();
        cbSelf.Checked = false;
        foreach (User s in UL)
        {
            if (lbTheAppName.SelectedItem.Value == s.AppName)
            {
                ddlUser.Items.Add(new ListItem(s.Name, s.Name));
                MatchUser.Items.Add(new ListItem(s.Name, s.Name));
            }
        }
    }

    /// <summary>
    /// 将角色对应的功能点复制给用户
    /// </summary>
    private void copyRoleToUserFuns()
    {
        for (int i = 0; i < cblRolesFun.Items.Count; i++)
        {
            cblUserFun.Items[i].Selected = cblRolesFun.Items[i].Selected;
            cblUserFun.Items[i].Enabled = cblRolesFun.Items[i].Selected;
        }
    }
    /// <summary>
    /// 更新用户不能使用的功能点
    /// </summary>
    private void changeUserFuns()
    {
        string sValue = ddlUser.SelectedValue;
        string[] aValue = sValue.Split(',');
        string s = "";
        copyRoleToUserFuns();
        for (int j = 1; j < aValue.Count(); j++)
        {
            s = aValue[j];
            foreach (ListItem li in cblUserFun.Items)
            {
                if (li.Value == s)
                {
                    li.Selected = false;
                    break;
                }
            }
        }
    }

    protected void lbTheAppName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlUser.Visible = false;
        cblUserFun.Visible = false;
        cblRolesFun.Visible = false;
        txtName.Text = "";
        ShowOrder.Text = "";
        btnOk.Enabled = true;
        string UserRightName = ReadConfig.TheReadConfig["URAppName"].ToLower();
        //RadioOper_SelectedIndexChanged(null, null);
        ddlBusinessNames.Items.Clear();
        ddlBusinessNames.Items.Add(new ListItem("", ""));
        USWebService Us = new USWebService();
        BL = Us.GetBusiness(lbTheAppName.SelectedItem.Value);
        foreach (BusinessInfo drv1 in BL)
        {
            if (drv1.ID.ToString().Length != 0 && drv1.AppName == lbTheAppName.SelectedItem.Value)
                ddlBusinessNames.Items.Add(new ListItem(drv1.BusinessDesc, drv1.ID + ":" + drv1.AppName + ":" + drv1.Parent +":"+drv1.BusinessName));
        }
        lbTheRoles.Items.Clear();
        List<string> RL = Us.GetRoleSelect(lbTheAppName.SelectedItem.Value);
        for (int i = 0; i < RL.Count; i = i + 2)
        {
            ListItem li = new ListItem();
            li.Text = RL[i + 1];
            li.Value = RL[i];
            lbTheRoles.Items.Add(li);
        }
        if (lbTheRoles.Items.Count > 0)
        {
            lbTheRoles.SelectedIndex = 0;
            lbTheRoles_SelectedIndexChanged(lbTheRoles, null);
        }
        else
        {
            TreeView1.Nodes.Clear();
            TreeNode rtNode = new TreeNode("业务列表", "-1");
            TreeView1.Nodes.Add(rtNode);
        }
        GetRoleAllUsers();

        string ImportAppName = lbTheAppName.SelectedItem.Value.ToLower();
        USWebService us = new USWebService();
        List<String> applist = us.GetImportApp(ImportAppName);

    }
    protected void ImportApp_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

}