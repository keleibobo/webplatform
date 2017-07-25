<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserMenu.aspx.cs" Inherits="Public_UserMenu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="../css/com.css" />
    <script type="text/javascript">
        function cbSelfCheck() {
            var o = window.document.getElementById("cbSelf");
            var o1 = window.document.getElementById("cblUser");
            if (o.checked) {
                o1.style.display = 'block';
            } else {
             o1.style.display = 'none'; }
    }

        // 点击复选框时触发事件
        function postBackByObject() {
            var o = window.event.srcElement;
            if (o.tagName == "INPUT" && o.type == "checkbox") {
                __doPostBack("", "");
            }
        }
    </script>
</head>
<body> 
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="width: 700px;">
                <tr>
                    <td style="background-color: #CDDFFA; width: 300px; padding-top: 6px" valign="top"
                        rowspan="2">
                        当前在设置的角色:&nbsp;<asp:DropDownList ID="lbTheRoles" runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="lbTheRoles_SelectedIndexChanged">
                        </asp:DropDownList>
                        <br />
                        <br />
                        <asp:TreeView ID="TreeView1" runat="server" ShowLines="false" NodeWrap="true" NodeStyle-CssClass="list4"
                            PopulateNodesFromClient="false" ShowCheckBoxes="All" OnTreeNodeCheckChanged="TreeView1_TreeNodeCheckChanged">
                            <NodeStyle CssClass="list4" />
                        </asp:TreeView>
                    </td>
                    <%--<td width='75 ' style="vertical-align: top;" valign='middle'>
                <asp:ImageButton ID="ImageButton1" onmouseout="this.src='../images/zengjia.gif'"
                    Style='cursor: hand' onmouseover="this.src='../images/zengjia_on.gif'" src='../images/zengjia.gif'
                    runat="server" />
            </td>
            <td width='75 ' style="vertical-align: top;" align='center' valign='middle'>
                <asp:ImageButton ID="btnModify" onmouseout="if(document.getElementById('btnDelete').disabled==false)this.src='../images/xgright.gif'"
                    Style='cursor: hand' onmouseover="if(document.getElementById('btnDelete').disabled==false)this.src='../images/xgright_on.gif'"
                    ImageUrl='../images/xgright_gray.gif' runat="server" Enabled="False" />
            </td>--%>
                    <td width='100 ' style="vertical-align: top;" align='center' valign='middle'>
                        <asp:ImageButton ID="btnDelete" onmouseout="if(document.getElementById('btnDelete').disabled==false)this.src='../images/shanchu.gif'"
                            Style='cursor: hand' onmouseover="if(document.getElementById('btnDelete').disabled==false)this.src='../images/shanchu_on.gif'"
                            ImageUrl='../images/shanchu_gray.gif' runat="server" Enabled="False" 
                            OnClientClick="return   confirm('确实删除该菜单吗？')" onclick="btnDelete_Click" />
                    </td>
                    <td width='300 '>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top" colspan="2">
                        &nbsp;
                        <fieldset>
                            <legend>添加修改菜单</legend>
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 30%">
                                        操作类型
                                    </td>
                                    <td style="width: 70%">
                                        <asp:RadioButtonList ID="RadioOper" runat="server" RepeatDirection="Horizontal" AutoPostBack="True"
                                            OnSelectedIndexChanged="RadioOper_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="add" onclick="document.getElementById('ddlBusinessNames').disabled=false;">添加</asp:ListItem>
                                            <asp:ListItem Value="modify" onclick="document.getElementById('ddlBusinessNames').disabled=true;">修改</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        菜&nbsp;&nbsp;单&nbsp;&nbsp;ID
                                    </td>
                                    <td>
                                        <asp:HiddenField ID="lbKeyid" runat="server" />
                                        <asp:Label ID="lbID" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        上级菜单
                                    </td>
                                    <td>
                                        <asp:Label ID="lbLead" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        菜&nbsp;&nbsp;单&nbsp;&nbsp;名
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        业&nbsp;&nbsp;务&nbsp;&nbsp;名
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlBusinessNames" runat="server" 
                                            onselectedindexchanged="ddlBusinessNames_SelectedIndexChanged" 
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="1">
                                        角色功能名
                                    </td>
                                    <td>
                                        <asp:CheckBoxList ID="cblRolesFun" runat="server" RepeatDirection="Horizontal" >
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2">
                                        功&nbsp;&nbsp;能&nbsp;&nbsp;名
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbSelf" runat="server" Text="自定义用户功能" AutoPostBack="True" 
                                            oncheckedchanged="cbSelf_CheckedChanged" />
                                        <asp:DropDownList ID="ddlUser" runat="server"  
                                            onselectedindexchanged="ddlUser_SelectedIndexChanged" Visible="False" 
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>                                  
                                                                        
                                    <td>
                                        <asp:CheckBoxList ID="cblUserFun" runat="server" AutoPostBack="True" 
                                            onselectedindexchanged="cblUserFun_SelectedIndexChanged" 
                                            RepeatDirection="Horizontal" >
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: center">
                                        <asp:Button runat="server" Text="确 定" OnClientClick="return   confirm('确实该操作吗？')"
                                            ID="btnOk" OnClick="btnOk_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: center">
                                        <asp:Label ID="lbRemark" runat="server" ForeColor="Green"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
