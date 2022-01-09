using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.Common
{
    public class FilteredEmployee
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string EmpCode { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public bool IsActive { get; set; }
        public bool Isdeleted { get; set; }
    }
}
