using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Investment
{
    [Table("EmployeeInvestmentDecalaration", Schema = "Investment")]
    public class EmployeeInvestmentDecalaration : BaseModel<int>
    {
        public int InvestmentMasterId { get; set; }
        public int InvestmentParticularId { get; set; }
        public int InvestmentChildNodeId { get; set; }
        public string EmpCode { get; set; }
        public decimal MaxAmount { get; set; }
        public decimal DeclaredAmount { get; set; }
        public decimal SubmitedAmount { get; set; }
        public decimal VerifiedAmount { get; set; }
        public int NoOfChildren { get; set; }
        public int LocatonId { get; set; }
         
    }
}
