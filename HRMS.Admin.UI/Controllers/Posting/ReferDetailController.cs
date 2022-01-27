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
    public class ReferDetailController : Controller
    {
        private readonly IDapperRepository<object> _IDapperRepository;

        public ReferDetailController(IDapperRepository<object> dapperRepository)
        {
            _IDapperRepository = dapperRepository;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.HeaderTitle = PageHeader.HeaderSetting["CandidateReferal"];

            return await Task.Run(() => View(ViewHelper.GetViewPathDetails("ReferalDetail", "ReferalIndex")));


        }

        [HttpGet]
        public async Task<IActionResult> GetReferCandidateDetails() 
        {
            var response = await Task.Run(() => _IDapperRepository.GetAll<ReferCandidateDetailVm>(SqlQuery.GetReferedCandidate, null));
            return PartialView(ViewHelper.GetViewPathDetails("ReferalDetail", "ReferalDetails"), response);
        }
    }
}
