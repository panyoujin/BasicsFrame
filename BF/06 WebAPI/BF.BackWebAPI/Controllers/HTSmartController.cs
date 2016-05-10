using BF.BackWebAPI.Models.Response;
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
        /// <summary>
        /// 幻腾添加设备接口
        /// </summary>
        /// <param name="qr_code"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage AddDevice(string qr_code)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };

            string url = string.Format("http://huantengsmart.com:80/api/devices?qr_code={0}", qr_code);
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "bearer " + Access_Token);

            try
            {
                string memberAccount = MemberInfo.Account;
                string returnStr = HttpRequestHelper.Request(url, "POST", 10, headers);
                if (!string.IsNullOrEmpty(returnStr))
                {
                    List<DeviceResponse> deviceJson = JsonConvert.DeserializeObject<List<DeviceResponse>>(returnStr);
                    if (deviceJson != null && deviceJson.Count > 0)
                    {
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        dic.Add("device_id", deviceJson[0].id);
                        dic.Add("device_identifier", deviceJson[0].device_identifier);
                        dic.Add("name", deviceJson[0].name);
                        dic.Add("device_type", deviceJson[0].device_type);
                        dic.Add("CreationUser", memberAccount);
                        dic.Add("MemberID", MemberInfo.ID);

                        DBBaseFactory.DALBase.ExecuteNonQuery("HTSmart_Add_Devices", dic);
                    }

                    apiResult.data = deviceJson;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "远程服务器返回错误: (400) 错误的请求。")
                {
                    apiResult.code = ResultCode.CODE_EXCEPTION;
                    apiResult.msg = "该二维码已经添加过";
                }
            }
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
        /// <summary>
        /// 幻腾删除设备接口
        /// </summary>
        /// <param name="device_identifier"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage DeleteDevice(string device_identifier)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();

            dic.Add("device_identifier", device_identifier);
            dic.Add("MemberID", MemberInfo.ID);

            DeviceResponse device = DBBaseFactory.DALBase.QueryForObject<DeviceResponse>("HTSmart_Get_Devices", dic);
            if (device != null)
            {
                string url = string.Format("http://huantengsmart.com:80/api/devices/{0}", device_identifier);
                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("Authorization", "bearer " + Access_Token);

                string returnStr = HttpRequestHelper.Request(url, "DELETE", 10, headers);

                if (!string.IsNullOrEmpty(returnStr))
                {
                    DerviceCodeResponse deviceJson = JsonConvert.DeserializeObject<DerviceCodeResponse>(returnStr);
                    if (deviceJson != null && deviceJson.success == true)
                    {
                        dic.Add("death_qr_code", deviceJson.qr_code);
                        dic.Add("User", MemberInfo.Account);

                        DBBaseFactory.DALBase.ExecuteNonQuery("HTSmart_Delete_Devices", dic);
                    }

                    apiResult.data = deviceJson;
                }

            }

            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        /// <summary>
        /// 获取我的设备
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage QueryMyDevices()
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("MemberID", MemberInfo.ID);

            List<MyDevices> devices = DBBaseFactory.DALBase.QueryForList<MyDevices>("HTSmart_Query_MyDevices", dic);

            //添加接口判断 默认设备是否在线状态
            apiResult.data = devices;
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        [HttpGet]
        public HttpResponseMessage SetDefaultMyDevices(int ID)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("MemberID", MemberInfo.ID);
            dic.Add("ID", ID);
            int result = DBBaseFactory.DALBase.ExecuteNonQuery("HTSmart_Set_MyDevicesDefault", dic);
            if (result > 0)
                apiResult.data = true;
            else
                apiResult.data = false;
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
        #endregion
    }
}
