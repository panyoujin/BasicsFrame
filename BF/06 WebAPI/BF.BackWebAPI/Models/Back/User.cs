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
        public string Account { get; set; }
        public string Passwd { set; get; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string QQ { get; set; }
        public string ImageUrl { get; set; }

        public bool IsAdmin { get; set; }
    }
}
