using BF.Common.Enums;
using BF.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF.BackWebAPI.Models.ResponseModel
{
    public class AftermarketResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public int uuid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string User_Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string QuestionTypeStr
        {
            get
            {
                return ((AftermarketQuestionType)this.QuestionType).Description();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int QuestionType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string QuestionDescribe { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AftermarketStatusStr
        {
            get
            {
                return ((AftermarketStatus)this.AftermarketStatus).Description();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int AftermarketStatus { get; set; }

        public string CreationDate { get; set; }
    }
}
