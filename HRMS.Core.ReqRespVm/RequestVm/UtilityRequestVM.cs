using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.RequestVm
{
   public class UtilityRequestVM
    {
        [Required(ErrorMessage = "Select Excel File First.")]
        public IFormFile UploadFile { get; set; }
    }
}
