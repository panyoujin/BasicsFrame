﻿using BF.BackWebAPI.Authorize;
using BF.BackWebAPI.Models.Back.InParam;
using BF.BackWebAPI.Models.Request;
using BF.Common.CommonEntities;
using BF.Common.CustomException;
using BF.Common.DataAccess;
using BF.Common.Enums;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
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
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            if (model == null || model.Plan_Time<DateTime.Now || model.Model_ID <= 0 )
            {
                throw new BusinessException("请填写完整数据在提交");
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Plan_Time", model.Plan_Time);
            dic.Add("Model_ID", model.Model_ID);
            //dic.Add("AftermarketStatus", (int)AftermarketStatus.Wait);
            dic.Add("UserAccount", this.MemberInfo.Account ?? this.MemberInfo.ID + "");
            dic.Add("User_ID", this.MemberInfo.ID);
            DBBaseFactory.DALBase.ExecuteNonQuery("BackWeb_AddAftermarket", dic);
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

    }
}