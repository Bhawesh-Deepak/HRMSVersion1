using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Talent
{
    [Table("LAndDHour", Schema = "Talent")]
    public class LAndDHour : BaseModel<int>
    {
        public string Name { get; set; }
    }
}
