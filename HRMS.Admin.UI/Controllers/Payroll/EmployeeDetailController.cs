using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Payroll
{
    public class EmployeeDetailController : Controller
    {
        private readonly IGenericRepository<EmployeeDetail, int> _IEmployeeDetailRepository;

        public EmployeeDetailController(IGenericRepository<EmployeeDetail, int> EmployeeDetailRepo)
        {
            _IEmployeeDetailRepository = EmployeeDetailRepo;
        }
        public async Task<IActionResult> Index()
        {
            var response = await _IEmployeeDetailRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            ViewBag.HeaderTitle = PageHeader.HeaderSetting["EmployeeDetailIndex"];
            return await Task.Run(() => View(ViewHelper.GetViewPathDetails("EmployeeDetail", "EmployeeDetailIndex"), response.Entities));
        }
    }
}
