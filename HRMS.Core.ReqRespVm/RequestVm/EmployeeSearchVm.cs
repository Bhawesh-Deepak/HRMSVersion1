using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.RequestVm
{
    public class EmployeeSearchVm
    {
        public string LegalEntity { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string PAndLName { get; set; }
        public string DOJ { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
    }
}
