using HRMS.Admin.UI.Helpers;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.Helpers.ExcelHelper;
using HRMS.Core.ReqRespVm.RequestVm;
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
    public class ECRController : Controller
    {
        private readonly IHostingEnvironment _IHostingEnviroment;
        private readonly IDapperRepository<ECRParams> _IECRRepository;
        public ECRController(IHostingEnvironment hostingEnvironment, IDapperRepository<ECRParams> ecrRepository)
        {
            _IHostingEnviroment = hostingEnvironment;
            _IECRRepository = ecrRepository;

        }
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View(ViewHelper.GetViewPathDetails("Reports", "_ECRReportDetail")));
        }

        [HttpPost]
        public async Task<IActionResult> ExportECRReportDetails(EmployeeSalaryRegisterVM model)
        {
            string empresponse = null;

            if (model.UploadFile != null)
                empresponse = new ReadEmployeeCode().GetSalaryRegisterEmpCodeDetails(model.UploadFile);

            var ecrParams = new ECRParams()
            {
                DateMonth = model.DateMonth,
                DateYear = model.DateYear,
                EmployeeCode = empresponse
            };

            var response = await Task.Run(() => _IECRRepository.GetAll<ECRReportModel>(SqlQuery.GetECRReport, ecrParams));

            CreateFileInRoot.CreateFileIfNotExistsOrDelete("ECRReport", _IHostingEnviroment,
                string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, "ECRReport.xlsx"));

            ExcelPackage Eps = new ExcelPackage();
            ExcelWorksheet Sheets = Eps.Workbook.Worksheets.Add("ECRReport");
           
            Sheets.Cells["A1:E1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            Sheets.Cells["A1:E1"].Style.Fill.BackgroundColor.SetColor(Color.Gray);
            Eps.Encryption.Password = "sqy" + model.DateMonth + "" + model.DateYear;
            Sheets.Cells["A1"].Value = "UAN";
            Sheets.Cells["B1"].Value = "Member_Name";
            Sheets.Cells["C1"].Value = "GROSS_WAGES";
            Sheets.Cells["D1"].Value = "EPF_WAGES";
            Sheets.Cells["E1"].Value = "EPS_WAGES";
            Sheets.Cells["F1"].Value = "EDLI_WAGES";
            Sheets.Cells["G1"].Value = "EFP_CONTRI_REMITTED";
            Sheets.Cells["H1"].Value = "EPS_CONTRI_REMITTED";
            Sheets.Cells["I1"].Value = "EPS_EPF_DIFF_REMITTED";
            Sheets.Cells["J1"].Value = "NCP_DAYS";
            Sheets.Cells["K1"].Value = "REFUND_OF_ADVANCE";
            int row = 2;
            foreach (var data in response)
            {
                Sheets.Cells[string.Format("A{0}", row)].Value = data.UANNumber;
                Sheets.Cells[string.Format("B{0}", row)].Value = data.EmployeeName;
                Sheets.Cells[string.Format("C{0}", row)].Value = data.GrossWages;
                Sheets.Cells[string.Format("D{0}", row)].Value = data.EPFWages;
                Sheets.Cells[string.Format("E{0}", row)].Value = data.EPSWages;
                Sheets.Cells[string.Format("F{0}", row)].Value = data.EDLIWages;
                Sheets.Cells[string.Format("G{0}", row)].Value = data.EPFCONTRIREMITTED;
                Sheets.Cells[string.Format("H{0}", row)].Value = (data.EPSWages * Convert.ToDecimal((8.33 / 100))).ToString();
                Sheets.Cells[string.Format("I{0}", row)].Value = (data.EPFCONTRIREMITTED - (data.EPSWages * Convert.ToDecimal((8.33 / 100)))).ToString();
                Sheets.Cells[string.Format("J{0}", row)].Value = "0";
                Sheets.Cells[string.Format("K{0}", row)].Value = "0";
                row++;
            }
            var stream = new MemoryStream(Eps.GetAsByteArray());
            return File(stream.ToArray(), "application/vnd.ms-excel", "ECRReport.xlsx");
        }
    }
}
