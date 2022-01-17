using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.SqlParams
{
    public class SalaryRegisterParams
    {
        public int DateMonth { get; set; }
        public int DateYear { get; set; }
    }
}
