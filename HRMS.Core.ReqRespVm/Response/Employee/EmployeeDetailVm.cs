using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.Employee
{
    public class EmployeeDetailVm
    {

        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
        public DateTime JoiningDate { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string Location { get; set; }
        public string LegalEntity { get; set; }
        public double Index { get; set; }
        public string OrderBy { get; set; }
        public string SortBy { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int TotalRecords { get; set; }
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
