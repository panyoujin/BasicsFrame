define(['domready!', 'zepto', 'common', 'angular', 'server_data/models_data'], function (doc, $, c, angular, m) {
    $(function () {


        getDetail();

        //保存按钮
        $("#save_shop").click(function () {
            if (formvalidate() == true) {
                var url = window.apibase + "/Shop/InsertShop";
                c.post($('#tab').serialize(), url, function (data) {
                    if (data != null && data.code == "200") {
                        window.location.href = "Shopping_Manage.html";
                    }
                })
            }
        });

        $("#before").click(function () { history.back(-1); });
        ////图片上传
        $('#FileUploadImg').change(function () {
            var options = {
                success: callbackuploadimage,
                type: 'post',
                clearForm: false
            };
            $('#UploadFormImg').ajaxSubmit(options);
        });

    })

    function getDetail() {
        var request = {
            QueryString: function (val) {
                var uri = window.location.search;
                var re = new RegExp("" + val + "=([^&?]*)", "ig");
                return ((uri.match(re)) ? (uri.match(re)[0].substr(val.length + 1)) : null);
            }
        }
        var ID = request.QueryString("ID");
        if (ID > 0) {//如果是编辑信息
            var url = window.apibase + "/Shop/QueryShopByID";
     
            c.get({ ID: ID }, url, function (data) {
                console.debug(data);
                if (data != null && data.code == "200") {
                    $("#Name").val(data.data.Name);
                    $("#ID").val(data.data.ID);
                    $("#ImageUrl").val(data.data.ImageUrl);
                    $("#ContentUrl").val(data.data.ContentUrl);
                    $("#Description").val(data.data.Description);
                    $("#NowPrice").val(data.data.NowPrice);
                    $("#Price").val(data.data.Price);
                    if (data.data.FullUrl != null)
                        $("#titlePic").attr("src", data.data.FullUrl)
                }
            })
        }
    }


    function formvalidate() {

        if ($("#Name").val().length <= 0) {
            alert("请填写商品名称!");
            return false;
        }
        if ($("#ContentUrl").val().length <= 0) {
            alert("请填写商品链接地址!");
            return false;
        }
        if ($("#NowPrice").val().length <= 0) {
            alert("请填写商品现价!");
            return false;
        }
        if ($("#NowPrice").val().length <= 0) {
            alert("请填写商品现价!");
            return false;
        }
        return true;
    }

    //上传图片回调方法
    function callbackuploadimage(data) {
        data = $.parseJSON(data);

        if (data != null && data.code == "200") {
            $("#ImageUrl").val(data.data.ImageUrl);
            $("#titlePic").attr("src", data.data.FullUrl)
        }
        else {
            alert("上传失败");
        }
    }

})