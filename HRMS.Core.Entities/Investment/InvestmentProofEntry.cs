using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Investment
{
    [Table("InvestmentProofEntry", Schema = "Investment")]
    public class InvestmentProofEntry:BaseModel<int>
    {
        public int InvestmentChildNodeId { get; set; }
        public string EmpCode { get; set; }
        public decimal AmountValue { get; set; } = 0;
        public string Reason { get; set; }
        public int ProofStatus { get; set; }
        public string ProofUrl { get; set; }
       
    }
}
