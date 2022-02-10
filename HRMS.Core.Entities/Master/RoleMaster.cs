using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HRMS.Core.Entities.Master
{
    [Table("RoleMaster", Schema= "UserManagement")]
    public class RoleMaster: BaseModel<int>
    {
        [Required(ErrorMessage = "RoleName is required.")]
        [Display(Prompt = "RoleName")]
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
    }
}
