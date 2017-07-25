<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Query.aspx.cs" Inherits="UI_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <asp:placeholder runat="server" ID="comcss"></asp:placeholder>
    <style type="text/css">
        body
        {
            font-family: "Verdana","Arial", "Helvetica", "sans-serif","宋体" !important;
         
            margin-left: 0px;
            margin-right: 0px;
            margin-bottom: 1px;
            margin-top: 0px;
        }

        html, body
        {
            height: 100%;
            margin: 0;
        }

        #Condition0_0
        {
            position: absolute;
            width: 100%;
        }
    </style>
    <script src="../js/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/jquery.signalR-1.1.4.min.js" ></script>
    <script type="text/javascript" src="../Signalr/Hubs" ></script>
    <script type="text/javascript" src="../js/message.js"></script>
    <script type="text/javascript" src="../js/svg-base.js"></script>
    <script type="text/javascript" src="../js/ucList.js"></script>
    <script type="text/javascript" src="../js/public.js"></script>
    <script type="text/javascript" src="../js/jquery.mousewheel.js"></script>
    <script type="text/javascript" src="../js/jquery.easyui.min.js"></script>
    <script type="text/javascript" src='../js/highcharts.js'></script>
    <script type="text/javascript" src='../js/modules/exporting.js'></script>
    <script type="text/javascript" src="../js/chart.js?1"></script>
    <script type="text/javascript" src="../js/af.js?1"></script>
    <script type="text/javascript" src="../js/jquery.parser.js"></script>
    <script type="text/javascript" src="../js/easyui-lang-zh_CN.js"></script>
    <script type="text/javascript" src="../js/WdatePicker.js"></script>
    <script type="text/javascript" src="../js/NewRecord.js"></script>
    <script type="text/javascript" src="../js/layout.js?123"></script>

    <script type="text/javascript">
        window.LoginTimeout = "<%=timeout%>";
        var file = "../js/ie8/svg.js";
        var CssPath = "../css/<%=style%>/scrollbar_ie.css";
        var version = document.documentMode;
        if ($.browser.msie || $.browser.mozilla) {
            if (version == 9) {
                file = "../js/ie9/svg.js";
            }
            else if (version == 10) {
                file = "../js/ie10/svg.js";
            }
            else if (version == 11) {
                file = "../js/ie11/svg.js";
            }
        }
        

        //将css文件添加到头中
       // if ($.browser.msie) {
            $("head").append("<link>");
            css = $("head").children(":last");
            css.attr({ rel: "stylesheet", type: "text/css", href: CssPath });
       // }
            var scriptNode = document.createElement('script');
            scriptNode.src = file + '?t=' + new Date().getTime(); /*附带时间参数，防止缓存*/
            var head = document.head || document.getElementsByTagName('head')[0];
            head.appendChild(scriptNode);

        var appname;
        appname = "<%=appname%>";
        var refData;
        
        function layoutresize() {

            setTimeout(function () {
                $("#weblayout").layout("resize");
            }, 500);

            if ($("svg").length > 0)
                SVGInit();
        }
    </script>
</head>
<body class="easyui-layout" onresize="layoutresize()" onclick="RefreshTime();">

    <div style="display:none;">
    <div id="win" data-options="onClose:function(){stopRealplay();}" class="easyui-window" closed="true" shadow="false" minimizable="false" maximizable="false" collapsible="false" resizable="false" title="实时预览" data-options="iconCls:'icon-save'" style="width:760px;height:537px; background-color:#88D3D8;">
    <div id="ocxpanel" style=" width:600px; height:491px; float:left; margin-top:6px; margin-left:6px;"></div>
    <div id="controlpanel" style=" width:137px; float:right; margin-top:8px; margin-left:0px; padding-left:0px; text-align:center; ">
	<table width='135' border="0" cellpadding="0" cellspacing="0" style="">
    <tr>
    <td  align="center" valign="top">
	
	<!--云台控制 部份-->
	<table  border="0" cellpadding="0" cellspacing="0">
      <tr>
        <td colspan="2" align="center"><div><img id="Img1" src="back.png" border="0" /><div style="position:absolute;float:left;top:44px;right:50px;font-size:14px;font-weight:bold;">控制操作</div></div></td>
      </tr>
        <tr>
        <td colspan="2" height="23" align="center"></td>
      </tr>
      <tr>
        <td colspan="2" align="center"><img id="controlmap" src="kongzhiback.gif" border="0"  usemap="#map" />
          <map name="map" id="map">
            <!--上-->      
            <area shape="poly" coords="63,61,106,19,102,15,97,11,93,9,87,5,82,3,74,1,64,0,52,1,44,2,35,6,26,12,21,18" onmousedown="PTZControl('up',0)" onmouseup="PTZControl('up',1)" href="#" />
			<!--下-->
            <area shape="poly" coords="64,62,19,107,30,115,39,119,54,123,67,123,78,122,87,119,94,115,100,111,107,103" onmousedown="PTZControl('down',0)" onmouseup="PTZControl('down',1)" href="#" />
			 <!--左-->
            <area shape="poly" coords="64,62,20,17,12,27,5,39,0,54,-1,65,3,80,7,88,9,94,13,100,19,106"  onmousedown="PTZControl('left',0)" onmouseup="PTZControl('left',1)" href="#" />
			<!-- 右-->
           <area shape="poly" coords="63,61,107,103,113,96,119,86,121,78,123,69,123,58,123,51,121,43,118,34,112,24,106,19"  onmousedown="PTZControl('right',0)" onmouseup="PTZControl('right',1)" href="#" />
          </map></td>
        </tr>
      <tr>
        <td height="70" width="65" align="center"><a href="#"><img id="zoomin" src="fanda.gif"  border="0"  onmousedown="PTZControl('zoomin',0)" onmouseup="PTZControl('zoomin',1)" /></a></td>
        <td  align="center"><a href="#"><img id="zoomout" src="shuoxiao.gif"  border="0"  onmousedown="PTZControl('zoomout',0)" onmouseup="PTZControl('zoomout',1)" /></a></td>
      </tr>
    </table>
	<!--云台控制 部份end-->	
	
	<!-- 选择预位置 部份-->	
	<table  border="0" cellpadding="0" cellspacing="0" style=" margin-top:30px;">
      <tr>
        <td  colspan="2" align="left" style="font-size:14px; font-weight:bold; color:#2C2C2C ;">
        <span style=" margin-left:25px; margin-top:36px; ">预位置：</span>
        <br />
        <br />
          <select id="presetList" style=" " onchange="presetSelect()">
          <option value="">请选择预置点</option>
        </select></td>
      </tr>
    </table>
	<!--选择预位置 部份end-->
    </td>
	</tr>
    </table>
    </div>
	</div>
    </div>
    <form id="form1" runat="server" style="width: 100%; height: 100%;">
        <input name="CurrentId" id="CurrentId" value="0" type="hidden" />
        <input id="ComponentId" type="hidden" runat="server" />
        <input id="nodeid" name="nodeid" type="hidden" runat="server" value="1" />


        <input id="menutext" runat="server" type="hidden" />
        <input id="flashparm" type="hidden" runat="server" />
        <input name="sWhere" id="sWhere" type="hidden" />
        <input name="link" id="link" value="0" type="hidden" runat="server" />
        <input name="KeyFieldName" id="KeyFieldName" value="" type="hidden" />
        <input name="ViewName" id="ViewName" value="" type="hidden" runat="server" />
        <input name="cell" id="cell" value="" type="hidden" runat="server" />
        <input name="SE" id="SE" value="" type="hidden" runat="server" />
        <input name="sDate" id="sDate" value="" type="hidden" runat="server" />
        <input name="Role" id="Role" value="" type="hidden" runat="server" />
        <input name="ToExcel" id="ToExcel" value="0" type="hidden" />
        <input id="otherid" name="otherid" type="hidden" runat="server" />
        <input id="stype" name="stype" type="hidden" runat="server" />
        <input id="spagetype" name="spagetype" type="hidden" runat="server" />
        <input id="svgnodeid" type="hidden" runat="server" />
        <table class="iframeback" width='100%' height='29' border='0' cellpadding='0' cellspacing='0'>
            <tbody>
                <tr>
                    <td width='34%'>
                        <table width='100%' height='30' border='0' cellpadding='0' cellspacing='0'>
                            <tbody>
                                <tr>
                                    <td height='6' align='right'></td>
                                    <td height='6'></td>
                                    <td width='10' height='6'></td>
                                </tr>
                                <tr>
                                    <td width='30' height='5' align='right' valign='middle'>
                                        <img src='../images/<%=style%>/dot.png' width='16' height='16' /></td>
                                    <td height='21' align='left' valign='middle' nowrap>&nbsp;当前位置<span class='texttitle' id='SiteAddress' runat='server'></span></td>
                                    <td width='10' height='5'>
                                        <img src='../images/<%=style%>/none.gif' width='10' height='10' /></td>
                                </tr>
                                <tr>
                                    <td height='3'></td>
                                    <td height='3'></td>
                                    <td height='3'></td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                    <td width='30%'>&nbsp;</td>
                    <td width='1%'>
                        <img src='../images/<%=style%>/none.gif' width='10' height='10' /></td>
                    <td width='32%' align='right'>
                        <table width='488' height='24' border='0' cellpadding='0' cellspacing='0'>
                            <tbody>
                                <tr>
                                    <td width='5'>
                                        <img src='../images/<%=style%>/none.gif' width='8' height='10' /></td>
                                    <td width='80' align='center'>&nbsp;</td>
                                    <td width='22' align='center'>
                                        <img src='../images/<%=style%>/none.gif' width='22' height='10' /></td>
                                    <td width='80' align='center'>&nbsp;</td>
                                    <td width='22'>
                                        <img src='../images/<%=style%>/none.gif' width='22' height='10' /></td>
                                    <td width='80' align='center'>&nbsp;</td>
                                    <td width='22'>
                                        <img src='../images/<%=style%>/none.gif' width='22' height='10' /></td>
                                    <td width='80' align='center'>&nbsp;</td>
                                    <td width='22'>
                                        <img src='../images/<%=style%>/none.gif' width='22' height='10' /></td>
                                    <td width='80' align='center' onclick='showtree()'>
                                        <table width='80' height='24' border='0' cellpadding='0' cellspacing='0' style='cursor: pointer; display: none' id='treebutton'>
                                            <tbody>
                                                <tr onmouseout='changeoutimg(this)' onmouseover='changeonimg(this)'>
                                                    <td width='8' align='right'>
                                                        <img src='../images/<%=style%>/button3_l.png' width='8' height='24' /></td>
                                                    <td width='20' align='center' background='../images/<%=style%>/button3_m.png'>
                                                        <img src='../images/<%=style%>/树状栏.png' width='20' height='20' /></td>
                                                    <td width='43' align='left' background='../images/<%=style%>/button3_m.png'>
                                                        <table width='100%' height='24' border='0' cellpadding='0' cellspacing='0'>
                                                            <tbody>
                                                                <tr>
                                                                    <td height='3'></td>
                                                                </tr>
                                                                <tr>
                                                                    <td width='44' height='21' align='center'>树状栏</td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                    <td width='8' align='left'>
                                                        <img src='../images/<%=style%>/button3_r.png' width='8' height='24' /></td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                    <td width='5'>
                                        <img src='../images/<%=style%>/none.gif' width='8' height='10' /></td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                    <td width='2%' align='right'>
                        <img src='../images/<%=style%>/朝上隐藏.gif' id="conditionhide" width='21' height='21' style='cursor: pointer' onclick="f_imghide(this,'<%=style %>')" /></td>
                </tr>
            </tbody>
        </table>


        <div runat="server" id="weblayout" class="easyui-layout" style="width: 100%; height: 100%;" fit='true'>
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <div>
            <asp:PlaceHolder ID="PlaceHolderWebUserControl" runat="server"></asp:PlaceHolder>
        </div>
    </form>

    <script type="text/javascript">
        var refData = 0;
        var configPageSize = "<%=ReadConfig.TheReadConfig["pagesizelist"]%>";
        function loadsvg() {
            appname = "<%=appname%>";
            refData = "<%=ReadConfig.TheReadConfig["refreshData"]%>";
             f_loadsvg(appname, refData);
         }

         function f_loadhtml(ctrlIdDesc, args) {
             var cid = ctrlIdDesc.split('_')[1];
             var fname = "iframe_" + cid;
             var sPara=f_conditon(args);
             var fdoc;
             var src = "../basepage/HandlerHTML.ashx?cid=" + cid + "&args=" + sPara;
             if (document.all) {//IE  
              if(document.frames[fname]){
                 document.frames[fname].location = src;
                 fdoc = document.frames[fname].document;
                 }
             } else {//Firefox or Chrome   
                 fdoc = document.getElementById(fname).src = src;
             }

         }
 
    </script>
    <div id="ModalWindow" class="easyui-window" title=" " closed="true" minimizable="false" maximizable="false" collapsible="true" resizable="false" data-options="iconCls:'icon-save'" style="width:500px;height:200px;padding:5px; "></div>
    
</body>
</html>