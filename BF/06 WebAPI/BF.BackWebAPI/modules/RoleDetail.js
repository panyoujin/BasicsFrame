define(['domready!', 'zepto', 'common', 'angular', 'pagination'], function (doc, $, c, angular, p) {

    loading(0);
    var width = Screen.ViewWidth();
    var height = Screen.ViewHeight();
    var info = {}
    var roleID = (c.GetUrlParam("roleID") || 0);
    function bindEvent() {
        $('body').show();
        loading(0);

        $('#save_Role').off('click')

        $('#save_Role').on('click', function () {
            var menus = getChecked();
            var roleName = $("#RoleName").val();
            var description = $("#RoleDescribe").val();
            var url = window.apibase + "/Role/Create";
            c.post({ ID: roleID, RoleName: roleName, Description: description, MenuIDs: menus }, url, function (data) {
                alert(data.msg);
                history.back(-1);
            })
        });
        $("#before").click(function () { history.back(-1); });
    }
    function init_data() {
        var url = window.apibase + "/Role/GetRoleInfoByRoleID";
        c.get({ roleID: roleID }, url, function (data) {
            info.RoleInfo = data.data;
            if (info.RoleInfo != null) {
                $("#RoleName").val(info.RoleInfo.RoleName);
                $("#RoleDescribe").val(info.RoleInfo.Description);
            }
        })

    }
    function init_tree() {
        var url = window.apibase + "/Role/GettreeJosn";
        c.get({ roleID: roleID, isShowAll: true }, url, function (data) {
            if (data.data != null) {
                info.MenuList = JSON.parse(data.data);
                $('#menu_tree').tree({ checkbox: true, data: info.MenuList });
            }
        })
    }

    function getChecked() {
        var nodes = $('#menu_tree').tree('getChecked');
        $("#menu_tree").find('.tree-checkbox2').each(function () {
            var node = $(this).parent();
            nodes.push($.extend({}, $.data(node[0], 'tree-node'), {
                target: node[0],
                checked: node.find('.tree-checkbox').hasClass('tree-checkbox2')
            }));
        });
        var s = '';
        for (var i = 0; i < nodes.length; i++) {
            if (nodes[i].id != undefined) {
                if (s != '') s += ',';
                s += nodes[i].id;
            }
        }
        return s;
    }

    $(function () {
        if (roleID > 0) {
            init_data();
        } else {

        }
        init_tree();
        angular.set('info', info, bindEvent);
    })



})