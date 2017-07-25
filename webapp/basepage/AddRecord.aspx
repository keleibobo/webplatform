<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddRecord.aspx.cs" Inherits="Public_AddRecord" %>
<%@ Register Src="WebAddItemControl.ascx" TagName="WebAddItemControl" TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <title>新增</title>
    <link rel="stylesheet" href="../css/com.css">
    <link rel="stylesheet" type="text/css" href="themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="themes/icon.css" />
    <script type="text/javascript" src="../js/NewRecord.js"></script>
    <script type="text/javascript" src="../js/jquery-1.8.0.min.js" ></script>
        <script type="text/javascript" src="../js/jquery.easyui.min.js"></script>
    <style type="text/css">
 
    body {
	    margin-left: 20px;
	    margin-top: 20px;
	    margin-right: 20px;
	    margin-bottom: 20px;
	    background-color: #A9DFE7;
        overflow:scroll;
    }
        .td001
        {
            height:35px;
        }
    .STYLE1 {color: #FFFFFF}
 
    </style>

    <script src="../js/public.js" type="text/javascript">
        
    </script>
   
</head>
<body style="overflow:hidden;" onLoad="handleload()"   >   
    <form id="form1" runat="server"   >
        <asp:ScriptManager ID="ScriptManager" EnablePageMethods="true" runat="server">
    </asp:ScriptManager>
    <uc1:WebAddItemControl ID="WebAddItemControl" runat="server" />
    </form>
    <script>
        $("#treeview").html($("#tc1").html());
        $("#tc1").html("");
    </script>
</body>
</html>