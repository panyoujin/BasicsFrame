define(['domready!', 'zepto', 'common'], function (doc, $, c) {

    /*
	*根据类型获取模式列表接口
    type：0.系统提供;1.自定义
	*/
    function GetHealthModelList(type,model_name, page, pageSize, cb, ischeck_code) {
        //登录
        var param = {
            type: (type || -1),
            model_name: model_name,
            page: (page || 0),
            pageSize: (pageSize || 10)
        };
        var url = window.apibase + "/Bask_HealthModel/GetHealthModelList";
        c.get(param, url, cb, function (err) { c.msgtips(err, "e"); }, ischeck_code)
    }
    /*
	*根据模式ID获取模式详情接口
	*/
    function GetHealthModelInfo(modelID, cb, ischeck_code) {
        //登录
        var param = {
            modelID: (modelID || 0)
        };
        var url = window.apibase + "/Bask_HealthModel/GetHealthModelInfo";
        c.get(param, url, cb, function (err) { c.msgtips(err, "e"); }, ischeck_code)
    }
    /*
	*新增模式
	*/
    function AddHealthModel(info, cb, ischeck_code) {
        //登录
        var param = info;
        var url = window.apibase + "/Bask_HealthModel/AddHealthModel";
        c.post(param, url, cb, function (err) { c.msgtips(err, "e"); }, ischeck_code)
    }
    return {
        GetHealthModelList: GetHealthModelList,
        AddHealthModel: AddHealthModel,
        GetHealthModelInfo: GetHealthModelInfo
    }
})