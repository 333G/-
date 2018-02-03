var id = $.request("id");
function submitForm() {
    if (!$('#form1').formValid()) {
        return false;
    }
    $.submitForm({
        url: "/OCManage/BlackWhiteList/SubmitForm?id=" + id,
        param: $("#form1").formSerialize(),
        success: function () {
            $.currentWindow().$("#gridList").trigger("reloadGrid");
        }
    })
}

$(function () {
    BlackWhiteManager.init();
});
//黑白名单编辑插件
var BlackWhiteManager = function () {
    //加载实体信息
    var LoadModelInfo = function (id) {
        $.ajax({
            url: "/OCManage/BlackWhiteList/GetFormJson",
            data: { id: id },
            dataType: "json",
            async: false,
            success: function (data) {
                $("#form1").formSerialize(data);
                $("#hidOldMobile").val(data.F_Mobile);
                if (data.F_Sign === false) {
                    $("#trLevel").show();
                }
            }
        });
    }
    //检查手机号
    var CheckMobile = function (phone) {
        if (!(/^1[34578]\d{9}$/.test(phone))) {
            return false;
        }
        return true;
    }
    //限制级别（100-180）
    var LimitLevel = function (level) {
        if (level > 180) {
            return 180;
        }
        if (level < 100) {
            return 100;
        }
        return level;
    }
    //类型选择
    var BindSignChange = function () {
        $("#F_Sign").change(function () {
            var selectSignId = $(this).val();
            if (selectSignId === "true") {
                $("#F_Level").val("0");
                $("#trLevel").hide();
            } else {
                $("#F_Level").val("180");
                $("#trLevel").show();
            }
        });
    }
    //绑定级别输入事件
    var BindLevelEvent = function () {
        //限制只能输入数字
        $("#F_Level").keypress(function () {
            if (event.keyCode < 48 || event.keyCode > 57)
                event.returnValue = false;
        });
        $("#F_Level").blur(function () {
            var level = LimitLevel($(this).val());
            $(this).val(level);
        });
    }
    //绑定手机号
    var BindMobileEvent = function () {
        $("#F_Mobile").blur(function () {
            var mobile=$(this).val();
            if (!CheckMobile(mobile)) {
                $.modalMsg("手机号码格式不正确，请重新输入", "error");
            }
            var oldMobile = $("#hidOldMobile").val();
            var newMobile = $("#F_Mobile").val();
            //检查手机号是否重复
            if (newMobile != oldMobile) {
                $.ajax({
                    type: "post",
                    url: "/OCManage/BlackWhiteList/CheckMobile",
                    data: {
                        mobile: mobile
                    },
                    success: function (data) {
                        var json = JSON.parse(data);
                        if (json.state == "success") {
                            $.modalMsg("该手机号码已存在，请检查", "error");
                        }
                    }
                });
            }
        });
    }
    return {
        init: function () {
            if (!!id) {
                LoadModelInfo(id);
            }
            BindMobileEvent();
            BindSignChange();
            BindLevelEvent();
        }
    }
}();
