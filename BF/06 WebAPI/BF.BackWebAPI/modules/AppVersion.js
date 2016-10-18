define(['domready!', 'zepto', 'common', 'angular', 'server_data/models_data', 'pagination'], function (doc, $, c, angular, m, p) {
    $(function () {
        loadData(1, 10);

        $("#btn_save").click(function () {

            var param = {
                text_Version: $("#text_Version").val(), text_AppType: $("#text_AppType").val(), text_Target_Url: $("#text_Target_Url").val(), text_Update_Status: $("#text_Update_Status").val(),
                text_VersionDate: $("#text_VersionDate").val(), text_FileSize: $("#text_FileSize").val(), text_UpdateContent: $("#text_UpdateContent").val()
            };
            console.debug(param);
            var url = window.apibase + "/Back_AppVersion/InsertAppVersion";

            c.get(param, url, function (data) {
                if (data != null && data.code == "200") {

                    $('#myModal').modal('hide')

                    loadData(1, 10);
                }

            })
            loadData();
        });
    })


    function loadData(page, size) {

        var param = {
            page: page, pageSize: size
        };

        var url = window.apibase + "/Back_AppVersion/GetAppVersions";
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
                str += "<td>" + data.data.table[i].Version + "</td>";
                str += "<td>" + data.data.table[i].Target_Url + "</td>";
                if (data.data.table[i].AppType == 0) {
                    str += "<td>Android</td>";
                } else {
                    str += "<td>IOS</td>";
                }
                if (data.data.table[i].Update_Status == 0) {
                    str += "<td>不需要更新</td>";
                } else if (data.data.table[i].Update_Status == 1) {
                    str += "<td>有更新但非必要</td>";
                } else {
                    str += "<td>强制更新</td>";
                }
                    
                str += "<td>" + data.data.table[i].VersionDate + "</td>";
                str += "<td>" + data.data.table[i].CreationDate + "</td>";
                str += "<td>" + data.data.table[i].CreationUser + "</td>";
                str += "<td>" + data.data.table[i].FileSize + "</td>";
                str += "<td>" + data.data.table[i].UpdateContent + "</td>";
                str += '<td><a href="#" role="button" id="btnDelete" data-id="' + data.data.table[i].ID + '" >删除</a> ';
                str += '</td></tr>';
                $("#tab").append(str);
            }
            $("td #btnDelete").off("click");
            $('td #btnDelete').on('click', function () {

                var param = { id: $(this).attr("data-id")};

                var url = window.apibase + "/Back_AppVersion/DeleteAppVersion";

                c.get(param, url, function (data) {
                    if (data != null && data.code == "200") {
                        loadData(1, 10);
                    }

                })
            });

   

        }
    }

})