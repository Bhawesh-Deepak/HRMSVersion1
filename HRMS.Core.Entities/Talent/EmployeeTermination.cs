using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Talent
{
    [Table("EmployeeTermination", Schema = "Talent")]
    public class EmployeeTermination : BaseModel<int>
    {
        [Required(ErrorMessage = "this field is required.")]
        public string EmpCode { get; set; }
        [Required(ErrorMessage = "this field is required.")]
        public int StatusId { get; set; }
        [Required(ErrorMessage = "this field is required.")]
        public DateTime TerminationDate { get; set; }
        [Required(ErrorMessage = "this field is required.")]
        [Display(Prompt = "Reason")]
        public int Reason { get; set; }
        [Required(ErrorMessage = "this field is required.")]
        public DateTime LastWorkingDate { get; set; }

    }
}
