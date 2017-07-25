<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestImage.aspx.cs" Inherits="Public_TestImage" %>
<%@ Register Src="WebUserControlChart.ascx" TagName="WebUserControlChart" TagPrefix="uc5" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="themes/default/easyui.css">
	<link rel="stylesheet" type="text/css" href="themes/icon.css">
<script type='text/javascript' src='../js/jquery-1.8.0.min.js'></script>
 <script type="text/javascript" src="../js/layout.js"></script>
  <script type="text/javascript" src="../js/public.js"></script>
  <script type="text/javascript" src="../js/chart.js"></script>
   <script src='../js/highcharts.js'></script>
    <script src='../js/modules/exporting.js'></script>
    <script type="text/javascript" src="../js/jquery.easyui.min.js"></script>
</head>

	
<body>
    <form id="form1" runat="server">
    <div>
   </div>
    </form>
    
    <div id="component_67"></div>
    <div id="charts"></div>
</body>
<script type="text/javascript" >

    var ctrlIdDesc = "component_67";
    var args = "";
    f_loadchart(ctrlIdDesc, args)
</script>
</html>


