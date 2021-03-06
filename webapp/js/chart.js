var lastime = 0;
var begintime = 0; //开始时间
var timerange = 0; //显示的实时数据的 时间间隔
var interval = 0; //0历史 不更新数据
var ctype = "";
var chartTimer;
var refreshTimer;//曲线自动刷新timer
var maxpoint = 10;
var tableTimer;
var componentName;
var componentParam;
var extendparam;
var hischart;
var doubleChartId = "";
var doubleChartParam = "";

Highcharts.setOptions({
    global: {
        useUTC: false
    }
});

function SetExtendparam(param) {
    interval = 0;
    param = param.replace("\"{", "{");
    param = param.replace("}\"", "}");
    extendparam = jQuery.parseJSON(param);
    if (extendparam["business"].indexOf('real') > -1) {
        interval = 1000 * parseInt(extendparam["chartRefreshData"]);
    }
}

var initTableTop;

function f_loadingchart(cname, param) {
    if (cname.indexOf(";") > -1)
        return;
    if (cname.indexOf("chart") > 0) {
        if ($("#" + cname).length == 0) {
            var dc = $("div[id^=component_]");
            dc.each(function (idx, item) {
                var split = this.id.split('_');
                var cid = split[1];
                var ctype = split[2];
                if (ctype.indexOf('chart') > -1) {
                    cname = this.id;
                }
            });
        }
    }
    if ($("#component_47_querydatatable").length > 0) {
        $("#component_47_querydatatable").parent().css("overflow", "scroll");
        initTableTop = parseInt($("#component_47_querydatatable").css("top").replace('px', '')) - 20;
    }
    var flag = ifchart(cname, $("#nodepath").val());
    if (!flag) return;

    if (chartTimer != null) {
        clearInterval(chartTimer);
    }

    var sPara = ""+param+f_conditon("");
    var sid = cname.split('_')[1];
    var url = "";
    if (appname == "joyoaffzpt") {
        url = "../basepage/AFHandlerChart.ashx?sid=" + sid + "&" + sPara + "&random=" + Math.random() * 10;
    }
    else {
        url = "../basepage/HandlerChart.ashx?sid=" + sid + "&" + sPara + "&random=" + Math.random() * 10;
    }
    $.ajax({
        url: url,
        type: 'get',
        success: function (data) {
            var title = "";
            
            if (data == "[]") {
                $('#' + cname).highcharts({
                    chart: {
                        type: 'line'
                    },
                    exporting: {
                        enabled: false
                    },
                    title: {
                        text: title,
                        x: -20 //center
                    },
                    series: []
                });
            }
            $.messager.progress('close');
            if ($("#valuetable").length != 0) {
                $("#valuetable").html("");
            }

            var options = eval("(" + data + ")");
            if (options.title == null) {
                options.title = getitle();
            }
            var StatistType = "";
            if ($("#webcomm_StatistType").length > 0) {
                StatistType = $("#webcomm_StatistType").combobox('getText');
            }
            var nodepath = $("#nodepath").val();
            if (options.series != null && options.series.length != null && options.series.length > 0) {
                lastime = options.series[0].data[0][0];
                begintime = lastime;
                if (StatistType.indexOf("时间") == -1) {
                    $("#component_38_querydatatable").parent().prev().show();
                }
            }
            if (appname == "joyoaffzpt") {
                if ((nodepath == "" || nodepath.indexOf("1(FZPTSvr.syspowerdatatype)") > -1 || nodepath.indexOf("3(FZPTSvr.syspowerdatatype)") > -1) && StatistType.indexOf("时间") == -1) {
                    $("#" + cname).parent().show();
                    if ($("#component_47_querydatatable").length > 0) {
                        $("#component_47_querydatatable").parent().css("top", initTableTop);
                    }
                    $("#tbCondition").show();
                } else {
                    $("#" + cname).parent().hide();
                    if ($("#component_47_querydatatable").length > 0) {
                        var filtertr = document.getElementById("filtertr");
                        if (filtertr.style.display == "none") {
                            $("#component_47_querydatatable").parent().css("top", "0px");
                        } else {
                            $("#component_47_querydatatable").parent().css("top", "40px");
                            $("#tbCondition").hide();
                        }
                    }
                }
            }
            else {
                if (!(nodepath.indexOf("2(C2Svr.syspowerdatatype)") > -1 || nodepath.indexOf("5(C2Svr.syspowerdatatype)") > -1)) {
                    $("#" + cname).parent().show();
                    $("#tbCondition").show();
                    if ($("#component_47_querydatatable").length > 0) {
                        $("#component_47_querydatatable").parent().css("top", "485px").css("position", "absolute");
                    }

                } else {
                    $("#" + cname).parent().hide();
                    if ($("#component_47_querydatatable").length > 0) {
                        var filtertr = document.getElementById("filtertr");
                        if (filtertr.style.display == "none") {
                            $("#component_47_querydatatable").parent().css("top", "0px");
                        } else {
                            $("#component_47_querydatatable").parent().css("top", "40px");
                            $("#tbCondition").hide();
                        }
                    }
                }
            }
            $.messager.progress('close');

            if (interval > 0) {
                if (data != "[]") {
                    if (timerange != 0) {
                        var lineNum = options.series.length;
                        var time = timerange / 1000;
                        if ($.browser.msie || $.browser.mozilla) {
                            if (version == 9 || version == 10 || version == 11) {
                                interval = parseInt((time / 1000) * lineNum) * 1000;
                            } else {
                                interval = parseInt((time / 300) * lineNum) * 1000;
                            }
                        }

                        if (interval < 5000) {
                            interval = 5000;
                        }
                    }
                    componentName = cname;
                    componentParam = param;
                    realtime(cname, param);
                }
                setTimeout(function() {
                    var nextDom = $("#" + cname).parent().next();
                    var trList = $("#" + $(nextDom).find("div:first").attr("id")).find("table tr");
                    if (trList.length > 0) {
                        clearInterval(tableTimer);
                        tableTimer = setInterval(function() {
                            UpdateTableData($(nextDom).find("div:first").attr("id"), $("#currentpage" + $(nextDom).find("div:first").attr("id")).val());
                        }, interval);
                    }
                }, 300);
            } else {
                if (data == "[]") {
                    return;
                }
                if (appname == "joyo-d41" && $("#stype").val() == "monthloadanalysis") {
                    if ($("#stattype").combobox('getText') == "平均值") {
                        options.tooltip.xDateFormat = '%Y-%m-%d';
                    }
                }
                options.chart.renderTo = cname;
                if (StatistType.indexOf("时间") == -1) {
                    if (StatistType != "采集数据" && StatistType !="") {
                        if ($("#webcomm_StatistCycle").combobox('getText') == "小时") {
                            options.xAxis.labels.formatter = formater;
                        } else if ($("#webcomm_StatistCycle").combobox('getText') == "日") {
                            options.xAxis.labels.formatter = Rformater;
                        } else if ($("#webcomm_StatistCycle").combobox('getText') == "月") {
                            options.xAxis.labels.formatter = Yformater;
                        }
                    } else {
                        options.xAxis.labels.formatter = Normalformater;
                    }
                    options.xAxis.tickPixelInterval = 180;
                    new Highcharts.Chart(options);
                }
            }
        }
    });
}

function Normalformater() {
    return Highcharts.dateFormat('%Y-%m-%d %H:%M:%S', this.value);
}

function formater () {
    return Highcharts.dateFormat('%m-%d %H:%M:%S', this.value);
}

function Rformater() {
    return Highcharts.dateFormat('%Y-%m-%d', this.value);
}
function Yformater() {
    return Highcharts.dateFormat('%Y-%m', this.value);
}

function getitle() {
    var title = $("#menutext").val();
    var indx= title.lastIndexOf(">");
    title = title.substring(indx + 1);
    title.replace("&nbsp;", " ");
    var title =eval("({'text':'" + title + "'})");
    return title;
}

function chartTypeChuange()
{
    alert("ccc");
}

var MinValue;
var MaxValue;
var MinTime;
var MaxTime;
var valueLength = 0;
var nameStr = "";

var globalChartName = "";
var globalArgs = "";
//客户端定时请求
function realtime(cname, args) {
    if ($("#valuetable").length != 0) {
        $("#valuetable").html("");
    }
    globalChartName = cname;
    globalArgs = args;
    var sid = cname.split('_')[1];
    var sPara = f_conditon(args);
    var url = "";
    if (appname == "joyoaffzpt") {
        url = "../basepage/AFHandlerChart.ashx?sid=" + sid + "&" + sPara + "&random=" + Math.random() * 10;
    }
    else {
        url = "../basepage/HandlerChart.ashx?sid=" + sid + "&" + sPara + "&random=" + Math.random() * 10;
    }
    if (chartTimer != null) {
        clearInterval(chartTimer);
    }
    new Highcharts.Chart({
        exporting: {
            enabled: false
        },
        chart: {
            renderTo: cname,
            defaultSeriesType: 'spline',
            events: {
                load: function () {
                    var index = new Array();
                    var series = $("#" + cname).highcharts().series;
                    $.ajax({
                        url: url,
                        type: 'get',
                        success: function (data) {
                            if (data == "" || data == "[]") return;
                            var options = eval("(" + data + ")");
                            $("#" + cname).css("height", options.chart.height);
                            if (options.chart.type == "spline") {
                                if (options.series.length > 0) {
                                    if (valueLength != options.series.length) {
                                        MinValue = new Array();
                                        MaxValue = new Array();
                                        MinTime = new Array();
                                        MaxTime = new Array();
                                        nameStr = "";
                                        $.each(options.series, function (indx, item) {
                                            MinValue[item.name] = item.data[0][1];
                                            MaxValue[item.name] = item.data[0][1];
                                            MinTime[item.name] = item.data[0][0];
                                            MaxTime[item.name] = item.data[0][0];
                                            nameStr += item.name + ",";
                                        });
                                        valueLength = options.series.length;
                                    }
                                    else {
                                        nameStr = "";
                                        $.each(options.series, function (indx, item) {
                                            if (MaxValue[item.name] != null) {
                                                if (MaxValue[item.name] < item.data[0][1]) {
                                                    MaxValue[item.name] = item.data[0][1];
                                                    MaxTime[item.name] = item.data[0][0];
                                                }
                                            }
                                            else {
                                                MaxValue[item.name] = item.data[0][1];
                                                MaxTime[item.name] = item.data[0][0];
                                            }
                                            if (MinValue[item.name] != null) {
                                                if (MinValue[item.name] > item.data[0][1]) {
                                                    MinValue[item.name] = item.data[0][1];
                                                    MinTime[item.name] = item.data[0][0];
                                                }
                                            }
                                            else {
                                                MinValue[item.name] = item.data[0][1];
                                                MinTime[item.name] = item.data[0][0];
                                            }
                                            nameStr += item.name + ",";
                                        });
                                    }
                                    if (series.length == 0) {
                                        options.title = getitle();
                                        $("#" + cname).highcharts(options);
                                        series = $("#" + cname).highcharts().series;
                                    }
                                    $.each(options.series, function (indx, item) {
                                        var id = indx;
                                        if (id != undefined) {
                                            var y = item.data[0][1];
                                            if (timerange > 0 && lastime - begintime > timerange) {
                                                series[id].addPoint([item.data[0][0], y], true, true);
                                            } else {
                                                series[id].addPoint([item.data[0][0], y], true, false);
                                            }
                                        }
                                    });
                                    AppendValueTable(MaxValue, MinValue, MaxTime, MinTime, cname, nameStr);
                                }
                            }
                            else if (options.chart.type == "column") {
                                $("#" + cname).highcharts(options);
                                if (options.series.length > 0) {
                                    if (valueLength != options.xAxis.categories.length) {
                                        MinValue = new Array();
                                        MaxValue = new Array();
                                        MinTime = new Array();
                                        MaxTime = new Array();
                                        nameStr = "";
                                        $.each(options.series[0].data, function (indx, item) {
                                            MinValue[options.xAxis.categories[indx]] = item;
                                            MaxValue[options.xAxis.categories[indx]] = item;
                                            MinTime[options.xAxis.categories[indx]] = GetDateStr();
                                            MaxTime[options.xAxis.categories[indx]] = GetDateStr();
                                        });
                                        
                                        valueLength = options.xAxis.categories.length;
                                    }
                                    else {
                                        nameStr = "";
                                        $.each(options.series[0].data, function (indx, item) {
                                            if (MaxValue[options.xAxis.categories[indx]] != null) {
                                                if (MaxValue[options.xAxis.categories[indx]] < item) {
                                                    MaxValue[options.xAxis.categories[indx]] = item;
                                                    MaxTime[options.xAxis.categories[indx]] = GetDateStr();
                                                }
                                            }
                                            else {
                                                MaxValue[options.xAxis.categories[indx]] = item;
                                                MaxTime[options.xAxis.categories[indx]] = GetDateStr();
                                            }
                                            if (MinValue[options.xAxis.categories[indx]] != null) {
                                                if (MinValue[options.xAxis.categories[indx]] > item) {
                                                    MinValue[options.xAxis.categories[indx]] = item;
                                                    MinTime[options.xAxis.categories[indx]] = GetDateStr();
                                                }
                                            }
                                            else {
                                                MinValue[options.xAxis.categories[indx]] = item;
                                                MinTime[options.xAxis.categories[indx]] = GetDateStr();
                                            }
                                        });
                                    }
                                    $.each(options.xAxis.categories, function (indx, item) {
                                        nameStr += item + ",";
                                    });
                                    AppendCValueTable(MaxValue, MinValue, MaxTime, MinTime, cname, nameStr);
                                }
                            }
                            else if (options.series[0].type == "pie") {
                                $("#" + cname).highcharts(options);
                                if (options.series123.length > 0) {
                                    if (valueLength != options.series123[0].data.length) {
                                        MinValue = new Array();
                                        MaxValue = new Array();
                                        MinTime = new Array();
                                        MaxTime = new Array();
                                        nameStr = "";
                                        $.each(options.series123[0].data, function (indx, item) {
                                            MinValue[item[0]] = item[1];
                                            MaxValue[item[0]] = item[1];
                                            MinTime[item[0]] = GetDateStr();
                                            MaxTime[item[0]] = GetDateStr();
                                            nameStr += item[0] + ",";
                                        });
                                        valueLength = options.series123[0].data.length;
                                    }
                                    else {
                                        nameStr = "";
                                        $.each(options.series123[0].data, function (indx, item) {
                                            if (MaxValue[item[0]] != null) {
                                                if (MaxValue[item[0]] < item[1]) {
                                                    MaxValue[item[0]] = item[1];
                                                    MaxTime[item[0]] = GetDateStr();
                                                }
                                            }
                                            else {
                                                MaxValue[item[0]] = item[1];
                                                MaxTime[item[0]] = GetDateStr();
                                            }
                                            if (MinValue[item[0]] != null) {
                                                if (MinValue[item[0]] > item[1]) {
                                                    MinValue[item[0]] = item[1];
                                                    MinTime[item[0]] = GetDateStr();
                                                }
                                            }
                                            else {
                                                MinValue[item[0]] = item[1];
                                                MinTime[item[0]] = GetDateStr();
                                            }
                                            nameStr += item[0] + ",";
                                        });
                                    }
                                    AppendCValueTable(MaxValue, MinValue, MaxTime, MinTime, cname, nameStr);
                                }
                            }
                        }
                    });
                    chartTimer = setInterval(function () {
                        var index = new Array();
                        var series = $("#" + cname).highcharts().series;
                        if (appname == "joyoaffzpt") {
                            url = "../basepage/AFHandlerChart.ashx?sid=" + sid + "&" + sPara + "&random=" + Math.random() * 10;
                        }
                        else {
                            url = "../basepage/HandlerChart.ashx?sid=" + sid + "&" + sPara + "&random=" + Math.random() * 10;
                        }
                        $.ajax({
                            url: url,
                            type: 'get',
                            success: function (data) {
                                if (data == "" || data == "[]") return;
                                var options = eval("(" + data + ")");
                                if (options.chart.type == "spline") {
                                    if (options.series.length > 0) {
                                        lastime = options.series[0].data[0][0];
                                        if (valueLength != options.series.length) {
                                            MinValue = new Array();
                                            MaxValue = new Array();
                                            MinTime = new Array();
                                            MaxTime = new Array();
                                            nameStr = "";
                                            $.each(options.series, function (indx, item) {
                                                MinValue[item.name] = item.data[0][1];
                                                MaxValue[item.name] = item.data[0][1];
                                                MinTime[item.name] = item.data[0][0];
                                                MaxTime[item.name] = item.data[0][0];
                                                nameStr += item.name + ",";
                                            });
                                            valueLength = options.series.length;
                                        }
                                        else {
                                            nameStr = "";
                                            $.each(options.series, function (indx, item) {
                                                if (MaxValue[item.name] != null) {
                                                    if (MaxValue[item.name] < item.data[0][1]) {
                                                        MaxValue[item.name] = item.data[0][1];
                                                        MaxTime[item.name] = item.data[0][0];
                                                    }
                                                }
                                                else {
                                                    MaxValue[item.name] = item.data[0][1];
                                                    MaxTime[item.name] = item.data[0][0];
                                                }
                                                if (MinValue[item.name] != null) {
                                                    if (MinValue[item.name] > item.data[0][1]) {
                                                        MinValue[item.name] = item.data[0][1];
                                                        MinTime[item.name] = item.data[0][0];
                                                    }
                                                }
                                                else {
                                                    MinValue[item.name] = item.data[0][1];
                                                    MinTime[item.name] = item.data[0][0];
                                                }
                                                nameStr += item.name + ",";
                                            });
                                        }
                                        if (series.length == 0) {
                                            options.title = getitle();
                                            $("#" + cname).highcharts(options);
                                            series = $("#" + cname).highcharts().series;
                                        }
                                        $.each(options.series, function (indx, item) {
                                            var id = indx;
                                            if (id != undefined) {
                                                var y = item.data[0][1];
                                                if (timerange > 0 && lastime - begintime > timerange) {
                                                    series[id].addPoint([item.data[0][0], y], true, true);
                                                } else {
                                                    series[id].addPoint([item.data[0][0], y], true, false);
                                                }
                                            }
                                        });
                                        AppendValueTable(MaxValue, MinValue, MaxTime, MinTime, cname, nameStr);
                                    }
                                }
                                else if (options.chart.type == "column") {
                                    $("#" + cname).highcharts(options);
                                    if (options.series.length > 0) {
                                        if (valueLength != options.xAxis.categories.length) {
                                            MinValue = new Array();
                                            MaxValue = new Array();
                                            MinTime = new Array();
                                            MaxTime = new Array();
                                            nameStr = "";
                                            $.each(options.series[0].data, function (indx, item) {
                                                MinValue[options.xAxis.categories[indx]] = item;
                                                MaxValue[options.xAxis.categories[indx]] = item;
                                                MinTime[options.xAxis.categories[indx]] = GetDateStr();
                                                MaxTime[options.xAxis.categories[indx]] = GetDateStr();
                                            });
                                            
                                            valueLength = options.xAxis.categories.length;
                                        }
                                        else {
                                            nameStr = "";
                                            $.each(options.series[0].data, function (indx, item) {
                                                if (MaxValue[options.xAxis.categories[indx]] != null) {
                                                    if (MaxValue[options.xAxis.categories[indx]] < item) {
                                                        MaxValue[options.xAxis.categories[indx]] = item;
                                                        MaxTime[options.xAxis.categories[indx]] = GetDateStr();
                                                    }
                                                }
                                                else {
                                                    MaxValue[options.xAxis.categories[indx]] = item;
                                                    MaxTime[options.xAxis.categories[indx]] = GetDateStr();
                                                }
                                                if (MinValue[options.xAxis.categories[indx]] != null) {
                                                    if (MinValue[options.xAxis.categories[indx]] > item) {
                                                        MinValue[options.xAxis.categories[indx]] = item;
                                                        MinTime[options.xAxis.categories[indx]] = GetDateStr();
                                                    }
                                                }
                                                else {
                                                    MinValue[options.xAxis.categories[indx]] = item;
                                                    MinTime[options.xAxis.categories[indx]] = GetDateStr();
                                                }
                                            });
                                        }
                                        $.each(options.xAxis.categories, function (indx, item) {
                                            nameStr += item + ",";
                                        });
                                        AppendCValueTable(MaxValue, MinValue, MaxTime, MinTime, cname, nameStr);
                                    }
                                }
                                else if (options.series[0].type == "pie") {
                                    $("#" + cname).highcharts(options);
                                    if (options.series123.length > 0) {
                                        if (valueLength != options.series123[0].data.length) {
                                            MinValue = new Array();
                                            MaxValue = new Array();
                                            MinTime = new Array();
                                            MaxTime = new Array();
                                            nameStr = "";
                                            $.each(options.series123[0].data, function (indx, item) {
                                                MinValue[item[0]] = item[1];
                                                MaxValue[item[0]] = item[1];
                                                MinTime[item[0]] = GetDateStr();
                                                MaxTime[item[0]] = GetDateStr();
                                                nameStr += item[0] + ",";
                                            });
                                            valueLength = options.series123[0].data.length;
                                        }
                                        else {
                                            nameStr = "";
                                            $.each(options.series123[0].data, function (indx, item) {
                                                if (MaxValue[item[0]] != null) {
                                                    if (MaxValue[item[0]] < item[1]) {
                                                        MaxValue[item[0]] = item[1];
                                                        MaxTime[item[0]] = GetDateStr();
                                                    }
                                                }
                                                else {
                                                    MaxValue[item[0]] = item[1];
                                                    MaxTime[item[0]] = GetDateStr();
                                                }
                                                if (MinValue[item[0]] != null) {
                                                    if (MinValue[item[0]] > item[1]) {
                                                        MinValue[item[0]] = item[1];
                                                        MinTime[item[0]] = GetDateStr();
                                                    }
                                                }
                                                else {
                                                    MinValue[item[0]] = item[1];
                                                    MinTime[item[0]] = GetDateStr();
                                                }
                                                nameStr += item[0] + ",";
                                            });
                                        }
                                        AppendCValueTable(MaxValue, MinValue, MaxTime, MinTime, cname, nameStr);
                                    }
                                }
                            }
                        });
                    }, interval);
                }
            }
        },
        title: ' '
    })
}

function AppendValueTable(max, min, maxt, mint, id, nameStr) {
    var tabStr = "<div id='valuetableson'  style='height:110px;'><table class='fixedtable' width='84%' border:1px' border='0' cellpadding='0' cellspacing='0'>";
    var name = nameStr.split(',');
    if (name.length > 0) {
        tabStr += "<tr height='20px'><td class='tdheadv' border='1' width='170px'>点名</td>";
        for (var i = 0; i < name.length - 1 && i < 8; i++) {
            tabStr += "<td class='tdheadv'>" + name[i] + "</td>";
        }
        tabStr += "</tr>";
        tabStr += "<tr height='20px'><td class='td1v'>最大值  /  时间</td>";
        for (var i = 0; i < name.length - 1 && i < 8; i++) {
            tabStr += "<td class='td1'>" + max[name[i]] + "  /  " + timestamptostr(maxt[name[i]]) + "</td>";
        }
        tabStr += "</tr>";
        tabStr += "<tr height='20px'><td class='td1v'>最小值  /  时间</td>";
        for (var i = 0; i < name.length - 1 && i < 8; i++) {
            tabStr += "<td class='td1'>" + min[name[i]] + "  /  " + timestamptostr(mint[name[i]]) + "</td>";
        }
        tabStr += "</tr>";
    }
    tabStr += "</table></div>";
    if ($("#valuetable").length == 0) {
        tabStr = "<div id='valuetable' style='height:120px;'>" + tabStr + "</div>";
        $("#" + id).after(tabStr);
        $("#valuetableson").css("width", $("#valuetableson table").css("width"));
        var nextDom = $("#valuetable").parent().next();
    }
    else {
        $("#valuetable").html(tabStr);
        $("#valuetableson").css("width", $("#valuetableson table").css("width"));
    }

    var ntop = $("#valuetable").parent().next()[0].offsetTop - 266 - 135;
    $("#valuetable").css("margin-top", ntop / 2);
}

function AppendPValueTable(max, min, maxt, mint, id, nameStr) {
    var tabStr = "<div id='valuetableson'  style='height:110px;'><table class='fixedtable' width='84%' border:1px' border='0' cellpadding='0' cellspacing='0'>";
    var name = nameStr.split(',');
    if (name.length > 0) {
        tabStr += "<tr height='20px'><td class='tdheadv' border='1' width='170px'>点名</td>";
        for (var i = 0; i < name.length - 1 && i < 8; i++) {
            tabStr += "<td class='tdheadv'>" + name[i] + "</td>";
        }
        tabStr += "</tr>";
        tabStr += "<tr height='20px'><td class='td1v'>最大值  /  时间</td>";
        for (var i = 0; i < name.length - 1 && i < 8; i++) {
            tabStr += "<td class='td1'>" + (parseFloat(max[name[i]]) * 100).toFixed(2) + "%  /  " + maxt[name[i]] + "</td>";
        }
        tabStr += "</tr>";
        tabStr += "<tr height='20px'><td class='td1v'>最小值  /  时间</td>";
        for (var i = 0; i < name.length - 1 && i < 8; i++) {
            tabStr += "<td class='td1'>" + (parseFloat(min[name[i]]) * 100).toFixed(2) + "%  /  " + mint[name[i]] + "</td>";
        }
        tabStr += "</tr>";
    }
    tabStr += "</table></div>";
    if ($("#valuetable").length == 0) {
        tabStr = "<div id='valuetable' style='height:120px;'>" + tabStr + "</div>";
        $("#" + id).after(tabStr);
        $("#valuetableson").css("width", $("#valuetableson table").css("width"));
        var nextDom = $("#valuetable").parent().next();
    }
    else {
        $("#valuetable").html(tabStr);
        $("#valuetableson").css("width", $("#valuetableson table").css("width"));
    }
    var ntop = $("#valuetable").parent().next()[0].offsetTop - 266 - 135;
    $("#valuetable").css("margin-top", ntop / 2);
}

function AppendCValueTable(max, min, maxt, mint, id, nameStr) {
    var tabStr = "<div id='valuetableson'  style='height:110px;'><table class='fixedtable' width='84%' border:1px' border='0' cellpadding='0' cellspacing='0'>";
    var name = nameStr.split(',');
    if (name.length > 0) {
        tabStr += "<tr height='20px'><td class='tdheadv' border='1' width='170px'>点名</td>";
        for (var i = 0; i < name.length - 1 && i < 8; i++) {
            tabStr += "<td class='tdheadv'>" + name[i] + "</td>";
        }
        tabStr += "</tr>";
        tabStr += "<tr height='20px'><td class='td1v'>最大值  /  时间</td>";
        for (var i = 0; i < name.length - 1 && i < 8; i++) {
            tabStr += "<td class='td1'>" + max[name[i]] + "  /  " + maxt[name[i]] + "</td>";
        }
        tabStr += "</tr>";
        tabStr += "<tr height='20px'><td class='td1v'>最小值  /  时间</td>";
        for (var i = 0; i < name.length - 1 && i < 8; i++) {
            tabStr += "<td class='td1'>" + min[name[i]] + "  /  " + mint[name[i]] + "</td>";
        }
        tabStr += "</tr>";
    }
    tabStr += "</table></div>";
    if ($("#valuetable").length == 0) {
        tabStr = "<div id='valuetable' style='height:120px;'>" + tabStr + "</div>";
        $("#" + id).after(tabStr);
        $("#valuetableson").css("width", $("#valuetableson table").css("width"));
        var nextDom = $("#valuetable").parent().next();
    }
    else {
        $("#valuetable").html(tabStr);
        $("#valuetableson").css("width", $("#valuetableson table").css("width"));
    }
    var ntop = $("#valuetable").parent().next()[0].offsetTop - 266 - 135;
    $("#valuetable").css("margin-top", ntop / 2);
}

function timestamptostr(timestamp) {

    var datetime = new Date();
    datetime.setTime(timestamp);
    var year = datetime.getFullYear();
    //月份重0开始，所以要加1，当小于10月时，为了显示2位的月份，所以补0
    var month = datetime.getMonth() + 1 < 10 ? "0" + (datetime.getMonth() + 1) : datetime.getMonth() + 1;
    var date = datetime.getDate() < 10 ? "0" + datetime.getDate() : datetime.getDate();
    var hour = datetime.getHours() < 10 ? "0" + datetime.getHours() : datetime.getHours();
    var minute = datetime.getMinutes() < 10 ? "0" + datetime.getMinutes() : datetime.getMinutes();
    var second = datetime.getSeconds() < 10 ? "0" + datetime.getSeconds() : datetime.getSeconds();
    return +hour + ":" + minute + ":" + second;
}

function GetDateStr() {

    var datetime = new Date();
    var hour = datetime.getHours() < 10 ? "0" + datetime.getHours() : datetime.getHours();
    var minute = datetime.getMinutes() < 10 ? "0" + datetime.getMinutes() : datetime.getMinutes();
    var second = datetime.getSeconds() < 10 ? "0" + datetime.getSeconds() : datetime.getSeconds();
    return +hour + ":" + minute + ":" + second;
}

function UpdateTableData(ctrlIdDesc, currentpage) {

    var sPara = f_conditon("");
    var totalnum = $("#table" + ctrlIdDesc + "total").val();
    var url = "../basepage/RealTimeData.ashx?ctrlIdDesc=" + ctrlIdDesc + "&currentpage=" + currentpage + "&sPara=" + escape(sPara) + "&totalnum=" + totalnum + "&t=" + new Date().getTime();
    $.ajax({
        url: url,
        type: 'get',
        success: function (data) {
            var DataList = jQuery.parseJSON(data);
            var TrList = $("#" + ctrlIdDesc).find("table tr");
            var pointName = 0;
            var pointValue = 0;
            var pointStatus = 0;
            var pointManual = 0;
            var pointRefresh = 0;
            var statusIs = false;
            if (appname == "joyoaffzpt") {
                $.each($(TrList[0]).find("td"), function (index, td) {
                    if (td.innerHTML.indexOf("点名") > -1) {
                        pointName = index;
                    }
                    else if (td.innerHTML.indexOf("状态") > -1) {
                        pointStatus = index;
                        statusIs = true;
                    }
                    else if (td.innerHTML.indexOf("值") > -1) {
                        pointValue = index;
                    }
                    else if (td.innerHTML.indexOf("人工置数") > -1) {
                        pointManual = index;
                    }
                    else if (td.innerHTML.indexOf("刷新") > -1) {
                        pointRefresh = index;
                    }
                });
                $.each(TrList, function (index, tritem) {
                    if (index != 0) {
                        var tdname = $(tritem).find("td:eq(" + pointName + ")");
                        for (i = 0; i < DataList.length; i++) {
                            if (DataList[i].pointname == tdname[0].innerHTML) {
                                var tdvalue = $(tritem).find("td:eq(" + pointValue + ")");
                                tdvalue[0].innerHTML = DataList[i].value;
                                var manual = $(tritem).find("td:eq(" + pointManual + ")");
                                manual[0].innerHTML = DataList[i].manual;
                                var refresh = $(tritem).find("td:eq(" + pointRefresh + ")");
                                refresh[0].innerHTML = DataList[i].refresh;
                                if (statusIs) {
                                    var tdvalue = $(tritem).find("td:eq(" + pointStatus + ")");
                                    tdvalue[0].innerHTML = DataList[i].status;
                                }
                                break;
                            }
                        }
                    }

                });
            }
            else if (appname == "joyoc2")
            {
                $.each($(TrList[0]).find("td"), function (index, td) {
                    if (td.innerHTML.indexOf("描述") > -1) {
                        pointName = index;
                    }
                    else if (td.innerHTML.indexOf("状态") > -1) {
                        pointStatus = index;
                    }
                    else if (td.innerHTML.indexOf("值") > -1) {
                        pointValue = index;
                    }
                });
                $.each(TrList, function (index, tritem) {
                    if (index != 0) {
                        var tdname = $(tritem).find("td:eq(" + pointName + ")");
                        for (i = 0; i < DataList.length; i++) {
                            if (DataList[i].pointname == tdname[0].innerHTML) {
                                var tdvalue = $(tritem).find("td:eq(" + pointValue + ")");
                                tdvalue[0].innerHTML = DataList[i].value;
                                if (pointStatus != 0) {
                                    var statusValue = $(tritem).find("td:eq(" + pointStatus + ")");
                                    statusValue[0].innerHTML = DataList[i].status;
                                }
                                break;
                            }
                        }
                    }
                });
            }
        }
    });
}

function ifchart(cname, treenode) {

    if (cname) {
        $("#" + cname).show();

        return true; //test
    }
    if (treenode == undefined) return false;
    var temp = treenode.split("|");
    var ntype = temp[temp.length - 2];  //bug

    //    var dc = $("div[id^=component_]");
    //    dc.each(function (idx, item) {
    //        if (this.id.split('_')[2].indexOf("chart") > -1) {
    //            if (ntype != ctype) {
    //                $(item).hide();
    //            } else {
    //                realtime(this.id,"");
    //                $(item).show();
    //            }
    //        }
    //    });



    if (ntype == undefined || ntype != ctype) {
        $("#" + cname).hide();
        return false;
    } else {
        if (interval != 0) {
            realtime(cname, "");
        }
        $("#" + cname).show();
    }
    return true;
}

function getComponent(type) {
    var rs = [];
    var dc = $("div[id^=component_]");
    dc.each(function (idx, item) {
        var split = this.id.split('_');
        var cid = split[1];
        var ctype = split[2];
        if (ctype == type) {
            rs.push(item);
        }
    });

}

function f_changetimerange(cid) {
    timerange = 0;
    $("#" + cid).combobox({
        onChange: function (n, o) {
            timerange = $("#" + cid).combobox('getValue');
            if (timerange > 0) {
                timerange = timerange * 60000;
            }
            else {
                timerange = 60 * 24 * 60000;
            }
            if (componentName != null) {
                f_loadingchart(componentName, componentParam);
            }
        },
        onSelect: function (n, o) {
            timerange = $("#" + cid).combobox('getValue');
            if (timerange > 0) {
                timerange = timerange * 60000;
            }
            else {
                timerange = 60 * 24 * 60000;
            }
        }
    });
}


//new chartjs


function f_loadingdoublechart(cname, param) {
    doubleChartId = cname;
    doubleChartParam = param;
    var idlist = cname.split(';');
    if (chartTimer != null) {
        clearInterval(chartTimer);
    }
    if (refreshTimer != null) {
        clearInterval(refreshTimer);
    }
    for (var i = 0; i < idlist.length; i++) {
        cname = idlist[i];
        var sPara = "" + param + f_conditon("");
        var sid = cname.split('_')[1];

        var dc = $("div[id^=component_]");
        dc.each(function (idx, item) {
            var split = this.id.split('_');
            var cid = split[1];
            if (cid == sid) {
                cname = this.id;
            }
        });

        var url = "../basepage/HandlerChart.ashx?sid=" + sid + "&" + sPara + "&rendername=" + cname + "&random=" + Math.random() * 10;
        $.ajax({
            url: url,
            type: 'get',
            success: function (data) {
                var title = "";
                if (interval > 0) {
                    title = "";
                } else {
                    title = "";
                }
                if (data == "[]") {
                    dc.each(function (idx, item) {
                        var split = this.id.split('_');
                        var cid = split[1];
                        if (split[2].indexOf("chart")>-1) {
                            $('#' + this.id).highcharts({
                                chart: {
                                    type: 'line'
                                },
                                exporting: {
                                    enabled: false
                                },
                                title: {
                                    text: title,
                                    x: -20 //center
                                },
                                series: []
                            });
                        }
                    });
                }
                $.messager.progress('close');
                var options = eval("(" + data + ")");
                
                if (options.series != null && options.series.length != null && options.series.length > 0) {
                    lastime = options.series[0].data[0][0];
                    begintime = lastime;
                }
                if (data != "[]" && extendparam[options.chart.renderTo.split('_')[1] + "interval"]!=null) {
                    interval = parseInt(extendparam[options.chart.renderTo.split('_')[1] + "interval"]);
                } else {
                    interval = 0;
                }
                $.messager.progress('close');
                if (data != "[]" && interval == 0) {
                    options.plotOptions = {};
                    options.plotOptions.series = {};
                    options.plotOptions.series = {
                        marker: {
                            enabled: false
                        }
                    }
                    options.yAxis.gridLineWidth = {};
                    options.yAxis.gridLineWidth = 0;
                    options.yAxis.labels = {
                            formatter: function() {
                                return this.value ;
                            }
                    }
                    options.legend = {};
                    options.legend.enabled = {};
                    options.legend.enabled = false;
                    options.xAxis.labels.formatter = function() {
                            return Highcharts.dateFormat('%H:%M:%S', this.value);
                    }
                    options.chart.marginRight = 0;
                    options.title.text = "历史数据";
                    options.title.x = -210;
                    options.title.style = { "opacity": "0.01" };
                    SetchartCenter(options.chart.renderTo);
                    hischart = new Highcharts.Chart(options);
                }
                else if (data == "[]")
                {
                    return;
                }
                if (interval > 0) {
                    if (data != "[]") {
                        if (timerange != 0) {
                            var lineNum = options.series.length;
                            var time = timerange / 1000;
                            if ($.browser.msie || $.browser.mozilla) {
                                if (version == 9 || version == 10 || version == 11) {
                                    interval = parseInt((time / 1000) * lineNum) * 1000;
                                } else {
                                    interval = parseInt((time / 300) * lineNum) * 1000;
                                }
                            }
                            if (interval < 5000) {
                                interval = 5000;
                            }
                        }
                        componentName = cname;
                        componentParam = param;
                        NewRealTime(cname, param);
                        refreshTimer = RefreshTimer();
                    }
                    setTimeout(function () {
                        var nextDom = $("#" + cname).parent().next();
                        var trList = $("#" + $(nextDom).find("div:first").attr("id")).find("table tr");
                        if (trList.length > 0) {
                            clearInterval(tableTimer);
                            tableTimer = setInterval(function () {
                                UpdateTableData($(nextDom).find("div:first").attr("id"), $("#currentpage" + $(nextDom).find("div:first").attr("id")).val());
                            }, interval);
                        }
                    }, 300);
                } else {
                    if (data == "[]") {
                        return;
                    }
                }
            }
        });
    }
}

function SetchartCenter(chartId) {
    var width = $("#" + chartId).parent().parent().width();
    if (width > 1000) {
        width = (width - 1000) / 2;
        $("#" + chartId).parent().css("margin-left", width-20);
    }
}


function NewRealTime(cname, args) {

    globalChartName = cname;
    globalArgs = args;
    var sid = cname.split('_')[1];
    var sPara = f_conditon(args);
    var url  = "../basepage/HandlerChart.ashx?sid=" + sid + "&" + sPara + "&random=" + Math.random() * 10;
    if (chartTimer != null) {
        clearInterval(chartTimer);
    }
    new Highcharts.Chart({
        exporting: {
            enabled: false
        },
        xAxis: {
            type: 'datetime',
            tickPixelInterval: 150,
            labels: {
                overflow: 'justify',
                x: -15
            }
        },
        chart: {
            renderTo: cname,
            defaultSeriesType: 'spline',
            events: {
                load: function() {
                    var series = $("#" + cname).highcharts().series;
                    $.ajax({
                        url: url,
                        type: 'get',
                        success: function(data) {
                            if (data == "" || data == "[]") return;
                            var options = eval("(" + data + ")");
                            $("#" + cname).css("height", options.chart.height);
                            if (options.chart.type == "spline") {
                                if (options.series.length > 0) {
                                    if (series.length == 0) {
                                        options.title = getitle();
                                        options.title.style = { "margin-left": "0px" };
                                        options.title.x = -210;
                                        options.chart.marginLeft = 0;
                                        //options.xAxis.labels= {
                                        //    overflow: 'justify',
                                        //    x: -15
                                        //}
                                        options.yAxis = {
                                            title: {
                                                enabled: false
                                            },
                                            labels: {
                                                enabled: false
                                            }
                                        }
                                        options.plotOptions = {};
                                        options.plotOptions.series = {};
                                        options.plotOptions.series = {
                                            marker: {
                                                enabled: false
                                            },
                                            events: {
                                                //监听图例的点击事件
                                            legendItemClick: function (event) {
                                                // ControlLegendShow(this);
                                                //alert('click');
                                                return false;
                                            }
                                        }
                                        }
                                        options.yAxis.gridLineWidth = {};
                                        options.yAxis.gridLineWidth = 0;
                                        options.legend = {};
                                        options.legend = {
                                            align: 'right',
                                            verticalAlign: 'top',
                                            layout: 'vertical',
                                            x: -10,
                                            y: 100
                                            }
                                    }
                                    if (hischart != null) {
                                        var hismax = hischart.yAxis[0].max;
                                        var hismin = hischart.yAxis[0].min;
                                        options.yAxis.max = {};
                                        options.yAxis.max = hismax;
                                        options.yAxis.min = {};
                                        options.yAxis.min = hismin;
                                    }
                                    $("#" + cname).highcharts(options);
                                    series = $("#" + cname).highcharts().series;
                                    $.each(options.series, function(indx, item) {
                                        var id = indx;
                                        if (id != undefined) {
                                            var y = item.data[0][1];
                                            if (timerange > 0 && lastime - begintime > timerange) {
                                                series[id].addPoint([item.data[0][0], y], true, true);
                                            } else {
                                                series[id].addPoint([item.data[0][0], y], true, false);
                                            }
                                        }
                                    });
                                }
                            }
                        }
                    });
                    chartTimer = setInterval(function() {
                        var series = $("#" + cname).highcharts().series;
                        var yaxis = $("#" + cname).highcharts().yAxis;
                        url = "../basepage/HandlerChart.ashx?sid=" + sid + "&" + sPara + "&random=" + Math.random() * 10;
                        $.ajax({
                            url: url,
                            type: 'get',
                            success: function(data) {
                                if (data == "" || data == "[]") return;
                                var options = eval("(" + data + ")");
                                if (options.chart.type == "spline") {
                                    if (options.series.length > 0) {

                                        var dynamicmax = yaxis[0].dataMax;
                                        var dynamicmin = yaxis[0].dataMin;
                                        if (hischart != null) {
                                            var hismax = hischart.yAxis[0].max;
                                            var hismin = hischart.yAxis[0].min;
                                            if (dynamicmax > hismax) {
                                                hischart.yAxis[0].update({ max: dynamicmax });
                                                yaxis[0].update({ max: dynamicmax });
                                            }
                                            if (hismin > dynamicmin) {
                                                hischart.yAxis[0].update({ min: dynamicmin });
                                                yaxis[0].update({ min: dynamicmin });
                                            }
                                        }
                                        $.each(options.series, function(indx, item) {
                                            var id = indx;
                                            if (id != undefined) {
                                                var y = item.data[0][1];
                                                series[id].addPoint([item.data[0][0], y], true, false);
                                            }
                                        });
                                    }
                                }
                            }
                        });
                    }, interval);
                }
            }
        },
        title: ' '
    });
}


function RefreshTimer() {
     return  setInterval(function() {
        var currentDate = new Date();
        var minute = currentDate.getMinutes();
        var seconds = currentDate.getSeconds();
        var refreshValue = parseInt($("#c2refreshtime").combobox('getValue'));
        if (seconds == 0 && minute % refreshValue == 0) {
            f_loadingdoublechart(doubleChartId, doubleChartParam);
        }
    }, 900);
}