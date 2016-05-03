

$(window).scroll(function () {
    var scrollTop = 0;
    if (document.documentElement && document.documentElement.scrollTop) {
        scrollTop = document.documentElement.scrollTop
    } else {
        if (document.body) {
            scrollTop = document.body.scrollTop
        }
    }
    if (scrollTop > 200) {
        document.getElementById("backToTop").style.display = "block"
    } else {
        if (scrollTop < 200) {
            document.getElementById("backToTop").style.display = "none"
        }
    }
});

var Common = {

    //����ײ�ִ�лص��¼�
    reachbottom: function (callback) {
        $(window).scroll(function () {

            var scrollTop = 0; //���������붥���ĸ߶�
            if (document.documentElement && document.documentElement.scrollTop) {
                scrollTop = document.documentElement.scrollTop;
            } else if (document.body) {
                scrollTop = document.body.scrollTop;
            }

            var scrollHeight = $(document).height(); //��ǰҳ����ܸ߶�           ����
            var windowHeight = $(this).height(); //��ǰ���ӵ�ҳ��߶�
            if (scrollTop + windowHeight >= scrollHeight) { //���붥��+��ǰ�߶� >=�ĵ��ܸ߶� �����������ײ�

                callback();
            }
        })
    },
    //�ײ�Loading��ʽ
    bottom_loading: function (isshow) {

        if (isshow) {
            if ($(".bottom-loading").length == 0)
                $('<div class="bottom-loading"> Ŭ��������... </div>').appendTo($('body'));
        } else {
            $(".bottom-loading").remove();
        }

    },
    //ҳ���м�Loading��ʽ
    loading: function (isshow) {

        if (isshow) {
            if ($(".loading").length == 0)
                $('<div class="loading"><span class= "ui-icon-loading" ></span> </div>').appendTo($('body'));
        } else {
            $(".loading").remove();
        }

    },
    //��ѯ���û��������ʾ��ʽ
    search_nodata: function (isshow) {

        if (isshow) {
            if ($(".search_nodata").length == 0)
                $('<div class="search_nodata"><div class="img1">\
                <img src="../images/search_nodata.png" alt="">\
              </div>\
             <div class="tips">��Ǹ,û���ҵ���ز�Ʒ<br>��ı������������³���</div></div>').appendTo($('.main'));
        } else {
            $(".search_nodata").remove();
        }

    },
    UrlEncode: function (str) {
        return this.utf8_encode(escape(str));
    },
    UrlDecode: function (str) {
        return this.utf8_decode(unescape(str));
    },
    utf8_encode: function (string) {
        string = string.replace(/\r\n/g, "\n");
        var utftext = "";

        for (var n = 0; n < string.length; n++) {

            var c = string.charCodeAt(n);

            if (c < 128) {
                utftext += String.fromCharCode(c);
            }
            else if ((c > 127) && (c < 2048)) {
                utftext += String.fromCharCode((c >> 6) | 192);
                utftext += String.fromCharCode((c & 63) | 128);
            }
            else {
                utftext += String.fromCharCode((c >> 12) | 224);
                utftext += String.fromCharCode(((c >> 6) & 63) | 128);
                utftext += String.fromCharCode((c & 63) | 128);
            }

        }

        return utftext;
    },
    utf8_decode: function (utftext) {
        var string = "";
        var i = 0;
        var c = c1 = c2 = 0;

        while (i < utftext.length) {

            c = utftext.charCodeAt(i);

            if (c < 128) {
                string += String.fromCharCode(c);
                i++;
            }
            else if ((c > 191) && (c < 224)) {
                c2 = utftext.charCodeAt(i + 1);
                string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
                i += 2;
            }
            else {
                c2 = utftext.charCodeAt(i + 1);
                c3 = utftext.charCodeAt(i + 2);
                string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
                i += 3;
            }

        }

        return string;
    }
}
//��ȡ�ͷ�������
//ʹ�÷�����get��ʽʹ����ͬ��
// Request.post("SendSMs", "Captcha=" + code + "&mobileno=" + _mobile.val() + "&v=" + Math.random(),
//     function(data) {
//         if (data.Status == "0") {

//         } else {

//         }
//     })
var Request = {
    prefix: "",//"/Registe/",
    get: function (url, callback) {
        $.ajax({
            type: "GET",
            url: Request.prefix + url,
            //timeout: 1000,
            dataType: "xml",
            success: function (data) {

                callback(data);

            },
            error: function (data) {
                console.log(data);
            }
        });
    },
    post: function (url, data, callback) {

        $.ajax({
            type: "POST",
            data: data,
            url: Request.prefix + url,
            dataType: "xml",
            //timeout: 1000,
            success: function (data) {

                var jsonobj = eval('(' + data + ')');
                callback(jsonobj);
            },
            error: function (data) {
                console.log(data);
            }
        });
    },
    QueryString: function (name) {
        //����һ������Ŀ�������������ʽ����  
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        //ƥ��Ŀ�����  
        var r = window.location.search.substr(1).match(reg);
        //���ز���ֵ  
        if (r != null) return unescape(r[2]);
        return null;
    }
}
, messageid = Request.QueryString("messageId") || 0
, uuId = Request.QueryString("uuId") || 0
, index = Request.QueryString("index") || 1
, messageKeyId = Request.QueryString("messageKeyId") || 0;

//Cookie����
// ʹ�÷���
// д��
// Cookie.set("checkcode","123");
// ��ȡ(�ж��Ƿ����)
// var code = Cookie.get("checkcode");
// if (code) {}
var Cookie = {
    set: function (name, value, domain, path, hour) {
        var expire = new Date;
        if (hour) {
            var today = new Date;

            expire.setTime(today.getTime() + 36E5 * hour);
        }
        document.cookie = name + "=" + encodeURIComponent(escape(value)) + "; " + (hour ? "expires=" + expire.toGMTString() + "; " : "") + (path ? "path=" + path + "; " : "path=/; ") + (domain ? "domain=" + domain + ";" : "");
        return true;
    },
    get: function (name) {
        var r = new RegExp("(?:^|;+|\\s+)" + name + "=([^;]*)");
        var m = document.cookie.match(r);
        return unescape(decodeURIComponent(!m ? "" : m[1]));
    },
    del: function (name, domain, path) {
        document.cookie = name + "=; expires=Mon, 26 Jul 1997 05:00:00 GMT; " + (path ? "path=" + path + "; " : "path=/; ") + (domain ? "domain=" + domain + ";" : "");
    }
};
//�޸Ĺ�������
function modifyBuyNum(d, a) {
    var b;
    var c = $("#buynum");
    if (a == -1) {
        b = parseInt(c.val()) || 1;
        if (b == 1) {
            return
        } else {
            c.val(b + a)
        }
    } else {
        b = parseInt(c.val()) || 1;
        if (b == 999) {
            alert("�����������ܳ���999��");
            return;
        } else {

            c.val(b + a)
        }
    }
}
//���������ʧȥ������
function Check(obj) {
    obj.value = obj.value.replace(/\D/g, '');

    if (parseInt(obj.value) > 999) {
        alert("�����������ܳ���999��");
        return;
    }

    if ($(obj).val() == "" || $(obj).val() == "0") {
        alert("��������ȷ������1-999");
        $(obj).val("1");
    }
}

//xmlҳ����
function PageObj(page, key, parameters, before, after) {
    this.Page = page;
    this.Key = key;
    this.Parameters = parameters;
    this.BeforePage = before;
    this.AfterPage = after;
    return this;
}
;
