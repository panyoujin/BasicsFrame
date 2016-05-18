define(['domready!', 'zepto', 'common', 'md5'], function (doc, $, c, md) {

    /*
	*根据类型获取模式列表接口
    type：0.系统提供;1.自定义
	*/
    function GetHealthModelList(type, page, pageSize, cb, ischeck_code) {
        //登录
        var param = {
            type: (type || 0),
            page: (page || 0),
            pageSize: (pageSize || 0)
        };
        var url = window.apibase + "/HealthModel/GetHealthModelList";
        c.get(param, url, cb, function (err) { c.msgtips(err, "e"); }, ischeck_code)
    }

    return {
        GetHealthModelList: GetHealthModelList
    }
})