define(['domready!', 'js/jweixin-1.0.0.js', "zepto"], function(doc, wxapi, $) {

	wx = window.wx || wxapi;

	$.ajax({
		type: "get",
		url: window.apibase + "/WeChatApi/Get_WX_Config",//window.apibase + "/common/jsapi_ticket",
		data: {
			path: encodeURIComponent(location.href.split('#')[0])
		},
		dataType: 'json',
		success: function(data) {
			if(data.code=='200'){
				wx.config(data.data);
			}

		},
		error: function(err) {}
	})

	function previewImage(current, allurls) {
		wx.previewImage({
			current: current,
			urls: allurls
		});
	}

	function chooseImage(cb, count, onlyzip) {
		var sizetype = ['original', 'compressed'];
		if (onlyzip === true)
			sizetype.shift();
		count = count ? count : 9;
		wx.chooseImage({
			count: count,
			sizeType: sizetype,
			sourceType: ['album', 'camera'],
			success: function(res) {
				var localIds = res.localIds;
				cb(localIds);
			},
			fail: function(err) {
				alert(JSON.stringify(err))
			},
			complete: function() {

			}
		});
	}

	function uploadImage(localId, cb) {
		try {
			wx.uploadImage({
				localId: localId,
				isShowProgressTips: 0,
				success: function(res) {
					var serverId = res.serverId;
					cb(serverId);
				}
			});
		} catch (e) {
			alert(e)
		}
	}

	//公众号支付
	function init_pay(order_no, price, desc, type, success) {
		order_no = order_no ? order_no : new Date().getTime();

		$.ajax({
			type: "get",
			url: window.payurl,
			data: {
				order: order_no,
				price: price, //单位为：分，比如25.5元，也就是2550分
				desc: desc, //最大32位，支持中文
				type: type,
				t: Math.random()
			},
			dataType: 'json',
			success: function(data) {
				if (data.return_code == undefined) {
					if (data.errmsg) {
						alert(data.errmsg);
					} else {
						if (data.code_url) {
							var src = window.resource + "temp/" + data.code_url;
							if (success) success(src);
						} else {
							data.success = function(res) {
								if (success) success();
							};
							wx.chooseWXPay(data);
						}
					}
				} else {
					if (data.msg) {
						alert(data.msg);
					}
				}
			},
			error: function(err) {
				alert(JSON.stringify(err))
			}
		})
	}

	/**
	 * @param {Object} options
	 * 		必填：
	 * 				targetN：终点维度
	 * 				targetE：终点经度
	 * 				mode：模式，transit(公交)、driving(开车，默认值)、walking(步行)
	 * 				region：地图所在地区(城市名)，例如：珠海市
	 * 		可选：
	 * 				originName：起点位置名称，默认值“我的位置”
	 * 				targetName：终点位置名称，默认值“终点”
	 * @param {function} cb 参数：url，百度地图api链接
	 */
	function getLocation(options, cb) {
		if (!options.originName) options.originName = "我的位置";
		if (!options.targetName) options.targetName = "终点";
		if (!options.mode) options.mode = 'driving'; //
		var url = 'http://api.map.baidu.com/direction?';

		var destination = '&destination=name:' + options.targetName + '|latlng:' + options.targetN + ',' + options.targetE;
		var mode = '&mode=' + options.mode;
		var region = '&region=' + options.region;
		var output = '&output=html';
		wx.getLocation({
			type: 'wgs84',
			success: function(res) {
				var latitude = res.latitude; // 纬度，浮点数，范围为90 ~ -90
				var longitude = res.longitude; // 经度，浮点数，范围为180 ~ -180。
				var speed = res.speed; // 速度，以米/每秒计
				var accuracy = res.accuracy; // 位置精度
				var origin = 'origin=name:' + options.originName + '|latlng:' + latitude + ',' + longitude;
				url = url + origin + destination + mode + region + output;
				cb(url,latitude,longitude)
			}
		});
	}

	//获取简单数据的微信回调
	function getSimpleLocation(Callback){
		wx.getLocation({
			type: 'wgs84',
			success: Callback
		});
	}
	//发起支付请求
	function chooseWXPay(data){
		wx.chooseWXPay(data);
	}

	function config(){
		$.ajax({
			type: "get",
			url: window.apibase + "/WeChatApi/Get_WX_Config",//window.apibase + "/common/jsapi_ticket",
			data: {
				path: encodeURIComponent(location.href.split('#')[0])
			},
			dataType: 'json',
			success: function(data) {
				if(data.code=='200'){
					wx.config(data.data);
				}

			},
			error: function(err) {}
		});
	}

	//通过ready接口处理成功验证
	function ready(Callback){
		wx.ready(function(){
			getSimpleLocation(Callback);
		})
	}
	//关闭当前浏览器
	function closeWindow(){
		wx.closeWindow();
	}

	return {
		img_choose: chooseImage,
		img_upload: uploadImage,
		init_pay: init_pay,
		img_preview: previewImage,
		gps: getSimpleLocation,
		chooseWXPay:chooseWXPay,
		ready:ready,
		getLocation:getLocation,
		closeWindow:closeWindow
	}
})