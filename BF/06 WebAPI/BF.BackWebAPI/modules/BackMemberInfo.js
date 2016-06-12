define(['domready!', 'zepto', 'common', 'angular', 'server_data/models_data', 'pagination'], function (doc, $, c, angular, m, p) {
    $(function () {
        loadData(1, 10);
        $("#btn_Search").click(function () { loadData(); });

    })



    function loadData(page, size) {

        var param = {
            search: $("#text_search").val(), page: page, pageSize: size
        };

        var url = window.apibase + "/Back_Member/GetMemberInfos";
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
                str += "<td>" + data.data.table[i].Account + "</td>";
                str += "<td>" + data.data.table[i].Name + "</td>";
                str += "<td>" + data.data.table[i].IsAdmin + "</td>";
                str += "<td>" + data.data.table[i].StatusStr + "</td>";
                str += "<td>" + data.data.table[i].Sex + "</td>";
                str += "<td>" + data.data.table[i].CreationDate + "</td>";

                //str += '<td><a href="Article_Detail.html?ID=' + data.data.table[i].ID + '" ><i class="icon-pencil">编辑</i></a>  <a href="#" role="button"  data-toggle="modal" id="btnDelete" data-id="' + data.data.table[i].ID + '" data-name="' + data.data.table[i].ArticleTitle + '"><i class="icon-remove">删除</i></a> </td></tr>';
                $("#tab").append(str);
            }

        }
    }

})