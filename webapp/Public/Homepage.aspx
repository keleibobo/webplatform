<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Homepage.aspx.cs" Inherits="Public_Homepage" %>

<%@ Register Src="../basepage/WebUserControlMessage.ascx" TagName="WebUserControlMessage" TagPrefix="uc1" %>
<%@ Import Namespace="System.Data" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%=title %></title>
    <meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
    <asp:PlaceHolder runat="server" ID="comcss"></asp:PlaceHolder>

    <style type="text/css">
        body
        {
            margin-left: 0px;
            margin-top: 0px;
            margin-right: 0px;
            margin-bottom: 1px;
            overflow: hidden;
        }

        .Menu1_0
        {
            background-color: #A9DFE7;
            visibility: hidden;
            display: none;
            position: absolute;
            top: 0px;
        }

        .Menu1_1
        {
            text-decoration: none;
        }
    </style>
    <script src="../js/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/jquery.signalR-1.1.4.min.js"></script>
    <script src="../Signalr/Hubs" type="text/javascript"></script>
    <script src="../js/public.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="../js/Menu.js"> </script>
    <script type="text/javascript" src="../js/public.js"> </script>
    <script type="text/javascript" src="../js/message.js"></script>
    <script type="text/javascript">
        window.EventDisplaySize = "<%=eventdisplaysize%>";
        window.ShowEventWindow = "<%=showeventwindow%>" == "true";
        function GetFrm(sName) {
            var _Frame;
            if (!document.all) {  //火狐中得到IFRAME的对象    
                _Frame = document.getElementById(sName).contentWindow;
            } else {
                _Frame = document.frames[sName];
            }
            return _Frame;
        }
        //单击菜单进入
        var IsExists;
        function f_menuclick(type, obj, menutext) {

            obj.style.background = "url(../images/<%=style%>/line005.gif)";
            document.getElementById("lastmenu").value = obj.id;
            //  var lastmenu = document.getElementById("lastmenu").value;
            //  document.getElementById(lastmenu).style.background = "";
            var surl = obj.getAttribute("surl");
            if (surl.indexOf("?") == 0) return;
            var objframe = GetFrm("myframe");

            var RoleNamepar = "&RoleName=" + document.getElementById("RoleName").value;
            surl += RoleNamepar;
            if (type.indexOf("&tb") > 0) {
                document.getElementById("myframe").src = surl + "&random=" + Math.random();
                return;
            }

            var objframe = GetFrm("myframe");
            if (objframe != null) {
                document.all.myframe.src = surl + "&random=" + Math.random();
            }
        }

        addjs("../js/<%=style%>/Menu.js");
        function addjs(jsrc) {
            var head = document.head || document.getElementsByTagName('head')[0];
            var scriptMenu = document.createElement('script');
            scriptMenu.src = jsrc;
            head.appendChild(scriptMenu);
        }
        $(document).ready(function () {
            setTimeout(function () {
                var obj = document.getElementById('Menu1n0');
                obj.style.background = "url(../images/<%=style%>/line005.gif)";
                document.getElementById("lastmenu").value = obj.id;
            }, 500);
        });
    </script>
</head>
<body scroll="no">
    <form method="post" runat="server" id="form1">

        <!--  <uc1:WebUserControlMessage ID="WebUserControlMessage" runat="server" /> -->
        <input id="lastmenu" name="lastmenu" value="" type="hidden" runat="server" />
        <input id="RoleName" name="RoleName" value="" type="hidden" runat="server" />
        <iframe id="PopFrame" runat="server" style="display: none"></iframe>

        <table style="width: 100%; height: 100%" cellpadding="0" cellspacing="0" border="0" id="tableMenu" background="../images/<%=style%>/photoback.jpg">

            <table width="100%" height="47" border="0" cellpadding="0" cellspacing="0" background="../images/<%=style%>/photoback.jpg">
                <tbody>
                    <tr>
                        <td width="18%">
                            <table width="238" height="47" border="0" align="left" cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr>
                                        <td width="36">&nbsp;</td>
                                        <td width="202" align="center" valign="top">
                                            <img src="../images/<%=style%><%=Appname%>state_grid_s.png" width="202" height="47"></td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                        <td width="64%" align="center" class="tdtitle17" nowrap><%=title %></td>
                        <td width="18%" align="right">
                            <table width="320" height="47" border="0" align="right" cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr>
                                        <td height="7"></td>
                                        <td height="7"></td>
                                    </tr>
                                    <tr>
                                        <td width="300" align="right">
                                            <table width="250" height="28" border="0" cellpadding="0" cellspacing="0">
                                                <tbody>
                                                    <tr>
                                                        <td width="15%" align="center">
                                                            <img src="../images/<%=style%>/登录_s.png" width="26" height="26"></td>
                                                        <td width="85%" align="center" class="tdtitle8">&nbsp;欢迎您！<asp:Label ID="UserInfo" runat="server"></asp:Label>
                                                            &nbsp;<asp:LinkButton ID="LinkButton1" runat="server" CssClass="list1" OnClick="LoginOut_Click">注 销</asp:LinkButton></td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                        <td width="20" height="33" align="right">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td height="7"></td>
                                        <td height="7"></td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>

            <!-- 菜单开始-->
            <tr style="width: 100%; height: 37px">
                <td align="center">
                    <input id="MainItemCount" value="<%= DbUm.mainCount %>" type="hidden" />
                    <table style="width: 100%; height: 37px" border="0" cellpadding="0" cellspacing="0"
                        background="../images/<%=style%>/line02.png">
                        <tr>
                            <td align="center">
                                <!-- 主菜单-->
                                <%=DbUm.menuTable %>
                                <a id="Menu1_SkipLink"></a>
                            </td>
                            <%=DbUm.msgTable %>
                        </tr>
                    </table>
                </td>
            </tr>
            <%=trAddress %>
            <tr style="height: 100%">
                <td>
                    <iframe class="iframeback" style="position: absolute; width: 100%; height: 100%;" marginwidth="0" marginheight="0" scrolling="no"
                        id="myframe" name="myframe" frameborder="0" runat="server"></iframe>
                    <!--     <div style="position:absolute;width: 100%;height:100%;" id="myframe" name="myframe>"></div>-->
                </td>
            </tr>
        </table>
        <script type="text/javascript">
            //<![CDATA[
            var Menu1_Data = new Object();
            Menu1_Data.disappearAfter = 500;
            Menu1_Data.horizontalOffset = 0;
            Menu1_Data.verticalOffset = 0;
            //]]>

            //   
            setH();
            $(window).resize(setH);
            function setH() {
                //   debugger
                var h = document.body.scrollHeight;
                h = $(window).height();
                $("#myframe").height(h - 85);
            }
        </script>
        <div id="messageback" style="position: fixed; right: 0px; bottom: 0px; width: 571px; height: 0px; opacity: 0.0; border: 0;">
            <iframe style="padding: 0; margin: 0; width: 571px; height: 271px; border: 0;"></iframe>
        </div>
    </form>
</body>
</html>
