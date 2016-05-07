using BF.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BF.BackWebAPI.Controllers
{
    /// <summary>
    /// 幻腾只能接口
    /// </summary>
    public class HTSmartController : BaseController
    {
        [HttpGet]
        public HttpResponseMessage RegisterAppCallBack(string code = "")
        {
            if (!string.IsNullOrEmpty(code))
            {
                string address = string.Format("https://huantengsmart.com/oauth2/token?client_id={0}&client_secret={1}&redirect_uri={2}&grant_type=authorization_code&code={3}",
                "6b6b48fcad497527c569fd49b7708294f21609038dd3e9b2f280b42cb7ac4d9b", "729bb36e1e54667cc4d474a57b5801fe10d6980d1b3cd36730d41516cd6ec508", "http%3A%2F%2Fwxtest.bosjk.com%2Fznsh%2FHTSmart%2FRegisterAppCallBack", code);

                string returnStr = HttpRequestHelper.Request(address, "POST", 10);
            }
            return null;
        }
        //, string error = "", string error_description = "",string access_token="",string token_type="",int expires_in=0,string refresh_token="",string scope="",string created_at=""
    }
}
