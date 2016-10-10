define(['domready!', 'zepto', 'common', 'angular', 'server_data/models_data', 'pagination'], function (doc, $, c, angular, m, p) {
    $(function () {
        loadData(1, 10);
        $("#btn_Search").click(function () { loadData(); });
        $("#new_Advertise").click(function () { window.location.href = "/Advertise_Detail.html"; });
    })



    function loadData(page, size) {

        var param = {
            search: $("#text_search").val(), page: page, pageSize: size
        };

        var url = window.apibase + "/Back_Share/GetUserShares";
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
            console.debug(data);
            for (var i = 0; i < data.data.table.length; i++) {
                var str = "<tr><td>" + (i + 1) + "</td>";
                str += "<td>" + data.data.table[i].ShareTitle + "</td>";
                str += "<td>" + data.data.table[i].ShareContent + "</td>";
                str += "<td>" + data.data.table[i].Source_TypeStr + "</td>";
                str += "<td>" + data.data.table[i].Account + "</td>";
                str += "<td>" + data.data.table[i].Name + "</td>";
                str += "<td>" + data.data.table[i].CreationDate + "</td>";
                str += "<td>" + data.data.table[i].IsHotStr + "</td>";
                str += "<td>" + data.data.table[i].HotSort + "</td>";

                str += '<td><a href="Advertise_Detail.html?ID=' + data.data.table[i].ID + '" ><i class="icon-pencil">编辑</i></a>  <a href="#" role="button"  data-toggle="modal" id="btnDelete" data-id="' + data.data.table[i].ID + '" data-name="' + data.data.table[i].Name + '"><i class="icon-remove">删除</i></a> </td></tr>';
                $("#tab").append(str);
            }
            $("td #btnDelete").off("click");
            $('td #btnDelete').on('click', function () {

                if (confirm('确定要删除_' + $(this).attr("data-name") + '_吗?')) {
                    var url = window.apibase + "/Back_Advertise/DeleteAdvertiseByID";
                    c.get({ ID: $(this).attr("data-id") }, url, function (data) {
                        if (data != null && data.code == "200") {
                            loadData(1, 10);
                        }
                    })
                } else {
                    return false;
                }
            });
        }
    }

})