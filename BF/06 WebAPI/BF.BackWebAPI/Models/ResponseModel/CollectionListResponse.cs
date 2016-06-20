using BF.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF.BackWebAPI.Models.ResponseModel
{
    public class CollectionListResponse
    {
        public int sID { get; set; }

        public int sType { get; set; }
        private string _sIntroduce;
        /// <summary>
        /// 
        /// </summary>
        public string sIntroduce
        {
            get
            {
                return HTMLHelper.NoHTML(_sIntroduce);
            }
            set
            {
                _sIntroduce = value;
            }
        }
        //public string sIntroduce { get; set; }

        public string sName { get; set; }

        private string _sIcoUrl;
        public string sIcoUrl
        {
            get
            {
                if (_sIcoUrl == null || _sIcoUrl.IndexOf("http://") == 0 || _sIcoUrl.IndexOf("https://") == 0)
                {

                    return _sIcoUrl;
                }
                return Global.AttmntUrl + _sIcoUrl;
            }
            set
            {
                _sIcoUrl = value;
            }
        }

        public string CollectionTime { get; set; }

        public long CollectionTimeTicks { get; set; }
        public string WeChatUrl { get; set; }
    }
}
