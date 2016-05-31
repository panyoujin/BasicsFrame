using BF.BackWebAPI.Models.RequestModels;
using BF.BackWebAPI.Models.ResponseModel;
using BF.Common.CommonEntities;
using BF.Common.CustomException;
using BF.Common.DataAccess;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace BF.BackWebAPI.Controllers
{
    public class PlanController : BaseController
    {
        /// <summary>
        /// 添加计划
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddPlan([FromBody]AddPlanRequest model)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS, data = true };
            if (model == null || model.Model_ID <= 0)
            {
                throw new BusinessException("请填写完整数据在提交");
            }
            if (model.Plan_Time < DateTime.Now)
            {
                throw new BusinessException("计划时间需要大于当前时间");
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Plan_Time", model.Plan_Time);
            dic.Add("Model_ID", model.Model_ID);
            dic.Add("Plan_RGB", model.Plan_RGB);
            //dic.Add("AftermarketStatus", (int)AftermarketStatus.Wait);
            dic.Add("UserAccount", this.MemberInfo.Account ?? this.MemberInfo.ID + "");
            dic.Add("User_ID", this.MemberInfo.ID);
            DBBaseFactory.DALBase.ExecuteNonQuery("BackWeb_AddPlan", dic);
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        /// <summary>
        /// 获取计划
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetPlanList(DateTime startTime, DateTime endTime)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            endTime = endTime.AddDays(1);
            dic.Add("StartPlan_Time", startTime);
            dic.Add("EndPlan_Time", endTime);
            dic.Add("User_ID", this.MemberInfo.ID);
            var planList = DBBaseFactory.DALBase.QueryForList<PlanList>("BackWeb_GetPlanListByPlanTime", dic);
            var planGroupeList = DBBaseFactory.DALBase.QueryForList<PlanGroupList>("BackWeb_GetPlanGroupListByPlanTime", dic);
            //var planList = DBBaseFactory.DALBase.TableToList< PlanList>(ds.Tables[0]);
            //var planGroupeList = DBBaseFactory.DALBase.TableToList<PlanGroupList>(ds.Tables[1]);
            apiResult.data = new { planList = planList, planGroupeList = planGroupeList };
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        /// <summary>
        /// 获取计划
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetPlanListByDate(DateTime startTime, DateTime endTime)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("StartPlan_Time", startTime);
            dic.Add("EndPlan_Time", endTime);
            dic.Add("User_ID", this.MemberInfo.ID);
            apiResult.data = DBBaseFactory.DALBase.QueryForList<PlanGroupList>("BackWeb_GetPlanListByPlanTime", dic);
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
    }
}