using System.ComponentModel.DataAnnotations;

namespace HRMS.Core.ReqRespVm.RequestVm
{
    public class ChangePasswordVm
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Old Password is required.")]
        [Display(Prompt ="Please enter old password")]
        public string OldPassword { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage ="New Password is required.")]
        [Display(Prompt = "Please enter new password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        [Required(ErrorMessage = "Confirm Password is required.")]
        [Display(Prompt = "Please enter Confirm password")]
        public string ConfirmPassword { get; set; }
    }
}
