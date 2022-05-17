using HRMS.Core.Entities.Investment;
using HRMS.Core.ReqRespVm.RequestVm.Investment;
using HRMS.Core.ReqRespVm.Response.Investment;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.UI.Helper
{
    public class InvestmentReadAPIHelper
    {

        public async Task<InvestmentDeclarationType> GetInvestmentDeclarationType(string EmpCode, int FinancialYear, string APIURL)
        {
            try
            {
                InvestmentDeclarationType DeclarationType = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIURL);
                    var responseTask = await client.GetAsync("/api/HRMS/DeclarationType/GetAllDeclarationType?EmpCode=" + EmpCode + "&FinancialYear=" + FinancialYear);
                    if (responseTask.IsSuccessStatusCode)
                    {
                        var responseDetails = await responseTask.Content.ReadAsStringAsync();
                        DeclarationType = JsonConvert.DeserializeObject<InvestmentDeclarationType>(responseDetails);
                        return DeclarationType;
                    }
                    else
                    {
                        return DeclarationType;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<InvestmentDeclarationWindow> GetInvestmentDeclarationWindow(string APIURL)
        {
            try
            {
                InvestmentDeclarationWindow DeclarationWindow = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIURL);
                    var responseTask = await client.GetAsync("api/HRMS/InvestmentDeclarationWindow/GetInvestmentDeclarationWindow");
                    if (responseTask.IsSuccessStatusCode)
                    {
                        var responseDetails = await responseTask.Content.ReadAsStringAsync();
                        DeclarationWindow = JsonConvert.DeserializeObject<InvestmentDeclarationWindow>(responseDetails);
                        return DeclarationWindow;
                    }
                    else
                    {
                        return DeclarationWindow;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<InvestmentDeclarationOldMethodVM>> GetInvestmentDeclarationOldMethod(string EmpCode, int FinancialYear, string APIURL)
        {
            try
            {
                List<InvestmentDeclarationOldMethodVM> OldMethod = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIURL);
                    var responseTask = await client.GetAsync("api/HRMS/InvestmentDeclaration/GetInvestmentDeclarationOldMethod?EmpCode=" + EmpCode + "&FinancialYear=" + FinancialYear);
                    if (responseTask.IsSuccessStatusCode)
                    {
                        var responseDetails = await responseTask.Content.ReadAsStringAsync();
                        OldMethod = JsonConvert.DeserializeObject<List<InvestmentDeclarationOldMethodVM>>(responseDetails);
                        return OldMethod;
                    }
                    else
                    {
                        return OldMethod;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<bool> PostInvestmentDeclarationOldMethod(List<InvestmentDeclarationVM> declarationVM, string APIURL)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIURL);
                    var stringContent = new StringContent(JsonConvert.SerializeObject(declarationVM), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("/api/HRMS/InvestmentDeclaration/PostInvestmentDeclarationOldMethod", stringContent);
                    if (response.IsSuccessStatusCode)
                    {
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> PostInvestmentDeclarationType(InvestmentDeclarationType declarationType, string APIURL)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIURL);
                    var stringContent = new StringContent(JsonConvert.SerializeObject(declarationType), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("/api/HRMS/DeclarationType/PostDeclarationType", stringContent);
                    if (response.IsSuccessStatusCode)
                    {
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> ClaculateTDS(string EmpCode, int FinancialYear, string APIURL)
        {
            try
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIURL);
                    var responseTask = await client.GetAsync("/api/HRMS/InvestmentDeclaration/CalculateTDS?EmpCode=" + EmpCode + "&FinancialYear=" + FinancialYear);
                    if (responseTask.IsSuccessStatusCode)
                    {

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<ComputationOfTaxReportVM> GetComputationOfTaxReport(string EmpCode, int FinancialYear, string APIURL)
        {
            try
            {
                ComputationOfTaxReportVM computationOfs = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIURL);
                    var responseTask = await client.GetAsync("/api/HRMS/EmployeeTentitiveTDS/ComputationOfTaxReport?EmpCode=" + EmpCode + "&FinancialYear=" + FinancialYear);
                    if (responseTask.IsSuccessStatusCode)
                    {
                        var responseDetails = await responseTask.Content.ReadAsStringAsync();
                        computationOfs = JsonConvert.DeserializeObject<ComputationOfTaxReportVM>(responseDetails);
                    }

                }
                return computationOfs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<bool> PostInvestmentDeclarationPDFDetails(InvestmentDeclarationPDFDetails declarationPDFDetails, string APIURL)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIURL);
                    var stringContent = new StringContent(JsonConvert.SerializeObject(declarationPDFDetails), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("/api/HRMS/InvestmentDeclarationPDF/CreateInvestmentDeclarationPDFDetails", stringContent);
                    if (response.IsSuccessStatusCode)
                    {
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<InvestmentProofEntry> DownloadProofEntry(string EmpCode, int FinancialYear, string APIURL, int InvestmentChildNodeId)
        {
            try
            {
                List<InvestmentProofEntry> proofEntries = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIURL);
                    var responseTask = await client.GetAsync("/api/HRMS/ProofEntry/GetProofEntry?EmpCode=" + EmpCode + "&FinancialYear=" + FinancialYear + "&InvestmentChildNodeId=" + InvestmentChildNodeId);
                    if (responseTask.IsSuccessStatusCode)
                    {
                        var responseDetails = await responseTask.Content.ReadAsStringAsync();
                        proofEntries = JsonConvert.DeserializeObject<List<InvestmentProofEntry>>(responseDetails);
                        return proofEntries.FirstOrDefault();
                    }
                    else
                    {
                        return proofEntries.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<bool> UpdateProofEntry(InvestmentProofEntry investmentProofEntry, string APIURL)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIURL);
                    var stringContent = new StringContent(JsonConvert.SerializeObject(investmentProofEntry), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("/api/HRMS/ProofEntry/UpdateInvestmentProofEntry", stringContent);
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
