define(['domready!', 'zepto', 'common', 'angular', 'pagination'], function (doc, $, c, angular, p) {

    loading(0);
    var width = Screen.ViewWidth();
    var height = Screen.ViewHeight();
    var info = {}
    var operation = (c.GetUrlParam("operation") || "");
    var roleID = c.GetUrlParam("roleID");
    //是否显示授权按钮
    var IsShowAuthor = "none";
    var IsShowUnAuthor = "inline-block";
    $(this).Title = "取消授权";
    if (operation.length <= 0) {
        //获取未授权用户显示授权按钮
        IsShowAuthor = "inline-block";
        var IsShowUnAuthor = "none";
        $(this).Title = "授权";
    }
    function bindEvent() {
        $('body').show();
        loading(0);
        $('#btn_Search').off('click')

        $('#btn_Search').on('click', function () {
            init_data(1, 10, $("#text_search").val());
        });
        $('td a .icon-remove').off('click')

        $('td a .icon-remove').on('click', function () {
            //取消授权
            if (confirm('确定要取消_' + $(this).attr("data-name") + '_授权吗?')) {
                var url = window.apibase + "/Role/AuthorizationUser";
                c.post({ ids: $(this).attr("data-id"), roleid: roleID, operation: operation }, url, function (data) {
                    init_data(1, 10, $("#text_search").val());
                })
            } else {
                return false;
            }

        });
        $('td a .icon-pencil').off('click')

        $('td a .icon-pencil').on('click', function () {
            //授权
            var url = window.apibase + "/Role/AuthorizationUser";
            c.post({ ids: $(this).attr("data-id"), roleid: roleID, operation: operation }, url, function (data) {
                init_data(1, 10, $("#text_search").val());
            })
        });
    }
    function init_data(page, size, name) {
        var url = window.apibase + "/Role/UserDataJson";
        c.get({ roleID: roleID, operation: operation, page: page, pageSize: size }, url, function (data) {
            info.roleUserList = data.data.RoleUserList;
            $.each(info.roleUserList, function (index, obj) {
                obj.IsShowAuthor = IsShowAuthor;
                obj.IsShowUnAuthor = IsShowUnAuthor;
            });
            p.setindex(page, size, data.data.total, function (pindex, psize) {
                init_data(pindex, psize, name);
            });
            angular.set('info', info, bindEvent);
        })

    }
    $(function () {
        init_data(1, 10);
    })



})