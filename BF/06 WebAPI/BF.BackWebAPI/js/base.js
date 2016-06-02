;
window.base = "http://115.29.49.135:1010/";
window.apibase = '/znsh';
window.webroot = '';
window.webbase = window.base+window.apibase;


var isScroll = true;
function is_gethospital() {
    var session = Cookies.Get("CACHED_SESSION_ID");
    //alert(session);
    if (session.length <= 0) {
        window.location.href = window.webroot + "/sign-in.html";
    }
    return 1;
}
document.addEventListener("touchmove", function(e) {
	if (!isScroll) {
		e.preventDefault();
		e.stopPropagation();
	}
}, false);

function loading(isshow, msg) {
	//msg = msg ? msg : "页面加载中...";
	//if (isshow == undefined || isshow == null) {
	//	var div = document.getElementById('wxloading');
	//	if (div) {
	//		document.getElementById('loadingMsg').innerHTML = msg;
	//	} else {
	//		div = document.createElement('div');
	//		div.setAttribute('class', 'wx_loading');
	//		div.setAttribute('id', 'wxloading');
	//		div.innerHTML = '<div class="wx_loading_inner"><i class="wx_loading_icon"></i><span id="loadingMsg">' + msg + '</span></div>';
	//		document.body.appendChild(div);
	//	}
	//	isScroll = false;
	//} else {
	//	var loading = document.getElementById('wxloading');
	//	loading && loading.parentNode && loading.parentNode.removeChild(loading);
	//	isScroll = true;
	//}
}

function defaultImg(e) {

	var type = $(e).attr("obj-type")
	if (!type) {
		type = "hos";
	}
	switch (type) {
		case "hos":
			e.src = 'img/test/1.png'
			break;
		case "doc":
			e.src = 'img/public/docDefault.png'
			break;
			/*default:
				e.src = 'img/test/1.png'
			break;*/
	}
}

window.onload = function() {
    is_gethospital();
	//$('body').hide();
	var notloading = ['static-service-intro.html'];
	var isloading = true;
	for (var i = 0; i < notloading.length; i++) {
		if (window.location.href.indexOf(notloading[i]) >= 0) isloading = false;
	}
	if (isloading) loading();
}

var Session = {
	Set: function(key, value) {
		window.sessionStorage.setItem(key, value);
	},
	Get: function(key) {
		return window.sessionStorage.getItem(key);
	},
	Del: function(key) {
		window.sessionStorage.removeItem(key);
	},
	Clear: function() {
		window.sessionStorage.clear();
	}
}

var Cache = {
	Set: function(key, value) {
		window.localStorage.setItem(key, value);
	},
	Get: function(key) {
		return window.localStorage.getItem(key);
	},
	Del: function(key) {
		window.localStorage.removeItem(key);
	},
	Clear: function() {
		window.localStorage.clear();
	}
}

var Screen = {
	/**
	 * 获取页面可视宽度
	 */
	ViewWidth: function() {
		var d = document;
		var a = d.compatMode == "BackCompat" ? d.body : d.documentElement;
		return a.clientWidth;
	},
	/**
	 * 获取页面浏览器的真实宽度
	 */
	Width: function() {
		var g = document;
		var a = g.body;
		var f = g.documentElement;
		var d = g.compatMode == "BackCompat" ? a : g.documentElement;
		return Math.max(f.scrollWidth, a.scrollWidth, d.clientWidth);
	},
	/**
	 * 获取页面可视高度
	 */
	ViewHeight: function() {
		var d = document;
		var a = d.compatMode == "BackCompat" ? d.body : d.documentElement;
		return a.clientHeight;
	},
	/**
	 * 获取页面浏览器的真实高度
	 */
	Height: function() {
		var g = document;
		var a = g.body;
		var f = g.documentElement;
		var d = g.compatMode == "BackCompat" ? a : g.documentElement;
		return Math.max(f.scrollHeight, a.scrollHeight, d.clientHeight);
	},
	/**
	 * 获取元素的绝对Y坐标
	 */
	Top: function(e) {
		try {
			var offset = e.offsetTop;
			if (e.offsetParent != null) offset += Screen.Top(e.offsetParent);
			return offset;
		} catch (e) {
			return -1;
		}
	},
	/**
	 * 获取元素的绝对X坐标
	 */
	Left: function(e) {
		var offset = e.offsetLeft;
		if (e.offsetParent != null) offset += Screen.Left(e.offsetParent);
		return offset;
	},
	/**
	 * 获取滚动条顶部距离页面顶部的高度
	 */
	ScrollBar: function() {
		var scrollPos;
		if (window.pageYOffset) {
			scrollPos = window.pageYOffset;
		} else if (document.compatMode && document.compatMode != 'BackCompat') {
			scrollPos = document.documentElement.scrollTop;
		} else if (document.body) {
			scrollPos = document.body.scrollTop;
		}
		return scrollPos;
	}
}
var Cookies = {
	/**
	 * 根据key获取cookie的值
	 */
	Get: function(key) {
		var arr, reg = new RegExp("(^| )" + key + "=([^;]*)(;|$)");
		if (arr = document.cookie.match(reg))
			return unescape(arr[2]);
		else
			return null;
	},
	/**
	 * 设置cookie的值，第3个参数days为可选参数，默认30天
	 */
	Set: function(key, value, days, hours, minutes, seconds) {
		var Days = (days > 0) ? days : 30;
		var Hours = (hours > 0) ? hours : 0;
		var Minutes = (minutes > 0) ? minutes : 0;
		var Seconds = (seconds > 0) ? seconds : 0;

		var exp = new Date();
		exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000 + Hours * 60 * 60 * 1000 + Minutes * 60 * 1000 + Seconds * 1000);
		document.cookie = key + "=" + escape(value) + ";expires=" + exp.toGMTString();
	},
	/**
	 * 根据指定的key删除cookie的值
	 */
	Del: function(key) {
		var exp = new Date();
		exp.setTime(exp.getTime() - 1);
		var cval = Cookies.Get(key);
		if (cval != null) {
			document.cookie = key + "=" + cval + ";expires=" + exp.toGMTString();
		}
	}
}

Date.prototype.ToShortDate = function() {
	if (!this) return "";
	var result = "";
	try {
		var date = this;
		var year = date.getFullYear().toString();
		var month = (date.getMonth() + 1);
		month = month < 10 ? "0" + month : month.toString();
		var day = date.getDate();
		day = day < 10 ? "0" + day : day.toString();

		result = year + "-" + month + "-" + day;
	} catch (e) {

	}
	return result;
}

Date.prototype.ToLongDate = function() {
	if (!this) return "";
	var result = this.ToShortDate();
	try {
		var date = this;
		var hour = date.getHours();
		hour = hour < 10 ? "0" + hour : hour.toString();
		var minute = date.getMinutes();
		minute = minute < 10 ? "0" + minute : minute.toString();
		var second = date.getSeconds();
		second = second < 10 ? "0" + second : second.toString();

		result += " " + hour + ":" + minute + ":" + second;
	} catch (e) {

	}
	return result;
}

Date.prototype.ToLongDate2 = function() {
	if (!this) return "";
	var result = this.ToShortDate();
	try {
		var date = this;
		var hour = date.getHours();
		hour = hour < 10 ? "0" + hour : hour.toString();
		var minute = date.getMinutes();
		minute = minute < 10 ? "0" + minute : minute.toString();

		result += " " + hour + ":" + minute;
	} catch (e) {

	}
	return result;
}

Date.prototype.Format = function(format) {
	var formatstr = format;
	if (format != null && format != "") {
		if (formatstr.indexOf("yyyy") >= 0) {
			formatstr = formatstr.replace("yyyy", this.getFullYear());
		}
		if (formatstr.indexOf("MM") >= 0) {
			var month = this.getMonth() + 1;
			if (month < 10) {
				month = "0" + month;
			}
			formatstr = formatstr.replace("MM", month);
		}
		if (formatstr.indexOf("dd") >= 0) {
			var day = this.getDate();
			if (day < 10) {
				day = "0" + day;
			}
			formatstr = formatstr.replace("dd", day);
		}
		var hours = this.getHours();
		if (formatstr.indexOf("HH") >= 0) {
			if (month < 10) {
				month = "0" + month;
			}
			formatstr = formatstr.replace("HH", hours);
		}
		if (formatstr.indexOf("hh") >= 0) {
			if (hours > 12) {
				hours = hours - 12;
			}
			if (hours < 10) {
				hours = "0" + hours;
			}
			formatstr = formatstr.replace("hh", hours);
		}
		if (formatstr.indexOf("mm") >= 0) {
			var minute = this.getMinutes();
			if (minute < 10) {
				minute = "0" + minute;
			}
			formatstr = formatstr.replace("mm", minute);
		}
		if (formatstr.indexOf("ss") >= 0) {
			var second = this.getSeconds();
			if (second < 10) {
				second = "0" + second;
			}
			formatstr = formatstr.replace("ss", second);
		}
	}
	return formatstr;
}

String.prototype.FormatMoney = function() {
	var result = "";
	if (this) {
		var s = this;
		var point = "00";
		if (s.indexOf('.') >= 0) {
			point = s.split('.')[1];
			s = s.split('.')[0];
		}
		if (s.length > 3) {
			while (s.length > 3) {
				var index = s.length - 3;
				var temp = s.substring(index, s.length);
				temp = result ? temp + ',' : temp;
				result = temp + result;
				s = s.substring(0, index);
			}
			result = "￥" + s + "," + result + "." + point;
		} else {
			result = "￥" + s + "." + point;
		}
	} else {
		return "";
	}
	return result;
}

/** 
 * 扩展方法：模拟C#的格式化字符串方法，仅支持6个占位符，{0}~{5}
 * */
String.prototype.Format = function(s1, s2, s3, s4, s5, s6) {
	var result = "";
	if (this.indexOf("{0}") >= 0) {
		result = this.replace(/\{0\}/ig, s1);
	}
	if (result.indexOf("{1}" >= 0)) {
		result = result.replace(/\{1\}/ig, s2);
	}
	if (result.indexOf("{2}" >= 0)) {
		result = result.replace(/\{2\}/ig, s3);
	}
	if (result.indexOf("{3}" >= 0)) {
		result = result.replace(/\{3\}/ig, s4);
	}
	if (result.indexOf("{4}" >= 0)) {
		result = result.replace(/\{4\}/ig, s5);
	}
	if (result.indexOf("{5}" >= 0)) {
		result = result.replace(/\{5\}/ig, s6);
	}
	return result;
}

String.prototype.HasEmoji = function() {
	var str = unescape(this.valueOf())
	var patts = [
		/[\ud800-\udbff][\udc00-\udfff]/g,
		/[\u2000-\u27ff]/g,
		/[\u2000-\u32ff][\ufe00-\ufeff]/g,
		/[\ufe00-\ufeff][\u2000-\u32ff]/g
	]
	var result = false;
	for (var i = 0; i < patts.length; i++) {
		result = result || patts[i].test(str);
	}
	return result;
}

String.prototype.Trim = function(is_global) {
	var str = this;
	var result;
	result = str.replace(/(^\s*)|(\s*$)/g, "");
	if (is_global && is_global.toLowerCase() == "g") {
		result = result.replace(/\s/g, "");
	}
	return result;
}

String.prototype.InputLength = function(len) {
	var str = this;
	var result = str;
	if (str.length > len) {
		//result = result.replace(/\d{len}/g, "");
		result = result.substring(0, len);
		//alert(result)
	}
	return result;
}

/**
 * 扩展方法：Html 解码
 * */
String.prototype.HtmlDecode = function() {
	var htmlstr = this;
	var s = "";
	if (htmlstr == null || htmlstr.length == 0) return "";
	s = htmlstr.replace(/&amp;/g, "&");
	s = s.replace(/&lt;/g, "<");
	s = s.replace(/&gt;/g, ">");
	s = s.replace(/&nbsp;/g, " ");
	s = s.replace(/&#39;/g, "\'");
	s = s.replace(/&quot;/g, "\"");
	s = s.replace(/<br>/g, "\n");
	return s;
}

/**
 * 扩展方法：Html  编码
 * */
String.prototype.HtmlEncode = function() {
	var htmlstr = this;
	var s = "";
	if (htmlstr == null || htmlstr.length == 0) return "";
	s = htmlstr.replace(/&/g, "&amp;");
	s = s.replace(/</g, "&lt;");
	s = s.replace(/>/g, "&gt;");
	s = s.replace(/ /g, "&nbsp;");
	s = s.replace(/\'/g, "&#39;");
	s = s.replace(/\"/g, "&quot;");
	return s;
}
Array.prototype.RemoveAt = function(n) {
	if (n < 0) {
		return this;
	} else {
		var newArray = [];
		for (var i = 0; i < this.length; i++) {
			if (i != n) newArray.push(this[i]);
		}
		return newArray;
	}
}

Array.prototype.Remove = function(value) {
	if (!value) {
		return this;
	} else {
		var newArray = [];
		for (var i = 0; i < this.length; i++) {
			if (value != this[i]) newArray.push(this[i]);
		}
		return newArray;
	}
}

Array.prototype.Clone = function() {
	var newArray = [];
	for (var i = 0; i < this.length; i++) {
		newArray.push(this[i]);
	}
	return newArray;
}

Array.prototype.IsInArray = function(n) {
	var index = -1;
	for (var i = 0; i < this.length; i++) {
		if (this[i] == n) {
			index = i;
		}
	}
	return index;
}
Array.prototype.insert = function(index, item) {
	this.splice(index, 0, item);
};

function Clone(obj) {
	var newObj = {};
	for (key in obj) {
		newObj[key] = obj[key];
	}
	return newObj;
}
/*
是否微信客户端
*/
function IsWeiXinBrowser() {
	var agent = navigator.userAgent.toLowerCase();
	var isweixin = false;
	if (agent.match(/MicroMessenger/i) == 'micromessenger') {
		isweixin = true;
	}
	return isweixin;
}

/*截取字符串，一个中文2个字符
str:源字符串
start:起始位置
len:截取长度
hasDot:是否在后面添加...
*/
function subString_zh(str, start, len, hasDot) {
	var newLength = 0;
	var newStr = "";
	var chineseRegex = /[^\x00-\xff]/g;
	var singleChar = "";
	var strLength = str.replace(chineseRegex, "**").length;
	for (var i = start; i < strLength; i++) {
		singleChar = str.charAt(i).toString();
		if (singleChar.match(chineseRegex) != null) {
			newLength += 2;
		} else {
			newLength++;
		}
		if (newLength > len) {
			break;
		}
		newStr += singleChar;
	}

	if (hasDot && strLength > len) {
		newStr += "...";
	}
	return newStr;
}
/*校验密码*/
function valid_pwd(pwd) {
	if (pwd.length < 6 || pwd.length > 18) {
		//长度6-18
		return false;
	}
	if (/[^\x00-\x7f]/.test(pwd)) {
		//汉字
		return false;
	}
	/*if (/^\d+$/.test(pwd)) {
		//密码不能全为数字
		return false;
	}
	if (/^[A-Za-z]+$/.test(pwd)) {
		//密码不能全为字母
		return false;
	}
	if (/^[^A-Za-z0-9]+$/.test(pwd)) {
		//密码不能全为字符
		return false;
	}*/
	return true;
};

/*校验手机号码*/
function valid_mobile(mobile) {
	if (!/^(13[0-9]|15[0-9]|18[0-9]|14[0-9]|17[0-9])\d{8}$/.test(mobile)) {
		return false;
	}
	return true;
};

/*校验诊疗卡*/
function valid_diagnosisCard(diagnosisCard) {
	if (diagnosisCard.length > 20) {
		//长度20
		return false;
	}
	if (!(/^\d+$/.test(diagnosisCard))) {
		return false;
	}
	return true;
};
//返回字符串长度
function getStringLength(str) {
	var newLength = 0;
	var chineseRegex = /[^\x00-\xff]/g;
	var singleChar = "";
	var strLength = str.replace(chineseRegex, "**").length;
	return strLength;
}

function StringTrim(str, is_global) {
	var result;
	result = str.replace(/(^\s*)|(\s*$)/g, "");
	if (is_global && is_global.toLowerCase() == "g") {
		result = result.replace(/\s/g, "");
	}
	return result;
}

function input_DiagnosisCard(value) {
	value = value.replace(/[^\d]/g, '');
	if (value.length > 20) {
		value = value.substring(0, 20);
	}
	return value;
}

function input_text(value) {
	value = value.replace(/(([^A-Za-z0-9\u4e00-\u9fa5\x21-\x7e，。；：‘“？、！【】｛｝《》*%@（）])[^，。；：‘“？、！【】｛｝《》*%@（）])/g, '');
	//value=value.replace(/([^`~!@#$%^&*()_+-=[\]\\{}|;':",.',.\/<>?][^·~！@#￥%……&*（）——+-=【】、｛｝|；；’‘，。、《》？][^A-Za-z0-9][^\u4e00-\u9fa5])/g,'');
	//value = value.Trim();
	/*var str = value.substring(value.length-1)
	code = str.charCodeAt(); 
	if(str.length>0)*/
	//alert(str+":"+code)

	//value=value.replace(/([^\x21-\x7e][^A-Za-z0-9][\u4e00-\u9fa5])/g,'');
	/*if(value.length>20){
		value=value.substring(0,20);
	}*/
	return value;
}

try {
	applicationCache.onprogress = function(e) {
		if (e.loaded < e.total) {
			loading(null, '正在加载资源' + e.loaded + '/' + e.total);
		} else {
			loading(0)
		}
	}
	applicationCache.onupdateready = function() {
		console.log('离线缓存已经更新');
		applicationCache.swapCache();
	}
} catch (e) {}