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
    [Table("LeaveType", Schema = "Master")]
    public class LeaveType : BaseModel<int>
    {
        [Required(ErrorMessage = "this field is required")]
        [Display(Prompt = "Laeve Type")]
        public string Name { get; set; }
        [Required(ErrorMessage = "this field is required")]
        [Display(Prompt = "Leave Code")]
        public string Code { get; set; }
    }
}
