using BF.BackWebAPI.Models.Request;
using BF.Common.CommonEntities;
using BF.Common.CustomException;
using BF.Common.DataAccess;
using BF.Common.Enums;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
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
        public HttpResponseMessage AddPraise()
        {
            CommentAndPraise praise = new CommentAndPraise(1);
            if (praise.Source_ID <= 0)
            {
                throw new BusinessException("请选择赞的信息！");
            }
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS, data = true };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Source_ID", praise.Source_ID);
            dic.Add("Source_Type", praise.Source_Type);
            dic.Add("Accept_Comment_User_ID", praise.Accept_Comment_User_ID);
            dic.Add("Comment_Type", (int)Comment_Type.Praise);
            dic.Add("Comment_User_ID", this.MemberInfo.ID);
            dic.Add("Comment_Content", this.MemberInfo.Name ?? "" + "点赞");
            dic.Add("UserAccount", this.MemberInfo.Account ?? this.MemberInfo.ID + "");
            var executeCount = DBBaseFactory.DALBase.ExecuteNonQuery("BackWeb_AddPraise", dic);
            if (executeCount > 0)
            {
                UpdatePraiseCountAndCommentCount(praise.Source_ID, 1, praise.Source_Type, Comment_Type.Praise);
            }
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }


        /// <summary>
        /// 取消赞
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage CancelPraise()
        {
            CommentAndPraise praise = new CommentAndPraise(1);
            if (praise.Source_ID <= 0)
            {
                throw new BusinessException("请选择赞的信息！");
            }
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS, data = true };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Source_ID", praise.Source_ID);
            dic.Add("Source_Type", praise.Source_Type);
            dic.Add("Accept_Comment_User_ID", praise.Accept_Comment_User_ID);
            dic.Add("Comment_Type", (int)Comment_Type.Praise);
            dic.Add("Comment_User_ID", this.MemberInfo.ID);
            dic.Add("UserAccount", this.MemberInfo.Account ?? this.MemberInfo.ID + "");
            var executeCount = DBBaseFactory.DALBase.ExecuteNonQuery("BackWeb_CancelPraise", dic);
            if (executeCount > 0)
            {
                UpdatePraiseCountAndCommentCount(praise.Source_ID, -1, praise.Source_Type, Comment_Type.Praise);
            }
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddComment()
        {
            CommentAndPraise comment = new CommentAndPraise(1);
            if (comment.Source_ID <= 0)
            {
                throw new BusinessException("请选择赞的信息！");
            }
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS, data = true };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Source_ID", comment.Source_ID);
            dic.Add("Source_Type", comment.Source_Type);
            dic.Add("Accept_Comment_User_ID", comment.Accept_Comment_User_ID);
            dic.Add("Comment_Type", (int)Comment_Type.ImageText);
            dic.Add("Comment_User_ID", this.MemberInfo.ID);
            dic.Add("Comment_Content", comment.Comment_Content);
            dic.Add("UserAccount", this.MemberInfo.Account ?? this.MemberInfo.ID + "");
            var executeCount = DBBaseFactory.DALBase.ExecuteNonQuery("BackWeb_AddComment", dic);
            if (executeCount > 0)
            {
                UpdatePraiseCountAndCommentCount(comment.Source_ID, 1, comment.Source_Type, Comment_Type.ImageText);
            }
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }


        /// <summary>
        /// 删除评论
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage DeleteComment()
        {
            var commentID = HttpContext.Current.Request.Form["commentID"];
            if (string.IsNullOrWhiteSpace(commentID))
            {
                throw new BusinessException("请选择需要删除的评论信息！");
            }
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS, data = true };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Comment_ID", commentID);
            dic.Add("Comment_Type", (int)Comment_Type.ImageText);
            dic.Add("Comment_User_ID", this.MemberInfo.ID);
            dic.Add("UserAccount", this.MemberInfo.Account ?? this.MemberInfo.ID + "");
            var comment = DBBaseFactory.DALBase.QueryForObject<CommentAndPraise>("BackWeb_DeleteComment", dic);
            if(comment.Source_ID>0)
            {
                UpdatePraiseCountAndCommentCount(comment.Source_ID, -1, comment.Source_Type, Comment_Type.ImageText);
            }

            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source_ID">被评论或者赞的目标ID</param>
        /// <param name="count">1:评论和赞；-1:取消赞和删除评论</param>
        /// <param name="source_Type">评论源类型  1:分享; 2:养生品; 3:养生堂</param>
        /// <param name="comment_Type">评论类型 1.图文（评论） 2:赞</param>
        private void UpdatePraiseCountAndCommentCount(int source_ID,int count,int source_Type, Comment_Type comment_Type)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Source_ID", source_ID);
            dic.Add("UserAccount", this.MemberInfo.Account ?? this.MemberInfo.ID + "");
            if (comment_Type == Comment_Type.ImageText)
            {
                dic.Add("CommentCount", count);
            }
            else
            {
                dic.Add("PraiseCount", count);
            }
            switch (source_Type)
            {
                case (int)Comment_Source_Type.Share:
                    var executeCount = DBBaseFactory.DALBase.ExecuteNonQuery("BackWeb_UpdateSharePraiseCountAndCommentCount", dic);
                    break;
            }
        }
    }
}
