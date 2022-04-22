using System.ComponentModel.DataAnnotations;

namespace HRMS.Core.ReqRespVm.RequestVm
{
    public class ChangePasswordVm
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Old Password is required.")]
        [Display(Prompt ="Enter Old Password")]
        public string OldPassword { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage ="New Password is required.")]
        [Display(Prompt = "Enter New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        [Required(ErrorMessage = "Confirm Password is required.")]
        [Display(Prompt = "Enter Confirm Password")]
        public string ConfirmPassword { get; set; }
        public int EmpId { get; set; }
    }
}
