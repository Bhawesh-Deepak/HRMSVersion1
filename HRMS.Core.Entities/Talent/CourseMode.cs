using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Talent
{
    [Table("CourseMode", Schema = "Talent")]
    public class CourseMode : BaseModel<int>
    {
        public string Name { get; set; }
    }
}
