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
    public class Bask_HealthModelController : BaseController
    {

        /// <summary>
        /// 根据类型获取模式列表
        /// </summary>
        /// <param name="type">类型：0.系统提供;1.自定义</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [BFAuthorizeAttribute(IsLogin = true)]
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
            var m = this.MemberInfo;
            var startSize = this.GetStartSize(page, pageSize);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("StartSize", startSize);
            dic.Add("PageSize", pageSize);
            if (type >= 0)
            {
                dic.Add("Model_Type", type);
            }
            if (!string.IsNullOrWhiteSpace(model_name))
            {
                dic.Add("Model_Name", "%" + model_name + "%");
            }
            var ds = DBBaseFactory.DALBase.QueryForDataSet("BackWeb_GetHealthModelListByType", dic);
            int total = 0;
            int.TryParse(ds.Tables[1].Rows[0][0].ToString(), out total);
            apiResult.data = new { modelList =  DBBaseFactory.DALBase.TableToList<HealthModelList>(ds.Tables[0]),total= total };
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
            apiResult.data = DBBaseFactory.DALBase.QueryForObject<HealthModelInfo>("BackWeb_GetHealthModelInfoByModelID", dic);
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
        /// <summary>
        /// 根据模式ID 获取模式，需要检验当前用户是否有权限访问该模式
        /// </summary>
        /// <param name="modelID"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddHealthModel([FromBody]Bask_HealthModel healthModel)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            if (string.IsNullOrWhiteSpace(healthModel.Model_Name) || healthModel.Cook_Time <= 0 || healthModel.Final_Temperature <= 0)
            {
                throw new BusinessException("请填写完整数据在提交");
            }
            if (healthModel.Cook_Temperature <= 0)
            {
                healthModel.Cook_Temperature = healthModel.Final_Temperature;
            }
            //需要生成对应的信息
            if (string.IsNullOrEmpty(healthModel.Introduce))
            {
                if (healthModel.IsFerv || healthModel.Final_Temperature >= 100 || healthModel.Cook_Temperature >= 100)
                {
                    healthModel.Introduce = string.Format("{0}需要煮沸、", healthModel.Introduce);
                }
                else
                {
                    healthModel.Introduce = string.Format("{0}目标温度:{1}度、", healthModel.Introduce, healthModel.Final_Temperature);
                }
                if (healthModel.Removal_Chlorine_Time > 0)
                {
                    healthModel.Introduce = string.Format("{0}需要除氯{1}分钟、", healthModel.Introduce, healthModel.Removal_Chlorine_Time);
                }
                if (healthModel.IsBubble)
                {
                    healthModel.Introduce = string.Format("{0}在{1}度下泡料{2}分钟、", healthModel.Introduce, healthModel.Bubble_Temperature, healthModel.Bubble_Time);
                }
                healthModel.Introduce = string.Format("{0}煮料{1}分钟到{2}度、", healthModel.Introduce, healthModel.Cook_Time, healthModel.Cook_Temperature);
                if (healthModel.Heat_Preservation_Temperature > 0)
                {
                    healthModel.Introduce = string.Format("{0}保温在{1}度、", healthModel.Introduce, healthModel.Heat_Preservation_Temperature);
                }
                healthModel.Introduce = healthModel.Introduce.Substring(0, healthModel.Introduce.Length - 1);
            }
            //需要生成对应的信息
            if (string.IsNullOrEmpty(healthModel.Remarks))
            {

            }
            if (!healthModel.IsBubble && (healthModel.Bubble_Time > 0 || healthModel.Bubble_Temperature > 0))
            {
                healthModel.IsBubble = true;
            }
            if (!healthModel.Is_Heat_Preservation && (healthModel.Heat_Preservation_Time > 0 || healthModel.Heat_Preservation_Temperature > 0))
            {
                healthModel.Is_Heat_Preservation = true;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Model_Name", healthModel.Model_Name);
            dic.Add("User_ID", this.MemberInfo.ID);
            dic.Add("IcoUrl", string.IsNullOrWhiteSpace(healthModel.IcoUrl) ? "" : healthModel.IcoUrl);
            dic.Add("ImageUrl", string.IsNullOrWhiteSpace(healthModel.ImageUrl) ? "" : healthModel.ImageUrl);
            dic.Add("Introduce", healthModel.Introduce);
            dic.Add("Describe", string.IsNullOrWhiteSpace(healthModel.Describe) ? "" : healthModel.Describe);
            dic.Add("Remarks", healthModel.Remarks);
            dic.Add("Sort", healthModel.Sort);
            dic.Add("IsBubble", healthModel.IsBubble);
            dic.Add("Bubble_Time", healthModel.Bubble_Time);
            dic.Add("Bubble_Temperature", healthModel.Bubble_Temperature);
            dic.Add("Cook_Time", healthModel.Cook_Time);
            dic.Add("Cook_Temperature", healthModel.Cook_Temperature);
            dic.Add("Is_Heat_Preservation", healthModel.Is_Heat_Preservation);
            dic.Add("Heat_Preservation_Time", healthModel.Heat_Preservation_Time);
            dic.Add("Heat_Preservation_Temperature", healthModel.Heat_Preservation_Temperature);
            dic.Add("Removal_Chlorine_Time", healthModel.Removal_Chlorine_Time);
            dic.Add("Final_Temperature", healthModel.Final_Temperature);
            dic.Add("IsFerv", healthModel.IsFerv);
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
    }
}