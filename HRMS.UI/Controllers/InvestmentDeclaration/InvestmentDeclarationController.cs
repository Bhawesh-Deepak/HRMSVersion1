using HRMS.Core.Entities.Investment;
using HRMS.Core.Entities.Organisation;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.RequestVm.Investment;
using HRMS.Core.ReqRespVm.Response.Investment;
using HRMS.UI.Helper;
using HRMS.UI.Helpers;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.UI.Controllers.InvestmentDeclaration
{
    public class InvestmentDeclarationController : Controller
    {
        private readonly string APIURL = string.Empty;
        private readonly IHostingEnvironment _IHostingEnviroment;
        public InvestmentDeclarationController(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            APIURL = configuration.GetSection("APIURL").Value;
            _IHostingEnviroment = hostingEnvironment;

        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var Window = await new InvestmentReadAPIHelper().GetInvestmentDeclarationWindow(APIURL);
                ViewBag.Window = Window.isWindowOpen;
                var DeclarationType = await new InvestmentReadAPIHelper().GetInvestmentDeclarationType(HttpContext.Session.GetString("EmpCode"), Convert.ToInt32(HttpContext.Session.GetString("financialYearId")), APIURL);
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("InvestmentDeclaration", "_InvestmentDeclarationIndex"), DeclarationType));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(InvestmentDeclarationController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> InvestmentDeclaration()
        {
            try
            {
                var Window = await new InvestmentReadAPIHelper().GetInvestmentDeclarationWindow(APIURL);
                ViewBag.Window = Window.isWindowOpen;
                var investmentDeclaration = await new InvestmentReadAPIHelper().GetInvestmentDeclarationOldMethod(HttpContext.Session.GetString("EmpCode"), Convert.ToInt32(HttpContext.Session.GetString("financialYearId")), APIURL);

                return await Task.Run(() => PartialView(ViewHelper.GetViewPathDetails("InvestmentDeclaration", "_InvestmentDeclaration"), investmentDeclaration));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(InvestmentDeclarationController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostInvestmentDeclaration(int[] hdnIV1, int[] hdnIV2, int[] hdnIV3, decimal[] DeclaredAmount, int[] hdnlocationId)
        {
            try
            {
                var declaration = new List<InvestmentDeclarationVM>();
                for (int i = 0; i < hdnIV1.Count(); i++)
                {
                    declaration.Add(new InvestmentDeclarationVM()
                    {
                        InvestmentMasterId = Convert.ToInt32(hdnIV1[i]),
                        InvestmentParticularId = Convert.ToInt32(hdnIV2[i]),
                        InvestmentChildNodeId = Convert.ToInt32(hdnIV3[i]),
                        DeclaredAmount = Convert.ToDecimal(DeclaredAmount[i]),
                        LocatonId = Convert.ToInt32(hdnlocationId[i]),
                        EmpCode = HttpContext.Session.GetString("EmpCode"),
                        FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId")),
                        CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId")),
                        MaxAmount = 0,
                        VerifiedAmount = 0,
                        NoOfChildren = 0,
                        SubmitedAmount = 0,
                    });
                }
                if (await new InvestmentReadAPIHelper().PostInvestmentDeclarationOldMethod(declaration, APIURL))
                {
                    var declarationType = new InvestmentDeclarationType()
                    {
                        EmpCode = HttpContext.Session.GetString("EmpCode"),
                        FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId")),
                        CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId")),
                        DeclarationType = 1,
                        CreatedDate = DateTime.Now,
                    };
                    var response = await new InvestmentReadAPIHelper().PostInvestmentDeclarationType(declarationType, APIURL);
                    var tdscalculte = await new InvestmentReadAPIHelper().ClaculateTDS(HttpContext.Session.GetString("EmpCode"), Convert.ToInt32(HttpContext.Session.GetString("financialYearId")), APIURL);
                }

                var tentitivetds = await new InvestmentReadAPIHelper().GetComputationOfTaxReport(HttpContext.Session.GetString("EmpCode"), Convert.ToInt32(HttpContext.Session.GetString("financialYearId")), APIURL);
                var responsepdf = new ViewAsPdf(ViewHelper.GetViewPathDetails("InvestmentDeclaration", "GeneratePDF"), tentitivetds)
                {
                    FileName = tentitivetds.EmployeSalary.First().EmpCode + "_" + DateTime.Now.Ticks + "_InvestmentDeclaration.pdf",
                };
                var declarationPDF = new InvestmentDeclarationPDFDetails()
                {
                    EmpCode = HttpContext.Session.GetString("EmpCode"),
                    FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId")),
                    CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId")),
                    FilePath = "/InvestmentDeclaration/" + responsepdf.FileName,
                    CreatedDate = DateTime.Now,
                };
                var pdfResponse = await new InvestmentReadAPIHelper().PostInvestmentDeclarationPDFDetails(declarationPDF, APIURL);
                var filePath = Path.Combine(_IHostingEnviroment.WebRootPath, "InvestmentDeclaration//");
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
                messages.From.Add(new MailboxAddress(companyDetails.Name, "payrollpgsj@gmail.com"));
                messages.To.Add(new MailboxAddress(companyDetails.Name, "squarehr@kudotech.in"));
                messages.Subject = "Tax Computation FY -" + HttpContext.Session.GetString("financialYear");
                var body = new TextPart(TextFormat.Html)
                {
                    Text = @"<p>Dear, " + tentitivetds.EmployeSalary.First().EmployeeName + "  <br /> <br/ >  Please find the attached tax computation for the FY  " + HttpContext.Session.GetString("financialYear") + " <br /> <br/ > If you have any queries please do email us on " + companyDetails.Email + "    <br /> <br /><br /> Thanks & Regards <br/ > <br/ >" + companyDetails.Name + "  HR Team, </P>"
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

                return RedirectToAction("Index", "InvestmentDeclaration");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(InvestmentDeclarationController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetLandLordDetail()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIURL);
                    var responseTask = await client.GetAsync("/api/HRMS/LandLord/GetLandLordDetail?EmpCode=" + HttpContext.Session.GetString("EmpCode") + "&FinancialYear=" + Convert.ToInt32(HttpContext.Session.GetString("financialYearId")));
                    if (responseTask.IsSuccessStatusCode)
                    {
                        var responseDetails = await responseTask.Content.ReadAsStringAsync();
                        var landlord = JsonConvert.DeserializeObject<LandLordDetail>(responseDetails);
                        if (landlord == null)
                        {
                            return Json(0);
                        }
                        else
                        {
                            return Json(landlord);
                        }

                    }
                    else
                    {
                        return Json(0);
                    }
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(InvestmentDeclarationController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> UploadLandLordPANFile()
        {

            string _imgname = string.Empty;
            string _virtualPath = string.Empty;
            var pic = Request.Form.Files[0];

            var upload = Path.Combine(_IHostingEnviroment.WebRootPath, "LandLordPanFile//");
            if (!Directory.Exists(upload))
            {
                Directory.CreateDirectory(upload);
            }
            using (FileStream fs = new FileStream(Path.Combine(upload, pic.FileName), FileMode.Create))
            {
                await pic.CopyToAsync(fs);
            }
            _virtualPath = "/LandLordPanFile/" + pic.FileName;


            return Json(Convert.ToString(_virtualPath));
        }

        [HttpGet]
        public async Task<IActionResult> UploadLandLord(string LandLordName, string LandLordPAN, string FileUrl)
        {
            try
            {
                var landLorddetails = new LandLordDetail()
                {
                    FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId")),
                    CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId")),
                    CreatedDate = DateTime.Now,
                    EmpCode = HttpContext.Session.GetString("EmpCode"),
                    LandLordName = LandLordName,
                    LandLordPAN = LandLordPAN,
                    FileUrl = FileUrl
                };

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIURL);
                    var stringContent = new StringContent(JsonConvert.SerializeObject(landLorddetails), Encoding.UTF8, "application/json");
                    var responseTask = await client.PostAsync("/api/HRMS/LandLord/PostLandLordDetail", stringContent);
                    if (responseTask.IsSuccessStatusCode)
                    {
                        return Json(1);
                    }
                    else
                    {
                        return Json(2);
                    }
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(InvestmentDeclarationController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetTaxCalculator()
        {
            try
            {
                var computationOfs = await new InvestmentReadAPIHelper().GetComputationOfTaxReport(HttpContext.Session.GetString("EmpCode"), Convert.ToInt32(HttpContext.Session.GetString("financialYearId")), APIURL);

                return await Task.Run(() => PartialView(ViewHelper.GetViewPathDetails("InvestmentDeclaration", "_TaxCalculator"), computationOfs));

            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(InvestmentDeclarationController)} action name {nameof(GetTaxCalculator)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> UploadProof()
        {
            try
            {

                return await Task.Run(() => PartialView(ViewHelper.GetViewPathDetails("InvestmentDeclaration", "_UploadProof")));

            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(InvestmentDeclarationController)} action name {nameof(GetTaxCalculator)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UploadPODataImage()
        {
            try
            {
                string _virtualPath = string.Empty;
                string _actualpath = string.Empty;
                StringBuilder builder = new StringBuilder();
                var formdatafile = Request.Form.Files;
                for (int i = 0; i < formdatafile.Count; i++)
                {
                    var upload = Path.Combine(_IHostingEnviroment.WebRootPath, "InvestmentDeclaration//ProofEntry//" + HttpContext.Session.GetString("EmpCode") + "//");
                    if (!Directory.Exists(upload))
                    {
                        Directory.CreateDirectory(upload);
                    }
                    using (FileStream fs = new FileStream(Path.Combine(upload, formdatafile[i].FileName), FileMode.Create))
                    {
                        await formdatafile[i].CopyToAsync(fs);
                    }
                    _virtualPath = "/InvestmentDeclaration/ProofEntry/" + HttpContext.Session.GetString("EmpCode") + "/" + formdatafile[i].FileName;
                    builder.Append(_virtualPath).Append(",");
                }
                _actualpath = builder.ToString().TrimEnd(new char[] { ',' });
                return Json(_actualpath);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(InvestmentDeclarationController)} action name {nameof(UploadPODataImage)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> InsertProof(int IDPPId, string ProofUrl)
        {
            try
            {
                var proof = new InvestmentProofEntry()
                {
                    InvestmentChildNodeId = IDPPId,
                    AmountValue = 0,
                    Reason = "",
                    ProofStatus = 1,
                    ProofUrl = ProofUrl,
                    FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId")),
                    CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId")),
                    CreatedDate = DateTime.Now,
                    EmpCode = HttpContext.Session.GetString("EmpCode"),
                };
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIURL);
                    var stringContent = new StringContent(JsonConvert.SerializeObject(proof), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("/api/HRMS/ProofEntry/PostInvestmentProofEntry", stringContent);
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(1);
                    }
                    else
                    {
                        return Json(2);
                    }
                }


            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(InvestmentDeclarationController)} action name {nameof(GetTaxCalculator)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> DownloadProof(int IV3)
        {
            try
            {
                var proof = await new InvestmentReadAPIHelper().DownloadProofEntry(HttpContext.Session.GetString("EmpCode"), Convert.ToInt32(HttpContext.Session.GetString("financialYearId")), APIURL, IV3);


                string[] pdffile = proof.ProofUrl.Split(',');

                var webRoot = _IHostingEnviroment.WebRootPath;
                var fileName = "MyProofZip.zip";
                var tempOutput = webRoot + "//InvestmentDeclaration//ProofEntry/" + proof.EmpCode + "/" + fileName;

                using (ZipOutputStream IzipOutputStream = new ZipOutputStream(System.IO.File.Create(tempOutput)))
                {
                    IzipOutputStream.SetLevel(9);
                    byte[] buffer = new byte[4096];
                    var imageList = new List<string>();
                    for (int i = 0; i < pdffile.Length; i++)
                    {
                        imageList.Add(webRoot + pdffile[i]);

                    }

                    for (int i = 0; i < imageList.Count; i++)
                    {
                        ZipEntry entry = new ZipEntry(Path.GetFileName(imageList[i]));
                        entry.DateTime = DateTime.Now;
                        entry.IsUnicodeText = true;
                        IzipOutputStream.PutNextEntry(entry);

                        using (FileStream oFileStream = System.IO.File.OpenRead(imageList[i]))
                        {
                            int sourceBytes;
                            do
                            {
                                sourceBytes = oFileStream.Read(buffer, 0, buffer.Length);
                                IzipOutputStream.Write(buffer, 0, sourceBytes);
                            } while (sourceBytes > 0);
                        }
                    }
                    IzipOutputStream.Finish();
                    IzipOutputStream.Flush();
                    IzipOutputStream.Close();
                }

                byte[] finalResult = System.IO.File.ReadAllBytes(tempOutput);
                if (System.IO.File.Exists(tempOutput))
                {
                    System.IO.File.Delete(tempOutput);
                }
                if (finalResult == null || !finalResult.Any())
                {
                    throw new Exception(String.Format("Nothing found"));

                }

                return File(finalResult, "application/zip", fileName);

            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(InvestmentDeclarationController)} action name {nameof(GetTaxCalculator)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> DeleteUploadedProof(int IDPPId)
        {
            try
            {
                var proof = await new InvestmentReadAPIHelper().DownloadProofEntry(HttpContext.Session.GetString("EmpCode"), Convert.ToInt32(HttpContext.Session.GetString("financialYearId")), APIURL, IDPPId);

                return await Task.Run(() => PartialView(ViewHelper.GetViewPathDetails("InvestmentDeclaration", "_DeleteUploadedProof"), proof));

            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(InvestmentDeclarationController)} action name {nameof(GetTaxCalculator)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> PostDeleteUploadedProof(int IDPPId, string ProofUrl, int InvestmentProofEntryId)
        {
            try
            {
                 
                var proof = new InvestmentProofEntry()
                {
                    Id = InvestmentProofEntryId,
                    ProofUrl = ProofUrl,
                    InvestmentChildNodeId=IDPPId
                };
                var proofentry = await new InvestmentReadAPIHelper().UpdateProofEntry(proof,APIURL);
                return Json(proofentry);

            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(InvestmentDeclarationController)} action name {nameof(GetTaxCalculator)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
