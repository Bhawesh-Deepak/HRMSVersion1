using HRMS.Admin.UI.AuthenticateService;
using HRMS.Admin.UI.Helpers;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.Response.Posting;
using HRMS.Services.Implementation.SqlConstant;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Posting
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ReferDetailController : Controller
    {
        private readonly IDapperRepository<object> _IDapperRepository;

        public ReferDetailController(IDapperRepository<object> dapperRepository)
        {
            _IDapperRepository = dapperRepository;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.HeaderTitle = PageHeader.HeaderSetting["CandidateReferal"];

               return await Task.Run(() => View(ViewHelper.GetViewPathDetails("ReferalDetail", "ReferalIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(ReferDetailController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetReferCandidateDetails() 
        {
            try
            {
                var response = await Task.Run(() => _IDapperRepository.GetAll<ReferCandidateDetailVm>(SqlQuery.GetReferedCandidate, null));
                return PartialView(ViewHelper.GetViewPathDetails("ReferalDetail", "ReferalDetails"), response);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(ReferDetailController)} action name {nameof(GetReferCandidateDetails)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
