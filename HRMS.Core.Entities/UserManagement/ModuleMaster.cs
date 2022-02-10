using HRMS.Core.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Core.Entities.UserManagement
{
    [Table("ModuleMaster",Schema = "UserManagement")]
    public class ModuleMaster: Model<int>
    {
        [Required(ErrorMessage = "this field is required.")]
        [Display(Prompt = "Module Name")]
        public string ModuleName { get; set; }

        [Required(ErrorMessage = "this field is required.")]
        [Display(Prompt = "Controller Name")]
        public string ControllerName { get; set; }
        [Required(ErrorMessage = "this field is required.")]
        [Display(Prompt = "Action Name")]
        public string ActionName { get; set; }
        [Required(ErrorMessage = "this field is required.")]
        [Display(Prompt = "Icon")]
        public string ModuleIcon { get; set; }
    }
}
