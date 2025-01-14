﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.RequestVm
{
    public class UploadExcelVm
    {
        [Required(ErrorMessage ="Select Excel File First.")]
        public IFormFile UploadFile { get; set; }
        [Required(ErrorMessage = "Select Excel File First.")]
        public IFormFile UploadFile1 { get; set; }
        public int YearId { get; set; }
        public int MonthId { get; set; }
    }
}
