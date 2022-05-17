using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Investment
{
    [Table("InvestmentParticular", Schema = "Investment")]
    public class InvestmentParticular : BaseModel<int>
    {
        public int InvestmentMasterId { get; set; }
        public int Name { get; set; }
        public decimal MaxAmount { get; set; }
        public int ComponentID { get; set; }
        public int NoOfChildren { get; set; }
        public int DateMonth { get; set; }

    }
}
