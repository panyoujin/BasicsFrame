define(['domready!', 'zepto', 'common', 'angular', 'server_data/models_data'], function (doc, $, c, angular, m) {

    $(function () {
        //获取分类
        var url = window.apibase + "/Back_Advertise/GetAdvertiseTypes";
        c.get({ Name: "", page: 1, pageSize: 100 }, url, function (data) {
            console.debug(data.data.table);
            if (data != null && data.code == "200") {
                $("#TypeCode").html('<option value="">请选择分类</option>');
                for (var i = 0; i < data.data.table.length; i++) {
                    var str = '<option value=' + data.data.table[i].Code + '>' + data.data.table[i].Name + '</option>';
                    $("#TypeCode").append(str);
                }

                getDetail();
            }
        });

        
        //保存按钮
        $("#save_advertise").click(function () {
            if (formvalidate()==true)
            {
                var url = window.apibase + "/Back_Advertise/InsertAdvertise";
                c.post($('#tab').serialize(), url, function (data) {
     
                    if (data != null && data.code == "200") {
                        window.location.href = "Advertise_Manage.html";
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
            var url = window.apibase + "/Back_Advertise/QueryAdvertiseByID";
            c.get({ ID: ID }, url, function (data) {
                if (data != null && data.code == "200") {
                    $("#Name").val(data.data.Name);
                    $("#ID").val(data.data.ID);
                    $("#TypeCode").val(data.data.TypeCode);
                    $("#GoUrl").val(data.data.GoUrl);
                    $("#ImageUrl").val(data.data.ImageUrl);
                    $("#Sort").val(data.data.Sort);

                    if (data.data.FullUrl != null)
                        $("#titlePic").attr("src", data.data.FullUrl)
                }
            })
        }
    }


    function formvalidate() {

        if ($("#Name").val().length <= 0) {
            alert("请填写广告名称!");
            return false;
        }
        if ($("#TypeCode").val().length <= 0) {
            alert("请填选择分类!");
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