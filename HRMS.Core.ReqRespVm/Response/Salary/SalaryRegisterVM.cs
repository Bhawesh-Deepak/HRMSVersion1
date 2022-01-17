using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.Salary
{
    public class SalaryRegisterVM
    {
        [Required(ErrorMessage = "this field is required..")]
        public int DaysMonth { get; set; }
        [Required(ErrorMessage = "this field is required..")]
        public int DaysYear { get; set; }
        public string EmployeeCode { get; set; }
    }
}
