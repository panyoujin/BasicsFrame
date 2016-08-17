define(['domready!', 'zepto', 'common', 'angular', 'server_data/models_data'], function (doc, $, c, angular, m) {

    loading(0);
    //var isedit = $('*[edit]').length > 0;
    var modelID = c.GetUrlParam("modelID");
    var width = Screen.ViewWidth();
    var height = Screen.ViewHeight();
    var info = {
        Model_Name: '',//模式名称，养生品名称
        Param: '0,0|0,0|0,0|0,0|0,0|0,0',//参数
        Sort: 0,//排序，越大越前
        IcoUrl: "",//图标地址
        ImageUrl: "",//图片地址
        Introduce: "",//简单介绍，用户自定义的需要动态生成
        Describe: "",//材料描述
        Remarks: "",//注意事项
        ImageUrl: "",
        FullUrl: "js/bootstrap/img/notimage.png",
        WeChatUrl: "",
        ModelType_ID: 0,
        byte1: 0,
        byte2: 0,
        byte3: 0,
        byte4: 0,
        byte5: 0,
        byte6: 0,
        byte7: 0,
        byte8: 0,
        byte9: 0,
        byte10: 0,
        byte11: 0,
        byte12: 0,

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

            info.ImageUrl = $("#ImageUrl").val();
            info.Introduce = $("#Introduce").val();
            info.Describe = $("#Describe").val();
            info.Param = (info.byte1 || 0) + ',' + (info.byte2 || 0) + '|' + (info.byte3 || 0) + ',' + (info.byte4 || 0) + '|' + (info.byte5 || 0) + ',' + (info.byte6 || 0) + '|' + (info.byte7 || 0) + ',' + (info.byte8 || 0) + '|' + (info.byte9 || 0) + ',' + (info.byte10 || 0) + '|' + (info.byte11 || 0) + ',' + (info.byte12 || 0);
            info.ModelType_ID = $("#ModelType_ID").val();
            m.AddHealthModel(info, function (data) {
                alert(data.msg);
                history.back(-1);
            });
        });


        $("#before").off('click')
        $("#before").on('click', function () {
            history.back(-1);
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

        var url = window.apibase + "/Back_ModelType/GetModelTypes";
        c.get({ type_name: "", page: 1, pageSize: 100 }, url, function (data) {

            if (data != null && data.code == "200") {
                $("#ModelType_ID").html('<option value="0">请选择分类</option>');
                for (var i = 0; i < data.data.table.length; i++) {
                    var str = "";
                    if (data.data.table[i].ID == info.ModelType_ID) {
                        str = '<option value=' + data.data.table[i].ID + ' selected="selected">' + data.data.table[i].Name + '</option>';
                    }
                    else {
                        str = '<option value=' + data.data.table[i].ID + '>' + data.data.table[i].Name + '</option>';
                    }
                    $("#ModelType_ID").append(str);
                }
            }
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
                    if (info.Param != null && info.Param.length > 0) {
                        var byteList = info.Param.split('|');
                        for (var i = 0; i < byteList.length; i++) {
                            var arry = byteList[i].split(',');
                            switch (i) {
                                case 0:
                                    info.byte1 = arry[0];
                                    info.byte2 = arry[1];
                                    break;
                                case 1:
                                    info.byte3 = arry[0];
                                    info.byte4 = arry[1];
                                    break;
                                case 2:
                                    info.byte5 = arry[0];
                                    info.byte6 = arry[1];
                                    break;
                                case 3:
                                    info.byte7 = arry[0];
                                    info.byte8 = arry[1];
                                    break;
                                case 4:
                                    info.byte9 = arry[0];
                                    info.byte10 = arry[1];
                                    break;
                                case 5:
                                    info.byte11 = arry[0];
                                    info.byte12 = arry[1];
                                    break;
                            }
                        }
                    }
                    else {
                        info.byte1 = 0;
                        info.byte2 = 0;
                        info.byte3 = 0;
                        info.byte4 = 0;
                        info.byte5 = 0;
                        info.byte6 = 0;
                        info.byte7 = 0;
                        info.byte8 = 0;
                        info.byte9 = 0;
                        info.byte10 = 0;
                        info.byte11 = 0;
                        info.byte12 = 0;
                    }
                }
                angular.set('info', info, bindEvent)
            });
        } else {

            info.byte1 = 0;
            info.byte2 = 0;
            info.byte3 = 0;
            info.byte4 = 0;
            info.byte5 = 0;
            info.byte6 = 0;
            info.byte7 = 0;
            info.byte8 = 0;
            info.byte9 = 0;
            info.byte10 = 0;
            info.byte11 = 0;
            info.byte12 = 0;
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
            info.IcoUrl = data.data.ImageUrl;
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