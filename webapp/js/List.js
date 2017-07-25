
function f_scroll()//滚动
{
    if (typeof (document.all.jerker) == "undefined")
        return false
    if (document.all.jerker.length >= 0) {
        jerker[0].style.top = jerker[0].offsetParent.scrollTop - 2//document.body.scrollTop    

    }
}
if (document.getElementById("stype").value == "OrderlyPowerConsumption") {
    f_allid();
}
window.onload = function () {
    if (parent.document.getElementById('btnModify') != null) {
        parent.document.getElementById('btnModify').disabled = true;
        parent.document.getElementById('ImgModify').src = '../images/xgright_gray.gif';
    }
    if (parent.document.getElementById('btnDelete') != null) {
        parent.document.getElementById('btnDelete').disabled = true;
        parent.document.getElementById('ImgDelete').src = '../images/shanchu_gray.gif';
    }
    if (parent.document.getElementById('btnAddCopy') != null) {
        parent.document.getElementById('btnAddCopy').disabled = true;
        parent.document.getElementById('ImgAddCopy').src = '../images/zengjiafz_gray.gif';
    }
    if (parent.document.getElementById("btnFB") != null) {
        parent.document.getElementById("btnFB").disabled = true;
    }
    if (parent.document.getElementById("btnPreview") != null) {
        parent.document.getElementById("btnPreview").disabled = true;
    }
    if (parent.document.getElementById("SumPage") != null) {
        //不分页
        if (document.getElementById("SumPage").value == "") {
            if (parent.document.getElementById("pagetr") != null)
                parent.document.getElementById("pagetr").style.display = "none";

        }
        else {
            if (parent.document.getElementById("spansumpage") != null) {
                parent.document.getElementById("spansumpage").innerText = document.getElementById("SumPage").value;
                parent.document.getElementById("spanpagenum").innerText = document.getElementById("CurrentPage").value;
                parent.document.getElementById("SumPage").value = document.getElementById("SumPage").value;
                //  parent.document.getElementById("PageInfo").value = document.getElementById("CurrentPage").value ;
                if (parent.f_pageCheck != null) parent.f_pageCheck(document.getElementById("CurrentPage").value);
                document.getElementById("tableBody").style.width = "100%";
            }
        }
    }
    if (parseInt(document.getElementById("CurrentPage").value) > parseInt(document.getElementById("SumPage").value))
        return;
    if (parent.document.getElementById("picframe") != null) {
        var username = document.getElementById("username").value;
        var pwd = document.getElementById("pwd").value;
        var parm = document.getElementById("flashparm").value;
        var obj = parent.document.getElementById("picframe");
        var screenwidth = document.body.clientWidth;    //屏幕宽度
        var screenheight = document.body.clientHeight;  //屏幕高度
        var tablewidth = screenwidth - 5;
        var iw = 1300;
        var ih = 330;

        var w = screenwidth * iw / 1276 - 50;
        if (parseInt(screenheight) < 300) {
            screenheight = 300;
        }
        var h = screenheight * parseInt(ih) / 318 - 150;
        if (h < 300) h = 300;
        // alert(parent.document.getElementById("flashparm").value+"|"+ parm);//

        //查询条件不变,不刷新
        //  if (parent.document.getElementById("flashparm").value != parm)
        f_flash(w, h, username, pwd, parm, obj);
        //  if (parent.document.getElementById("flashparm") != null) parent.document.getElementById("flashparm").value = parm;
    }

    if (document.getElementById("spanrightmenu") != null) {
        var MM = new csMenu(document.getElementById("spanrightmenu"), document.getElementById("Menu1"));
    }
}
//单击右键隐藏字段
function f_hidetd() {

    var fieldname = document.getElementById("Hidden1").value;
    var objFieldName = eval("td" + fieldname);
    document.getElementById("Menu1").style.display = "none";
    //  document.getElementById("Menu1_iframe").style.display = "none";
    var ilen = objFieldName.length;
    if (typeof (ilen) == "undefined") {
        objFieldName.style.display = "none";
        return;
    }

    for (var i = 0; i < ilen; i++) {
        objFieldName[i].style.display = "none";
    }
}

//选中一行数据，对按钮变化
var CurrentId = "0";
function f_choose(currentrow) {

    var trd = $("#table  tr");

    //遍历所有的行，移除class:selected
    $.each(trd, function (i, n) {
        $(n).removeClass("selecttr");
        if (i % 2 == 0) {
            $(n).addClass("t1");
        } else {
            $(n).addClass("t0");
        }
    });
}


function f_chooseUserRunPlan(currentrow, iUserRunPlanBegin, sType) {
    try {
        if (typeof (iPosition) != "undefined") {

            if (selectedClass == "t0")
                jerker[iPosition].className = "t0";
            else
                jerker[iPosition].className = "t1";
        }
    }
    catch (e) {
    }
    iPosition =iUserRunPlanBegin;
    selectedClass = currentrow.className;
    currentrow.className = "selecttr";
    parent.document.getElementById('stype').value = sType;
    if (parent.document.getElementById('btnModify') != null) {
        parent.document.getElementById('btnModify').disabled = false;
        parent.document.getElementById('ImgModify').src = '../images/xgright.gif';
    }
    if (parent.document.getElementById('btnDelete') != null) {
        parent.document.getElementById('btnDelete').disabled = false;
        parent.document.getElementById('ImgDelete').src = '../images/shanchu.gif';
    }
    if (parent.document.getElementById('btnAddCopy') != null) {
        parent.document.getElementById('btnAddCopy').disabled = false;
        parent.document.getElementById('ImgAddCopy').src = '../images/zengjiafz.gif';
    }
    if (parent.document.getElementById("CurrentId") != null) {
        parent.document.getElementById("CurrentId").value = currentrow.getAttribute("rowid");
        parent.document.getElementById("KeyFieldName").value = currentrow.getAttribute("KeyFieldName");
    }

    if (parent.document.getElementById("btnFB") != null) {
        parent.document.getElementById("btnFB").disabled = false;
    }
    if (parent.document.getElementById("btnPreview") != null) {
        parent.document.getElementById("btnPreview").disabled = false;
    }
}

function csMenu(_object, _menu) {
    this.IEventHander = null;
    this.IFrameHander = null;
    this.IContextMenuHander = null;

    this.Show = function (_menu) {

        var e = window.event || event;
        if (e.button == 2) {
            if (window.document.all) {
                this.IContextMenuHander = function () { return false; };
                document.attachEvent("oncontextmenu", this.IContextMenuHander);
            }
            else {
                this.IContextMenuHander = document.oncontextmenu;
                document.oncontextmenu = function () { return false; };
            }

            window.csMenu$Object = this;
            this.IEventHander = function () { window.csMenu$Object.Hide(_menu); };

            if (window.document.all)
                document.attachEvent("onmousedown", this.IEventHander);
            else
                document.addEventListener("mousedown", this.IEventHander, false);

            _menu.style.left = document.body.scrollLeft + e.clientX;
            _menu.style.top = e.clientY;
            _menu.style.display = "";

            if (this.IFrameHander) {
                var _iframe = document.getElementById(this.IFrameHander);
                _iframe.style.left = e.clientX;
                _iframe.style.top = e.clientY;
                _iframe.style.height = _menu.offsetHeight;
                _iframe.style.width = _menu.offsetWidth;
                _iframe.style.display = "";
            }
        }
    };

    this.Hide = function (_menu) {
        var e = window.event || event;


        var _element = e.srcElement;
        //  alert(_element.tagName + "|" + _menu.tagName + "|" + (_element == _menu));
        do {
            // alert((_element == _menu))
            if (_element == _menu) {
                //     alert(1);
                return false;
            }
        }
        while ((_element = _element.offsetParent));

        if (window.document.all)
            document.detachEvent("on" + e.type, this.IEventHander);
        else
            document.removeEventListener(e.type, this.IEventHander, false);

        if (this.IFrameHander) {
            var _iframe = document.getElementById(this.IFrameHander);
            _iframe.style.display = "none";
        }

        _menu.style.display = "none";

        if (window.document.all)
            document.detachEvent("oncontextmenu", this.IContextMenuHander);
        else
            document.oncontextmenu = this.IContextMenuHander;

    };

    this.initialize = function (_object, _menu) {
        window._csMenu$Object = this;
        var _eventHander = function () { window._csMenu$Object.Show(_menu); };

        if(_menu==null){return}
        _menu.style.position = "absolute";
        _menu.style.display = "none";
        _menu.style.zIndex = "1000000";

        if (window.document.all) {
            //  var _iframe = document.createElement('iframe');
            //    document.body.insertBefore(_iframe, document.body.firstChild);
            //                 _iframe.id = _menu.id + "_iframe";
            //                 this.IFrameHander = _iframe.id;

            //                 _iframe.style.position = "absolute";
            //                 _iframe.style.display = "none";
            //                 _iframe.style.zIndex = "999999";
            //                 _iframe.style.border = "0px";
            //                 _iframe.style.height = "0px";
            //                 _iframe.style.width = "0px";

            _object.attachEvent("onmouseup", _eventHander);
        }
        else {
            _object.addEventListener("mouseup", _eventHander, false);
        }
    };

    this.initialize(_object, _menu);
}

function f_getName(filedName) {
    document.getElementById("Hidden1").value = filedName;
    if (filedName == "") return;
    document.getElementById("Menu1").innerHTML = "隐藏字段[<b>" + filedName + "</b>]";
}
//遍历有序用电
//var AllId;
function f_allid() {
    var formlen = window.document.forms(0).all.length;
    var AllId = "";
    for (i = 0; i < formlen; i++) {
        //下拉列表框检查
        if (window.document.forms(0).all(i).type == "checkbox") {
            if (window.document.forms(0).all(i).checked == false) AllId += window.document.forms(0).all(i).id.replace("id", "") + ",";
        }
    }
    return AllId;
}
//有序用电单击Checkbox(限电操作)
function checkbox(obj) {
    //    var objid = obj.id;
    //    var objNone = eval(objid + "none");
    //    var objInline = eval(objid + "inline");
    //    var xd = 0;
    //    if (!obj.checked) {
    //        xd = 0;
    //        objNone.style.display = "none";
    //        objInline.style.display = "";
    //        AllId += objid.replace("id", "") + ",";
    //    }
    //    else {
    //        xd = 1;
    //        objNone.style.display = "";
    //        objInline.style.display = "none";
    //        AllId = AllId.replace(objid.replace("id", "") + ",", "");
    //    }
    AllId = f_allid();
    var Flashid = AllId; // AllId;

    if (Flashid.length > 0) Flashid = Flashid.substring(0, AllId.length - 1);

    if (parent.document.frames["picframe"].document.getElementById("RData") != null)
        parent.document.frames["picframe"].document.getElementById("RData").orderlypower(Flashid);
}
thevalue = "";
//峰调谷
function fpgAdjustCheckbox(obj) {
    if (obj.checked) {

        thevalue += obj.value + ",";

    }
    else {
        if (thevalue.indexOf(",") >= 0)
            thevalue = thevalue.replace(obj.value + ",", "");
        else
            thevalue = thevalue.replace(obj.value, "");
    }
    document.getElementById("hiddenCheckbox").value = thevalue;

}

//月报管理安全用电单击查看显示Flash图片
function f_lookflash(flashparm) {
    var username = document.getElementById("username").value;
    var pwd = document.getElementById("pwd").value;
    var parm = flashparm;
    var obj = parent.document.getElementById("picframe");
    var screenwidth = document.body.clientWidth;    //屏幕宽度
    var screenheight = document.body.clientHeight;  //屏幕高度
    var tablewidth = screenwidth - 5;
    var iw = 1300;
    var ih = 330;

    var w = screenwidth * iw / 1276 - 50;
    if (parseInt(screenheight) < 300) {
        screenheight = 300;
    }
    var h = screenheight * parseInt(ih) / 318 - 150;
    if (h < 300) h = 300;
    f_flash(w, h, username, pwd, parm, obj);
}

 