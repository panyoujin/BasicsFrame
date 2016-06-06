using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BF.BackWebAPI.Models.Back.InParam
{
    public class InsertArticle
    {
        public int ID { set; get; }
        public int ArticleType_ID { set; get; }
        public string ArticleTitle { set; get; }
        public string ArticleContent { set; get; }
        public string ImageUrl { set; get; }
        public string FullUrl { set; get; }
        public string PublishDate { set; get; }
        public int ArticleSort { set; get; }
    }
}