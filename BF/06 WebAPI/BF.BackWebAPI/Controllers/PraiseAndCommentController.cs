using BF.BackWebAPI.Models.Request;
using BF.Common.CommonEntities;
using BF.Common.CustomException;
using BF.Common.DataAccess;
using BF.Common.Enums;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace BF.BackWebAPI.Controllers
{
    public class PraiseAndCommentController : BaseController
    {

        /// <summary>
        /// 赞
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddPraise([FromBody]CommentAndPraise praise)
        {
            if (praise.Source_ID <= 0)
            {
                throw new BusinessException("请选择赞的信息！");
            }
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Source_ID", praise.Source_ID);
            dic.Add("Source_Type", praise.Source_Type);
            dic.Add("Accept_Comment_User_ID", praise.Accept_Comment_User_ID);
            dic.Add("Comment_Type", (int)Comment_Type.Praise);
            dic.Add("Comment_User_ID", this.MemberInfo.ID);
            dic.Add("Comment_Content", this.MemberInfo.Name ?? "" + "点赞");
            dic.Add("UserAccount", this.MemberInfo.Account ?? this.MemberInfo.ID + "");
            var executeCount = DBBaseFactory.DALBase.ExecuteNonQuery("BackWeb_PraiseAndComment", dic);
            if (executeCount > 0)
            {
                dic.Add("PraiseCount", 1);
                switch (praise.Source_Type)
                {
                    case (int)Comment_Source_Type.Share:
                        executeCount = DBBaseFactory.DALBase.ExecuteNonQuery("BackWeb_UpdateSharePraiseCountAndCommentCount", dic);
                        break;
                }
            }
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }


        /// <summary>
        /// 取消赞
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage CancelPraise([FromBody]CommentAndPraise praise)
        {
            if (praise.Source_ID <= 0)
            {
                throw new BusinessException("请选择赞的信息！");
            }
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Source_ID", praise.Source_ID);
            dic.Add("Source_Type", praise.Source_Type);
            dic.Add("Accept_Comment_User_ID", praise.Accept_Comment_User_ID);
            dic.Add("Comment_Type", (int)Comment_Type.Praise);
            dic.Add("Comment_User_ID", this.MemberInfo.ID);
            dic.Add("UserAccount", this.MemberInfo.Account ?? this.MemberInfo.ID + "");
            var executeCount = DBBaseFactory.DALBase.ExecuteNonQuery("BackWeb_CancelPraiseAndDeleteComment", dic);
            if (executeCount > 0)
            {
                dic.Add("PraiseCount", -1);
                switch (praise.Source_Type)
                {
                    case (int)Comment_Source_Type.Share:
                        executeCount = DBBaseFactory.DALBase.ExecuteNonQuery("BackWeb_UpdateSharePraiseCountAndCommentCount", dic);
                        break;
                }
            }
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddComment([FromBody]CommentAndPraise comment)
        {
            if (comment.Source_ID <= 0)
            {
                throw new BusinessException("请选择赞的信息！");
            }
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Source_ID", comment.Source_ID);
            dic.Add("Source_Type", comment.Source_Type);
            dic.Add("Accept_Comment_User_ID", comment.Accept_Comment_User_ID);
            dic.Add("Comment_Type", (int)Comment_Type.Praise);
            dic.Add("Comment_User_ID", this.MemberInfo.ID);
            dic.Add("Comment_Content", comment.Comment_Content);
            dic.Add("UserAccount", this.MemberInfo.Account ?? this.MemberInfo.ID + "");
            var executeCount = DBBaseFactory.DALBase.ExecuteNonQuery("BackWeb_PraiseAndComment", dic);
            if (executeCount > 0)
            {
                dic.Add("CommentCount", 1);
                switch (comment.Source_Type)
                {
                    case (int)Comment_Source_Type.Share:
                        executeCount = DBBaseFactory.DALBase.ExecuteNonQuery("BackWeb_UpdateSharePraiseCountAndCommentCount", dic);
                        break;
                }
            }
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }


        /// <summary>
        /// 删除评论
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage DeleteComment([FromBody]CommentAndPraise comment)
        {
            if (comment.Source_ID <= 0)
            {
                throw new BusinessException("请选择需要删除的信息！");
            }
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Source_ID", comment.Source_ID);
            dic.Add("Source_Type", comment.Source_Type);
            dic.Add("Accept_Comment_User_ID", comment.Accept_Comment_User_ID);
            dic.Add("Comment_Type", (int)Comment_Type.Praise);
            dic.Add("Comment_User_ID", this.MemberInfo.ID);
            dic.Add("UserAccount", this.MemberInfo.Account ?? this.MemberInfo.ID + "");
            var executeCount = DBBaseFactory.DALBase.ExecuteNonQuery("BackWeb_CancelPraiseAndDeleteComment", dic);
            if (executeCount > 0)
            {
                dic.Add("CommentCount", -1);
                switch (comment.Source_Type)
                {
                    case (int)Comment_Source_Type.Share:
                        executeCount = DBBaseFactory.DALBase.ExecuteNonQuery("BackWeb_UpdateSharePraiseCountAndCommentCount", dic);
                        break;
                }
            }
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
    }
}
