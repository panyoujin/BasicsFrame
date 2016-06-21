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
            var url = window.apibase + "/Back_Advertise/QueryAdvertiseTypeByID";
            c.get({ ID: ID }, url, function (data) {
                if (data != null && data.code == "200") {
                    $("#Name").val(data.data.Name);
                    $("#ID").val(data.data.ID);
                    $("#Description").val(data.data.Description);
                    $("#Code").val(data.data.Code);
                }
            })
        }

        $("#Save_AdvertiseType").click(function () {
            if (formvalidate()) {
                var url = window.apibase + "/Back_Advertise/InsertAdvertiseType";
                c.post($('#tab').serialize(), url, function (data) {
                    if (data != null && data.code == "200") {
                        window.location.href = "AdvertiseTypeManage.html";
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
        if ($("#Code").val().length <= 0) {
            alert("请填写分类Code!");
            return false;
        }
        return true;
    }


})