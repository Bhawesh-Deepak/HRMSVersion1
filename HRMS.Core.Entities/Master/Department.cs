using HRMS.Core.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Core.Entities.Master
{
    [Table("Department", Schema = "Master")]
    public class Department: BaseModel<int>
    {
        [Required(ErrorMessage ="this field is required.")]
        [Display(Prompt ="Department Name")]
        public string Name { get; set; }

        [Display(Prompt = "Code")]
        public string Code { get; set; }

        [Display(Prompt = "Description")]
        public string Description { get; set; }
    }
}
