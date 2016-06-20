using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF.BackWebAPI.Models.Back
{
    public class TB_CM_Role
    {

        /// <summary>
        /// auto_increment
        /// </summary>	
        public int ID { get; set; }
        /// <summary>
        /// RoleName
        /// </summary>	
        public string RoleName { get; set; }
        /// <summary>
        /// Description
        /// </summary>	
        public string Description { get; set; }
        /// <summary>
        /// Status
        /// </summary>	
        public bool Status { get; set; }
        /// <summary>
        /// CreationUser
        /// </summary>	
        public string CreationUser { get; set; }
        /// <summary>
        /// CreationDate
        /// </summary>	
        public string CreationDate { get; set; }
        /// <summary>
        /// ModificationUser
        /// </summary>	
        public string ModificationUser { get; set; }
        /// <summary>
        /// ModificationDate
        /// </summary>	
        public string ModificationDate { get; set; }
        /// <summary>
        /// 菜单ID集合 ,分割
        /// </summary>
        public string MenuIDs { get; set; }
    }
}
