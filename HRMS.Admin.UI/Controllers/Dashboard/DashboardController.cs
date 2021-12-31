using HRMS.Core.Helpers.CommonHelper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Dashboard
{
    public class DashboardController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View(ViewHelper.GetViewPathDetails("Dashboard", "Dashboard")));
        }
    }
}
