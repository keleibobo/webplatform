
//var appname;
//var refData;
var lasttime = "";
var svglt; //标记刷新数据
var refTree;
var sign = "-UT-";
var treeinfo = [];
var showtype;
var firload = true;
var lastcl;
var filesize;
var initWidth;
var getsvgdata_timer;
var tree_timer;

// 2013-07-01 
// svg control 
var svgDoc = null;
var svgRoot = null;
var svgWnd = null;
//var svgid = "18";


var miniSize = 0;
$(document).ready(function () {
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
});

function setInit(iappname,irefData) {
    appname=iappname;
    refData=irefData;
}


function getSVG() {
    var o = $("svg")//$("#component_" + svgid);
    return o;
}



function clicklink(e) {
    var paramater = this.param.split(',');

    sval = paramater[1];
    if (sval != undefined && sval != "")
        PageMethods.GetLinkFilename(sval, function (result) {

            if (result == '' || result == '[]') return;

            var fInfo = eval("(" + result + ")");
            $("#TreeNodeText").val(sval);
            //setmenu(sval);
            showSVG(fInfo); //.width, fInfo.height

        });
}


function AttachEvent(target, eventName, handler, argsObject) {
    var eventHandler = handler;
    if (argsObject) {
        eventHander = function (e) {
            handler.call(argsObject, e);
        }
    }
    if (target != null)
        target.addEventListener(eventName, eventHander, false);
}
var retry = 0;
function bindEvent() {
    SVGInit();
    GetSvgData();
    NoRightClick();
    wheel();
    var url = "../basepage/HandlerSVG.ashx?method=GetDataActionsJson&nodeid=" + $("#nodeid").val();
    $.ajax({
        url: url,
        type: 'get',
        success: function (data) {
            var datajson = eval("(" + data + ")");
            if (datajson != null) {
                $.each(datajson, function (itemIndex, item) {
                    var guid = item.shape_guid;
                    var type = item.type;
                    var funstr = item.svalue;
                    if (funstr.indexOf(',') > 0) {
                        try {
                            var svgEle = document.getElementById('svgEle');
                            var svgDoc = svgEle.getSVGDocument();
                            var g2 = svgDoc.getElementById(guid);
                            if (g2 == null) {
                                if (retry < 10) {
                                    retry++;
                                    setTimeout(onlyBindEvent, 600);
                                    return false;
                                }
                            }
                            else {
                                retry = 0;
                            }
                            obj = new Object();
                            obj.param = funstr.split(';')[2];
                            if (funstr.indexOf(',') > 0) {
                                AttachEvent(g2, funstr.split(';')[0], eval(funstr.split(';')[1]), obj);
                                var pn = g2.parentNode;
                                if (pn.nodeName == "a") return;
                                var aspan = svgDoc.createElementNS("http://www.w3.org/2000/svg", "a");
                                aspan.setAttributeNS("http://www.w3.org/2000/xlink/namespace/", "xlink:href", "");
                                aspan.appendChild(g2);
                                pn.appendChild(aspan);
                            }
                        } catch (e) { }
                    }
                });
            }
        }
    });
}


function onlyBindEvent() {
    GetSvgData();
    wheel();
    var url = "../basepage/HandlerSVG.ashx?method=GetDataActionsJson&nodeid=" + $("#nodeid").val();
    $.ajax({
        url: url,
        type: 'get',
        success: function (data) {
            var datajson = eval("(" + data + ")");
            if (datajson != null) {
                $.each(datajson, function (itemIndex, item) {
                    var guid = item.shape_guid;
                    var type = item.type;
                    var funstr = item.svalue;
                    if (funstr.indexOf(',') > 0) {
                        try {
                            var svgEle = document.getElementById('svgEle');
                            var svgDoc = svgEle.getSVGDocument();
                            var g2 = svgDoc.getElementById(guid);
                            if (g2 == null) {
                                if (retry < 10) {
                                    retry++;
                                    setTimeout(onlyBindEvent, 600);
                                    return false;
                                }
                            }
                            else {
                                retry = 0;
                            }
                            obj = new Object();
                            obj.param = funstr.split(';')[2];
                            if (funstr.indexOf(',') > 0) {
                                AttachEvent(g2, funstr.split(';')[0], eval(funstr.split(';')[1]), obj);
                                var pn = g2.parentNode;
                                if (pn.nodeName == "a") return;
                                var aspan = svgDoc.createElementNS("http://www.w3.org/2000/svg", "a");
                                aspan.setAttributeNS("http://www.w3.org/2000/xlink/namespace/", "xlink:href", "");
                                aspan.appendChild(g2);
                                pn.appendChild(aspan);
                            }
                        } catch (e) { }
                    }
                });
            }
        }
    });
}


function showSVG(fInfo) { //, appname, refData
    $.each(twinkleTimer, function (item) {
        clearInterval(item);
    });
    twinkleTimer = new Array();
    fillColor = new Array();
    var sval = fInfo.desc;
    setmenu(sval);
    if (fInfo == null) return;
    var w = fInfo.width;
    var h = fInfo.height;
    var wi = getWindowInfo();
 //   w = document.body.clientWidth - $("#layoutwest").width() - $("#layouteast").width() - 40; //
    h = document.documentElement.clientHeight-120;
    document.getElementById("svg").parentNode.parentNode.parentNode.style.overflow = "hidden";
    document.getElementById("svg").parentNode.parentNode.style.overflow = "auto";
    document.getElementById("svg").parentNode.style.textAlign = "center";
   // var tdocument = document.getElementById("svg");
   // tdocument.style.height = h+"px";

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

    filesize = fInfo.filesize;//svg.split("_")[2].split(".")[0];
    if (filesize == undefined || nid == undefined || filesize == "0" || filesize == 0) {
     //   alert('该节点SVG图形文件不存在.');
        return;
    }
    if (getsvgdata_timer != null) {
        clearInterval(getsvgdata_timer);
    }
    dTime = "";

    if (nid == undefined || filesize == undefined || filesize == 0) return;


    PageMethods.GetSvgFileName(nid, appname, filesize, function (result) {

        //等待下载文件
        if (result == "") return;
        var win = $.messager.progress({
            title: '正在从服务端加载数据中...',
            msg: '',
            interval: 500
        });
        var embedsvg = "<embed wmode='transparent' id='svgEle'  src='../svg/svgfiles/" + svg + "'  width='" + w + "' height='" + h + "' oncontextmenu='customContextMenu(event)'></embed>"; //width='"+w+"' height='"+h+"'
        if ($("#svg")[0].childNodes != null && $("#svg")[0].childNodes.length > 0) {
            var sd = document.getElementById("svg");
            var sdd = document.getElementById("svgEle");
            sd.removeChild(sdd);
            sdd = null;
            CollectGarbage();
        }
        $("#svg").html(embedsvg);
        $("#svgEle").attr("width", w);
        $("#svgEle").attr("height", h);
        SvgHubInit();
        checksvg();
        $("#svgEle").ready(function () {
            setTimeout(bindEvent, 500);
        })


        //  }
        $("#nodeid").val(nid);
        svglt = undefined;

        $("#svg").hide();
        if (refData > 0) {
            getsvgdata_timer = setInterval("GetSvgData()", refData * 1000);
        }

        $(document.getElementById("svg").parentNode.parentNode).bind("scroll", function () {
            if (scrollMark) {
                scrollMark = false;
                setTimeout(redrawArea, 500);
            }
        });

        document.getElementById("svg").parentNode.parentNode.style.height = document.documentElement.clientHeight - 71;
        zoomout();
        //        window.onresize = function () {
        //            $(".easyui-layout").layout("resize");
        //            document.getElementById("svg").parentNode.parentNode.style.height = document.documentElement.clientHeight - 71;

        //        }
    });


}
var scrollMark = true;
OCXStreamInit();
function SVGInit() {
    var wi = getWindowInfo();
    if (wi == undefined)
        return;
    var back = document.getElementById("svg").parentNode.parentNode;
    initWidth = parseInt(wi.Width);
    initHeight = parseInt(wi.Height);
    $("#svg").parent().parent().css("height", $("#layoutcenter").height() - $("div[id*='_condition']").height() - 30 + "px");
    var back = document.getElementById("svg").parentNode.parentNode;
    $("#svgEle").attr("width", back.clientWidth-20);
    $("#svgEle").attr("height", back.clientHeight-20);

    if ($.browser.msie && $.browser.version == "8.0") {
        var url = "../basepage/HandlerSVG.ashx?method=GetRDDataJson&a=1&b=1&c=backcolor&d=fid=" + $("#nodeid").val()+";appname="+appname;
        $.ajax({
            url: url,
            type: 'get',
            success: function (data) {

                if (data == ''||data=='[]') return;
                var d = eval("(" + data + ")");
                var color = jQuery.parseJSON(d[1].v);

                //  $(window.frames["listframe"].document.body).css("background", color.color);
                document.getElementById("layoutcenter").style.background = color.color;

            }

            ,
            error: function (XMLHttpRequest, textStatus, errorThrown) {

                alert("error:" + textStatus);

            }
        });
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

function zoomin() {
    if ($.browser.msie && $.browser.version == "8.0") {
        var svgele = $("#svgEle");
        svgele.attr("width", svgele.width() * 1.1);
        svgele.attr("height", svgele.height() * 1.1);
    }

    setNavPos();
    redrawArea();

}
function zoomout() {
    if ($.browser.msie && $.browser.version == "8.0") {

        //  var svgele = $(window.frames["listframe"].document).find("#svgEle");
        var svgele = $("#svgEle");
        if (svgele.width() < 100 || svgele.height() < 100) return;
        svgele.attr("width", svgele.width() * 0.9)
        svgele.attr("height", svgele.height() * 0.9)



    }

    setNavPos();
    redrawArea();

}

function SetSVGInit(attr, name, value) {
    for (var i = 0; i < attr.length; i++) {
        if (attr[i].localName == name) {
            attr[i].value = value;

        }
    }
}

/**
* 时间对象的格式化
*/
Date.prototype.format = function (format) {
    /*
    * format="yyyy-MM-dd hh:mm:ss";
    */
    var o = {
        "M+": this.getMonth() + 1,
        "d+": this.getDate(),
        "h+": this.getHours(),
        "m+": this.getMinutes(),
        "s+": this.getSeconds(),
        "q+": Math.floor((this.getMonth() + 3) / 3),
        "S": this.getMilliseconds()
    }

    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4
- RegExp.$1.length));
    }

    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1
? o[k]
: ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
}


function getWindowInfo() {
    var scrollX = 0, scrollY = 0, width = 0, height = 0, contentWidth = 0, contentHeight = 0;
    if (typeof (window.pageXOffset) == 'number') {
        scrollX = window.pageXOffset;
        scrollY = window.pageYOffset;
    }
    else if (document.body && (document.body.scrollLeft || document.body.scrollTop)) {
        scrollX = document.body.scrollLeft;
        scrollY = document.body.scrollTop;
    }
    else if (document.documentElement && (document.documentElement.scrollLeft || document.documentElement.scrollTop)) {
        scrollX = document.documentElement.scrollLeft;
        scrollY = document.documentElement.scrollTop;
    }
    if (typeof (window.innerWidth) == 'number') {
        width = window.innerWidth;
        height = window.innerHeight;
    }
    else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
        width = document.documentElement.clientWidth;
        height = document.documentElement.clientHeight;
    }
    else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
        width = document.body.clientWidth;
        height = document.body.clientHeight;
    }
    if (document.documentElement && (document.documentElement.scrollHeight || document.documentElement.offsetHeight)) {
        if (document.documentElement.scrollHeight > document.documentElement.offsetHeight) {
            contentWidth = document.documentElement.scrollWidth;
            contentHeight = document.documentElement.scrollHeight;
        }
        else {
            contentWidth = document.documentElement.offsetWidth;
            contentHeight = document.documentElement.offsetHeight;
        }
    }
    else if (document.body && (document.body.scrollHeight || document.body.offsetHeight)) {
        if (document.body.scrollHeight > document.body.offsetHeight) {
            contentWidth = document.body.scrollWidth;
            contentHeight = document.body.scrollHeight;
        } else {
            contentWidth = document.body.offsetWidth;
            contentHeight = document.body.offsetHeight;
        }
    } else {
        contentWidth = width;
        contentHeight = height;
    }
    if (height > contentHeight)
        height = contentHeight;
    if (width > contentWidth)
        width = contentWidth;
    if (width == 0)
        return;
    var rect = new Object();
    rect.ScrollX = scrollX;
    rect.ScrollY = scrollY;
    rect.Width = width;
    rect.Height = height;
    rect.ContentWidth = contentWidth;
    rect.ContentHeight = contentHeight;
    return rect;
}

// this js code use in svg image 
// 2013-07-01 
function fnId(o) {
    return document.getElementById(o);
}



function fnCloseWindows() {
    fnId("divWin").style.display = "none";
    fnId("win_bg").style.display = "none";
}

function startMove(o, e) {
    var wb;
    if (document.all && e.button == 1) wb = true;
    else if (e.button == 0) wb = true;
    if (wb) {
        var x_pos = parseInt(e.clientX - o.parentNode.offsetLeft);
        var y_pos = parseInt(e.clientY - o.parentNode.offsetTop);
        if (y_pos <= o.offsetHeight) {
            document.documentElement.onmousemove = function (mEvent) {
                var eEvent = (document.all) ? event : mEvent;
                o.parentNode.style.left = eEvent.clientX - x_pos + "px";
                o.parentNode.style.top = eEvent.clientY - y_pos + "px";
            }
        }
    }
}

function stopMove(o, e) {
    document.documentElement.onmousemove = null;
}



function fnCallBySvg(id, op, st) {
    //alert('这个消息框是在svg的js中调用html的js函数产生的');
    var p = "Svg -> aspx: " + id + op + st;
    //alert(p);
    // 显示窗口选项 
    fnShowDialog(id, op, st);
}




// this js code use in svg.js 
// 2013-07-02 
function fnOpenMenu(e) {
    var x_pos = parseInt(e.clientX/* - this.parentNode.offsetLeft*/);
    var y_pos = parseInt(e.clientY/* - this.parentNode.offsetTop*/);
    var pm = this.param.split(',');

    fnShowDialog(pm[1], pm[2], "", x_pos, y_pos);
}

function fnOpenVideo(e) {
    var pm = this.param.split(',');
    $("#win").window("open");
    if ($("#ocxpanel").html() == "") {
        $("#ocxpanel").html('<object id="UTStream" name="UTStream" width="600px" height="490px" style="float:left;" codebase="../apppage/JOYOAFFZPT/cab/UTStreamOcx.cab#version=1,0,1,3" classid="clsid:b82ea52f-404b-4f74-899c-1fbcffffb15e"></object>');
    }
    if (pm.length > 2 && pm[2] != "") {
        var pa = pm[2].split("￥");
        init(pa[1].split("=")[1], pa[0].split("=")[1]);
    }
}

function fnShowEditDialog(id, op, st, x, y) {
    fnShowWindows(x, y);
    var str = "";
    var initValue = 0;
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
                        initValue = tspan.firstChild.nodeValue;
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
    str += '<table border="0" cellspacing="0" cellpadding="0" width="100%" style=" margin-top:10px;height:78px; "><tr><td>值：<input id="pointValue" onkeyup="verifyVal()" type="text" style=" width:70px;" value="' + initValue + '" /></td></tr>';
    str += '<tr><td height="10px"></td></tr>';
    str += '<tr><td><input type="button" value="提交" onclick="SubmitSetValue(' + id + ')" /> </td></tr>';
    str += '</table></td></table></td></tr></table>';
    fnId("divWin").innerHTML = str;
}

function fnOpenEdit(e) {
    var x_pos = parseInt(e.clientX/* - this.parentNode.offsetLeft*/);
    var y_pos = parseInt(e.clientY/* - this.parentNode.offsetTop*/);
    var pm = this.param.split(',');

    fnShowEditDialog(pm[1], pm[2], "", x_pos, y_pos);
}

function fnOpenLink(e) {
    var pm = this.param.split(',');
}

function NoRightClick() {
    var el = document.getElementById("svgEle");
    try{
        var svgDoc = el.getSVGDocument();
        var svgRoot = svgDoc.documentElement;
        svgRoot.oncontextmenu = false;

        var contextMenu = el.window.contextMenu;
        var menuXml = "<menu><item></item><menu>";
        var newMenuRoot = el.window.parseXML(menuXml, contextMenu);
        contextMenu.replaceChild(newMenuRoot, contextMenu.firstChild);
    }catch(e){}

}

function wheel() {
    document.getElementById("svg").parentNode.parentNode.onmousewheel = function () {
        var delta = event.wheelDelta;
        if (delta > 0) {
            zoomin()
        } else {
            zoomout()
        }
        return false;

    }
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

function mm(e, id) {
    if (drag) {
        var t = id.parentNode.parentNode;
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
    var x = ev.offsetX / ev.srcElement.clientWidth;
    var h = ev.offsetY  / ev.srcElement.clientHeight;

    var t = document.getElementById("svg").parentNode.parentNode;
    if (t.scrollWidth > t.offsetWidth || t.scrollHeight > t.offsetHeight)
        $(t).animate({ scrollLeft: x * t.scrollWidth - t.offsetWidth / 2, scrollTop: h * t.scrollHeight - t.offsetHeight / 2 }, 400, function () {
            redrawArea();
        });
}




