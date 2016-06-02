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
        if (ID > 0) {
            var url = window.apibase + "/Article/QueryArticleTypeByID";
            c.get({ ID: ID }, url, function (data) {
                if (data != null && data.code == "200") {
                    $("#Name").val(data.data.Name);
                    $("#ID").val(data.data.ID);
                    $("#TypeDescribe").val(data.data.TypeDescribe);
                    $("#ImageUrl").val(data.data.ImageUrl);
                    $("#TypeSort").val(data.data.TypeSort);
                    //window.location.href = "Article_TypeManage.html";
                }
            })
        }

        $('#fileInput').change(function () {
            alert($(this).val());
            //str = $(this).val();
        })

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
    })

    function formvalidate() {

        if ($("#Name").val().length <= 0) {
            alert("请填写分类名称!");
            return false;
        }
        return true;
    }

})