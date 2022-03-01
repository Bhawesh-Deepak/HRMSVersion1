using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.RequestVm.NeedSupport
{
    public class NeedSupportVM
    {
        [Required(ErrorMessage = "this field is required.")]
        [Display(Prompt = "Name")]
        public string Name { get; set; }
        [Display(Prompt = "Email")]
        [Required(ErrorMessage = "this field is required.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
        [Required(ErrorMessage = "this field is required.")]
        [Display(Prompt = "Employee Code")]
        public string EmpCode { get; set; }
        [Required(ErrorMessage = "this field is required.")]
        [Display(Prompt = "Message.")]
        public string Message { get; set; }

    }
}
