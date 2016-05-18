define(['domready!', 'zepto', 'common', 'angular','server_data/member_data'], function (doc, $, c, angular,m) {

    var info = {}
    function bindEvent() {
        $('body').show();
        $('#sign').off('click')

        $('#sign').on('click', function () {
            var _account = $("#txt-account").val();
            var _password = $("#txt-password").val();
            if (_account == "") {
                //alert("请输入账号");
                $("#txt-account").focus();
                return;
            }
            if (_password == "") {
                //alert("请输入密码");
                $("#txt-password").focus();
                return;
            }
            m.login(_account, _password, function (data) {
                console.log(data);
                Cookies.Set("CACHED_SESSION_ID",data.data.SessionID);
                window.location.href = c.ReturnUrl;
            });
        });

    }

    $(function () {
        angular.set('info', info, bindEvent);
    })

})