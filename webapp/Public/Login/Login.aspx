<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="user_Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>系统登陆</title>
   <asp:placeholder runat="server" ID="comcss"></asp:placeholder>
    <link rel="stylesheet" type="text/css" href="../../css/green/themes/default/easyui.css">
    <link rel="stylesheet" type="text/css" href="../../css/green/themes/icon.css">
    <script language="JavaScript" type="text/javascript" src="../../js/JScript.js"></script>
    <script type="text/javascript" src="../../js/jquery-1.8.0.min.js"></script>
    <script type="text/javascript" src="../../js/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="../../js/public.js"></script>
    <script type="text/javascript" src="../../js/jquery.cookie.js"></script>
    <style>
        #namearea div {
            margin: 3px;
            font-size: 15px;
            height: 16px;
            width: 155px;
        }
        .version_number{
            text-align:right;
            font-size:small;
            color:darkcyan;
        }
    </style>
    <script language="JavaScript" type="text/JavaScript">
        var rememberuser = "<%=RememberUser%>" == "true";
        var seperator = "-ut_ut-";
        var nameArray = [];
        var pwdArray = [];
        $(document).ready(function () {
            if (rememberuser) {
                $("#UserNameInput").html('<select id="TxtUserName" style="width:170px;border:0;" /><div id="namearea"></div>');
                $('#TxtUserName').combo({
                    required: false,
                    editable: true
                });
                AutoInput();
            }
            else {
                $("#UserNameInput").html('<input type="text" id="TxtUserName" name="TxtUserName" size="26" class="TextBorder">');
            }
            if (!rememberuser) {
                document.getElementById("TxtUserName").focus();
            } else {
                $.parser.parse($("#TxtUserName"));
            }
        });

        //自动填充用户名密码
        function AutoInput() {
            if ($.cookie("username") != undefined) {
                nameArray = $.cookie("username").split(seperator);
                pwdArray = $.cookie("psw").split(seperator);
                var panel = '';
                for (var i = 0; i < nameArray.length; i++) {
                    if (nameArray[i] != "") {
                        panel += '<div><span>' + nameArray[i] + '<span></div>';
                    }
                    if (i == nameArray.length - 1) {
                        $('#TxtUserName').combo('setText', nameArray[i-1]);
                        $("#TxtPsw").val(pwdArray[i-1]);
                    }
                }
                $("#namearea").html(panel);
                $('#namearea').appendTo($('#TxtUserName').combo('panel'));

                $('#namearea div').click(function () {
                    var s = $(this).text();
                    $('#TxtUserName').combo('setText', s).combo('hidePanel');
                    for (var i = 0; i < nameArray.length; i++) {
                        if (nameArray[i] == s) {
                            $("#TxtPsw").val(pwdArray[i]);
                            break;
                        }
                    }
                }).hover(function () {
                    //  $(this).css("background-color", "#E8E8EC");
                    $(this).append("<img src='../../css/green/themes/icons/cancel.png' style='float:right;' onclick='DeleteCacheUser(\"" + $(this).text() + "\",event,this)'>");
                }, function () {
                    $(this).find("img").remove();
                    //  $(this).css("background-color", "white");
                });
            }
        }

        function DeleteCacheUser(username, ev, element) {
            if (username ==$('#TxtUserName').combo('getText') ) {
                $('#TxtUserName').combo('setText', "");
                $("#TxtPsw").val("");
            }
            for (var i = 0; i < nameArray.length; i++) {
                if (nameArray[i] == username) {
                    nameArray[i] = "";
                    pwdArray[i] = "";
                    break;
                }
            }
            var namestr = "";
            var pwdstr = "";
            for (var i = 0; i < nameArray.length - 1; i++) {
                    namestr += nameArray[i] + seperator;
                    pwdstr += pwdArray[i] + seperator;
            }

            $.cookie("username", namestr, { expires: 365 });
            $.cookie("psw", pwdstr, { expires: 365 });
            $(element).parent().remove();

            var e = (ev) ? ev: window.event;
            if (window.event) {
                e.cancelBubble = true;
            } else {
                e.stopPropagation();
            }
        }

        document.onkeydown = function(e)
        {
            var theEvent = window.event || e;
            var code = theEvent.keyCode || theEvent.which;
            if(code == 13)
            {
                document.getElementById('btnload').click();
            }
        }
        function MM_preloadImages() { //v3.0
            var d = document; if (d.images) {
                if (!d.MM_p) d.MM_p = new Array();
                var i, j = d.MM_p.length, a = MM_preloadImages.arguments; for (i = 0; i < a.length; i++)
                    if (a[i].indexOf("#") != 0) { d.MM_p[j] = new Image; d.MM_p[j++].src = a[i]; }
            }
        }

        function MM_swapImgRestore() { //v3.0
            var i, x, a = document.MM_sr; for (i = 0; a && i < a.length && (x = a[i]) && x.oSrc; i++) x.src = x.oSrc;
        }

        function MM_findObj(n, d) { //v4.01
            var p, i, x; if (!d) d = document; if ((p = n.indexOf("?")) > 0 && parent.frames.length) {
                d = parent.frames[n.substring(p + 1)].document; n = n.substring(0, p);
            }
            if (!(x = d[n]) && d.all) x = d.all[n]; for (i = 0; !x && i < d.forms.length; i++) x = d.forms[i][n];
            for (i = 0; !x && d.layers && i < d.layers.length; i++) x = MM_findObj(n, d.layers[i].document);
            if (!x && d.getElementById) x = d.getElementById(n); return x;
        }

        function MM_swapImage() { //v3.0
            var i, j = 0, x, a = MM_swapImage.arguments; document.MM_sr = new Array; for (i = 0; i < (a.length - 2) ; i += 3)
                if ((x = MM_findObj(a[i])) != null) { document.MM_sr[j++] = x; if (!x.oSrc) x.oSrc = x.src; x.src = a[i + 2]; }
        }
        function VerifyTxt() {

            document.getElementById('LblFaultShow').innerText = "";
            if (document.getElementById('TxtUserName').value == "" && $('#TxtUserName').combo('getText') =="") {
                document.getElementById('LblFaultShow').innerText = " 必须填写“用户名”。";
                return;
            }
            
            var userName = document.getElementById('TxtUserName').value == "" ? $('#TxtUserName').combo('getText') : document.getElementById('TxtUserName').value;
            var value = document.getElementById('TxtPsw').value;
            var psw = hex_md5(value);
            //document.getElementById('TxtPsw').value = psw;
            var apdom = document.getElementById("AppNameDw");
            var appname;
            if (apdom != null)
                appname = document.getElementById("AppNameDw").value;
            else
                appname = "";

            var usertype = "";
            if (document.getElementById("logintype") != null) {
                usertype = document.getElementById("logintype").value;
            }

            var VldNum = "";
            if (document.getElementById('TxtVerifier') != null) {
                VldNum = document.getElementById('TxtVerifier').value;
            }
            var sip = "<%=userhostip %>";
            var sname = "<%=userhostname %>";
            PageMethods.VerifyLoginUser(userName, psw, VldNum, usertype, appname, sip, sname, function (result) {
                if (result.indexOf("登录") < 0) {
                    if (rememberuser) {
                        if ($.cookie("username") == undefined) {
                            $.cookie("username", userName + seperator, { expires: 365 });
                            $.cookie("psw", value + seperator, { expires: 365 });
                        } else if ($.cookie("username").indexOf(userName + seperator) < 0) {
                            $.cookie("username", $.cookie("username") + userName + seperator, { expires: 365 });
                            $.cookie("psw", $.cookie("psw") + value + seperator, { expires: 365 });
                        }
                        else if ($.cookie("username").indexOf(userName + seperator) > -1) {
                            var nameList = $.cookie("username").split(seperator);
                            var pwdList = $.cookie("psw").split(seperator);
                            var nameStr = "";
                            var pwdStr = "";
                            for (var i = 0; i < nameList.length - 1; i++) {
                                if (nameList[i] == userName) {
                                    var tempname = nameList[i];
                                    nameList[i] = nameList[nameList.length - 2];
                                    nameList[nameList.length - 2] = tempname;

                                    var temppwd = value;
                                    pwdList[i] = pwdList[nameList.length - 2];
                                    pwdList[nameList.length - 2] = temppwd;
                                }
                                nameStr += nameList[i] + seperator;
                                pwdStr += pwdList[i] + seperator;
                            }
                            $.cookie("username", nameStr, { expires: 365 });
                            $.cookie("psw", pwdStr, { expires: 365 });
                        }
                        
                    }
                    window.location.href = result;
                } else {

                    document.getElementById('LblFaultShow').innerHTML = result;
                }
            }, function (err) {
                document.getElementById('LblFaultShow').innerHTML = err;

            });
        }
    </script>
</head>

<body style=" overflow:hidden;" >
    <form id="Form1" runat="server">
        <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td align="center" valign="top">
                    <table width="100%" height="99" border="0" align="center" cellpadding="0" cellspacing="0" background="../../images/<%=style %>/top_back.jpg">
                        <tr>
                            <td align="center" valign="top">&nbsp;</td>
                        </tr>
                    </table>
                    <table width="100%" height="661"  border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td height="462" align="center" valign="top" background="../../images/<%=style %>/middle_back.jpg">
                                <table width="90%" height="66" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td width="10%">
                                            <img src="../../images/<%=style %>/none.gif" width="150" height="10"></td>
                                        <td width="80%" align="left" valign="middle">
                                            <span class="tdtitle18"><%=LogoTitle%></span></td>
                                        <td width="10%" valign="middle" >
                                            <img src="../../images/<%=style %>/none.gif" width="88" height="10">
                                            </td>
                                    </tr>
                                </table>
                                <table width="600" height="375" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td width="35" align="right" valign="top">
                                            <img src="../../images/<%=style %>/login_back_left.png" width="35" height="375"></td>
                                        <td width="524" align="left" valign="top" background="../../images/<%=style %>/login_back.png">
                                            <table width="100%" height="38" border="0" align="center" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    
                                                    <td>&nbsp;
                                                        <div class="version_number">
                                                            
                                                             版本号<%=ReadConfig.TheReadConfig["VersionNumber"]%>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table width="100%" height="36" border="0" align="center" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    
                                                    <td align="left" valign="middle" class="tdtitle13"><%=title %></td>
                                              
                                                </tr>
                                            </table>
                                            <table width="100%" height="2" border="0" align="center" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td  class="loginline"></td>
                                                </tr>
                                            </table>
                                            <table width="100%" height="18" border="0" align="center" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>&nbsp;</td>
                                                </tr>
                                            </table>
                                            <table width="278" height="43" border="0" align="center" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="56" align="center">
                                                        <img src="../../images/<%=style %>/1_注销用户.png" width="45" height="43"></td>
                                                    <td width="30"></td>
                                                    <td width="192" align="right" style="visibility: hidden;">
                                                        <table width="192" height="30" border="0" cellpadding="0" cellspacing="0">
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table width="278" height="15" border="0" align="center" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="265"></td>
                                                </tr>
                                            </table>
                                            <table width="278" height="37" border="0" align="center" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="242" align="left" valign="middle" background="../../images/<%=style %>/1_注销用户_back2.png">
                                                        <table width="278" height="25" border="0" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td width="87" align="center" valign="middle" class="tdtitle2"><span class="tdtitle2 STYLE3">用户名</span></td>
                                                                <td width="10">&nbsp;</td>
                                                                <td>
                                                                    <div id="UserNameInput"></div></td>
                                                                <td width="12">&nbsp;</td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table width="278" height="22" border="0" align="center" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>&nbsp;</td>
                                                </tr>
                                            </table>
                                            <table width="278" height="37" border="0" align="center" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="242" align="left" valign="middle" background="../../images/<%=style %>/1_注销用户_back2.png">
                                                        <table width="278" height="25" border="0" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td width="87" align="center" valign="middle" class="tdtitle2"><span class="tdtitle2 STYLE3">密 码</span></td>
                                                                <td width="10">&nbsp;</td>
                                                                <td>
                                                                    <input type="password" id="TxtPsw" size="26" class="TextBorder"></td>
                                                                <td width="12">&nbsp;</td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table width="100%" height="38" border="0" align="center" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="center" valign="middle">
                                                        <label id="LblFaultShow"></label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table width="278" height="29" border="0" align="center" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="194" align="center">&nbsp;</td>
                                                    <td width="84" align="center" valign="middle">
                                                        <table width="84" height="29" border="0" cellpadding="0" cellspacing="0">
                                                          
                                                            <tr onmouseout="changeout(this)" onmouseover="changeon(this)" style="cursor: pointer">
                                                                <td width="10" background="../../images/<%=style %>/button_l.png"></td>
                                                                <td width="64" align="center" background="../../images/<%=style %>/button_m.png" id="btnload" onclick="VerifyTxt()">登录</td>
                                                                <td width="10" background="../../images/<%=style %>/button_r.png"></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td width="35" align="left" valign="top">
                                            <img src="../../images/<%=style %>/login_back_right.png" width="35" height="375"></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table width="100%" height="68" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="49%" height="15"></td>
                            <td width="50%" height="40" align="right" valign="bottom">
                                <img src="../../images/<%=style %>/utlogo.png" width="124" height="32"></td>
                            <td width="1%" height="10">
                                <img src="../../images/<%=style %>/none.gif" width="10" height="10"></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td height="28" align="right" valign="middle" class="tdtitle16">版权所有&nbsp;&copy; 珠海优特电力科技股份有限公司   粤ICP备00000000号</td>
                            <td></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>

        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
    </form>
    <script type="text/javascript">
        $("body").css({ "background": "url('../../images/<%=style %>/back.jpg')" });
    
        if (window.parent.frames.length != 0) {
            window.parent.location.href = "../../Default.aspx";
        }

        $(document).ready(function () {
            if (navigator.appName != "Microsoft Internet Explorer") {
                $("#marktable").css("margin-top", "105px");
            }
        });




        initbody();
        window.onresize = function () {
            initbody()
        }

        function initbody() {
            //   document.getElementById("tdback").style.height = document.documentElement.offsetHeight ;
            //    document.getElementById("tablebody").style.height = document.documentElement.offsetHeight - 200;
        }

        function changeon(id) {
            //  $(id).css({ "background-image": "url(../../images/button__on_m.png)" });
            var td = $(id).find("td");


            td[0].background = changeimg(td[0].background, "l", "_on");
            td[1].background = changeimg(td[1].background, "m", "_on");
            td[2].background = changeimg(td[2].background, "r", "_on");

            //     $(td[0]).css({ "background-image": "url(../../images/button_on_l.png)" });
            //     $(td[1]).css({ "background-image": "url(../../images/button__on_m.png)" });
            //     $(td[2]).css({ "background-image": "url(../../images/button__on_r.png)" });

        }

        function changeout(id) {
            //  $(id).css({ "background-image": "url(../../images/button__on_m.png)" });
            var td = $(id).find("td");

            td[0].background = changeimg(td[0].background, "l", "");
            td[1].background = changeimg(td[1].background, "m", "");
            td[2].background = changeimg(td[2].background, "r", "");
            //   $(td[0]).css({ "background-image": "url(../../images/button_l.png)" });
            //   $(td[1]).css({ "background-image": "url(../../images/button_m.png)" });
            //   $(td[2]).css({ "background-image": "url(../../images/button_r.png)" });
        }
    </script>
</body>
</html>
