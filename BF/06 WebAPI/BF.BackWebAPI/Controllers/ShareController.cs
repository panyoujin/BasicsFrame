using BF.BackWebAPI.Models.RequestModels;
using BF.BackWebAPI.Models.ResponseModel;
using BF.Common.CommonEntities;
using BF.Common.CustomException;
using BF.Common.DataAccess;
using BF.Common.Enums;
using BF.Common.FileProcess;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace BF.BackWebAPI.Controllers
{
    public class ShareController : BaseController
    {
        /// <summary>
        /// 获取动态
        /// </summary>
        /// <param name="maxID">已经获取的最大ID</param>
        /// <param name="minID">已经获取的最小ID</param>
        /// <param name="request_type">获取类型：
        /// 1：获取当前没获取过的新动态
        ///  0：获取当前获取后的下一页
        /// </param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetShareListByID(int maxID = 0, int minID = 0, int request_type = 0, int pageSize = CommonConstant.PAGE_SIZE)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("MyUser_ID", this.MemberInfo.ID);
            if (request_type == 1)
            {
                if (maxID > 0)
                {
                    dic.Add("MaxID", maxID);
                }
            }
            else
            {
                if (pageSize <= 0)
                {
                    pageSize = 10;
                }
                if (minID > 0)
                {
                    dic.Add("MinID", minID);
                }
                dic.Add("StartSize", 0);
                dic.Add("PageSize", pageSize);
            }
            var spList = DBBaseFactory.DALBase.QueryForList<ShareListResponse>("BackWeb_GetShareListByUserID", dic) ?? new List<ShareListResponse>();
            if (spList != null && spList.Count > 0)
            {
                var tempID = spList.Max(s => s.ShareID);
                maxID = maxID > tempID ? maxID : tempID;

                tempID = spList.Min(s => s.ShareID);
                minID = minID > 0 && minID < tempID ? minID : tempID;
            }
            apiResult.data = new { ShareList = spList, MaxID = maxID, MinID = minID };
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
        /// <summary>
        /// 获取分享详情-评价列表和是否点赞
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetShareInfoByID(int shareID, int page = CommonConstant.PAGE, int pageSize = CommonConstant.PAGE_SIZE)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            int startSize = 0;
            int endSize = 0;
            this.SetPageSize(page, pageSize, ref startSize, ref endSize);
            Dictionary<string, object> dic = new Dictionary<string, object>();

            dic.Add("User_ID", this.MemberInfo.ID);
            dic.Add("Source_ID", shareID);
            dic.Add("StartSize", startSize);
            dic.Add("EndSize", endSize);


            var ds = DBBaseFactory.DALBase.QueryForDataSet("BackWeb_GetShareInfoByID", dic);
            var myPraiseCount = 0;
            if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {
                var obj = ds.Tables[1].Rows[0][0];
                int.TryParse(obj.ToString(), out myPraiseCount);
            }
            var commentList = DBBaseFactory.DALBase.TableToList<ShareInfoResponse>(ds.Tables[0]);
            apiResult.data = new { CommentList = commentList, MyPraiseCount = myPraiseCount };

            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
        /// <summary>
        /// 添加分享
        /// </summary>
        /// <param name="share"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddShare()
        {
            AddShareRequest share = new AddShareRequest();
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS, data = true };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Source_ID", share.Source_ID);
            dic.Add("Source_Type", share.Source_Type);
            dic.Add("ShareTitle", share.ShareTitle);
            dic.Add("ShareContent", share.ShareContent);
            dic.Add("ShareUrl", share.ShareUrl);
            dic.Add("User_ID", this.MemberInfo.ID);
            dic.Add("UserAccount", this.MemberInfo.Account ?? this.MemberInfo.ID + "");
            var obj = DBBaseFactory.DALBase.ExecuteScalar("BackWeb_AddShare", dic);
            int sID = 0;
            if (!int.TryParse(obj.ToString(), out sID) || sID <= 0)
            {
                throw new BusinessException("分享失败，请重试！");
            }
            //如果失败需要回滚
            for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
            {
                Dictionary<string, object> paramInsert = new Dictionary<string, object>();
                paramInsert = FileProcessHelp.Save(HttpContext.Current.Request.Files[i], Global.AttmntServer);
                paramInsert.Add("Source_ID", sID);
                paramInsert.Add("Attmnt_Type", (int)Share_Attmnt_Type.Share);
                paramInsert.Add("UserAccount", this.MemberInfo.Account ?? this.MemberInfo.ID + "");
                DBBaseFactory.DALBase.ExecuteNonQuery("BackWeb_AddShareAttmnt", paramInsert);
            }
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        /// <summary>
        /// 删除分享
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage DeleteShare()
        {
            var shareID = HttpContext.Current.Request.Form["shareID"];
            if (string.IsNullOrWhiteSpace(shareID))
            {
                throw new BusinessException("请选择需要删除的分享信息！");
            }
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS, data = true };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ID", shareID);
            dic.Add("User_ID", this.MemberInfo.ID);
            dic.Add("UserAccount", this.MemberInfo.Account ?? this.MemberInfo.ID + "");
            var executeCount = DBBaseFactory.DALBase.ExecuteNonQuery("BackWeb_DeleteShare", dic);
            if (executeCount <= 0)
            {
                throw new BusinessException("删除失败，请确认该分享是否已删除！");
            }
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
        /// <summary>
        /// 获取动态
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="maxID">已经获取的最大ID</param>
        /// <param name="minID">已经获取的最小ID</param>
        /// <param name="request_type">获取类型：
        /// 1：获取当前没获取过的新动态
        ///  0：获取当前获取后的下一页
        /// </param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetShareListByUserID(int userID, int maxID = 0, int minID = 0, int request_type = 0, int pageSize = CommonConstant.PAGE_SIZE)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };

            Dictionary<string, object> dic = new Dictionary<string, object>();
            //try
            //{
            dic.Add("MyUser_ID", this.MemberInfo.ID);
            //}
            //catch
            //{ }
            dic.Add("User_ID", userID);
            if (request_type == 1)
            {
                if (maxID > 0)
                {
                    dic.Add("MaxID", maxID);
                }
            }
            else
            {
                if (pageSize <= 0)
                {
                    pageSize = 10;
                }
                if (minID > 0)
                {
                    dic.Add("MinID", minID);
                }
                dic.Add("StartSize", 0);
                dic.Add("PageSize", pageSize);
            }
            var spList = DBBaseFactory.DALBase.QueryForList<ShareListResponse>("BackWeb_GetShareListByUserID", dic) ?? new List<ShareListResponse>();
            if (spList != null && spList.Count > 0)
            {
                var tempID = spList.Max(s => s.ShareID);
                maxID = maxID > tempID ? maxID : tempID;

                tempID = spList.Min(s => s.ShareID);
                minID = minID > 0 && minID < tempID ? minID : tempID;
            }
            apiResult.data = new { ShareList = spList, MaxID = maxID, MinID = minID };
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }


        /// <summary>
        /// 获取动态
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="maxID">已经获取的最大ID</param>
        /// <param name="minID">已经获取的最小ID</param>
        /// <param name="request_type">获取类型：
        /// 1：获取当前没获取过的新动态
        ///  0：获取当前获取后的下一页
        /// </param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetShareListByHot(int maxID = 0, int minID = 0, int request_type = 0, int pageSize = CommonConstant.PAGE_SIZE)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };

            Dictionary<string, object> dic = new Dictionary<string, object>();
            //try
            //{
            dic.Add("MyUser_ID", this.MemberInfo.ID);
            //}
            //catch
            //{ }
            if (request_type == 1)
            {
                if (maxID > 0)
                {
                    dic.Add("MaxID", maxID);
                }
            }
            else
            {
                if (pageSize <= 0)
                {
                    pageSize = 10;
                }
                if (minID > 0)
                {
                    dic.Add("MinID", minID);
                }
                dic.Add("StartSize", 0);
                dic.Add("PageSize", pageSize);
            }
            var spList = DBBaseFactory.DALBase.QueryForList<ShareListResponse>("BackWeb_GetShareListByHot", dic) ?? new List<ShareListResponse>();
            if (spList != null && spList.Count > 0)
            {
                var tempID = spList.Max(s => s.ShareID);
                maxID = maxID > tempID ? maxID : tempID;

                tempID = spList.Min(s => s.ShareID);
                minID = minID > 0 && minID < tempID ? minID : tempID;
            }
            apiResult.data = new { ShareList = spList, MaxID = maxID, MinID = minID };
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        [HttpGet]
        public HttpResponseMessage GetMyMessage(int page = CommonConstant.PAGE, int pageSize = CommonConstant.PAGE_SIZE)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            int startSize = 0;
            int endSize = 0;
            this.SetPageSize(page, pageSize, ref startSize, ref endSize);
            Dictionary<string, object> dic = new Dictionary<string, object>();

            dic.Add("User_ID", this.MemberInfo.ID);
            dic.Add("StartSize", startSize);
            dic.Add("EndSize", endSize);

            var obj = new { CommentID = 0, Comment_User_ID = 0, };
            apiResult.data = DBBaseFactory.DALBase.QueryForList<MyMessageResponse>("GetMyMessage", dic);

            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        [HttpPost]
        public HttpResponseMessage CleanMyMessage()
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };

            Dictionary<string, object> dic = new Dictionary<string, object>();

            dic.Add("User_ID", this.MemberInfo.ID);
            dic.Add("ModificationUser", this.MemberInfo.Name ?? this.MemberInfo.Account);


            apiResult.data = DBBaseFactory.DALBase.ExecuteNonQuery("CleanMyMessage", dic);

            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
    }
}