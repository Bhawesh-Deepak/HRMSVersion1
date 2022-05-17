using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Investment
{
    [Table("InvestmentDeclarationType", Schema = "Investment")]
    public class InvestmentDeclarationType:BaseModel<int>
    {
        public string EmpCode { get; set; }
        public int DeclarationType { get; set; }
         
    }
}
