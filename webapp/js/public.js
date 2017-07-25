var operationTime = new Date();
setInterval(function () { CherkTime(); }, 5000);

function RefreshTime() {
    operationTime = new Date();
}

//检查窗口闲置时间
function CherkTime() {
    var total = GetDateDiff(operationTime, "minute");
    if (total >= window.LoginTimeout) {
        PageMethods.DestroySession(function() {
            window.location.href = "../default.aspx";
        });
        
    }
}

function GetDateDiff(startTime,diffType) {
    //将计算间隔类性字符转换为小写
    diffType = diffType.toLowerCase();
    var endTime = new Date(); //结束时间
    //作为除数的数字
    var divNum = 1;
    switch (diffType) {
        case "second":
            divNum = 1000;
            break;
        case "minute":
            divNum = 1000 * 60;
            break;
        case "hour":
            divNum = 1000 * 3600;
            break;
        case "day":
            divNum = 1000 * 3600 * 24;
            break;
        default:
            break;
    }
    return parseInt((endTime.getTime() - startTime.getTime()) / parseInt(divNum));
}

var screenwidth = screen.width;     //屏幕宽度
var screenheight = screen.height; //屏幕高度
var sperator = "-UT_";
function f_add() {
    var stype = document.getElementById("stype").value;
    var iw = 800;
    var ih = 350;
    //   var iSelectNode = document.getElementById("SelectNode").value
    if (stype == "UserManagement" && appname=="urptfrm") {
        iw = 800;
        ih = 695;
    }
    else if (stype == "UserManagement")
    {
        iw = 800;
        if (appname == "joyoc2") {
            ih = 520;
        } else {
            ih = 520;
        }

    }
    else if (stype == "RoleManagement") {
        iw = 800;
        ih = 210;
    }
    else if (stype == "ExtendData") {
        iw = 800;
        ih = 203;
    }
    else if (stype == "AppManagement") {
        iw = 800;
        ih = 150;
    }
    else if (stype == "IuserManagement") {
        iw = 800;
        ih = 205;
    }
    var sUrl = "../basepage/GetAddHtml.ashx?type=" + stype+"&random="+Math.random()*10;// + "&C_MP_ID=" + iSelectNode;

    $('#ModalWindow').window({
        width: iw,
        height: ih+10,
        top:60
    });

    $('#ModalWindow').window('open');
    $.ajax({
        url: sUrl,
        type: 'get',
        success: function (data) {
            $('#ModalWindow').html(data);
            var combo = $('#ModalWindow').find('.easyui-combobox');
            $.parser.parse(combo.parent());
        }
    });
}

function f_modify() {
    var nameArray = new Array();
    var valueArray = new Array();
    var ComponentId = document.getElementById('ComponentId').value;
    var CurrentId = document.getElementById('listframe').contentWindow.document.getElementById('CurrentId').value;
    var tbinfo = $(window.frames["listframe"].document).find("#" + ComponentId);
    var EditWindow = $(window.frames["listframe"].document).find("#EditWindow");
    var firstRow = $(tbinfo).find("tr:first td");
    $.each(firstRow, function (i, n) {
        nameArray.push($(n).attr("sqlId"));
    });

    var dataRow = $(tbinfo).find("tr:eq(" + CurrentId + ") td");
    $.each(dataRow, function (i, n) {
        valueArray.push($(n).text());
    });

    var stype = document.getElementById("stype").value;
    var namestr = "";
    for (var i = 0; i < nameArray.length; i++) {
        namestr += nameArray[i] + sperator.toLowerCase();
    }
    var valuestr = "";
    for (var i = 0; i < valueArray.length; i++) {
        valuestr += valueArray[i] + sperator.toLowerCase();
    }
    var sUrl = "../Public/UpdateRecord.aspx?type=" + stype + "&name=" + namestr + "&value=" + valuestr;
    utf_openshowwin(sUrl, iw, ih, "no");
}
function f_delete() {
    var nameArray = new Array();
    var valueArray = new Array();

    var ComponentId = document.getElementById('ComponentId').value;
    var CurrentId = document.getElementById('CurrentId').value;
    var tbinfo = $("#" + ComponentId);
    var firstRow = $(tbinfo).find("tr:first td");
    $.each(firstRow, function (i, n) {
        nameArray.push($(n).attr("sqlId"));
    });

    var run = true;
    var dataRow = $(tbinfo).find("tr:eq(" + CurrentId + ") td");
    $.each(dataRow, function (i, n) {
        if (i == 0 && $(n).text() == "") {
            alert("请选择正确的记录！");
            run = false;
        }
        if ($(n).text().length > 50) {
            valueArray.push("");
        }
        else {
            valueArray.push($(n).text());
        }
    });
    if (!run) {
        return;
    }
    if (CurrentId == "") {
        alert("请选择一条数据");
        return;
    }
    var smsg = "是否确定删除？";

    if (confirm(smsg)) {
        PageMethods.DelData(ComponentId, nameArray, valueArray, function (result) {

            if (result == 1) {
                alert("删除成功!");
                f_reloading();///刷新树
                f_query();
                if (document.getElementById("stype").value == "v_user" || document.getElementById("stype").value == "v_role")
                    $.ajax({
                        url: '../basepase/Init.aspx',
                        success: function () { }
                    });
            }
            else if (result == -3) {
                alert('连接用户权限服务失败 ');
                window.returnValue = 'false';
            }
            else if (result == -2) {
                alert('用户被锁定 ');
                window.returnValue = 'false';
            }
            else if (result == -1) {
                alert('用户未激活 ');
                window.returnValue = 'false';
            }
            else if (result == 0) {
                alert('用户名不存在 ');
                window.returnValue = 'false';
            }
            else if (result == 2) {
                alert('密码错误 ');
                window.returnValue = 'false';
            }
            else if (result == 3) {
                alert('会话超时 ');
                window.returnValue = 'false';
            }
            else if (result == 4) {
                alert('没有权限 ');
                window.returnValue = 'false';
            }
            else if (result == 5) {
                alert('用户名已存在 ');
                window.returnValue = 'false';
            }
            else if (result == 6) {
                alert('角色名已存在 ');
                window.returnValue = 'false';
            }
            else if (result == 7) {
                alert('修改失败 ');
                window.returnValue = 'false';
            }
            else if (result == 8) {
                alert('删除的角色不存在或者改角色下存在相关用户 ');
                window.returnValue = 'false';
            }
            else if (result == 9) {
                alert('不能删除自己 ');
                window.returnValue = 'false';
            }
            else if (result == 10) {
                alert('删除失败，存在关联数据 ');
                window.returnValue = 'false';
            }
            else if (result == 11) {
                alert('删除失败 ');
                window.returnValue = 'false';
            }
            else if (result == 12) {
                alert('项目已存在 ');
                window.returnValue = 'false';
            }
            else if (result == 16) {
                alert('添加的记录已存在 ');
                window.returnValue = 'false';
            }
            else if (result == 19) {
                alert('导入完成，已存在的用户无法导入 ');
            }
            else if (result == 48) {
                alert('输入内用包含关键字 ');
                window.returnValue = 'false';
            }
            else if (result == 99) {
                alert('该用户为系统默认管理员,不能进行删除！');
                window.returnValue = 'false';
            }
            else {
                alert("删除失败!");
            }
        });
    }
}

//发布
function f_FB() {
    var KeyFieldName = document.getElementById("KeyFieldName").value;
    var CurrentId = document.getElementById("CurrentId").value;
    if (CurrentId == "") {
        alert("请选择一条数据");
        return;
    }
    var sWhere = KeyFieldName + "='" + CurrentId + "'";
    if (confirm("是否确定发布？")) {
        PageMethods.FBData(document.getElementById("stype").value, sWhere, CurrentId, function (result) {

            if (result == "True") {
                alert("发布成功!");
                f_load();
            }
            else {
                alert("发布失败!" + result);
            }
        });
    }
}
//js获取url参数的function
function request(paras) {
    var url = location.href;
    var paraString = url.substring(url.indexOf("?") + 1, url.length).split("&");
    var paraObj = {}
    for (i = 0; j = paraString[i]; i++) {
        paraObj[j.substring(0, j.indexOf("=")).toLowerCase()] = j.substring(j.indexOf
("=") + 1, j.length);
    }
    var returnValue = paraObj[paras.toLowerCase()];
    if (typeof (returnValue) == "undefined") {
        return "";
    } else {
        return returnValue;
    }
}
var theurl
theurl = request("url");
if (theurl != '') {
    location = theurl
}

//0:日期加时间
//1:日期
//2:时间
function utf_calendar(current, mode) {
    var transcal
    var cdvalue
    //  mode = 1;
    //   var s = request('type');
    //  if (s == 'ElectricQuantityAnalysis') {
    //      mode = 0;
    //   }
    if (navigator.appName == "Microsoft Internet Explorer")
        transcal = window.showModalDialog("../basepage/UTcalendar.aspx?cdvalue=" + current + "&mode=" + mode, window, "dialogheight:21.0em;dialogwidth:26.0em;status:no;help:no");
    else
        transcal = utf_openshowwin("../basepage/UTcalendar.aspx?cdvalue=" + current + "&mode=" + mode, 420, 340, "no");
}

function utf_date(current) {
    mode = 0;
    if (navigator.appName == "Microsoft Internet Explorer")
        transcal = window.showModalDialog("../basepage/UTDate.aspx?cdvalue=" + current + "&mode=" + mode, window, "dialogheight:21.0em;dialogwidth:15.0em;status:no;help:no");
    else
        transcal = utf_openshowwin("../basepage/UTDate.aspx?cdvalue=" + current + "&mode=" + mode, 420, 340, "no");
}

function utf_time(current) {
    mode = 1;
    if (navigator.appName == "Microsoft Internet Explorer")
        transcal = window.showModalDialog("../basepage/UTTime.aspx?cdvalue=" + current + "&mode=" + mode, window, "dialogheight:21.0em;dialogwidth:12.0em;status:no;help:no");
    else
        transcal = utf_openshowwin("../basepage/UTTime.aspx?cdvalue=" + current + "&mode=" + mode, 420, 340, "no");
}

function f_query() {
    //if (document.frames["contents"]) {
    //    document.frames["contents"].document.location.reload();
    //}
   // f_reloading();///刷新树
   //  f_load();
    //getCondition("");
    //sPara = f_conditon("");
    initLoadDataTable = false;
    timerTableRowId = 0;
    f_loadlayout();
    if ($("div[id*=_splinechart]").length == 2) {
        f_loadingdoublechart(doubleChartId, doubleChartParam);
    }
}
function utf_openshowwin(sUrl, iWidth, iHeight, sResize) {

    //如果可调整大小
    if (sResize == "yes") {
        iWidth = screenwidth / 800 * iWidth
        iHeight = screenheight / 600 * iHeight
        l = (screenwidth - iWidth) / 2 - 5
        t = (screenheight - iHeight) / 2 - 20
    }
    else {
        l = (screen.width - iWidth) / 2 - 5
        t = (screen.height - iHeight) / 2 - 20
        sResize = "no"
    }
    w_show = window.open(sUrl, "", "resizable=" + sResize + ",scrollbars=no,menubar=no,location=no,status=no,toolbar=no,width=" + iWidth + ",height=" + iHeight + ",left=" + l + ",top=" + t);
    w_show.document.write("<input type='hidden' id='svalue' value='123123' />");
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
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }

    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
}

//保存重查询面板获取的条件
var sPara = "";
function f_conditon(args) {
    sPara = "";
    if (args != "") {
        sPara = args + ";";
    }
    if (document.getElementById("TreeNodeText")) {
        sPara += "SelectNode=" + document.getElementById("SelectNode").value + ";"; 
        sPara += "SelectNodeName=" + $("#SelectNodeName").val() + ";"; 
        sPara += "TreeNodeText=" + $("#TreeNodeText").val() + ";"; 
        sPara += "SelectNodeType=" + $("#SelectNodeType").val() + ";"; 
        if ($("#SelectNodeType").val() != "") {
            sPara += $("#SelectNodeType").val() + "=" + $("#SelectNode").val() + ";";
        }
        sPara += "SiteAddress=" + $(document.getElementById("SiteAddress")).val() + ";"; 
        sPara += getTreeNodevalue(document.getElementById("nodepath").value);

    }

    //普通条件控件
    var tbCondition = document.getElementsByName("ConditionName");
    if (tbCondition != "undefined") {
        for (i = 0; i < tbCondition.length; i++)//查询条件input
        {
            if (tbCondition[i].style.display != "none") {
                if (sPara != "")
                    sPara += "&";
             
                var vid = tbCondition[i].id;
                if (vid == "") { //easyui combobox 
                     vid = tbCondition[i].parentNode.parentNode.children[0].getAttribute("vid");
                }

                sPara += vid+ "=" + tbCondition[i].value;
            }
        }
    }

    tbCondition = $("[name*='ConditionNameDatetime_']");
    if (tbCondition != "undefined") {
        //var tNames = tbCondition.getElementsByName("ConditionName");
        for (i = 0; i < tbCondition.length; i++)//查询条件input
        {
            if (tbCondition[i].style.display != "none") {
                if (sPara != "") {
                    sPara += "&";
                }
                var dTime = new Date(tbCondition[i].value.replace(/-/ig, '/'));
                //var s = dTime.format("yyyyMMddHHmmss") + paddingLeft(dTime.getMilliseconds(), 4);
                var s = dTime.getFullYear()
                    + paddingLeft(dTime.getMonth()+1, 2)
                    + paddingLeft(dTime.getDate(), 2)
                    + paddingLeft(dTime.getHours(), 2)
                    + paddingLeft(dTime.getMinutes(), 2)
                    + paddingLeft(dTime.getSeconds(),2)
                    + paddingLeft(dTime.getMilliseconds(), 4);
                if (s.indexOf("NaN") > -1) {
                    s = tbCondition[i].value;
                }
                sPara += $(tbCondition[i]).attr("name").split('_')[1] + "=" + s;
            }
        }
    }

    tbCondition = $("[name*='ConditionNameDate_']");
    if (tbCondition != "undefined") {
        for (i = 0; i < tbCondition.length; i++)//查询条件input
        {
            if (tbCondition[i].style.display != "none") {
                if (sPara != "")
                    sPara += "&";
                var dtValue = tbCondition[i].value;
                if (dtValue.split('-').length == 2) {
                    dtValue = dtValue + "-01";
                }
                var dTime = new Date(dtValue.replace(/-/ig, '/'));
              //  var s = dTime.format("yyyyMMdd0000000000");
                var s="";
                if ($(tbCondition[i]).attr("name").split('_')[1].toLowerCase().indexOf('end') > -1) {
                    s=dTime.format("yyyyMMdd23595900000")
                } else {
                    s = dTime.format("yyyyMMdd0000000000");
                }
                sPara += $(tbCondition[i]).attr("name").split('_')[1] + "=" + s;
            }
        }
    }

    tbCondition = document.getElementsByName("ConditionNameTime");
    if (tbCondition != "undefined") {
        for (i = 0; i < tbCondition.length; i++)//查询条件input
        {
            if (tbCondition[i].style.display != "none") {
                if (sPara != "")
                    sPara += "&";
                var dtNow = new Date();
                var s1 = dtNow.format("yyyy-MM-dd ") + tbCondition[i].value;
                var dTime = new Date(s1.replace(/-/ig, '/'));
                var s = dTime.format("yyyyMMddhhmmss") + paddingLeft(dTime.getMilliseconds(), 4);
                sPara += tbCondition[i].id + "=" + s;
            }
        }
    }

    tbCondition = document.getElementsByName("ConditionNameYear");
    if (tbCondition != "undefined") {
        for (i = 0; i < tbCondition.length; i++)//查询条件input
        {
            if (tbCondition[i].style.display != "none") {
                if (sPara != "")
                    sPara += "&";
                var dTime = new Date(tbCondition[i].value, 1, 1, 0, 0, 0, 0);
                var s = dTime.format("yyyyMMddhhmmss") + paddingLeft(dTime.getMilliseconds(), 4);
                sPara += tbCondition[i].id + "=" + s;
            }
        }
    }

    tbCondition = document.getElementsByName("ConditionNameCheckbox");
    if (tbCondition != "undefined") {
        //var tNames = tbCondition.getElementsByName("ConditionName");
        for (i = 0; i < tbCondition.length; i++)//查询条件input
        {
            if (tbCondition[i].style.display != "none") {
                var sChecks = "";
                var cbCondition = document.getElementsByName("ConditionNameCheckbox" + tbCondition[i].id);
                if (cbCondition != "undefined") {
                    for (j = 0; j < cbCondition.length; j++)//查询条件input
                    {
                        if (cbCondition[j].checked) {
                            if (sChecks != "")
                                sChecks += "|";
                            sChecks += cbCondition[j].value;
                        }
                    }
                }
                if (sPara != "")
                    sPara += "&";
                sPara += tbCondition[i].id + "=" + sChecks;
            }
        }
    }

    tbCondition = document.getElementsByName("ConditionNameRadio");
    if (tbCondition != "undefined") {
        //var tNames = tbCondition.getElementsByName("ConditionName");
        for (i = 0; i < tbCondition.length; i++)//查询条件input
        {
            if (tbCondition[i].style.display != "none") {
                var sChecks = "";
                var cbCondition = document.getElementsByName("ConditionNameRadio" + tbCondition[i].id);
                if (cbCondition != "undefined") {
                    for (j = 0; j < cbCondition.length; j++)//查询条件input
                    {
                        if (cbCondition[j].checked) {
                            sChecks = cbCondition[j].value;
                            break;
                        }
                    }
                }
                if (sPara != "")
                    sPara += "&";
                sPara += tbCondition[i].id + "=" + sChecks;
            }
        }
    }
    //年月条件控件
    var tbConditionYear = document.getElementsByName("ConditionNameYearMonth");
    var tbConditionMonth = document.getElementsByName("ConditionNameMonth");
    if (tbConditionYear != "undefined" && tbConditionMonth != "undefined" && tbConditionYear.length > 0 && tbConditionMonth.length > 0) {
        //var tNames = tbCondition.getElementsByName("ConditionName");

        if (tbConditionYear.length > 0 && tbConditionMonth.length) {
            if (tbConditionYear[0].style.display != "none" && tbConditionMonth[0].style.display != "none") {
                if (sPara != "")
                    sPara += "&";
                var dTime = new Date(tbConditionYear[i].value, tbConditionMonth[i].value, 1, 0, 0, 0, 0);
                var s = dTime.format("yyyyMMddhhmmss") + paddingLeft(dTime.getMilliseconds(), 4);
                sPara += tbConditionMonth[i].id.replace("month", "") + "=" + s;
            }
        }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    ///table grid


    var gridCondition = document.getElementsByName("ConditionNameGrid");
    gridCondition = document.getElementsByName("ConditionNametable");
    gridCondition = $("table[name=ConditionNameGrid]")
    if (gridCondition != "undefined" && gridCondition.length > 0) {
        gridCondition.each(function (idx, item) {
            var name = item.id;
            var row = $(item).datagrid('getSelected');
            if (sPara != "")
                sPara += "&";
            if(row&&row.name)
                sPara += item.id + "=" + encodeURIComponent(row.name);
        })
    }

    var iSumPage = document.getElementById("SumPage");
    if (iSumPage != null) {
        iSumPage = parseInt(iSumPage.value)
    } else {
        iSumPage = 1;
    }
    // Mod @ 2012-11-01
    var iSurrentPage = 1;
    if (document.getElementById("currentpage")) {
        iSurrentPage = parseInt(document.getElementById("currentpage").value);
    }

    if (document.getElementById("btnQuery") == undefined) {
     //   sPara = "";
    }

    if (parseInt(iSurrentPage) <= parseInt(iSumPage)) {
        //f_changeyear();
       // f_getdatebyyearmonth();
        return sPara;
        //  document.getElementById("ToExcel").value = "0";
        //form1.action = "../basepage/Main.aspx?" + sPara;
        //if (document.getElementById("listframe") != null) {
        //    form1.target = "listframe";

        //}
        //else
        //    if (document.getElementById("picframe") != null) {
        //        form1.target = "picframe";

        //    }
        //   form1.submit();
    } else {
        return null;
    }
}

//function f_loadchart(ctrlIdDesc, args) {
//   
//    f_conditon(args);
////    PageMethods.getChartData(ctrlIdDesc, sPara, function (result) {
////        var c = document.getElementById(ctrlIdDesc);
////        alert(c)
//    //    })
//    var sid=ctrlIdDesc.split('_')[1];
//     var url = "../basepage/HandlerChart.ashx?sid="+sid+"&"+sPara;
//     $.ajax({
//         url: url,
//         type: 'get',
//         success: function (data) {
//             if (data == "") return;
//             var options = eval("(" + data + ")");
//             if (options.series.length > 0) {
//                 $('#' + ctrlIdDesc).highcharts(options)
//             }

//         }
//     })

////    var opt = {
////        series: [{ "name": "1 ", "data": [100, 101, 102, 99, 98, 106, 100, 109, 89, 97]}]

////    };


//  
//}
var freshData;

function f_loadpage(ctrlIdDesc, currentpage, args) {
    if (ctrlIdDesc.indexOf("_38_") > 0) {
        var win = $.messager.progress({
            title: '正在从服务端获取数据中...',
            msg: ''
        });
    }
    var sPara = f_conditon(args);
    var totalnum = $("#table" + ctrlIdDesc + "total").val() == undefined ? "" : $("#table" + ctrlIdDesc + "total").val();
    PageMethods.getTable(ctrlIdDesc, currentpage, sPara, totalnum, function(result) {
        if (ctrlIdDesc.indexOf("_38_") > 0) {
            $.messager.progress('close');
        }
        var c = document.getElementById(ctrlIdDesc);
        var v = $("#" + ctrlIdDesc);
        c = $("#" + ctrlIdDesc).html();
        v.html(result);
        var h = $(window).height();
        var tr = $("#" + ctrlIdDesc + " table tr");
        if (tr.length > 1) {
            if (tr[1] && tr[1].firstChild && tr[1].firstChild.firstChild && tr[1].firstChild.firstChild.type == "checkbox") {
            } else
                $(tr[1]).trigger("click");
        }
        f_getChecked(ctrlIdDesc);
        var pid = "component_8";
        setTimeout(function () {
            $(document.getElementById(ctrlIdDesc).parentNode.parentNode).panel('resize');
            $('#weblayout').layout('resize');
            $('#weblayout').layout('panel', 'center').panel('resize', { width: $('#layoutcenter').width() + 1 });
            getotherheight(ctrlIdDesc);
        }, 500);
        var nodepath = $("#nodepath").val();
        if (nodepath != null && appname == "joyoaffzpt") {
            if (nodepath.indexOf("1(FZPTSvr.syspowerdatatype)") > -1 || nodepath.indexOf("3(FZPTSvr.syspowerdatatype)") > -1) {

            } else {
                var trlist = $("#component_47_querydatatable").find("table tr");
                $.each(trlist, function(index, item) {
                    $.each($(item).find("td"), function(ind, td) {
                        if (ind == 0) {
                            $(td).hide();
                        }
                    });
                });
            }
        } else if (appname == "joyoc2") {
            if (nodepath!=undefined && (nodepath.indexOf("2(C2Svr.syspowerdatatype)") > -1 || nodepath.indexOf("5(C2Svr.syspowerdatatype)") > -1)) {
                var trlist = $("#component_47_querydatatable").find("table tr");
                $.each(trlist, function(index, item) {
                    $.each($(item).find("td"), function(ind, td) {
                        if (ind == 0) {
                            $(td).hide();
                        }
                    });
                });
            }
        }

        PageMethods.getPageID(ctrlIdDesc, function (result) {
            pid = result.split('_')[1];
            if (pid == "") return;
            var pagesum = $("#tablepagesum" + pid).val();
            var sp = $("#" + result + " input[id^=SumPage]");
            var psize = $("#" + ctrlIdDesc + " input[id^=rowcount]").val();
            sp.val(pagesum);
            sp = $("#" + result + " span[id^=spansumpage]");
            sp.html(pagesum);
            pid = $("#" + result + " input[id=currentpage" + ctrlIdDesc + "]").val();
            if (parseInt(pid) > parseInt(pagesum)) {
                pid = pagesum;
            }
            if (pid == undefined) return;

            var cid = $(sp).attr("id").substring("spanSumPage".length);
            f_pageCheck(cid, pid);
        });

    });

}

//function getParentFrameDocument(fname) {
//    var fdoc = "";//document.frames[fname] || document.getElementById(fname);

//    if (document.all) {//IE  
//        fdoc = document.frames[fname].document;
//    } else {//Firefox or Chrome   
//        fdoc = document.getElementById(fname).contentDocument;
//    }

//  //  var sv = fdoc.getElementById("SelectNode").value;
//  //  var tv = $(fdoc).find("#TreeNodeText").val()
//    return fdoc;
//}

function getFrameDocument(fname) {
    var fdoc = "";//document.frames[fname] || document.getElementById(fname);

    if (document.all) {//IE  
        fdoc = document.frames[fname].document;
    } else {//Firefox or Chrome   
        fdoc = document.getElementById(fname).contentDocument;
    }

    //  var sv = fdoc.getElementById("SelectNode").value;
    //  var tv = $(fdoc).find("#TreeNodeText").val()
    return fdoc;
}


function f_load() {
    //新的框架查询条件通过url传递
    sPara = f_conditon("");
   
 

    if (sPara != null) {
        f_changeyear();
        f_getdatebyyearmonth();
        // document.getElementById("ToExcel").value = "0";
        form1.action = "../public/Query.aspx?" + sPara;
        if (document.getElementById("listframe") != null) {
            form1.target = "listframe";

        }
        else
            if (document.getElementById("picframe") != null) {
                form1.target = "picframe";

            }
        form1.submit();
    }
}


var ctop ="";

function f_imghide(obj,style) {
    //getFrameDocument("myframe")
    var filtertr = document.getElementById("filtertr");
    if (obj.src.indexOf("朝上隐藏.gif") > 0) {
        obj.src = '../images/'+style+'/朝下伸展.gif';
        obj.title = '显示查询区域';
        filtertr.style.display = "none";
        if ($("#component_47_querydatatable").length > 0) {
            if (ctop == "" && $("#component_142_splinechart").css("display") != "block") {
                ctop = $("#component_47_querydatatable").parent().css("top");
            }
            if ($("#component_142_splinechart").css("display") != "block") {
                $("#component_47_querydatatable").parent().css("top", "0px");
            }
        }
    }
    else {
        obj.src = '../images/'+style+'/朝上隐藏.gif';
        obj.title = '隐藏查询区域';
        filtertr.style.display = "";
        if ($("#component_47_querydatatable").length > 0) {
            if ($("#component_142_splinechart").css("display") != "block") {
                ctop = "40px";
                $("#component_47_querydatatable").parent().css("top", ctop);
            }
            
        }
    }
    
    $(window).resize();
}


function request(paras) {
    var url = location.href;
    var paraString = url.substring(url.indexOf("?") + 1, url.length).split("&");
    var paraObj = {}
    for (i = 0; j = paraString[i]; i++) {
        paraObj[j.substring(0, j.indexOf("=")).toLowerCase()] = j.substring(j.indexOf
("=") + 1, j.length);
    }
    var returnValue = paraObj[paras.toLowerCase()];
    if (typeof (returnValue) == "undefined") {
        return "";
    } else {
        return returnValue;
    }
}
var theurl
theurl = request("url");
if (theurl != '') {
    location = theurl
}

function GetParm(ps, p) {
    var paraString = ps.split("&");
    var paraObj = {}
    for (i = 0; i < paraString.length; i++) {
        s = paraString[i];
        if (s.substring(0, s.indexOf("=")).toLowerCase() == p)
            return s.substring(s.indexOf("=") + 1, s.length);
    }
    return "";
}

function f_flash(w, h, username, pwd, parm, objiframe) {
    //alert(parm)
    if (objiframe != null) {
        var s = request('type');
        //alert(s);
        var url;
        var s1 = parm.toString();
        var k = GetParm(parm, "k");
        if (k == "h") {
            if (s1.indexOf("&p1") > 0)
                url = "../flex/FalshH1.html?w=" + w / 2 + "&h=" + h + "&a=" + username + "&b=" + pwd + "&p=" + parm;
        }
        else if (k == "v") {
            if (s1.indexOf("&p1") > 0)
                url = "../flex/FalshV1.html?w=" + w + "&h=" + h + "&a=" + username + "&b=" + pwd + "&p=" + parm;
            else if (s1.indexOf("&p2") > 0)
                url = "../flex/FalshV2.html?w=" + w + "&h=" + h + "&a=" + username + "&b=" + pwd + "&p=" + parm;
        }
        else
            url = "../flex/RData.html?w=" + w + "&h=" + h + "&a=" + username + "&b=" + pwd + "&p=" + parm;
        //url += "&type="+s;
        objiframe.src = url;

    }
}

function f_flashView(w, h, parm) {
    //alert(parm)
    var username = document.getElementById("username").value;
    var pwd = document.getElementById("pwd").value;
    var s = request('type');
    //alert(s);
    var url;
    var s1 = parm.toString();
    var k = GetParm(parm, "k");
    if (k == "h") {
        if (s1.indexOf("&p1") > 0)
            url = "../flex/FalshH1.html?w=" + w / 2 + "&h=" + h + "&a=" + username + "&b=" + pwd + "&p=" + parm;
    }
    else if (k == "v") {
        if (s1.indexOf("&p1") > 0)
            url = "../flex/FalshV1.html?w=" + w + "&h=" + h + "&a=" + username + "&b=" + pwd + "&p=" + parm;
        else if (s1.indexOf("&p2") > 0)
            url = "../flex/FalshV2.html?w=" + w + "&h=" + h + "&a=" + username + "&b=" + pwd + "&p=" + parm;
    }
    else
        url = "../flex/RData.html?w=" + w + "&h=" + h + "&a=" + username + "&b=" + pwd + "&p=" + parm;
    //url += "&type="+s;
    window.showModalDialog(url, window, "dialogheight:40em;dialogwidth:60em;status:no;help:no");
    //window.open(url);
}

//function f_getdatebyyearmonth() {
//    var obj = document.getElementById('cdtPeriod');
//    if (obj != null) {
//        if (obj.value == "峰谷平电量")
//            return;
//        if (obj.value == "按日同比" || obj.value == "日"
//        || obj.value == "峰平谷电量" || obj.value == "峰谷平电量" || obj.value == "时刻电量")
//            return;
//    }
//    var ObjMonth = document.getElementById("sMonth");
//    var ObjYear = document.getElementById("sYear");
//    if (ObjYear == null)
//        return;

//    if (ObjMonth != null) {
//        if (ObjMonth.value != "") {
//            document.getElementById("dtStart").value = ObjYear.value + "-" + ObjMonth.value + "-01"
//        }
//    }
//    else {
//        if (document.getElementById("dtStart") != null) document.getElementById("dtStart").value = ObjYear.value + "-01-01";
//    }
//    var ObjMonthEnd = document.getElementById("sMonthEnd");
//    var ObjYearEnd = document.getElementById("sYearEnd");

//    if (ObjYearEnd != null && ObjYearEnd.style.display != "none") {
//        if (document.getElementById("dtEnd") != null) {
//            if (ObjMonthEnd != null) {
//                if (ObjMonthEnd.value != "") {
//                    document.getElementById("dtEnd").value = ObjYearEnd.value + "-" + ObjMonthEnd.value + "-01"
//                }
//            }
//            else {
//                if (document.getElementById("dtEnd") != null) document.getElementById("dtEnd").value = ObjYear.value + "-01-01";
//            }
//        }
//    }
//    else {
//        if (document.getElementById("dtEnd") != null)
//            document.getElementById("dtEnd").value = document.getElementById("dtStart").value;
//    }
//}

//function f_changeyear() {
//    var obj = document.getElementById('cdtPeriod');
//    if (obj != null) {
//        if (obj.value == "按日同比" || obj.value == "日"
//        || obj.value == "峰平谷电量" || obj.value == "峰谷平电量" || obj.value == "时刻电量" || obj.value == "月电量")
//            return;
//    }
//    var ObjMonth = document.getElementById("sMonth");
//    var ObjYear = document.getElementById("sYear");
//    if (ObjYear == null)
//        return;


//    //  alert(ObjMonth.value)
//    if (ObjMonth != null) {
//        if (ObjMonth.value != "") {
//            document.getElementById("dtStart").value = ObjYear.value + "-" + ObjMonth.value + "-01"
//        }
//    }
//    else {
//        if (document.getElementById("dtStart") != null) document.getElementById("dtStart").value = ObjYear.value + "-01-01";
//    }

//    var ObjMonthEnd = document.getElementById("sMonthEnd");
//    var ObjYearEnd = document.getElementById("sYearEnd");
//    if (ObjMonthEnd != null) {
//        if (ObjMonthEnd.value != "") {
//            document.getElementById("dtEnd").value = ObjYearEnd.value + "-" + ObjMonthEnd.value + "-01"
//        }
//    }
//    else {
//        if (document.getElementById("dtEnd") != null) document.getElementById("dtEnd").value = ObjYear.value + "-01-01";
//    }

//}





//预览
function f_Preview() {
    var CurrentId = document.getElementById("CurrentId").value;
    var surl = "http://isdemo/website/website/InfoShow.aspx?page=trdmsnews&id=" + CurrentId + "&sTitle=";
    window.open(surl);
}

/**      
* 对Date的扩展，将 Date 转化为指定格式的String      
* 月(M)、日(d)、12小时(h)、24小时(H)、分(m)、秒(s)、周(E)、季度(q) 可以用 1-2 个占位符      
* 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字)      
* eg:      
* (new Date()).pattern("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423      
* (new Date()).pattern("yyyy-MM-dd E HH:mm:ss") ==> 2009-03-10 二 20:09:04      
* (new Date()).pattern("yyyy-MM-dd EE hh:mm:ss") ==> 2009-03-10 周二 08:09:04      
* (new Date()).pattern("yyyy-MM-dd EEE hh:mm:ss") ==> 2009-03-10 星期二 08:09:04      
* (new Date()).pattern("yyyy-M-d h:m:s.S") ==> 2006-7-2 8:9:4.18      
*/
Date.prototype.format = function (format) //author: meizz   
{
    var o = {
        "M+": this.getMonth() + 1, //month   
        "d+": this.getDate(),    //day   
        "h+": this.getHours(),   //hour   
        "m+": this.getMinutes(), //minute   
        "s+": this.getSeconds(), //second   
        "q+": Math.floor((this.getMonth() + 3) / 3),  //quarter   
        "S": this.getMilliseconds() //millisecond   
    }
    if (/(y+)/.test(format))
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o) if (new RegExp("(" + k + ")").test(format)) {
        format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
    }
    return format;
}




function f_lookall(sPS) {
    var stype = document.getElementById("stype").value;
    var sUrl = "../public/List.aspx?ps=" + sPS + "&stype=" + stype + "all";
    var v1 = document.getElementById("listframe");
    //window.open(sUrl);
    window.open(sUrl, "", "resizable=no,scrollbars=no,menubar=no,location=no,status=no,toolbar=no,width=1100,height=700,left=100,top=100")
    // window.showModalDialog(sUrl, window, "dialogheight:31.0em;dialogwidth:56.0em;status:no;help:no");
}

function f_SelectType(fieldName, stable, obj) {
    if (stable.toString() == "t_shirtpeak_target" || stable.toString() == "t_shirtpeak_warning") {
        PageMethods.GetTypeData(obj.value, function (result) {

            var obj = document.getElementById('selectC_OBJID');
            var length = obj.length;
            for (var j = length - 1; j >= 0; j--) {
                obj.options.remove(j);
            }
            var arr1 = result.split("|");
            for (var i = 0; i < arr1.length; i++) {
                if (arr1[i].length > 0) {
                    var arr2 = arr1[i].split(":");
                    var varitem = new Option(arr2[1], arr2[0]);
                    obj.options.add(varitem);
                }
            }


        });
    }


    else if (stable.toString() == "T_MPAlarmInfoSetting") {
        PageMethods.GetMPAlarmInfoSettingDataType(obj.value, function (result) {

            var obj = document.getElementById('selectc_smalltype');
            var length = obj.length;
            for (var j = length - 1; j >= 0; j--) {
                obj.options.remove(j);
            }
            var arr1 = result.split("|");
            for (var i = 0; i < arr1.length; i++) {
                if (arr1[i].length > 0) {
                    var arr2 = arr1[i].split(":");
                    var varitem = new Option(arr2[1], arr2[0]);
                    obj.options.add(varitem);
                }
            }
        });
    }
}



function changeselected(ConditionName) {
    var it = $("#" + ConditionName).find("option").text()
    if (it == "所有" || $.trim(it) == "") {
        PageMethods.getConditions(ConditionName, function (result) {

            $("#" + ConditionName).html(result);//"<option   value=''>   </option>" +
        });
    }

}

function new_modify() {
    var nameArray = new Array();
    var valueArray = new Array();
    var ComponentId = document.getElementById('ComponentId').value;
    var CurrentId = document.getElementById('CurrentId').value;
    if (CurrentId == "") {
        return;
    }
    var tbinfo = $("#" + ComponentId);
    var EditWindow = $("#EditWindow");
    var firstRow = $(tbinfo).find("tr:first td");
    $.each(firstRow, function (i, n) {
        nameArray.push($(n).attr("sqlId"));
    });
    var run = true;
    var dataRow = $(tbinfo).find("tr:eq(" + CurrentId + ") td");
    $.each(dataRow, function (i, n) {
        if (i == 0 && $(n).text() == "") {
            alert("请选择正确的记录！");
            run = false;
        }
        valueArray.push($(n).text());
    });
    if (!run)
        return;
    var iw = 800;
    var ih = 700;

    var stype = document.getElementById("stype").value;
    if (stype == "UserManagement" && appname == "urptfrm") {
        iw = 800;
        ih = 695;
    }
    else if (stype == "UserManagement") {
        iw = 800;
        ih = 520;
    }
    else if (stype == "RoleManagement") {
        iw = 800;
        ih = 210;
    }
    else if (stype == "AppManagement") {
        iw = 800;
        ih = 150;
    }
    var namestr = "";
    for (var i = 0; i < nameArray.length; i++) {
        namestr += nameArray[i] + sperator.toLowerCase();
    }
    var valuestr = "";
    for (var i = 0; i < valueArray.length; i++) {
        valuestr += valueArray[i] + sperator.toLowerCase();
    }

    $('#ModalWindow').window({
        width: iw,
        height: ih+10,
        top:60
    });
    $('#ModalWindow').window('open');
    var sUrl = "../basepage/GetUpdateHtml.ashx?random="+Math.random()*10;
    $.ajax({
        url: sUrl,
        type: 'post',
        data: { 'type': stype, 'name': namestr, 'value':valuestr },
        success: function (data) {
            $('#ModalWindow').html(data);
            var combo = $('#ModalWindow').find('.easyui-combobox');
            $.parser.parse(combo.parent());
        }
    });
}



///放大缩小

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

function enterquery() {
    if (event.keyCode == 13) {
        f_query();
    }
}

///在main.aspx 测试加载 可以
function f_reloading() {
    //if (parent.document.frames["contents"]) {
    //    parent.document.frames["contents"].document.location.reload();
    //}

    var dc = $("div[id^=component_]");
    dc.each(function (idx, item) {
        var split = this.id.split('_');
        var cid = split[1];
        var ctype = split[2];
        if (ctype == 'treeview') {
            f_loadtree("ul_" + cid)
        }
    });
}


function customContextMenu(event) {
    event.preventDefault ? event.preventDefault() : (event.returnValue = false);

}

//function changepagesize(tableid) {
//    if (event.keyCode == 13) {
//        var tid = "tablepagesize" + tableid;
//        var tval = document.getElementById(tid).value;
//        tid = tableid.indexOf("_") + 1;
//        tid = tableid.substring(tid);
//        PageMethods.setPagesize(tid, tval, function () {


//            cp = document.getElementById("currentpage" + tableid).value;
//            f_pageCheck(tableid, cp);
//            f_loadpage(tableid, cp, "");
//        })
//        return false;
//    }
//}


function changeonimg(id) {
    //  $(id).css({ "background-image": "url(../../images/button__on_m.png)" });
    var td = $(id).find("td");
    var flag = "_on";
    if (!id.disabled) {
    //    flag = "_gray";
    //    return;
    }

    if (document.getElementById("CurrentId")) {
    //    return;
    }

    if ($("#CurrentId").val() == "") {
    var bn=$(id).text();
    if(bn=="修改"||bn=="删除")
        return;
    }

    td[1].background = changeimg(td[1].background, "m", flag);
    td[2].background = changeimg(td[2].background, "m", flag);

    td = $(id).find("img")
    td[0].src = changeimg(td[0].src, "l", flag);

    td[2].src= changeimg(td[2].src, "r", flag);

    //     $(td[0]).css({ "background-image": "url(../../images/button_on_l.png)" });
    //     $(td[1]).css({ "background-image": "url(../../images/button__on_m.png)" });
    //     $(td[2]).css({ "background-image": "url(../../images/button__on_r.png)" });

}

function rechangeimg(id) {
    var td = $(id).find("td");
    
    td[1].background =  td[1].background .replace("_gray","")
    td[2].background =  td[2].background .replace("_gray","")

    td = $(id).find("img")
    td[0].src =  td[0].src .replace("_gray","")

    td[2].src = td[2].src.replace("_gray", "")
}

function changegrayimg(id) {
    //  $(id).css({ "background-image": "url(../../images/button__on_m.png)" });
    var td = $(id).find("td");
    var flag = "_gray";
  
    td[1].background = changeimg(td[1].background, "m", flag);
    td[2].background = changeimg(td[2].background, "m", flag);

    td = $(id).find("img")
    td[0].src = changeimg(td[0].src, "l", flag);

    td[2].src = changeimg(td[2].src, "r", flag);

}

function changeoutimg(id) {
    //  $(id).css({ "background-image": "url(../../images/button__on_m.png)" });
    var td = $(id).find("td");

    if (td[1].background.indexOf("_gray_") > -1) {
        return;
    }
    td[1].background = changeimg(td[1].background, "m", "");
    td[2].background = changeimg(td[2].background, "m", "");

    td = $(id).find("img")
    td[0].src = changeimg(td[0].src, "l", "");

    td[2].src = changeimg(td[2].src, "r", "");
    //   $(td[0]).css({ "background-image": "url(../../images/button_l.png)" });
    //   $(td[1]).css({ "background-image": "url(../../images/button_m.png)" });
    //   $(td[2]).css({ "background-image": "url(../../images/button_r.png)" });
}

function changeimg(backimg, layout, inout) {

    var fix = inout + "_" + layout + ".png"
    var i = backimg.indexOf("_");
    return backimg.substring(0, i) + fix;
}

function closetree(treeid) {
    var pn = document.getElementById(treeid).parentNode.parentNode;
    
    $('#weblayout').layout('collapse', 'west');

    setTimeout(function () { $(".layout-expand").css({ "display": "none" }); }, 500);
   document.getElementById("treebutton").style.display="block"

}

function showtree() {
    document.getElementById("treebutton").style.display = "none";
    //var fdoc = getFrameDocument("myframe");
    
    //var w = $(fdoc).find("#weblayout")
    //$(fdoc).find ('.panel-header').trigger("click")
    //$(fdoc).find('#layoutwest').layout("resize");
    $("#weblayout").layout('expand', 'west');
    setTimeout(function () {
     //   $(fdoc).find('#weblayout').layout('expand', 'west');
    })

}

function setTitle(treeid) {
   

    //var treetitle = "<table width='100%' height='24' border='0' cellpadding='0' cellspacing='0' class='TextBorder009'> <tbody><tr><td width='6%'></td><td width='74%' height='23' valign='bottom' class='tdtitle5'><table width='100%' height='23' border='0' cellpadding='0' cellspacing='0'> <tbody><tr> <td height='6'></td> </tr> <tr><td height='15' class='tdtitle5'>树状列表主题</td></tr><tr><td height='2'></td></tr></tbody></table></td> <td width='51' align='center' ><nobr><img src='../images/展开.gif' width='17' height='16'><img src='../images/收起.gif' width='17' height='16'><img src='../images/关闭_s.gif' width='17' height='16' onclick='closetree(\""+treeid+"\")' style='cursor:pointer'></nobr></td></tr></tbody></table>"
    //setTimeout(function () {
    //    var t = $(".layout-panel-west").find('.panel-header')
      
    //        t.html(treetitle);
    //},800);

 //  $('#layoutwest').attr("title", "动态的值");
}

function setmenu(sval) {
    $(document.getElementById('SiteAddress')).html(document.getElementById('menutext').value + " >> " + sval);
}

function addStyle(stylePath) {
    var container = document.getElementsByTagName("head")[0];
    var addStyle = document.createElement("link");
    addStyle.rel = "stylesheet";
    addStyle.type = "text/css";
    addStyle.media = "screen";
    addStyle.href = stylePath;
    container.appendChild(addStyle);
}
function startPickedFunc() {
    var id = this.id;
    var datetime = $(this).val();
    if (datetime.split(':').length == 3) {
        setTimeout(function () {
            $("#" + id).val(datetime);
        }, 100);
    }
    else if (datetime.split(':').length == 2) {
        var dateStr = datetime + ":00";
        setTimeout(function () {
            $("#" + id).val(dateStr);
        }, 100);
    }
    else if (datetime.split(' ').length == 2) {
        var dateStr = datetime + ":00:00";
        setTimeout(function () {
            $("#" + id).val(dateStr);
        }, 100);
    }
    else if (datetime.split('-').length == 3) {
        var dateStr = datetime + " 00:00:00";
        setTimeout(function () {
            $("#" + id).val(dateStr);
        }, 100);
    }
    else if (datetime.split('-').length == 2) {
        var dateStr = datetime + "-01 00:00:00";
        setTimeout(function () {
            $("#" + id).val(dateStr);
        }, 100);
    }
    else {
        var dateStr = datetime + "-01-01 00:00:00";
        setTimeout(function () {
            $("#" + id).val(dateStr);
        }, 100);
    }
}

function endPickedFunc() {
    var id = this.id;
    var datetime = $(this).val();
    if (datetime.split(':').length == 3) {
        setTimeout(function () {
            $("#" + id).val(datetime);
        }, 100);
    }
    else if (datetime.split(':').length == 2) {
        var dateStr = datetime + ":59";
        setTimeout(function () {
            $("#" + id).val(dateStr);
        }, 100);
    }
    else if (datetime.split(' ').length == 2) {
        var dateStr = datetime + ":59:59";
        setTimeout(function () {
            $("#" + id).val(dateStr);
        }, 100);
    }
    else if (datetime.split('-').length == 3) {
        var dateStr = datetime + " 23:59:59";
        setTimeout(function () {
            $("#" + id).val(dateStr);
        }, 100);
    }
    else if (datetime.split('-').length == 2) {
        var d = datetime.split('-');
        var month = d[1];
        var year = d[0];
        var dateStr = "";
        if (parseInt(d[1]) < 12) {
            month = parseInt(d[1]) + 1;
            dateStr = d[0]+"-"+month + "-01 00:00:00";
        }
        else {
            dateStr = d[0]+"-"+month + "-31 23:59:59";
        }

        setTimeout(function () {
            $("#" + id).val(dateStr);
        }, 100);
    }
    else {
        var dateStr = datetime + "-12-31 23:59:59";
        setTimeout(function () {
            $("#" + id).val(dateStr);
        }, 100);
    }
}


function setTimeInput(cid, param) {
    var parameter = param.split(","); 
    $("#" + cid).combobox({
        onChange: function (n, o) {
            if ($("#" + cid).combobox('getValue') == "-1" || $("#" + cid).combobox('getValue') == "0" ) {
                document.getElementById(parameter[0]).onfocus = 
                function onFocus() {
                    WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss', onpicked: startPickedFunc });
                    document.getElementById(parameter[0]).blur();
                }
                document.getElementById(parameter[1]).onfocus =
                function onFocus() {
                    WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss', onpicked: endPickedFunc });
                    document.getElementById(parameter[1]).blur();
                }
            }
            else if ( $("#" + cid).combobox('getValue') == "1" || $("#" + cid).combobox('getValue') == "2") {
                document.getElementById(parameter[0]).onfocus =
                function onFocus() {
                    WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:00', onpicked: startPickedFunc });
                    document.getElementById(parameter[0]).blur();
                }
                document.getElementById(parameter[1]).onfocus =
                function onFocus() {
                    WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:59', onpicked: endPickedFunc });
                    document.getElementById(parameter[1]).blur();
                }
            }
            else if ($("#" + cid).combobox('getValue') == "3") {
                document.getElementById(parameter[0]).onfocus =
                function onFocus() {
                    WdatePicker({ dateFmt: 'yyyy-MM-dd HH:00:00', onpicked: startPickedFunc });
                    document.getElementById(parameter[0]).blur();
                }
                document.getElementById(parameter[1]).onfocus =
                function onFocus() {
                    WdatePicker({ dateFmt: 'yyyy-MM-dd HH:59:59', onpicked: endPickedFunc });
                    document.getElementById(parameter[1]).blur();
                }
            }
            else if ($("#" + cid).combobox('getValue') == "4") {
                document.getElementById(parameter[0]).onfocus =
                function onFocus() {
                    WdatePicker({ dateFmt: 'yyyy-MM-dd 00:00:00', onpicked: startPickedFunc });
                    document.getElementById(parameter[0]).blur();
                }
                document.getElementById(parameter[1]).onfocus =
                function onFocus() {
                    WdatePicker({ dateFmt: 'yyyy-MM-dd 23:59:59', onpicked: endPickedFunc });
                    document.getElementById(parameter[1]).blur();
                }
            }
            else if ($("#" + cid).combobox('getValue') == "5") {
                document.getElementById(parameter[0]).onfocus =
                function onFocus() {
                    WdatePicker({ dateFmt: 'yyyy-MM-01 00:00:00', onpicked: startPickedFunc });
                    document.getElementById(parameter[0]).blur();
                }
                document.getElementById(parameter[1]).onfocus =
                function onFocus() {
                    WdatePicker({ dateFmt: 'yyyy-MM', onpicked: endPickedFunc });
                    document.getElementById(parameter[1]).blur();
                }
            }
            else if ($("#" + cid).combobox('getValue') == "6") {
                document.getElementById(parameter[0]).onfocus =
                function onFocus() {
                    WdatePicker({ dateFmt: 'yyyy-01-01 00:00:00', onpicked: startPickedFunc });
                    document.getElementById(parameter[0]).blur();
                }
                document.getElementById(parameter[1]).onfocus =
                function onFocus() {
                    WdatePicker({ dateFmt: 'yyyy-12-31 23:59:59', onpicked: endPickedFunc });
                    document.getElementById(parameter[1]).blur();
                }
            }
        }
    });
}


function GetNextCombox(cid, nextId) {
    $("#" + cid).combobox({
        onChange: function (n, o) {
            var param = $("#"+cid).combobox('getValue');
            GetCombox(nextId,cid+"="+param);
        }
    });
}

function GetCombox(cname,param) {
    if (document.getElementById(cname) == undefined) return;
    var nodepath = $("#nodepath").val(); //增加
    var url = "../basepage/HandlerCombobox.ashx?ConditionName=" + cname + "&nodepath=" + nodepath+"&"+param;
        $('#' + cname).combobox({
            url: url,
            valueField: 'id',
            textField: 'text'
        });
}


function exportExcel() {
    var tabList = $("[id^='component_'][id$='_querydatatable']");
    if (tabList.length > 0) {
        var ctrlIdDesc = $(tabList[tabList.length-1]).attr("id");
        var nav = $("#" + $(tabList[tabList.length-1]).attr("id") + "_excelnav").val();
        if (nav == null)
            nav = "";
        var sPara = f_conditon("");
        if (sPara.indexOf("StationTree=") < 0 && $("div[id*='treeview']").length > 0) {
            sPara += ";StationTree=ascxvf;";
        }
        var currentpage = $("#currentpage" + ctrlIdDesc).val();
        if (currentpage == null) {
            currentpage = 1;
        }
        var win = $.messager.progress({
            title: '正在生成excel文件...',
            msg: ''
        });
        PageMethods.ExportExcel(ctrlIdDesc, currentpage, sPara, nav, function (result) {
            $.messager.progress('close');
            if (result == "no data") {
                alert("没有可导出数据");
                return;
            }
            var url = "http://" + location.host + "/public/tempfile/" + result;
            window.open(url);
        });
    }
    else {
        alert("找不到数据表格，无法导出！");
    }
}


function paddingLeft(str, lenght) {
    str = str.toString();
    if (str.length >= lenght)
        return str;
    else
        return paddingLeft("0" + str, lenght);
}

var timerTableId = "";


function startTimer(componentId, timeInterval) {
    if (!timerTableId.indexOf(componentId) > -1) {
        timerTableId += componentId + ";";
    }
    setInterval(function () {
        if (componentId.indexOf("querydatatable") > -1) {
            queryDataTableTimer(componentId,"");
        }
    }, timeInterval*1000);
}

var timerTableRowId = 0;
function queryDataTableTimer(ctrlIdDesc, param) {
    var trd = $("#" + ctrlIdDesc + " tr");
    $.each(trd, function (i, n) {
        if (i > 0) {
            if ($(n)[0].className == "selecttr") {
                timerTableRowId = i;
            }
        }
    });
    var sPara =param + ";" + f_conditon("");
    if (timerTableRowId == 0)
        return;
    var currentpage = $("#currentpage" + ctrlIdDesc).val();
    var totalnum = $("#table" + ctrlIdDesc + "total").val() == undefined ? "" : $("#table" + ctrlIdDesc + "total").val();
    PageMethods.getTable(ctrlIdDesc, currentpage, sPara,totalnum, function (result) {
        var c = document.getElementById(ctrlIdDesc);
        var v = $("#" + ctrlIdDesc);
        c = $("#" + ctrlIdDesc).html();
        v.html(result);
        var h = $(window).height();
        var tr = $("#" + ctrlIdDesc + " table tr");
        $.each(tr, function (i, n) {
            if (i > 0 && i == timerTableRowId) {
                $(n).trigger("click");
            }
        });
        PageMethods.getPageID(ctrlIdDesc, function (result) {
            pid = result.split('_')[1];
            if (pid == "") return;
            var pagesum = $("#tablepagesum" + pid).val();
            var sp = $("#" + result + " input[id^=SumPage]")
            var psize = $("#" + ctrlIdDesc + " input[id^=rowcount]").val();
            sp.val(pagesum);
            sp = $("#" + result + " span[id^=spansumpage]")
            sp.html(pagesum);
            pid = $("#" + result + " input[id=currentpage" + ctrlIdDesc + "]").val();
            if (pid == undefined) return;
            var cid = $(sp).attr("id").substring("spanSumPage".length);
        });

    });
}