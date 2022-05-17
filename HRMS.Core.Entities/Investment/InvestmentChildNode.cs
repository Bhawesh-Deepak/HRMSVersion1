using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Investment
{
    [Table("InvestmentChildNode", Schema = "Investment")]
    public class InvestmentChildNode : BaseModel<int>
    {      
        public int InvestmentParticularId { get; set; }
        public string Name { get; set; }
        public string LabelName { get; set; }
        public decimal Value { get; set; }
        public string IsReadonly { get; set; }
        public int IsRadio { get; set; }
        public int IsTentitiveDeclared { get; set; }
        
    }
}
