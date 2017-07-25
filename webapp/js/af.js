



function f_dbDevice(ctype) {
  
    var nodetype = $("#SelectNodeType").val();
    if (nodetype == ctype) {
        var nid = $("#SelectNode").val();
        var name = $("#TreeNodeText").val();
        var nodepath = $("#nodepath").val();
        var parentid;
        var patharray = nodepath.split("\\");
        if (patharray.length > 2) {
            parentid = patharray[1].substring(0, 1);
        }
        var dc = $("iframe");
        dc.each(function (idx, item) {
            $(item).contents().find("#nodeid").val(nid);
            $(item).contents().find("#nodename").val(name);
            $(item).contents().find("#parentid").val(parentid);
            $(item).contents().find("#treedbclick").trigger("click");
        });
    }
    else if (nodetype == "FZPTSvr.webcomm_Station") {
        var nid = $("#SelectNode").val();
        var name = $("#TreeNodeText").val();
        var nodepath = $("#nodepath").val();
        var parentid;
        var patharray = nodepath.split("\\");
        if (patharray.length >= 2) {
            parentid = patharray[1].substring(0, patharray[1].indexOf('('));
        }
        var dc = $("iframe");
        dc.each(function (idx, item) {
            $(item).contents().find("#nodeid").val(nid);
            $(item).contents().find("#nodename").val(name);
            $(item).contents().find("#parentid").val(parentid);
            $(item).contents().find("#stationdbclick").trigger("click");
        });
    }

}

//不刷新，仅获取导航节点
function f_treeclick(ctype) {

 

    var nodetype = $("#SelectNodeType").val();
    if (nodetype == ctype) {
        var nid = $("#SelectNode").val();
        var name = $("#TreeNodeText").val();
        var nodepath = $("#nodepath").val();
        var parentid;
        var patharray = nodepath.split("\\");
        if (patharray.length > 2) {
            parentid = patharray[1].substring(0, 1);
        }
        var dc = $("iframe");
        dc.each(function (idx, item) {
            $(item).contents().find("#nodeid").val(nid);
            $(item).contents().find("#nodename").val(name);
            $(item).contents().find("#parentid").val(parentid);
            $(item).contents().find("#treeclick").trigger("click");
        });
    }
}

function f_svgnode(ctype) {
       var nodetype = $("#SelectNodeType").val();
     if (nodetype == ctype) {
         loadsvg();
     }
}


function jsys_treeclick(ctype) {
    var tree = $(".easyui-tree");
    var selectedNode = $(tree[0]).tree('getSelected');
    if (selectedNode != null) {

    }
}