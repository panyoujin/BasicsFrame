using BF.Common.CommonEntities;
using BF.Common.CustomException;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using System.Web.Http.Filters;

namespace BF.BackWebAPI.Authorize
{
    public class ExceptionAttribute : ExceptionFilterAttribute
    {
        private HttpActionExecutedContext _httpContext;

        private RequestHelper _requestInfo;
        public RequestHelper RequestInfo
        {
            get
            {
                if (_requestInfo == null)
                {
                    _requestInfo = new RequestHelper(_httpContext.Request);
                }
                return _requestInfo;
            }
        }
        public override void OnException(HttpActionExecutedContext context)
        {
            try
            {
                this._httpContext = context;
                string msg = string.Format("IP：{0},错误信息：{1}", RequestInfo.RequestIP, context.Exception.Message);
                if (context.Exception is NotLoginException || context.Exception is BusinessException)
                {
                    LogHelper.Info(msg);
                }
                else
                {
                    LogHelper.Info(msg, context.Exception);
                }
            }
            catch
            {

            }
            ApiResult<string> apiResult = new ApiResult<string> { code = ResultCode.CODE_EXCEPTION, msg = ResultMsg.CODE_EXCEPTION };
            if (context.Exception is NotLoginException)
            {
                apiResult.code = ResultCode.CODE_ERROR_USER_NOT_LOGIN;
                apiResult.msg = ResultMsg.CODE_ERROR_USER_NOT_LOGIN;
            }
            else if(context.Exception is BusinessException)
            {
                apiResult.code = ResultCode.CODE_BUSINESS_ERROR;
                apiResult.msg = context.Exception.Message;
            }
            context.Response = JsonHelper.SerializeObjectToWebApi(apiResult);
        }

    }
}
