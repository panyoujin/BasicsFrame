using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF.BackWebAPI.Models.Back
{
    public class UserModel
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string UserAccount { get; set; }
        public string PhoneNum { get; set; }
        public string EMail { get; set; }
        public string NickName { get; set; }
        public string QQ { get; set; }
        public string ImageUrl { get; set; }
        public Boolean IsAdmin { get; set; }
    }
}
