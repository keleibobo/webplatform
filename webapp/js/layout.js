var svgFirstLoad = "true";
function f_loadsvg(iappname, irefData) {
    document.body.style.textAlign = "center";
    
        appname = iappname;
        refData = irefData;
    
    var sval = "";
    try {
        sval = $("#svgnodeid").val();
    }
    catch (ex) {
        sval = "220kV杨桥变电站10kVⅢ母PT";
    }
    if (typeof (sval) != "undefined" && sval != "" && sval != "0") {
        PageMethods.GetFilename(sval, svgFirstLoad, function (result) {
            svgFirstLoad = "false";
            if (result == '' || result == '[]') {
                return;
            }
            var fInfo = eval("(" + result + ")");
            showSVG(fInfo);
        });
    }
    else
    {
        setTimeout("f_loadsvg('" + appname + "','" + refData + "')", 500);
    }
}

var firstext = "";
function ComboxAutoHidden(elmentId) {
    $("#" + elmentId).parent().css("display", "none");
    $("#" + elmentId).parent().prev().css("display", "none");
}

function f_hidecombox(sourceid, destid) {
    var nodepath = $("#nodepath").val();
    if (nodepath.indexOf("1(FZPTSvr.syspowerdatatype)") > -1 || nodepath.indexOf("3(FZPTSvr.syspowerdatatype)") > -1) {
        $("#component_47_querydatatable").parent().prev().show();
        $("#component_47_querydatatable").parent().css("position", "absolute");
        $("#component_47_querydatatable").parent().css("bottom", "65px");
        $("#tbCondition").show();
    } else {
        $("#tbCondition").hide();
    }
    
}

function loaddata(sourceid,destid) {
    if ($("#" + sourceid).attr("class").indexOf("tree") >= 0 && $("#" + destid).attr("class") != null && $("#" + destid).attr("class").indexOf("datagrid") >= 0) {
        
    }
    else if ($("#" + sourceid).attr("class").indexOf("tree") >= 0 && $("#component_" + destid + "_condition").length > 0 && $("#component_" + destid + "_condition").attr("id").indexOf("condition") >= 0) {
        var List = $("#component_" + destid + "_condition" + " [sattr*='combobox_']");
        var node = $('#' + sourceid).tree('getSelected');
        if (node != null) {
            if (node.id != "11" && node.id != "12" && node.id != "13" && node.id != "14") {
                if (node.attributes.value.split('FZPTSvr.webcomm_Profession').length > 2) {
                    node = $('#' + sourceid).tree("getParent", node.target);
                    node = $('#' + sourceid).tree("getParent", node.target);
                }
                else if (node != $('#' + sourceid).tree("getRoot")) {
                    node = $('#' + sourceid).tree("getParent", node.target);
                }
            }
            for (x = 0; x < List.length; x++) {
                if (node.id == "11") {
                    if ("webcomm_EventType" == $(List[x]).attr("id")) {
                        $(List[x]).parent().css("display", "");
                        $(List[x]).parent().prev().css("display", "");
                    }
                    if ("webcomm_EventLevel" == $(List[x]).attr("id")) {
                        $(List[x]).parent().css("display", "");
                        $(List[x]).parent().prev().css("display", "");
                    }
                    if ("urm_user" == $(List[x]).attr("id")) {
                        $(List[x]).parent().css("display", "none");
                        $(List[x]).parent().prev().css("display", "none");
                    }
                    if ("pointtype" == $(List[x]).attr("id")) {
                        $(List[x]).parent().css("display", "none");
                        $(List[x]).parent().prev().css("display", "none");
                    }
                }
                if (node.id == "12") {
                    if ("webcomm_EventType" == $(List[x]).attr("id")) {
                        $(List[x]).parent().css("display", "");
                        $(List[x]).parent().prev().css("display", "");
                    }
                    if ("webcomm_EventLevel" == $(List[x]).attr("id")) {
                        $(List[x]).parent().css("display", "");
                        $(List[x]).parent().prev().css("display", "");
                    }
                    if ("urm_user" == $(List[x]).attr("id")) {
                        $(List[x]).parent().css("display", "none");
                        $(List[x]).parent().prev().css("display", "none");
                    }
                    if ("pointtype" == $(List[x]).attr("id")) {
                        $(List[x]).parent().css("display", "none");
                        $(List[x]).parent().prev().css("display", "none");
                    }
                }
                if (node.id == "13") {
                    if ("webcomm_EventType" == $(List[x]).attr("id")) {
                        $(List[x]).parent().css("display", "");
                        $(List[x]).parent().prev().css("display", "");
                    }
                    if ("webcomm_EventLevel" == $(List[x]).attr("id")) {
                        $(List[x]).parent().css("display", "none");
                        $(List[x]).parent().prev().css("display", "none");
                    }
                    if ("urm_user" == $(List[x]).attr("id")) {
                        $(List[x]).parent().css("display", "");
                        $(List[x]).parent().prev().css("display", "");
                    }
                    if ("pointtype" == $(List[x]).attr("id")) {
                        $(List[x]).parent().css("display", "none");
                        $(List[x]).parent().prev().css("display", "none");
                    }
                }
                if (node.id == "14") {
                    if ("webcomm_EventType" == $(List[x]).attr("id")) {
                        $(List[x]).parent().css("display", "none");
                        $(List[x]).parent().prev().css("display", "none");
                    }
                    if ("webcomm_EventLevel" == $(List[x]).attr("id")) {
                        $(List[x]).parent().css("display", "none");
                        $(List[x]).parent().prev().css("display", "none");
                    }
                    if ("urm_user" == $(List[x]).attr("id")) {
                        $(List[x]).parent().css("display", "none");
                        $(List[x]).parent().prev().css("display", "none");
                    }
                    if ("pointtype" == $(List[x]).attr("id")) {
                        $(List[x]).parent().css("display", "");
                        $(List[x]).parent().prev().css("display", "");
                    }
                }
                var nodepath = "&nodepath=" + $("#nodepath").val();
                asynloadcombobox($(List[x]).attr("id"), nodepath, destid);
            }
        }
    }
    //else if ($("#component_0_condition" + " [sattr*='combobox_']").length > 0) {
    //    var list = $("#component_0_condition" + " [sattr*='combobox_']");
    //    var nodepath = "&nodepath=" + $("#nodepath").val();
    //    for (var i = 0; i < list.length; i++) {
    //        asynloadcombobox($(list[i]).attr("id"), nodepath, destid);
    //    }
    //}
}

function asynloadcombobox(cname, sourceValue, conditionId) {
    if (document.getElementById(cname) == undefined) return;
    var nodepath = $("#nodepath").val(); //增加
    var param = "";
    var List = $("#component_" + conditionId + "_condition" + " [sattr*='combobox_']");
    for (i = 0; i < List.length; i++) {
        if ($(List[i]).attr("id") != cname) {
            param += "&" + $(List[i]).attr("id") + "=" + $('#' + $(List[i]).attr("id")).combobox('getValue');
        }
    }

    var url = "../basepage/HandlerCombobox.ashx?ConditionName=" + cname + sourceValue + param;
    setTimeout(function() {
        $('#' + cname).combobox({ url: url, valueField: 'id', textField: 'text' });
        $('#' + cname).combobox('textbox').bind('blur', function() {
            var inputV = $('#' + cname).combobox('getText');
            var datas = $('#' + cname).combobox('getData');
            if (datas == null) return;
            var size = datas.length;
            if (inputV == "" || size == 0) {
                return;
            } else {
                for (var i = 0; i < size; i++) {
                    if (inputV == datas[i].text) {
                        return;
                    }
                }
                $('#' + cname).combobox('clear');
            }
        });
    }, 10);
}

function comboboxInit(cid) {
    setTimeout(function () {
        $('#' + cid).combobox({ url: '../public/data.json', valueField: 'id', textField: 'text' });
    }, 500);
}

var firstHide = true;
function f_loadtree(treeview) {               //加载导航树数据
    var win = $.messager.progress({           //设置弹出框
        title: '正在获取导航树数据中...',
        msg: ''
    }); 
    if ($("body").find("#svg").length > 0) {   //设置样式
        if (parent.document.getElementById('style').value == "green") {
            $(".treeback").css("background", "#018781");
            $("#layoutwest").css("background", "#018781");
        }
        else {
            $(".treeback").css("background", "#0062A0");
            $("#layoutwest").css("background", "#0062A0");
        }
    }
    var url = "../basepage/HandlerTreeview.ashx?action=getTree&random=123123";

    $("#" + treeview).tree({   //获取数据
        lines: true,
        url: url,
        animate:false,
        onBeforeExpand: function (node, param) {// 获取该节点下其他数据  
            var win = $.messager.progress({
                title: '正在获取导航树数据中...',
                msg: ''
            });
            $("#" + treeview).tree("options").url = url + "&nodepath=" + node.attributes.value;
        },
        onClick: function (node) {  //单击事件
            clickNode(node, treeview);
        },
        onExpand: function (node) {  //树展开的时候触发
            $.messager.progress('close');
            if (node.checked) {
                $('#' + treeview).tree('check',node.target);
            }
        },
        onDblClick: function (node) { //双击事件
            $("#TreeNodeText").val(node.text);
            $("#svgnodeid").val(node.id);
            $("#nodepath").val(node.attributes.value);
            setTreeNodevalue(node.attributes.value);
            if ($("#" + treeview).attr("twoclick") != "") {
                eval($("#" + treeview).attr("twoclick") + "('" + $("#" + treeview).attr("id") + "','" + $("#" + treeview).attr("destid") + "')");
            }
            var clicktype = node.attributes.ondblclicktype;
            if (clicktype) {
                var nodetype = $("#SelectNodeType").val();
                eval(node.attributes.ondblclick + "('" + node.attributes.ondblclicktype + "')");
            }
        },
        onLoadSuccess: function (data, node) {
            $.messager.progress('close');
            if ($("#layoutwest").attr("conditionhide") == "true" && firstHide) {
                $("#conditionhide").trigger("click");
                firstHide = false;
            }
            if ($("body").find("#svg").length > 0) {
                if (parent.document.getElementById('style').value == "green") {
                    $(".tree-node").css("color", "white");
                }
                else {
                    $(".tree-node").css("color", "#ffffff");
                }
            }
            $("#" + treeview).show();
            resizeding(treeview);
            if (node.length > 100) {
                setTreeCss(treeview);
            }
            if (firstext == "" && node[0] && node[0].children[0]) {
                firstext = node[0].children[0].text;
                $("#svgnodeid").val(node[0].children[0].id);
                var treeOption = $('#' + treeview).tree('options');
                if (treeOption.checkbox == false) {
                    $("#TreeNodeText").val(firstext);
                }
                if ($("body").find("#svg").length == 0 && $("#layoutwest")[0].innerText.indexOf("用户列表") < 0) {
                    setTimeout(function () {
                        clickNode(node[0].children[0], treeview);
                    }, 2000);
                }
            }
        }
    });
}

function clickNode(node, treeview) {//单机事件
    $("#svgnodeid").val(node.id);
    $("#TreeNodeText").val(node.text);
    var treeOption = $('#' + treeview).tree('options');
    if (treeOption.checkbox == false) {
        $("#TreeNodeText").val(node.text);
    }
    $("#nodepath").val(node.attributes.value);
    setTreeNodevalue(node.attributes.value);
    if ($("#" + treeview).attr("oneclick") != "") {
        var nodearray = $("#nodepath").val().split('\\');
        var param = "";
        if (nodearray.length > 1) {
            param = nodearray[nodearray.length - 2].substring(nodearray[nodearray.length - 2].indexOf("(") + 1, nodearray[nodearray.length - 2].lastIndexOf(")"));
            if (param.indexOf('.') > 0) {
                param = param.split(".")[1];
            }
        }
        if ($("#" + treeview).attr("param") == "" || ($("#" + treeview).attr("param") != "" && $("#" + treeview).attr("param").indexOf(param) >= 0))
            if ($("#" + treeview).attr("destid").indexOf(",") > 0) {
                var ar = $("#" + treeview).attr("destid").split(',');
                var ac = $("#" + treeview).attr("oneclick").split(',');
                var i = 0;
                for (i = 0; i < ar.length; i++) {
                    eval(ac[i] + "('" + $("#" + treeview).attr("id") + "','" + ar[i] + "')");
                }
            }
            else {
                eval($("#" + treeview).attr("oneclick") + "('" + $("#" + treeview).attr("id") + "','" + $("#" + treeview).attr("destid") + "')");
            }
    }
    else {
        var clicktype = node.attributes.onclicktype;
        if (clicktype) {
            var nodetype = $("#SelectNodeType").val();
            if (node.attributes.onclick) {
                eval(node.attributes.onclick + "('" + node.attributes.onclicktype + "')");
            }
            else if (node.attributes.clickjs) {
                eval(node.attributes.clickjs); // + "()"
            }
        }
        else {
            var options = $("#" + treeview).tree('options');
            if (options.checkbox == false) {
                if (node.attributes.clickjs) {
                    eval(node.attributes.clickjs); // + "()"
                }
            }
        }
    }

}

function f_dbtree(cname, func) {
    $("#" + cname).tree({
        onDblClick: function(node) {
            eval(func + "()");
        }
    });
}


//单个条件组件
function f_initcombobox(sourceid, destid) {
    var cid = document.getElementById("component_" + destid + "_combox");
    var cbox = $(cid).find("table input[class*='easyui-combobox']")[0];
    if (cbox&&cbox.id) {
        f_loadcombobox(cbox.id);
    }
}


function f_loadcombobox(cname) {
    if (cname == "reportname") return;
    if (document.getElementById(cname) == undefined) return;
    var nodepath = $("#nodepath").val();//增加

    var url = "../basepage/HandlerCombobox.ashx?ConditionName=" + cname + "&nodepath=" + nodepath;
    if (cname == "charttype") {
        $('#charttype').combobox({
            onChange: function () {
                if (globalChartName != "") {
                    valueLength = 0;
                    realtime(globalChartName, globalArgs);
                }
            },
            url: url,
            valueField: 'id',
            textField: 'text'
        });
    }
    if (cname == "datatype" && $("#stype").val() == "eventtimesquery") {
        setTimeout(function () {
            $('#' + cname).combobox({
                url: url,
                valueField: 'id',
                textField: 'text',
                onLoadSuccess: function () {
                    if ($("#smalltype").length > 0) {
                        url = "../basepage/HandlerCombobox.ashx?ConditionName=smalltype&nodepath=" + nodepath + ";datatype=" + $("#datatype").combobox('getValue') + ";";
                        $('#smalltype').combobox({ url: url, valueField: 'id', textField: 'text' });
                    }
                },
                onChange: function () {
                    if ($("#smalltype").length > 0) {
                        url = "../basepage/HandlerCombobox.ashx?ConditionName=smalltype&nodepath=" + nodepath + ";datatype=" + $("#datatype").combobox('getValue') + ";";
                        $('#smalltype').combobox({ url: url, valueField: 'id', textField: 'text' });
                    }
                }
            });

        }, 10);
    }
    else {
        setTimeout(function () {
            $('#' + cname).combobox({ url: url, valueField: 'id', textField: 'text' });
            $('#' + cname).combobox('textbox').bind('blur', function () {
                var inputV = $('#' + cname).combobox('getText');
                var datas = $('#' + cname).combobox('getData');
                if (datas == null) return;
                var size = datas.length;
                if (inputV == "" || size == 0) {
                    return;
                } else {
                    for (var i = 0; i < size; i++) {
                        if (inputV == datas[i].text) {
                            return;
                        }
                    }
                    $('#' + cname).combobox('clear');
                }
            });
        }, 10);
    }
}


//暂不考虑导航树条件
function f_loadlayout() { //reinit
    var setcenter = true;
    f_buttondisable();
    document.getElementById("CurrentId").value = "";
    var hiddentext = $("input[type=text]");
    hiddentext.each(function(idx, item) {
        if (item) {
        }
    });

    var dc = $("div[id^=component_]");
    dc.each(function(idx, item) {
        var split = this.id.split('_');
        var cid = split[1];
        var ctype = split[2];
        if (ctype == 'querydatatable') {
            if ($("#" + this.id).attr("ctype") == "condition" && checkparam.length > 0) {

            }
            else {
                if (!(this.id.indexOf('170') > 0 || this.id.indexOf('204') > 0)) {
                    f_loadpage(this.id, '1', f_getChecked(this.id));
                    $(" input[id=currentpage" + this.id + "]").val("1");
                    $(" span[id=spanpagenum" + this.id + "]").html("1");
                }
            }

        }
        else if (ctype == 'webpage' || ctype == 'htmlreport') {
            var fname = "iframe_" + cid;
            var src = "../basepage/iframe.html";
            if (document.all) {  
                if (document.frames[fname]) {
                    document.frames[fname].location = src;
                    fdoc = document.frames[fname].document;
                }
            } else {   
                if (document.getElementById(fname)) {
                    fdoc = document.getElementById(fname);
                    fdoc.src = src;
                }
            }

        }
        else if (ctype.indexOf('chart') > -1) {
            if ($("div[id$=_splinechart]").length > 1 && setcenter) {
                SetchartCenter(this.id);
                setcenter = false;
            } else if ($("div[id$=_splinechart]").length == 1) {
                f_loadingchart(this.id, f_getChecked(this.id));
            }

        }
        else if(ctype == 'htmlcontent') {
            LoadHtml(this.id);
        }
    });
    setTimeout(function() {
        if (document.getElementById("weblayout")) {
            $('#weblayout').layout('resize');
            $('#weblayout').layout('panel', 'center').panel('resize', { width: $('#layoutcenter').width() + 1 });
        }
    }, 500);
}


function LoadHtml(id) {
    var sPara = f_conditon(f_getChecked(id));
    var cid = id.split('_')[1];
    sPara = escape(sPara.replace(/;/g, '').replace(/&/g, ';'));
    var url = '../basepage/handlergethtmlcontent.ashx?sid=' + cid + '&params=' + sPara;
    var fname = "iframe_" + cid;
    if (document.all) {//IE  
        if (document.frames[fname]) {
            document.frames[fname].location = url;
        }
    } 
}


function f_initgrid(cname){
 $('#'+cname).datagrid({
   width:'auto',
   height:300,               
   striped: true,  
   singleSelect : true,  
   pagination: true,  
   rownumbers: true,
   columns:[[]]
   });
}

function f_loadgrid(cname,sPara,pagerange){
   var sid=cname.split("_")[1]; 
   var url = "../basepage/HandlerTable.ashx?sid="+sid+"&"+sPara;
    $.ajax({
        url: url + torange(pagerange),
        type: 'get',
        success: function(data) {
            if (data == "") return;
            $.messager.progress('close');
            data = eval("(" + data + ")");
            $("#table_" + sid).datagrid({
                columns: [data.columns]
            }).datagrid("loadData", data.rows);
            var p = $("#table_" + sid).datagrid('getPager');
            $(p).pagination({
                onSelectPage: function(pageNumber, pageSize) {
                    $(this).pagination('loading');

                    $.ajax({
                        url: url + torange(pageSize + "," + pageNumber),
                        type: 'get',
                        success: function(data) {
                            data = eval("(" + data + ")");
                            $("#table_" + sid).datagrid("loadData", data.rows);
                        }
                    });

                    $(this).pagination('loaded');
                },
                onBeforeRefresh: function(pageNumber, pageSize) {
                    $(this).pagination('loading');
                },
                onRefresh: function() {
                    $(this).pagination('loaded');
                }
            });
        }
    });
}

function torange(pagerange,total){
   var rows=pagerange.split(',')[0];
   var page=pagerange.split(',')[1];
   var ibegin=(page-1)*rows;
   var iend=page*rows-1;
    if (iend >= total && total >= rows) iend = total - 1;
   return ";ibegin="+ibegin+";iend="+iend;
}


function f_loadtable() { //reinit
    f_buttondisable();
    document.getElementById("CurrentId").value = "";
    var dc = $("div[id^=component_]");
    dc.each(function(idx, item) {
        var split = this.id.split('_');
        var cid = split[1];
        var ctype = split[2];
        if (ctype == 'querydatatable') {
            f_loadpage(this.id, '1', '');
        } else if (ctype == 'webpage' || ctype == 'htmlreport') {
            document.getElementById("iframe_" + cid).src = "../basepage/iframe.html";
        }
    });
}

function f_loadiframe(fname, src) {
    var fdoc = document.getElementById(fname);
    if ($.browser.msie) {
        $("#" + fname).css("height", fdoc.document.documentElement.clientHeight);
    } else if ($.browser.chrome) {
        $("#" + fname).css("height", fdoc.scrollHeight);
    }
    if (document.all) { //IE  
        if (document.frames[fname]) {
            if ($.browser.version == "8.0") {
                document.frames[fname].location.href = src;
            } else if ($.browser.version == "9.0") {
                setTimeout(function() {
                    $("#" + fname).attr("src", src);
                }, 500);
            }
        }
    } else //Firefox or Chrome   
    if (document.getElementById(fname)) {
        document.getElementById(fname).src = src;
    }
}


function f_loadaccordion(cid){
var url="../basepage/HandlerAccordion.ashx";
    $(function() {
        $.ajax({
            url: url,
            type: 'get',
            success: function(data) {

                var ul = "<div title='c'><p>c</p></div><div title='c++'><p>c++</p></div><div title='Java'><p>Java</p></div>";


                $("#" + cid).append(ul);
                $("#" + cid).accordion({ border: false, fit: true });
            }
        });
    });
}

function f_loadcondition(cid) {
    var sid = cid.split("_")[1];
    var url = "../basepage/HandlerCondition.ashx?sid=" + sid;

    $.ajax({
        url: url,
        type: 'get',
        success: function(data) {

            if (data) {
                $("#" + cid).html(data);
            }
        }
    });
}

function f_openbtniframe(){
    var sPara=$("#ComponentId").val();
	if(sPara&&sPara!=""){
      var src = "../basepage/HandlerHTML.ashx?"+sPara;
        window.open(src);
		}
}

function f_openiframe(cname) {
    var args = "";
    var cid = cname.split("_")[1];
    $("tr").live("click", function(event) {
            var id = $(this).attr("id");
            event.preventDefault(); //关键是这句
            var tr = event.target;
            if (tr.nodeName == "TD") {
                tr = tr.parentNode;
            }

            var onc = tr.attributes["onclick"];
            var args = "";
            if (onc != null)
                args = onc.value.split(",");

            if (onc && args != undefined && args.length > 3) {
                var val = args[3].replace(/\"/g, "")
                var sPara = f_conditon(val);
                $("#ComponentId").val("cid=" + cid + "&args=" + sPara);
            }
        }
    );

    $("tr").live("dblclick", function(event) {
            event.preventDefault(); //关键是这句
            var tr = event.target;
            if (tr.nodeName == "TD") {
                tr = tr.parentNode;
            }
            var rowIndex = tr.rowIndex;
            var onc = tr.attributes["onclick"];
            var args = "";
            if (onc)
                args = onc.value.split(",");
            if (onc && args != undefined && args.length > 3 && args[3].indexOf('=\"') < 0) {
                var reportId = args[0].split('_')[args[0].split('_').length - 2];
                var tabid = args[1].replace(/\"/g, "");
                var src = "../basepage/HandlerHTML.ashx?cid=" + reportId + "&componentid=" + tabid + "&rowindex=" + rowIndex;
                window.open(src);
            }
        });
}


function setclick(tr, cid) {
    if (tr.nodeName == "TD") {
        tr = tr.parentNode;
    }
    var onc = tr.attributes["onclick"];
    var args = "";
    if (onc != null)
        args = onc.value.split(",");

    if (onc && args != undefined && args.length > 3) {


        var val = args[3].replace(/\"/g, "");
        var sPara = f_conditon(val);
        $("#ComponentId").val("cid=" + cid + "&args=" + sPara);
    }
}

var treeHeight;
var ltype = "BaseSvr.BusinessComponentLayoutCall";

function setLayout(nodeid, css) {
    var uicss;
    try {
        uicss = eval("(" + css + ")");
    } catch (e) {
        uicss = css;
    }

    var pn = document.getElementById(nodeid);
    var lid = nodeid.split('_')[1];
    var ln = document.getElementById(lid + ltype);
    if (ln == null) return;
    ln.appendChild(pn);
    setTreeCss(nodeid);
    if (uicss.layoutcss) {
        pn = document.getElementById(lid + ltype).parentNode;
        $(pn).css(uicss.layoutcss);
    }

    if (uicss.divcss) {
        $("#" + nodeid).css(uicss.divcss);
        if (uicss.divcss.top) {
            $("#" + nodeid).parent().css("top", uicss.divcss.top);
        }
        if (uicss.divcss.height) {
        }
    }
    if (nodeid == "component_171_querydatatable") {
        $("#" + nodeid).css("height", document.body.offsetHeight - 360);
    }
    if (uicss.callcss) {
        $(ln).css(uicss.callcss);
    }

    if (uicss.layoutclass) {
        pn = document.getElementById(lid + ltype).parentNode;
        $(pn).addClass(uicss.layoutclass);
    }

    if (uicss.selflayout) {
        pn = document.getElementById(nodeid).parentNode;
        $("#" + nodeid).parent().css(uicss.selflayout);
    }

    if (uicss.height) {
        var ch = uicss.height;
        if (ch == '100%') {
            resizeding(nodeid);
        }
    }

    if (uicss.target == '_blank') {
        f_openiframe(nodeid);
    }
    if (uicss.pheight != null) {
        if (firstload) {
            $("#layoutcenter").css("height", $("#layoutcenter")[0].offsetHeight - 40);
            firstload = false;
            if ($("#" + nodeid).parent().next().length > 0) {
                $("#" + nodeid).parent().css("height", $("#layoutcenter")[0].offsetHeight - 60);
            } else {
                $("#" + nodeid).parent().css("height", $("#layoutcenter")[0].offsetHeight - 31);
            }
        }
    }

    if (uicss.splits) {
        var zindex = uicss.splits.split("_")[1];
        f_splitpanel(nodeid, uicss.splits);
    }

    setTimeout(function() {
        $("#weblayout").layout();
        if (document.getElementById("innerlayout"))
            $('#innerlayout').layout();
    }, 100);
}

function setTreeCss(nodeid) {
    if (nodeid.indexOf("_treeview") > -1) {
        if ($("body").find("div[id*='datatablepagenavigate']").length < 2 && $("body").find("#svg").length == 0 || $("#stype").val() == "UserManagement") {
            if (treeHeight != null) {
                clearInterval(treeHeight);
            }
            treeHeight = setInterval(function () {
                pn = $(".treeback");
                $(pn.find("ul")[0]).css("overflow", "");
                var height = $(pn).css("height");
                height = height.replace("px", "");
                if (height > 50 && height < window.screenheight) {
                    clearInterval(treeHeight);
                    if ($(pn).parent().parent().next().length > 0) { //判断导航树的layout是否有表格或者条件
                        if ($("#component_83_treeview").length > 0) {
                            $(pn).css("height", $("#layoutwest")[0].offsetHeight - 428);
                            $(pn.find("ul")[0]).css("height", $("#layoutwest")[0].offsetHeight - 434);
                        }
                        else {
                            $(pn).css("height", $("#layoutwest")[0].offsetHeight - 402);
                            $(pn.find("ul")[0]).css("height", $("#layoutwest")[0].offsetHeight - 409);
                        }
                    }
                    else {
                        $(pn).css("height", $("#layoutwest")[0].offsetHeight - 57);
                        $(pn.find("ul")[0]).css("height", $("#layoutwest")[0].offsetHeight - 60);
                    }
                }
                else if (height > window.screenheight) {
                    clearInterval(treeHeight);
                    $(pn).css("height", parseInt(height) - $("#layoutwest")[0].offsetHeight - 57);
                }
                else if (appname == "joyo-d41") {
                    clearInterval(treeHeight);
                    $(pn).parent().parent().css("height", $("#layoutwest")[0].offsetHeight - 57);
                    if ($.browser.msie) {
                        if ($.browser.version == "9.0") {
                           // $(pn).css("height", "");
                        }
                        else if ($.browser.version == "8.0") {
                            $(pn.find("ul")[0]).css("overflow", "auto");
                        }
                    }
                    $(pn.find("ul")[0]).css("height", $("#layoutwest")[0].offsetHeight - 60);
                }
            }, 200);
        }
        else {
            if (treeHeight != null) {
                clearInterval(treeHeight);
            }
            treeHeight = setInterval(function () {
                pn = $(".treeback");
                $(pn.find("ul")[0]).css("overflow", "auto");
                var height = $(pn)[0].offsetHeight;
                if (height > 50 && height < window.screenheight) {
                    clearInterval(treeHeight);
                    if ($("body").find("#svg").length > 0) {
                        $(pn).css("height", $("#layoutwest")[0].offsetHeight - 57);
                        $(pn.find("ul")[0]).css("height", $("#layoutwest")[0].offsetHeight - 60);
                    }
                    else {
                        $(pn).css("height", $("#layoutwest")[0].offsetHeight - 57);
                    }
                }
                else if (height > $("#layoutwest")[0].offsetHeight) {
                    clearInterval(treeHeight);
                    if ($("body").find("#svg").length > 0) {
                        $(pn).css("height", $("#layoutwest")[0].offsetHeight - 57);
                        $(pn.find("ul")[0]).css("height", $("#layoutwest")[0].offsetHeight - 60);
                    }
                    else {
                        $(pn).css("height", $("#layoutwest")[0].offsetHeight - 57);
                    }
                }
            }, 200);
        }
    }
}

var firstload = true;
var version = document.documentMode;

function resizeding(nodeid) {
    lid = nodeid.split('_')[1];
    if (nodeid.indexOf("ul_") < -1)
        $("#" + nodeid).height(document.getElementById(lid + ltype).offsetHeight);
    document.getElementById(nodeid).onresize = function () {
        getotherheight(nodeid);
    }

    $('#' + lid + ltype).layout();
    window.onresize = function () {
        setTimeout(function () {
            if ($.browser.msie || $.browser.mozilla) {
                var datatable = $("div[id^='component'][id$='querydatatable']");
                $.each(datatable, function (index, item) {
                    getotherheight(item.id);
                });
            }

            $('#weblayout').layout('resize');
            $('#weblayout').layout('panel', 'center').panel('resize', { width: $('#layoutcenter').width() + 1 });
            $("iframe[id*='iframe_']").css("height", document.documentElement.clientHeight);
            setTimeout(function () {
                
                if ($("svg").length > 0 || $("#svgEle").length > 0)
                    SVGInit();
                if ($("#svgEle").length > 0)
                    SVGInit();
                if ($("#component_96_treeview").length > 0) {
                    setTreeCss($($("div[id*='treeview']")[0]).attr("id"));
                }
                else if ($("#component_73_treeview").length > 0) {
                    $("ul[id*='ul_']").parent().css("height", $("div[id*='treeview']").parent().parent()[0].offsetHeight - 401);
                }
                else if ($("#component_83_treeview").length > 0) {
                    $("ul[id*='ul_']").parent().css("height", $("div[id*='treeview']").parent().parent()[0].offsetHeight - 427);
                }
                else if ($("#component_97_treeview").length > 0) {
                    $("ul[id*='ul_']").parent().css("height", $("div[id*='treeview']").parent().parent()[0].offsetHeight - 368);
                }

                else if ($("div[id*='treeview']").length > 0) {
                    setTreeCss($($("div[id*='treeview']")[0]).attr("id"));
                }
            }, 200);
        }, 500);
    }
}

var firstLoad = true;

function f_splitpanel(cname, csplit) {
    var position = csplit.split("_")[0];
    var zindex = csplit.split("_")[1];
    var cid = cname.split("_")[1] + ltype;
    var ui = "<div class='easyui-layout' data-options='fit:true' id='easyui" + zindex + "' style='width:100%;height:100%'></div>";

    var panel = document.getElementById("easyui" + zindex);
    var playout = "";
    if (position == "bottom") {
        playout = "<div data-options=\"region:'south',split:true\" id='panel" + cid + "' style='width:100%;height:210px'></div>"; //181+22
    } else if (position == 'top') {
        playout = "<div data-options=\"region:'center',split:true\" id='panel" + cid + "' style='width:100%;height:100%'></div>";
    }
    if (panel) {
        $(panel).append(playout);
    } else {
        $(document.getElementById(cid).parentNode).append(ui);
        document.getElementById(cid).appendChild(document.getElementById(cname));
        panel = document.getElementById("easyui" + zindex);
        $(panel).append(playout);
    }
    document.getElementById("panel" + cid).appendChild(document.getElementById(cid));
}

function splitheight(zindex, cid) {
    var eh = $(document.getElementById("easyui" + zindex)).height();
    var divlayout = $(document.getElementById("easyui" + zindex)).find("div [id$='BaseSvr.BusinessComponentLayoutCall']");
    var lh = 0;
    $.each(divlayout, function(indx, item) {
        if (item.id.indexOf(cid) == 0) {
            lh += $(item).height();
        }
    });
    $(document.getElementById("panel" + cid).parentNode).height(eh - lh - 50);
}


function getotherheight(nodeid) {
    if (nodeid.indexOf('ul_') > -1) {
        return false;
    }
    var lid = nodeid.split('_')[1];
    var ch = 35;
    var ln = document.getElementById(lid + ltype);
    var allayout = $(ln.parentNode).find(" [id$='" + ltype + "']");
    $.each(allayout, function(idx, item) {
        if (item.id != lid + ltype && $(item).css("display")!="none") {
            var h0 = $(item).height();
            ch = ch + h0;
        }
    });

    var page = $("#weblayout").find(" [id$='datatablepagenavigate']");
    if (page.length >= 1) {
        ch = ch + $(page[0]).height();
    }
    else
    {
        setTimeout(function() {
            page = $("#weblayout").find(" [id$='datatablepagenavigate']");
            if (page.length == 0) { //无分页
            } else {
                ch = ch + $(page[0]).height();
            }
            $("#" + nodeid).height(document.documentElement.offsetHeight - ch);
            if (document.getElementById("weblayout"))
                $("#weblayout").layout();
        }, 500);
        return false;
    }

    var ph = document.documentElement.offsetHeight;
    var lh = ln.parentNode.scrollHeight;
    if (ph > lh & lh > ch && ph - ln < ch) {
        ph = ln.parentNode.scrollHeight;
    }
    if (nodeid.indexOf('_333_')>-1 && appname == 'joyoc2') {
        ch = 430;
    }
    var tableHeight = ph - ch;
    if (nodeid.indexOf('_47_') > -1 && tableHeight < 420 && tableHeight >310) {
        tableHeight = tableHeight - 100;
        $("#" + nodeid).height(tableHeight);
    }
    var currentPageId = nodeid.split('_')[1];
    $("#" + nodeid).height(tableHeight);
    if (nodeid.indexOf('_333_') > -1 && appname == 'joyoc2') {
        ch = 430;
    }
    if (nodeid.indexOf('_111_') > -1 && (appname == "joyoaffzpt"))
        return;
    if (tableHeight > 300 && tableHeightArray[nodeid] != tableHeight) {
        tableHeightArray[nodeid] = tableHeight;
        var pageSize = parseInt((tableHeight - 24 - 18) / 22.5);
        var options = $("#" + nodeid + "_pagesize").find("option");
        var exist = false;
        $.each(options, function(index, item) {
            if (item.value == pageSize) {
                exist = true;
                item.selected = true;
            }
            if (configPageSize.indexOf(item.value) == -1) {
                $("#" + nodeid + "_pagesize option:last").remove();
            }
        });
        if (!exist) {
            $("#" + nodeid + "_pagesize").append("<option selected='true'>" + pageSize + "</option>");
        }
        PageMethods.SetPageNum(nodeid, pageSize, function (result) {
            $("#currentpagecomponent_" + currentPageId + "_querydatatable").val("1");
            f_loadpage(nodeid, "1", "");
            var dc = $("div[id^=component_]");
            dc.each(function (idx, item) {
                var split = item.id.split('_');
                var cid = split[1];
                var ctype = split[2];
                if (ctype.indexOf('chart') > -1) {
                    if (appname == "joyoaffzpt") {
                        nodeid = nodeid == "component_111_querydatatable" ? "component_47_querydatatable" : "component_111_querydatatable";
                        f_loadingchart(item.id, f_getChecked(nodeid));
                    } else {
                        if ($("div[id*=_splinechart]").length == 2) {
                            SetchartCenter($("div[id*=_splinechart]")[0].id);
                        } else {
                            nodeid = nodeid == "component_111_querydatatable" ? "component_47_querydatatable" : "component_111_querydatatable";
                            f_loadingchart(item.id, f_getChecked(nodeid));
                        }
                    }
                }
            });
        });
    } else if (tableHeight < 300 && tableHeightArray[nodeid] != tableHeight) {
        PageMethods.SetPageNum(nodeid, 10, function (result) {
            $("#currentpagecomponent_" + currentPageId + "_querydatatable").val("1");
            f_loadpage(nodeid, "1", "");
            tableHeightArray[nodeid] = tableHeight;
            var options = $("#" + nodeid + "_pagesize").find("option");
            $.each(options, function (index, item) {
                if (item.value == "10") {
                    item.selected = true;
                }
            });
            var dc = $("div[id^=component_]");
            dc.each(function (idx, item) {
                var split = item.id.split('_');
                var cid = split[1];
                var ctype = split[2];
                if (ctype.indexOf('chart') > -1) {
                    if (appname == "joyoaffzpt") {
                        nodeid = nodeid == "component_111_querydatatable" ? "component_47_querydatatable" : "component_111_querydatatable";
                        f_loadingchart(item.id, f_getChecked(nodeid));
                    } else {
                        if ($("div[id*=_splinechart]").length == 2) {
                            SetchartCenter($("div[id*=_splinechart]")[0].id);
                        } else {
                            nodeid = nodeid == "component_111_querydatatable" ? "component_47_querydatatable" : "component_111_querydatatable";
                            f_loadingchart(item.id, f_getChecked(nodeid));
                        }
                    }
                }
            });
        });
    }

    $("#weblayout").layout();
    return ch;
}

var tableHeightArray = [];      //保存表格高度

function getpageheight() {
    var page = $("#weblayout").find(" [id$='datatablepagenavigate']");
    var ch = $(page[0]).height();
    if (ch) {
        return ch;
    }
    else
    {
        return setTimeout("getpageheight()", 500);
    }
}

function moving(bid, afterid) {
    $("#" + bid).insertAfter($("#" + afterid));
}

function setTreeNodevalue(nodepath) {
    if (nodepath == undefined)return "";
    var ss = new Array();
    ss = nodepath.split("/");
    s = ss[ss.length - 1];

    ss = s.split("|");
    if (ss.length > 2) {
        sSelectNode = ss[0];
        sSelectNodeType = ss[1];
        $("#SelectNode").val(sSelectNode);
        $("#SelectNodeType").val(sSelectNodeType);
        if (document.getElementById("SiteAddress")) {
            if ($("#TreeNodeText").val() != "") {
                $(document.getElementById("SiteAddress")).html(document.getElementById("menutext").value + " >> " + $("#TreeNodeText").val());
            }
        }
    }
    nodepath = GetValuePath(nodepath, "\\");
    $("#nodepath").val(nodepath);
    return nodepath;
}

function GetValuePath(strValuePath)
    {
    var strSeparator="\\";
       var strRt = "";
        var aStr1 = strValuePath.split('/');
        for (var i = 0; i < aStr1.length; i++)
        {
            var s1 = aStr1[i];
            if (s1.length > 0)
            {
                var aStr2 = s1.split('|');
                if (aStr2.length > 2)
                {
                    if (strRt.length != 0)
                    {
                        strRt += strSeparator;
                    }
                    strRt += aStr2[0] + "(" + aStr2[1] + ")";
                    if (i == aStr1.length - 1)
                    {
                        if (aStr2[1]!=aStr2[2]&&aStr2[2]!="--")
                        {
                            strRt = strRt + strSeparator + "(" + aStr2[2] + ")";
                        }
                    }
                }
            }
        }
        return strRt;
    }


function getTreeNodevalue(nodepath) {
    var nodevalue = "";
    if ($(".treeback").find("ul").length > 0) {
        if ($(".treeback").find("ul")[0].id.indexOf("ul_") > -1) {
            var tid = $(".treeback").find("ul")[0].id;
            var root = $("#" + tid).tree('getRoot');
            var options = $("#" + tid).tree('options');
            if (options != null && options.checkbox == true) {
                var nodes = $("#" + tid).tree('getChildren');
                var nodePathStr = "";
                for (i = 0; i < nodes.length; i++) {
                    if ((nodes[i].state == null || childrenCheck(tid, nodes[i].target)) && nodes[i].checked == true) {
                        nodePathStr += GetValuePath(nodes[i].attributes.value,"\\") + ",";
                    }
                }
                nodePathStr = nodePathStr.substring(0,nodePathStr.length - 1);
                nodevalue = "nodepath=" + nodePathStr + ";";
            }
            else {
                nodevalue = "nodepath=" + nodepath + ";";
                var s = new Array();
                s = nodepath.split("/");
                var sss = s[s.length - 1].split("|");
                if (sss.length > 2) { //有改动
                    if (nodepath.indexOf("/") > -1) {
                        nodevalue += sss[1] + "=" + $("#SelectNode").val() + ";"
                    } else {//根节点
                        nodevalue += sss[2] + "=;";
                    }
                }
            }
        }
    }
    return nodevalue;
}

function childrenCheck(tid, nodeTarget) {
    if ($('#' + tid).tree('getChildren', nodeTarget).length > 0) {
        var nodeList = $('#' + tid).tree('getChildren', nodeTarget);
        for (var i = 0; i < nodeList.length;i++) {
            if (nodeList[i].checked == true) {
                return false;
            }
        }
    }
    return true;
}


function GetValuePath(strValuePath, strSeparator) {
    var strRt = "";
    var aStr1 = strValuePath.split('/');
    for (var i = 0; i < aStr1.length; i++) {
        var s1 = aStr1[i];
        if (s1.length > 0) {
            var aStr2 = s1.split('|');
            if (aStr2.length > 2) {
                if (strRt.length != 0) {
                    strRt += strSeparator;
                }
                strRt += aStr2[0] + "(" + aStr2[1] + ")";
                if (i == aStr1.length - 1) {
                    if (!aStr2[1] == aStr2[2]) {
                        strRt = strRt + strSeparator + "(" + aStr2[2] + ")";
                    }
                }
            }
        }
    }
    return strRt;
}



////获取导航树 选中
function getChecked(treeview) {
    var nodes = $('#'+treeview).tree('getChecked');
    var s = '';
    for (var i = 0; i < nodes.length; i++) {
        if (s != '') s += ',';
        s += nodes[i].text;
    }
    return s;
}

function f_initcondition(cid) {
    var cn = document.getElementById("component_0_condition");
    var pn = document.getElementById(cid + ltype);

    if (cn && pn) {
        pn.clientHeight = cn.clientHeight;
        pn.appendChild(cn);
    } else {
    setTimeout(function () {
        f_initcondition(cid);
    }, 500);
    }
}

function f_componentParam() {
    var dc = $("div[id^=component_]");
    var param = ";";
    dc.each(function(idx, item) {
        var i = 0;
        var ctype = this.id.split('_')[2];
        if (ctype == "treeview") {
            //  param += getTreeNodevalue(document.getElementById("nodepath").value) + ";";
        } else if (ctype == "querydatatable") {

        } else if (ctype == "pagrid") {

        }
    });
    return param;
}



