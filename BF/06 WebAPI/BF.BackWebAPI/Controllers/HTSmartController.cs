using BF.Common.CommonEntities;
using BF.Common.DataAccess;
using BF.Common.Helper;
using BF.Common.StaticConstant;
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
                    Access_TokenJson tokenJson = JsonHelper.DeserializeObject<Access_TokenJson>(returnStr);
                    if (tokenJson != null)
                    {
                        Global.APPInfo.Access_Token = tokenJson.access_token;
                        Global.APPInfo.Expires_in = tokenJson.expires_in;
                        Global.APPInfo.Created_at = tokenJson.created_at;
                        Global.APPInfo.Token_Type = tokenJson.token_type;
                    }
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("ID", Global.APPInfo.ID);
                    dic.Add("Access_Token", Global.APPInfo.Access_Token);
                    dic.Add("Expires_in", Global.APPInfo.Expires_in);
                    dic.Add("Created_at", Global.APPInfo.Created_at);
                    dic.Add("Token_Type", Global.APPInfo.Token_Type);
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

        class Access_TokenJson
        {
            public string access_token { set; get; }
            public string token_type { set; get; }
            public int expires_in { set; get; }
            public string refresh_token { set; get; }
            public string scope { set; get; }
            public int created_at { set; get; }
        }
        //, string error = "", string error_description = "",string access_token="",string token_type="",int expires_in=0,string refresh_token="",string scope="",string created_at=""
    }
}
