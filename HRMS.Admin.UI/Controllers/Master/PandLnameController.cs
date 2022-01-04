using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.Response.Master;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Master
{
    public class PandLnameController : Controller
    {
        private readonly IGenericRepository<PAndLMaster, int> _IPandLnameRepository;



        public PandLnameController(IGenericRepository<PAndLMaster, int> PandLnameRepo)
        {
            _IPandLnameRepository = PandLnameRepo;
            

        }
        public async Task<IActionResult> Index()
        {
            ViewBag.HeaderTitle = PageHeader.HeaderSetting["DesignationIndex"];

            return await Task.Run(() => View(ViewHelper.GetViewPathDetails("PandLname", "PandLnameIndex")));
        }

        public async Task<IActionResult> GetPandLList()
        {
            try
            {
                var response = new DBResponseHelper<PAndLMaster, int>()
                    .GetDBResponseHelper(await _IPandLnameRepository
                    .GetAllEntities(x => x.IsActive && !x.IsDeleted));

                return PartialView(ViewHelper.GetViewPathDetails("PandLname", "PandLnameDetails"), response.Item2.Entities);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Department)} action name {nameof(GetPandLList)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> CreatePandLname(int id)
        {
          
            var response = await _IPandLnameRepository.GetAllEntities(x => x.Id == id);

            if (id == 0)
            {
                return PartialView(ViewHelper.GetViewPathDetails("PandLname", "PandLNameCreate"));
            }
            else
            {

                return PartialView(ViewHelper.GetViewPathDetails("PandLname", "PandLNameCreate"), response.Entities.First());
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpsertPandLname(PAndLMaster model)
        {
            if (model.Id == 0)
            {
                var response = await _IPandLnameRepository.CreateEntity(model);
                return Json(response.Message);
            }
            else
            {
                var response = await _IPandLnameRepository.UpdateEntity(model);
                return Json(response.Message);
            }
        }

    }
}
