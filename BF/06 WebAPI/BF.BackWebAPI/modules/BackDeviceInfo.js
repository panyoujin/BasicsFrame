define(['domready!', 'zepto', 'common', 'angular', 'server_data/models_data', 'pagination'], function (doc, $, c, angular, m, p) {
    $(function () {
        getDetail();
        loadData(1, 10);
        $("#btn_Search").click(function () { loadData(); });

    })



    function loadData(page, size) {

        var param = {
            search: $("#text_search").val(), memberID:$("#memberID").val(), page: page, pageSize: size
        };

        var url = window.apibase + "/Back_Member/GetDevices";
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
                str += "<td>" + data.data.table[i].device_id + "</td>";
                str += "<td>" + data.data.table[i].device_identifier + "</td>";
                str += "<td>" + data.data.table[i].name + "</td>";
                str += "<td>" + data.data.table[i].device_type + "</td>";
                str += "<td>" + data.data.table[i].StatusStr + "</td>";
                str += "<td>" + data.data.table[i].CreationUser + "</td>";
                str += "<td>" + data.data.table[i].CreationDate + "</td>";

                //str += '<td><a href="Article_Detail.html?ID=' + data.data.table[i].ID + '" ><i class="icon-pencil">编辑</i></a>  <a href="#" role="button"  data-toggle="modal" id="btnDelete" data-id="' + data.data.table[i].ID + '" data-name="' + data.data.table[i].ArticleTitle + '"><i class="icon-remove">删除</i></a> </td></tr>';
                $("#tab").append(str);
            }

        }
    }

    function getDetail() {
        var request = {
            QueryString: function (val) {
                var uri = window.location.search;
                var re = new RegExp("" + val + "=([^&?]*)", "ig");
                return ((uri.match(re)) ? (uri.match(re)[0].substr(val.length + 1)) : null);
            }
        }
     
        var memberID = request.QueryString("memberID");

        if (memberID > 0) {//如果是编辑信息
            $("#memberID").val(memberID);
        }
    }

})