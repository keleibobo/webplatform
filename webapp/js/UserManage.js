var screenwidth = screen.width     //屏幕宽度
var screenheight = screen.height   //屏幕高度
var roleId;
var sperator = "-UT_";
var userId;
var userName;
var toolbar = [{
    iconCls: 'icon-add',
    text: '添加用户',
    handler: function () {
        $('#UserName').val("");
        $('#Password1').val("");
        $('#Password2').val("");
        $('#email').val("");
        $('#desKey').val("");
        $('#MobilePIN').val("");
        $('#PasswordQuestion').val("");
        $('#PasswordAnswer').val("");
        $('#MobileAlias').val("");
        $('#value').val("");
        $('#proValues').text("");
        $('#addtips').text("");
        $("#PwdArea").show();
        $("#btnadd").css("display", "inline");
        $("#btnsave").css("display", "none");
        $('#UserWindow').window('open');
        $('#UserName').removeAttr("disabled");
        AppChuange();
    }
}]


var toolbarR = [{
    iconCls: 'icon-add',
    text: '添加角色',
    handler: function () {
        $('#w').window('open');
        $('#RoleName').val("");
        $('#Desc').val("");
        $("#IndexPage").val("");
        $("#AddRole").css("display", "inline");
        $("#UpdateRole").css("display", "none");
        $('#RoleName').removeAttr("disabled");
    }
}]

var toolbarA = [{
    iconCls: 'icon-add',
    text: '添加项目',
    handler: function () {
        $('#w').window('open');
        $('#AppNmae').val("");
    }
}]


function getData() {
    var opts = $('#dg').datagrid('options');
    PageMethods.GetUserList(opts.pageNumber, opts.pageSize, function (result) {
        var data = jQuery.parseJSON(result);
        $('#dg').datagrid('loadData', result);
    }, function (err) {
        alert("failed");
    });



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
    w_show = window.open(sUrl, "", "resizable=" + sResize + ",scrollbars=no,menubar=no,location=no,status=no,toolbar=no,width=" + iWidth + ",height=" + iHeight + ",left=" + l + ",top=" + t)
}

function deleteUser(ID) {
    if (window.confirm("确定要删除？")) {
        PageMethods.DeleteUser(ID.split(":")[0], ID.split(":")[1], function (result) {
            $.messager.alert('提示', result);
            $('#dg').datagrid('reload');
        }, function (err) {
            $.messager.alert('提示', err);
        });
    }
}

function modifyPwd(ID, UserName) {
    $('#w').window('open');
    $('#newPwd').val("");
    $('#newPwd2').val("");
    userId = ID;
    userName = UserName;
}
function UpdatePwd() {
    if ($('#newPwd').val().length < 6) {
        $('#tips').text("密码长度必须不小于6位！");
    }
    else if ($('#newPwd').val() != $('#newPwd2').val()) {
        $('#tips').text("2次密码输入不一致！");
    }
    else {
        var row = $('#dg').datagrid('getSelected');
        if (row == null) {
            $.messager.alert('提示', "请选择要修改的用户");
            return;
        }
        PageMethods.ModifyUserPwd(row.AppName, userId, userName, hex_md5($('#newPwd').val()), function (result) {
            $('#tips').text("");
            $.messager.alert('提示', result);
            $('#w').window('close');
            return;
        }, function (err) {
            $.messager.alert('提示', err);
        });
    }
}



function doSearch(value) {
    $('#dg').datagrid({
        url: 'GetUserData.aspx?type=ul',
        queryParams: {
            UserName: value
        }
    });
}

function addProperty() {
    var find = false;
    if ($("#PropertySelect").val() != "" && $("#value").val() != "") {
        $("#proValues option").each(function () {
            var txt = $(this).text(); //获取单个text
            if (txt.indexOf($("#PropertySelect").val()) == 0) {
                alert('属性名重复！');
                find = true;
                return false;
            }
        });
        if (!find) {
            var propertyvalue = $("#PropertySelect").val() + ":" + $("#value").val();
            $("#proValues").append("<option value='" + $("#PropertySelect").val() + "-UT_" + $("#value").val() + "'>" + propertyvalue + "</option>");
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

function AddRole() {
    if ($("#RoleName").val() == "") {
        $("#tips").text("请输入角色名");
        return;
    }
    else if ($("#IndexPage").val().indexOf("\\") >= 0) {
        $.messager.alert('提示', "请输入正确的首页地址");
        return;
    }
    else {
        $("#tips").text("");
        var RoleName = $("#RoleName").val();
        var Desc = $("#Desc").val();
        var AppNameDW = $("#AppNameDw").val();
        if (AppNameDW == null)
            AppNameDW = "";
        var IndexPage = $("#IndexPage").val();
        PageMethods.AddRole(RoleName, Desc, AppNameDW, IndexPage, function (result) {
            if (result == "redirect") {
                window.parent.Location = "default.aspx";
            }
            else {
                $.messager.alert('提示', result);
                $("#dg").datagrid("reload");
                $('#w').window('close');
            }
            return;
        }, function (err) {
            document.getElementById('tips').innerText = err;
        });
    }
}

function deleteRole(ID) {
    if (confirm("确定要删除吗？")) {
        PageMethods.DeleteRole(ID, function (result) {
            $.messager.alert('提示', result);
            $("#dg").datagrid("reload");
            return;
        }, function (err) {
            alert(err);
        });
    }
}

function updateRole(ID) {
    setTimeout(function () { getRoleInfo(ID); }, 100);
}

function getRoleInfo(ID) {
    var row = $('#dg').datagrid('getSelected');
    if (row) {
        roleId = ID;
        $('#RoleName').val("");
        $('#Desc').val("");
        $("#IndexPage").val("");
        $('#RoleName').val(row.RoleName);
        $('#Desc').val(row.Desc);
        $('#IndexPage').val(row.IndexPage);
        $("#AddRole").css("display", "none");
        $("#UpdateRole").css("display", "inline");
        $('#w').window('open');
        $('#RoleName').attr("disabled", "disabled");
    }
}

function UpdateRole() {
    if ($("#IndexPage").val().indexOf("\\") >= 0) {
        $.messager.alert('提示', "请输入正确的首页地址");
        return;
    }
    PageMethods.UpdateRole(roleId, $('#Desc').val(), $("#IndexPage").val(), function (result) {
        $.messager.alert('提示', result);
        $("#dg").datagrid("reload");
        $('#w').window('close');
        return;
    }, function (err) {
        $.messager.alert('提示', err);
    });
}

function AddUser() {
    if ($('#UserName').val() == "") {
        $('#addtips').text("请输入用户名！");
        return;
    }
    else if ($('#UserName').val().length < 6) {
        $('#addtips').text("用户名长度必须不小于6位！");
        return;
    }
    else if ($('#Password1').val().length < 6) {
        $('#addtips').text("密码长度必须不小于6位！");
        return;
    }
    else if ($('#Password1').val() != $('#Password2').val()) {
        $('#addtips').text("2次密码输入不一致！");
        return;
    }
    else if ($('#RoleType').css('display') == "inline" && $('#RoleType').val() == null) {
        $('#addtips').text("请选择所属角色！");
        return;
    }
    var UserName = document.getElementById('UserName').value;
    var appname = $('#AppNameDw').val();
    if (appname == null) {
        appname = "";
    }
    var pwd = hex_md5(document.getElementById('Password1').value);
    var role = document.getElementById('RoleType').value;
    var Email = document.getElementById('email').value;
    var deskey = document.getElementById('deskey').value;
    var MobilePIN = document.getElementById('MobilePIN').value;
    var PasswordQuestion = document.getElementById('PasswordQuestion').value;
    var PasswordAnswer = document.getElementById('PasswordAnswer').value;
    var MobileAlias = document.getElementById('MobileAlias').value;
    if (Email != "" && !valid_email(Email)) {
        $('#addtips').text("输入的EMail格式不正确");
        return;
    }

    var property = "";
    var pvalue = "";
    var start = 0
    var end = 0;
    $("#proValues option").each(function () {
        var txt = $(this).val(); //获取单个text
        end = start + txt.split(sperator)[1].length - 1;
        property += txt.split(sperator)[0] + ":S:" + start + ":" + end + "-BO_";
        pvalue += txt.split(sperator)[1];
        start = end + 1;
    });
    var propertyvalue = property + "-UTTT_" + pvalue;
    PageMethods.AddUser(UserName, pwd, appname, role, Email, deskey, MobilePIN, PasswordQuestion, PasswordAnswer, MobileAlias, propertyvalue, function (result) {
        $.messager.alert('提示', result);
        $("#dg").datagrid("reload");
        $('#UserName').val("");
        $('#Password1').val("");
        $('#Password2').val("");
        $('#email').val("");
        $('#desKey').val("");
        $('#MobilePIN').val("");
        $('#PasswordQuestion').val("");
        $('#PasswordAnswer').val("");
        $('#MobileAlias').val("");
        $('#value').val("");
        $('#proValues').text("");
        $('#addtips').text("");
        return;
    }, function (err) {
        $.messager.alert('提示', err);
    });
}

function ModifyInfo(ID) {
    userId = ID;
    $('#UserName').val("");
    $('#Password1').val("");
    $('#Password2').val("");
    $('#email').val("");
    $('#desKey').val("");
    $('#MobilePIN').val("");
    $('#PasswordQuestion').val("");
    $('#PasswordAnswer').val("");
    $('#MobileAlias').val("");
    $('#value').val("");
    $('#proValues').text("");
    $('#addtips').text("");
    $("#PwdArea").hide();
    $("#btnadd").css("display", "none");
    $("#btnsave").css("display", "inline");
    $("#UserWindow").window('open');
    $.ajax({
        type: "GET",
        url: "GetUserData.aspx?type=ui&UID=" + ID + "&number=" + Math.random(),
        success: function (data) {
            $('#proValues').text("");
            var jsond = jQuery.parseJSON(data);
            $('#UserName').val(jsond.Name);
            $('#UserName').attr("disabled", "disabled");
            var count = $("#AppNameDw option").length;
            for (var i = 0; i < count; i++) {
                if ($("#AppNameDw").get(0).options[i].text == jsond.AppName) {
                    $("#AppNameDw").get(0).options[i].selected = true;
                    $("#AppNameDw").trigger("onchange");
                    break;
                }
            }
            $('#email').val(jsond.Email);
            $('#RoleType').val(jsond.RoleName);
            $('#desKey').val(jsond.PasswordSalt);
            $('#MobilePIN').val(jsond.MobliePin);
            $('#PasswordQuestion').val(jsond.PasswordQuestion);
            $('#PasswordAnswer').val(jsond.PasswordAnswer);
            $('#MobileAlias').val(jsond.MobileAlias);
            var proValues = jsond.proValues.split('-UTTT_')[0].split('-BO_');
            var valueStr = jsond.proValues.split('-UTTT_')[1];
            for (var i = 0; i < proValues.length - 1; i++) {
                var valueName = proValues[i].split(':')[0];
                var startIndex = proValues[i].split(':')[2];
                var endIndex = proValues[i].split(':')[3];
                var value = valueStr.substr(startIndex, endIndex - startIndex + 1);
                $("#proValues").append("<option value='" + valueName + sperator + value + "'>" + valueName + ":" + value + "</option>");
            }
        },
        error: function (msg) {
            alert(msg);
        }
    });
}

function SaveInfo() {

    var UserName = document.getElementById('UserName').value;
    var pwd = hex_md5(document.getElementById('Password1').value);
    var role = document.getElementById('RoleType').value;
    var Email = document.getElementById('email').value;
    var deskey = document.getElementById('deskey').value;
    var MobilePIN = document.getElementById('MobilePIN').value;
    var PasswordQuestion = document.getElementById('PasswordQuestion').value;
    var PasswordAnswer = document.getElementById('PasswordAnswer').value;
    var MobileAlias = document.getElementById('MobileAlias').value;
    if (Email != "" && !valid_email(Email)) {
        $('#addtips').text("输入的EMail格式不正确");
        return;
    }

    var property = "";
    var pvalue = "";
    var start = 0
    var end = 0;
    $("#proValues option").each(function () {
        var txt = $(this).val(); //获取单个text
        end = start + txt.split(sperator)[1].length - 1;
        property += txt.split(sperator)[0] + ":S:" + start + ":" + end + "-BO_";
        pvalue += txt.split(sperator)[1];
        start = end + 1;
    });
    var propertyvalue = property + "-UTTT_" + pvalue;
    var row = $('#dg').datagrid('getSelected');
    if (row == null) {
        $.messager.alert('提示', "请选择要修改的用户");
        return;
    }
    PageMethods.UpdateUser(row.AppName, userId, role, Email, deskey, MobilePIN, PasswordQuestion, PasswordAnswer, MobileAlias, propertyvalue, function (result) {
        $.messager.alert('提示', result);
        $("#dg").datagrid("reload");
        $('#UserWindow').window('close');
        return;
    }, function (err) {
        $.messager.alert('提示', err);
    });
}

function valid_email(email) {
    var patten = new RegExp(/^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]+$/);
    return patten.test(email);
}

function SaveSelfInfo() {
    var Email = document.getElementById('email').value;
    if (Email != "" && !valid_email(Email)) {
        $('#addtips').text("输入的EMail格式不正确");
        return;
    }
    var MobilePIN = document.getElementById('MobilePIN').value;
    var PasswordQuestion = document.getElementById('PasswordQuestion').value;
    var PasswordAnswer = document.getElementById('PasswordAnswer').value;
    var MobileAlias = document.getElementById('MobileAlias').value;
    PageMethods.UpdateSelfInfo(Email, MobilePIN, PasswordQuestion, PasswordAnswer, MobileAlias, function (result) {
        $.messager.alert('提示', result);
        return;
    }, function (err) {
        $.messager.alert('提示', err);
    });
}

function MatchUser() {
    var UserRow = $('#UserList').datagrid('getSelected');
    var OutUserrow = $('#OutUserList').datagrid('getSelected');
    if (!UserRow) {
        $.messager.alert('提示', '请选择要映射的系统用户！');
        return;
    }
    else if (!OutUserrow) {
        $.messager.alert('提示', '请选择要映射的接入用户！');
        return;
    }
    PageMethods.MatchUser(OutUserrow.UserId, UserRow.UserId, function (result) {
        $.messager.alert('提示', result);
        $("#OutUserList").datagrid("reload");
        $("#MatchInfo").datagrid("reload");
        return;
    }, function (err) {
        $.messager.alert('提示', err);
    });
}

function DeleteMatchInfo() {
    var MatchInforow = $('#MatchInfo').datagrid('getSelected');
    if (!MatchInforow) {
        $.messager.alert('提示', '请选择要删除的映射用户！');
        return;
    }
    PageMethods.DeleteMatchInfo(MatchInforow.OutUserId, function (result) {
        $.messager.alert('提示', result);
        $("#OutUserList").datagrid("reload");
        $("#MatchInfo").datagrid("reload");
        return;
    }, function (err) {
        $.messager.alert('提示', err);
    });
}

function ModifyActive(v) {
    var val = v.split(':');
    if (val[2] == "激活") {
        val[2] = "1";
    }
    else {
        val[2] = "0";
    }
    PageMethods.UpdateActive(val[0], val[1], val[2], function (result) {
        $.messager.alert('提示', result);
        $("#dg").datagrid("reload");
        return;
    }, function (err) {
        $.messager.alert('提示', err);
    });
}

function ModifyLocked(v) {
    var val = v.split(':');
    if (val[2] == "锁定") {
        val[2] = "1";
    }
    else {
        val[2] = "0";
    }
    PageMethods.UpdateLocked(val[0], val[1], val[2], function (result) {
        $.messager.alert('提示', result);
        $("#dg").datagrid("reload");
        return;
    }, function (err) {
        $.messager.alert('提示', err);
    });
}


function SaveConfigInfo() {
    if ($("#EncName").val() == "") {
        $.messager.alert('提示', "请输入加密方法名！");
        return;
    }
    if ($("#DesName").val() == "") {
        $.messager.alert('提示', "请输入解密方法名！");
        return;
    }
    if ($("#Namespace").val() == "") {
        $.messager.alert('提示', "请输入命名空间！");
        return;
    }
    if ($("#DLLName").val() == "") {
        $.messager.alert('提示', "请上传加密DLL！");
        return;
    }
    var Description = document.getElementById('Description').value;
    var EncType = document.getElementById('EncType').value;
    var EncName = document.getElementById('EncName').value;
    var DesName = document.getElementById('DesName').value;
    var DesKey = document.getElementById('DesKey').value;
    var Namespace = document.getElementById('Namespace').value;
    var DLLName = document.getElementById('DLLName').value;
    PageMethods.SaveConfigInfo(Description, EncType, EncName, DesName, DesKey, Namespace, DLLName, function (result) {
        $.messager.alert('提示', result);
        return;
    }, function (err) {
        $.messager.alert('提示', err);
    });
}

function pwdonblur() {
    if ($('#NEWPWD').val().length < 6) {
        $('#addtips').text("新密码长度必须不小于6位！");
        return;
    }
    else if ($('#NEWPWD').val() != $('#NEWPWD2').val()) {
        $('#addtips').text("2次密码输入不一致！");
        return;
    }
    else {
        $('#addtips').text("");
    }
}

function modifyselfpwd() {
    if ($('#OPWD').val() == "") {
        $('#addtips').text("请输入原始密码！");
        return;
    }
    else if ($('#NEWPWD').val().length < 6) {
        $('#addtips').text("新密码长度必须不小于6位！");
        return;
    }
    else if ($('#NEWPWD').val() != $('#NEWPWD2').val()) {
        $('#addtips').text("2次密码输入不一致！");
        return;
    }
    PageMethods.ModifyPsw(hex_md5($('#OPWD').val()), hex_md5($('#NEWPWD').val()), function (result) {
        $('#addtips').text(result);
      //  $.messager.alert('提示', result);
        $('#OPWD').val("");
        $('#NEWPWD').val("");
        $('#NEWPWD2').val("");
        return;
    }, function (err) {
        $('#addtips').text(result);
    });
}

function deleteApp(name) {
    if (window.confirm("确定要删除？")) {
        PageMethods.DeleteApp(name, function (result) {
            $.messager.alert('提示', result);
            $("#dg").datagrid("reload");
            return;
        }, function (err) {
            $.messager.alert('提示', err);
        });
    }
}

function AddApp() {
    if ($('#AppName').val() == "") {
        $('#addtips').text("请输入项目名称");
        return;
    }
    PageMethods.AddApp($('#AppName').val(), function (result) {
        $.messager.alert('提示', result);
        $("#dg").datagrid("reload");
        return;
    }, function (err) {
        $.messager.alert('提示', err);
    });
}

function AppChuange() {
    $.ajax({
        type: 'POST',
        url: 'GetUserData.aspx?type=rd&an=' + $('#AppNameDw').val(),
        success: function (data) {
            $('#RoleType').empty();
            $('#RoleType').append(data);
        }
    });
    if ($('#AppNameDw').val() == "URPLTTFRM") {
        $('#RoleType').attr('disabled', true);
    }
    else {
        $('#RoleType').attr('disabled', false);
    }

}

function ReadData(ID) {
    $('#OutAppList').datagrid({
        url: 'GetUserData.aspx?type=app&roleid=' + ID
    });
}

function MatchRole(ID) {
    var status = "";
    if ($("#CK" + ID)[0].checked == false) {
        status = "remove";
    }
    else {
        status = "add";
    }
    var Row = $('#dg').datagrid('getSelected');
    if (Row != null) {
        PageMethods.UpdateRoleMatch(Row.RID, ID, status, function (result) {
            $.messager.alert('提示', result);
            $("#OutAppList").datagrid("reload");
            return;
        }, function (err) {
            $.messager.alert('提示', err);
        });
    }

}