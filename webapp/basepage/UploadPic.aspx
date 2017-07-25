<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadPic.aspx.cs" Inherits="Public_UploadPic" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="background-color: #B1CCEF">
    <form id="form1" runat="server">
    <table style="width: 100%;">
        <tr>
            <td style="width:300px;">
                <input id="File1" type="file" style="width: 300px" runat="server" />
            </td>
            <td style="width:80px;">
                <asp:Button ID="btnOk" runat="server" Width="50" Text="上 传" OnClick="btnOk_Click" />
            </td>
            <td style="width:100px;">
                <asp:Label ID="labFileName" runat="server" Text=""></asp:Label>
            </td>
            <td style="width:100px;">
                <asp:Image ID="img1" runat="server" Visible="false" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
