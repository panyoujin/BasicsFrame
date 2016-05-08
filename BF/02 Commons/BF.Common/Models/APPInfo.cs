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

        private string _access_token = string.Empty;
        public string Access_Token
        {
            get
            {
                if (Created_at > 0)
                {
                    DateTime DateStart = new DateTime(1970, 1, 1, 8, 0, 0);
                    int timeInt = Convert.ToInt32((DateTime.Now - DateStart).TotalSeconds);
                    if (timeInt - this.Created_at + 86400 <= Expires_in)
                    {
                        return _access_token;
                    }
                }
                return "";

            }
            set
            {
                this._access_token = value;
            }
        }

        public int Expires_in { get; set; }
        public int Created_at { get; set; }

        public string Token_Type { set; get; }
        public string Refresh_Token { set; get; }
    }
}
