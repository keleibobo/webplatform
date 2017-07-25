function checksvg() {
    if (navigator.appName == "Microsoft Internet Explorer") {
        if (window.ActiveXObject) {
            try {
                var asv = new ActiveXObject("Adobe.SVGCtl");
            } catch (err) {
                alert(" 未安装 Adobe SVG Viewer 插件请安装并设置插件关联\n或者IE安全模式设置过高，请把安全模式设置中的安全级别修改为中后在尝试！ ");
                window.location.href = "../public/download/SVGView_3815.exe";
            }
        }
    }
    if (navigator.appVersion.match(/7./i) == '7.') {
    }

}
var fillColor = [];
var twinkleTimer = [];
var typeArray = [];
var valueArray = [];


function GetSvgDom(guid) {

    var theSquare = document.getElementById(guid);

    if (theSquare == undefined) {
        var svgEle = document.getElementById("svgEle");
        if (svgEle != null) {
            try {
                var svgDoc = svgEle.getSVGDocument();
                if (svgDoc != null) {
                    theSquare = svgDoc.getElementById(guid);
                }
            } catch (e) { }
        }
    }
    return theSquare;
}

function updatehref(guid, value, item) {
    var theSquare = document.getElementById(guid);

    if (theSquare == undefined) {
        var svgEle = document.getElementById("svgEle");
        if (svgEle != null) {
            try{
                var svgDoc = svgEle.getSVGDocument();
                if (svgDoc != null) {
                    theSquare = svgDoc.getElementById(guid);
                }
            } catch (e) { }
        }
    }
    if (theSquare) {
        for (var a = 0; a < theSquare.childNodes.length; a++) {
            var user = theSquare.childNodes.item(a);
            if (user.nodeName == "use") {
                var href = user.getAttributeNS("http://www.w3.org/1999/xlink", "href");
                var type = href.split("@")[0].replace("#", "");
                var find = false;
                for (j = 0; j < typeArray.length; j++) {
                    if (typeArray[j].split("-")[0] == guid) {
                        find = true;
                    }
                }
                if (!find) {
                    typeArray.push(guid + "-" + type);
                }
                hrefTwinkle(user, type, item);
                if (type == "") {
                    for (k = 0; k < typeArray.length; k++) {
                        if (typeArray[k].split("-")[0] == guid) {
                            type = typeArray[k].split("-")[1];
                        }
                    }
                }
                var hrefs = "#" + type + "@" + value.replace(".000", "");
                try {
                    user.setAttributeNS("http://www.w3.org/1999/xlink", "href", hrefs);
                    valueArray[guid] = value;
                }
                catch (ex) {
                    //alert(ex);
                }
            }
        }
    }

}



function updatetspans(guid, value, item) {
    var prefix = "";
    var postfix = "";
    var theSquare = document.getElementById(guid);
    if (theSquare == undefined) {
        var svgEle = document.getElementById("svgEle");
        if (svgEle != null) {
            try{
                var svgDoc = svgEle.getSVGDocument();
                if (svgDoc != null) {
                    theSquare = svgDoc.getElementById(guid);
                }
            } catch (e) { }
        }
    }

    if (theSquare) {
        for (var x = 0; x < theSquare.childNodes.length; x++) {
            var fixItem = theSquare.childNodes.item(x);
            if (fixItem.nodeName.toLocaleLowerCase() == "analogdata") {
                for (var k = 0; k < fixItem.attributes.length; k++) {
                    if (fixItem.attributes[k].localName.toLocaleLowerCase() == "postfix") {
                        postfix = fixItem.attributes[k].nodeValue;
                    }
                    if (fixItem.attributes[k].localName.toLocaleLowerCase() == "prefix") {
                        prefix = fixItem.attributes[k].nodeValue;
                    }
                }
            }
        }

        for (var a = 0; a < theSquare.childNodes.length; a++) {
            var txt = theSquare.childNodes.item(a);
            if (txt.nodeName == "text") {
                for (var b = 0; b < txt.childNodes.length; b++) {
                    var tspan = txt.childNodes.item(b);
                    if (tspan.nodeName == "tspan") {
                        try {
                            var myString = value;
                            var ip = myString.indexOf(".");
                            var ps = myString.substr(ip, myString.length - ip);
                            if (ps == ".000" || ps == ".00" || ps == ".0" || ps == ".0000") {
                                value = myString.substr(0, ip);
                            }
                            value = prefix + value + postfix;
                            tspan.firstChild.nodeValue = value;
                            valueArray[guid] = value;
                            ChuangeColor(txt, item);
                            spanTwinkle(tspan, item);
                        }
                        catch (ex) {
                            //alert(ex);
                        }
                    }
                }
            }
        }
    }
}

function hrefTwinkle(svgdom, type, item) {
    var param = item.extendparams.split(';');
    var find = false;
    if (item.extendparams.indexOf("twinkle") > 0) {
        for (i = 0; i < param.length; i++) {
            if (param[i].indexOf("twinkle") >= 0) {
                if (param[i].split("=")[1] == "1") {
                    if (twinkleTimer[item.shape_guid]!=null) {
                            find = true;
                        }
                    if (!find) {
                        var number = setInterval(function () {
                            if (svgdom.getAttributeNS("http://www.w3.org/1999/xlink", "href") == "") {
                                var hrefs = "#" + type + "@" + valueArray[item.shape_guid].replace(".000", "");
                                svgdom.setAttributeNS("http://www.w3.org/1999/xlink", "href", hrefs);
                            }
                            else {
                                svgdom.setAttributeNS("http://www.w3.org/1999/xlink", "href", "");
                            }
                        }, 500);
                        twinkleTimer[item.shape_guid] =number;
                    }
                }
            }
        }
    }
    else {
            if (twinkleTimer[item.shape_guid]!=null) {
                clearInterval(twinkleTimer[item.shape_guid]);
                twinkleTimer[item.shape_guid] = null;
                setTimeout(function () {
                    for (k = 0; k < typeArray.length; k++) {
                        if (typeArray[k].split("-")[0] == item.shape_guid.toString()) {
                            var hrefs = "#" + typeArray[k].split("-")[1] + "@" + item.value.replace(".000", "");
                            svgdom.setAttributeNS("http://www.w3.org/1999/xlink", "href", hrefs);
                        }
                    }
                }, 800);
            }
    }
}

function ChuangeColor(svgdom, item) {
    var param = item.extendparams.split(';');
    if (item.extendparams.indexOf("color") > 0) {
        for (i = 0; i < param.length; i++) {
            if (param[i].indexOf("color") >= 0) {
                if (param[i].split("=")[1] != "") {
                    fillColor[item.shape_guid] = $(svgdom).attr("fill");
                    $(svgdom).attr("fill", param[i].split("=")[1]);
                }
                
            }
        }
    }
    else {
        if (fillColor[item.shape_guid] != null) {
            $(svgdom).attr("fill", fillColor[item.shape_guid]);
        }
    }
}

function spanTwinkle(svgdom, item) {
    var param = item.extendparams.split(';');
    var find = false;
    if (item.extendparams.indexOf("twinkle") > 0) {
        for (i = 0; i < param.length; i++) {
            if (param[i].indexOf("twinkle") >= 0) {
                if (param[i].split("=")[1] == "1") {
                    if (twinkleTimer[item.shape_guid] != null) {
                        find = true;
                    }
                    if (!find) {
                        try {
                            var number = setInterval(function () {
                                var value = valueArray[item.shape_guid];
                                var myString = valueArray[item.shape_guid];
                                var ip = myString.indexOf(".");
                                var ps = myString.substr(ip, myString.length - ip);
                                if (ps == ".000" || ps == ".00" || ps == ".0" || ps == ".0000") {
                                    value = myString.substr(0, ip);
                                }
                                if (svgdom.firstChild.nodeValue == "") {
                                    svgdom.firstChild.nodeValue = value;
                                }
                                else {
                                    svgdom.firstChild.nodeValue = "";
                                }
                            }, 1500);
                            twinkleTimer[item.shape_guid] = number;
                        }
                        catch (ex) {

                        }
                    }
                }
            }
        }
    }
    else {
        if (twinkleTimer[item.shape_guid] != null) {
                clearInterval(twinkleTimer[item.shape_guid]);
                twinkleTimer[item.shape_guid] = null;
        }
    }
}

//保留
function buildButton(datajson) {
    if (datajson == null) {
        return;
    }
    var source = []
    data = eval("(" + datajson + ")")
    $.each(data, function (idx, itemdata) {
        if (itemdata != undefined && itemdata["parent_id"] == 0) {
            source[source.length] = itemdata;

        }
    });
    var bsvg = "";
    if (source == null) {
        return;
    }
    $.each(source, function (idx, item) {
        var svg = appname + "_" + item.id + "_" + item.filesize + ".svg";
        bsvg = bsvg + "<input type='button' onclick=\"showSVG(this,'" + item.id + "','" + svg + "','" + item.width + "','" + item.height + "')\" value=" + item.name + " >&nbsp;";
    });

    $("#buttonSVG").html(bsvg);
}


var dTime="";
function GetSvgData() {
    var nodeid = $("#nodeid").val();
    if (typeof (nodeid) == "undefined") {
           return;
       }
    var url = "../basepage/HandlerSVG.ashx?method=GetDatasJSON&nodeid=" + nodeid + "&lasttime=" + dTime+"&random="+Math.random()*10;
    $.ajax({
        url: url,
        type: 'get',
        success: function (data) {
            var ds = "";
            if (data == "") {
                if ($("#svg").css("display") == "none")
                    $("#svg").css("display", "inline");
                $.messager.progress('close');
            }
            if (data != "[]") {
                var datajson = jQuery.parseJSON(data);
                if (datajson != null) {
                    var i = 0;
                    $.each(datajson, function (idx, item) {
                        if (idx == 0) {
                            var svgelement = GetSvgDom(item.shape_guid);
                            if (svgelement == null) {
                                setTimeout(function () {
                                    GetSvgData();
                                }, 500);
                                return;
                            }
                            dTime = item.lastrefreshtime;
                        }
                        var guid = item.shape_guid;
                        var type = item.mappingtabletype_id;
                        var value = item.value;
                        i++;
                        if (type != "") {
                            if (type == "3") {
                                updatetspans(guid, value, item);
                                if (i == datajson.length) {
                                    if ($("#svg").css("display") == "none")
                                        $("#svg").css("display", "inline");
                                    $.messager.progress('close');
                                }
                            } else if (type == "1" || type == "2") {

                                updatehref(guid, value, item);
                                if (i == datajson.length) {
                                    if ($("#svg").css("display") == "none")
                                        $("#svg").css("display", "inline");
                                    $.messager.progress('close');
                                }
                            }
                        }
                    });
                }
            }
            else {
                if ($("#svg").css("display") == "none")
                    $("#svg").css("display", "inline");
                $.messager.progress('close');
            }
        }

        ,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            if (textStatus == "timeout")
                setTimeout(function () { BuildNavigate(); }, 200); //timeout后重新连接
            else {
                //   alert("error:" + textStatus);
            }
            //   alert(XMLHttpRequest.responseText);
        }

    });

}


function resizesvg() {
    var pn = document.getElementById("svg").parentNode.parentNode;
    var ph = document.documentElement.clientHeight;
    pn.style.height = ph - 80;
}







var NavWidth = 350;
var NavHeight = 200;

function setNavPos() {
    if (getScrollDiv().scrollHeight > getScrollDiv().clientHeight) {
        $("#svgbutton").css("right", "18px");
        $("#SvgNavigation").css("right", "18px");
    }
    else {
        $("#svgbutton").css("right", miniSize);
        $("#SvgNavigation").css("right", miniSize);
    }

    if (getScrollDiv().scrollWidth > getScrollDiv().clientWidth) {
        $("#svgbutton").css("bottom", "18px");
        $("#SvgNavigation").css("bottom", "18px");
    }
    else {
        $("#svgbutton").css("bottom", "0px");
        $("#SvgNavigation").css("bottom", "0px");
    }
}

function getScrollDiv() {
    return $("#svg").parent().parent()[0];
}

function redrawArea() {
    var t = getScrollDiv();
    if (t.scrollWidth + 5 > t.offsetWidth || t.scrollHeight + 5 > t.offsetHeight) {
        var areaWidth = t.offsetWidth / t.scrollWidth;
        var areaHeight = 1 - (t.scrollHeight - t.offsetHeight) / t.scrollHeight;
        var areaLeft = t.scrollLeft / t.scrollWidth;
        var areaTop = t.scrollTop / t.scrollHeight;
        areaTop = ((NavHeight) - (NavHeight) * areaTop) == (NavHeight) ? (NavHeight + 3) : (NavHeight) - (NavHeight) * areaTop - 2;
        areaWidth = areaWidth * (NavWidth - 4) > (NavWidth - 4) ? (NavWidth - 4) : areaWidth * (NavWidth - 4) - 2;
        areaHeight = (areaHeight * NavHeight) >= NavHeight ? NavHeight - 4 : areaHeight * NavHeight - 10;
        areaLeft = areaLeft * (NavWidth - 4) < 0 ? 0 : areaLeft * (NavWidth - 4);
        $("#NavigationArea").animate({ bottom: areaTop, width: areaWidth, height: areaHeight, left: areaLeft }, 150);
    }
    scrollMark = true;
}

function SvgHubInit() {
    var SvgHub = $.connection.svgHub;
    $.connection.hub.logging = true;

    SvgHub.client.UpdateStatus = function (svgid, shapeguid, svgshapename, svalue, mappingtabletype) {
        if (mappingtabletype == "2") {
            if ($("#nodeid").val() == svgid) {
                updatehref(shapeguid, svgshapename, svalue);
            }
        }
        else if (mappingtabletype == "3") {
            updatetspans(shapeguid, svalue);
        }
    }

    $.connection.hub.stateChanged(function (change) {
        var oldState = null,
            newState = null;

        for (var state in $.signalR.connectionState) {
            if ($.signalR.connectionState[state] === change.oldState) {
                oldState = state;
            } else if ($.signalR.connectionState[state] === change.newState) {
                newState = state;
            }
        }
    //     alert("[" + new Date().toTimeString() + "]: " + oldState + " => " + newState + " " + $.connection.hub.id );
    });
    $.connection.hub.start();
}



function fnCallByDlg(id, op) {
    ///svgsvr.aspx
    var url = "../basepage/HandlerSVG.ashx?type=control&appname=" + appname + "&controldata=nodeid=" + $("#nodeid").val() + ";svgshapeguid=" + id + ";svalue=" + op + "&random=" + Math.random() * 10;
    $.ajax({
        url: url,
        type: 'get',
        success: function (data) {
        }
    });
    fnCloseWindows();
    var p = "控制命令已经下发。 ";
    alert(p);

}

function verifyVal() {
   
     var regex = /^[+|-]?\d*\.?\d*$/; 
     if (!regex.test($("#pointValue").val()))
         {    
           alert("输入值格式错误！");
         $("#pointValue").val("");
             return ;     
         }   
}

function SubmitSetValue(id) {
    try{
        var temp = parseFloat($("#pointValue").val());
    }
    catch (e) {
        alert("请输入正确的值！");
        return;
    }
    if ($("#pointValue").val() == "") {
        alert("请先输入预置值！");
        return;     
    }
    var op = $("#pointValue").val();
    var url = "../basepage/HandlerSVG.ashx?type=control&appname=" + appname + "&controldata=nodeid=" + $("#nodeid").val() + ";svgshapeguid=" + id + ";svalue=" + op + ";&random=" + Math.random() * 10;
    $.ajax({
        url: url,
        type: 'get',
        success: function (data) {
        }
    });
    fnCloseWindows();
    var p = "预置值命令已经下发。 ";
    alert(p);
}


///初始化弹出框
function fnShowWindows(x, y) {
    if (document.getElementById("divWin")) {
        fnId("divWin").style.zIndex = 999;
        fnId("divWin").style.display = "";
        fnId("divWin").style.left = x+110;
        fnId("divWin").style.top = y+76;
    }
    else {
        var objWin = document.createElement("div");
        objWin.id = "divWin";
        objWin.style.left = x+110;
        objWin.style.top = y+76;
        objWin.style.position = "absolute";
        objWin.style.border = "2px solid #AEBBCA";
        objWin.style.background = "#FFF";
        objWin.style.zIndex = 999;
        document.body.appendChild(objWin);
    }

    if (document.getElementById("win_bg")) {
        fnId("win_bg").style.zIndex = 998;
        fnId("win_bg").style.display = "";
        //      var bg = $(window.frames["listframe"].document.body);
        //       var w = window.frames["listframe"].document.body.scrollWidth
        //     $("#win_bg").width(w);
        //     $("#win_bg").height(bg.height());
    }
    else {
        var obj_bg = document.createElement("div");
        obj_bg.id = "win_bg";
        obj_bg.className = "win_bg";
        document.body.appendChild(obj_bg);
        //   var bg = $(window.frames["listframe"].document.body);
        //   var w = window.frames["listframe"].document.body.scrollWidth
        //   $("#win_bg").width(w);
        //   $("#win_bg").height(bg.height());
    }
}

///弹出框
function fnShowDialog(id, op, st, x, y) {
    fnShowWindows(x, y);

    fnShowWindows(x, y);

    var theSquare = document.getElementById(id);

    if (theSquare == undefined) {
        var svgEle = document.getElementById("svgEle");
        if (svgEle != null) {
            try {
                var svgDoc = svgEle.getSVGDocument();
                if (svgDoc != null) {
                    theSquare = svgDoc.getElementById(id);
                }
            } catch (e) { }
        }
    }
    var status = "";
    if (theSquare) {
        for (var a = 0; a < theSquare.childNodes.length; a++) {
            var user = theSquare.childNodes.item(a);
            if (user.nodeName == "use") {
                // hrefTwinkle(user, type, item);
                // var hrefs = "#" + type + "@" + value.replace(".000", "");
                try {
                    status = user.getAttributeNS("http://www.w3.org/1999/xlink", "href");
                }
                catch (ex) {

                }
            }
        }
    }

    var str = "";
    str += '<table width="160" border="0" cellpadding="0" cellspacing="0"  bgcolor="#FFFFFF">';
    str += ' <tr> <td align="left" valign="top"><table width="100%"  border="1" cellspacing="0" cellpadding="1" bordercolorlight="#97B9D7" bordercolordark="#ffffff" bgcolor="#FFFFFF" class="TextBorder01">'
    str += '<tr align="center"> <td height="32" align="left" class="t2">'
    str += '<table width="100%" height="26" border="0" cellpadding="0" cellspacing="0"> <tr> <td width="80%" class="tdtitle15">&nbsp;&nbsp;选项</td><td width="5%">&nbsp;</td><td width="15%" onclick="fnCloseWindows()" align="center"><div class="closeimg"></div></td></tr> </table>'
    str += ' </td></tr><tr> <td align="center" valign="top">';

    var ps = op.split("￥");
    var it = ps.length;
    while (it-- > 0) {
        var p = ps[it];
        var q = p.split("=");
        var p1 = q[1];
        var p2 = q[0];

        if (p != '') {
            if (status.indexOf("@1") > -1 && p2 == "1") {
                str += ' <table width="100%" height="30" border="0" cellpadding="0" cellspacing="0">';
                str += '<tr> <td width="10%"></td><td width="85">';
                str += '<table width="68%" height="22" border="0" cellpadding="0" cellspacing="0"> <tr><td width="10%" align="right"><img src="../images/选项按钮1.gif" width="8" height="22"></td><td width="80%" align="center" background="../images/选项按钮2.gif" style="color:gray;">' + p1 + '</td><td width="10%"><img src="../images/选项按钮3.gif" width="8" height="22"></td></tr></table>';
                str += ' </td> <td width="5%"></td></tr></table>';
            }
            else if (status.indexOf("@0") > -1 && p2 == "0") {
                str += ' <table width="100%" height="30" border="0" cellpadding="0" cellspacing="0">';
                str += '<tr> <td width="10%"></td><td width="85">';
                str += '<table width="68%" height="22" border="0" cellpadding="0" cellspacing="0"> <tr><td width="10%" align="right"><img src="../images/选项按钮1.gif" width="8" height="22"></td><td width="80%" align="center" background="../images/选项按钮2.gif" style="color:gray;">' + p1 + '</td><td width="10%"><img src="../images/选项按钮3.gif" width="8" height="22"></td></tr></table>';
                str += ' </td> <td width="5%"></td></tr></table>';
            }
            else {
                str += ' <table width="100%" height="30" border="0" cellpadding="0" cellspacing="0">';
                str += '<tr> <td width="10%"></td><td width="85">';
                str += '<table width="68%" height="22" border="0" cellpadding="0" cellspacing="0"> <tr><td width="10%" align="right"><img src="../images/选项按钮1.gif" width="8" height="22"></td><td width="80%" align="center" background="../images/选项按钮2.gif" onclick="fnCallByDlg(' + id + ', \'' + p2 + '\')">' + p1 + '</td><td width="10%"><img src="../images/选项按钮3.gif" width="8" height="22"></td></tr></table>';
                str += ' </td> <td width="5%"></td></tr></table>';
            }
        }
    }
    fnId("divWin").innerHTML = str;
    $("#divWin").fadeIn(300, function () {
        fnId("divWin").style.display = "";
    });

    // HTML3
    // fnId("divWin").innerHTML = str;
    //alert('窗口创建完成');
}






var username = "";
var pwd = "";
var dev_list = "";
var combine_list = "";
var selnum = 0;
var dataList;
var Right = new Array();


function OCXStreamInit() {
    var imgList = $("#controlpanel img");
    if (parent.document.getElementById('style') == null) {
        return;
    }
    var style = parent.document.getElementById('style').value;
    for (i = 0; i < imgList.length; i++) {
        $(imgList[i]).attr("src", "../images/" + style + "/control/" + imgList[i].nameProp);
    }
    if (style == "blue") {
        $("#win").css("background-color", "#87BDEB");
    }
    
  //  $("#control").attr("src","../images/"+$("#style")+"/control/kongzhiback.gif");
    var url = "../basepage/GetDeviceList.ashx?random=" + Math.random() * 10;
    $.ajax({
        url: url,
        type: 'get',
        timeout: 60000,
        success: function (data) {
            dataList = jQuery.parseJSON(data);
            dev_list = '<?xml Version=\"1.0\" Encoding=\"UTF-8\"?><DeviceList>';
            if (dataList.ltstm != null) {
                $.ajax({
                    url: "../basepage/GetStreamUserData.ashx?random=" + Math.random() * 10,
                    type: 'get',
                    timeout: 60000,
                    success: function (rstr) {
                        for (i = 0; i < dataList.ltstm.length; i++) {
                            if (i == 0) {
                                username = dataList.ltstm[i].user;
                                pwd = dataList.ltstm[i].password;
                            }
                            dev_list += '<Server id=\"' + dataList.ltstm[i].id + '\" ip=\"' + dataList.ltstm[i].addr + '\" port=\"' + dataList.ltstm[i].port + '\" name=\"' + dataList.ltstm[i].name + '\"  attribute=\"' + dataList.ltstm[i].attribute + '\"  upid=\"0\" addrcode=\"temp\" regionid=\"0\">';
                            for (k = 0; k < dataList.ltstm[i].ltdev.length; k++) {
                                dev_list += '<Dvr id=\"' + (k + 1) + '\" ip=\"' + dataList.ltstm[i].ltdev[k].addr + '\" port=\"' + dataList.ltstm[i].ltdev[k].port + '\" name=\"DVR' + (k + 1) + '\" type=\"' + dataList.ltstm[i].ltdev[k].Type + '\" serverid=\"' + dataList.ltstm[i].id + '\"  attribute=\"' + dataList.ltstm[i].ltdev[k].attribute + '\"  regionid=\"0\">';
                                for (j = 0; j < dataList.ltstm[i].ltdev[k].ltch.length; j++) {
                                    Right[dataList.ltstm[i].ltdev[k].ltch[j].id] = dataList.ltstm[i].ltdev[k].ltch[j].PTZControl;
                                    dev_list += '<Channel id=\"' + dataList.ltstm[i].ltdev[k].ltch[j].id + '\" name=\"' + dataList.ltstm[i].ltdev[k].ltch[j].name + '\" no=\"' + dataList.ltstm[i].ltdev[k].ltch[j].no + '\" type=\"1\" defPreset=\"1\" interval=\"5\"  attribute=\"' + dataList.ltstm[i].ltdev[k].ltch[j].attribute + '\" dvrid=\"' + (k + 1) + '\" regionid=\"1\">';
                                    for (l = 0; l < dataList.ltstm[i].ltdev[k].ltch[j].ltpre.length; l++) {
                                        dev_list += '<Preset camid=\"' + (j + 1) + '\" no=\"' + dataList.ltstm[i].ltdev[k].ltch[j].ltpre[l].id + '\" name=\"' + dataList.ltstm[i].ltdev[k].ltch[j].ltpre[l].desc + '\"/>';
                                    }
                                    dev_list += '</Channel>';
                                }
                                dev_list += '</Dvr>';
                            }
                            dev_list += '</Server>';
                        }
                        dev_list += '</DeviceList>';
                        setRight(rstr);
                    }
                });
            }

            if (dataList.ltcomb != null) {
                combine_list += '<?xml Version=\"1.0\" Encoding=\"UTF-8\"?><CombinList>';
                for (i = 0; i < dataList.ltcomb.length; i++) {
                    combine_list += '<COMBINE Id=\"' + dataList.ltcomb[i].id + '\" Name=\"' + dataList.ltcomb[i].name + '\">';
                    for (k = 0; k < dataList.ltcomb[i].list.length; k++) {
                        combine_list += '<NET_COMBINE_PARAM ServerId=\"' + dataList.ltcomb[i].list[k].ServerId + '\" DvrIp=\"' + dataList.ltcomb[i].list[k].DvrIp + '\" DvrPort=\"' + dataList.ltcomb[i].list[k].DvrPort + '\" CamChanNo=\"' + dataList.ltcomb[i].list[k].CamChanNo + '\" PresetNo=\"' + dataList.ltcomb[i].list[k].PresetNo + '\"/>';
                    }
                    combine_list += '</COMBINE>';
                }
                combine_list += '</CombinList>';
            }
        },
        error: function (data) {
            alert(data);
        }
    });
}

function setRight(rstr) {
    if (rstr == "[]")
        return;
    var auList = jQuery.parseJSON(rstr);
    if (auList != null) {
        for (i = 0; i < auList[0].userparamlist.length; i++) {
            if (i == 0) {
                var up = jQuery.parseJSON(auList[0].userparamlist[0]);
                username = up.streamusername;
                pwd = up.streampassword;
            }
            var temp = jQuery.parseJSON(auList[0].userparamlist[i]);

            if (temp.ControlID != null) {
                for (j = 0; j < temp.ControlID.length; j++) {
                    Right[parseInt(temp.ControlID[j])] = "1";
                }
            }
        }
    }
}

var isInit = false;
function init(channelName, stationName) {
    stationName = stationName.replace(/[ ]/g, "");
    if (!isInit) {
        UTStream.UT_STREAM_Init(0);
        stm_dev_list();
        stm_setuser(username, pwd);
        setWindowNum(1);
        UTStream.UT_STREAM_SetSelWndNo(1);
        isInit = true;
    }
    SetRealplay(channelName, stationName);
    UTStream.UT_STREAM_Timeproc();
}
function stopRealplay() {
    UTStream.UT_STREAM_StopRealPlay(1);
}
//传入设备列表和组合列表
function stm_dev_list() {
    UTStream.UT_STREAM_SetDeviceList(dev_list);
    var mark = UTStream.UT_STREAM_SetCombineList(combine_list);
}

//设置流媒体用户
function stm_setuser(user, pwd) {
    UTStream.UT_STREAM_SetUser(user, pwd);
}

function SetRealplay(channelName, stationName) {
    if (dataList != null) {
        if (dataList.ltstm != null) {
            for (i = 0; i < dataList.ltstm.length; i++) {
                for (k = 0; k < dataList.ltstm[i].ltdev.length; k++) {
                    for (j = 0; j < dataList.ltstm[i].ltdev[k].ltch.length; j++) {
                        if (dataList.ltstm[i].ltdev[k].ltch[j].name == channelName && dataList.ltstm[i].ltdev[k].ltch[j].stInfo.name == stationName) {
                            viewNum = UTStream.UT_STREAM_GetSelWndNo();
                            if (Right[dataList.ltstm[i].ltdev[k].ltch[j].id] == "0") {
                                var imgList = $("#controlpanel img");
                                var style = parent.document.getElementById('style').value;
                                for (b = 0; b < imgList.length ; b++) {
                                    $(imgList[b]).attr("src", "../images/gray/" + style+"/" + imgList[b].nameProp);
                                }
                                $("#presetList").attr("disabled", "disabled"); 
                            }
                            else {
                                var imgList = $("#controlpanel img");
                                var style = parent.document.getElementById('style').value;
                                for (a = 0; a < imgList.length; a++) {
                                    $(imgList[a]).attr("src", "../images/" + style + "/control/" + imgList[a].nameProp);
                                }
                                $("#presetList").removeAttr("disabled"); 
                            }
                            stm_realplay(dataList.ltstm[i].id, dataList.ltstm[i].ltdev[k].ltch[j].id, dataList.ltstm[i].ltdev[k].addr, dataList.ltstm[i].ltdev[k].port, viewNum);
                            $("#presetList").html('<option value=\"\">请选择预置点</option>');
                            for (l = 0; l < dataList.ltstm[i].ltdev[k].ltch[j].ltpre.length; l++) {
                                $("#presetList").append('<option value=\"' + dataList.ltstm[i].ltdev[k].ltch[j].ltpre[l].id + '\">' + dataList.ltstm[i].ltdev[k].ltch[j].ltpre[l].desc + '</option>');
                            }
                            return;
                        }
                    }
                }
            }
        }
    }
}

// 实时预览
function stm_realplay(serverId, channel, ip, port, viewNum) {
    var test_dev = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><NET_PREVIEW_PARAM  CameraId=\"" + channel + "\" StreamType=\"0\" StreamContent=\"0\" Protocol=\"0\" Reconnect=\"1\"/>";
    UTStream.UT_STREAM_RealPlay(test_dev, viewNum, 0);
    UTStream.UT_STREAM_Timeproc();
}

function setWindowNum(num) {
    UTStream.UT_STREAM_SetWndNum(num);
    UTStream.UT_STREAM_Timeproc();
}

//云台方向控制 
function PTZControl(CommandStr, so) {
    if ($("#" + CommandStr).length != 0) {
        if ($("#" + CommandStr).attr("src").indexOf("/gray/") > 0) {
            return;
        }
    }
    if (CommandStr == "left" || CommandStr == "right" || CommandStr == "up" || CommandStr == "down") {
        if ($("#controlmap").attr("src").indexOf("/gray/") > 0) {
            return;
        }
    }
    var Command = strToMark(CommandStr);
    if (Command != 0) {
        UTStream.UT_STREAM_PTZCtrlByWnd(Command, so, 1, UTStream.UT_STREAM_GetSelWndNo());
    }
}
//云台缩放控制
function PTZoom(CommandStr, so) {
    var Command = strToMark(CommandStr);
    if (Command != 0) {
        UTStream.UT_STREAM_PTZCtrlByWnd(Command, so, 1, UTStream.UT_STREAM_GetSelWndNo());
    }
}

//标记转换
function strToMark(str) {
    if (str == "up") {
        return 1;
    }
    else if (str == "down") {
        return 2;
    }
    else if (str == "left") {
        return 3;
    }
    else if (str == "right") {
        return 4;
    }
    else if (str == "upleft") {
        return 5;
    }
    else if (str == "upright") {
        return 6;
    }
    else if (str == "downleft") {
        return 7;
    }
    else if (str == "downright") {
        return 8;
    }
    else if (str == "zoomin") {
        return 9;
    }
    else if (str == "zoomout") {
        return 10;
    }
    else if (str == "focusnear") {
        return 11;
    }
    else if (str == "focusfar") {
        return 12;
    }
    else if (str == "stepreset") {
        return 17;
    }
    else if (str == "delpreset") {
        return 18;
    }
    else if (str == "callpreset") {
        return 19;
    }
    else {
        return 0;
    }
}
var preset;

function presetSelect() {
    var presetid = $("#presetList").val();
    if (presetid != "") {
        viewNum = UTStream.UT_STREAM_GetSelWndNo();
        UTStream.UT_STREAM_PTZCtrlByWnd(strToMark("callpreset"), false, presetid, viewNum);
    }
}


function loadXML(xmlString) {

    var xmlDoc;
    if (window.ActiveXObject) {
        xmlDoc = new ActiveXObject('Microsoft.XMLDOM');
        if (!xmlDoc) xmldoc = new ActiveXObject("MSXML2.DOMDocument.3.0");
        xmlDoc.async = false;
        xmlDoc.loadXML(xmlString);
    } else if (document.implementation && document.implementation.createDocument) {
        var domParser = new DOMParser();
        xmlDoc = domParser.parseFromString(xmlString, 'text/xml');
    } else {
        return null;
    }
    return xmlDoc;
}


window.onbeforeunload = function () {
    if ($("#UTStream").length>0) {
        UTStream.UT_STREAM_UnInit();
    }
}