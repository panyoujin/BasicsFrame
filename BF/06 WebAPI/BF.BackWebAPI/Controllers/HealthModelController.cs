using BF.BackWebAPI.Authorize;
using BF.BackWebAPI.Models.Back.InParam;
using BF.BackWebAPI.Models.RequestModels;
using BF.BackWebAPI.Models.ResponseModel;
using BF.Common.CommonEntities;
using BF.Common.CustomException;
using BF.Common.DataAccess;
using BF.Common.Enums;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace BF.BackWebAPI.Controllers
{
    /// <summary>
    /// 前端模式管理页面
    /// </summary>
    public class HealthModelController : BaseController
    {

        /// <summary>
        /// 根据类型获取模式列表
        /// </summary>
        /// <param name="type">类型：0.系统提供;1.自定义</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetHealthModelList(int type = 0, string model_name = "", int page = CommonConstant.PAGE, int pageSize = CommonConstant.PAGE_SIZE)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            if (page <= 0)
            {
                page = 1;
            }
            if (pageSize <= 0)
            {
                pageSize = 10;
            }
            var startSize = this.GetStartSize(page, pageSize);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("StartSize", startSize);
            dic.Add("PageSize", pageSize);
            if (type >= 0)
            {
                dic.Add("Model_Type", type);
            }
            dic.Add("User_ID", 0);
            //如果获取自定义的，必须登录
            if (type == (int)Model_Types.Custom)
            {
                dic["User_ID"] = this.MemberInfo.ID;
            }
            else
            {
                try
                {

                    dic["User_ID"] = this.MemberInfo.ID;

                }
                catch (Exception ex)
                {
                    dic.Remove("User_ID");
                }
            }
            if (!string.IsNullOrWhiteSpace(model_name))
            {
                dic.Add("Model_Name", "%" + model_name + "%");
            }
            apiResult.data = DBBaseFactory.DALBase.QueryForList<HealthModelList>("FrontWeb_GetHealthModelListByType", dic);
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        /// <summary>
        /// 根据模式ID 获取模式，需要检验当前用户是否有权限访问该模式
        /// </summary>
        /// <param name="modelID"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetHealthModelInfo(int modelID)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ModelID", modelID);

            try
            {
                dic.Add("User_ID", this.MemberInfo.ID);
            }
            catch (Exception ex)
            {

            }
            apiResult.data = DBBaseFactory.DALBase.QueryForList<HealthModelInfo>("BackWeb_GetHealthModelInfoByModelID", dic);
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        /// <summary>
        /// 根据模式ID 获取模式，需要检验当前用户是否有权限访问该模式
        /// </summary>
        /// <param name="modelID"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetHealthModelInfoToHtml(int modelID)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ModelID", modelID);
            apiResult.data = DBBaseFactory.DALBase.QueryForObject<HealthModelInfo>("BackWeb_GetHealthModelInfoByModelID", dic);
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
        /// <summary>
        /// 根据模式ID 获取模式，需要检验当前用户是否有权限访问该模式
        /// </summary>
        /// <param name="modelID"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddHealthModel([FromBody]HealthModel healthModel)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            if (string.IsNullOrWhiteSpace(healthModel.model_Name) || healthModel.cook_Time <= 0 || healthModel.final_Temperature <= 0)
            {
                throw new BusinessException("请填写完整数据在提交");
            }
            if (healthModel.cook_Temperature <= 0)
            {
                healthModel.cook_Temperature = healthModel.final_Temperature;
            }
            //需要生成对应的信息
            if (string.IsNullOrEmpty(healthModel.introduce))
            {
                if (healthModel.isFerv || healthModel.final_Temperature >= 100 || healthModel.cook_Temperature >= 100)
                {
                    healthModel.introduce = string.Format("{0}需要煮沸、", healthModel.introduce);
                }
                else
                {
                    healthModel.introduce = string.Format("{0}目标温度:{1}度、", healthModel.introduce, healthModel.final_Temperature);
                }
                if (healthModel.removal_Chlorine_Time > 0)
                {
                    healthModel.introduce = string.Format("{0}需要除氯{1}分钟、", healthModel.introduce, healthModel.removal_Chlorine_Time);
                }
                if (healthModel.isBubble)
                {
                    healthModel.introduce = string.Format("{0}在{1}度下泡料{2}分钟、", healthModel.introduce, healthModel.bubble_Temperature, healthModel.bubble_Time);
                }
                healthModel.introduce = string.Format("{0}煮料{1}分钟到{2}度、", healthModel.introduce, healthModel.cook_Time, healthModel.cook_Temperature);
                if (healthModel.heat_Preservation_Temperature > 0)
                {
                    healthModel.introduce = string.Format("{0}保温在{1}度、", healthModel.introduce, healthModel.heat_Preservation_Temperature);
                }
                healthModel.introduce = healthModel.introduce.Substring(0, healthModel.introduce.Length - 1);
            }
            //需要生成对应的信息
            if (string.IsNullOrEmpty(healthModel.remarks))
            {

            }
            if (!healthModel.isBubble && (healthModel.bubble_Time > 0 || healthModel.bubble_Temperature > 0))
            {
                healthModel.isBubble = true;
            }
            if (!healthModel.is_Heat_Preservation && (healthModel.heat_Preservation_Time > 0 || healthModel.heat_Preservation_Temperature > 0))
            {
                healthModel.is_Heat_Preservation = true;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Model_Name", healthModel.model_Name);
            dic.Add("User_ID", this.MemberInfo.ID);
            dic.Add("IcoUrl", string.IsNullOrWhiteSpace(healthModel.icoUrl) ? "" : healthModel.icoUrl);
            dic.Add("ImageUrl", string.IsNullOrWhiteSpace(healthModel.imageUrl) ? "" : healthModel.imageUrl);
            dic.Add("Introduce", healthModel.introduce);
            dic.Add("Model_Describe", string.IsNullOrWhiteSpace(healthModel.describe) ? "" : healthModel.describe);
            dic.Add("Remarks", healthModel.remarks);
            dic.Add("Sort", healthModel.sort);
            dic.Add("IsBubble", healthModel.isBubble);
            dic.Add("Bubble_Time", healthModel.bubble_Time);
            dic.Add("Bubble_Temperature", healthModel.bubble_Temperature);
            dic.Add("Cook_Time", healthModel.cook_Time);
            dic.Add("Cook_Temperature", healthModel.cook_Temperature);
            dic.Add("Is_Heat_Preservation", healthModel.is_Heat_Preservation);
            dic.Add("Heat_Preservation_Time", healthModel.heat_Preservation_Time);
            dic.Add("Heat_Preservation_Temperature", healthModel.heat_Preservation_Temperature);
            dic.Add("Removal_Chlorine_Time", healthModel.removal_Chlorine_Time);
            dic.Add("Final_Temperature", healthModel.final_Temperature);
            dic.Add("IsFerv", healthModel.isFerv);
            dic.Add("Model_Type", this.MemberInfo.IsAdmin ? (int)Model_Types.System : (int)Model_Types.Custom);
            dic.Add("Model_Status", this.MemberInfo.IsAdmin ? (int)Model_Status.Public : (int)Model_Status.Private);
            dic.Add("CreationUser", this.MemberInfo.Account ?? this.MemberInfo.ID + "");
            dic.Add("WeChatUrl", healthModel.WeChatUrl);
            var key = "BackWeb_AddHealthModel";
            if (healthModel.MID > 0)
            {
                dic.Add("MID", healthModel.MID);
                key = "BackWeb_EditHealthModel";
            }
            apiResult.data = DBBaseFactory.DALBase.ExecuteNonQuery(key, dic);
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
        /// <summary>
        /// 设置常用
        /// </summary>
        /// <param name="modelID"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SetCommonModel([FromBody]CommonModel model)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            if (model == null)
            {
                model = new CommonModel();
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ModelID", model.modelID);
            dic.Add("UserAccount", this.MemberInfo.Account ?? this.MemberInfo.ID + "");
            dic.Add("User_ID", this.MemberInfo.ID);
            DBBaseFactory.DALBase.ExecuteNonQuery("BackWeb_SetCommonModel", dic);
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        /// <summary>
        /// 取消常用
        /// </summary>
        /// <param name="modelID"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage CancelCommonModel([FromBody]CommonModel model)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            if (model == null)
            {
                model = new CommonModel();
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ModelID", model.modelID);
            dic.Add("UserAccount", this.MemberInfo.Account ?? this.MemberInfo.ID + "");
            dic.Add("User_ID", this.MemberInfo.ID);
            DBBaseFactory.DALBase.ExecuteNonQuery("BackWeb_CancelCommonModel", dic);
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        /// <summary>
        /// 获取常用模式列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetCommonHealthModelList()
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("User_ID", this.MemberInfo.ID);
            apiResult.data = DBBaseFactory.DALBase.QueryForList<HealthModelList>("BackWeb_GetCommonHealthModelList", dic);
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
    }
}