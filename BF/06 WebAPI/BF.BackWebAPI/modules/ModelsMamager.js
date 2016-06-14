define(['domready!', 'zepto', 'common', 'angular', 'server_data/models_data', 'pagination'], function (doc, $, c, angular, m, p) {

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
            init_data(1, 10, $("#model_name").val());
        });
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
    }
    function init_data(page, size, name) {
        m.GetHealthModelList(type, name, page, size, function (data) {
            info.models = data.data.modelList;
            $.each(info.models, function (index, obj) {
                obj.model_hide = "none";
                obj.edit_url = "";
                if (obj.Model_Type == 0) {
                    obj.model_hide = "inline-block";
                    obj.edit_url = window.webroot + "/new-model.html?modelID=" + obj.MID;
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