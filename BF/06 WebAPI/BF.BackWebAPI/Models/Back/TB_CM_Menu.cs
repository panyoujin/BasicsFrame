using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF.BackWebAPI.Models.Back
{
    [Serializable]
    public class TB_CM_Menu
    {
        public int ID { get; set; }

        public int ParentMenuID { get; set; }

        public string MenuName { get; set; }

        public string MenuUrl { get; set; }

        public string IConUrl { get; set; }

        public string Description { get; set; }

        public bool IsRoot { get; set; }

        public bool Status { get; set; }

        public bool IsFunction { get; set; }

        public int Sort { get; set; }

        public string CreationUser { get; set; }

        public DateTime? CreationDate { get; set; }

        public string ModificationUser { get; set; }

        public DateTime? ModificationDate { get; set; }

       
    }
}
