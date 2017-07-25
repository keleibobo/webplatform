

//子菜单移过
function Menu_HoverDynamic(item, subitem) {
    $(item).mouseover(function () {
        var lid = item.parentNode.parentNode.parentNode.id;
        var ib = lid.indexOf("Items");
        var pn = lid.substring(0, ib);
         document.getElementById(pn).style.background = "url(../images/blue/line005.gif)";
      //  $("#" + pn).addClass("menuback");
    })

    var node = (item.tagName.toLowerCase() == "td") ?
        item :
        item.cells[0];
    var data = Menu_GetData(item);
    if (!data) return;
    var nodeTable = WebForm_GetElementByTagName(node, "table");
    if (data.hoverClass) {
        nodeTable.hoverClass = data.hoverClass;
        WebForm_AppendToClassName(nodeTable, data.hoverClass);
    }
    node = nodeTable.rows[0].cells[0].childNodes[0];
    if (data.hoverHyperLinkClass) {
        node.hoverHyperLinkClass = data.hoverHyperLinkClass;
        WebForm_AppendToClassName(node, data.hoverHyperLinkClass);
    }
    if (data.disappearAfter >= 200) {
        __disappearAfter = data.disappearAfter;
    }

    Menu_Expand(node, data.horizontalOffset, data.verticalOffset);
    item.style.background = "#96DDFC";
    //第三级子菜单over时，它的上级颜色加载
    if (typeof (subitem) != "undefined") eval(subitem).style.background = "#96DDFC";

}


//主菜单移过时
function Menu_HoverStatic(item, isum) {
    for (i = 0; i < parseInt(isum); i++) {
        eval("Menu1n" + i).style.background = "";
    }
    item.style.background = "url(../images/blue/line005.gif)";
 //   $(item).addClass("menuback");

 //   $(item).css("background", "url(../images/line005.gif)");
    
    var node = Menu_HoverRoot(item);
    var data = Menu_GetData(item);
    if (!data) return;
    __disappearAfter = data.disappearAfter;
    Menu_Expand(node, data.horizontalOffset, data.verticalOffset);
}


function Menu_Unhover(item) {
    
    if (item.tagName.toLowerCase() != "td")//子菜单out时执行
        item.style.background = "";

    var lastmenu = document.getElementById("lastmenu").value;
    if (lastmenu != "") {

        if (document.getElementById(lastmenu).nodeName == "TR") {
            var lid = document.getElementById(lastmenu).parentNode.parentNode.parentNode.id;
            var ib = lid.indexOf("Items");
            lastmenu = lid.substring(0, ib);
          //  document.getElementById(lastmenu).style.background = "url(../images/line005.gif)";
        } else if (document.getElementById(lastmenu).nodeName == "TD") {
         
        }

        document.getElementById(lastmenu).style.background = "url(../images/blue/line005.gif)";
      //  $(lastmenu).addClass("menuback");
    }

    var m1 = $("td[id^=Menu1n]")
    $.each(m1, function (indx, item) {
        if (this.id != lastmenu) {
            document.getElementById(this.id).style.background = "";
            $(this.id).css("background","")
        } 
    });
      
   

    //item.tagName.toLowerCase()="td"主菜单 tr:子菜单
    var node = (item.tagName.toLowerCase() == "td") ?
        item :
        item.cells[0];
    var nodeTable = WebForm_GetElementByTagName(node, "table");
    if (nodeTable.hoverClass) {
        WebForm_RemoveClassName(nodeTable, nodeTable.hoverClass);
    }
    node = nodeTable.rows[0].cells[0].childNodes[0];
    if (node.hoverHyperLinkClass) {
        WebForm_RemoveClassName(node, node.hoverHyperLinkClass);
    }
    Menu_Collapse(node);
}
