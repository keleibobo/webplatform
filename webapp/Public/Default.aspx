<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Public_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../css/green/themes/default/easyui.css">
    <link rel="stylesheet" type="text/css" href="../css/green/themes/icon.css">
    <script type="text/javascript" src="../js/jquery-1.8.0.min.js"></script>
    <script type="text/javascript" src="../js/jquery.easyui.min.js"></script>
</head>
<body class="easyui-layout">
    
   <%-- <div data-options="region:'west',split:true,title:'West'" style="width: 150px; padding: 10px;">west content</div>
    <div data-options="region:'center',title:'Center'"></div>--%>

    <div data-options="region:'west',split:true" title='123' id='layoutwest' style='width: 270px; height: 100%; overflow: hidden;'>
        <%--<div id='96UTDtBaseSvr.BusinessComponentLayoutCall' style='width: 100%;'></div>--%>
    </div>
    <div data-options="region:'center'" id='layoutcenter' style='width: 100%; height: 100%; overflow: hidden;'>
       <%-- <div id='0BaseSvr.BusinessComponentLayoutCall'></div>
        <div id='116UTDtBaseSvr.BusinessComponentLayoutCall' style='width: 100%;'></div>
        <div id='95UTDtBaseSvr.BusinessComponentLayoutCall' style='width: 100%;'></div>--%>
    </div>
</body>
</html>
