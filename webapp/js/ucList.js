var param = "";//分页 事件额外参数
var maxck = 8; //最多多选 个数
function f_page(btnPage, ctrlIdDesc) {
    var cp = 0;
    //alert(document.getElementById('currentpage').value);
    //alert(document.getElementById('SumPage').value);
    if (btnPage == 1) {
        cp = "1";
    }
    else if (btnPage == 2 && parseInt(document.getElementById("currentpage" + ctrlIdDesc).value) > 1) {
        cp = parseInt(document.getElementById("currentpage" + ctrlIdDesc).value) - 1;
    }
    else if (btnPage == 3 && parseInt(document.getElementById("currentpage" + ctrlIdDesc).value) < parseInt(document.getElementById("SumPage" + ctrlIdDesc).value)) {

        cp = parseInt(document.getElementById("currentpage" + ctrlIdDesc).value) + 1;
    }
    else if (btnPage == 4)
        cp = document.getElementById("SumPage" + ctrlIdDesc).value;

    if (cp != document.getElementById("currentpage" + ctrlIdDesc).value) {
        if (btnPage == 0) {
            cp = document.getElementById("currentpage" + ctrlIdDesc).value;
            
            cp = parseInt(cp);
                if (cp > parseInt(document.getElementById("SumPage" + ctrlIdDesc).value)) {
                    cp = 0;
                }
           
        }

        if (cp == 0 ) return;
        document.getElementById("currentpage" + ctrlIdDesc).value = cp;
        document.getElementById("spanpagenum" + ctrlIdDesc).innerHTML = cp;
        f_pageCheck(ctrlIdDesc, cp);
        f_loadpage(ctrlIdDesc, cp, param);
     
     //   param = "";
       
    }
    return false;
}



window.onload = function () {
    //f_pageCheck(1);

    if (document.getElementById('btnModify') != null) {
        document.getElementById('btnModify').disabled = true;
  //      document.getElementById('ImgModify').src = '../images/xgright_gray.gif';
    }
    if (document.getElementById('btnDelete') != null) {
        document.getElementById('btnDelete').disabled = true;
   //     document.getElementById('ImgDelete').src = '../images/shanchu_gray.gif';
    }
    if (document.getElementById('btnAddCopy') != null) {
        document.getElementById('btnAddCopy').disabled = true;
   //     document.getElementById('ImgAddCopy').src = '../images/zengjiafz_gray.gif';
    }
}

function f_chooserow(ctrldestid, ctrloldid, rowid, para, currentrow) {

    if (document.getElementById(rowid) != null && document.getElementById(rowid) != "undefined") {

        if (document.getElementById(rowid).className = "t1") {
            document.getElementById(rowid).className = "t0"
        }
        else {
            document.getElementById(rowid).className = "t1"
        }

        document.getElementById(ctrloldid).value = rowid;
    }

    if (document.getElementById(ctrloldid) != null && document.getElementById(ctrloldid) != "undefined" && document.getElementById(ctrloldid).value!="") {
        if (document.getElementById(document.getElementById(ctrloldid).value).className = "t1") {
            document.getElementById(document.getElementById(ctrloldid).value).className = "t0"
        }
        else {
            document.getElementById(document.getElementById(ctrloldid).value).className = "t1"
        }
    }


   var trd = $("#" + ctrloldid + " tr");

    //currentrow.className = "selecttr";


    //遍历所有的行，移除class:selected
   $.each(trd, function (i, n) {
       if (i > 0) {
           $(n).removeClass("selecttr");
           if (i % 2 == 0) {
               $(n).addClass("t1");
           } else {
               $(n).addClass("t0");
           }
       } else {
         //  $(n).addClass("tdhead");
       }

   });
    //给当前行添加class:selected
    currentrow.className = "selecttr";

  //  f_openiframe(ctrldestid.split('_')[1], para);
    f_loadhtml(ctrldestid, para);

    lastcompoment(ctrldestid);

}


 function f_clickrow(ctrldestid, ctrloldid, rowid, para, currentrow) {

     if (document.getElementById(rowid) != null && document.getElementById(rowid) != "undefined") {

         if (document.getElementById(rowid).className = "t1") {
             document.getElementById(rowid).className = "t0"
         }
         else {
             document.getElementById(rowid).className = "t1"
         }

         document.getElementById(ctrloldid).value = rowid;
     }

     if (document.getElementById(ctrloldid) != null && document.getElementById(ctrloldid) != "undefined" && document.getElementById(ctrloldid).value != "") {
         if (document.getElementById(document.getElementById(ctrloldid).value).className = "t1") {
             document.getElementById(document.getElementById(ctrloldid).value).className = "t0"
         }
         else {
             document.getElementById(document.getElementById(ctrloldid).value).className = "t1"
         }
     }


     var trd = $("#" + ctrloldid + " tr");

     //currentrow.className = "selecttr";


     //遍历所有的行，移除class:selected
     $.each(trd, function (i, n) {
         if (i > 0) {
             $(n).removeClass("selecttr");
             if (i % 2 == 0) {
                 $(n).addClass("t1");
             } else {
                 $(n).addClass("t0");
             }
         } else {
             //  $(n).addClass("tdhead");
         }

     });
     //给当前行添加class:selected
     currentrow.className = "selecttr";

 }


 var checkparam = [];
 function f_getChecked(ctrloldid) {
     //  var checkparam = [];
     var trow = $("#" + ctrloldid).find("tr");
//
     trow.each(function (idx, item) {
         var i = 0;
         if (idx != 0) {
             var cbox = $(item.firstChild).find(" input[type='checkbox']")[0] //input[type="text"
             if (cbox != undefined) {
                 cbox.checked = false;
                 var onfunc = $(item).attr("onclick").split(',');
                 if (onfunc != null && onfunc.length > 4) {
                     var ckarg = onfunc[3].substring(1, onfunc[3].length - 1);
                     if (ckarg != "") {
                         for (var i = 0; i < checkparam.length; i++) {
                             if (checkparam[i] == ckarg) {
                                 cbox.checked = true;
                                 break;
                             }
                         }
                     }
                 }
             }
         }

     });

    

     var cparam = [];
     if (checkparam.length > 0)
         for (var i = 0; i < checkparam.length; i++) {
             var args = checkparam[i].split(";");
             if (args.length > 0) {
                 for (var j = 0; j < args.length; j++) {
                     var a = args[j].split("=");
                     if (cparam[a[0]]) {
                         cparam[a[0]] = cparam[a[0]] + "," + a[1]
                     } else {
                         cparam[a[0]] = a[1];
                     }
                 }
             }
         }

    var checkedarg = "";
     for (var p in cparam) {
         if (p != "" && cparam[p]) {
             checkedarg += "&" + p + "=" + cparam[p];
         }
     }
     checkedarg += ";";
     return checkedarg;
 }

 function f_addchecked(rowid,para) {
     var cbox = $("#" + rowid).find(" input[type='checkbox']")[0];
     var prevChecked = cbox.checked;
     if (cbox == undefined) return;
     if (rowid.split('_')[1] == "110") {
         var nodepath = $("#nodepath").val();
         if (appname =="joyoaffzpt" && !(nodepath.indexOf("1(FZPTSvr.syspowerdatatype)") > -1 || nodepath.indexOf("3(FZPTSvr.syspowerdatatype)") > -1)) {
             var ckList = $("#component_110_querydatatable").find("input[type='checkbox']");
             $.each(ckList, function (index, item) {
                 item.checked = false;
             });
             cbox.checked = prevChecked;
             checkparam.length = 0;
             checkparam.push(para);
             return;
         }

         if (appname == "joyoc2" && (nodepath.indexOf("2(C2Svr.syspowerdatatype)") > -1 || nodepath.indexOf("5(C2Svr.syspowerdatatype)") > -1)) {
             var ckList = $("#component_110_querydatatable").find("input[type='checkbox']");
             $.each(ckList, function (index, item) {
                 item.checked = false;
             });
             cbox.checked = prevChecked;
             checkparam.length = 0;
             checkparam.push(para);
             return;
         }
     }

     if (window.event && window.event.srcElement.type == "checkbox") {

     } else {

         cbox.checked = !cbox.checked;
     }
     if (cbox.checked == true) {
         //if (checkparam.length == 0) {
         // checkparam.push(para);
         // }
         var addcheck = true;
         for (var i = 0; i < checkparam.length; i++) {
             if (checkparam[i] == para) {
                 addcheck = false;
                 break;
             }
         }
         if (addcheck&&!ifmax(checkparam,maxck)) {
            
                 checkparam.push(para);
             
         }

     } else {
         for (var i = 0; i < checkparam.length; i++) {
             if (checkparam[i] == para) {
                 checkparam[i] = "";
             }
         }
     }
 }

 function f_showtable(sourceid, destid) {  //sourceid, 
     checkparam = [];
     f_loadlayout();
     if ($("div[id*=_splinechart]").length == 2) {
         $("div[id*=_splinechart]")[0].innerHTML="";
         $("div[id*=_splinechart]")[1].innerHTML = "";
     }
 }

 function f_changecombox(sourceid, targetid, clicktype) {  //
     var cid = sourceid;
     $("#" + cid).combobox({

         onChange: function (n, o) {
             // alert('change')
         },
         onSelect: function (n, o) {
             // alert('select');
             checkparam = [];
             f_loadpage(targetid, 1, "");
         }


     });
    
 }

 function f_showchart(ctrldestid, ctrloldid, rowid, para, currentrow) {
     if (currentrow.outerText == '') return;
     f_clickrow(ctrldestid, ctrloldid, rowid, para, currentrow);

     f_addchecked(rowid, para);
    
     //  realtime(ctrldestid, checkparam);
     f_loadingchart(ctrldestid, f_getChecked(ctrloldid));
    // f_loadchartrealtime(ctrldestid, checkparam);
     
 }

 function f_showdoublechart(ctrldestid, ctrloldid, rowid, para, currentrow) {
     if (currentrow.outerText == '') return;
     f_clickrow(ctrldestid, ctrloldid, rowid, para, currentrow);
     f_addchecked(rowid, para);
     f_loadingdoublechart(ctrldestid, f_getChecked(ctrloldid));
 }

 function f_loadpointable(ctrldestid, ctrloldid, rowid, para, currentrow) {

     //test
     checkparam = [];
     f_clickrow(ctrldestid, ctrloldid, rowid, para, currentrow)


     // param = f_getChecked(ctrloldid);
     param = para;
     f_loadpage(ctrldestid, 1, param);

     var dc = $("div[id^=component_]");
     dc.each(function (idx, item) {
         var split = this.id.split('_');
         var cid = split[1];
         var ctype = split[2];
         if (ctype.indexOf('chart') > -1) {
             var win = $.messager.progress({
                 title: '正在从服务端获取数据中...',
                 msg: ''
             });
             $("#" + this.id).hide();
             f_loadingchart(this.id, param);
         }
     });
     
    lastcompoment(ctrldestid);

    if (document.getElementById("weblayout")) {
        $('#weblayout').layout('resize');
        $('#weblayout').layout('panel', 'center').panel('resize', { width: $('#layoutcenter').width() + 1 });
    }

 }

 var initLoadDataTable = false;
 var triggerTimes = 0;
 function f_updatetable(ctrldestid, ctrloldid, rowid, para, currentrow) {
     f_clickrow(ctrldestid, ctrloldid, rowid, para, currentrow);
     if (!initLoadDataTable || triggerTimes == 1 ) {
         if (ctrldestid.indexOf(';') > 0) {
             var destid = ctrldestid.split(';');
             for (k = 0; k < destid.length; k++) {
                 if (destid[k] != "") {
                     f_loadpage(destid[k], 1, para);
                 }
             }
         }
         else {
             f_loadpage(ctrldestid, 1, para);
         }
         triggerTimes = 0;
     }
     else {
         if (timerTableId.indexOf(ctrloldid) > -1 && initLoadDataTable && triggerTimes == 0) {
             triggerTimes++;
             queryDataTableTimer(ctrloldid, para);
         }
     }
     initLoadDataTable = true;
 }

function ifmax(checkparam,max) {
    var size=0;
    for (var i = 0; i < checkparam.length; i++) {
        if (checkparam[i] != "") {
            size++;
            if (size >= max) return true;
        }
    }
    return false;
}

function f_checkedpointable(ctrldestid, ctrloldid, rowid, para, currentrow) {

    if (currentrow.outerText == '') return;

    f_clickrow(ctrldestid, ctrloldid, rowid, para, currentrow);

    f_addchecked(rowid, para);
     
    param = f_getChecked(ctrloldid);
    f_loadpage(ctrldestid, 1, param);

    var dc = $("div[id^=component_]");
    dc.each(function (idx, item) {
        var split = this.id.split('_');
        var cid = split[1];
        var ctype = split[2];
        if (ctype.indexOf('chart') > -1) {
            var win = $.messager.progress({
                title: '正在从服务端获取数据中...',
                msg: ''
            });
            f_loadingchart(this.id, param);
        }
    });

    lastcompoment(ctrldestid);


}

function lastcompoment(ctrldestid) {
  
    var lh = document.getElementById("layoutcenter").childNodes;
    var pid = ctrldestid.split('_')[1];
    var ph = 0;
    for (var i = 0; i < lh.length; i++) {

        if (lh[i].id.indexOf("easyui") > -1) {
            return;
            
        }
        if (lh[i].id != pid + "BaseSvr.BusinessComponentLayoutCall") {
            ph += $(lh[i]).height();
        }

    }

   // $("#" + ctrldestid).height($(window).height() - ph - 22); //标题大小


    $("#iframe_" + pid).height($(window).height() - ph - 22 - 42);
//    var pf = document.getElementById("iframe_" + pid).parentNode;
//    $("#iframe_" + pid).height($(pf).height());
}



function f_choosetr(currentrow, TableComponentID) {
    if (TableComponentID.indexOf("_170_") > 0 || TableComponentID.indexOf("_204_") > 0) {
        return;
    }
    var trd = $("#"+TableComponentID+" table  tr");
    document.getElementById("CurrentId").value = currentrow.rowIndex;
    document.getElementById("ComponentId").value = $(currentrow)[0].parentNode.parentNode.parentNode.id;
    $.each(trd, function (i, n) {
        if (n.rowIndex == currentrow.rowIndex) {
            $(n).addClass("selecttr");
        }
        else {
            if (i != 0) {
                $(n).removeClass("selecttr");
                if (i % 2 != 0) {
                    $(n).addClass("t0");
                }
                else {
                    $(n).addClass("t1");
                }
            }
        }
    });
    currentrow.className = "selecttr";
    if (document.getElementById('btnModify') != null) {
        document.getElementById('btnModify').disabled = false;
        ///    document.getElementById('ImgModify').src = '../images/xgright.gif';
        rechangeimg(document.getElementById('btnModify'));
    }
    if (document.getElementById('btnDelete') != null) {
        document.getElementById('btnDelete').disabled = false;
        ///    document.getElementById('ImgDelete').src = '../images/shanchu.gif';
        rechangeimg(document.getElementById('btnDelete'));
    }
    if (document.getElementById('btnAddCopy') != null) {
        document.getElementById('btnAddCopy').disabled = false;
        ///    document.getElementById('ImgAddCopy').src = '../images/zengjiafz.gif';
        rechangeimg(document.getElementById('btnAddCopy'));
    }
}

function f_pageCheck( ctrlIdDesc, ipage) {
    var ipage = parseInt(ipage);
    var style = $("#style").val();
    var iSumPage = 1;
    $("#spanpagenum" + ctrlIdDesc).text(ipage);
    $("#currentpage" + ctrlIdDesc).val(ipage);
    if (document.getElementById("SumPage" + ctrlIdDesc) != null) {

        iSumPage = parseInt(document.getElementById("SumPage" + ctrlIdDesc).value);
    }
    if (ipage == 1) {
        //  firsttd.disabled = true;
        document.getElementById("firstimg" + ctrlIdDesc).src = "../images/"+style+"/首页_gray.gif";
        document.getElementById("firsttd" + ctrlIdDesc).className = "page1";
        // pretd.disabled = true;
        document.getElementById("preimg" + ctrlIdDesc).src = "../images/"+style+"/上一页_gray.gif";
        document.getElementById("pretd" + ctrlIdDesc).className = "page1";

    }
    else {
        //  firsttd.disabled = false;
        document.getElementById("firstimg" + ctrlIdDesc).src = "../images/"+style+"/首页.gif";
        document.getElementById("firsttd" + ctrlIdDesc).className = "page";
        //   pretd.disabled = false;
        document.getElementById("preimg" + ctrlIdDesc).src = "../images/"+style+"/上一页.gif";
        document.getElementById("pretd" + ctrlIdDesc).className = "page";

    }
    if (ipage < iSumPage) {
        // nexttd.disabled = false;
        document.getElementById("nextimg" + ctrlIdDesc).src = "../images/"+style+"/下一页.gif";
        document.getElementById("nexttd" + ctrlIdDesc).className = "page";
        // endtd.disabled = false;
        document.getElementById("endimg" + ctrlIdDesc).src = "../images/"+style+"/末页.gif";
        document.getElementById("endtd" + ctrlIdDesc).className = "page";
    }
    else {
        // nexttd.disabled = true;
        document.getElementById("nextimg" + ctrlIdDesc).src = "../images/"+style+"/下一页_gray.gif";
        document.getElementById("nexttd" + ctrlIdDesc).className = "page1";
        //endtd.disabled = true;
        document.getElementById("endimg" + ctrlIdDesc).src = "../images/"+style+"/末页_gray.gif";
        document.getElementById("endtd" + ctrlIdDesc).className = "page1";
    }

}

function f_buttondisable() {
    if (document.getElementById('btnModify') != null) {
        document.getElementById('btnModify').disabled = false;
        ///    document.getElementById('ImgModify').src = '../images/xgright.gif';
        changegrayimg(document.getElementById('btnModify'));
    }
    if (document.getElementById('btnDelete') != null) {
        document.getElementById('btnDelete').disabled = false;
        ///    document.getElementById('ImgDelete').src = '../images/shanchu.gif';
        changegrayimg(document.getElementById('btnDelete'));
    }
    if (document.getElementById('btnAddCopy') != null) {
        document.getElementById('btnAddCopy').disabled = false;
        ///    document.getElementById('ImgAddCopy').src = '../images/zengjiafz.gif';
        changegrayimg(document.getElementById('btnAddCopy'));
    }
}

function addTableCursor(obj) {
    $(obj).css("cursor","pointer");
}

function removeTableCursor(obj) {
    $(obj).css("cursor", "default");
}


function lastdiv(divid,bottomcss) {
   if(bottomcss=="") return;
   var cid = divid.split("_")[1];
   var bcss = eval("("+bottomcss+")");
    //if (cid == 27 || cid ==31) 
    //$("#"+divid).css({'left':0,'bottom':'45px','position':'absolute','width':'100%'});

    $("#"+divid).css(bcss);

}



function SetPageNum(element) {
    var componentId = element.id.replace("_pagesize", "");
    var pageSize = $(element).val();
    $("#currentpage" + componentId).val("1");
    PageMethods.SetPageNum(componentId, pageSize, function (result) {
        f_loadpage(componentId, $("#currentpage" + componentId).val(), param);
    });
}


