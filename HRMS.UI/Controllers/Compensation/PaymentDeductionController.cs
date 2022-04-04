using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.Response.Reporting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HRMS.UI.Controllers.Compensation
{
    public class PaymentDeductionController : Controller
    {
        private readonly string APIURL = string.Empty;
        public PaymentDeductionController(IConfiguration configuration)
        {
            APIURL = configuration.GetSection("APIURL").Value;

        }
        public async Task<IActionResult> Index(int FinancialYear)
        {
            try
            {
                if (FinancialYear == 0)
                    FinancialYear = 3;// Convert.ToInt32(HttpContext.Session.GetString("financialYearId"));
                List<PaymentDeductionVM> paymentDeductions = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIURL);
                    var responseTask = await client.GetAsync("/api/HRMS/PaymentDeduction/GetEmployeePaymentDeduction?EmpCode=" + HttpContext.Session.GetString("EmpCode") + "&FinancialYear=" + FinancialYear);
                    if (responseTask.IsSuccessStatusCode)
                    {
                        var responseDetails = await responseTask.Content.ReadAsStringAsync();
                        paymentDeductions = JsonConvert.DeserializeObject<List<PaymentDeductionVM>>(responseDetails);
                        return await Task.Run(() => View(ViewHelper.GetViewPathDetails("PaymentDeduction", "_PaymentDeductionIndex"), paymentDeductions));
                    }
                    else
                    {
                        return await Task.Run(() => View(ViewHelper.GetViewPathDetails("PaymentDeduction", "_PaymentDeductionIndex"), paymentDeductions));
                    }
                }

            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(PaymentDeductionController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
