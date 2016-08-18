using BF.BackWebAPI.Authorize;
using BF.BackWebAPI.BLL;
using BF.BackWebAPI.Models.Back.InParam;
using BF.BackWebAPI.Models.RequestModels;
using BF.BackWebAPI.Models.ResponseModel;
using BF.Common.CommonEntities;
using BF.Common.CustomException;
using BF.Common.DataAccess;
using BF.Common.Enums;
using BF.Common.FileProcess;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
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
        /// <param name="modelType_ID">类型ID</param>
        /// <param name="isCustom">是否自定义：0.系统提供;1.自定义</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetHealthModelList(int modelType_ID = 0, int isCustom = 0, string model_name = "", int page = CommonConstant.PAGE, int pageSize = CommonConstant.PAGE_SIZE)
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
            //var startSize = this.GetStartSize(page, pageSize);
            int startSize = 0;
            int endSize = 0;
            this.SetPageSize(page, pageSize, ref startSize, ref endSize);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("StartSize", startSize);
            dic.Add("EndSize", endSize);
            dic.Add("ModelType_ID", modelType_ID);
            dic.Add("IsCustom", isCustom);
            dic.Add("User_ID", 0);
            //如果获取自定义的，必须登录
            if (isCustom == (int)Model_Types.Custom)
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
                    //dic.Remove("User_ID");
                }
            }
            dic.Add("Model_Name", "");
            if (!string.IsNullOrWhiteSpace(model_name))
            {
                dic["Model_Name"] = "%" + model_name + "%";
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

            dic.Add("User_ID", 0);
            try
            {

                dic["User_ID"] = this.MemberInfo.ID;

            }
            catch (Exception ex)
            {
                //dic.Remove("User_ID");
            }
            apiResult.data = DBBaseFactory.DALBase.QueryForList<HealthModelInfo>("FrontWeb_GetHealthModelInfoByModelID", dic);
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
            dic.Add("User_ID", 0);
            try
            {

                dic["User_ID"] = this.MemberInfo.ID;

            }
            catch (Exception ex)
            {
                //dic.Remove("User_ID");
            }
            apiResult.data = DBBaseFactory.DALBase.QueryForObject<HealthModelInfo>("FrontWeb_GetHealthModelInfoByModelID", dic);
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
        /// <summary>
        /// 根据模式ID 获取模式，需要检验当前用户是否有权限访问该模式
        /// </summary>
        /// <param name="modelID"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddHealthModel()
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            HealthModel healthModel = new HealthModel();
            if (string.IsNullOrWhiteSpace(healthModel.model_Name) || string.IsNullOrWhiteSpace(healthModel.Param))
            {
                throw new BusinessException("请填写完整数据在提交");
            }
            //组装描述
            Dictionary<string, object> dic = new Dictionary<string, object>();

            dic.Add("IcoUrl", string.IsNullOrWhiteSpace(healthModel.icoUrl) ? "" : healthModel.icoUrl);
            dic.Add("ImageUrl", string.IsNullOrWhiteSpace(healthModel.imageUrl) ? "" : healthModel.imageUrl);
            Dictionary<string, object> paramInsert = new Dictionary<string, object>();
            try
            {
                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    paramInsert = FileProcessHelp.Save(HttpContext.Current.Request.Files[0], Global.AttmntServer);
                    dic["ImageUrl"] = paramInsert["AttachmentUrl"];
                    dic["IcoUrl"] = paramInsert["AttachmentUrl"];
                }
            }
            catch
            {
            }

            dic.Add("Model_Name", healthModel.model_Name);
            dic.Add("User_ID", this.MemberInfo.ID);
            if (string.IsNullOrWhiteSpace(healthModel.introduce))
            {
                healthModel.introduce = HealthModelBLL.GetIntroduceByParam(healthModel.Param);
            }
            dic.Add("Introduce", healthModel.introduce);
            dic.Add("Model_Describe", string.IsNullOrWhiteSpace(healthModel.describe) ? "" : healthModel.describe);
            dic.Add("Remarks", healthModel.remarks);
            dic.Add("Sort", healthModel.sort);
            dic.Add("Param", healthModel.Param);
            dic.Add("IsCustom", this.MemberInfo.IsAdmin ? (int)Model_Types.System : (int)Model_Types.Custom);
            dic.Add("Model_Status", this.MemberInfo.IsAdmin ? (int)Model_Status.Public : (int)Model_Status.Private);
            dic.Add("CreationUser", this.MemberInfo.Account ?? this.MemberInfo.ID + "");
            dic.Add("WeChatUrl", healthModel.WeChatUrl);
            dic.Add("ModelType_ID", healthModel.ModelType_ID);
            var key = "FrontWeb_AddHealthModel";
            if (healthModel.MID > 0)
            {
                dic.Add("MID", healthModel.MID);
                key = "FrontWeb_EditHealthModel";
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
            DBBaseFactory.DALBase.ExecuteNonQuery("FrontWeb_SetCommonModel", dic);
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
            DBBaseFactory.DALBase.ExecuteNonQuery("FrontWeb_CancelCommonModel", dic);
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
            apiResult.data = DBBaseFactory.DALBase.QueryForList<HealthModelList>("FrontWeb_GetCommonHealthModelList", dic);
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
    }
}