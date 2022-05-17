using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Investment
{
    [Table("InvestmentDeclarationWindow", Schema = "Investment")]
    public class InvestmentDeclarationWindow:BaseModel<int>
    {
        public int isWindowOpen { get; set; }
    }
}
