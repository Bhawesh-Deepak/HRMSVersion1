using HRMS.Core.Entities.Master;
using HRMS.Core.Entities.Organisation;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.Entities.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.UserManagement
{
    public class UserAuthenticateVM
    {
        public AssesmentYear AssesmentYearDetail { get; set; }
        public List<EmployeeDetail> EmployeeDetail { get; set; }
        public List<Company> CompanyDetail { get; set; }
        public AuthenticateUser AuthenticateUser { get; set; }
        public bool IsSucess { get; set; }
        public string Message { get; set; }
    }
}
