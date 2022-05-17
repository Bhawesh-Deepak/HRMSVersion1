using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.RequestVm.Investment
{
    public class InvestmentDeclarationVM
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
        public int FinancialYear { get; set; }
        public int CreatedBy { get; set; }
    }
}
