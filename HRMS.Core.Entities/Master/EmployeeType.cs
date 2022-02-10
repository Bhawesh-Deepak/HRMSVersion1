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
    [Table("EmployeeType", Schema = "Master")]
    public class EmployeeType : BaseModel<int>
    {
        [Required(ErrorMessage = "This field is required.")]
        [Display(Prompt = "Employee Type ")]
        public string Name { get; set; }
    }
}
