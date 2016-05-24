using BF.BackWebAPI.Models.ResponseModel;
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
        #region ---base---
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
        #endregion


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
            //DerviceCodeResponse wangguanCode = null;
            DerviceCodeResponse shuihuCode = null;
            MyDevices shuihu = GetDeviceStatus(device_identifier);
            if (shuihu != null)
            {
                //MyDevices wangguan = GetRouterStatus(shuihu.router_id);
                //if (wangguan != null)
                //{
                //    wangguanCode = DeleteDeviceByIdentifier(wangguan.device_identifier);

                shuihuCode = DeleteDeviceByIdentifier(shuihu.device_identifier);
                //}
            }

            //DeleteDerviceCodeResponse result = new DeleteDerviceCodeResponse();
            //if (wangguanCode != null)
            //{
            //    result.success = true;
            //    result.wangguan_qr_code = wangguanCode.qr_code;
            //}
            //if (shuihuCode != null)
            //{
            //    result.success = true;
            //    result.shuihu_qr_code = shuihuCode.qr_code;
            //}
            apiResult.data = shuihuCode;

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
            if (devices != null && devices.Count > 0)
            {
                //获取设备当前的状态
                MyDevices device = GetDeviceStatus(devices[0].device_identifier);
                if (device != null)
                {
                    devices[0].turned_on = device.turned_on;
                    devices[0].connectivity = device.connectivity;
                }
            }
            apiResult.data = devices;
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        [HttpGet]
        public HttpResponseMessage SetDefaultMyDevices(int uuid)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("MemberID", MemberInfo.ID);
            dic.Add("ID", uuid);
            int result = DBBaseFactory.DALBase.ExecuteNonQuery("HTSmart_Set_MyDevicesDefault", dic);
            if (result > 0)
                apiResult.data = true;
            else
                apiResult.data = false;
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        /// <summary>
        /// 查询设备是否在线状态
        /// </summary>
        /// <param name="device_identifier"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage QueryDeviceStatus(string device_identifier)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            MyDevices device = null;
            try
            {
                //获取设备当前的状态
                device = GetDeviceStatus(device_identifier);
            }
            catch (Exception ex)
            {
                apiResult.code = ResultCode.CODE_EXCEPTION;
                apiResult.msg = ex.Message;
            }
            apiResult.data = device;
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
        /// <summary>
        /// 获取某个通用模块的当前信息
        /// </summary>
        /// <param name="deviceID"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GenericModulesByID(int deviceID)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            GenericModules module = GetGenericModules(deviceID);

            apiResult.data = module;
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        /// <summary>
        /// 设置水壶的保温温度
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage SetGenericModulesModel(int deviceID, int model)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            GenericSuccess module = null;
            GenericModules shuihu = GetGenericModules(deviceID);
            if (shuihu != null && shuihu.connectivity == "在线" && shuihu.basics != null && shuihu.basics.bools.Count >= 4)
            {
                //水壶开关打开状态
                if (shuihu.basics.bools[0] == 1)
                {
                    string url = string.Format("http://huantengsmart.com:80/api/generic_modules/{0}/modes/0?mode={1}", deviceID, model);
                    Dictionary<string, string> headers = new Dictionary<string, string>();
                    headers.Add("Authorization", "bearer " + Access_Token);
                    string returnStr = HttpRequestHelper.Request(url, "PUT", 10, headers);
                    if (!string.IsNullOrEmpty(returnStr))
                    {
                        module = JsonConvert.DeserializeObject<GenericSuccess>(returnStr);
                    }
                }
                else
                {
                    apiResult.code = ResultCode.CODE_BUSINESS_ERROR;
                    apiResult.msg = "水壶已关";
                }
            }
            else
            {
                apiResult.code = ResultCode.CODE_BUSINESS_ERROR;
                if (shuihu == null)
                {
                    apiResult.msg = "获取状态失败";
                }
                else if (shuihu.connectivity == "离线")
                {
                    apiResult.msg = "设备离线";
                }
            }
            apiResult.data = module;
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
        /// <summary>
        /// 获取当前设备的水温
        /// </summary>
        /// <param name="deviceID"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetWaterTemperature(int deviceID)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            GenericContent module = null;
            GenericModules shuihu = GetGenericModules(deviceID);
            if (shuihu != null && shuihu.connectivity == "在线" && shuihu.basics != null && shuihu.basics.bools.Count >= 4)
            {
                string url = string.Format("http://huantengsmart.com:80/api/generic_modules/{0}/data/0", deviceID);
                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("Authorization", "bearer " + Access_Token);
                string returnStr = HttpRequestHelper.Request(url, "GET", 10, headers);
                if (!string.IsNullOrEmpty(returnStr))
                {
                    module = JsonConvert.DeserializeObject<GenericContent>(returnStr);
                }
            }

            apiResult.data = module;
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
        /// <summary>
        /// 更新设备的操作
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="model">如果参数不是-1 那就是设置水壶的温度</param>
        /// <param name="n"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage UpdateGenericModuleStatus(int deviceID, int model = -1, int n = -1, int flagInt = 0)
        {
            bool flag = flagInt == 0 ? false : true;
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            GenericSuccess module = null;
            GenericModules shuihu = GetGenericModules(deviceID);
            if (shuihu != null && shuihu.connectivity == "在线" && shuihu.basics != null && shuihu.basics.bools.Count >= 4)
            {
                string url = string.Empty;
                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("Authorization", "bearer " + Access_Token);
                if (n == 0 && flag == false)
                {//关闭水壶开关
                    url = string.Format("http://huantengsmart.com:80/api/generic_modules/{0}/bools/{1}?bool={2}", deviceID, n, flag);
                    HttpRequestHelper.Request(url, "PUT", 10, headers);
                }
                else
                {
                    //默认启动水壶开关
                    url = string.Format("http://huantengsmart.com:80/api/generic_modules/{0}/bools/{1}?bool={2}", deviceID, 0, true);
                    HttpRequestHelper.Request(url, "PUT", 10, headers);
                }


                if (model > -1 && model <= 9)//设置保温温度
                {
                    url = string.Format("http://huantengsmart.com:80/api/generic_modules/{0}/modes/0?mode={1}", deviceID, model);
                }
                else if (n > 0 && n <= 3)//操作水壶
                {
                    url = string.Format("http://huantengsmart.com:80/api/generic_modules/{0}/bools/{1}?bool={2}", deviceID, n, flag);
                }

                if (!string.IsNullOrEmpty(url))
                {
                    string returnStr = HttpRequestHelper.Request(url, "PUT", 10, headers);
                    if (!string.IsNullOrEmpty(returnStr))
                    {
                        module = JsonConvert.DeserializeObject<GenericSuccess>(returnStr);
                    }
                }
            }
            else
            {
                apiResult.code = ResultCode.CODE_BUSINESS_ERROR;
                if (shuihu == null)
                {
                    apiResult.msg = "获取状态失败";
                }
                else if (shuihu.connectivity == "离线")
                {
                    apiResult.msg = "设备离线";
                }
            }
            apiResult.data = module;
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
        #endregion


        #region ---方法---
        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="device_identifier"></param>
        /// <returns></returns>
        private DerviceCodeResponse DeleteDeviceByIdentifier(string device_identifier)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            DerviceCodeResponse deviceJson = null;
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
                    deviceJson = JsonConvert.DeserializeObject<DerviceCodeResponse>(returnStr);
                    if (deviceJson != null && deviceJson.success == true)
                    {
                        dic.Add("death_qr_code", deviceJson.qr_code);
                        dic.Add("User", MemberInfo.Account);

                        DBBaseFactory.DALBase.ExecuteNonQuery("HTSmart_Delete_Devices", dic);
                    }
                }

            }
            return deviceJson;
        }
        /// <summary>
        /// 判断设备是否在线
        /// </summary>
        /// <param name="device_identifier"></param>
        /// <returns></returns>
        private MyDevices GetDeviceStatus(string device_identifier)
        {
            MyDevices device = null;

            string url = string.Format("http://huantengsmart.com:80/api/devices/{0}", device_identifier);
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "bearer " + Access_Token);
            string returnStr = HttpRequestHelper.Request(url, "GET", 10, headers);
            if (!string.IsNullOrEmpty(returnStr))
            {
                device = JsonConvert.DeserializeObject<MyDevices>(returnStr);
            }
            return device;
        }

        /// <summary>
        /// 判断设备是否在线
        /// </summary>
        /// <param name="device_identifier"></param>
        /// <returns></returns>
        private MyDevices GetRouterStatus(int router_id)
        {
            MyDevices device = null;

            string url = string.Format("http://huantengsmart.com:80/api/routers/{0}", router_id);
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "bearer " + Access_Token);
            string returnStr = HttpRequestHelper.Request(url, "GET", 10, headers);
            if (!string.IsNullOrEmpty(returnStr))
            {
                device = JsonConvert.DeserializeObject<MyDevices>(returnStr);
            }
            return device;
        }

        /// <summary>
        /// 查询水壶当前状态
        /// </summary>
        /// <param name="deviceID"></param>
        /// <returns></returns>
        private GenericModules GetGenericModules(int deviceID)
        {
            GenericModules module = null;
            string url = string.Format("http://huantengsmart.com:80/api/generic_modules/{0}", deviceID);
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "bearer " + Access_Token);
            string returnStr = HttpRequestHelper.Request(url, "GET", 10, headers);
            if (!string.IsNullOrEmpty(returnStr))
            {
                module = JsonConvert.DeserializeObject<GenericModules>(returnStr);
            }
            return module;
        }
        #endregion
    }
}
