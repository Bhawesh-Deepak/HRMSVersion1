using HRMS.Core.Entities.Posting;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Posting
{
    public class CandidateReferalController : Controller
    {
        private readonly IGenericRepository<CurrentOpening, int> _ICurrentOpeningRepository;
        private readonly IHostingEnvironment _IhostingEnviroment;
        private readonly IGenericRepository<ReferCandidate, int> _IReferCandidateRepository;
        public CandidateReferalController(IGenericRepository<CurrentOpening, int> iCurrentOpeningRepository, IHostingEnvironment ihostingEnviroment, IGenericRepository<ReferCandidate, int> iReferCandidateRepository)
        {
            _ICurrentOpeningRepository = iCurrentOpeningRepository;
            _IhostingEnviroment = ihostingEnviroment;
            _IReferCandidateRepository = iReferCandidateRepository;
        }


        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetCurrentOpeningDetails()
        {
            var openingDetails = await _ICurrentOpeningRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted
                    && x.LastApplyDate.Date <= DateTime.Now.Date);

            return PartialView(ViewHelper.GetViewPathDetails("CurrentOpening", "CurrentOpeningDetails"), openingDetails.Entities);

        }
    }
}
