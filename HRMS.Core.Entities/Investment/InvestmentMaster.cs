using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Investment
{
    [Table("InvestmentMaster", Schema = "Investment")]
    public class InvestmentMaster : BaseModel<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int isRadiButton { get; set; }
        public string StyleSheet { get; set; }

    }
}
