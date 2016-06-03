define(['domready!', 'zepto', 'common', 'angular', 'server_data/models_data'], function (doc, $, c, angular, m) {
    $(function () {
        var request = {
            QueryString: function (val) {
                var uri = window.location.search;
                var re = new RegExp("" + val + "=([^&?]*)", "ig");
                return ((uri.match(re)) ? (uri.match(re)[0].substr(val.length + 1)) : null);
            }
        }
        var ID = request.QueryString("ID");
        if (ID > 0) {//如果是编辑信息
            var url = window.apibase + "/Article/QueryArticleTypeByID";
            c.get({ ID: ID }, url, function (data) {
                if (data != null && data.code == "200") {
                    $("#Name").val(data.data.Name);
                    $("#ID").val(data.data.ID);
                    $("#TypeDescribe").val(data.data.TypeDescribe);
                    $("#ImageUrl").val(data.data.ImageUrl);
                    $("#TypeSort").val(data.data.TypeSort);

                    $("#ImageUrl").val(data.data.ImageUrl);
                    if (data.data.FullUrl != null)
                        $("#titlePic").attr("src", data.data.FullUrl)
                }
            })
        }

        $("#save_articleType").click(function () {
            if (formvalidate()) {
                var url = window.apibase + "/Article/InsertArticleType";
                c.post($('#tab').serialize(), url, function (data) {
                    if (data != null && data.code == "200") {
                        window.location.href = "Article_TypeManage.html";
                    }
                })
            }

        });

        $("#before").click(function () { history.back(-1); });
        //图片上传
        $('#FileUploadImg').change(function () {
            var options = {
                success: callbackuploadimage,
                type: 'post',
                clearForm: false
            };
            $('#UploadFormImg').ajaxSubmit(options);
        });

    })

    function formvalidate() {
        alert($("#ImageUrl").val());

        if ($("#Name").val().length <= 0) {
            alert("请填写分类名称!");
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