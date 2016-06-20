$.extend($.fn.validatebox.defaults.rules, {    
    CHS: {
        validator: function (value, param) {
            return /^[\u0391-\uFFE5]+$/.test(value);
        },
        message: '请输入汉字'
    },
    ZIP: {
        validator: function (value, param) {
            return /^[1-9]\d{5}$/.test(value);
        },
        message: '邮政编码不存在'
    },
    QQ: {
        validator: function (value, param) {
            return /^[1-9]\d{4,10}$/.test(value);
        },
        message: 'QQ号码不正确'
    },
    phone: {// 验证电话号码 
        validator: function (value) {
            return /^((\(\d{2,3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7}(\-\d{1,4})?$/i.test(value);
        },
        message: '电话号码输入不正确'
    },
    mobile: {
        validator: function (value, param) {
            return /^((\(\d{2,3}\))|(\d{3}\-))?1\d{10}$/.test(value);
        },
        message: '手机号码不正确'
    },
    loginName: {
        validator: function (value, param) {
            return /^[\u0391-\uFFE5\w]+$/.test(value);
        },
        message: '登录名称只允许汉字、英文字母、数字及下划线。'
    },
    safepass: {
        validator: function (value, param) {
            return safePassword(value);
        },
        message: '密码由字母和数字组成，至少6位'
    },
    equalTo: {
        validator: function (value, param) {
            return value == $(param[0]).val();
        },
        message: '两次输入的字符不一样'
    },
    number: {
        validator: function (value, param) {
            return /^\d+$/.test(value);
        },
        message: '请输入数字'
    },
    Longitude: {
        validator: function (value, param) {
            return /^(-?((180)|(((1[0-7]\d)|(\d{1,2}))(\.\d+)?)))$/g.test(value);
        },
        message: '经度输入不正确'
    },
    Latitude: {
        validator: function (value, param) {
            return /^(-?((90)|((([0-8]\d)|(\d{1}))(\.\d+)?)))$/g.test(value);
        },
        message: '纬度输入不正确'
    },
    float: {
        validator: function (value, param) {
            //原正则不支持0或者0.00
            //return /^((\d+\.\d*[1-9]\d*)|(\d*[1-9]\d*\.\d+)|(\d*[1-9]\d*))$/.test(value);
            //可以支持千位符
            return /^-?0(\.\d+)?$|^-?[1-9]\d*(\.\d+)?$|^-?([1-9][0-9]{0,2},)(\d{3},)*(\d{3})(\.\d+)?$/.test(value);
        },
        message: '请输入数值'
    },
    integer: {
        validator: function (value, param) {
            return /^[\-\+]?\d+$/.test(value);
        },
        message: '请输入整数'
    },
    certificateNumber: {
        validator: function (value, param) {
            return /^[0-9]{15}$/.test(value);
        },
        message: '医生执业证书编号正确格式为15位数字'
    },
    idcard: {
        validator: function (value, param) {
            var rules = $.fn.validatebox.defaults.rules;
            rules.idcard.message = "";
            var checkvalue = true;
            var checkRes = idCard(value);
            if (checkRes == "OK") {
                checkvalue = true;
            }
            else {
                rules.idcard.message = checkRes;
                checkvalue = false;
            }
            return checkvalue;
        },
        message: '请输入正确的身份证号码'
    },
    TimeCheck: {
        validator: function (value, param) {
            var s = Date.parse($("input[name=" + param[0] + "]").val());
            //因为日期是统一格式的所以可以直接比较字符串 否则需要Date.parse(_date)转换   
            value = Date.parse(value);
            return value > s;
        },
        message: '非法数据'
    },
    CustomEmail: {
        validator: function (value, param) {
            var rules = $.fn.validatebox.defaults.rules;
            rules.CustomEmail.message = "";
            if (!rules.email.validator(value)) {
                rules.CustomEmail.message = "请输入正确的Email地址";
                return false;
            }
            var memberId = parseInt($(param[0]).val());
            var checkvalue = true;
            if (memberId >= 0) {
                $.ajax({
                    type: "get",
                    url: "/ExpMember/CheckEmail?&Email=" + value + "&MemberID=" + memberId,
                    async: false,
                    success: function (data) {
                        var result = eval('(' + data + ')');
                        if (result.Status == 1) {
                            rules.CustomEmail.message = 'Email地址已经注册,请更换另一个Email';
                            checkvalue = false;
                        }
                        else {
                            checkvalue = true;
                        }
                    },
                });
            }
            return checkvalue;
        },
        message: ''
    },
    CustomMobileNo: {
        validator: function (value, param) {
            var rules = $.fn.validatebox.defaults.rules;
            rules.CustomMobileNo.message = "";
            if (!rules.mobile.validator(value)) {
                rules.CustomMobileNo.message = "请输入正确的手机号码";
                return false;
            }
            var memberId = parseInt($(param[0]).val());
            var checkvalue = true;
            if (memberId >= 0) {
                $.ajax({
                    type: "get",
                    url: "/ExpMember/CheckMobileNo?&MobileNo=" + value + "&MemberID=" + memberId,
                    async: false,
                    success: function (data) {
                        var result = eval('(' + data + ')');
                        if (result.Status == 1) {
                            rules.CustomMobileNo.message = '手机号码已经注册,请更换另一个手机号码';
                            checkvalue = false;
                        }
                        else {
                            checkvalue = true;
                        }
                    },
                });
            }
            return checkvalue;
        },
        message: ''
    },
    CustomCardId: {
        validator: function (value, param) {
            var rules = $.fn.validatebox.defaults.rules;
            rules.CustomCardId.message = "";

            var checkvalue = true;
            var checkRes = idCard(value);
            if (checkRes == "OK") {
                checkvalue = true;
            }
            else {
                checkvalue = false;
                rules.CustomCardId.message = checkRes;
                return checkvalue;
            }

            var memberId = parseInt($(param[0]).val());

            if (memberId >= 0) {
                $.ajax({
                    type: "get",
                    url: "/ExpMember/CheckCardId?&CardId=" + value + "&MemberID=" + memberId,
                    async: false,
                    success: function (data) {
                        var result = eval('(' + data + ')');
                        if (result.Status == 0) {
                            rules.CustomCardId.message = '身份证已存在';
                            checkvalue = false;
                        }
                        else {
                            checkvalue = true;
                        }
                    },
                });
            }
            return checkvalue;
        },
        message: ''
    },
    CustomPhone: {
        validator: function (value, param) {
            var rules = $.fn.validatebox.defaults.rules;
            rules.CustomPhone.message = "";
            var checkvalue = true;
            if (rules.phone.validator(value) || rules.mobile.validator(value)) {
                checkvalue = true;
            }
            else {
                rules.CustomPhone.message = "您的输入有误，请重新输入";
                checkvalue = false;
            }
            return checkvalue;
        },
        message: ''
    },
    //支持多重验证, 使用如data-options="required:true,validType:'multiple[\'float\',\'SmallerThan\']'":
    multiple: {
        validator: function (value, vtypes) {
            var returnFlag = true;
            var opts = $.fn.validatebox.defaults;
            for (var i = 0; i < vtypes.length; i++) {
                var methodinfo = /([a-zA-Z_]+)(.*)/.exec(vtypes[i]);
                var rule = opts.rules[methodinfo[1]];
                if (value && rule) {
                    var parame = eval(methodinfo[2]);
                    if (!rule["validator"](value, parame)) {
                        returnFlag = false;
                        this.message = rule.message;
                        break;
                    }
                }
            }
            return returnFlag;
        }
    },
    LargerThan: {
        validator: function (value, param) {
            var checkResult = false;
            if (param.length > 0) {
                var targetValue = $("#" + param[0]).val();
                var int1 = parseInt(targetValue);
                var int2 = parseInt(value);
                if (targetValue.length == 0 || isNaN(int1)) {
                    return true;
                }
                if (!isNaN(int2) && int2 > int1) {
                    checkResult = true;
                }
            }

            var rules = $.fn.validatebox.defaults.rules;
            if (param.length > 1)
                rules.LargerThan.message = param[1];
            return checkResult;
        },
        message: '大于最小值'
    },
    SmallerThan: {
        validator: function (value, param) {
            var checkResult = false;
            if (param.length > 0) {
                var targetValue = $("#" + param[0]).val();
                var int1 = parseInt(targetValue);
                var int2 = parseInt(value);
                if (targetValue.length == 0 || isNaN(int1)) {
                    return true;
                }
                if (!isNaN(int2) && int2 < int1) {
                    checkResult = true;
                }
            }

            var rules = $.fn.validatebox.defaults.rules;
            if (param.length > 1)
                rules.SmallerThan.message = param[1];
            return checkResult;
        },
        message: '小于最大值'
    },
    Max: {
        validator: function (value, param) {
            var checkResult = false;
            if (param.length > 0) {
                var int1 = parseInt(param[0]);
                var int2 = parseInt(value);
                checkResult = int2 <= int1;
            }

            var rules = $.fn.validatebox.defaults.rules;
            rules.Max.message = "不可以大于" + param[0];
            return checkResult;
        }
    },
    Min: {
        validator: function (value, param) {
            var checkResult = false;
            if (param.length > 0) {
                var int1 = parseInt(param[0]);
                var int2 = parseInt(value);
                checkResult = int2 >= int1;
            }

            var rules = $.fn.validatebox.defaults.rules;
            rules.Min.message = "不可以小于" + param[0];
            return checkResult;
        }
    }
});

/* 密码由字母和数字组成，至少6位 */
var safePassword = function (value) {
    return !(/^(([A-Z]*|[a-z]*|\d*|[-_\~!@#\$%\^&\*\.\(\)\[\]\{\}<>\?\\\/\'\"]*)|.{0,5})$|\s/.test(value));
}

idCard = function (idcard) {
    idcard = idcard.toUpperCase();
    var Iderror;
    var IdCardErrors = new Array(
    "OK",
    "身份证号码位数不对!",
    "身份证号码出生日期超出范围或含有非法字符!",
    "身份证号码校验错误!",
    "身份证地区非法!",
    "请输入身份证号码"
    );
    if (idcard == "") return IdCardErrors[5];
    var area = { 11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江", 31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北", 43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏", 61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外" }
    var Y, JYM;
    var S, M;
    var idcard_array = new Array();
    idcard_array = idcard.split("");
    ///验证港澳身份证
    if ((/^[A-Za-z]\d{6}[\dA-Za-z]$|^[A-Za-z]\d{6}\([\dA-Za-z]\)$|^\d{7}\(\d\)$|^\d{8}$|^\d\/\d{6}\/\d$/).test(idcard)) return IdCardErrors[0];
    //地区检验
    if (area[parseInt(idcard.substr(0, 2))] == null) return IdCardErrors[4];
    //身份号码位数及格式检验 
    switch (idcard.length) {
        case 15:
            if ((parseInt(idcard.substr(6, 2)) + 1900) % 4 == 0 || ((parseInt(idcard.substr(6, 2)) + 1900) % 100 == 0 && (parseInt(idcard.substr(6, 2)) + 1900) % 4 == 0)) {
                ereg = /^[1-9][0-9]{5}[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))[0-9]{3}$/; //测试出生日期的合法性 
            } else {
                ereg = /^[1-9][0-9]{5}[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8]))[0-9]{3}$/; //测试出生日期的合法性 
            }
            if (ereg.test(idcard)) return IdCardErrors[0];
            else return IdCardErrors[2];
            break;
        case 18:
            //18位身份号码检测 
            //出生日期的合法性检查 
            //闰年月日:((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9])) 
            //平年月日:((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8])) 
            if (parseInt(idcard.substr(6, 4)) % 4 == 0 || (parseInt(idcard.substr(6, 4)) % 100 == 0 && parseInt(idcard.substr(6, 4)) % 4 == 0)) {
                ereg = /^[1-9][0-9]{5}((19[0-9]{2})|(200[0-9]|201[0-5]))((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))[0-9]{3}[0-9Xx]$/; //闰年出生日期的合法性正则表达式 
            } else {
                ereg = /^[1-9][0-9]{5}((19[0-9]{2})|(200[0-9]|201[0-5]))((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8]))[0-9]{3}[0-9Xx]$/; //平年出生日期的合法性正则表达式 
            }
            if (ereg.test(idcard)) {//测试出生日期的合法性 
                //计算校验位 
                S = (parseInt(idcard_array[0]) + parseInt(idcard_array[10])) * 7
        + (parseInt(idcard_array[1]) + parseInt(idcard_array[11])) * 9
        + (parseInt(idcard_array[2]) + parseInt(idcard_array[12])) * 10
        + (parseInt(idcard_array[3]) + parseInt(idcard_array[13])) * 5
        + (parseInt(idcard_array[4]) + parseInt(idcard_array[14])) * 8
        + (parseInt(idcard_array[5]) + parseInt(idcard_array[15])) * 4
        + (parseInt(idcard_array[6]) + parseInt(idcard_array[16])) * 2
        + parseInt(idcard_array[7]) * 1
        + parseInt(idcard_array[8]) * 6
        + parseInt(idcard_array[9]) * 3;
                Y = S % 11;
                M = "F";
                JYM = "10X98765432";
                M = JYM.substr(Y, 1); //判断校验位 
                if (M == idcard_array[17].toUpperCase()) return IdCardErrors[0]; //检测ID的校验位 
                else return IdCardErrors[3];
            }
            else return IdCardErrors[2];
            break;
        default:
            return IdCardErrors[1];
            break;
    };
}
/*
var idCard = function (value) {
    if (value.length == 18 && 18 != value.length) return false;
    var number = value.toLowerCase();
    var d, sum = 0, v = '10x98765432', w = [7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2], a = '11,12,13,14,15,21,22,23,31,32,33,34,35,36,37,41,42,43,44,45,46,50,51,52,53,54,61,62,63,64,65,71,81,82,91';
    var re = number.match(/^(\d{2})\d{4}(((\d{2})(\d{2})(\d{2})(\d{3}))|((\d{4})(\d{2})(\d{2})(\d{3}[x\d])))$/);
    if (re == null || a.indexOf(re[1]) < 0) return false;
    if (re[2].length == 9) {
        number = number.substr(0, 6) + '19' + number.substr(6);
        d = ['19' + re[4], re[5], re[6]].join('-');
    } else d = [re[9], re[10], re[11]].join('-');
    if (!isDateTime.call(d, 'yyyy-MM-dd')) return false;
    for (var i = 0; i < 17; i++) sum += number.charAt(i) * w[i];
    return (re[2].length == 9 || number.charAt(17) == v.charAt(sum % 11));
}
*/
var isDateTime = function (format, reObj) {
    format = format || 'yyyy-MM-dd';
    var input = this, o = {}, d = new Date();
    var f1 = format.split(/[^a-z]+/gi), f2 = input.split(/\D+/g), f3 = format.split(/[a-z]+/gi), f4 = input.split(/\d+/g);
    var len = f1.length, len1 = f3.length;
    if (len != f2.length || len1 != f4.length) return false;
    for (var i = 0; i < len1; i++) if (f3[i] != f4[i]) return false;
    for (var i = 0; i < len; i++) o[f1[i]] = f2[i];
    o.yyyy = s(o.yyyy, o.yy, d.getFullYear(), 9999, 4);
    o.MM = s(o.MM, o.M, d.getMonth() + 1, 12);
    o.dd = s(o.dd, o.d, d.getDate(), 31);
    o.hh = s(o.hh, o.h, d.getHours(), 24);
    o.mm = s(o.mm, o.m, d.getMinutes());
    o.ss = s(o.ss, o.s, d.getSeconds());
    o.ms = s(o.ms, o.ms, d.getMilliseconds(), 999, 3);
    if (o.yyyy + o.MM + o.dd + o.hh + o.mm + o.ss + o.ms < 0) return false;
    if (o.yyyy < 100) o.yyyy += (o.yyyy > 30 ? 1900 : 2000);
    d = new Date(o.yyyy, o.MM - 1, o.dd, o.hh, o.mm, o.ss, o.ms);
    var reVal = d.getFullYear() == o.yyyy && d.getMonth() + 1 == o.MM && d.getDate() == o.dd && d.getHours() == o.hh && d.getMinutes() == o.mm && d.getSeconds() == o.ss && d.getMilliseconds() == o.ms;
    return reVal && reObj ? d : reVal;
    function s(s1, s2, s3, s4, s5) {
        s4 = s4 || 60, s5 = s5 || 2;
        var reVal = s3;
        if (s1 != undefined && s1 != '' || !isNaN(s1)) reVal = s1 * 1;
        if (s2 != undefined && s2 != '' && !isNaN(s2)) reVal = s2 * 1;
        return (reVal == s1 && s1.length != s5 || reVal > s4) ? -10000 : reVal;
    }
};