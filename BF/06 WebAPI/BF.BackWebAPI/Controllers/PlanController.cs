using BF.BackWebAPI.Models.RequestModels;
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
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS ,data=true};
            if (model == null || model.Plan_Time < DateTime.Now || model.Model_ID <= 0)
            {
                throw new BusinessException("请填写完整数据在提交");
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Plan_Time", model.Plan_Time);
            dic.Add("Model_ID", model.Model_ID);
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
            dic.Add("StartPlan_Time", startTime);
            dic.Add("EndPlan_Time", endTime);
            var ds = DBBaseFactory.DALBase.QueryForDataSet("BackWeb_GetPlanListByPlanTime", dic);
            var planList = ds.Tables[0];
            var planGroupeList = ds.Tables[1];
            apiResult.data = new { PlanList = planList, PlanGroupList = planGroupeList };
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
    }
}