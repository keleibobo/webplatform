var username = "";
var pwd = "";
var dev_list = "";
var dataList;


$(document).ready(function () {
    var url = "../../../basepage/GetDeviceList.ashx";
    $.ajax({
        url: url,
        type: 'get',
        success: function (data) {
            dataList = jQuery.parseJSON(data);
            dev_list = '<?xml Version=\"1.0\" Encoding=\"UTF-8\"?><DeviceList>';
            if (dataList.ltstm != null) {
                $.ajax({
                    url: "../../../basepage/GetStreamUserData.ashx?random=" + Math.random() * 10,
                    type: 'get',
                    timeout: 60000,
                    success: function (rstr) {
                        for (i = 0; i < dataList.ltstm.length; i++) {
                            if (i == 0) {
                                username = dataList.ltstm[i].user;
                                pwd = dataList.ltstm[i].password;
                            }
                            setRight(rstr);
                            dev_list += '<Server id=\"' + dataList.ltstm[i].id + '\" ip=\"' + dataList.ltstm[i].addr + '\" port=\"' + dataList.ltstm[i].port + '\" name=\"' + dataList.ltstm[i].name + '\" attribute=\"' + dataList.ltstm[i].attribute + '\" upid=\"0\" addrcode=\"temp\" regionid=\"1\">';
                            for (k = 0; k < dataList.ltstm[i].ltdev.length; k++) {
                                dev_list += '<Dvr id=\"' + (k + 1) + '\" ip=\"' + dataList.ltstm[i].ltdev[k].addr + '\" port=\"' + dataList.ltstm[i].ltdev[k].port + '\" name=\"DVR' + (k + 1) + '\" type=\"' + dataList.ltstm[i].type + '\" serverid=\"' + dataList.ltstm[i].id + '\" attribute=\"5\" regionid=\"1\">';
                                for (j = 0; j < dataList.ltstm[i].ltdev[k].ltch.length; j++) {
                                    dev_list += '<Channel id=\"' + dataList.ltstm[i].ltdev[k].ltch[j].id + '\" name=\"' + dataList.ltstm[i].ltdev[k].ltch[j].name + '\" no=\"' + dataList.ltstm[i].ltdev[k].ltch[j].no + '\" type=\"1\" defPreset=\"0\" interval=\"60\" attribute=\"5\" dvrid=\"' + (k + 1) + '\" regionid=\"1\">';
                                    for (l = 0; l < dataList.ltstm[i].ltdev[k].ltch[j].ltpre.length; l++) {
                                        dev_list += '<Preset camid=\"' + dataList.ltstm[i].ltdev[k].ltch[j].id + '\" no=\"' + dataList.ltstm[i].ltdev[k].ltch[j].ltpre[l].id + '\" name=\"' + dataList.ltstm[i].ltdev[k].ltch[j].ltpre[l].desc + '\"/>';
                                    }
                                    dev_list += '</Channel>';
                                }
                                dev_list += '</Dvr>';
                            }
                            dev_list += '</Server>';
                        }
                        dev_list += '</DeviceList>';
                        init();
                    }
                });
            }
        },
        error: function (data) {
            alert(data);
        }
    });

    if ($.browser.msie) {
        var rwidth = parseInt($("#jk").css("width").replace("px", ""));
        if ($.browser.version == "9.0") {
            var rheight = window.innerHeight;
            $("#UTStream").css("width", document.body.clientWidth - rwidth);
            $("#UTStream").css("height", rheight-50);
        }
        else {
            setTimeout(function () {
                var height = window.document.documentElement.clientHeight;
                $("#UTStream").css("width", document.body.clientWidth - rwidth);
                $("#UTStream").css("height", height - 50);
            }, 100);
        }
    }
    $('#recdg').datagrid({
        pageList: [10, 20, 30],
        onDblClickRow: function (rowIndex, rowData) {
            UTStream.UT_STREAM_SetWorkType(2);
            UTStream.UT_STREAM_StopRPlayBack(1);
            var xml = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" + rowData.fstart + rowData.fend + rowData.finfo;
            UTStream.UT_STREAM_StartRPlayBack(xml, 1, 0);
            UTStream.UT_STREAM_Timeproc();
        }
    });
});

function setRight(rstr) {
    if (rstr == "[]")
        return;
    var auList = jQuery.parseJSON(rstr);
    if (auList != null) {
        for (k = 0; k < auList[0].userparamlist.length; k++) {
            if (k == 0) {
                var up = jQuery.parseJSON(auList[0].userparamlist[0]);
                username = up.streamusername;
                pwd = up.streampassword;
            }
        }
    }
}

function init() {
    UTStream.UT_STREAM_Init(0);
    stm_dev_list();
    stm_setuser(username, pwd);
    stm_setappuser(username);
    UTStream.UT_STREAM_SetWndNum(1);
    UTStream.UT_STREAM_SetSelWndNo(1);
    UTStream.UT_STREAM_SetWorkType(2);
    UTStream.UT_STREAM_Timeproc();
    setInterval(function () { UTStream.UT_STREAM_Timeproc(); }, 100);
}

function stm_setappuser(user) {
    if (dataList.ltstm[0].authority != null) {
        UTStream.UT_STREAM_SetAppUser(user, parseInt(dataList.ltstm[0].authority));
    }
}

function stm_setuser(user, pwd) {
    UTStream.UT_STREAM_SetUser(user, pwd);
}

function stm_dev_list() {
    UTStream.UT_STREAM_SetDeviceList(dev_list);
}


function stm_setuser(user, pwd) {
    UTStream.UT_STREAM_SetUser(user, pwd);
}


function recQuery() {
    if ($("#nodeid").val() == "") {
        alert("请先选择要查询的通道！");
        return;
    }

    if ($("#starttime").datebox('getValue') == "" || $("#endtime").datebox('getValue') == "") {
        alert("请先选择开始结束时间！");
        return;
    }

    $('#recdg').datagrid('loadData', { total: 0, rows: [] });
    var startStr = $("#starttime").datebox('getValue');
    var startTime = new Date(Date.parse(startStr.replace(/-/g, "/")));
    var endStr = $("#endtime").datebox('getValue');
    var endTime = new Date(Date.parse(endStr.replace(/-/g, "/")));
    var serverId = "";
    if (dataList != null) {
        for (i = 0; i < dataList.ltstm.length; i++) {
            for (k = 0; k < dataList.ltstm[i].ltdev.length; k++) {
                for (j = 0; j < dataList.ltstm[i].ltdev[k].ltch.length; j++) {
                    if (dataList.ltstm[i].ltdev[k].ltch[j].id == $("#nodeid").val() && dataList.ltstm[i].ltdev[k].ltch[j].stInfo.id == $("#parentid").val()) {
                        $("#DvrIp").val(dataList.ltstm[i].ltdev[k].addr);
                        $("#DvrPort").val(dataList.ltstm[i].ltdev[k].port);
                        $("#CamChanNo").val(dataList.ltstm[i].ltdev[k].ltch[j].no);
                        serverId = dataList.ltstm[i].id;
                    }
                }
            }
        }
    }

    var dvrIp = $("#DvrIp").val();
    var dvrPort = $("#DvrPort").val();
    var camChanNo = $("#CamChanNo").val();

    var queryType = 1;
    var recordType = 1;
    var queryXml = '<?xml version="1.0" encoding="UTF-8" ?>' +
                  '<TIMESTART Year="' + startTime.getFullYear() + '" Month="' + (startTime.getMonth() + 1) + '" Day="' + startTime.getDate() + '" Hour="' + (startTime.getHours() + 1) + '" Minute="' + (startTime.getMinutes() + 1) + '" Second="' + (startTime.getSeconds() + 1) + '"/>' +
                  '<TIMEEND Year="' + endTime.getFullYear() + '" Month="' + (endTime.getMonth() + 1) + '" Day="' + endTime.getDate() + '" Hour="' + (endTime.getHours() + 1) + '" Minute="' + (endTime.getMinutes() + 1) + '" Second="' + (endTime.getSeconds() + 1) + '"/>' +
                  '<NET_RECORD_INFO ServerId="'+serverId+'"  DvrIp="' + dvrIp + '" DvrPort="' + dvrPort + '" CamChanNo="' + camChanNo + '" QueryType="' + queryType + '" RecordType="' + recordType + '"/>';

    var result = UTStream.UT_STREAM_QueryRecord(queryXml, 0, 0);
    var xDoc = loadXML(result);
    var root = xDoc.selectNodes("FILE_LIST")[0];
    var rowsitem = [];
    if (root != null) {
        for (i = 0; i < root.childNodes.length; i++) {
            var TIMESTART = root.childNodes[i].selectNodes("TIMESTART")[0];
            var startTime = TIMESTART.getAttribute("Year") + "-" + TIMESTART.getAttribute("Month") + "-" + TIMESTART.getAttribute("Day") + " " + TIMESTART.getAttribute("Hour") + ":" + TIMESTART.getAttribute("Minute") + ":" + TIMESTART.getAttribute("Second");
            var TIMEEND = root.childNodes[i].selectNodes("TIMEEND")[0];
            var endTime = TIMEEND.getAttribute("Year") + "-" + TIMEEND.getAttribute("Month") + "-" + TIMEEND.getAttribute("Day") + " " + TIMEEND.getAttribute("Hour") + ":" + TIMEEND.getAttribute("Minute") + ":" + TIMEEND.getAttribute("Second");
            var NET_RECORD_INFO = root.childNodes[i].selectNodes("NET_RECORD_INFO")[0];
            var size = NET_RECORD_INFO.getAttribute("Size");
            var row = {
                "fstart": TIMESTART.xml,
                "fend": TIMEEND.xml,
                "finfo": NET_RECORD_INFO.xml,
                "start": startTime,
                "end": endTime,
                "size": size
            };
            rowsitem.push(row);
        }
        $('#recdg').datagrid({ loadFilter: pagerFilter }).datagrid('loadData', rowsitem);
    }

}

function pagerFilter(data) {
    if (typeof data.length == 'number' && typeof data.splice == 'function') {	// is array
        data = {
            total: data.length,
            rows: data
        }
    }
    var dg = $(this);
    var opts = dg.datagrid('options');
    var pager = dg.datagrid('getPager');
    pager.pagination({
        onSelectPage: function (pageNum, pageSize) {
            opts.pageNumber = pageNum;
            opts.pageSize = pageSize;
            pager.pagination('refresh', {
                pageNumber: pageNum,
                pageSize: pageSize
            });
            dg.datagrid('loadData', data);
        }
    });
    if (!data.originalRows) {
        data.originalRows = (data.rows);
    }
    var start = (opts.pageNumber - 1) * parseInt(opts.pageSize);
    var end = start + parseInt(opts.pageSize);
    data.rows = (data.originalRows.slice(start, end));
    return data;
}


function loadXML(xmlString) {

    var xmlDoc;
    if (window.ActiveXObject) {
        xmlDoc = new ActiveXObject('Microsoft.XMLDOM');
        if (!xmlDoc) xmldoc = new ActiveXObject("MSXML2.DOMDocument.3.0");
        xmlDoc.async = false;
        xmlDoc.loadXML(xmlString);
    } else if (document.implementation && document.implementation.createDocument) {
        var domParser = new DOMParser();
        xmlDoc = domParser.parseFromString(xmlString, 'text/xml');
    } else {
        return null;
    }
    return xmlDoc;
}


var toolbar = [{
    text: '下载录像',
    iconCls: 'icon-save',
    handler: function () {
        var row = $('#recdg').datagrid('getSelected');
        if (row) {
            var xml = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" + row.fstart + row.fend + row.finfo;
            UTStream.UT_STREAM_StartDownload(xml, 0, 0);
        }
        else {
            alert('请先选择要下载的录像!');
        }
    }
}, {
    text: '<img src="../images/play.gif" width="20px" height="20px" border=0  /><div style="margin-left:25px; margin-top:-19px;">播放本地录像</div>',
    iconCls: '',
    handler: function () {
        UTStream.UT_STREAM_SetWorkType(3);
        UTStream.UT_STREAM_LocalPlay();
    }
}];


window.onunload = function () {
    UTStream.UT_STREAM_UnInit();
}