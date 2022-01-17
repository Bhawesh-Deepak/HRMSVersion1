using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.Salary
{
    public class SalaryRegisterVM
    {

        public string Months { get; set; }
        public int Years { get; set; }
        public int DateMonth { get; set; }
        public string salutation { get; set; }
        public int Id { get; set; }
        public string empCode { get; set; }
        public string employeeName { get; set; }
        public DateTime dateOfBirth { get; set; }
        public DateTime joiningDate { get; set; }
        public DateTime confirmationDate { get; set; }
        public string AadharCardNumber { get; set; }
        public string BiometricCode { get; set; }
        public string BranchOfficeId { get; set; }
        public string departmentName { get; set; }
        public string DesignationName { get; set; }
        public string LegalEntity { get; set; }
        public string UANNumber { get; set; }
        public string PAndFBankAccountNumberx { get; set; }
        public string esicNo { get; set; }
        public string PanCardNumber { get; set; }
        public string ifscCode { get; set; }
        public string bankName { get; set; }
        public string bankAccountNumber { get; set; }
        public string Band { get; set; }
        public string Functions { get; set; }
        public string PermanentLocation { get; set; }
        public string SubDepartment { get; set; }
        public string Zone { get; set; }
        public string PTStateName { get; set; }
        public string Status_Description { get; set; }
        public DateTime exitDate { get; set; }
        public decimal WorkingDays { get; set; }
        public decimal ArrearDays { get; set; }
        public decimal LOPDays { get; set; }
        public decimal LFDays { get; set; }
        public decimal OthersDays { get; set; }
        public string ComponentName { get; set; }
        public int ComponentId { get; set; }
        public int ComponentType { get; set; }
        public decimal ComponentValue { get; set; }
        public decimal SalaryAmount { get; set; }
        public decimal CTC { get; set; }
        public string address { get; set; }
        public string ComponentCategory { get; set; }
        public string PAndLHeadName { get; set; }
    }
}
