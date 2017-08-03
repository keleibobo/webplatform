
function messagerShow(title, msg) {
    if ($("#MessagePop").html() == undefined) {
        //if (msg != "") {
        //    msg = "<div style='left:0px;'>" + msg + "</div>";
        //}
        $.messager.show({
            id: "MessagePop",
            title: title,
            msg: msg,
            showSpeed: 800,
            timeout: 0,
            height: 270,
            width: 570,
            showType: 'slide'
        });
        if (appname == "joyoaffzpt") {
            $("#messageback").animate({ height: 271 }, 800);
        }
        var style = parent.document.getElementById('style').value;
        $("#MessagePop").css({ "overflow-y": "scroll" });
        $(".panel-tool-close").unbind();
        $(".panel-tool-close").css("background", "url('../css/" + style + "/themes/default/images/collapse.bmp')");
        $(".panel-tool-close").click(function () {
            if ($(".panel-tool-close").css("background-image").indexOf("collapse") > 0) {
                if (appname == "joyoaffzpt") {
                    $("#messageback").animate({ height: 0 }, 600);
                }
                $("#MessagePop").animate({ height: 0 }, 500, function () {
                    $("#MessagePop").css({ "padding": "0px" });
                    $("#MessagePop").parent().css({ "display": "none" });
                });

                $(".panel-tool-close").css("background", "url('../css/" + style + "/themes/default/images/expand.bmp')");
            }
            else {
                $(".panel-tool-close").css("background", "url('../css/" + style + "/themes/default/images/collapse.bmp')");
                $("#MessagePop").animate({ height: 200 }, 500);
            }
            if (document.all != null && window.event!=null) {
                window.event.returnValue = false;
            }
            else if (event != null) {
                event.preventDefault();
            }
        });
    }
    else {
        if (msg != "") {
            $("#evtb").prepend(msg);
            if (appname == "joyoaffzpt") {
                $("#messageback").animate({ height: 236 }, 800);
            }
        }
        if ($(".panel-tool-close").css("background-image").indexOf("expand") > 0) {
            $(".panel-tool-close").click();
        }
    }
    $("#MessagePop").animate({ scrollTop: 0 }, 500);
    if (msg != "") {
        var messageList = $("#MessagePop tr");
        if (messageList.length - 1 > window.EventDisplaySize) {
            $(messageList[messageList.length-1]).remove();
        }
        else if(msg.indexOf('table') < 0) {
            $("#EventTimes").text(parseInt($("#EventTimes").text()) + 1);
        }

    }
    $(".messager-body").css("padding-top", "0px");
}



function delMessage(t) {
    $(t).parent().fadeOut(500, function () {
        $(t).parent().remove();
    });
}


var totalData;
function startshow(sapp) {
    appname = sapp;
    EventHubInit();
    var url = "../basepage/HandlerEvent.ashx?componentid=&random=" + Math.random() * 10;
    $.ajax({
        url: url,
        type: 'get',
        timeout: 3000,
        success: function (data) {
            totalData = data;
            if (window.ShowEventWindow == true) {

                /*
                 *时间 2017.7.28
                 *维护人员 李士群
                 *修改内容 .对这行代码进行注释,从而使弹框不会一开始就弹出来
                 */

              //  messagerShow("告警事件", GetTableTh());
                InitData();
            }
            else {
                if (totalData != "") {
                    var mlist = jQuery.parseJSON(totalData);
                    $("#EventTimes").text(parseInt(mlist.total));
                }
            }
        }
    });
}

function InitData() {
    if (totalData != "") {
        var mlist = jQuery.parseJSON(totalData);
        var ev = "";
        for (var i = 0; i < mlist.event.length; i++) {
            ev += GetTableTd(mlist.event[i]);
        }
        if (ev != "") {
            $("#evtb").append(ev);
            if ($(".panel-tool-close").length > 0 && $(".panel-tool-close").css("background-image").indexOf("expand") > 0) {
                $(".panel-tool-close").click();
            }
        }
        $("#EventTimes").text(parseInt(mlist.total));
    }
    else {
        $("#EventTimes").text("0");
    }
}

var appname = "";
function GetTableTh() {
    var str = "";
        if (appname == "joyoaffzpt" ) {
            str = "<div style='height:22px;'><table class='fixedtable'><tr><th height='20' width='190'>时间</th><th width='344'>事件描述</th></tr></table></div>";
        }
        else if (appname == "joyo-d41") {
            str = "<div style='height:22px;'><table class='fixedtable'><tr><th height='20' width='108'>时间</th><th width='79'>报警设备</th><th width='58'>事件类型</th><th width='59'>报警级别</th><th width='199'>事件描述</th></tr></table></div>";
        }
        else if (appname == "joyoc2") {
            str = "<div style='height:22px;'><table class='fixedtable'><tr><th height='20' width='140'>时间</th><th width='394'>事件描述</th></tr></table></div>";
        }
        str += "<table id='evtb'><table>";
    return str;
}

function GetTableTd(event) {
    var str = "";
    var length = $("#evtb").find("tr").length;
    if (event != null && length == 0) {
        if (appname == "joyoaffzpt" ) {
            str = "<tr id='tr1' height='25'><td width='190'>" + event.dt + "</td><td width='344'>" + event.desc + "</td></tr>";
        }
        else if (appname == "joyo-d41") {
            str = "<tr id='tr1' height='25'><td width='108'>" + event.dt + "</td>" + "<td width='79'>" + event.alarmtype + "</td>" + "<td width='58'>" + event.eventtype + "</td>" + "<td width='59'>" + event.alarmlevel + "</td>" + "<td width='199'>" + event.desc + "</td></tr>";
        }
        else if (appname == "joyoc2") {
            str = "<tr id='tr1' height='25'><td width='140'>" + event.dt + "</td><td width='394'>" + event.desc + "</td></tr>";
        }
    }
    else {
        if (appname == "joyoaffzpt" ) {
            str = "<tr height='25'><td width='190'>" + event.dt + "</td><td width='344'>" + event.desc + "</td></tr>";
        }
        else if (appname == "joyo-d41") {
            str = "<tr height='25'><td width='108'>" + event.dt + "</td>" + "<td width='79'>" + event.alarmtype + "</td>" + "<td width='58'>" + event.eventtype + "</td>" + "<td width='59'>" + event.alarmlevel + "</td>" + "<td width='199'>" + event.desc + "</td></tr>";
        }
        else if (appname == "joyoc2") {
            str = "<tr height='25'><td width='140'>" + event.dt + "</td><td width='394'>" + event.desc + "</td></tr>";
        }
    }
    str = str.replace("undefined", "");
    return str;
}

function Getcomfirm(event) {
    var str = "";
    if (event != null) {
        if (appname == "joyoaffzpt") {
            str = "<td width=\"190\">" + event.dt + "</td><td width=\"344\">" + event.desc + "</td>";
        }
        else if (appname == "joyo-d41") {
            str = "<td width='108'>" + event.dt + "</td>" + "<td width='79'>" + event.alarmtype + "</td>" + "<td width='58'>" + event.eventtype + "</td>" + "<td width='59'>" + event.alarmlevel + "</td>" + "<td width='199'>" + event.desc + "</td>";
        }
        else if (appname == "joyoaffzpt") {
            str = "<td width=\"140\">" + event.dt + "</td><td width=\"394\">" + event.desc + "</td>";
        }
    }
    str = str.replace("undefined", "");
    return str;
}


function confirmEvent(event) {
    var messageList = $("#MessagePop tr");
    for (var i = 0; i < messageList.length; i++) {
        if (messageList[i].innerHTML == event) {
            $(messageList[i]).fadeOut(500, function () {
                $(messageList[i]).remove();
            });
            $("#EventTimes").text(parseInt($("#EventTimes").text()) - 1);
            break;
        }
    }
}

function showEventlist() {
    if (appname == "joyoaffzpt") {
        $("#messageback").animate({ height: 236 }, 600);
    }
    if (window.ShowEventWindow == false) {
        window.ShowEventWindow = true;
        messagerShow("告警事件", GetTableTh());
        InitData();
    }
    else {
        messagerShow("告警事件", "");
    }
    $("#MessagePop").parent().css({ "display": "inline" });
    if ($(".panel-tool-close").css("background-image").indexOf("expand") > 0) {
        $(".panel-tool-close").click();
    }
}

function EventHubInit() {
    var EventHub = $.connection.eventHub;
    $.connection.hub.logging = true;

    EventHub.client.pushEvent = function (event) {
        if (window.ShowEventWindow == false) {
            
            window.ShowEventWindow = true;
            messagerShow("告警事件", GetTableTh());
            InitData();
        }
        if (event != "") {
            $('body').append('<bgsound src="http://' + location.host + '/sound/Alarm.wav" loop=false  autostart=false >');
            var evt = jQuery.parseJSON(event);
            for (var i = 0; i < evt.length; i++) {
                if (evt[i].extendparams.indexOf("cf=1") >= 0) {
                    confirmEvent(Getcomfirm(evt[i]));
                }
                else {
                    $("#MessagePop").parent().css({ "display": "inline" });
                    messagerShow("告警事件", GetTableTd(evt[i]));
                }
            }
        }
    };

    EventHub.client.confirmEvent = function (event) {
        if (event != "") {
            var evt = jQuery.parseJSON(event);
            for (var i = 0; i < evt.length; i++) {
                confirmEvent(evt[i].dt + " " + evt[i].desc);
            }
        }
    };

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
        //   alert("[" + new Date().toTimeString() + "]: " + oldState + " => " + newState + " " + $.connection.hub.id );
    });
    $.connection.hub.start();
    
}