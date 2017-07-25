<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditSelfInfo.aspx.cs" Inherits="Public_EditSelfInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../basepage/themes/default/easyui.css">
	<link rel="stylesheet" type="text/css" href="../basepage/themes/icon.css">
    <link rel="stylesheet" href="../css/com.css" />
    <script type="text/javascript" src="../js/jquery-1.8.0.min.js"></script>
    <script type="text/javascript" src="../js/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="../js/UserManage.js"></script>
     <style>
        .txtmargin
        {
            margin-left:40%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
       
                                <div style="padding:20px;height:700px; width:1020px;">
                                <span class="txtmargin"></span>E-mail&nbsp&nbsp&nbsp&nbsp&nbsp：<asp:TextBox runat="server"  id="email" class='TextBorder04' size="20" /><p />
                                <span class="txtmargin"></span>移动端别名：<asp:TextBox runat="server" type="text" id='MobileAlias'  class='TextBorder04' size='30' /><p />
                                <span class="txtmargin"></span>移动端序列号：<asp:TextBox runat="server" type="text" id='MobilePIN'  class='TextBorder04' size='29' /><p />
                                <span class="txtmargin"></span>密码提示：<asp:TextBox runat="server" type="text" id='PasswordQuestion'  class='TextBorder04' size='30' /><p />
                                <span class="txtmargin"></span>密码答案：<asp:TextBox runat="server" type="text" id='PasswordAnswer'  class='TextBorder04' size='30' /><p />
                                <span style="margin-left:45%;"></span><a href="javascript:void(0)" id="btnsave" onclick="SaveSelfInfo()"  class="easyui-linkbutton" data-options="iconCls:'icon-save'">保存</a>
                                <p />
                                <span id="addtips" style="color:red;margin-left:45%;"></span>
                                </div>
    <asp:ScriptManager ID="ScriptManager2" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    </form>
</body>
</html>
