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
    [Table("Course", Schema = "Talent")]
    public class Course : BaseModel<int>
    {
        [Required(ErrorMessage = "this field is required.")]
        public int DesignationId { get; set; }
        [Required(ErrorMessage = "this field is required.")]
        public int CourseModeId { get; set; }
        [Required(ErrorMessage = "this field is required.")]
        [Display(Prompt = "Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "this field is required.")]
        [Display(Prompt = "Code")]
        public string Code { get; set; }
        [Required(ErrorMessage = "this field is required.")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "this field is required.")]
        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "this field is required.")]
        public DateTime TrainingDateTime { get; set; }
        [NotMapped]
        public string DesignationName { get; set; }
        [NotMapped]
        public string CourseModeName { get; set; }

    }
}
