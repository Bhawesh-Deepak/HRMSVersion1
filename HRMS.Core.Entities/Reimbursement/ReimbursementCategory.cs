using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Reimbursement
{
    [Table("ReimbursementCategory", Schema = "Reimbursement")]
    public class ReimbursementCategory : BaseModel<int>
    {
        public string Name { get; set; }
    }
}
