var date_help = function (inp, divide) {
    this.inp = document.getElementById(inp);
    this.divide = divide;
    this.format = 'YYYY' + divide + 'MM' + divide + 'DD';

    this.load(); // 初始化
}
var sperator = "-UT_";
var sUploadFileName="";
date_help.prototype = {
    load: function () {

        // 加载输入框事件
        var oThis = this;


        this.inp.onblur = function () {
            oThis.inp_h.call(oThis);
        }
        this.inp.onkeypress = function (e) { //键盘按下事件
            e ? intKey = e.which : intKey = event.keyCode;
            return oThis.inp_press.call(oThis, intKey);
        }
        this.inp.onkeyup = function (e) {
            e ? intKey = e.which : intKey = event.keyCode;
            oThis.inp_chk.call(oThis, intKey);
        }

    },

    inp_h: function () { // 提示信息显示
        if (new RegExp('^\\d+' + this.divide + '\\d' + this.divide).test(this.inp.value)) { // 月数补0
            this.inp.value = this.inp.value.replace(new RegExp('^(\\d+' + this.divide + ')(\\d)(?=' + this.divide + ')'), '$10$2');
            this.parseDate();
            this.parseFormat();
        }
        if (new RegExp('^\\d+' + this.divide + '\\d+' + this.divide + '\\d$').test(this.inp.value)) { // 天数补0
            this.inp.value = this.inp.value.replace(/(\d)$/, '0$1');
            this.parseDate();
            this.parseFormat();
        }

    },
    inp_press: function (intKey) { //键键按下事件 只接受数字和分隔符
        return (new RegExp('[\\d' + this.divide + ']').test(String.fromCharCode(intKey)) || (intKey == 0 || intKey == 8));
    },
    inp_chk: function (intkey) { // 格式化日期及提示信息样式
        if (intKey == 37 || intKey == 39) return; // 左右键不检测
        this.parseDate(intKey); // 格式化日期
        this.parseFormat(); // 格式化提示信息
    },

    parseDate: function (intKey) { // 格式化日期
        var intDate = this.inp.value.replace(/\s/g, '');
        var divide = this.divide;
        var strDateArray = intDate.split(divide);
        strDateArray[0] ? intYear = strDateArray[0].replace(/^(\d*).*$/, '$1') : intYear = '';
        strDateArray[1] ? intMonth = strDateArray[1].replace(/^(\d*).*$/, '$1') : intMonth = '';
        strDateArray[2] ? intDay = strDateArray[2].replace(/^(\d*).*$/, '$1') : intDay = '';
        if ((intKey == 8 || intKey == 46) && (!strDateArray[0] || strDateArray[0].length < 5) && (!strDateArray[1] || strDateArray[1].length < 3) && (!strDateArray[2] || strDateArray[2].length < 3)) return intDate; // 退格键检测
        isNaN(intYear) ? intYear = '' : intYear = intYear.slice(0, 4);
        isNaN(intMonth) ? intMonth = '' : intMonth = intMonth.slice(0, 2);
        isNaN(intDay) ? intDay = '' : intDay = intDay.slice(0, 2);
        // 判断在修改阶段
        if (intYear.length < 4 && (intMonth != '' || intDay != '')) {
            if (strDateArray[1].length < 3 && strDateArray[2] < 3) return intDate;
        }
        if (intMonth.length < 2 && intDay != '') {
            if (strDateArray[0].length < 4 && strDateArray[2] < 3) return intDate;
        }
        if (intMonth.slice(0, 1) > 1) intMonth = '0' + intMonth.slice(0, 1);
        if (intMonth > 12) {
            var m = '0' + intMonth.slice(0, 1);
            var d = intMonth.slice(1, 2);
            intMonth = m;
            if (intDay == '') intDay = d;
        }
        var intM = Number(intMonth);
        if (intM == 1 || intM == 3 || intM == 5 || intM == 7 || intM == 8 || intM == 10 || intM == 12) {
            if (intDay.slice(0, 1) > 3) intDay = '0' + intDay.slice(0, 1);
            intDay > 31 ? intDay = intDay.slice(0, 1) : '';
        }
        if (intM == 4 || intM == 6 || intM == 9 || intM == 11) {
            if (intDay.slice(0, 1) > 3) intDay = '0' + intDay.slice(0, 1);
            parseInt(intDay) > 30 ? intDay = intDay.slice(0, 1) : '';
        }
        if (intM == 2) {
            boolLeapYear = false;
            if ((intYear % 100) === 0) {
                if ((intYear % 400) === 0) {
                    boolLeapYear = true;
                }
            } else {
                if ((intYear % 4) === 0) {
                    boolLeapYear = true;
                }
            }
            if (boolLeapYear) {
                if (intDay.slice(0, 1) > 2) intDay = '0' + intDay.slice(0, 1);
            } else {
                if (intDay.slice(0, 1) > 2) intDay = '0' + intDay.slice(0, 1);
                if (intDay > 28) intDay = intDay.slice(0, 1);
            }
        }
        temDate = '';
        if (intYear != '') intYear.length == 4 ? temDate = intYear + divide : temDate = intYear;
        if (intMonth != '') intMonth.length == 2 ? temDate = intYear + divide + intMonth + divide : new RegExp('\\d+' + divide + '\\d+' + divide).test(intDate) ? temDate = intYear + divide + '0' + intMonth + divide : temDate = intYear + divide + intMonth;
        if (intDay != '') temDate = intYear + divide + intMonth + divide + intDay;
        this.inp.value = temDate;
        return temDate;
    },

    parseFormat: function () { //格式化提示信息
        var intDate = this.inp.value;

        var format = this.format; // 提示字符
        var divide = this.divide;
        var uarr = intDate.split(divide), farr = format.split(divide) // 用户输入的字符分组
        var styleH = this.styleH, styleB = this.styleB;
        var y = farr[0], m = farr[1], d = farr[2]; // 年, 月, 日
        if (uarr != '') y = styleH + y.slice(0, uarr[0].length) + styleB + y.slice(uarr[0].length, y.length);
        if (uarr[1] != '' && uarr[1] != null) {
            m = styleH + divide + m.slice(0, uarr[1].length) + styleB + m.slice(uarr[1].length, m.length);
        } else {
            new RegExp('\\d*' + divide).test(intDate) ? m = styleH + divide + styleB + m : m = divide + m;
        }
        if (uarr[2] != '' && uarr[2] != null) {
            d = styleH + divide + d.slice(0, uarr[2].length) + styleB + d.slice(uarr[2].length, d.length);
        } else {
            new RegExp('\\d*?' + divide + '\\d*?' + divide).test(intDate) ? d = styleH + divide + styleB + d : d = divide + d;
        }

    }

}



//time
function isTime(str) {
    var a = str.match(/^(\d{0,2}):(\d{0,2}):(\d{0,2})$/);
    if (a == null) return false;
    if (a[1] >= 24 || a[2] >= 60 || a[3] >= 60) return false;
    return true;
}
function isDateTime(str) {
    var a = str.match(/^(\d{0,4})-(\d{0,2})-(\d{0,2}) (\d{0,2}):(\d{0,2}):(\d{0,2})$/);
    if (a == null) return false;
    if (a[2] >= 13 || a[3] >= 32 || a[4] >= 24 || a[5] >= 60 || a[6] >= 60) return false;
    return true;
}
function isDate(str) {
    var a = str.match(/^(\d{0,4})-(\d{0,2})-(\d{0,2})$/);
    if (a == null) return false;
    if (a[2] >= 13 || a[3] >= 32 || a[4] >= 24) return false;
    return true;
}
function validate(obj, type) {

    var range = obj.createTextRange();
    var text = range.text;
    var selrange = document.selection.createRange();
    var seltext = selrange.text;
    var startpos = 0, endpos = 0;
    while (selrange.compareEndPoints("StartToStart", range) > 0) {
        selrange.moveStart("character", -1);
        startpos++;
    }
    while (selrange.compareEndPoints("EndToStart", range) > 0) {
        selrange.moveEnd("character", -1);
        endpos++;
    }
    if (event.keyCode >= 48) {

        var keytext = String.fromCharCode(event.keyCode);

        text = text.substring(0, startpos) + keytext + text.substring(endpos, text.length);
    } else if (event.keyCode == 46) {//delete
        if (startpos == endpos) text = text.substring(0, startpos) + text.substring(startpos + 1, text.length);
        else text = text.substring(0, startpos) + text.substring(endpos, text.length);
    } else if (event.keyCode == 8) {
        if (startpos == endpos) text = text.substring(0, startpos - 1) + text.substring(startpos, text.length);
        else text = text.substring(0, startpos) + text.substring(endpos, text.length);
    }
    if (event.keyCode == 45) {
        event.returnValue = false;
        return;
    }
    var valid;
    switch (type) {
        case 1: valid = isDate(text); break;
        case 2: valid = isTime(text); break;
        case 3: valid = isDateTime(text); break;
        default: valid = false;
    }
    if (!valid) {
        event.returnValue = false;
    }
}
//下拉选择
function f_changeselect(fieldName, stable, obj) {
    //增加弹出窗口消息，选对谁用户
    //   alert(stable + "|" + fieldName)
    if (stable == 'T_Notice' && fieldName == 'C_OBJ_TYPE') {

        if (obj.value != "User") {
            f_unallchoose();
            trC_OBJ_Name.style.display = "none";

        }
        else {
            trC_OBJ_Name.style.display = "";
        }
    }
}

function f_choose(objsoucre, objdest) {
    try {
        if (objsoucre == null) objsoucre = document.getElementById("source");
        if (objdest == null) objdest = document.getElementById("dest");
        if (typeof (objsoucre) == "undefined")
            return false
        var i, j, badd
        for (i = objsoucre.length - 1; i >= 0; i--) {
            if (objsoucre.options[i].selected) {
                badd = true
                var obj = document.createElement("OPTION")
                obj.text = objsoucre.options[i].text
                obj.value = objsoucre.options[i].value
                for (j = 0; j < objdest.length; j++) {
                    if (objdest.options[j].value == obj.value)//已经选择了的项目就不再选择了
                    {

                        badd = false
                        break
                    }
                }
                if (badd) {
                    badd = false
                    for (j = 0; j < objdest.length; j++) {
                        //  if (obj.text<Form1.dest.options[j].text)不需要排序
                        {
                            objdest.add(obj, j);
                            badd = true
                            break;
                        }
                    }
                    if (!badd) objdest.add(obj)
                    badd = false
                    objsoucre.remove(i)
                }
            }
        }
    }
    catch (e) { }
}

//选择所有
function f_chooseall(sourceObj, destObj) {
    if (typeof (sourceObj) == "undefined") sourceObj = document.getElementById("source");
    if (typeof (sourceObj) == "undefined") sourceObj = document.all.source;
    if (typeof (destObj) == "undefined") destObj = document.getElementById("dest");
    if (typeof (destObj) == "undefined") destObj = document.all.dest;

    var i, j, badd
    for (i = sourceObj.length - 1; i >= 0; i--) {
        badd = true
        var obj = document.createElement("OPTION")
        obj.text = sourceObj.options[i].text
        obj.value = sourceObj.options[i].value
        for (j = 0; j < destObj.length; j++) {
            if (destObj.options[j].value == obj.value) {
                badd = false
                break
            }
        }
        if (badd) {
            badd = false
            for (j = 0; j < destObj.length; j++) {
                if (obj.text < destObj.options[j].text) {
                    destObj.add(obj, j);
                    badd = true
                    break;
                }
            }
            if (!badd) destObj.add(obj)
            sourceObj.remove(i)
        }
    }
}

function f_unchoose(objsoucre, objdest) {
    var i, j, badd
    if (objsoucre == null) objsoucre = document.all.source;
    if (objdest == null) objdest = document.all.dest;

    for (i = objdest.length - 1; i >= 0; i--) {
        if (objdest.options[i].selected) {
            var obj = document.createElement("OPTION")
            obj.text = objdest.options[i].text
            obj.value = objdest.options[i].value
            objsoucre.add(obj, i)
            objdest.remove(i)
        }
    }
}

//删除所有
function f_unallchoose(sourceObj, destObj) {
    if (typeof (sourceObj) == "undefined") sourceObj = document.getElementById("source");
    if (typeof (sourceObj) == "undefined") sourceObj = document.all.source;
    if (typeof (destObj) == "undefined") destObj = document.getElementById("dest");
    if (typeof (destObj) == "undefined") destObj = document.all.dest;
    var i, j, badd
    for (i = destObj.length - 1; i >= 0; i--) {
        var obj = document.createElement("OPTION")
        obj.text = destObj.options[i].text
        obj.value = destObj.options[i].value
        badd = false
        for (j = 0; j < sourceObj.length; j++) {
            if (sourceObj.options[j].value == obj.value)
                badd = true
        }
        if (!badd)
            sourceObj.add(obj, i)
        destObj.remove(i)
    }
}

//增加保存
function AddData() {

    if (!f_GetAddData()) return;
    if (document.getElementById('HiddenField2') != null) {
        if (document.getElementById('HFColName') != null) {
            nameArray[index] = document.getElementById('HFColName').value;
            valueArray[index] = document.getElementById('HiddenField2').value;
            index++;
        }
    }

    if (document.getElementById('HFSQL') != null && document.getElementById('HFSQL').value != '') {
        var list = document.getElementById('HFSQL').value.split(';');
        for (var i = 0; i < list.length; i++) {
            var itemArray = list[i].split('=');
            nameArray[index] = itemArray[0];
            valueArray[index] = itemArray[1];
            index++;
        }
    }

    PageMethods.AddNewRecord(document.getElementById('HiddenField1').value, nameArray, valueArray, sUploadFileName, function (result) {
        if (result == 1) {
            alert('保存成功');
            window.returnValue = 'true';
            $('#ModalWindow').window('close');
            window.f_query();
            window.f_reloading(); //刷新树
            form1.reset();
            handleload();
            if (document.getElementById('HiddenField1').value == "v_user" || document.getElementById('HiddenField1').value == "v_role")
                $.ajax({
                    url: '../basepage/Init.aspx',
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
            alert('超级管理员无法删除 ');
            window.returnValue = 'false';
        }
        else {
            alert('保存失败');
            window.returnValue = 'false';
        }
        // window.close();
    })
}


//增加保存
function SaveData() {

    if (!f_GetsaveData()) return;
    if (document.getElementById('HiddenField2') != null) {
        if (document.getElementById('HFColName') != null) {
            nameArray[index] = document.getElementById('HFColName').value;
            valueArray[index] = document.getElementById('HiddenField2').value;
            index++;
        }
    }

    if (document.getElementById('HFSQL') != null && document.getElementById('HFSQL').value != '') {
        var list = document.getElementById('HFSQL').value.split(';');
        for (var i = 0; i < list.length; i++) {
            var itemArray = list[i].split('=');
            nameArray[index] = itemArray[0];
            valueArray[index] = itemArray[1];
            index++;
        }
    }

    PageMethods.SaveNewRecord(document.getElementById('HiddenField1').value, nameArray, valueArray,sUploadFileName, function (result) {
        if (result == 1) {
            alert('保存成功');
            window.returnValue = 'true';
            $('#ModalWindow').window('close');
            window.f_query();
            window.f_reloading(); //刷新树
            form1.reset();
            handleload();
            if (document.getElementById('HiddenField1').value == "v_user" || document.getElementById('HiddenField1').value == "v_role")
                $.ajax({
                    url: '../basepage/Init.aspx',
                    success: function () {

                    }
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
            alert('超级管理员无法删除 ');
            window.returnValue = 'false';
        }
        else {
            alert('保存失败');
            window.returnValue = 'false';
        }
        // window.close();
    })
}
function InputNum() {
    if (event.keyCode < 48 || event.keyCode > 57) {
        alert('只能输入整数！');
        event.returnValue = false;
    }
}
function Inputfloat() {
    if (event.keyCode < 48 || event.keyCode > 57) {
        if (event.keyCode != 46)
            event.returnValue = false;
    }
}

function handlesaveover() {
    document.getElementById("btnsave").src = '../images/save_on.gif';
}
function handlesaveout() {
    document.getElementById("btnsave").src = '../images/save.gif';
}
function handlecancelover() {
    document.getElementById("btncancel").src = '../images/cancel_on.gif';
}
function handlecancelout() {
    document.getElementById("btncancel").src = '../images/cancel.gif';
}

function handleload() {

    if (document.getElementById("selectC_OBJ_TYPE")) f_changeselect('C_OBJ_TYPE', 'T_Notice', document.getElementById("selectC_OBJ_TYPE"));
    if (document.getElementsByTagName("input")) {
        var inputlist = document.getElementsByTagName("input");
        for (i = 0; i < inputlist.length; i++)
            if (inputlist[i].type == "text" && inputlist[i].defineName == "RecordPageType" && inputlist[i].isdate != null) {
                new date_help(inputlist[i].id, '-');
            }
    }
}
var nameArray = new Array();
var valueArray = new Array();

function f_GetAddData() {
    var inputlist = document.getElementsByTagName("input");
    nameArray = new Array();
    new Array();
    index = 0;
    var property = "";
    var pvalue = "";
    var start = 0
    var end = 0;
    for (i = 0; i < inputlist.length; i++) {
        if (inputlist[i].type == "text" && $("#" + inputlist[i].id).attr("defineName") == "RecordPageType") {
            if ($("#" + inputlist[i].id).attr("enableEmpty") == "False" && inputlist[i].value == "") {
                inputlist[i].focus();
                alert($("#" + inputlist[i].id).attr("stralert"));
                return false;
                break;
            }
            if (inputlist[i].id.indexOf("extendpro") == -1) {
                nameArray[index] = $("#" + inputlist[i].id).attr("sqlId");
                valueArray[index] = inputlist[i].value;
                index++;
            }
            else {
                var txt = $("#" + inputlist[i].id).val();
                end = start + txt.length - 1;
                property += $("#" + inputlist[i].id).attr("sqlId").replace("extendpro", "") + ":S:" + start + ":" + end + "-BO_";
                pvalue += txt;
                start = end + 1;
            }
        }
        else if (inputlist[i].type == "password" && $("#" + inputlist[i].id).attr("defineName") == "RecordPageType") {
            if ($("#" + inputlist[i].id).attr("enableEmpty") == "False" && inputlist[i].value == "") {
                inputlist[i].focus();
                alert($("#" + inputlist[i].id).attr("stralert"));
                return false;
                break;
            }
            if (document.getElementById("new" + $("#" + inputlist[i].id).attr("sqlId")).value.length < 6 || document.getElementById("repeat" + $("#" + inputlist[i].id).attr("sqlId")).value.length < 6 ) {
                inputlist[i].focus();
                alert("密码长度不能小于6位！");
                return false;
            }
            if ("new" + $("#" + inputlist[i].id).attr("sqlId") == inputlist[i].id) {
                var NewDom = document.getElementById("new" + $("#" + inputlist[i].id).attr("sqlId"));
                if (NewDom != null) {
                    var RepeatDom = document.getElementById("repeat" + $("#" + inputlist[i].id).attr("sqlId"));
                    if (NewDom.value != RepeatDom.value) {
                        inputlist[i].focus();
                        alert("密码输入不一致！");
                        return false;
                    }
                    else {
                        nameArray[index] = $(NewDom).attr("sqlId");
                        valueArray[index] = hex_md5(NewDom.value);
                        index++;
                    }
                }
            }
        }
        else if (inputlist[i].type == "checkbox" && $("#" + inputlist[i].id).attr("defineName") == "RecordPageType") {
            if (inputlist[i].id.indexOf("ck") != -1) {
                nameArray[index] = $("#" + inputlist[i].id).attr("sqlId");
                valueArray[index] = $("#" + inputlist[i].id)[0].checked;
                index++;
            }
        }
    }

    var selectlist = document.getElementsByTagName("select");
    for (i = 0; i < selectlist.length; i++) {
        if (selectlist[i].selectedIndex == -1 && selectlist[i].multiple == false) {
            alert("下拉列表的不能为空！");
            return false;
            break;
        }
        else if ($("#" + selectlist[i].id).attr("class") != null && $("#" + selectlist[i].id).attr("class").indexOf("combobox") >= 0) {
            if ($("#" + selectlist[i].id).combobox('getText') == null || $("#" + selectlist[i].id).combobox('getText') == "") {
                alert("下拉列表的不能为空！");
                return false;
                break;
            }
        }
        if ($("#" + selectlist[i].id).attr("defineName") == "RecordPageType" && selectlist[i].selectedIndex != -1 && selectlist[i].id.indexOf("extendpro") == -1) {
            nameArray[index] = $("#" + selectlist[i].id).attr("sqlId");
            if ($("#" + selectlist[i].id).attr("class") != null && $("#" + selectlist[i].id).attr("class").indexOf("combobox") >= 0) {
                valueArray[index] = $("#" + selectlist[i].id).combobox('getText');
            }
            else {
                valueArray[index] = selectlist[i].options[selectlist[i].selectedIndex].value;
            }
            index++;
        }
        else if (selectlist[i].id.indexOf("extendpro") >= 0) {
            var txt = selectlist[i].options[selectlist[i].selectedIndex].value;
            end = start + txt.length - 1;
            property += selectlist[i].id.replace("select", "").replace("extendpro", "") + ":S:" + start + ":" + end + "-BO_";
            pvalue += txt;
            start = end + 1;
        }
    }
    var datagrid = $(".easyui-treegrid");
    for (i = 0; i < datagrid.length; i++) {
        var value = [];
        var data = $('#' + $(datagrid[i]).attr("id")).treegrid("getData");
        for (k = 0; k < data.length; k++) {
            if ($('#view' + data[k].id).attr("checked") != "checked") {
                value.push(data[k].ExtendStr);
            }
            if (data[k].children != null && data[k].children.length > 0) {
                getSelectValue(data[k].children, value);
            }
        }
        var valStr = "";
        for (l = 0; l < value.length; l++) {
            valStr += value[l] + ",";
        }
        var txt = valStr;
        end = start + txt.length - 1;
        property += $(datagrid[i]).attr("id").replace("extendpro", "") + ":S:" + start + ":" + end + "-BO_";
        pvalue += txt;
        start = end + 1;
    }

    var propertyvalue = property + "-UTTT_" + pvalue;
    nameArray[index] = "proValues";
    valueArray[index] = propertyvalue;
    index++;

    var allselectedvalue = "";
    var allselectedText = "";
    if (document.getElementById("dest")) {
        if (typeof (document.getElementById("dest")) != "undefined") {
            iLen = document.all.dest.length

            for (i = 0; i < iLen; i++) {
                allselectedvalue += document.all.dest.options[i].value + ";";
                allselectedText += document.all.dest.options[i].text + ";";
            }
            nameArray[index] = "C_OBJ_Name";
            valueArray[index] = allselectedText;
            index++;
            nameArray[index] = "C_OBJ_ID";
            valueArray[index] = allselectedvalue;
            index++;
        }

        if (document.getElementById("selectC_OBJ_TYPE") != null) {

            if (document.getElementById("selectC_OBJ_TYPE").value == "User" && allselectedvalue == "") {
                alert("请选择用户");
                return false;
            }
        }
    }
    return true;
}


function f_GetsaveData() {
    var inputlist = document.getElementsByTagName("input");
    nameArray = new Array();
    new Array();
    index = 0;
    var property = "";
    var pvalue = "";
    var start = 0
    var end = 0;
    for (i = 0; i < inputlist.length; i++) {
        if (inputlist[i].type == "text" && $("#" + inputlist[i].id).attr("defineName") == "RecordPageType") {
            if ($("#" + inputlist[i].id).attr("enableEmpty") == "False" && inputlist[i].value == "") {
                inputlist[i].focus();
                alert($("#" + inputlist[i].id).attr("stralert"));
                return false;
                break;
            }
            if (inputlist[i].id.indexOf("extendpro") == -1) {
                nameArray[index] = $("#" + inputlist[i].id).attr("sqlId");
                valueArray[index] = inputlist[i].value;
                index++;
            }
            else {
                var txt = $("#" + inputlist[i].id).val();
                end = start + txt.length - 1;
                property += $("#" + inputlist[i].id).attr("sqlId").replace("extendpro", "") + ":S:" + start + ":" + end + "-BO_";
                pvalue += txt;
                start = end + 1;
            }
        }
        else if (inputlist[i].type == "password" && $("#" + inputlist[i].id).attr("defineName") == "RecordPageType") {
            if ("new" + $("#" + inputlist[i].id).attr("sqlId") == inputlist[i].id) {
                var NewDom = document.getElementById("new" + $("#" + inputlist[i].id).attr("sqlId"));
                if (NewDom != null) {
                    var RepeatDom = document.getElementById("repeat" + $("#" + inputlist[i].id).attr("sqlId"));
                    if (NewDom.value != RepeatDom.value) {
                        inputlist[i].focus();
                        alert("密码输入不一致！");
                        return false;
                    }
                    else {
                        nameArray[index] =$(NewDom).attr("sqlId");
                        valueArray[index] = hex_md5(NewDom.value);
                        index++;
                    }
                }
            }
        }
    }

    var selectlist = document.getElementsByTagName("select");
    for (i = 0; i < selectlist.length; i++) {
        if (selectlist[i].selectedIndex == -1 && selectlist[i].multiple == false) {
            alert("下拉列表的不能为空！");
            return false;
            break;
        }
        if ($("#" + selectlist[i].id).attr("defineName") == "RecordPageType" && selectlist[i].selectedIndex != -1 && selectlist[i].id.indexOf("extendpro") == -1) {
            nameArray[index] = $("#" + selectlist[i].id).attr("sqlId");
            valueArray[index] = selectlist[i].options[selectlist[i].selectedIndex].value;
            index++;
        }
        else if (selectlist[i].id.indexOf("extendpro") >= 0) {
            var txt = selectlist[i].options[selectlist[i].selectedIndex].value;
            end = start + txt.length - 1;
            property += selectlist[i].id.replace("select", "").replace("extendpro", "") + ":S:" + start + ":" + end + "-BO_";
            pvalue += txt;
            start = end + 1;
        }
    }
    var datagrid = $(".easyui-treegrid");
    for (i = 0; i < datagrid.length; i++) {
        var value = [];
        var data = $('#' + $(datagrid[i]).attr("id")).treegrid("getData");
        for (k = 0; k < data.length; k++) {
            if ($('#view' + data[k].id).attr("checked") != "checked") {
                value.push(data[k].ExtendStr);
            }
            if (data[k].children != null && data[k].children.length > 0) {
                getSelectValue(data[k].children, value);
            }
        }
        var valStr = "";
        for (l = 0; l < value.length; l++) {
            valStr += value[l]+",";
        }
        var txt = valStr;
        end = start + txt.length - 1;
        property += $(datagrid[i]).attr("id").replace("extendpro", "") + ":S:" + start + ":" + end + "-BO_";
        pvalue += txt;
        start = end + 1;
    }


    var propertyvalue = property + "-UTTT_" + pvalue;
    nameArray[index] = "proValues";
    valueArray[index] = propertyvalue;
    index++;

    var allselectedvalue = "";
    var allselectedText = "";
    if (document.getElementById("dest")) {
        if (typeof (document.getElementById("dest")) != "undefined") {
            iLen = document.all.dest.length

            for (i = 0; i < iLen; i++) {
                allselectedvalue += document.all.dest.options[i].value + ";";
                allselectedText += document.all.dest.options[i].text + ";";
            }
            nameArray[index] = "C_OBJ_Name";
            valueArray[index] = allselectedText;
            index++;
            nameArray[index] = "C_OBJ_ID";
            valueArray[index] = allselectedvalue;
            index++;
        }

        if (document.getElementById("selectC_OBJ_TYPE") != null) {

            if (document.getElementById("selectC_OBJ_TYPE").value == "User" && allselectedvalue == "") {
                alert("请选择用户");
                return false;
            }
        }
    }
    return true;
}

function getSelectValue(rows,extendstr) {
    for (x = 0; x < rows.length; x++) {
        if ($('#view' + rows[x].id).attr("checked") != "checked") {
            extendstr.push(rows[x].ExtendStr);
        }
        if (rows[x].children != null && rows[x].children.length > 0) {
            getSelectValue(rows[x].children, extendstr);
        }
    }
}

function SaveModifyData() {
    if (!f_GetsaveData()) return;

    var smsg = "";
    if (document.getElementById("HiddenField1").value == "t_shirtpeak_target") {
        smsg = "必须删除这月的错峰预警记录才能修改指标，是否确定修改?";
    }

    if (smsg != "") {
        if (confirm(smsg)) {
            PageMethods.SaveToDB(document.getElementById('HiddenField1').value, document.getElementById('HiddenField2').value, nameArray, valueArray, sUploadFileName, function (result) {
                if (result) {
                    alert('保存成功');
                    window.returnValue = 'true';
                    opener.f_query();
                    opener.f_reloading(); //刷新树
                    window.close();
                }
                else {
                    alert('保存失败');
                    window.returnValue = 'false';
                }

            })
        }
    }
    else {
        PageMethods.SaveToDB(document.getElementById('HiddenField1').value, document.getElementById('HiddenField2').value, nameArray, valueArray, sUploadFileName, function (result) {
            if (result) {
                alert('保存成功');
                window.returnValue = 'true';
                opener.f_query();
                opener.f_reloading(); //刷新树
                window.close();
            }
            else {
                alert('保存失败');
                window.returnValue = 'false';
            }

        })
    }    
}

//将表单输入中的回车键置换为Tab键 
function utf_keydown() {
    if (event.keyCode == 13 && (event.srcElement.type == "text" || event.srcElement.type == "password" || event.srcElement.type == "select-one")) {
        event.keyCode = 9
    }
}

///////////////////////////////////////////文件上传成功后的回调函数///////////////////////////////////////////
        function fileUpload(sFileName) {
            alert('上传成功!' + sFileName);
            sUploadFileName = sFileName;
        }


        function addProperty(objId) {
            var find = false;
            if ($("#"+objId).val() != "" && $("#value").val() != "") {
                $("#proValues option").each(function () {
                    var txt = $(this).text(); //获取单个text
                    if (txt.indexOf($("#" + objId).val()) == 0) {
                        alert('属性名重复！');
                        find = true;
                        return false;
                    }
                });
                if (!find) {
                    var propertyvalue = $("#" + objId).val() + ":" + $("#value").val();
                    $("#proValues").append("<option value='" + $("#" + objId).val() + sperator + $("#value").val() + "'>" + propertyvalue + "</option>");
                }
            }
            else {
                alert('请输入属性和键值！');
            }
        }


        function delProperty() {
            var checkIndex = $("#proValues").get(0).selectedIndex;
            $("#proValues option:selected").remove();
        }


        function f_changeselect(obj, WebUrl, Method, ColumnName, dest) {
            if (dest.indexOf(';') == -1) {
                var dataparam = "";
                if (ColumnName.indexOf(";") > 0) {
                    var paramName = ColumnName.split(';');
                    for (var i = 0; i < paramName.length; i++) {
                        if (paramName[i] != "") {
                            dataparam += paramName[i] + "=" + $("#select" + paramName[i]).val() + ";";
                        }
                    }
                }
                else {
                    dataparam = ColumnName + "=" + $(obj).val();
                }
                $("#" + dest).empty();
                PageMethods.GetData(WebUrl, Method, dataparam, function (result) {
                    var res = result.split("|");
                    for (var i = 0; i < res.length - 1; i = i + 2) {
                        $("#" + dest).append("<option value='" + res[i] + "'>" + res[i + 1] + "</option>");
                    }
                });
            }
            else {
                if (dest.indexOf(";") > 0) {
                    var paramName = dest.split(';');
                    var methodparam = Method.split(';');
                    var destcolumn = null;
                    if (methodparam.length > 3) {
                        destcolumn = methodparam[2].split(sperator.toLocaleLowerCase());
                    }
                    for (var i = 0; i < paramName.length; i++) {
                        if (paramName[i] != "") {
                            if ($("#" + paramName[i]).attr("class")!=null && $("#" + paramName[i]).attr("class").indexOf("combobox") >= 0) {
                                $("#" + paramName[i]).combobox('clear');
                                $("#" + paramName[i]).combobox('loadData', []);
                            }
                            else {
                                $("#" + paramName[i]).empty();
                            }
                         //   $("#" + paramName[i]).trigger("onchange");
                            Method = methodparam[0] + ";" + methodparam[1] + ";" + destcolumn[i] + ";" + methodparam[3];
                            dataparam = ColumnName + "=" + $(obj).val();
                            PageMethods.GetDWData(WebUrl, Method, dataparam, paramName[i], function (result) {
                                var res = result.split("|");
                                if ($("#" + res[0]).attr("class") != null && $("#" + res[0]).attr("class").indexOf("combobox") >= 0) {
                                    var data = [];
                                    for (var k = 1; k < res.length - 1; k = k + 2) {
                                        data.push({ text: res[k + 1], value: res[k] });
                                    }
                                    $("#" + res[0]).combobox('loadData', data);
                                }
                                else {
                                    for (var k = 1; k < res.length - 1; k = k + 2) {
                                        $("#" + res[0]).append("<option value='" + res[k] + "'>" + res[k + 1] + "</option>");
                                    }
                                }
                            });
                        }
                    }
                }
            }
        }

        var hexcase = 0;
        var b64pad = "";
        var chrsz = 8;

        function hex_md5(s) { return binl2hex(core_md5(str2binl(s), s.length * chrsz)); }
        function b64_md5(s) { return binl2b64(core_md5(str2binl(s), s.length * chrsz)); }
        function hex_hmac_md5(key, data) { return binl2hex(core_hmac_md5(key, data)); }
        function b64_hmac_md5(key, data) { return binl2b64(core_hmac_md5(key, data)); }
        function calcMD5(s) { return binl2hex(core_md5(str2binl(s), s.length * chrsz)); }

        function md5_vm_test() {
            return hex_md5("abc") == "900150983cd24fb0d6963f7d28e17f72";
        }

        function core_md5(x, len) {

            x[len >> 5] |= 0x80 << ((len) % 32);
            x[(((len + 64) >>> 9) << 4) + 14] = len;
            var a = 1732584193;
            var b = -271733879;
            var c = -1732584194;
            var d = 271733878;
            for (var i = 0; i < x.length; i += 16) {
                var olda = a;
                var oldb = b;
                var oldc = c;
                var oldd = d;

                a = md5_ff(a, b, c, d, x[i + 0], 7, -680876936);
                d = md5_ff(d, a, b, c, x[i + 1], 12, -389564586);
                c = md5_ff(c, d, a, b, x[i + 2], 17, 606105819);
                b = md5_ff(b, c, d, a, x[i + 3], 22, -1044525330);
                a = md5_ff(a, b, c, d, x[i + 4], 7, -176418897);
                d = md5_ff(d, a, b, c, x[i + 5], 12, 1200080426);
                c = md5_ff(c, d, a, b, x[i + 6], 17, -1473231341);
                b = md5_ff(b, c, d, a, x[i + 7], 22, -45705983);
                a = md5_ff(a, b, c, d, x[i + 8], 7, 1770035416);
                d = md5_ff(d, a, b, c, x[i + 9], 12, -1958414417);
                c = md5_ff(c, d, a, b, x[i + 10], 17, -42063);
                b = md5_ff(b, c, d, a, x[i + 11], 22, -1990404162);
                a = md5_ff(a, b, c, d, x[i + 12], 7, 1804603682);
                d = md5_ff(d, a, b, c, x[i + 13], 12, -40341101);
                c = md5_ff(c, d, a, b, x[i + 14], 17, -1502002290);
                b = md5_ff(b, c, d, a, x[i + 15], 22, 1236535329);
                a = md5_gg(a, b, c, d, x[i + 1], 5, -165796510);
                d = md5_gg(d, a, b, c, x[i + 6], 9, -1069501632);
                c = md5_gg(c, d, a, b, x[i + 11], 14, 643717713);
                b = md5_gg(b, c, d, a, x[i + 0], 20, -373897302);
                a = md5_gg(a, b, c, d, x[i + 5], 5, -701558691);
                d = md5_gg(d, a, b, c, x[i + 10], 9, 38016083);
                c = md5_gg(c, d, a, b, x[i + 15], 14, -660478335);
                b = md5_gg(b, c, d, a, x[i + 4], 20, -405537848);
                a = md5_gg(a, b, c, d, x[i + 9], 5, 568446438);
                d = md5_gg(d, a, b, c, x[i + 14], 9, -1019803690);
                c = md5_gg(c, d, a, b, x[i + 3], 14, -187363961);
                b = md5_gg(b, c, d, a, x[i + 8], 20, 1163531501);
                a = md5_gg(a, b, c, d, x[i + 13], 5, -1444681467);
                d = md5_gg(d, a, b, c, x[i + 2], 9, -51403784);
                c = md5_gg(c, d, a, b, x[i + 7], 14, 1735328473);
                b = md5_gg(b, c, d, a, x[i + 12], 20, -1926607734);
                a = md5_hh(a, b, c, d, x[i + 5], 4, -378558);
                d = md5_hh(d, a, b, c, x[i + 8], 11, -2022574463);
                c = md5_hh(c, d, a, b, x[i + 11], 16, 1839030562);
                b = md5_hh(b, c, d, a, x[i + 14], 23, -35309556);
                a = md5_hh(a, b, c, d, x[i + 1], 4, -1530992060);
                d = md5_hh(d, a, b, c, x[i + 4], 11, 1272893353);
                c = md5_hh(c, d, a, b, x[i + 7], 16, -155497632);
                b = md5_hh(b, c, d, a, x[i + 10], 23, -1094730640);
                a = md5_hh(a, b, c, d, x[i + 13], 4, 681279174);
                d = md5_hh(d, a, b, c, x[i + 0], 11, -358537222);
                c = md5_hh(c, d, a, b, x[i + 3], 16, -722521979);
                b = md5_hh(b, c, d, a, x[i + 6], 23, 76029189);
                a = md5_hh(a, b, c, d, x[i + 9], 4, -640364487);
                d = md5_hh(d, a, b, c, x[i + 12], 11, -421815835);
                c = md5_hh(c, d, a, b, x[i + 15], 16, 530742520);
                b = md5_hh(b, c, d, a, x[i + 2], 23, -995338651);
                a = md5_ii(a, b, c, d, x[i + 0], 6, -198630844);
                d = md5_ii(d, a, b, c, x[i + 7], 10, 1126891415);
                c = md5_ii(c, d, a, b, x[i + 14], 15, -1416354905);
                b = md5_ii(b, c, d, a, x[i + 5], 21, -57434055);
                a = md5_ii(a, b, c, d, x[i + 12], 6, 1700485571);
                d = md5_ii(d, a, b, c, x[i + 3], 10, -1894986606);
                c = md5_ii(c, d, a, b, x[i + 10], 15, -1051523);
                b = md5_ii(b, c, d, a, x[i + 1], 21, -2054922799);
                a = md5_ii(a, b, c, d, x[i + 8], 6, 1873313359);
                d = md5_ii(d, a, b, c, x[i + 15], 10, -30611744);
                c = md5_ii(c, d, a, b, x[i + 6], 15, -1560198380);
                b = md5_ii(b, c, d, a, x[i + 13], 21, 1309151649);
                a = md5_ii(a, b, c, d, x[i + 4], 6, -145523070);
                d = md5_ii(d, a, b, c, x[i + 11], 10, -1120210379);
                c = md5_ii(c, d, a, b, x[i + 2], 15, 718787259);
                b = md5_ii(b, c, d, a, x[i + 9], 21, -343485551);

                a = safe_add(a, olda);
                b = safe_add(b, oldb);
                c = safe_add(c, oldc);
                d = safe_add(d, oldd);
            }
            return Array(a, b, c, d);

        }

        function md5_cmn(q, a, b, x, s, t) {
            return safe_add(bit_rol(safe_add(safe_add(a, q), safe_add(x, t)), s), b);
        }
        function md5_ff(a, b, c, d, x, s, t) {
            return md5_cmn((b & c) | ((~b) & d), a, b, x, s, t);
        }
        function md5_gg(a, b, c, d, x, s, t) {
            return md5_cmn((b & d) | (c & (~d)), a, b, x, s, t);
        }
        function md5_hh(a, b, c, d, x, s, t) {
            return md5_cmn(b ^ c ^ d, a, b, x, s, t);
        }
        function md5_ii(a, b, c, d, x, s, t) {
            return md5_cmn(c ^ (b | (~d)), a, b, x, s, t);
        }

        function core_hmac_md5(key, data) {
            var bkey = str2binl(key);
            if (bkey.length > 16) bkey = core_md5(bkey, key.length * chrsz);

            var ipad = Array(16), opad = Array(16);
            for (var i = 0; i < 16; i++) {
                ipad[i] = bkey[i] ^ 0x36363636;
                opad[i] = bkey[i] ^ 0x5C5C5C5C;
            }

            var hash = core_md5(ipad.concat(str2binl(data)), 512 + data.length * chrsz);
            return core_md5(opad.concat(hash), 512 + 128);
        }

        function safe_add(x, y) {
            var lsw = (x & 0xFFFF) + (y & 0xFFFF);
            var msw = (x >> 16) + (y >> 16) + (lsw >> 16);
            return (msw << 16) | (lsw & 0xFFFF);
        }

        function bit_rol(num, cnt) {
            return (num << cnt) | (num >>> (32 - cnt));
        }

        function str2binl(str) {
            var bin = Array();
            var mask = (1 << chrsz) - 1;
            for (var i = 0; i < str.length * chrsz; i += chrsz)
                bin[i >> 5] |= (str.charCodeAt(i / chrsz) & mask) << (i % 32);
            return bin;
        }

        function binl2hex(binarray) {
            var hex_tab = hexcase ? "0123456789ABCDEF" : "0123456789abcdef";
            var str = "";
            for (var i = 0; i < binarray.length * 4; i++) {
                str += hex_tab.charAt((binarray[i >> 2] >> ((i % 4) * 8 + 4)) & 0xF) +
                   hex_tab.charAt((binarray[i >> 2] >> ((i % 4) * 8)) & 0xF);
            }
            return str;
        }

        function binl2b64(binarray) {
            var tab = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
            var str = "";
            for (var i = 0; i < binarray.length * 4; i += 3) {
                var triplet = (((binarray[i >> 2] >> 8 * (i % 4)) & 0xFF) << 16)
                        | (((binarray[i + 1 >> 2] >> 8 * ((i + 1) % 4)) & 0xFF) << 8)
                        | ((binarray[i + 2 >> 2] >> 8 * ((i + 2) % 4)) & 0xFF);
                for (var j = 0; j < 4; j++) {
                    if (i * 8 + j * 6 > binarray.length * 32) str += b64pad;
                    else str += tab.charAt((triplet >> 6 * (3 - j)) & 0x3F);
                }
            }
            return str;
        }

        function viewcheck(val, row) {
            if (val == "true") {
                return '<input id="view' + row.id + '" type="checkbox" value="1" checked="checked" onclick="viewclick(' + row.id + ')"  />';
            } else {
                return '<input id="view' + row.id + '" type="checkbox" value="1"  onclick="viewclick(' + row.id + ')" />';
            }
        }

        var tGId = "";

        function viewclick(id) {
            $('#' + tGId).treegrid('unselectAll'); 	
            if (event && event.stopPropagation) {
                event.stopPropagation();
            }
            else {
                window.event.cancelBubble = true;
            }
            var children = $('#' + tGId).treegrid('getChildren', id);
            if ($('#view' + id).attr("checked") == "checked") {
                if (children != null && children.length > 0) {
                    SelectAll(children);
                }
            }
            else {
                if (children != null && children.length > 0) {
                    unSelectAll(children);
                }
            }
            var parent = $('#' + tGId).treegrid('getParent', id);
            if ($('#view' + id).attr("checked") == "checked") {
                if (parent != null) {
                    SelectAllParent(parent.id);
                }
            }
        }

        function treegridSelectAll(self ,treeGridId) {
            $('#' + self).unbind();
            tGId = treeGridId;
            $('#' + self).bind("click", function () { treegridSelect(self, treeGridId); });
            $('#' + treeGridId).treegrid({
                onLoadSuccess: function () {
                    if ($("#" + treeGridId).attr("provalue") != null) {
                        var stationStr = $("#" + treeGridId).attr("provalue");
                        var rows = $('#' + treeGridId).treegrid('getData');
                        for (i = 0; i < rows.length; i++) {
                            if (stationStr.indexOf(rows[i].ExtendStr + ",") < 0) {
                                $("#view" + rows[i].id).attr("checked", true);
                                if (rows[i].children != null && rows[i].children.length > 0) {
                                    checkChildren(rows[i].children, stationStr);
                                }
                            }
                        }
                    }
                    $('#' + self).unbind();
                    tGId = treeGridId;
                    $('#' + self).bind("click", function () { treegridSelect(self, treeGridId); });
                }
            });
        }

        function checkChildren(rows, stationStr) {
            for (x = 0; x < rows.length; x++) {
                if (stationStr.indexOf(rows[x].ExtendStr + ",") < 0) {
                    $("#view" + rows[x].id).attr("checked", true);
                    if (rows[x].children != null && rows[x].children.length > 0) {
                        checkChildren(rows[x].children, stationStr);
                    }
                }
            }
        }

        function treegridSelect(self, treeGridId) {
            if ($('#' + self).attr("checked") == "checked") {
                var rows = $('#' + treeGridId).treegrid('getData');
                for (i = 0; i < rows.length; i++) {
                    if (rows[i].children == null) {
                        $("#view" + rows[i].id).attr("checked", true);
                    }
                    else {
                        $("#view" + rows[i].id).attr("checked", true);
                        SelectAll(rows[i].children);
                    }
                }
                $('#' + treeGridId).treegrid('selectAll');
            }
            else {
                var rows = $('#' + treeGridId).treegrid('getData');
                for (i = 0; i < rows.length; i++) {
                    if (rows[i].children == null) {
                        $("#view" + rows[i].id).attr("checked", false);
                    }
                    else {
                        $("#view" + rows[i].id).attr("checked", false);
                        unSelectAll(rows[i].children);
                    }
                }
                $('#' + treeGridId).treegrid('unselectAll');
            }
        }

        function SelectAllParent(_parentId) {
            $("#view" + _parentId).attr("checked", true);
            var parent = $('#' + tGId).treegrid('getParent', _parentId);
            if (parent != null) {
                SelectAllParent(parent.id);
            }
        }

        function SelectAll(rows) {
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].children == null) {
                    $("#view" + rows[i].id).attr("checked", true);
                }
                else if (rows[i].children != null && rows[i].children.length > 0) {
                    $("#view" + rows[i].id).attr("checked", true);
                    SelectAll(rows[i].children);
                }
            }
        }

        function unSelectAll(rows) {
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].children == null) {
                    $("#view" + rows[i].id).attr("checked", false);
                }
                else if (rows[i].children != null && rows[i].children.length > 0) {
                    $("#view" + rows[i].id).attr("checked", false);
                    unSelectAll(rows[i].children);
                }
            }
        }