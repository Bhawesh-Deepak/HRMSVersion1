using HRMS.Admin.UI.Models;
using HRMS.Core.Entities.Master;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGenericRepository<AssesmentYear, int> _IAssesmentYearRepository;
        public HomeController(ILogger<HomeController> logger, IGenericRepository<AssesmentYear, int> assesmentYearRepository )
        {
            _logger = logger;
            _IAssesmentYearRepository = assesmentYearRepository;
        }

        public async Task<IActionResult> Index()
        {
            await PopulateViewBag();
            return View();
        }

        private async Task PopulateViewBag()
        {
            ViewBag.AssesmentYear = (await _IAssesmentYearRepository.GetAllEntities(x => x.IsActive == true && x.IsDeleted == false)).Entities;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
