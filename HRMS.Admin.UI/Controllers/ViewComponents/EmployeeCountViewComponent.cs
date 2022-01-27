using HRMS.Admin.UI.AuthenticateService;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.ReqRespVm.Response.Employee;
using HRMS.Core.ReqRespVm.SqlParams;
using HRMS.Services.Implementation.SqlConstant;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.ViewComponents
{
    [CustomAuthenticate]
    public class EmployeeCountViewComponent: ViewComponent
    {
        private readonly IDapperRepository<EmployeeCountParms> _IEmployeeCountParmsRepository;
        public EmployeeCountViewComponent(IDapperRepository<EmployeeCountParms> EmployeeCountParmsRepository)
        {
            _IEmployeeCountParmsRepository = EmployeeCountParmsRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new EmployeeCountParms(){};
            var response = await Task.Run(() => _IEmployeeCountParmsRepository.GetAll<EmployeeCountVM>(SqlQuery.EmployeeCount, model));
            return await Task.FromResult((IViewComponentResult)View("_EmployeeCount", response.FirstOrDefault()));
        }
    }
}
