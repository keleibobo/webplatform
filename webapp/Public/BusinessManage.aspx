<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BusinessManage.aspx.cs" Inherits="Public_BusinessManage" %>
<%@ Register Src="../basepage/WebBusinessManageControl.ascx" TagName="WebBusinessManageControl" TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <link rel="stylesheet" href="../css/com.css" />
        <link rel="stylesheet" type="text/css" href="../basepage/themes/default/easyui.css" />
        <link rel="stylesheet" type="text/css" href="../basepage/themes/icon.css" />
     <style type="text/css">
        body {
            overflow:scroll;
            font-family: "Verdana","Arial", "Helvetica", "sans-serif","宋体" !important;
            scrollbar-shadow-color: #ffffff;
            scrollbar-highlight-color: #ffffff;
            scrollbar-3dlight-color: #709CCD;
            scrollbar-darkshadow-color: #709CCD;
            scrollbar-track-color: #A2BEDB;
            scrollbar-arrow-color: #426496;
            scrollbar-base-color: #CCDCF2;
           background-color: #A9DFE7;
             margin-left: 0px;
             margin-right: 0px;
             margin-bottom:1px;
            margin-top: 0px;

        }
         </style>

    <script src="../js/jquery-1.8.0.min.js"></script>
    <script type="text/javascript" src="../js/jquery.easyui.min.js"></script>
    <script type="text/javascript">
        var sperator = "-tnt_";
        function cbSelfCheck() {
            var o = window.document.getElementById("cbSelf");
            var o1 = window.document.getElementById("cblUser");
            if (o.checked) {
                o1.style.display = 'block';
            } else {
                o1.style.display = 'none';
            }
        }
        var duser = "";
        function ImportChuange(businessname) {

            if (businessname == null)
                businessname = $("#WebBusinessManageControl_businessname").val();
            $.ajax({
                url: "../basepage/HandlerExtendRights.ashx?type=getuserright&appname=" + $("#WebBusinessManageControl_lbTheAppName").val() + "&username=" + $("#WebBusinessManageControl_MatchUser").val() + "&businessname=" + $("#WebBusinessManageControl_businessname").val() + "&random=" + Math.random() * 10,
                type: 'get',
                success: function (data) {
                    val = data;
                    if (data != "") {
                        $('#dgRights').treegrid({
                            url: "../basepage/HandlerExtendRights.ashx",
                            queryParams: {
                                appname: $("#WebBusinessManageControl_lbTheAppName").val(),
                                businessname: businessname,
                                random: Math.random() * 10,
                                type: "businessright"
                            },
                            onLoadSuccess: function (row, data) {
                                var rightList = jQuery.parseJSON(val);
                                for (var k = 0; k < rightList.length; k++) {
                                    for (var i = 0; i < data.rows.length; i++) {
                                        if (rightList[k].NodeName == data.rows[i].name) {
                                                $("#view" + data.rows[i]._parentId).attr("checked", $("#view" + data.rows[i]._parentId)[0].checked || rightList[k].controlType == "1" ? true : false);
                                                $("#control" + data.rows[i]._parentId).attr("checked", $("#control" + data.rows[i]._parentId)[0].checked || rightList[k].viewType == "1" ? true : false);
                                                    $("#view" + data.rows[i].id).attr("checked", viewControl[0] == "1" ? true : false);
                                                    $("#control" + data.rows[i].id).attr("checked", viewControl[1] == "1" ? true : false);
                                            break;
                                        }
                                    }
                                }
                            }
                        });
                    }
                    else {
                        $('#dgRights').treegrid({
                            url: "../basepage/HandlerExtendRights.ashx",
                            queryParams: {
                                appname: $("#WebBusinessManageControl_lbTheAppName").val(),
                                businessname: businessname,
                                random: Math.random() * 10,
                                type: "businessright"
                            }
                        });
                    }
                }
            });
        }
        
        function getRightList(rows,array) {
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].children == null && ($("#view" + rows[i].id)[0].checked || $("#control" + rows[i].id)[0].checked)) {
                    var obj = {};
                    obj.stationname = rows[i].parname;
                    obj.nodename = rows[i].name;
                    var viewType = $("#view" + rows[i].id)[0].checked == true ? "1" : "0";
                    var controlType = $("#control" + rows[i].id)[0].checked == true ? "1" : "0";
                    obj.viewtype = viewType;
                    obj.controltype = controlType;
                    array.push(obj);
                }
                else if (rows[i].children != null && rows[i].children.length>0) {
                    getRightList(rows[i].children, array);
                }
            }
        }


        function Update() {
            operation = "update";
            var array = new Array();
            var rows = $('#dgRights').treegrid('getData');
            getRightList(rows, array);
            var aToStr = JSON.stringify(array);
            var url = "../basepage/HandlerExtendRights.ashx";
            $.ajax({
                url: url,
                type: 'post',
                data: { appname: $("#WebBusinessManageControl_lbTheAppName").val(), username: $("#WebBusinessManageControl_MatchUser").val(), rights: aToStr, type: "updateright", businessname: $("#WebBusinessManageControl_businessname").val(), random: Math.random() * 10 },
                success: function (data) {
                    if (data == "1") {
                        alert('更新成功！');
                    //    ImportChuange(null);
                    }
                    else {
                        alert('更新失败！');
                    }
                }
            });
        }

        function SelectAll(row,type,checked) {
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].children == null) {
                    $("#view" + rows[i].id).attr("checked", false);
                }
                else if (rows[i].children != null && rows[i].children.length > 0) {

                }
            }
        }

        function ViewselectAll()
        {
            var nodes = $('#dgRights').treegrid("getSelections");
            var rows = $('#dgRights').treegrid('getData');
            
        }


        function ControlselectAll() {
            var nodes = $('#dgRights').treegrid("getSelections");
            
        }

        function expandAll() {
            $('#dgRights').treegrid("expandAll");
        }

        function collapseAll() {
            $('#dgRights').treegrid("collapseAll");
        }


        function viewcheck(val, row) {
            if (val == "true") {
                return '<input id="view' + row.id + '" type="checkbox" value="1" checked="checked" onclick="viewclick(' + row.id + ')"  />';
            } else {
                return '<input id="view' + row.id + '" type="checkbox" value="1"  onclick="viewclick(' + row.id + ')" />';
            }
        }

        function viewclick(id) {
            var cnodes = $('#dgRights').treegrid("getChildren", id);
            var pnode = $('#dgRights').treegrid("getParent", id);
            var status = false;
            if ($("#view" + id)[0].checked == true) {
                status = true;
            }
            if (status) {
                $(cnodes).each(function () {
                    if ($(this)[0].children == null) {
                        $("#view" + $(this)[0].id).attr("checked", true);
                    }
                    else {
                        viewcheck($(this)[0].id);
                    }
                });
                if (pnode !=null) {
                    $("#view" + pnode.id).attr("checked", true);
                }
            }
            else {
                $(cnodes).each(function () {
                    if ($(this)[0].children == null) {
                        $("#control" + $(this)[0].id).attr("checked", false);
                        $("#view" + $(this)[0].id).attr("checked", false);
                    }
                    else {
                        viewcheck($(this)[0].id);
                    }
                });
                $("#control" + id).attr("checked", false);
            }
        }

        function controlcheck(val, row) {
            if (val == "true") {
                return '<input id="control' + row.id + '" type="checkbox" value="1" checked="checked" onclick="controlclick(' + row.id + ')" />';
            } else {
                return '<input id="control' + row.id + '" type="checkbox" value="1" onclick="controlclick(' + row.id + ')" />';
            }
        }

        function controlclick(id) {
            var nodes = $('#dgRights').treegrid("getChildren", id);
            var pnode = $('#dgRights').treegrid("getParent", id);
            var status = false;
            if ($("#control" + id)[0].checked == true) {
                status = true;
            }
            if (status) {
                $(nodes).each(function () {
                    if ($(this)[0].children == null) {
                        $("#control" + $(this)[0].id).attr("checked", true);
                        $("#view" + $(this)[0].id).attr("checked", true);
                    }
                    else {
                        controlclick($(this)[0].id);
                    }
                });
                if (pnode != null) {
                    $("#control" + pnode.id).attr("checked", true);
                    $("#view" + pnode.id).attr("checked", true);
                }
                $("#view" + id).attr("checked", true);
            }
            else {
                $(nodes).each(function () {
                    if ($(this)[0].children == null) {
                        $("#control" + $(this)[0].id).attr("checked", false);
                        $("#view" + $(this)[0].id).attr("checked", false);
                    }
                    else {
                        controlclick($(this)[0].id);
                    }
                });
                    $("#view" + id).attr("checked", false);
            }
        }
        // 点击复选框时触发事件
        function postBackByObject() {
            var o = window.event.srcElement;
            if (o.tagName == "INPUT" && o.type == "checkbox") {
                __doPostBack("", "");
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" style=" width:96%; margin:0 auto;">
    <div style="width:96%; margin:0 auto;" >
     <uc1:WebBusinessManageControl ID="WebBusinessManageControl" runat="server" />
    </div>
    </form>
</body>
</html>
