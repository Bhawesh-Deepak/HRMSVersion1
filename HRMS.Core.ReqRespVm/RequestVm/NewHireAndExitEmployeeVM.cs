using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.RequestVm
{
    public class NewHireAndExitEmployeeVM
    {
        [Required(ErrorMessage = "this filed is required.")]
        public DateTime FromDate { get; set; }
        [Required(ErrorMessage = "this filed is required.")]
        public DateTime ToDate { get; set; }
        [Required(ErrorMessage = "this filed is required.")]
        public int ValueType { get; set; }
    }
}
