﻿using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.HR;
using HRMS.Core.Entities.Master;
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
        private readonly IGenericRepository<PaidRegister, int> _IPaidRegisterRepository;
        private readonly IGenericRepository<AssesmentYear, int> _IAssesmentYearRepository;
        public SalaryPaidRegisterController(IHostingEnvironment hostingEnvironment,
            IGenericRepository<AssesmentYear, int> assesmentyearRepo, 
            IGenericRepository<PaidRegister, int> paidregisterRepository)
        {
            
            _IHostingEnviroment = hostingEnvironment;
            _IPaidRegisterRepository = paidregisterRepository;
            _IAssesmentYearRepository = assesmentyearRepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                await PopulateViewBag();
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
        public async Task<IActionResult> DownloadSalaryPaidRegister(PaidRegister model)
        {
 
            try
            {
                var response = await _IPaidRegisterRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted && x.DateMonth == model.DateMonth && x.DateYear == model.DateYear);

            var net = new System.Net.WebClient();
             
            var data = net.DownloadData(_IHostingEnviroment.WebRootPath + response.Entities.FirstOrDefault().UploadFilePath);
            var content = new System.IO.MemoryStream(data);
            var contentType = "APPLICATION/octet-stream";
            var fileName = "SalaryPaidRegister_"+model.DateMonth+"_"+model.DateYear+ ".xlsx";
            return File(content, contentType, fileName);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(SalaryPaidRegisterController)} action name {nameof(DownloadSalaryPaidRegister)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

             
        }
        private async Task PopulateViewBag()
        {
            var assesmentyearResponse = await _IAssesmentYearRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            if (assesmentyearResponse.ResponseStatus == ResponseStatus.Success)
                ViewBag.AssesmentYearList = assesmentyearResponse.Entities;

 
        }
    }
}
