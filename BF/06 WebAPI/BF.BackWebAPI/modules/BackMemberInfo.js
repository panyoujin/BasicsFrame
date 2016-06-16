define(['domready!', 'zepto', 'common', 'angular', 'server_data/models_data', 'pagination'], function (doc, $, c, angular, m, p) {
    $(function () {
        loadData(1, 10);
        $("#btn_Search").click(function () { loadData(); });
        $("#new_Member").click(function () { window.location.href = "/MemberInfo_Detail.html"; });
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
                str += '<td><a href="#" id="btnReset" data-id="' + data.data.table[i].ID + '"><i class="icon-pencil">重置密码</i></a>  <a href="#" role="button"  data-toggle="modal" id="btnDeivces" data-id="' + data.data.table[i].ID + '"><i class="icon-remove">设备管理</i></a> ';
                str += '<a href="#" role="button"  data-toggle="modal" id="btnDelete" data-account="' + data.data.table[i].Account + '" data-id="' + data.data.table[i].ID + '"><i class="icon-remove">删除</i></a> ';
                str += "</td></tr>"
                $("#tab").append(str);
            }
            $("td #btnReset").off("click");
            $('td #btnReset').on('click', function () {

                if (confirm('确定要重置密码吗?')) {
                    var url = window.apibase + "/Back_Member/ResetPasswd";

                    c.get({ memberID: $(this).attr("data-id") }, url, function (data) {
                        if (data != null && data.code == "200") {
                            alert("重置密码成功,默认密码:a000000！");
                        }
                    })
                } else {
                    return false;
                }
            });

            $("td #btnDeivces").off("click");
            $('td #btnDeivces').on('click', function () {
                window.location.href = "DeviceInfo_Manage.html?memberID=" + $(this).attr("data-id");

            });

            $("td #btnDelete").off("click");
            $('td #btnDelete').on('click', function () {

                if (confirm('确定要删除_' + $(this).attr("data-account") + '_吗?')) {
                    var url = window.apibase + "/Back_Member/DeleteMember";
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