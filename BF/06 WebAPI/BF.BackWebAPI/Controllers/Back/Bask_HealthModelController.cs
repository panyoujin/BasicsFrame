using BF.BackWebAPI.Authorize;
using BF.BackWebAPI.BLL;
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
        /// <param name="modelType_ID">类型ID</param>
        /// <param name="isCustom">是否自定义：0.系统提供;1.自定义</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [BFAuthorizeAttribute(IsLogin = true)]
        public HttpResponseMessage GetHealthModelList(int modelType_ID = 0, int isCustom = -1, string model_name = "", int ModelTypeID = 0, int page = CommonConstant.PAGE, int pageSize = CommonConstant.PAGE_SIZE)
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

            dic.Add("ModelType_ID", modelType_ID);
            dic.Add("IsCustom", isCustom);

            dic.Add("Model_Name", "");
            if (!string.IsNullOrWhiteSpace(model_name))
            {
                dic["Model_Name"] = "%" + model_name + "%";
            }
            dic.Add("User_ID", 0);
            try
            {

                dic["User_ID"] = this.MemberInfo.ID;

            }
            catch (Exception ex)
            {
                //dic.Remove("User_ID");
            }
            var ds = DBBaseFactory.DALBase.QueryForDataSet("BackWeb_GetHealthModelListByType", dic);
            int total = 0;
            int.TryParse(ds.Tables[1].Rows[0][0].ToString(), out total);
            apiResult.data = new { modelList = DBBaseFactory.DALBase.TableToList<HealthModelList>(ds.Tables[0]), total = total };
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
            dic.Add("User_ID", 0);
            try
            {

                dic["User_ID"] = this.MemberInfo.ID;

            }
            catch (Exception ex)
            {
                //dic.Remove("User_ID");
            }
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
            if (string.IsNullOrWhiteSpace(healthModel.Model_Name) || string.IsNullOrWhiteSpace(healthModel.Param))
            {
                throw new BusinessException("请填写完整数据在提交");
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Model_Name", healthModel.Model_Name);
            dic.Add("User_ID", this.MemberInfo.ID);
            dic.Add("IcoUrl", string.IsNullOrWhiteSpace(healthModel.IcoUrl) ? "" : healthModel.IcoUrl);
            dic.Add("ImageUrl", string.IsNullOrWhiteSpace(healthModel.ImageUrl) ? "" : healthModel.ImageUrl);

            if (string.IsNullOrWhiteSpace(healthModel.Introduce))
            {
                healthModel.Introduce = HealthModelBLL.GetIntroduceByParam(healthModel.Param);
            }
            dic.Add("Introduce", healthModel.Introduce);
            dic.Add("Model_Describe", string.IsNullOrWhiteSpace(healthModel.Describe) ? "" : healthModel.Describe);
            dic.Add("Remarks", healthModel.Remarks);
            dic.Add("Sort", healthModel.Sort);
            dic.Add("Param", healthModel.Param);
            dic.Add("IsCustom", this.MemberInfo.IsAdmin ? (int)Model_Types.System : (int)Model_Types.Custom);
            dic.Add("Model_Status", this.MemberInfo.IsAdmin ? (int)Model_Status.Public : (int)Model_Status.Private);
            dic.Add("CreationUser", this.MemberInfo.Account ?? this.MemberInfo.ID + "");
            dic.Add("WeChatUrl", healthModel.WeChatUrl);
            dic.Add("ModelType_ID", healthModel.ModelType_ID);
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
        /// 根据模式ID 获取模式，需要检验当前用户是否有权限访问该模式
        /// </summary>
        /// <param name="modelID"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage DeleteHealthModelByModelID(int modelID)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ModelID", modelID);
            dic.Add("UserAccount", this.MemberInfo.Account);
            apiResult.data = DBBaseFactory.DALBase.QueryForObject<HealthModelInfo>("BackWeb_DeleteHealthModelByModelID", dic);
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
    }
}