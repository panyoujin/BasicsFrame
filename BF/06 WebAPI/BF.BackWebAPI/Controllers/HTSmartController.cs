using BF.Common.CommonEntities;
using BF.Common.DataAccess;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using Newtonsoft.Json;
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
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(Global.APPInfo.TokenRequest))
            {
                string address = Global.APPInfo.TokenRequest + code;

                string returnStr = HttpRequestHelper.Request(address, "POST", 10);
                if (!string.IsNullOrEmpty(returnStr))
                {
                    Access_TokenJson tokenJson = JsonConvert.DeserializeObject<Access_TokenJson>(returnStr);
                    //Access_TokenJson tokenJson = JsonHelper.Deserialize<Access_TokenJson>(returnStr);
                    if (tokenJson != null)
                    {
                        Global.APPInfo.Access_Token = tokenJson.access_token;
                        Global.APPInfo.Expires_in = tokenJson.expires_in;
                        Global.APPInfo.Created_at = tokenJson.created_at;
                        Global.APPInfo.Token_Type = tokenJson.token_type;
                        Global.APPInfo.Refresh_Token = tokenJson.refresh_token;

                    }
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("ID", Global.APPInfo.ID);
                    dic.Add("Access_Token", Global.APPInfo.Access_Token);
                    dic.Add("Expires_in", Global.APPInfo.Expires_in);
                    dic.Add("Created_at", Global.APPInfo.Created_at);
                    dic.Add("Token_Type", Global.APPInfo.Token_Type);
                    dic.Add("Refresh_Token", Global.APPInfo.Refresh_Token);

                    DBBaseFactory.DALBase.ExecuteNonQuery("HTSmart_UpdateAPPInfoToken", dic);
                    apiResult.data = tokenJson;
                }
                else
                {
                    apiResult.code = ResultCode.CODE_EXCEPTION;
                    apiResult.msg = "token请求返回空";
                }
            }
            else
            {
                apiResult.code = ResultCode.CODE_EXCEPTION;
                apiResult.msg = "回调code为空";
            }
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        [HttpGet]
        public HttpResponseMessage Refresh_Token()
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            string address = string.Format("https://huantengsmart.com/oauth2/token?grant_type=refresh_token&refresh_token={1}", Global.APPInfo.Refresh_Token);

            string returnStr = HttpRequestHelper.Request(address, "POST", 10);
            if (!string.IsNullOrEmpty(returnStr))
            {
                Access_TokenJson tokenJson = JsonConvert.DeserializeObject<Access_TokenJson>(returnStr);
                if (tokenJson != null)
                {
                    Global.APPInfo.Access_Token = tokenJson.access_token;
                    Global.APPInfo.Expires_in = tokenJson.expires_in;
                    Global.APPInfo.Created_at = tokenJson.created_at;
                    Global.APPInfo.Token_Type = tokenJson.token_type;
                    Global.APPInfo.Refresh_Token = tokenJson.refresh_token;

                }
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("ID", Global.APPInfo.ID);
                dic.Add("Access_Token", Global.APPInfo.Access_Token);
                dic.Add("Expires_in", Global.APPInfo.Expires_in);
                dic.Add("Created_at", Global.APPInfo.Created_at);
                dic.Add("Token_Type", Global.APPInfo.Token_Type);
                dic.Add("Refresh_Token", Global.APPInfo.Refresh_Token);

                DBBaseFactory.DALBase.ExecuteNonQuery("HTSmart_UpdateAPPInfoToken", dic);
                apiResult.data = tokenJson;
            }
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }



        #region --- model ---
        [Serializable]
        public class Access_TokenJson
        {
            public string access_token { set; get; }
            public string token_type { set; get; }
            public int expires_in { set; get; }
            public string refresh_token { set; get; }
            public string scope { set; get; }
            public int created_at { set; get; }
        }
        #endregion


        #region --- Operation ---
        [HttpGet]
        public HttpResponseMessage AddDevice(string qr_code)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };

            string url = string.Format("http://huantengsmart.com:80/api/devices?qr_code={0}", qr_code);
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "bearer " + Access_Token);

            string result = HttpRequestHelper.Request("http://huantengsmart.com:80/api/generic_modules", "GET", 10, headers);

            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
        #endregion
    }
}
