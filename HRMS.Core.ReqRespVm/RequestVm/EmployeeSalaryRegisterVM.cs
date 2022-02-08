using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HRMS.Core.ReqRespVm.RequestVm
{
    public class EmployeeSalaryRegisterVM
    {
        [Required(ErrorMessage = "This field is required.")]
        public int DateMonth { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public int DateYear { get; set; }
        public string EmployeeCode { get; set; }
        public string Name { get; set; }
        public IFormFile UploadFile { get; set; }
    }
}
