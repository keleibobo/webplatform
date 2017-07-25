<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>登录</title>
</head>
<body style="text-align: center; width:100%;">
    <form id="form1" runat="server">
    <center>
        <br />
        <table class="tab_info">
            <tr>
                <td class="tab_title" colspan="3">
                    测试用户
                </td>
            </tr>
            <tr>
                <td>
                    用户类型
                </td>
                <td>
                    用户名
                </td>
                <td>
                    密码
                </td>
            </tr>
            <tr>
                <td>
                    企业用户
                </td>
                <td>
                    user
                </td>
                <td>
                    user
                </td>
            </tr>
            <tr>
                <td>
                    经贸局
                </td>
                <td>
                    jmj
                </td>
                <td>
                    jmj
                </td>
            </tr>
            <tr>
                <td>
                    管理员
                </td>
                <td>
                    admin
                </td>
                <td>
                    admin
                </td>
            </tr>
        </table>
        <br />
        <br />
        <br />
        <asp:Login ID="Login1" runat="server" CreateUserText="还没有账户" PasswordRecoveryText="找回密码"
            DestinationPageUrl="~/user/homepage.aspx" FailureText="用户名或密码错误" OnLoginError="Login1_LoginError" >
            <LayoutTemplate>
                <table class="tab_info">
                    <tr>
                        <td colspan="2" class="tab_title">
                            登 录
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%;">
                            &nbsp;<asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">用户名</asp:Label>
                        </td>
                        <td style="width: 70%;">
                            &nbsp;
                            <asp:TextBox ID="UserName" Width="120px" runat="server" Text="user"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                ErrorMessage="必须填写“用户名”。" ToolTip="必须填写“用户名”。" ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;<asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">密&nbsp;&nbsp;&nbsp;&nbsp;码</asp:Label>
                        </td>
                        <td>
                            &nbsp;
                            <asp:TextBox ID="Password" Width="120px" runat="server" TextMode="SingleLine" Text="user"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                ErrorMessage="必须填写“密码”。" ToolTip="必须填写“密码”。" ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>     
                    <tr>
                        <td>
                            &nbsp;<asp:Label ID="Label1" runat="server">验证码</asp:Label>
                        </td>
                        <td>
                            &nbsp;
                            <asp:TextBox ID="TxtValidateNumber" Width="50px" runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/VldNum.aspx" ImageAlign="Top"
                                alt="看不清？点击更换" onclick="this.src=this.src+'?'" />
<%--                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TxtValidateNumber"
                                ErrorMessage="必须填写【验证码】" ValidationGroup="Login1" Display="Dynamic">*</asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="【验证码】错误"
                                ControlToValidate="TxtValidateNumber" OnServerValidate="CustomValidator1_ServerValidate"
                                ValidationGroup="Login1" Display="Dynamic">*</asp:CustomValidator>--%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <center>
                                <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="登 录" 
                                    ValidationGroup="Login1" onclick="LoginButton_Click" />
                                <asp:CheckBox ID="RememberMe" runat="server" Text="记住我" />
                            </center>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:HyperLink ID="PasswordRecoveryLink" runat="server" NavigateUrl="~/Account/ChangePassword.aspx">密码重置</asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:HyperLink ID="CreateUserLink" runat="server" NavigateUrl="~/Account/Register.aspx">还没有注册</asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="color: Red;">
                            &nbsp;<asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="Login1" Display="Dynamic"
                                runat="server" />
                            <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </LayoutTemplate>
        </asp:Login>
    </center>
    </form>
</body>
</html>
