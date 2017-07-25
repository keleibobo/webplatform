<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WebBusinessManageControl.ascx.cs" Inherits="UI_WebBusinessManageControl" %>
<%@ Register TagPrefix="cc1" Namespace="UNLV.IAP.WebControls" Assembly="DropDownCheckList" %>
<style type="text/css">
    body
    {
        overflow: scroll;
        font-size:14px;
    }

    .style1
    {
        height: 20px;
    }

    .auto-style1
    {
        width: 30%;
        height: 30px;
    }

    .auto-style2
    {
        width: 70%;
        height: 30px;
    }
</style>
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<asp:HiddenField runat="server" ID="businessname" />
<div style="width: 100%; height: 95%; margin: 20px auto;">
    <div style="width: 970px; height: 95%; margin: 0 auto;">
    <div style="width:500px; height: 80%; float: left;">
        &nbsp;当前项目：&nbsp;<asp:DropDownList ID="lbTheAppName" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lbTheAppName_SelectedIndexChanged" Width="180px">
        </asp:DropDownList>
        当前在设置的角色:&nbsp;<asp:DropDownList ID="lbTheRoles" runat="server" AutoPostBack="True"
            OnSelectedIndexChanged="lbTheRoles_SelectedIndexChanged" Width="100px">
        </asp:DropDownList>
        <br />
        <fieldset style="height: 90%; overflow:auto; margin-top:15px;">
            <legend>已有权限</legend>
            <asp:TreeView ID="TreeView1" runat="server" ShowLines="True" NodeWrap="true" NodeStyle-CssClass="list4"
                PopulateNodesFromClient="false" ShowCheckBoxes="All" OnTreeNodeCheckChanged="TreeView1_TreeNodeCheckChanged" ForeColor="Black">
                <NodeStyle CssClass="list4" />
            </asp:TreeView>
            <br />
            <br />
            <br />
        </fieldset>
    </div>
    <div style="width: 450px; float: left; margin-top:36px;">
        <div style="width: 100%;">
            <fieldset style=" height:90%; ">
                <legend>添加修改菜单</legend>
                <table style="width: 100%;">
                    <tr>
                        <td class="auto-style1">操作类型
                        </td>
                        <td class="auto-style2">
                            <asp:RadioButtonList ID="RadioOper" runat="server" RepeatDirection="Horizontal" AutoPostBack="True"
                                OnSelectedIndexChanged="RadioOper_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Value="add" onclick="$('#ddlBusinessNames').attr('disabled','false');">添加</asp:ListItem>
                                <asp:ListItem Value="modify" onclick="$('#ddlBusinessNames').attr('disabled','true');">修改</asp:ListItem>
                                <asp:ListItem Value="delete">删除</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>菜&nbsp;&nbsp;单&nbsp;&nbsp;名
                        </td>
                        <td>
                            <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>业&nbsp;&nbsp;务&nbsp;&nbsp;名
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBusinessNames" runat="server" Width="153px"
                                OnSelectedIndexChanged="ddlBusinessNames_SelectedIndexChanged"
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="1">排序索引
                        </td>
                        <td>
                            <asp:TextBox ID="ShowOrder" Width="50px" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="1">角色功能名
                        </td>
                        <td>
                            <asp:CheckBoxList ID="cblRolesFun" runat="server" RepeatDirection="Horizontal">
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="2">功&nbsp;&nbsp;能&nbsp;&nbsp;名
                        </td>
                        <td>
                            <asp:CheckBox ID="cbSelf" runat="server" Text="自定义用户功能" AutoPostBack="True"
                                OnCheckedChanged="cbSelf_CheckedChanged" />
                            <asp:DropDownList ID="ddlUser" runat="server"
                                OnSelectedIndexChanged="ddlUser_SelectedIndexChanged" Visible="False"
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>

                        <td>
                            <asp:CheckBoxList ID="cblUserFun" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="cblUserFun_SelectedIndexChanged"
                                RepeatDirection="Horizontal">
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: center">
                            <asp:Button runat="server" Text="确 定" OnClientClick="return   confirm('确实该操作吗？')"
                                ID="btnOk" OnClick="btnOk_Click" />
                            &nbsp;
                                         <asp:Button ID="btnDelete" Text="删 除" runat="server" Enabled="False" OnClientClick="return   confirm('确实删除该菜单吗？')" OnClick="btnDelete_Click" Visible="False" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: center" class="style1">
                            <asp:Label ID="lbRemark" runat="server" ForeColor="Green"></asp:Label>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>

        <div style="float: left; width: 100%; margin-top: 20px; display:none;">
            <fieldset>
            <legend>第三方权限</legend>
                <asp:DropDownList ID="MatchUser" onchange="ImportChuange()" runat="server">
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;&nbsp;
    <a href="#" class="easyui-linkbutton" data-options="" onclick="ViewselectAll()">浏览全选</a>
                             &nbsp;&nbsp;&nbsp;
    <a href="#" class="easyui-linkbutton" data-options="" onclick="ControlselectAll()">控制全选</a>
                             &nbsp;&nbsp;&nbsp;
    <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-add'" onclick="expandAll()">展开</a>
                             &nbsp;&nbsp;&nbsp;
    <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-remove'" onclick="collapseAll()">收缩</a>
                                &nbsp;&nbsp;&nbsp;
    <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-save'" onclick="Update()">更新</a>
                <br />
                <br />
                <table id="dgRights" class="easyui-treegrid" style="width: 500px; height: 300px"
                    data-options="idField:'id',treeField:'name'">
                    <thead>
                        <tr>
                            <th data-options="field:'name',width:180">名称</th>
                            <th field='view'  width="80"  formatter="viewcheck">浏览</th>
                            <th field='control'  width="80"  formatter="controlcheck">控制</th>
                        </tr>
                    </thead>
                </table>
            </fieldset>
        </div>

    </div>
    </div>
    </div>
