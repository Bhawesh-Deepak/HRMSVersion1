using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.Reporting
{
    public class EmployeePaySlipVM
    {
        public int Id { get; set; }
        public string EmpCode { get; set; }
        public string Salutation { get; set; }
        public string UANNumber { get; set; }
        public string ESICNew { get; set; }
        public string PanCardNumber { get; set; }
        public DateTime JoiningDate { get; set; }
        public string LegalEntity { get; set; }
        public string Department { get; set; }
        public string DesignationId { get; set; }
        public string ComponentName { get; set; }
        public int ComponentType { get; set; }
        public int ComponentValueType { get; set; }
        public string ComponentCategory { get; set; }
        public decimal SalaryAmount { get; set; }
        public string EmployeeName { get; set; }
        public int DateMonth { get; set; }
        public int DateYear { get; set; }
        public decimal PresentDays { get; set; }
        public decimal LopDays { get; set; }
        public decimal ArrearDays { get; set; }
    }
}
