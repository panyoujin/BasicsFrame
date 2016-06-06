using BF.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BF.Common.Extensions;
using System.Web;

namespace BF.BackWebAPI.Models.RequestModels
{
    public class HealthModelInfo
    {
        public int MID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Introduce { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Model_Name { get; set; }

        private string _iocUrl;
        /// <summary>
        /// 
        /// </summary>
        public string IcoUrl
        {
            get
            {
                if (_iocUrl == null || _iocUrl.IndexOf("http://") == 0 || _iocUrl.IndexOf("https://") == 0)
                {

                    return _iocUrl;
                }
                return Global.AttmntUrl + _iocUrl;
            }
            set
            {
                _iocUrl = value;
            }
        }

        private string _imgUrl;
        /// <summary>
        /// 
        /// </summary>
        public string ImageUrl
        {
            get
            {
                if (_imgUrl == null || _imgUrl.IndexOf("http://") == 0 || _imgUrl.IndexOf("https://") == 0)
                {

                    return _imgUrl;
                }
                return Global.AttmntUrl + _imgUrl;
            }
            set
            {
                _imgUrl = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long CreationDateTicks { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CreationDate { get; set; }

        public int Model_Type { get; set; }

        public string Model_TypeStr
        {
            get
            {
                try
                {
                    return ((Model_Types)this.Model_Type).Description();
                }
                catch (Exception ex)
                {

                }
                return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Plan_RGB { get; set; }

        public int User_ID { get; set; }

        public string Describe { get; set; }
        public string Remarks { get; set; }
        public bool IsBubble { get; set; }
        public int Bubble_Time { get; set; }
        public int Bubble_Temperature { get; set; }
        public int Cook_Time { get; set; }
        public int Cook_Temperature { get; set; }
        public bool Is_Heat_Preservation { get; set; }
        public int Heat_Preservation_Time { get; set; }
        public int Heat_Preservation_Temperature { get; set; }
        public int Removal_Chlorine_Time { get; set; }
        public int Final_Temperature { get; set; }
        public bool IsFerv { get; set; }
        public int Model_Status { get; set; }
        /// <summary>
        /// 是否收藏
        /// </summary>
        public int IsCollection { get; set; }

        /// <summary>
        /// 是否常用
        /// </summary>
        public int IsCommonModel { get; set; }

        public string ModelUrl
        {
            get
            {
                return string.Format("http://{0}/WebPage/ModelDetails.html?modelID={1}", HttpContext.Current.Request.Url.Authority, MID);
            }
        }
        public string WeChatUrl { get; set; }
        /// <summary>
        /// 收藏数量
        /// </summary>
        public int CollectionCount { get; set; }
    }
}
