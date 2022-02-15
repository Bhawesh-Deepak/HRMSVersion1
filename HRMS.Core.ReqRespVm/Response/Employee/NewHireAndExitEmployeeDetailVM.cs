using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.Employee
{
    public class NewHireAndExitEmployeeDetailVM
    {
        public int Id { get; set; }
        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string LegalEntity { get; set; }
        public string Location { get; set; }
        public DateTime CalenderDate { get; set; }
        
    }
}
