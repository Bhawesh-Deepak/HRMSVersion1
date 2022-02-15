using HRMS.Core.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Core.Entities.UserManagement
{
    [Table("SubModuleMaster",Schema = "UserManagement")]
    public class SubModuleMaster: Model<int>
    {
        [Required(ErrorMessage = "this field   is required.")]
        public int ModuleId { get; set; }
        [Required(ErrorMessage = "this field   is required.")]
        [Display(Prompt = "Sub Module Name")]
        public string SubModuleName { get; set; }

        [Required(ErrorMessage = "this field   is required.")]
        [Display(Prompt = "Icon")]
        public string SubModuleIcon { get; set; }
        public int MenuLevel { get; set; }
    }
}
