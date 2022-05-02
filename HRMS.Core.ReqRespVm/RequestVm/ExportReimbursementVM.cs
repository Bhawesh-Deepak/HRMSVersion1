using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.RequestVm
{
    public class ExportReimbursementVM
    {
        [Required(ErrorMessage = "this field is required.")]
        public string DateMonth { get; set; }
        [Required(ErrorMessage = "this field is required.")]
        public string DateYear { get; set; }
         
        public int ? CategoryId { get; set; }
        
        public string Status { get; set; }
    }
}
