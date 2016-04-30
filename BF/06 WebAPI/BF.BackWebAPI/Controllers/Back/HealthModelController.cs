using BF.BackWebAPI.Authorize;
using BF.Common.CommonEntities;
using BF.Common.CustomException;
using BF.Common.DataAccess;
using BF.Common.Enums;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace BF.BackWebAPI.Controllers.Back
{
    public class HealthModelController : BackBaseController
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
        public HttpResponseMessage GetHealthModelList(int type = 0, int page = CommonConstant.PAGE, int pageSize = CommonConstant.PAGE_SIZE)
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
            dic.Add("Model_Type", type);

            apiResult.data = DBBaseFactory.DALBase.QueryForDataTable("BackWeb_GetHealthModelListByType", dic);
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
            if (!UserInfo.IsAdmin)
            {
                dic.Add("User_ID", UserInfo.ID);
            }
            apiResult.data = DBBaseFactory.DALBase.QueryForDataTable("BackWeb_GetHealthModelInfoByModelID", dic);
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        /// <summary>
        /// 根据模式ID 获取模式，需要检验当前用户是否有权限访问该模式
        /// </summary>
        /// <param name="modelID"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage AddHealthModel(string model_Name, string icoUrl = "", string imageUrl = "", string introduce = "", string describe = "", string remarks = "", int sort = 0, bool isBubble = false, int bubble_Time = 0, int bubble_Temperature = 0, int cook_Time = 0, int cook_Temperature = 0, bool is_Heat_Preservation = false, int heat_Preservation_Time = 0, int heat_Preservation_Temperature = 0, int removal_Chlorine_Time = 0, int final_Temperature = 0, bool isFerv = false)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            if (string.IsNullOrWhiteSpace(model_Name) || cook_Time <= 0 || final_Temperature <= 0)
            {
                throw new BusinessException("请填写完整数据在提交");
            }
            if (cook_Temperature <= 0)
            {
                cook_Temperature = final_Temperature;
            }
            //需要生成对应的信息
            if (string.IsNullOrEmpty(introduce))
            {
                introduce = string.Format("目标温度:{0}、", final_Temperature);
                if (isFerv)
                {
                    introduce = string.Format("{0}需要煮沸、", introduce);
                }
                if (removal_Chlorine_Time > 0)
                {
                    introduce = string.Format("{0}除氯{1}分钟、", introduce, removal_Chlorine_Time);
                }
                if (isBubble)
                {
                    introduce = string.Format("{0}{1}度下泡料{2}分钟、", introduce, bubble_Temperature, bubble_Time);
                }
                introduce = string.Format("{0}煮料{1}分钟到{2}度、", introduce, cook_Time, cook_Temperature);
                if (heat_Preservation_Temperature > 0)
                {
                    introduce = string.Format("保温在{0}度、", heat_Preservation_Temperature);
                }
                introduce = introduce.Substring(0, introduce.Length - 1);
            }
            //需要生成对应的信息
            if (string.IsNullOrEmpty(remarks))
            {
               
            }
            if (!isBubble && (bubble_Time > 0 || bubble_Temperature > 0))
            {
                isBubble = true;
            }
            if (!is_Heat_Preservation && (heat_Preservation_Time > 0 || heat_Preservation_Temperature > 0))
            {
                is_Heat_Preservation = true;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Model_Name", model_Name);
            dic.Add("User_ID", UserInfo.ID);
            dic.Add("IcoUrl", string.IsNullOrWhiteSpace(icoUrl) ? "" : icoUrl);
            dic.Add("ImageUrl", string.IsNullOrWhiteSpace(imageUrl) ? "" : imageUrl);
            dic.Add("Introduce", introduce);
            dic.Add("Describe", string.IsNullOrWhiteSpace(describe) ? "" : describe);
            dic.Add("Remarks", remarks);
            dic.Add("Sort", sort);
            dic.Add("IsBubble", isBubble);
            dic.Add("Bubble_Time", bubble_Time);
            dic.Add("Bubble_Temperature", bubble_Temperature);
            dic.Add("Cook_Time", cook_Time);
            dic.Add("Cook_Temperature", cook_Temperature);
            dic.Add("Is_Heat_Preservation", is_Heat_Preservation);
            dic.Add("Heat_Preservation_Time", heat_Preservation_Time);
            dic.Add("Heat_Preservation_Temperature", heat_Preservation_Temperature);
            dic.Add("Removal_Chlorine_Time", removal_Chlorine_Time);
            dic.Add("Final_Temperature", final_Temperature);
            dic.Add("IsFerv", isFerv);
            dic.Add("Model_Type", UserInfo.IsAdmin ? (int)Model_Type.System : (int)Model_Type.Custom);
            dic.Add("Model_Status", UserInfo.IsAdmin ? (int)Model_Status.Public : (int)Model_Status.Private);
            dic.Add("CreationUser", UserInfo.UserAccount ?? UserInfo.ID + "");
            apiResult.data = DBBaseFactory.DALBase.ExecuteNonQuery("BackWeb_AddHealthModel", dic);
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
        /// <summary>
        /// 设置常用
        /// </summary>
        /// <param name="modelID"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage SetCommonModel(int modelID)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ModelID", modelID);
            dic.Add("UserAccount", UserInfo.UserAccount ?? UserInfo.ID + "");
            dic.Add("User_ID", UserInfo.ID);
            apiResult.data = DBBaseFactory.DALBase.QueryForDataTable("BackWeb_SetCommonModel", dic);
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        /// <summary>
        /// 取消常用
        /// </summary>
        /// <param name="modelID"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage CancelCommonModel(int modelID)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ModelID", modelID);
            dic.Add("UserAccount", UserInfo.UserAccount ?? UserInfo.ID + "");
            dic.Add("User_ID", UserInfo.ID);
            apiResult.data = DBBaseFactory.DALBase.QueryForDataTable("BackWeb_CancelCommonModel", dic);
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        /// <summary>
        /// 获取常用模式列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [BFAuthorizeAttribute(IsLogin = true)]
        public HttpResponseMessage GetCommonHealthModelList()
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("User_ID", UserInfo.ID);
            apiResult.data = DBBaseFactory.DALBase.QueryForDataTable("BackWeb_GetCommonHealthModelList", dic);
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
    }
}