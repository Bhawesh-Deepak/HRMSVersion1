using HRMS.Core.Entities.HR;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.Helpers.ExcelHelper;
using HRMS.Core.ReqRespVm.SqlParams;
using HRMS.Services.Implementation.SqlConstant;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Reporting
{
    public class SalaryPaidRegisterController : Controller
    {
        
        private readonly IHostingEnvironment _IHostingEnviroment;

        public SalaryPaidRegisterController(IHostingEnvironment hostingEnvironment)
        {
            
            _IHostingEnviroment = hostingEnvironment; 
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("SalaryPaidRegister", "_SalaryPaidRegister")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(SalaryPaidRegisterController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }


        [HttpPost]
        public async Task<IActionResult> ExportSalaryRegister(PaidRegister model)
        {
            return await Task.Run(() => View(ViewHelper.GetViewPathDetails("SalaryPaidRegister", "_SalaryPaidRegister")));
        }
    }
}
