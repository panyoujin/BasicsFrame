define(['domready!', 'zepto', 'common', 'angular', 'server_data/models_data', 'pagination'], function (doc, $, c, angular, m, p) {
    $(function () {
        loadData(1, 10);
        $("#btn_Search").click(function () { loadData(); });
        $("#new_type").click(function () { window.location.href = "/Article_TypeDetail.html"; });
        bindEvent();
    })



    function loadData(page, size) {

        var param = {
            type_name: $("#type_name").val(), page: page, pageSize: size
        };

        var url = window.apibase + "/Article/GetArticleTypes";
        c.get(param, url, function (data) {
            init_html(data);
            p.setindex(page, size, data.data.total, function (pindex, psize) {
                loadData(pindex, psize);
            });
        })
    }
    function init_html(data) {
        if (data != null && data.code == "200" && data.data != null) {
            $("#tab").html("");
            for (var i = 0; i < data.data.table.length; i++) {
                var str = "<tr><td>" + (i + 1) + "</td>";
                str += '<td><img src="' + data.data.table[i].ImageUrl + '" width="50px" height="50px" /></td>';
                str += "<td>" + data.data.table[i].Name + "</td><td>" + data.data.table[i].TypeDescribe + "</td> <td>" + data.data.table[i].TypeSort + "</td><td>" + data.data.table[i].OperatioinDate + "</td>";
                str += '<td><a href="Article_TypeDetail.html?ID='+ data.data.table[i].ID + '" ><i class="icon-pencil">编辑</i></a>  <a  role="button" data-toggle="modal"><i class="icon-remove">删除</i></a> </td></tr>';
                $("#tab").append(str);
            }
        }
    }

})