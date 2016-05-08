using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF.Common.Models
{
    [Serializable]
    public class APPInfo
    {
        public int ID { get; set; }

        public string HTAccount { get; set; }
        public string HTPasswd { get; set; }

        public string AppName { get; set; }
        public string AppID { get; set; }

        public string SecretID { get; set; }
        public string Scopes { get; set; }

        public string CallbackUrl { get; set; }
        public string AuthorizeRequest { get; set; }

        public string TokenRequest { get; set; }
        public string Access_Token { get; set; }

        public int Expires_in { get; set; }
        public int Created_at { get; set; }
        public string Token_Type { set; get; }
    }
}
