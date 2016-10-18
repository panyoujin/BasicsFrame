define(['domready!', 'zepto', 'common', 'angular', 'server_data/models_data', 'pagination'], function (doc, $, c, angular, m, p) {
    $(function () {
        loadData(1, 10);
        $("#btn_Search").click(function () { loadData(); });

        $("#btn_save").click(function () {

            var param = { id: _Shart_ID, sort: $("#text_sort").val() };

            var url = window.apibase + "/Back_Share/SetHotSort";

            c.get(param, url, function (data) {
                if (data != null && data.code == "200") {
    
                    $('#myModal').modal('hide')
                    loadData(1, 10);
                }

            })

        })
    })

    var _Shart_ID = 0;


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

            for (var i = 0; i < data.data.table.length; i++) {
                var str = "<tr><td>" + (i + 1) + "</td>";
                str += "<td>" + data.data.table[i].ShareTitle + "</td>";
                //str += "<td>" + data.data.table[i].ShareContent + "</td>";
                str += "<td>" + data.data.table[i].Source_TypeStr + "</td>";
                str += "<td>" + data.data.table[i].Account + "</td>";
                str += "<td>" + data.data.table[i].Name + "</td>";
                str += "<td>" + data.data.table[i].CreationDate + "</td>";
                str += "<td>" + data.data.table[i].IsHotStr + "</td>";
                str += "<td>" + data.data.table[i].HotSort + "</td>";

                str += '<td><a href="#" data-toggle="modal" data-content="' + data.data.table[i].ShareContent + '" data-target="#modal_Content" id="btnContent">查看内容</a> ';
                if (data.data.table[i].IsHot == 1) {//已经设置热门
                    str += '<a href="#" role="button" id="btnSetHot"  data-id="' + data.data.table[i].ID + '"  data-hot="0" >取消热门</a> ';
                } else {
                    str += '<a href="#" role="button" id="btnSetHot" data-id="' + data.data.table[i].ID + '"  data-hot="1">设置热门</a> ';
                }
                str += '<a href="#" data-toggle="modal" data-target="#myModal" role="button" id="btnSetSort" data-sort="' + data.data.table[i].HotSort + '" data-id="' + data.data.table[i].ID + '"  data-hot="1">设置排序</a> ';
                str += '</td></tr>';
                $("#tab").append(str);
            }
            $("td #btnSetHot").off("click");
            $('td #btnSetHot').on('click', function () {
                var param = { id: $(this).attr("data-id"), hot: $(this).attr("data-hot") };

                var url = window.apibase + "/Back_Share/SetHot";

                c.get(param, url, function (data) {
                    if (data != null && data.code == "200") {
                        loadData(1, 10);
                    }

                })
            });

            $("td #btnSetSort").off("click");
            $('td #btnSetSort').on('click', function () {
                _Shart_ID = $(this).attr("data-id");

                $("#text_sort").val($(this).attr("data-sort"))
            });

            $("td #btnContent").off("click");
            $('td #btnContent').on('click', function () {
                $("#lbContent").html($(this).attr("data-content"));

            });

        }
    }

})