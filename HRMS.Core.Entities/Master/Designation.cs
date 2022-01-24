using HRMS.Core.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Core.Entities.Master
{
    [Table("Designation", Schema = "Master")]
    public class Designation : BaseModel<int>
    {
        [Required(ErrorMessage = "this field is required.")]
        [Display(Prompt = "Designation")]
        public string Name { get; set; }

        [Display(Prompt = "Code")]
        public string Code { get; set; }

        [Display(Prompt = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "this field is required.")]
        public int DepartmentId { get; set; }
        [NotMapped]
        public string DepartmentName { get; set; }
    }
}
