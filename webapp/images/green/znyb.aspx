<%@ Page Language="C#" AutoEventWireup="true" CodeFile="znyb.aspx.cs" Inherits="Public_dnydt" %>
<%@ Import Namespace="System.Data" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="../js/jquery-1.8.3.min.js"></script>
    <link rel="stylesheet" href="../css/com.css" />
    <script type="text/javascript">
        function GetSvgData() {

            PageMethods.GetSvgData(document.getElementById("SelectNode").value, function (result) {

                document.getElementById("dvSvg").innerHTML = result;
            })
        }
        function update() {
            PageMethods.GetUpdateData(document.getElementById("SelectNode").value, function (result) {
                //document.getElementById("dvSvg").innerHTML = result;
                var aStr = result.split(";");
                var aStr1;
                for (var i = 0; i < aStr.length; i++) {
                    aStr1 = aStr[i].split(",");
                    if (aStr1[0].indexOf("OtherClass") >= 0) {
                        updateType1(aStr1[0], aStr1[1], aStr1[2]);
                    }
                    else if (aStr1[0].indexOf("AnalogClass") >= 0) {
                        updateType2(aStr1[0], aStr1[1], aStr1[2]);
                    }
                }
                updateType2("AnalogClass", "15", "12121");
            })
        }

        function updateType1(type, id, value) {
            theSquare = document.getElementById(id); // Set this custom property after the page loads.
            if (theSquare) {
                var use = theSquare.getElementsByTagName("use");
                if (use[0]) {
                    //  alert(cnodes)
                    var hrefs = "#" + type + "@" + value.replace(".000", ""); 
                    try {
                        use[0].setAttributeNS("http://www.w3.org/1999/xlink", "href", hrefs);
                    }
                    catch (ex) { }
                }
            }
        }
        function updateType2(type, id, value) {

            theSquare = document.getElementById(id); // Set this custom property after the page loads.
            if (theSquare) {
                var txt = theSquare.getElementsByTagName("text");
                if (txt[0]) {
                    var tspan = txt[0].getElementsByTagName("tspan");
                    if (tspan[0]) {
                        tspan[0].innerHTML = value;
                    }
                }
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input id="SelectNode" name="SelectNode" type="hidden" runat="server" value="1" />
    <input id="AutoRefreshTime" name="SelectNode" type="hidden" runat="server" value="1" />
    <asp:ScriptManager ID="ScriptManager" EnablePageMethods="true" runat="server">
    </asp:ScriptManager>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#B1CCEF">
        <tr>
            <td style="width: 200px; border: 1px; border-color: Blue; vertical-align: top;">
                <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#B1CCEF">
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TreeView ID="TreeView1" runat="server" NodeWrap="true" NodeStyle-CssClass="list4"
                                PopulateNodesFromClient="false">
                            </asp:TreeView>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <table width="100%" height="29" border="0" cellpadding="0" cellspacing="0" bgcolor="#B1CCEF">
                    <tr>
                        <td width="51%">
                            <table width="100%" height="29" border="0" id="tableaddress" runat="server" cellpadding="0"
                                cellspacing="0">
                                <tr>
                                    <td height="5" align="right">
                                    </td>
                                    <td height="5">
                                    </td>
                                    <td width="10" height="5">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="32" height="5" align="right" valign="middle">
                                        <img src="../images/dot.gif" width="16" height="16">
                                    </td>
                                    <td id="Td1" height="21" align="left" valign="middle" runat="server">
                                        &nbsp;您的当前位置<span class="texttitle" id="SiteAddress" runat="server"> > 智能压板 </span>
                                    </td>
                                    <td width="10" height="5">
                                        <img src="../images/none.gif" width="10" height="10">
                                    </td>
                                </tr>
                                <tr>
                                    <td height="3">
                                    </td>
                                    <td height="3">
                                    </td>
                                    <td height="3">
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td width="46%" align="right" id="righttd" runat="server">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td id="dvSvg" runat="server">
                                <div>
        <svg id="mySVG" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink"
        width="700px" height="700px" onload="initialize()">        
            <g id="WorkSpace" transform="matrix(1, 0, 0, -1, 350, 350)">
              <rect x="10" y="10" width="50" height="50" stroke="red" fill="blue" />
                <circle cx="100" cy="40" r="20" stroke="red" fill="blue" />
            </g>
        </svg>
    </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
    <script type="text/javascript">

        /*firefox----这段js重新封装了event对象，经验证可以在火狐下支持！----*/

        function __firefox() {

            HTMLElement.prototype.__defineGetter__("runtimeStyle", __element_style);

            window.constructor.prototype.__defineGetter__("event", __window_event);

            Event.prototype.__defineGetter__("srcElement", __event_srcElement);

        }

        function __element_style() {

            return this.style;

        }

        function __window_event() {

            return __window_event_constructor();

        }

        function __event_srcElement() {

            return this.target;

        }

        function __window_event_constructor() {

            if (document.all) {

                return window.event;

            }

            var _caller = __window_event_constructor.caller;

            while (_caller != null) {

                var _argument = _caller.arguments[0];

                if (_argument) {

                    var _temp = _argument.constructor;

                    if (_temp.toString().indexOf("Event") != -1) {

                        return _argument;

                    }

                }

                _caller = _caller.caller;

            }

            return null;

        }

        if (window.addEventListener) {

            __firefox();

        }

        /*end firefox------------------------------------------------*/  
  
    </script>
    <script type="text/javascript">

        //单击树的节点
        function TreeView_SelectNode(data, node, nodeId) {

            setReturnValueFalse();  //取消单击回发事件
            document.getElementById("SelectNode").value = nodeId.replace("TreeView1t", "");

            document.getElementById("SiteAddress").innerHTML = " > 智能压板 >> " + node.innerHTML;

            GetSvgData();
            // document.getElementById('Frm1').contentWindow.f_load();
            //  toUrl(sSelectNode, sSelectNodeName, sSelectNodeType, document.getElementById("URLtype").value);



        }
        function setReturnValueFalse() {
            if (document.all) {
                window.event.returnValue = false;
            }
            else {
                event.preventDefault();
            }
        }
        //GetSvgData();
        var AutoRefreshTime = document.getElementById("AutoRefreshTime").value;
        //setInterval(update, 1000 * AutoRefreshTime);
    </script>
</body>
</html>
