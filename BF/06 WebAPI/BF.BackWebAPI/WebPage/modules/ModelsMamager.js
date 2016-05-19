﻿define(['domready!', 'zepto', 'common', 'angular', 'server_data/models_data'], function (doc, $, c, angular, m) {

    loading(0);
    //$('body').hide();
    var islist = $('*[select]').length > 0 ? false : true;
    var width = Screen.ViewWidth();
    var height = Screen.ViewHeight();
    var info = {}
    //0.系统提供;1.自定义
    var type = -1;
    var page = 0;
    var pagesize = 10;
    function bindEvent() {
        $('body').show();
        loading(0);
        
        $('#btn_Search').off('click')

        $('#btn_Search').on('click', function () {
            page = 0;
            init_data($("#model_name").val());
        });

        $('#new_model').off('click')

        $('#new_model').on('click', function () {
            alert(window.webroot+"/new-model.html")
            window.location.href = window.webroot+"/new-model.html";
        });
    }
    function init_data(name) {
        page = page + 1;
        m.GetHealthModelList(type, name, page, pagesize, function (data) {
            info.models = data.data;
            angular.set('info', info, bindEvent);
        })
    }
    $(function () {
        init_data();
        //alert(1);
        //info.models = [{ MID: 1, Introduce: "菊花茶的味道", Model_Name: "菊花茶", CreationDate: "2016-01-01" }, { MID: 2, Introduce: "菊花茶的味道", Model_Name: "菊花茶", CreationDate: "2016-01-01" }, { MID: 3, Introduce: "菊花茶的味道", Model_Name: "菊花茶", CreationDate: "2016-01-01" }]
        //angular.set('info', info, bindEvent);
    })

})