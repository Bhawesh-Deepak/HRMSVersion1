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
    [Table("Award", Schema = "Talent")]
    public class Award : BaseModel<int>
    {
        [Required(ErrorMessage = "this field is required.")]
        public string EmpCode { get; set; }
        [Required(ErrorMessage = "this field is required.")]
        public int AwardTypeId { get; set; }
        [Required(ErrorMessage = "this field is required.")]
        [Display(Prompt = "Award Name")]
        public string AwardName { get; set; }
        [Required(ErrorMessage = "this field is required.")]
        [Display(Prompt = "Project")]
        public string Project { get; set; }
        [Required(ErrorMessage = "this field is required.")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "this field is required.")]
        public DateTime AwardDate { get; set; }
        public string EmployeeName { get; set; }
        public string LegalEntity { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }

        [NotMapped]
        public string AwardTypeName { get; set; }

    }
}
