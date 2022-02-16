using HRMS.Core.ReqRespVm.Response.Reporting;
using HRMS.Core.ReqRespVm.SqlParams;
using HRMS.Services.Implementation.SqlConstant;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Reporting
{
    public class MediClaimReport : Controller
    {
        private readonly IDapperRepository<MediClaimSqlParams> _MediClaimRepository;

        public MediClaimReport(IDapperRepository<MediClaimSqlParams> mediClaimRepository)
        {
            _MediClaimRepository = mediClaimRepository;
        }
        public IActionResult Index()
        {
            var response=GetMediClaimReport(2020, 4, "");
            return View();
        }

        private IEnumerable<EmployeeMediClaimVm> GetMediClaimReport(int year, int month, string empCode)
        {
            var sqlParams = new MediClaimSqlParams() { 
                DateYear=year,
                DateMonth= month,
                EmployeeCode= empCode
            };
            var response =  _MediClaimRepository.GetAll<EmployeeMediClaimVm>(SqlQuery.GetEmployeeMediClaimReport, sqlParams);
            return response
        }
    }
}
