<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UpdateRecord.aspx.cs" Inherits="Public_UpdateRecord" %>
<%@ Register Src="WebUpdateItemControl.ascx" TagName="WebUpdateItemControl" TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <title>修改</title>
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
      
    .STYLE1 {color: #FFFFFF}
 
    </style>

    <script src="../js/public.js" type="text/javascript">
        
    </script>
   
</head>
<body style="overflow:hidden;" onLoad="handleload()"   >   
    <form id="form1" runat="server"   >
        <uc1:WebUpdateItemControl ID="WebUpdateItemControl" runat="server" />
    </form>
    <script type="text/javascript"></script>
</body>
</html>