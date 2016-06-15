using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF.BackWebAPI.Models.Back
{
    public class TB_CM_Rolemenu
    {

        /// <summary>
        /// auto_increment
        /// </summary>	
        public int ID { get; set; }
        /// <summary>
        /// RoleID
        /// </summary>	
        public int RoleID { get; set; }
        /// <summary>
        /// MenuID
        /// </summary>	
        public int MenuID { get; set; }
        /// <summary>
        /// CreationUser
        /// </summary>	
        public string CreationUser { get; set; }
        /// <summary>
        /// Status
        /// </summary>	
        public bool Status { get; set; }
        /// <summary>
        /// CreationDate
        /// </summary>	
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// ModificationUser
        /// </summary>	
        public string ModificationUser { get; set; }
        /// <summary>
        /// ModificationDate
        /// </summary>	
        public DateTime ModificationDate { get; set; }

    }
}
