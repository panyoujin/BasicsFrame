define(['domready!', 'zepto', 'common', 'angular', 'pagination'], function (doc, $, c, angular, p) {

    loading(0);
    var width = Screen.ViewWidth();
    var height = Screen.ViewHeight();
    var info = {}
    function bindEvent() {
        $('body').show();
        loading(0);

        $('#btn_Search').off('click')

        $('#btn_Search').on('click', function () {
            init_data(1, 10, $("#text_search").val());
        });
        //$('td a .icon-pencil').off('click')

        //$('td a .icon-pencil').on('click', function () {
        //    //授权
        //    var url = window.apibase + "/Back_Feedback/GetFeedbackList";
        //    c.post({ ids: $(this).attr("data-id"), roleid: roleID, operation: operation }, url, function (data) {
        //        init_data(1, 10, $("#text_search").val());
        //    })
        //});
    }
    function init_data(page, size, name) {
        var url = window.apibase + "/Back_Feedback/GetFeedbackList";
        c.get({ status: -1, search: name, page: page, pageSize: size }, url, function (data) {
            info.dataList = data.data.dataList;
            
            p.setindex(page, size, data.data.total, function (pindex, psize) {
                init_data(pindex, psize, name);
            });
            angular.set('info', info, bindEvent);
        })

    }
    $(function () {
        init_data(1, 10);

        angular.set('info', info, bindEvent);
    })



})