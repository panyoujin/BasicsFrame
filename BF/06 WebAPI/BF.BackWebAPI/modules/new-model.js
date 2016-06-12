define(['domready!', 'zepto', 'common', 'angular', 'server_data/models_data'], function (doc, $, c, angular, m) {

    loading(0);
    //var isedit = $('*[edit]').length > 0;
    var modelID = c.GetUrlParam("modelID");
    var width = Screen.ViewWidth();
    var height = Screen.ViewHeight();
    var info = {
        Model_Name: '',//模式名称，养生品名称
        Final_Temperature: 100,//最终温度
        Cook_Time: 5,//煮料时长，单位分钟
        Cook_Temperature: 100,//煮料温度，单位摄氏度，最小单位1摄氏度
        Removal_Chlorine_Time: 0,//除氯时长，单位分钟
        Is_Heat_Preservation: false,//是否保温，默认不保温
        Heat_Preservation_Time: 0,//保温时长，单位分钟
        Heat_Preservation_Temperature: 0,//保温温度，单位摄氏度，最小单位1摄氏度
        IsFerv: false,//是否煮沸,默认不煮沸
        IsBubble: false,//是否泡料，默认不泡
        Bubble_Temperature: 0,//泡料温度，单位摄氏度，最小单位1摄氏度
        Bubble_Time: 0,//泡料时长，单位分钟
        Sort: 0,//排序，越大越前
        IcoUrl: "",//图标地址
        ImageUrl: "",//图片地址
        Introduce: "",//简单介绍，用户自定义的需要动态生成
        Describe: "",//材料描述
        Remarks: "",//注意事项
        ImageUrl: "",
        FullUrl: "js/bootstrap/img/notimage.png",
        WeChatUrl:""
    }
    function bindEvent() {
        $('body').show();
        loading(0);
        $('#save_model').off('click')

        $('#save_model').on('click', function () {
            if (!info.Model_Name) {
                alert('名称不能为空');
                return;
            }
            if (!info.Final_Temperature) {
                alert('最终温度不能为空');
                return;
            }
            info.ImageUrl = $("#ImageUrl").val();
            if (!info.Cook_Time) {
                alert('煮料时间不能为空');
                return;
            }

            m.AddHealthModel(info, function (data) {
                alert(data.msg);
            });
        });
        ////图片上传
        $('#FileUploadImg').change(function () {
            var options = {
                success: callbackuploadimage,
                type: 'post',
                clearForm: false
            };
            $('#UploadFormImg').ajaxSubmit(options);
        });
    }
    function init_data() {
        if (modelID > 0) {
            m.GetHealthModelInfo(modelID, function (data) {
                if (data.data != null && data.data.MID > 0) {
                    info = data.data;
                    if (info.FullUrl == null || info.FullUrl.length <= 0) {
                        info.FullUrl = "js/bootstrap/img/notimage.png";
                    }
                }
                angular.set('info', info, bindEvent)
            });
        } else {
            angular.set('info', info, bindEvent)
        }
    }
    $(function () {
        init_data();
    })
    //上传图片回调方法
    function callbackuploadimage(data) {
        data = $.parseJSON(data);

        if (data != null && data.code == "200") {
            info.ImageUrl = data.data.ImageUrl;
            info.FullUrl = data.data.FullUrl;
            $("#ImageUrl").val(data.data.ImageUrl);
            $("#titlePic").attr("src", data.data.FullUrl);
        }
        else {
            alert("上传失败");
        }
    }
})