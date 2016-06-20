define(['domready!', 'zepto', 'common', 'angular', 'pagination'], function (doc, $, c, angular, p) {

    loading(0);
    //$('body').hide();
    var islist = $('*[select]').length > 0 ? false : true;
    var width = Screen.ViewWidth();
    var height = Screen.ViewHeight();
    var info = {}
    //0.系统提供;1.自定义
    var type = -1;
    function bindEvent() {
        $('body').show();
        loading(0);

        $('#btn_Search').off('click')

        $('#btn_Search').on('click', function () {
            init_data(1, 10, $("#role_name").val());
        });
        $('td a .icon-remove').off('click')

        $('td a .icon-remove').on('click', function () {
            if (confirm('确定要删除_' + $(this).attr("data-name") + '_吗?')) {
                var url = window.apibase + "/Role/DeleteRole";
                c.post({ ids: $(this).attr("data-id") }, url, function (data) {
                    init_data(1, 10, $("#role_name").val());
                });
            } else {
                return false;
            }

        });

        $('#new_role').off('click')

        $('#new_role').on('click', function () {
            window.location.href = window.webroot + "/RoleDetail.html";
        });
    }
    function init_data(page, size, name) {
        var url = window.apibase + "/Role/GetRoleDataJson";
        c.get({ rolename: name, page: page, pageSize: size }, url, function (data) {
            info.roleList = data.data.RoleList;
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