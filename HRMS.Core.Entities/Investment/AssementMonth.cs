using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Investment
{
    [Table("AssementMonth", Schema = "Investment")]
    public class AssementMonth:BaseModel<int>
    {
        public int DateMonth { get; set; }
    }
}
