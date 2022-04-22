using HRMS.Admin.UI.AuthenticateService;
using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Entities.Organisation;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.Helpers.ExcelHelper;
using HRMS.Core.ReqRespVm.RequestVm;
using HRMS.Core.ReqRespVm.Response.Reporting;
using HRMS.Core.ReqRespVm.SqlParams;
using HRMS.Services.Implementation.SqlConstant;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Posting
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class PaySlipMailingController : Controller
    {
        private readonly IHostingEnvironment _IHostingEnviroment;
        private readonly IDapperRepository<EmployeePaySlipParams> _IEmployeePaySlipRepository;
        private readonly IGenericRepository<AssesmentYear, int> _IAssesmentYearRepository;
        public PaySlipMailingController(IHostingEnvironment hostingEnvironment,
            IGenericRepository<AssesmentYear, int> assesmentyearRepo,
            IDapperRepository<EmployeePaySlipParams> employeepayslipRepository)
        {
            _IHostingEnviroment = hostingEnvironment;
            _IEmployeePaySlipRepository = employeepayslipRepository;
            _IAssesmentYearRepository = assesmentyearRepo;
        }

        public async Task<IActionResult> Index()
        {
            try
            {

                await PopulateViewBag();
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("PaySlipMailing", "_PaySlipMailingIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CandidateReferalController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> DownloadPaySlip(EmployeeSalaryRegisterVM model)
        {
            try
            {
                string empresponse = null;
                if (model.UploadFile != null && model.EmployeeCode == null)
                    empresponse = new ReadEmployeeCode().GetSalaryRegisterEmpCodeDetails(model.UploadFile);
                else if (model.UploadFile == null && model.EmployeeCode != null)
                    empresponse = model.EmployeeCode;

                var payslipparams = new EmployeePaySlipParams()
                {
                    DateMonth = model.DateMonth,
                    DateYear = model.DateYear,
                    EmployeeCode = empresponse
                };
                System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                string strMonthName = mfi.GetMonthName(model.DateMonth).ToString();
                var response = await Task.Run(() => _IEmployeePaySlipRepository.GetAll<EmployeePaySlipVM>(SqlQuery.GetPaySlip, payslipparams));
                if (response.Count() > 0)
                {
                    var responsepdf = new ViewAsPdf(ViewHelper.GetViewPathDetails("PaySlipMailing", "_PaySlip"), response)
                    {
                        FileName = strMonthName + "_" + model.DateYear + DateTime.Now.Ticks + "_PaySlip.pdf",
                    };
                    var filePath = Path.Combine(_IHostingEnviroment.WebRootPath, "PaySlipDocument//");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    byte[] pdfData = await responsepdf.BuildFile(this.ControllerContext);
                    using (var fileStream = new FileStream(Path.Combine(filePath, responsepdf.FileName), FileMode.Create, FileAccess.Write))
                    {
                        fileStream.Write(pdfData, 0, pdfData.Length);
                    }
                    var companyDetails = HttpContext.Session.GetObjectFromJson<Company>("companyDetails");
                    var messages = new MimeMessage();
                    messages.From.Add(new MailboxAddress(companyDetails.Name, "squarehr@kudotech.in"));
                    messages.To.Add(new MailboxAddress(companyDetails.Name, response.First().OfficeEmailId));
                    messages.Subject = companyDetails.Name + "    Pay Slip of month : " + strMonthName;
                    var body = new TextPart(TextFormat.Html)
                    {
                        Text = @"Dear " + response.First().EmployeeName + ",<br /> Please find the attached pay slip"
                    };
                    var attachment = new MimePart("image", "pdf")
                    {
                        Content = new MimeContent(System.IO.File.OpenRead(filePath + responsepdf.FileName)),
                        ContentDisposition = new MimeKit.ContentDisposition(MimeKit.ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = Path.GetFileName(filePath + responsepdf.FileName)
                    };
                    var multipart = new Multipart("mixed");
                    multipart.Add(body);
                    multipart.Add(attachment);
                    messages.Body = multipart;
                    using (var client = new MailKit.Net.Smtp.SmtpClient())
                    {

                        client.Connect("smtp.gmail.com", 587, false);
                        client.Authenticate("squarehr@kudotech.in", "Kudotech@1234");
                        client.Send(messages);
                        client.Disconnect(true);

                    }
                }
                else
                {
                    return RedirectToAction("Index", "PaySlipMailing");
                }

                return RedirectToAction("Index", "PaySlipMailing");


               

            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(PaySlipMailingController)} action name {nameof(DownloadPaySlip)} exception is {ex.Message}";
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
