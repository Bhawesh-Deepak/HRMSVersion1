﻿using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.Helpers.ExcelHelper;
using HRMS.Core.ReqRespVm.RequestVm;
using HRMS.Core.ReqRespVm.Response.Reporting;
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
using HRMS.Admin.UI.AuthenticateService;

namespace HRMS.Admin.UI.Controllers.Reporting
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class IncentivePaidRegisterController : Controller
    {
        private readonly IDapperRepository<SalaryRegisterByEmployeeCodeParams> _ISalaryRegisterParamsRepository;
        private readonly IHostingEnvironment _IHostingEnviroment;
        private readonly IGenericRepository<AssesmentYear, int> _IAssesmentYearRepository;
        public IncentivePaidRegisterController(IHostingEnvironment hostingEnvironment,
            IGenericRepository<AssesmentYear, int> assesmentyearRepo, 
            IDapperRepository<SalaryRegisterByEmployeeCodeParams> SalaryRegisterParamsRepository)
        {
            _ISalaryRegisterParamsRepository = SalaryRegisterParamsRepository;
            _IHostingEnviroment = hostingEnvironment;
            _IAssesmentYearRepository = assesmentyearRepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                await PopulateViewBag();
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("IncentivePaidRegister", "_IncentivePaidRegister")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(IncentivePaidRegisterController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> ExportIncentivePaidRegister(EmployeeSalaryRegisterVM model)
        {
            try
            {
                string empresponse = null;
                if (model.UploadFile != null)
                empresponse = new ReadEmployeeCode().GetSalaryRegisterEmpCodeDetails(model.UploadFile);

                var request = new SalaryRegisterByEmployeeCodeParams()
                {
                    DateMonth = model.DateMonth,
                    DateYear = model.DateYear,
                    EmployeeCode = empresponse
                };
                var response = (await Task.Run(() => _ISalaryRegisterParamsRepository.GetAll<IncentivePaidRegisterVM>(SqlQuery.GetIncentivePaidRegister, request))).ToList();


            var sWebRootFolder = _IHostingEnviroment.WebRootPath;
            var sFileName = @"IncentivePaidRegister.xlsx";
            var URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(fileName: Path.Combine(sWebRootFolder, sFileName));
            }
            ExcelPackage Eps = new ExcelPackage();
            ExcelWorksheet Sheets = Eps.Workbook.Worksheets.Add("Incentive");
            Sheets.View.FreezePanes(1, 4);
            Sheets.Cells["A1:E1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            Sheets.Cells["A1:E1"].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
             
            Sheets.Cells["A1"].Value = "Month";
            Sheets.Cells["B1"].Value = "Year";
            Sheets.Cells["C1"].Value = "Employee_Code";
            Sheets.Cells["D1"].Value = "Employee_Name";
            Sheets.Cells["E1"].Value = "Performance Incentive";
            int row = 2;
            foreach (var data in response)
            {
                Sheets.Cells[string.Format("A{0}", row)].Value = data.MonthNames;
                Sheets.Cells[string.Format("B{0}", row)].Value = data.DateYear;
                Sheets.Cells[string.Format("C{0}", row)].Value = data.EmpCode;
                Sheets.Cells[string.Format("D{0}", row)].Value = data.EmployeeName;
                Sheets.Cells[string.Format("E{0}", row)].Value = data.SalaryAmount;
                row++;
            }
            var stream = new MemoryStream(Eps.GetAsByteArray());
            return File(stream.ToArray(), "application/vnd.ms-excel", sFileName);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(IncentivePaidRegisterController)} action name {nameof(ExportIncentivePaidRegister)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }
        #region PrivateFields
        private async Task PopulateViewBag()
        {
            
            var assesmentyearResponse = await _IAssesmentYearRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            if (assesmentyearResponse.ResponseStatus == ResponseStatus.Success)
                ViewBag.AssesmentYearList = assesmentyearResponse.Entities;

        }

        #endregion
    }
}
