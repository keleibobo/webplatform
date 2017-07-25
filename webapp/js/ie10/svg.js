//var host = "localhost:8686";
var lasttime = "";
var svglt; //标记刷新数据
var refTree;
var refData;
var sign = "-UT-";
var treeinfo = [];
var showtype;
var firload = true;
var NavWidth = 350;
var NavHeight = 200;
var miniSize = 0;
var filesize;
var initWidth;
var initHeight;
var getsvgdata_timer;
var tree_timer;
var svgFileId = 0;
var svgDoc = null; 
var svgRoot = null; 
var svgWnd = null;

$(document).ready(function () {
    setTimeout(function () {
        $('#SvgNavigation').animate({ height: miniSize, width: miniSize }, 10, function () {
            $('#SvgNavigation').css("visibility", "visible");
            $('#NavigationArea').css("visibility", "hidden");
        });
        $("#svgbutton").click(function () {
            if ($("#svgpic").attr("src") == "" || $("#svgpic").attr("src").indexOf(svgFileId) < 0) {
                PageMethods.GetSvgJPGFile(svgFileId, appname, function (result) {
                    var svg = appname + "_" + svgFileId + ".jpg";
                    var url = "../svg/svgfiles/" + svg;
                    $("#svgpic").attr("src", url);
                    if ($("#svgpic")[0].offsetWidth > 20 && $("#svgpic")[0].offsetHeight > 20) {
                        NavWidth = $("#svgpic")[0].offsetWidth;
                        NavHeight = $("#svgpic")[0].offsetHeight;
                        redrawArea();
                    }
                });

            }
            if ($("#svgbutton").css("background-image").indexOf("expand.bmp") > 0) {
                $('#NavigationArea').css("visibility", "visible");
                $("#svgpic").attr("width", NavWidth);
                $("#svgpic").attr("height", NavHeight);
                $("#svgbutton").css("background", "url('../images/slide.bmp')");
                $('#SvgNavigation').animate({ height: NavHeight, width: NavWidth }, 250);
            }
            else {
                $("#svgbutton").css("background", "url('../images/expand.bmp')");
                $('#NavigationArea').css("visibility", "hidden");
                $('#SvgNavigation').animate({ height: miniSize, width: miniSize }, 250, function () {
                    $("#svgpic").attr("width", miniSize);
                    $("#svgpic").attr("height", miniSize);
                    redrawArea();
                });
            }
        });
    }, 100);
});
function setInit(iappname, irefData) { //iappname, irefTree, irefData, ishowtype, itreeInfo
    appname = iappname;

    refData = irefData;
 
}


var dTime;

function clicklink(e) {
    var paramater = e.data.params.split(',');
    sval = paramater[1];
    $("#TreeNodeText").val(sval);
    if (sval != undefined && sval != "")
        PageMethods.GetLinkFilename(sval, function (result) {
        if (result == '' || result == '[]') return;
        var fInfo = eval("(" + result + ")");
        setmenu(sval);
        showSVG(fInfo);
    });
}

function bindEvent() {
    $("svg").css("overflow", "hidden");
    SVGInit();
    GetSvgData();
    var url = "../basepage/HandlerSVG.ashx?method=GetDataActionsJson&nodeid=" + $("#nodeid").val() + "&random=" + Math.random() * 10;
    $.ajax({
        url: url,
        type: 'get',
        success: function (data) {
            var datajson = eval(data);
            if (datajson != null) {
                $.each(datajson, function (itemIndex, item) {
                    var guid = item.shape_guid;
                    var type = item.type;
                    var funstr = item.svalue;
                    if (funstr.indexOf(',') > 0) {
                        $("#" + guid).bind(funstr.split(';')[0], { params: funstr.split(';')[2] }, eval(funstr.split(';')[1]));
                        $("#" + guid).hover( function () {
                            $("#" + guid).css("cursor", "pointer");
                        });
                    }
                });
            }            
        }
    });
}

function AttachEvent(target, eventName, handler, argsObject) {
    var eventHandler = handler;
    if (argsObject) {
        eventHander = function (e) {
            handler.call(argsObject, e);
        }
    }
    target.addEventListener(eventName, eventHander, false);
}


function showSVG(fInfo) {
    $.each(twinkleTimer, function (item) {
        clearInterval(item);
    });
    twinkleTimer = new Array();
    fillColor = new Array();
    dTime = "";
    $("#svg").parent().parent().css("overflow", "auto");

    var sval = fInfo.desc;
    setmenu(sval);
    if (fInfo == null) return;
    var nid = fInfo.id;
    svgFileId = nid;
    var svg = appname + "_" + nid + "_" + fInfo.filesize + ".svg";

    if ($("#svgbutton").css("background-image").indexOf("slide.bmp") > 0) {
        if ($("#svgpic").attr("src") == "" || $("#svgpic").attr("src").indexOf(svgFileId) < 0) {
            PageMethods.GetSvgJPGFile(svgFileId, appname, function (result) {
                var svg = appname + "_" + svgFileId + ".jpg";
                var url = "../svg/svgfiles/" + svg;
                $("#svgpic").attr("src", url);
                if ($("#svgpic")[0].offsetWidth > 20 && $("#svgpic")[0].offsetHeight > 20) {
                    NavWidth = $("#svgpic")[0].offsetWidth;
                    NavHeight = $("#svgpic")[0].offsetHeight;
                    $("#svgbutton").css("bottom", miniSize);
                    $("#SvgNavigation").css("bottom", miniSize);
                    $("#svgbutton").css("right", miniSize);
                    $("#SvgNavigation").css("right", miniSize);
                    redrawArea();
                }
            });

        }
    }
    filesize = fInfo.filesize;
    if (filesize==undefined||nid==undefined||filesize == "0"||filesize==0) {
      //  alert('该节点SVG图形文件不存在.');
        return;
    }
    if (getsvgdata_timer != null) {
        clearInterval(getsvgdata_timer);
    }

    var url = "../svg/svgfiles/" + svg;
    if (nid == undefined || filesize == undefined || filesize == 0) return;
    var win = $.messager.progress({
        title: '正在从服务端加载数据中...',
        msg: ''
    });
    $("#svg").css("display", "none");
    PageMethods.GetSvgFileName(nid, appname, filesize, function (result) {
        if (result == "") return;
        $("#svg").load(url, function () { bindEvent(); });
        svglt = undefined;
        $("#nodeid").val(nid);
        SvgHubInit();
        if (refData > 0) {
            getsvgdata_timer = setInterval("GetSvgData()", refData * 1000);
        }
    });
    document.getElementById("svg").parentNode.parentNode.addEventListener("mousewheel", wheel, null);
    $(getScrollDiv()).bind("scroll", function () {
        if (scrollMark) {
            scrollMark = false;
            setTimeout(redrawArea, 500);
        }
    });

}

var scrollMark = true;
function wheel(event) {
    var t = getScrollDiv();
    var scstatus = true;
    if (t.clientHeight <= t.scrollHeight && t.clientWidth <= t.scrollWidth) {
      scstatus =false;
    }
    var d = $(this).parent().parent();
    if (getBG()) {
        var ev = event.wheelDelta;
        if (ev > 0) {
            Mousezoomin();
        }
        else if (ev < 0) {
            Mousezoomout();
        }
        event.stopPropagation();
        event.preventDefault();
    }
    else if (!getBG() && !scstatus)
    {
        event.stopPropagation();
        event.preventDefault();
    }
}

function getSvgBackDiv() {
    return $("#svg").parent().parent()[0];
}

function getBG() {
    if ($("#divWin").length > 0) {
        if ($("#divWin").css("display") == "none")
            return true;
        else
            return false;
    }
    else {
        return true;
    }
}
if (appname == "joyoaffzpt") {
    OCXStreamInit();
}
function SVGInit() {
    CollectGarbage();
    if (!getBG())
        return;
    if (document.getElementById("svg") == undefined) {
        return;
    }
    $("#svg").parent().parent().css("height", $("#layoutcenter").height() - $("div[id*='_condition']").height() - 30 + "px");
    $("#svg").css("text-align","center");
    var SvgDom = $("svg");
    var wi = getSvgBackDiv();
        initWidth = parseInt(wi.offsetWidth);
        initHeight = parseInt(wi.offsetHeight);
        if ($("#svg").length > 0) {
            $("#svg").width(parseInt(wi.offsetWidth));
            $("#svg").height(parseInt(wi.offsetHeight));
        }
        else {
            $(getSVGBack()[0]).width(parseInt(wi.Width) - 30);
            $(getSVGBack()[0]).height(this.innerHeight - 120);
        }
        if ($('g').length == 0) {
            var nodelist = $(window.frames["listframe"].document).find("g")[0].childNodes;
        }
        else {
            var nodelist = $('g')[0].childNodes;
        }
        var color = "";
        for (var i = 0; i < nodelist.length; i++) {
            if (nodelist[i].localName == "rect") {
                for (var j = 0; j < nodelist[i].attributes.length; j++) {
                    if (nodelist[i].attributes[j].localName == "fill") {
                        color = nodelist[i].attributes[j].value;
                        break;
                    }
                }
            }
        }
        if (SvgDom.length > 0) {
            SetSVGInit(SvgDom[0].attributes, "width", parseInt(wi.offsetWidth) - 50);
            SetSVGInit(SvgDom[0].attributes, "height", parseInt(wi.offsetHeight) - 60);
            $("#svg").parent().parent().css("background", color);
        }
        $("#svgpic").css("visibility", "visible");
        $("#svgbutton").css("visibility", "visible");
        setNavPos();
        redrawArea();
}
function fileExist() {
    if (filesize == "0") {
        alert('该节点SVG图形文件不存在.');
        return true;
    }
    else
        return false;
}


function getSVG() {
    return $("svg");
}

function getSVGBack() {
    return $(window.frames["listframe"].document).find("#svg");
}

function Mousezoomin() {

    var SvgDom = getSVG();
    SetSVGwh(SvgDom[0].attributes, "zoomin", "mouse");
   // setscroll();
}

function Mousezoomout() {

    var SvgDom = getSVG();
    SetSVGwh(SvgDom[0].attributes, "zoomout","mouse");
}

function zoomin() {

    var SvgDom = getSVG();
    if (getBG())
        SetSVGwh(SvgDom[0].attributes, "zoomin","");
      //  setscroll();
}

function zoomout() {

    var SvgDom = getSVG();
    if (getBG())
        SetSVGwh(SvgDom[0].attributes, "zoomout","");
}

function setscroll()
{
    var t = getScrollDiv();
    if (t.clientHeight < t.scrollHeight || t.clientWidth < t.scrollWidth) {
        $("#svg").parent().parent().css("height", $("#layoutcenter").height() - $("div[id*='_condition']").height() - 30 + "px");
    }
}

function SetSVGInit(attr, name,value) {
    for (var i = 0; i < attr.length; i++) {
        if (attr[i].localName == name) {
            attr[i].value = value;

        }
    }
}

function SetSVGwh(attr, operation,type) {
    var wi = getSvgBackDiv();
    if (wi == undefined||attr==undefined)
        return;
    if (type != "mouse") {
        initWidth = parseInt(wi.offsetWidth - 20);
        initHeight = parseInt(wi.offsetHeight - 110);
    }
    else {
        initWidth = parseInt(wi.offsetWidth - 20);
        initHeight = parseInt(wi.offsetHeight - 30);
    }

    for (var i = 0; i < attr.length; i++) {
        if (attr[i].localName == "height" || attr[i].localName == "width") {
            if (operation == "zoomin") {
                    attr[i].value = parseInt(attr[i].value) * 1.1;
            }
            else {
                if (parseInt(attr[i].value) * 0.9 > initWidth * 0.65 && attr[i].localName == "width") {
                    if (parseInt(attr[i].value) * 0.9 < wi.offsetWidth-5) {
                        $("#NavigationArea").css({ "bottom": NavHeight + 3, "width": NavWidth - 4, "height": NavHeight - 4, "left": "0px" });
                    }
                    attr[i].value = parseInt(attr[i].value) * 0.9;
                }
                if (parseInt(attr[i].value) * 0.9 > initHeight * 0.65 && attr[i].localName == "height") {
                    if (parseInt(attr[i].value) * 0.9 < wi.offsetHeight - 100) {
                        $("#NavigationArea").css({ "bottom": NavHeight + 3, "width": NavWidth - 4, "height": NavHeight - 4, "left": "0px" });
                    }
                    attr[i].value = parseInt(attr[i].value) * 0.9;
                }
            }
        }
    }
    setNavPos();
    redrawArea();
}





// 2013-07-01 
function fnId(o) 
{
    return document.getElementById(o);
}

function fnShowWindows(x, y)
{
    if (document.getElementById("divWin")) 
    {
        fnId("divWin").style.zIndex = 999;
         $("#divWin").fadeIn(300, function () {
        fnId("divWin").style.display = "";
        });

        if (x + 170 < document.body.clientWidth) {
            fnId("divWin").style.left = x + "px";
        }
        else {
            fnId("divWin").style.left = (x - 165) + "px";
        }
        if (y + 110 < document.body.clientHeight) {
            fnId("divWin").style.top = y + "px";
        }
        else {
            fnId("divWin").style.top = (y - 110) + "px";
        }
    }
    else 
    {

        var objWin = document.createElement("div");
        objWin.id = "divWin";
        objWin.style.position = "absolute";
        objWin.style.display = "none";
        if (x + 180 < document.body.clientWidth) {
            objWin.style.left = x + "px";
        }
        else {
            objWin.style.left = (x - 180) + "px";
        }
        if (y + 110 < document.body.clientHeight) {
            objWin.style.top = y + "px";
        }
        else {
            objWin.style.top = (y-110) + "px";
        }
        objWin.style.top = y + "px";
        objWin.style.border = "2px solid #AEBBCA";
        objWin.style.background = "#FFF";
        objWin.style.zIndex = 999;
        document.body.appendChild(objWin);

        fnId("divWin").style.zIndex = 999;
        $("#divWin").fadeIn(300, function () {
            fnId("divWin").style.display = "";
        });
    }

//    if (document.getElementById("win_bg")) 
//    {
//        fnId("win_bg").style.zIndex = 998;
//        var x = $("#svg")[0];
//        $("#win_bg").width(x.offsetWidth + 50);
//        $("#win_bg").height(x.offsetTop + 80);
//        fnId("win_bg").style.display = ""; 
//    }
//    else 
//    {
//        var obj_bg = document.createElement("div");
//        obj_bg.id = "win_bg";
//        obj_bg.className = "win_bg";
//        var x = $("#svg")[0].parent();
//        document.body.appendChild(obj_bg);
//        $("#win_bg").width(x.offsetWidth+50);
//        $("#win_bg").height(x.offsetTop + 80);
//    }
}

function fnCloseWindows() {

    $("#divWin").fadeOut(300, function () {
        fnId("divWin").style.display = "none";
    });
   // fnId("win_bg").style.display = "none";
}

function startMove(o, e) 
{
    var wb;
    if (document.all && e.button == 1) wb = true;
    else if (e.button == 0) wb = true;
    if (wb) 
	{
        var x_pos = parseInt(e.clientX - o.parentNode.offsetLeft);
        var y_pos = parseInt(e.clientY - o.parentNode.offsetTop);
        if (y_pos <= o.offsetHeight) 
		{
            document.documentElement.onmousemove = function (mEvent) 
			{
                var eEvent = (document.all) ? event : mEvent;
                o.parentNode.style.left = eEvent.clientX - x_pos + "px";
                o.parentNode.style.top = eEvent.clientY - y_pos + "px";
            }
        }
    }
}

function stopMove(o, e) 
{
    document.documentElement.onmousemove = null;
}

function fnShowDialog(id, op, st, x, y) {
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
    str += '<table width="160"  border="0" cellpadding="0" cellspacing="0"  bgcolor="#FFFFFF">';
    str += ' <tr> <td align="left" valign="top"><table width="100%"  cellspacing="0" cellpadding="1" bgcolor="#FFFFFF" class="TextBorder01">'
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
}


function fnShowEditDialog(id, op, st, x, y) {
    fnShowWindows(x, y);
    var initValue = 0;
    var str = "";
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

    if (theSquare) {
        for (var a = 0; a < theSquare.childNodes.length; a++) {
            var txt = theSquare.childNodes.item(a);
            if (txt.nodeName == "text") {
                for (var b = 0; b < txt.childNodes.length; b++) {
                    var tspan = txt.childNodes.item(b);
                    if (tspan.nodeName == "tspan") {
                        initValue= tspan.firstChild.nodeValue;
                    }
                }
            }
        }
    }
    str += '<table width="160" border="0" cellpadding="0" cellspacing="0"  bgcolor="#FFFFFF">';
    str += '<tr> <td align="left" valign="top"><table width="100%" cellspacing="0" cellpadding="1" bordercolorlight="#97B9D7" bordercolordark="#ffffff" bgcolor="#FFFFFF" class="TextBorder01">';
    str += '<tr align="center"> <td height="32" align="left" class="t2">';
    str += '<table width="100%" height="26" border="0" cellpadding="0" cellspacing="0"> <tr> <td width="80%" class="tdtitle15">&nbsp;&nbsp;选项</td><td width="5%">&nbsp;</td><td width="15%" onclick="fnCloseWindows()" align="center"><div class="closeimg"></div></td></tr> </table>';
    str += '</td></tr><td align="center" valign="top">';
    str += '<table border="0" cellspacing="0" cellpadding="0" width="100%" style=" margin-top:10px;height:78px; "><tr><td>值：<input id="pointValue" onkeyup="verifyVal()" type="text" style=" width:70px;" value="'+initValue+'" /></td></tr>';
    str += '<tr><td height="10px"></td></tr>';
    str += '<tr><td><input type="button" value="提交" onclick="SubmitSetValue(' + id + ')" /> </td></tr>';
    str += '</table></td></table></td></tr></table>';
    fnId("divWin").innerHTML = str;
}

function fnCallBySvg(id, op, st) {
    var p = "Svg -> aspx: " + id + op + st;
    fnShowDialog(id, op, st);
}



function fnOpenMenu(e) {
    var x_pos = parseInt(e.pageX);
    var y_pos = parseInt(e.pageY);
    var pm = e.data.params.split(',');

    fnShowDialog(pm[1], pm[2], "", x_pos, y_pos);
}

function fnOpenVideo(e) {
    var pm = e.data.params.split(',');
    $("#win").window("open");
    if ($("#ocxpanel").html() == "") {
       // $("#ocxpanel").html('<div id="UTStream" style="float:left;width:600px;height:490px;background-color:black;" ></div>');
        $("#ocxpanel").html('<object id="UTStream" name="UTStream" width="600px" height="490px" style="float:left;" codebase="../apppage/JOYOAFFZPT/cab/UTStreamOcx.cab#version=1,0,1,3" classid="clsid:b82ea52f-404b-4f74-899c-1fbcffffb15e"></object>');
    }
    if (pm.length > 2 && pm[2] != "") {
        var pa = pm[2].split("￥");
        init(pa[1].split("=")[1], pa[0].split("=")[1]);
    }
}

function fnOpenEdit(e) {
    var x_pos = parseInt(e.pageX);
    var y_pos = parseInt(e.pageY);
    var pm = e.data.params.split(',');

    fnShowEditDialog(pm[1], pm[2], "", x_pos, y_pos);
}

function fnOpenLink(e) {
    var pm = this.param.split(',');
}


var drag = false;
var startx = 0;
var starty = 0;
function md(e) {
    startx = e.clientX;
    starty = e.clientY;
    drag = true;
    $("#svg").css("cursor", "move");
}

function mu() {
    drag = false;
    startx = 0;
    starty = 0;
    $("#svg").css("cursor", "");
    redrawArea();
}



function mm(e,dom) {
    if (drag) {
        var t = getScrollDiv();
        if (t.clientHeight < t.scrollHeight || t.clientWidth < t.scrollWidth) {
            var xx; var yy;
            xx = t.scrollLeft + startx - e.clientX;
            yy = t.scrollTop + starty - e.clientY;
            t.scrollLeft = xx;
            t.scrollTop = yy;
        }
        startx = e.clientX;
        starty = e.clientY;
    }
}

function Navigation(ev) {
    var x = ev.offsetX/ev.target.clientWidth;
    var h = (ev.offsetY) / ev.target.clientHeight;
    var t = getScrollDiv();
    if(t.scrollWidth>t.offsetWidth || t.scrollHeight>t.offsetHeight)
        $(t).animate({ scrollLeft: x * t.scrollWidth - t.offsetWidth / 2, scrollTop: h * t.scrollHeight - t.offsetHeight / 2 }, 400, function () {
            redrawArea();
        });
}
