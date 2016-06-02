define(['zepto'], function () {

    var op = '<option value="CODE">NAME</option>';
    var empty = op.replace(/CODE/, "").replace(/NAME/, "请选择");
    var isweixin = IsWeiXinBrowser();
    var ReturnUrl = GetUrlParam("ReturnUrl");
    if (ReturnUrl.length <= 0) {
        ReturnUrl = "/WebPage/index.html"; //需要修改
    }

    
    //(function () {
    //    /*alert(Cookies.Get("WECHAT_OPENID_SAFE"));*/
    //    //is_gethospital();
    //    //alert(document.cookie);
    //    $('div[template]').each(function () {
    //        var obj = $(this);
    //        var tempurl = window.webbase + $(obj).attr('template');
    //        $.ajax({
    //            type: 'get',
    //            url: tempurl,
    //            data: null,
    //            async: true,
    //            dataType: 'html',
    //            success: function (d) {
    //                $(obj).html(d.toString())
    //            },
    //            error: function (e) {
    //                alert(e)
    //            }
    //        })
    //    })

    //})()

    function FormPage() {
        $('input,textare').attr('dir', 'rtl');
        $('input,textare').attr('onfocus', "this.setAttribute('dir', 'ltr');");
        $('input,textare').attr('onblur', "if(!this.getAttribute('notdir'))this.setAttribute('dir', 'rtl');");
        $('input[readonly]').attr('onfocus', '');
        $('input[readonly]').attr('onblur', '');
        $('input[readonly]').attr('dir', 'ltr');
        $('input[notdir]').attr('dir', 'ltr');
    }

    function Checkbox(onCheck, onCheckAll) {
        $('.checkbox-all').off('click');
        $('.checkbox').off('click');

        $('.checkbox').on('click', function () {
            var checked = $(this).attr('check') == "1" ? false : true;

            if (checked) {
                $(this).removeClass('checked');
                $(this).addClass('checked');
                $(this).attr('check', '1')
            } else {
                $(this).removeClass('checked');
                $(this).attr('check', '0')
            }

            onCheck($(this), checked)
        })

        $('.checkbox-all').on('click', function () {
            var checked = $(this).attr('check') == "1" ? false : true;
            if (checked) {
                $('.checkbox').removeClass('checked');
                $('.checkbox').addClass('checked')
                $('.checkbox').attr('check', 1)
                $(this).removeClass('checked');
                $(this).addClass('checked');
                $(this).attr('check', 1)
            } else {
                $('.checkbox').removeClass('checked');
                $('.checkbox').attr('check', 0)
                $(this).removeClass('checked');
                $(this).attr('check', 0)
            }

            onCheckAll(checked)
        })
    }

    function close() {
        WeixinJSBridge.call('closeWindow');
    }

    var pindex = 1;
    var psize = 10;
    var ptotal = 0;
    var pmaxindex = 0;

    var add_event = function (ele, eventname, func) {
        var oldevent = ele[eventname];
        if (typeof oldevent != 'function') {
            ele[eventname] = function () {
                func(ele);
            };
        } else {
            ele[eventname] = function () {
                func(ele);
                oldevent.apply(this, arguments);
            };
        }
    }

    function paging(callback) {

        callback(pindex, psize);

        add_event(window, 'onscroll', function () {
            var height = Screen.ScrollBar() + Screen.ViewHeight();
            var totalheight = Screen.Height();
            if (height + 100 >= totalheight) {
                //alert(pmaxindex)
                if (pindex < pmaxindex && pmaxindex != 0) {
                    //loading();
                    pindex += 1;
                    callback(pindex, psize);
                }
            }
        })

    }

    function upper_paging(callback) {

        callback(pindex, psize);

        add_event(window, 'onscroll', function () {
            var height = Screen.ScrollBar();
            var totalheight = Screen.Height();
            if (height <= 0) {
                //alert(pmaxindex)
                if (pindex < pmaxindex && pmaxindex != 0) {
                    //loading();
                    pindex += 1;
                    callback(pindex, psize);
                }
            }
        })

    }

    function setindex(i, total) {
        if (i > 0)
            pindex = i;
        if (total) {
            pmaxindex = Math.ceil(total / psize);
            ptotal = total;
        }
    }

    function _param(data) {
        var p = null;
        if (typeof data != "string") {
            p = {};
            for (key in data) {
                if (data[key] != undefined) {
                    p[key] = data[key];
                }
            }
        } else {
            p = data;
        }
        return p
    }

    function init_header(request) {
        //alert(is_gethospital());
        var session = Cookies.Get("CACHED_SESSION_ID");
        request.setRequestHeader("CACHED_SESSION_ID", session);
    }
    /*
	 *ischeck_response_code 是否调用统一的返回码校验，如果调用将会统一处理异常情况,默认校验，如果需要自定义函数处理指定的code就需要传入false
	 */
    function post(data, url, succ, err, ischeck_response_code) {
        if (ischeck_response_code == undefined || ischeck_response_code.length <= 0) {
            ischeck_response_code = true;
        }
        /*var hearders_data={
			User_ToKen:"",
			COOKIE_HOSPITAL_ID:401,
			App_Version:"1.0",
			Client_Type:4
		}*/

        $.ajax({
            type: 'post',
            url: url,
            beforeSend: function (request, settings) {
                init_header(request);
            },
            data: _param(data),
            dataType: 'json',
            success: function (data) {
                if (ischeck_response_code) {
                    check_response_code(data, succ);
                } else {
                    succ(data);
                }
            },
            error: function (err) {
                err(err);
                //loading(0);
            }
        })
    }
    /*
	 *ischeck_response_code 是否调用统一的返回码校验，如果调用将会统一处理异常情况,默认校验，如果需要自定义函数处理指定的code就需要传入false
	 */
    function get(data, url, succ, err, ischeck_response_code) {

        if (ischeck_response_code == undefined || ischeck_response_code.length <= 0) {
            ischeck_response_code = true;
        }
        /*var hearders_data={
			User_ToKen:"",
			COOKIE_HOSPITAL_ID:401,
			App_Version:"1.0",
			Client_Type:4
		}*/
        //console.log(_param(data));
        //console.log(url);
        $.ajax({
            type: 'get',
            url: url,
            beforeSend: function (request) {
                init_header(request);
            },
            data: _param(data),
            dataType: 'json',
            success: function (data) {
                if (ischeck_response_code) {
                    check_response_code(data, succ);
                } else {
                    succ(data);
                }
            },
            error: function (err) {
                err(err);
                //loading(0);
            }
        })
    }

    function GetUrlParam(name) {
        try {
            var reg = new RegExp("(^|&)" + name.toLowerCase() + "=([^&]*)(&|$)");
            var r = window.location.search.toLowerCase().substr(1).match(reg);
            if (r != null) return unescape(r[2]).replace(/</, "&lt;").replace(/>/, "&gt;");
            return "";
        } catch (e) { }
    }

    var Dialog = {
        show: function (msg) {
            //$('.modal').remove();
            var args = arguments,
				opt = args[0] || {};
            if (typeof opt != 'object') {
                // 兼容map
                opt = {};
                $.each({
                    1: 'tit',
                    2: 'okFn',
                    3: 'cancelFn',
                    4: 'okText',
                    5: 'icon',
                    6: 'subtit',
                    7: 'cancelText',
                    8: 'warn'
                }, function (k, v) {
                    if (args[k] != null) opt[v] = args[k];
                });
            }
            opt.tit = opt.tit ? opt.tit : "";
            opt.icon = opt.icon || false;
            opt.subtit = opt.subtit || "";
            //opt.okText = opt.okText || '确定';
            var show_btn_cancel = $.isFunction(opt.cancelFn) ? "" : "none";
            var show_btn_ok = opt.okText ? "" : "none";
            var show_btn = show_btn_cancel == "none" && show_btn_ok == "none" ? "none" : "";

            var str = '<div class="modal">\
			 <div class="inner">\
			<ul>\
			<li class="tit" style="display:' + (opt.tit ? "" : "none") + '"><b class="icon" style="display:' + (opt.icon ? "" : "none") + '"></b>' + opt.tit + '</li>\
			<li class="tit" style="display:' + (opt.warn ? "" : "none") + '"><img width="29" src="img/public/warn.png" style="vertical-align:-7px;") " />' + opt.warn + '</li>\
		<li class="sub-tit" style="display:' + (opt.subtit != "" ? "" : "none") + '">' + opt.subtit + '</li>\
	    <li class="con">' + (opt.con || "") + '</li>\
		<li class="button" style="display:' + show_btn + '">\
			<a href="javascript:;" id="btnCancel" class="btn-cancel" style="display:' + show_btn_cancel + '">' + (opt.cancelText || '取消') + '</a>\
			<a href="javascript:;" id="btnOk"  class="btn-ok" style="display:' + show_btn_ok + '">' + opt.okText + '</a>\
		</li>\
		</ul>\
		</div>\
		</div>';
            $('body').append(str);

            if (show_btn == "none") {
                $('.modal').addClass('tips')
                $(".modal").on("webkitAnimationEnd", function () {
                    $(".modal").remove();
                    if (opt.okFn) opt.okFn();
                })
            }

            return this.loaded(opt);
        },
        loaded: function (opt) {
            var _opt = opt || {};
            // console.log(opt);
            var btnok = document.getElementById("btnOk");
            var btncancel = document.getElementById("btnCancel");

            function _close(triggerDefaultAction) {
                if (triggerDefaultAction !== false && $.isFunction(_opt.cancelFn))
                    _opt.cancelFn.apply(null);
                $(".modal").remove();
            }

            function _ok(triggerDefaultAction) {
                if (triggerDefaultAction !== false && $.isFunction(_opt.okFn))
                    _opt.okFn.apply(null);
                $(".modal").remove();
            }

            //为了使btnok点击事件可移除
            btnok.onclick = _ok;
            //btnok.addEventListener('click', _ok, false);
            btncancel.onclick = _close;
            //btncancel.addEventListener('click', _close, false);

        }
    }

    //顶部提示
    //type:"e"是错误红色，"s"是成功绿色,callbackFun执行完回调
    function msgtips(msg, type, callbackFun, time) {
        if (!time) {
            time = 3 * 1000;
        } else {
            time = time * 1000;
        }
        if ($(".msgtips").length > 0) {
            $(".msgtips").remove();
        }
        if (!type) type = 's';

        var strinner = "<div class='msgtips " + type + "'>" + msg + "</div>";
        $(document.body).append(strinner);
        var timer = setTimeout(function () {
            var t = document.querySelector(".msgtips");
            $(t).addClass("out");
            var animationend = function () {
                $(t).remove();
                $.isFunction(callbackFun) ? callbackFun() : $.noop;
            }
            t.addEventListener("webkitAnimationEnd", animationend, false);
            t.addEventListener("animationend", animationend, false);
            clearTimeout(timer);
        }, time);
    }

    var time = 300,
		timeour = null,
		sending = false;

    function clock() {

        $(".sms").html(time + "s");
        if (time > 0) {
            timeour = setTimeout(clock, 1000);
        } else {
            sending = false;
            $(".sms").html("重新获取");
            $(".sms").removeClass("sended");

        }
        time--;
    };

    /**
	 * @param {Number} type 	1：激活会员，2：注册用户，3：找回密码，4：修改手机号码
	 */
    function init_send_sms(type) {
        if (!type) {
            type = 2;
        }
        $(".sms").click(function () {
            var _mobile = $("#txt-mobile");

            if (_mobile.val() == "") {
                msgtips("请输入手机号码", "e", null);
                return;
            }
            if (!/^(13[0-9]|15[0-9]|18[0-9]|14[0-9]|17[0-9])\d{8}$/.test(_mobile.val())) {
                msgtips("请输入正确的手机号码", "e", null);
                return;
            } else {
                var param = {
                    checkCodeType: type,
                    phoneNumber: _mobile.val()
                };
                if (sending) return;
                $(this).html("发送中...");
                var url = window.apibase + "/user_center/get_check_code";
                post(param, url, function (data) {
                    if (data.code == "200") {
                        sending = true;
                        $(".sms").addClass("sended");
                        msgtips("发送成功，请查看手机");
                        time = 60;
                        clock();
                    } else {
                        msgtips(data.msg, "e");
                        $(".sms").html("获取验证码");
                    }
                }, function (err) { }, false)
            }
        })
    }

    function getCityName(ip, callback) {
        var oHead = document.getElementsByTagName('HEAD').item(0);
        var oScript = document.createElement("script");
        oScript.language = "javascript";
        oScript.type = "text/javascript";
        oScript.defer = true;
        oScript.src = 'http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=js&ip=' + ip;
        oHead.appendChild(oScript);
        var timer = window.setInterval(function () {
            if (remote_ip_info) {
                callback(remote_ip_info)
                window.clearInterval(timer);
            }
        }, 100);
    }
    /*
	 *统一处理返回的验证码
	 *jimmy.pan 2016-03-30 add
	 */
    function check_response_code(data, cb) {
        //返回格式不符合规范不往下处理
        if (!data || data == null || !data.code || data.code == null) {
            return;
        }
        //msgtips(data.msg);
        var code = data.code;
        switch (code) {
            case "200":
                cb(data);
                break;
            case "300":
                alert(data.msg);
                cb(data);
                break;
            case "600":
                alert(600);
                window.location.href = window.webroot+'/sign-in.html?ReturnUrl=' + window.location.href;
                break;
            default:
                alert(data.msg);
                break;
        }
    }

    function ConvertToSex(sex) {
        var sexStr = "未知";
        switch (parseInt(sex)) {
            case 0:
                sexStr = "女";
                break;
            case 1:
                sexStr = "男";
                break;
        }
        return sexStr;
    }

    /*
	 *判断用户是否已绑定  
	 */
    function hasbinding(cb, ischeck_code) {
        //alert(cb);
        var openid_safe = Cookies.Get("WECHAT_OPENID_SAFE");
        var openid = Cookies.Get("WECHAT_OPENID");
        var param = {
            openID: openid,
            openSafe: openid_safe
        };

        var url = window.apibase + "/weixin_bind/hasbinding";
        post(param, url, cb, function (err) {
            c.msgtips(err, "e");
        }, ischeck_code)
    }
    return {
        post: post,
        get: get,
        GetUrlParam: GetUrlParam,
        msgtips: msgtips,
        init_send_sms: init_send_sms,
        Dialog: Dialog,
        addEvent: add_event,
        paging: paging,
        upper_paging: upper_paging,
        setindex: setindex,
        close: close,
        form_page: FormPage,
        init_chk: Checkbox,
        get_city_name: getCityName,
        check_response_code: check_response_code,
        ReturnUrl: ReturnUrl,
        ConvertToSex: ConvertToSex
    }
})