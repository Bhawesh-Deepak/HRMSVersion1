using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Investment
{
    [Table("InvestmentDeclarationPDFDetails", Schema = "Investment")]
    public class InvestmentDeclarationPDFDetails : BaseModel<int>
    {
        public string EmpCode { get; set; }
        public string FilePath { get; set; }
        [NotMapped]
        public string AssesmentYear { get; set; }

    }
}
