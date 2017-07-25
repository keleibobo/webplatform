using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using AppCode;
using UserRightObj;

public partial class Public_UserMenu : System.Web.UI.Page
{
    private DataView _dvMenu = null;
    private DataView _dvMenuCopy = null;
    int iIndex = 0;
    AppCode.UserMenuModel dbUM = null;
    protected DataView dvMenu
    {
        get
        {
            if (_dvMenu == null)
            {
                _dvMenu = dbUM.GetUserMenu(lbTheRoles.SelectedValue, appname);
            }
            return _dvMenu;
        }
    }
    protected DataView dvMenuCopy
    {
        get
        {
            if (_dvMenuCopy == null)
            {
                _dvMenuCopy = dbUM.GetUserMenuCopy();
            }
            return _dvMenuCopy;
        }
    }
    public string appname = "";
    public string rolename = "";
    protected void Page_Load(object sender, EventArgs e)
    {        
        if (!ValidateSession.Check())
            Response.Redirect(ValidateSession.GetRedirectUrl(1));
        appname =((Loginresult) HttpContext.Current.Session["Session"]).AppName;
        rolename = ((Loginresult)HttpContext.Current.Session["Session"]).RoleName;
        dbUM = new AppCode.UserMenuModel(appname);
        if (!(IsPostBack || IsAsync))
        {
            ddlBusinessNames.Items.Add(new ListItem("", ""));
            //foreach (DataRowView drv1 in dvMenuCopy)
            //{
            //    if (drv1["keyid"].ToString().Length != 0)
            //        ddlBusinessNames.Items.Add(new ListItem(drv1["des"].ToString(), drv1["keyid"].ToString()));
            //}

            AppCode.ValidateUserModel dbVU = new AppCode.ValidateUserModel();
            DataView dv = dbVU.getAllRoles();
            if (dv != null)
            {
                foreach (DataRowView drv1 in dv)
                {
                    lbTheRoles.Items.Add(drv1["RoleName"].ToString());
                }
                lbTheRoles.SelectedIndex = 0;
                lbTheRoles_SelectedIndexChanged(lbTheRoles, null);
            }

            RadioOper_SelectedIndexChanged(null, null);
        }
        //System.Threading.Thread.Sleep(5000);//当前休眠5秒
    }

    private void FillTree(DataView dv1, TreeNodeCollection tnc1)
    {
        TreeNode tn1 = null;
        while (iIndex < dv1.Count)
        {
            if (tn1 == null || tn1.Value.Split(',')[0].Length == dv1[iIndex]["id"].ToString().Length)
            {
                tn1 = new TreeNode(dv1[iIndex]["des"].ToString(), dv1[iIndex]["id"].ToString() + "," + dv1[iIndex]["keyid"].ToString());
                tn1.Collapse();
                tnc1.Add(tn1);
                iIndex++;
            }
            else if (tn1.Value.Split(',')[0].Length < dv1[iIndex]["id"].ToString().Split(',')[0].Length)
            {
                FillTree(dv1, tn1.ChildNodes);
            }
            else if (tn1.Value.Split(',')[0].Length > dv1[iIndex]["id"].ToString().Split(',')[0].Length)
                break;
        }
    }

    protected void CheckChanged(TreeNode tn1)
    {
        string str1 = "";
        if (tn1 != null && tn1.Checked)
        {
            btnOk.Enabled = true;

            //btnModify.Enabled = true;
            //btnModify.ImageUrl = "../images/xgright.gif";
            if (tn1.ChildNodes.Count == 0)
            {
                btnDelete.Enabled = true;
                btnDelete.ImageUrl = "../images/shanchu.gif";
            }
            else
            {
                btnDelete.Enabled = false;
                btnDelete.ImageUrl = "../images/shanchu_gray.gif";
            }
            if (RadioOper.SelectedValue == "modify")
            {
                if (tn1.Depth >= 1)
                {
                    lbID.Text = tn1.Value.Split(',')[0];
                    txtName.Text = tn1.Text;
                    lbKeyid.Value = tn1.Value.Split(',')[1];
                    foreach (DataRowView drv1 in dvMenu)
                    {
                        if (drv1["id"].ToString() == tn1.Value.Split(',')[0])
                            ddlBusinessNames.SelectedValue = drv1["keyid"].ToString();
                    }
                    if (tn1.Parent != null)
                        lbLead.Text = tn1.Parent.Text;
                    else
                        lbLead.Text = "无";
                }
                else
                {
                    btnOk.Enabled = false;
                }
            }
            else if (RadioOper.SelectedValue == "add")
            {
                lbID.Text = dbUM.GetMaxId(tn1.Depth + 1, tn1.Value.Split(',')[0]);
                txtName.Text = "";
                ddlBusinessNames.SelectedIndex = 0;
                lbLead.Text = tn1.Text;
            }
            
            tn1.Expand();
        }
        else
        {
            //btnModify.Enabled = false;
            //btnModify.ImageUrl = "../images/xgright_gray.gif";
            btnDelete.Enabled = false;
            btnDelete.ImageUrl = "../images/shanchu_gray.gif";
            txtName.Text = "";
            lbID.Text = "";
            ddlBusinessNames.SelectedIndex = 0;
            lbLead.Text = "无";
            btnOk.Enabled = false;
            if (tn1 != null)
                tn1.Collapse();
        }

        Check(TreeView1.Nodes, tn1);        
        ddlBusinessNames_SelectedIndexChanged(null, null);        
    }

    private bool isCustomUserFunc()
    {
        return dbUM.IsCustomUserFunct(lbID.Text);
    }
    protected void TreeView1_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
    {
        CheckChanged(e.Node);
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
        TreeView1.Nodes.Clear();
        _dvMenu = null;
        TreeNode rtNode = new TreeNode("系统", "-1");
        TreeView1.Nodes.Add(rtNode);
        iIndex = 0;
        FillTree(dvMenu, rtNode.ChildNodes);
        TreeView1.Attributes.Add("onclick", "postBackByObject()");

        //GetRoleAllUsers();
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        string RoleName = lbTheRoles.SelectedItem.Value;
        string KeyId = ddlBusinessNames.SelectedItem.Value;
        string Des = txtName.Text.Trim();
        TreeNodeCollection tnc = TreeView1.CheckedNodes;
        string ID = tnc[0].Value.Split(',')[0];
        string sfunc = getSelectedRoleFunctions();
        string sUserfunc = getSelectedUserFunctions();
        int bflag = cbSelf.Checked ? 1 : 0;
        if (RadioOper.SelectedValue == "modify")
        {
            //object rt = WSUtil.AddMenu("updatemenu", KeyId, ID, Des, RoleName, sfunc, sUserfunc, "0");
            //if (rt.ToString() == "True")
            //{
            //    ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), System.DateTime.Now.Ticks.ToString(), "alert('修改成功!')", true);
            //    lbTheRoles_SelectedIndexChanged(lbTheRoles, null);
            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), System.DateTime.Now.Ticks.ToString(), "alert('修改失败!')", true);
            //}  
        }
        else if (RadioOper.SelectedValue == "add")
        {
            //object rt = WSUtil.AddMenu("addmenu", KeyId, ID, Des, RoleName, sfunc, sUserfunc, "0");
            //if (rt.ToString() == "True")
            //{                
            //    ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), System.DateTime.Now.Ticks.ToString(), "alert('添加成功!')", true);
            //    lbTheRoles_SelectedIndexChanged(lbTheRoles, null);               
            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), System.DateTime.Now.Ticks.ToString(), "alert('添加失败!')", true);
            //}  
        }
        CheckChanged(GetChecked());
    }

    protected void RadioOper_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RadioOper.SelectedValue == "modify")
        {
            ddlBusinessNames.Enabled = false;
            lbRemark.Text = "请先在左侧选择要修改的菜单";           
        }
        else if (RadioOper.SelectedValue == "add")
        {
            btnOk.Enabled = true;
            ddlBusinessNames.Enabled = true;
            lbRemark.Text = "请先在左侧选择要添加的上级菜单";
        }
        CheckChanged(GetChecked());
    }

    protected void btnDelete_Click(object sender, ImageClickEventArgs e)
    {
        TreeNode tn1 = GetChecked();
        AppCode.UserMenuModel pam1 = new AppCode.UserMenuModel(appname);
        if (tn1 != null)
        {
            string ID = tn1.Value.Split(',')[0];
            //object rt = WSUtil.DeleteMenu("delmenu", ID);
            //if (rt.ToString() == "True")
            //{
            //    ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), System.DateTime.Now.Ticks.ToString(), "alert('删除成功!')", true);
            //    lbTheRoles_SelectedIndexChanged(lbTheRoles, null);
            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), System.DateTime.Now.Ticks.ToString(), "alert('删除失败!')", true);
            //}  
     
        }
        CheckChanged(GetChecked());
    }

    protected void ddlBusinessNames_SelectedIndexChanged(object sender, EventArgs e)
    {
        string keyid = ddlBusinessNames.SelectedValue;
        bool bHaveFunc = addBusinessFunction(cblRolesFun);
        addBusinessFunction(cblUserFun);

        if (bHaveFunc)
        {
            cblRolesFun.Visible = true;
            cbSelf.Visible = true;
            cbSelf.Checked = isCustomUserFunc();
            cbSelf_CheckedChanged(null, null);

            changeRoleFuns();
            //addUserRight();
            changeUserFuns();
        }
        else
        {
            cbSelf.Visible = false;
            cbSelf_CheckedChanged(null, null);
            cblRolesFun.Visible = false;
        }
        txtName.Text = ddlBusinessNames.SelectedItem.Text;
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
            ddlUser.Visible = true;
            ddlUser.SelectedIndex = 0;            
            ddlUser_SelectedIndexChanged(null, null);
            cblUserFun.Visible = true;
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
                    rt = rt + ";" + li.Value;
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
        bool rt = false;
        
        string keyid = ddlBusinessNames.SelectedValue;
        cblObj.Items.Clear();
        if (keyid == "")
            return rt;
        ValidateUserModel vm = new ValidateUserModel();
        string RoleName = lbTheRoles.SelectedItem.Value;
        DataTable dt = vm.getTableInfo("usermenuright", RoleName,appname);
        DataRow[] dr = dt.Select("KeyID='" + keyid+"'");
        string sAllFuns="";
        if (dr.Length > 0)
            sAllFuns = dr[0]["functions"].ToString();
        if (sAllFuns.Length == 0)
        {
            return rt;
        }
        string sAllFunsDesc = dr[0]["functionsdes"].ToString();
        string[] aAllFuns = sAllFuns.Split(',');
        string[] aAllFunsDesc = sAllFunsDesc.Split(',');
        
        for (int i = 0; i < aAllFuns.Length; i++)
        {
            cblObj.Items.Add(new ListItem(aAllFunsDesc[i], aAllFuns[i]));
        }
        if (RadioOper.SelectedValue == "modify")
        {
            DataTable menu = vm.getTableInfo("usermenu", RoleName,appname);
            DataRow[] menudr = menu.Select("keyID='" + keyid + "'");
            if (menudr.Length > 0)
                sAllFuns = menudr[0]["rolecanusefunction"].ToString();
            if (sAllFuns.Length == 0)
            {
                return rt;
            }
            foreach (ListItem li in cblRolesFun.Items)
            {
                if (sAllFuns.Contains(li.Value))
                {
                    li.Selected = true;
                }
            }
        }
        rt = true;
        return rt;
    }

    /// <summary>
    /// 根据角色获取角色对应的所有用户
    /// </summary>
    private void GetRoleAllUsers()
    {
        AppCode.ValidateUserModel dtVU = new AppCode.ValidateUserModel();
        string[] aUser = dtVU.getAllUsersByRoles(lbTheRoles.SelectedValue);
        ddlUser.Items.Clear();
        cbSelf.Checked = false;
        if(aUser!=null)
        foreach (string s in aUser)
        {
            ddlUser.Items.Add(new ListItem(s, s));
        }
    }

    /// <summary>
    /// 重新获取角色针对业务对应的功能点
    /// </summary>
    private void changeRoleFuns()
    {       
        string sFunc = dbUM.GetFunctionsByMenuID(appname,rolename,lbID.Text);
        string[] aFunc = sFunc.Split(',');
        bool buse = false;
        foreach (ListItem li in cblRolesFun.Items)
        {
            buse = false;
            foreach (string s in aFunc)
            {                   
                if (s == li.Value)
                {
                    buse = true;
                    break;
                }
            }
            li.Selected = buse;
        }
    }
    /// <summary>
    /// 将角色对应的功能点复制给用户
    /// </summary>
    private void copyRoleToUserFuns()
    {
        for (int i = 0; i < cblRolesFun.Items.Count;i++ )
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

    /// <summary>
    /// 重新获取用户针对业务对应的功能点，此功能点集合必须小于角色的功能点集合,保存在用户列表中
    /// </summary>
    private void addUserRight()
    {
        string sFunc = dbUM.GetUserFunctionsByMenuID(appname,rolename,lbID.Text);
        string[] aFunc = sFunc.Split(';');
        bool buse = false;
        string svalue ="";
        foreach (ListItem li in ddlUser.Items)
        {
            buse = false;
            svalue = li.Text;
            foreach (string s in aFunc)
            {
                if (s.Length != 0)
                {
                    string[] atemp = s.Split(',');
                    if (atemp[0] == li.Text)
                    {
                        svalue = s;
                        buse = true;
                        break;
                    }
                }
            }            
            li.Selected = buse;
            li.Value = svalue;
        }
    }
}