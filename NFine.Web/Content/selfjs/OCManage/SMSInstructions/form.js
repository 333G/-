var id = $.request("id");
function submitForm() {
    if (!$('#form1').formValid()) {
        return false;
    }
    $.submitForm({
        url: "/OCManage/SMSInstructions/SubmitForm?id=" + id,
        param: $("#form1").formSerialize(),
        success: function () {
            $.currentWindow().$("#gridList").trigger("reloadGrid");
        }
    })
}

$(function () {
    SMSInstructionsManager.init();
});
//短信指令编辑插件
var SMSInstructionsManager = function () {
    //加载实体信息
    var LoadModelInfo = function (id) {
        $.ajax({
            url: "/OCManage/SMSInstructions/GetFormJson",
            data: { id: id },
            dataType: "json",
            async: false,
            success: function (data) {
                $("#form1").formSerialize(data);                              
            }
        });
    }
	var CheckMoney = function (gold) {
        if (!(/^\d+$/.test(gold))) {
            return false;
        }
        return true;
    }
	
	var BindMoneyEvent = function () {
        $("#F_Money").blur(function () {
            var Money=$(this).val();
            if (!CheckMoney(Money)) {
				 $.modalMsg("金额格式不正确，请重新输入", "error");
            }                     
        });
    }
	
	return {
        init: function () {
            if (!!id) {
                LoadModelInfo(id);
            }
				BindMoneyEvent();
        }
    }
	
}();