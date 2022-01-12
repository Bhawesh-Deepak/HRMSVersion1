using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Payroll
{
    [Table("CtcComponentDetail", Schema = "Payroll")]
    public class CtcComponentDetail : BaseModel<int>
    {
        public string ComponentName { get; set; }
        public int ComponentType { get; set; }
        public int ComponentValueType { get; set; }
        public string ComponentCategory { get; set; }
        public int CalculationOrder { get; set; }

    }
}
