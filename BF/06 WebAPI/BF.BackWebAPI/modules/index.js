define(['domready!', 'zepto', 'common', 'angular'], function (doc, $, c, angular) {

    loading(0);
    //$('body').hide();
    var islist = $('*[select]').length > 0 ? false : true;
    var width = Screen.ViewWidth();
    var height = Screen.ViewHeight();
    var info = { max_member_count: 5 }

    function bindEvent() {
        $('body').show();
        loading(0);
    }
    function menu_click_iframe(obj) {
        var w = $("#if_page").width();
        var h = $("#if_page").height();
        //alert(h);
        var url = $(obj).attr("rel");
        if (!url || url == null || url.length <= 0) {
            return;
        }
        var menuid = $(obj).attr("menu-id");
        var menuname = $(obj).html();
        var page = $("#if_page iframe[menu-id='" + menuid + "']");
        var tab = $(".content-tabs nav .page-tabs-content a[menu-id='" + menuid + "']");
        //如果页面没打开过创建
        if (!page || page == null || page.html() == null) {
            $("#if_page").append('<iframe style="display: inline;width:100%;height:' + (h - 5) + 'px;" class="J_iframe" menu-id="' + menuid + '" src="' + url + '" seamless="" frameborder="0"></iframe>');
        }
        if (!tab || tab == null || tab.html() == null) {
            $(".content-tabs nav ").append('<div style="margin-left: 0px;" class="page-tabs-content"><a href="javascript:tab_click(\'' + menuid + '\');" class="J_menuTab" menu-id="' + menuid + '" >' + menuname + ' </a><a class="fa" close-id="' + menuid + '" href="javascript:tab_close(\'' + menuid + '\');">x</a></div>');
        }
        tab_click(menuid);
    }
    function tab_click(menuid) {
        //修改页面展示
        $("#if_page iframe").css("display", "none");
        $("#if_page iframe[menu-id='" + menuid + "']").css("display", "inline");
        $(".content-tabs nav .page-tabs-content a").removeClass("active");
        $(".content-tabs nav .page-tabs-content a[menu-id='" + menuid + "']").addClass("active");
        $(".content-tabs nav .page-tabs-content a[close-id='" + menuid + "']").addClass("active");
    }
    function tab_close(menuid) {
        var d = $("#if_page iframe[menu-id='" + menuid + "']").css("display");
        //alert(d);
        $("#if_page iframe[menu-id='" + menuid + "']").remove();
        //$(".content-tabs nav .page-tabs-content a[menu-id='"+menuid+"']").remove();
        $(".content-tabs nav .page-tabs-content a[menu-id='" + menuid + "']").parent().remove();
        //修改页面展示
        if (d == "inline") {
            $("#if_page iframe").css("display", "none");
            $("#if_page iframe[menu-id='home']").css("display", "inline");
            $(".content-tabs nav .page-tabs-content a").removeClass("active");
            $(".content-tabs nav .page-tabs-content a[menu-id='home']").addClass("active");
        }
    }

    $(function () {
        
        var b_h = $("body").height();
        if (window.innerHeight) {
            b_h = window.innerHeight;
        }
        $("#if_page").css("height", (b_h - 130) + "px");
        var w = $("#if_page").width();
        var h = $("#if_page").height();
        $("#if_page iframe[menu-id='home']").css("height", (h - 5) + "px");
        $(".sidebar-nav a").click(function () {
            menu_click_iframe(this);
        });
    })

})