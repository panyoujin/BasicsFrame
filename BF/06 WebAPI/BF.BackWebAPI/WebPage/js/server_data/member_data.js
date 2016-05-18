define(['domready!', 'zepto', 'common', 'md5'], function (doc, $, c, md) {

    /*
	*登录接口
	*/
    function login(account, password, cb, ischeck_code) {
        //登录
        var param = {
            account: account,
            passwd: md5(password)
        };
        var url = window.apibase + "/Member/Login";
        c.get(param, url, cb, function (err) { c.msgtips(err, "e"); }, ischeck_code)
    }

    return {
        login: login
    }
})