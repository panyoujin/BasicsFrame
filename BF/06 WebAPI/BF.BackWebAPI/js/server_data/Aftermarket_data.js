define(['domready!', 'zepto', 'common'], function (doc, $, c) {

    /*
	*根据类型获取模式列表接口
    type：0.系统提供;1.自定义
	*/
    function GetAftermarketList(status, code, page, pageSize, cb, ischeck_code) {
        //登录
        var param = {
            status: status,
            code: code,
            page: (page || 0),
            pageSize: (pageSize || 0)
        };
        var url = window.apibase + "/Back_Aftermarket/GetAftermarketList";
        c.get(param, url, cb, function (err) { c.msgtips(err, "e"); }, ischeck_code)
    }
    /*
	*修改状态
	*/
    function SetAftermarketStatus(aid, status, cb, ischeck_code) {
        //登录
        var param = {
            aid: aid,
            status: status
        };
        var url = window.apibase + "/Back_Aftermarket/SetAftermarketStatus";
        c.post(param, url, cb, function (err) { c.msgtips(err, "e"); }, ischeck_code)
    }

    /*
	*修改状态
	*/
    function AddRemarks(aid, remarks, cb, ischeck_code) {
        //登录
        var param = {
            aid: aid,
            remarks: remarks
        };
        var url = window.apibase + "/Back_Aftermarket/AddRemarks";
        c.post(param, url, cb, function (err) { c.msgtips(err, "e"); }, ischeck_code)
    }

    return {
        GetAftermarketList: GetAftermarketList,
        SetAftermarketStatus: SetAftermarketStatus,
        AddRemarks: AddRemarks
    }
})