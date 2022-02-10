using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.SqlParams
{
    public class ExportEmployeeParams
    {
        public string LeagalEntity { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string PAndLHeadName { get; set; }
        public string JoiningDate { get; set; }
        public string Location { get; set; }
        public bool IsActive { get; set; }
    }
}
