using HRMS.Core.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Core.Entities.Master
{
    [Table("Department", Schema = "Master")]
    public class Department: BaseModel<int>
    {
        [Required(ErrorMessage = "This field is required.")]
        public int BranchId { get; set; }
        [Required(ErrorMessage ="This field is required.")]
        [Display(Prompt ="Department Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [Display(Prompt = "Code")]
        public string Code { get; set; }
    }
}
