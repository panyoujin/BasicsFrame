using System;

namespace BF.BackWebAPI.Models.ResponseModel
{
    public class ArticleListResponse
    {
        public int aID { get; set; }
        public string ArticleTitle { get; set; }
        public string ArticleContent { get; set; }
        public string ArticleUrl { get; set; }
        public string ImageUrl { get; set; }
        public DateTime PublishDate { get; set; }
        public long PublishDateTicks { get; set; }
        public int PraiseCount { get; set; }
        //public int CommentCount { get; set; }
        public int ReadCount { get; set; }
        public int MyPraiseCount { get; set; }
    }
}
