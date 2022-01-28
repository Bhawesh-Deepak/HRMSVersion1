using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Reporting
{
    public class ESIController : Controller
    {
        private readonly IHostingEnvironment _IHostingEnviroment;
        public ESIController(IHostingEnvironment hostingEnviroment)
        {
            _IHostingEnviroment = hostingEnviroment;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
