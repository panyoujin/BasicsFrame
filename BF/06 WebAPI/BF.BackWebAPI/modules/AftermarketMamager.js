define(['domready!', 'zepto', 'common', 'angular', 'server_data/Aftermarket_data', 'pagination'], function (doc, $, c, angular, a, p) {

    loading(0);
    //$('body').hide();
    var islist = $('*[select]').length > 0 ? false : true;
    var width = Screen.ViewWidth();
    var height = Screen.ViewHeight();
    var info = {}
    function bindEvent() {
        $('body').show();
        loading(0);

        $('#btn_Search').off('click')

        $('#btn_Search').on('click', function () {
            init_data(1, 10, $("#aftermarket_name").val());
        });
        $('#datalist tr td a').off('click')

        $('#datalist tr td a').on('click', function () {
            var aid = $(this).attr("data-id");
            var status = $(this).attr("data-status");
            a.SetAftermarketStatus(aid, status, function (data) {
                alert(data.msg);
                init_data(1, 10);
            })
        });
    }
    function init_data(page, size, name) {
        var status = $("#aftermarketStatus").val();
        var code = $("#productCode").val();
        a.GetAftermarketList(status, code, page, size, function (data) {
            info.dataList = data.data.dataList;
            $.each(info.dataList, function (index, obj) {
                obj.edit_url = window.webroot + "/Aftermarket-info.html?aftermarket=" + obj.uuid;
                obj.Processing_Hide = 'none';
                obj.No_Hide = 'none';
                obj.Complete_Hide = 'none';
                if (obj.AftermarketStatus == 1) {
                    obj.Processing_Hide = 'inline-block';
                    obj.No_Hide = 'inline-block';
                    obj.Complete_Hide = 'inline-block';
                } else if (obj.AftermarketStatus == 3) {
                    obj.Processing_Hide = 'none';
                    obj.No_Hide = 'inline-block';
                    obj.Complete_Hide = 'inline-block';
                }

            })
            p.setindex(page, size, data.data.total, function (pindex, psize) {
                init_data(pindex, psize);
            });
            angular.set('info', info, bindEvent);
        })
    }
    $(function () {
        init_data(1, 10);
    })

})