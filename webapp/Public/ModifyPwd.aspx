<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ModifyPwd.aspx.cs" Inherits="Public_Admin_ModifyPwd" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../basepage/themes/default/easyui.css">
	<link rel="stylesheet" type="text/css" href="../basepage/themes/icon.css">
    <link rel="stylesheet" href="../css/com.css" />
    <script src="Login/js/JScript.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/jquery-1.8.0.min.js"></script>
    <script type="text/javascript" src="../js/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="../js/UserManage.js?1"></script>
        <script type="text/javascript" src="../js/JScript.js"></script>
    <script type="text/javascript" src="../js/public.js"></script>
    <style>
        .txtmargin
        {
            margin-left:35%;
        }
        #area
        {
            font-weight: bold;
            color: #003331;
            font-size: 10.5pt;
        }
    </style>
</head>
<body >
    <form id="form1" runat="server">
    <div id="area" style="margin:20px;padding-top:20px; background-color:white; height:95%;">
        <div style="width:300px;margin:0 auto">
                原始密码：<input id='OPWD' type="password" class='TextBorder04' size='30' /><p />
                   新&nbsp&nbsp密&nbsp&nbsp码：<input type="password" id='NEWPWD' class='TextBorder04' size='30' /><p />
                    重复输入：<input  id='NEWPWD2' onblur="pwdonblur()" type="password" class='TextBorder04' size='30' /><p />
           </div>
        <div style="cursor:pointer;width:80px;margin:0 auto" id="btnsave" onclick="modifyselfpwd()" onmouseout="changeoutimg(this)" onmouseover="changeonimg(this)" >
        <%=savebutton %>
                  </div>
        <div style=" width:210px; margin:0 auto;">
                    <p></p>
                    <span id="addtips" style="color:red;padding-left:20px;"></span>
            </div>
    </div>
         <asp:ScriptManager ID="ScriptManager2" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    </form>
    <script>
        var x = $('html')[0].offsetHeight;
        $('#area').height(x - 80);
    </script>
</body>
</html>
