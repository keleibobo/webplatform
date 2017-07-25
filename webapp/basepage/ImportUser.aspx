<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImportUser.aspx.cs" Inherits="basepage_UploadUser" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="../css/uclist.css" />
    <link rel="stylesheet" href="../css/com.css" />
    <script type="text/javascript" src="../js/jquery-1.8.0.min.js"></script>
</head>
<body style="background-color: #BBE1EA; overflow:scroll;">

    <form id="form1" runat="server">
        <input type="hidden" runat="server" id="hpath" />
        <asp:ScriptManager ID="ScriptManager" EnablePageMethods="true" runat="server">
        </asp:ScriptManager>
        <div style="text-align: center; margin-top: 30px;">
            <asp:FileUpload ID="FileUpload1" runat="server" Width="664px" BorderColor="#66ccff" onChange="selectfile()" />
           <span style="display:none;"> &nbsp;&nbsp;<asp:Button ID="Preview"  runat="server" BorderColor="#66ccff" Text="预览" Width="65px" OnClick="Preview_Click" /></span>
            &nbsp;&nbsp;
        导入项目：<asp:DropDownList ID="lbTheAppName" runat="server" OnSelectedIndexChanged="lbTheAppName_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
            &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Import" runat="server" OnClick="Import_Click" Text="导入用户" BorderColor="#66ccff" Height="26px" />
            <div id="listtable" runat="server" style="margin-top: 50px;"></div>
        </div>
        <script>
            function selectfile() {
                $("#Preview").trigger('click');
            }
        </script>
    </form>
</body>
</html>
