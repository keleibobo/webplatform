function upPage() {
    var currentId = parseInt($("#currentId").val());
    var listId = parseInt($("#listParam").val());
    var componentId = $("#componentId").val();
    var reportIndex = 0;
    for (i = 1; i <= listId; i++) {
        if (i == currentId) {
            if (i == 1) {
                alert('当前是首张票！');
                return;
            }
            reportIndex = i - 1;
        }
    }
    var src = "http://" + window.location.host + "/basepage/HandlerHTML.ashx?cid=" + $("#cId").val() + "&componentid=" + componentId + "&rowindex=" + reportIndex;
    // var src = "../basepage/HandlerHTML.ashx?cid=" + $("#cId").val() + "&componentid=" + componentId + "&rowindex=" + reportIndex;
    $.ajax({
        url: src,
        type: 'get',
        success: function (data) {
            if (data != "") {
                $("body").html(data);
            }
        }
    });
}

function nextPage() {
    var currentId = parseInt($("#currentId").val());
    var listId = parseInt($("#listParam").val());
    var componentId = $("#componentId").val();
    var reportIndex = 0;
    for (i = 1; i <= listId; i++) {
        if (i == currentId) {
            if (i == listId) {
                alert('当前是最后一张票！');
                return;
            }
            reportIndex = i + 1;
        }
    }
    var src = "http://" + window.location.host + "/basepage/HandlerHTML.ashx?cid=" + $("#cId").val() + "&componentid=" + componentId + "&rowindex=" + reportIndex;
    // var src = "../basepage/HandlerHTML.ashx?cid=" + $("#cId").val() + "&componentid=" + componentId + "&rowindex=" + reportIndex;
    $.ajax({
        url: src,
        type: 'get',
        success: function (data) {
            if (data != "") {
                $("body").html(data);
            }
        }
    });

}