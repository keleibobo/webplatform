﻿var username = "";
var pwd = "";
var dev_list = "";
var combine_list = "";
var selnum = 0;
var dataList;
var totalNum = 16;
var Right = new Array();
var style = parent.parent.document.getElementById('style').value;

var call = "0";
var capture = "0";
var rec = "0";
var totalRight = 0;

//初始化生成设备列表和组合列表
$(document).ready(function () {
    var imgList = $("img");
    for (i = 0; i < imgList.length; i++) {
        $(imgList[i]).attr("src", "../images/" + style + "/" + imgList[i].nameProp);
    }
    var k = 1;
    var url = "../../../basepage/GetDeviceList.ashx?random=" + Math.random() * 10;
    $.ajax({
        url: url,
        type: 'get',
        timeout: 60000,
        success: function (data) {
            dataList = jQuery.parseJSON(data);
            dev_list = '<?xml Version=\"1.0\" Encoding=\"UTF-8\"?><DeviceList>';
            if (dataList.ltstm != null) {
                $.ajax({
                    url: "../../../basepage/GetStreamUserData.ashx?random=" + Math.random() * 10,
                    type: 'get',
                    timeout: 60000,
                    success: function (rstr) {
                        for (i = 0; i < dataList.ltstm.length; i++) {
                            if (i == 0) {
                                username = dataList.ltstm[i].user;
                                pwd = dataList.ltstm[i].password;
                            }
                            dev_list += '<Server id=\"' + dataList.ltstm[i].id + '\" ip=\"' + dataList.ltstm[i].addr + '\" port=\"' + dataList.ltstm[i].port + '\" name=\"' + dataList.ltstm[i].name + '\" attribute=\"' + dataList.ltstm[i].attribute + '\" upid=\"0\" addrcode=\"temp\" regionid=\"1\">';
                            for (k = 0; k < dataList.ltstm[i].ltdev.length; k++) {
                                dev_list += '<Dvr id=\"' + (k + 1) + '\" ip=\"' + dataList.ltstm[i].ltdev[k].addr + '\" port=\"' + dataList.ltstm[i].ltdev[k].port + '\" name=\"DVR' + (k + 1) + '\" type=\"' + dataList.ltstm[i].ltdev[k].Type + '\" serverid=\"' + dataList.ltstm[i].id + '\" attribute=\"' + dataList.ltstm[i].ltdev[k].attribute + '\" regionid=\"1\" addrcode=\"' + (dataList.ltstm[i].ltdev[k].addrCode == null ? "" : dataList.ltstm[i].ltdev[k].addrCode) + '\">';
                                for (j = 0; j < dataList.ltstm[i].ltdev[k].ltch.length; j++) {
                                    Right[dataList.ltstm[i].ltdev[k].ltch[j].id] = "0";
                                    dev_list += '<Channel id=\"' + dataList.ltstm[i].ltdev[k].ltch[j].id + '\" name=\"' + dataList.ltstm[i].ltdev[k].ltch[j].name + '\" no=\"' + dataList.ltstm[i].ltdev[k].ltch[j].no + '\" type=\"1\" defPreset=\"0\" interval=\"60\" attribute=\"' + dataList.ltstm[i].ltdev[k].ltch[j].attribute + '\" dvrid=\"' + (k + 1) + '\" regionid=\"1\" addrcode=\"' + (dataList.ltstm[i].ltdev[k].ltch[j].addrCode == null ? "" : dataList.ltstm[i].ltdev[k].ltch[j].addrCode) + '\">';
                                    for (l = 0; l < dataList.ltstm[i].ltdev[k].ltch[j].ltpre.length; l++) {
                                        dev_list += '<Preset camid=\"' + dataList.ltstm[i].ltdev[k].ltch[j].id + '\" no=\"' + dataList.ltstm[i].ltdev[k].ltch[j].ltpre[l].id + '\" name=\"' + dataList.ltstm[i].ltdev[k].ltch[j].ltpre[l].desc + '\"/>';
                                    }
                                    dev_list += '</Channel>';
                                }
                                dev_list += '</Dvr>';
                            }
                            dev_list += '</Server>';
                        }
                        dev_list += '</DeviceList>';
                        setRight(rstr);
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
                        init();
                    }
                });
            }
        },
        error: function (data) {
            alert(data);
        }
    });
    if ($.browser.msie) {
        var rwidth = parseInt($("#jk").css("width").replace("px", ""));
        if ($.browser.version == "9.0") {
            setTimeout(function () {
            $("#UTStream").css("width", document.body.clientWidth - rwidth);
            $("#UTStream").css("height", window.innerHeight - 50);
            }, 100);
        }
        else {
            setTimeout(function () {
                $("#UTStream").css("width", document.body.clientWidth - rwidth);
                $("#UTStream").css("height", document.documentElement.clientHeight - 50);
            }, 100);
        }
    }
    else if ($.browser.chrome) {
        var rwidth = parseInt($("#jk").css("width").replace("px", ""));
        $("#UTStream").css("width", document.body.clientWidth - rwidth);
        $("#UTStream").css("height", document.body.scrollHeight);
    }
});

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
            if (temp.call == "1") {
                call = "1";
            }
            if (temp.capture == "1") {
                capture = "1";
            }
            if (temp.rec == "1") {
                rec = "1";
            }
            if (temp.ControlID != null) {
                for (j = 0; j < temp.ControlID.length; j++) {
                    Right[parseInt(temp.ControlID[j])] = "1";
                }
            }
        }
    }
    var tempRight = call + capture + rec;
    if (tempRight == "111") {
        totalRight = 7;
    }
    else if (tempRight == "110") {
        totalRight = 6;
    }
    else if (tempRight == "101") {
        totalRight = 5;
    }
    else if (tempRight == "100") {
        totalRight = 4;
    }
    else if (tempRight == "011") {
        totalRight = 3;
    }
    else if (tempRight == "010") {
        totalRight = 2;
    }
    else if (tempRight == "001") {
        totalRight = 1;
    }
    else if (tempRight == "000") {
        totalRight = 0;
    }
}

window.onresize = function () {

    if ($.browser.msie) {
        var rwidth = parseInt($("#jk").css("width").replace("px", ""));
        if ($.browser.version == "9.0") {
            setTimeout(function () {
                var dHeight = window.innerHeight;
                $("#UTStream").css("width", document.body.clientWidth - rwidth);
                $("#UTStream").css("height", dHeight - 50);
            }, 100);
        }
        else {
            setTimeout(function () {
                $("#UTStream").css("width", document.body.clientWidth - rwidth);
                $("#UTStream").css("height", document.documentElement.clientHeight - 50);
            }, 100);
        }
    }
    else if ($.browser.chrome) {
        var rwidth = parseInt($("#jk").css("width").replace("px", ""));
        $("#UTStream").css("width", document.body.clientWidth - rwidth);
        $("#UTStream").css("height", document.body.scrollHeight);
    }

}

var Timeproc;
//初始化连接设置用户
function init() {
    UTStream.UT_STREAM_Init(0);
    stm_dev_list();
    stm_setuser(username, pwd);
    stm_setAuthority(username);
    UTStream.UT_STREAM_SetSelWndNo(1);
    UTStream.UT_STREAM_SetWorkType(1);
    Timeproc = setInterval(function () { UTStream.UT_STREAM_Timeproc(); }, 100);

    if (preset == null) {
        preset = setInterval(function () {
            if (selnum != UTStream.UT_STREAM_GetSelWndNo()) {
                selnum = UTStream.UT_STREAM_GetSelWndNo();
                $("#presetList").html('<option value=\"\">请选择预置点</option>');
                var result = UTStream.UT_STREAM_GetPresets(UTStream.UT_STREAM_GetSelWndNo());
                var xx = UTStream.UT_STREAM_GetCamByWnd(UTStream.UT_STREAM_GetSelWndNo());

                if (xx != "") {
                    var xDoc = loadXML(xx);
                    var root = xDoc.selectNodes("NET_PREVIEW_PARAM")[0];
                    if (Right[root.getAttribute("CameraId")] == "0") {
                        var imgList = $("img");
                        var style = parent.parent.document.getElementById('style').value;
                        for (b = 0; b < imgList.length && b < 3; b++) {
                            $(imgList[b]).attr("src", "../images/gray/" + imgList[b].nameProp);
                            $(imgList[b]).unbind();
                        }
                        $("#presetList").attr("disabled", "disabled");
                    }
                    else {
                        var imgList = $("img");
                        var style = parent.parent.document.getElementById('style').value;
                        for (a = 0; a < imgList.length; a++) {
                            $(imgList[a]).attr("src", "../images/" + style + "/" + imgList[a].nameProp);
                        }
                        $("#presetList").removeAttr("disabled");
                    }
                }
                if (result != "") {
                    var xDoc = loadXML(result);
                    var root = xDoc.selectNodes("PRESET_LIST")[0];
                    if (root != null) {
                        for (i = 0; i < root.childNodes.length; i++) {
                            var PRESET = root.childNodes[i];
                            $("#presetList").append('<option value=\"' + PRESET.getAttribute("No") + '\">' + PRESET.getAttribute("Name") + '</option>');
                        }
                    }
                }
            }
        }, 1000);
    }
}

//传入设备列表和组合列表
function stm_dev_list() {
    UTStream.UT_STREAM_SetDeviceList(dev_list);
    var mark = UTStream.UT_STREAM_SetCombineList(combine_list);
}

//设置流媒体用户
function stm_setuser(user, pwd) {
   var result = UTStream.UT_STREAM_SetUser(user, pwd);
}

//设置流媒体用户
function stm_setAuthority(user) {
    if (dataList.ltstm[0].authority != null) {
        UTStream.UT_STREAM_SetAppUser(user, parseInt(totalRight));
    }
}


// 实时预览
function stm_realplay(serverId,channel, ip, port, viewNum) {
    var test_dev = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><NET_PREVIEW_PARAM  CameraId=\"" + channel + "\" StreamType=\"0\" StreamContent=\"0\"  Protocol=\"0\" Reconnect=\"1\"/>";
    var result = UTStream.UT_STREAM_RealPlay(test_dev, viewNum, 0);
}

function stm_stoprealplay(dev, ch) {

}

//设置ocx控件窗口数
function setWindowNum(num) {
    var NumList = $("#huamianNum img");
    $.each(NumList, function (index, item) {
        if ($(item).attr("id") == "huamian" + num) {
            $(item).attr("src", "../images/" + style + "/" + $(item).attr("id") + ".gif");
            totalNum = num;
            UTStream.UT_STREAM_SetWndNum(num);
        }
        else {
            $(item).attr("src", "../images/" + style + "/" + $(item).attr("id") + "_off.gif");
        }
    });
}

//云台方向控制 
function PTZControl(CommandStr, so) {
    if ($("#" + CommandStr).length != 0) {
        if ($("#" + CommandStr).attr("src").indexOf("/gray/") > 0) {
            return;
        }
    }
    if (CommandStr == "left" || CommandStr == "right" || CommandStr == "up" || CommandStr == "down") {
        if ($("#ptzcontrol").attr("src").indexOf("/gray/") > 0) {
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
//导航树双击事件
function nodeDbClick() {
    if (dataList != null) {
        if (dataList.ltstm != null) {
            for (i = 0; i < dataList.ltstm.length; i++) {
                for (k = 0; k < dataList.ltstm[i].ltdev.length; k++) {
                    for (j = 0; j < dataList.ltstm[i].ltdev[k].ltch.length; j++) {
                        if (dataList.ltstm[i].ltdev[k].ltch[j].id == $("#nodeid").val() && dataList.ltstm[i].ltdev[k].ltch[j].stInfo.id == $("#parentid").val()) {
                            viewNum = UTStream.UT_STREAM_GetSelWndNo();
                            selnum = 18;
                            stm_realplay(dataList.ltstm[i].id, dataList.ltstm[i].ltdev[k].ltch[j].id, dataList.ltstm[i].ltdev[k].addr, dataList.ltstm[i].ltdev[k].port, viewNum);
                            if (Right[dataList.ltstm[i].ltdev[k].ltch[j].id] == "0") {
                                var imgList = $("img");
                                var style = parent.parent.document.getElementById('style').value;
                                for (b = 0; b < imgList.length && b < 3; b++) {
                                    $(imgList[b]).attr("src", "../images/gray/" + imgList[b].nameProp);
                                }
                            }
                            else {
                                var imgList = $("img");
                                var style = parent.parent.document.getElementById('style').value;
                                for (a = 0; a < imgList.length; a++) {
                                    $(imgList[a]).attr("src", "../images/" + style + "/" + imgList[a].nameProp);
                                }
                            }
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


function stationDbClick() {
    var imgList = $("img");
    var style = parent.parent.document.getElementById('style').value;
    for (b = 0; b < imgList.length && b < 3; b++) {
        $(imgList[b]).attr("src", "../images/gray/" + imgList[b].nameProp);
    }
    UTStream.UT_STREAM_StopAllPlay();
    var playTotal = 0;
    if (dataList != null) {
        if (dataList.ltstm != null) {
            for (i = 0; i < dataList.ltstm.length; i++) {
                for (k = 0; k < dataList.ltstm[i].ltdev.length; k++) {
                    for (j = 0; j < dataList.ltstm[i].ltdev[k].ltch.length; j++) {
                        if (dataList.ltstm[i].ltdev[k].ltch[j].stInfo.id == $("#parentid").val()) {
                            playTotal++;
                        }
                    }
                }
            }
        }
    }
    if (playTotal == 1) {
        setWindowNum(1);
        totalNum = 1;
    }
    else if (playTotal > 1 && playTotal <= 4) {
        setWindowNum(4);
        totalNum = 4;
    }
    if (playTotal > 4 && playTotal <= 9) {
        setWindowNum(9);
        totalNum = 9;
    }
    if (playTotal > 9 && playTotal <= 16) {
        setWindowNum(17);
        totalNum = 16;
    }
    var playNum = 1;
    if (dataList != null) {
        if (dataList.ltstm != null) {
            for (i = 0; i < dataList.ltstm.length; i++) {
                for (k = 0; k < dataList.ltstm[i].ltdev.length; k++) {
                    for (j = 0; j < dataList.ltstm[i].ltdev[k].ltch.length; j++) {
                        if (dataList.ltstm[i].ltdev[k].ltch[j].stInfo.id == $("#parentid").val()) {
                            stm_realplay(dataList.ltstm[i].id, dataList.ltstm[i].ltdev[k].ltch[j].id, dataList.ltstm[i].ltdev[k].addr, dataList.ltstm[i].ltdev[k].port, playNum);
                            playNum++;
                            if (playNum == 17) {
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
    $("#presetList").html('<option value=\"\"></option>');
    if (preset == null) {
        preset = setInterval(function () {
            if (selnum != UTStream.UT_STREAM_GetSelWndNo()) {
                selnum = UTStream.UT_STREAM_GetSelWndNo();
                $("#presetList").html('<option value=\"\">请选择预置点</option>');
                var result = UTStream.UT_STREAM_GetPresets(UTStream.UT_STREAM_GetSelWndNo());
                var xx = UTStream.UT_STREAM_GetCamByWnd(UTStream.UT_STREAM_GetSelWndNo());

                if (xx != "") {
                    var xDoc = loadXML(xx);
                    var root = xDoc.selectNodes("NET_PREVIEW_PARAM")[0];
                    if (Right[root.getAttribute("CameraId")] == "0") {
                        var imgList = $("img");
                        var style = parent.parent.document.getElementById('style').value;
                        for (b = 0; b < imgList.length && b < 3; b++) {
                            $(imgList[b]).attr("src", "../images/gray/" + imgList[b].nameProp);
                            $(imgList[b]).unbind();
                            $("#presetList").attr("disabled", "disabled"); 
                        }
                    }
                    else {
                        var imgList = $("img");
                        var style = parent.parent.document.getElementById('style').value;
                        for (a = 0; a < imgList.length; a++) {
                            $(imgList[a]).attr("src", "../images/" + style + "/" + imgList[a].nameProp);
                            $("#presetList").removeAttr("disabled"); 
                        }
                    }
                }
                if (result != "") {
                    var xDoc = loadXML(result);
                    var root = xDoc.selectNodes("PRESET_LIST")[0];
                    if (root != null) {
                        for (i = 0; i < root.childNodes.length; i++) {
                            var PRESET = root.childNodes[i];
                            $("#presetList").append('<option value=\"' + PRESET.getAttribute("No") + '\">' + PRESET.getAttribute("Name") + '</option>');
                        }
                    }
                }
            }
        }, 1000);
    }
}

//导航树单击事件
function nodeClick() {
    if (dataList != null) {
        if (dataList.ltstm != null) {
            for (i = 0; i < dataList.ltstm.length; i++) {
                for (k = 0; k < dataList.ltstm[i].ltdev.length; k++) {
                    for (j = 0; j < dataList.ltstm[i].ltdev[k].ltch.length; j++) {
                        if (dataList.ltstm[i].ltdev[k].ltch[j].id == $("#nodeid").val() && dataList.ltstm[i].ltdev[k].ltch[j].stInfo.id == $("#parentid").val()) {
                            UTStream.UT_STREAM_SetSelWnd(dataList.ltstm[i].id, dataList.ltstm[i].ltdev[k].addr, parseInt(dataList.ltstm[i].ltdev[k].ltch[j].id));
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

//预置点选择
function presetSelect() {
    var presetid = $("#presetList").val();
    if (presetid != "") {
        viewNum = UTStream.UT_STREAM_GetSelWndNo();
        UTStream.UT_STREAM_PTZCtrlByWnd(strToMark("callpreset"), false, presetid, viewNum);
    }
}

//打开组合播放窗口
function openCombineWindow() {
    selnum = 18;
    UTStream.UT_STREAM_PlayCombineDb();
}

function openScoutWindow() {
    if ($("#xunshi").text() == "开始") {
        var reslut = UTStream.UT_STREAM_StartScoutDb();
        if (reslut) {
            $("#xunshi").text("停止");
        }
    }
    else {
        $("#xunshi").text("开始");
        UTStream.UT_STREAM_StopScout();
    }
}

//字符串序列号xml
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

function pcCall() {
    UTStream.UT_STREAM_CallPcTalk();
}

function cutPic() {
    UTStream.UT_STREAM_CapPicByWnd(UTStream.UT_STREAM_GetSelWndNo(), 1);
}

var recMark = false;

function Rec() {
    recMark = true;
    UTStream.UT_STREAM_StartLRecByWnd(UTStream.UT_STREAM_GetSelWndNo());
}

function stopRec() {
    UTStream.UT_STREAM_StopLRecByWnd(UTStream.UT_STREAM_GetSelWndNo());
    if (recMark == true) {
        recMark = false;
        alert("已停止录像!");
    }
}

window.onbeforeunload = function () {
    clearInterval(Timeproc);
    UTStream.UT_STREAM_UnInit();
}

function openSound() {
   var result = UTStream.UT_STREAM_OpenSound(UTStream.UT_STREAM_GetSelWndNo());
}

function closeSound() {
    var result = UTStream.UT_STREAM_CloseSound(UTStream.UT_STREAM_GetSelWndNo());
}
