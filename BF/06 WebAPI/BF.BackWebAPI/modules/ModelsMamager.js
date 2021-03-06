﻿define(['domready!', 'zepto', 'common', 'angular', 'server_data/models_data', 'pagination'], function (doc, $, c, angular, m, p) {

    loading(0);
    //$('body').hide();
    var islist = $('*[select]').length > 0 ? false : true;
    var width = Screen.ViewWidth();
    var height = Screen.ViewHeight();
    var info = {}
    //0.系统提供;1.自定义
    var IsCustom = -1;
    function bindEvent() {
        $('body').show();
        loading(0);

        $('#btn_Search').off('click')

        $('#btn_Search').on('click', function () {
            init_data(1, 10, $("#model_name").val());
        });
        $('td a .icon-remove').off('click')

        $('td a .icon-remove').on('click', function () {
            if (confirm('确定要删除_' + $(this).attr("data-name") + '_吗?')) {
                m.DeleteHealthModelByModelID($(this).attr("data-mid"), function (data) {

                    init_data(1, 10, $("#model_name").val());
                });
            } else {
                return false;
            }

        });
        $('#new_model').off('click')

        $('#new_model').on('click', function () {
            window.location.href = window.webroot + "/new-model.html?r=0622";
        });
    }
    function init_data(page, size, name) {
        m.GetHealthModelList(IsCustom, name, page, size, function (data) {
            info.models = data.data.modelList;
            $.each(info.models, function (index, obj) {
                obj.model_hide = "none";
                obj.edit_url = "";
                if (obj.IsCustom == 0 || obj.IsCustom == 1) {
                    obj.model_hide = "inline-block";
                    obj.edit_url = window.webroot + "/new-model.html?r=0622&modelID=" + obj.MID;
                }
            })
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