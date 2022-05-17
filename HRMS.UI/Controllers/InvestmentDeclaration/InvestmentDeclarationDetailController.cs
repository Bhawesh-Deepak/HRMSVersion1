using HRMS.Core.Entities.Investment;
using HRMS.Core.Helpers.CommonHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HRMS.UI.Controllers.InvestmentDeclaration
{
    public class InvestmentDeclarationDetailController : Controller
    {
        private readonly string APIURL = string.Empty;
        private readonly IHostingEnvironment _IHostingEnviroment;
        public InvestmentDeclarationDetailController(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            APIURL = configuration.GetSection("APIURL").Value;
            _IHostingEnviroment = hostingEnvironment;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                List<InvestmentDeclarationPDFDetails> pDFDetails = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIURL);
                    var responseTask = await client.GetAsync("/api/HRMS/InvestmentDeclarationPDF/GetInvestmentDeclarationPDFDetails?EmpCode=" + HttpContext.Session.GetString("EmpCode") + "&FinancialYear=" + Convert.ToInt32(HttpContext.Session.GetString("financialYearId")));
                    if (responseTask.IsSuccessStatusCode)
                    {
                        var responseDetails = await responseTask.Content.ReadAsStringAsync();
                        pDFDetails = JsonConvert.DeserializeObject<List<InvestmentDeclarationPDFDetails>>(responseDetails);
                         
                    }
                    return await Task.Run(() => View(ViewHelper.GetViewPathDetails("InvestmentDeclarationDetail", "_InvestmentDeclarationDetail"), pDFDetails));
                }
               
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(InvestmentDeclarationDetailController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
