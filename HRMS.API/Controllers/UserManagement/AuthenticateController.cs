using HRMS.Core.Entities.Master;
using HRMS.Core.Entities.Organisation;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.Entities.UserManagement;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.API.Controllers.UserManagement
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IGenericRepository<EmployeeDetail, int> _IEmployeeDetailRepository;
        private readonly IGenericRepository<AdminEmployeeDetail, int> _IAdminEmployeeDetailRepository;
        private readonly IGenericRepository<AuthenticateUser, int> _IAuthenticateRepository;
        private readonly IGenericRepository<Company, int> _ICompanyRepository;
        private readonly IGenericRepository<AssesmentYear, int> _IAssesmentYearRepository;

        public AuthenticateController(IGenericRepository<EmployeeDetail, int> employeeDetailRepository, IGenericRepository<AdminEmployeeDetail, int> adminemployeeDetailRepository,
          IGenericRepository<AuthenticateUser, int> authenticateRepo, IGenericRepository<Company, int> companyRepository,
          IGenericRepository<AssesmentYear, int> assesmentyearRepository)
        {
            _IEmployeeDetailRepository = employeeDetailRepository;
            _IAdminEmployeeDetailRepository = adminemployeeDetailRepository;
            _IAuthenticateRepository = authenticateRepo;
            _ICompanyRepository = companyRepository;
            _IAssesmentYearRepository = assesmentyearRepository;
        }

        //[HttpPost]
        //[Produces("application/json")]
        //[Consumes("application/json")]

        //public async Task<IActionResult> Authenticate()
        //{

        //}
    }
}
