using HRMS.Core.Entities.Organisation;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Dashboard
{
    public class DashboardController : Controller
    {
        private readonly IGenericRepository<Subsidiary, int> _ISubsidiaryRepository;
        
        public DashboardController(IGenericRepository<Subsidiary, int> subsidryRepository)
        {
            _ISubsidiaryRepository = subsidryRepository;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _ISubsidiaryRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            return await Task.Run(() => View(ViewHelper.GetViewPathDetails("Dashboard", "Dashboard"), model.Entities));
        }
    }
}
