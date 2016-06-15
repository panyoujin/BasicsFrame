using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF.BackWebAPI.Models.Back
{
    public class MenuTreeModel
    {
        public string menuid { get; set; }
        public string menuname { get; set; }
        public string icon { get; set; }
        public string url { get; set; }
        public List<MenuTreeModel> child { get; set; }
        public List<MenuTreeModel> menus { get; set; }
    }
}
