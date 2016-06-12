define(['domready!', 'zepto', 'common', 'angular', 'server_data/models_data'], function (doc, $, c, angular, m) {
    var ue = UE.getEditor('ArticleContent');
    UE.getEditor('ArticleContent').ready(function () {
        //获取分类
        var url = window.apibase + "/Article/GetArticleTypes";
        c.get({ type_name: "", page: 1, pageSize: 100 }, url, function (data) {

            if (data != null && data.code == "200") {
                $("#ArticleType_ID").html('<option value="">请选择分类</option>');
                for (var i = 0; i < data.data.table.length; i++) {
                    var str = '<option value=' + data.data.table[i].ID + '>' + data.data.table[i].Name + '</option>';
                    $("#ArticleType_ID").append(str);
                }

                getDetail();
            }
        });

        
        //保存按钮
        $("#save_article").click(function () {
            if (formvalidate()==true)
            {
                var url = window.apibase + "/Article/InsertArticle";
                c.post($('#tab').serialize(), url, function (data) {
     
                    if (data != null && data.code == "200") {
                        window.location.href = "Article_Manage.html";
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

    function getDetail()
    {
        var request = {
            QueryString: function (val) {
                var uri = window.location.search;
                var re = new RegExp("" + val + "=([^&?]*)", "ig");
                return ((uri.match(re)) ? (uri.match(re)[0].substr(val.length + 1)) : null);
            }
        }
        var ID = request.QueryString("ID");
        if (ID > 0) {//如果是编辑信息
            var url = window.apibase + "/Article/QueryArticleByID";
            c.get({ ID: ID }, url, function (data) {
                if (data != null && data.code == "200") {
                    $("#ArticleTitle").val(data.data.ArticleTitle);
                    $("#ID").val(data.data.ID);
                    $("#ArticleType_ID").val(data.data.ArticleType_ID);

                    $("#ImageUrl").val(data.data.ImageUrl);
                    $("#ArticleSort").val(data.data.ArticleSort);

                    $("#ArticleContent").val(data.data.ArticleContent);

                    ue.setContent(data.data.ArticleContent);

                    $("#PublishDate").val(data.data.PublishDate);
                    if (data.data.FullUrl != null)
                        $("#titlePic").attr("src", data.data.FullUrl)
                }
            })
        }
    }


    function formvalidate() {

        if ($("#ArticleTitle").val().length <= 0) {
            alert("请填写文章标题!");
            return false;
        }
        
        if ($("#PublishDate").val().length <= 0) {
            alert("请填写发布时间!");
            return false;
        }
        if ($("#ArticleContent").val().length <= 0) {
            alert("请填写发布内容!");
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