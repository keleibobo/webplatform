



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
/*
*时间 2017.7.26
*维护人员 李士群
*函数功能 如果点击的是导航树,会将数据库中的数据存储到前台界面中的隐藏控件中.
*/

function jsys_treeclick(ctype) {

    //var selectCamNodes = [];
    //var selectNum = 0;


    var tree = $(".easyui-tree");
    var selectedNode = $(tree[0]).tree('getSelected');
    if (selectedNode != null) {
        var dc = $("iframe");
        var nodeJson = JSON.stringify(selectedNode);
        $(dc[0]).contents().find("#nodedata").val(nodeJson);//将数据传送到前台隐藏空间中
        var nodeType = selectedNode.attributes.value.split('|')[2];//截取数据取第三个
        if (nodeType.indexOf('CAM') > -1) {
            $(dc[0]).contents().find("#nodeid").val(nodeType.split('_')[1]);
            $(dc[0]).contents().find("#treedbclick").trigger('click');//触发元素的单机事件


            //var nodeJson = JSON.stringify(selectedNode);
            //$(dc[0]).contents().find("#lsq_onenodedata").val(nodeJson);
            //$(dc[0]).contents().find("#lsq_one").trigger('click');

        }
        else {
            var selectCamNodes = [];//存储节点
            var mytree = $(tree[0]);
            var nodes = mytree.tree('getChildren', selectedNode.target);
            var j = 0;
            for (var i = 0; i < nodes.length; i++) {
                var type = nodes[i].attributes.value;
                if (type.indexOf('CAM') > -1) {
                    selectCamNodes[j] = nodes[i];
                    j++
                }

            }
            var nodeJson = JSON.stringify(selectCamNodes);
            $(dc[0]).contents().find("#lsqnodedata").val(nodeJson);
            $(dc[0]).contents().find("#lsq").trigger('click');
        }
       
    }
    
    else {
        return;
    }
    //if (selectedNode.id.indexOf('area') >= 0) {//3

    //    if (mytree.tree('isLeaf', selectedNode.target)) {
    //        selectNum = 0;
    //        // return;
    //    }
    //    else {
    //        // alert(selectedNode.id);
    //        path = selectedNode.id;
    //        var nodes = mytree.tree('getChildren', selectedNode.target);
    //        var camNodes = [];//保存的节点
    //        var j = 0;
    //        for (var i = 0; i < nodes.length; i++) {
    //            if (nodes[i].id.indexOf('cam') >= 0) {
    //                camNodes[j] = nodes[i];
    //                j++;
    //            }
    //        }
    //        //  alert(j);
    //        selectNum = j;
    //        selectCamNodes = camNodes;
    //    }
    //}
    //else {
    //    if (selectedNode.id.indexOf('station') >= 0) {//2

    //        if (mytree.tree('isLeaf', selectedNode.target))//判断是否是叶子节点 是,结束
    //        {
    //            selectNum = 0;
    //            //return;
    //        }
    //        else {
    //            var nodes = mytree.tree('getChildren', selectedNode.target);//获取所有节点
    //            var camNodes = [];//保存的的节点
    //            var sj = 0;
    //            for (var si = 0; si < nodes.length; si++) {
    //                if (nodes[si].id.indexOf('cam') >= 0) {
    //                    camNodes[sj] = nodes[si];
    //                    sj++;
    //                }
    //            }
    //            selectNum = sj;
    //            selectCamNodes = camNodes;
    //            //  alert(sj);
    //            var node3 = mytree.tree('getParent', selectedNode.target);
    //            path = node3.id + selectedNode.id;
    //        }
    //    } else {//1
    //        var camNodes = [];//保存的的节点
    //        camNodes[0] = selectedNode;
    //        selectNum = 1;
    //        selectCamNodes = camNodes;
    //        //var node2 = mytree.tree('getParent', selectedNode.target);
    //        //var node3 = mytree.tree('getParent', node2.target);
    //        //path=node3.id+node2.id+selectedNode.id;
    //    }

    //}


    //alert(selectCamNodes[0].text);
}

function f_svgnode(ctype) {
       var nodetype = $("#SelectNodeType").val();
     if (nodetype == ctype) {
         loadsvg();
     }
}

