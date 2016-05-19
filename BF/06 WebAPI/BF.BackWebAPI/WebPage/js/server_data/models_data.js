define(['domready!', 'zepto', 'common', 'md5'], function (doc, $, c, md) {

    /*
	*根据类型获取模式列表接口
    type：0.系统提供;1.自定义
	*/
    function GetHealthModelList(type,model_name, page, pageSize, cb, ischeck_code) {
        //登录
        var param = {
            type: (type || 0),
            model_name: model_name,
            page: (page || 0),
            pageSize: (pageSize || 0)
        };
        var url = window.apibase + "/HealthModel/GetHealthModelList";
        c.get(param, url, cb, function (err) { c.msgtips(err, "e"); }, ischeck_code)
    }
    /*
	*根据类型获取模式列表接口
    type：0.系统提供;1.自定义
	*/
    function AddHealthModel(info, cb, ischeck_code) {
        //登录
        var param = info;
        var url = window.apibase + "/HealthModel/AddHealthModel";
        c.post(param, url, cb, function (err) { c.msgtips(err, "e"); }, ischeck_code)
    }
    return {
        GetHealthModelList: GetHealthModelList,
        AddHealthModel: AddHealthModel
    }
})