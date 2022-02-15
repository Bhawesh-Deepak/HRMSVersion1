using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Talent
{
    public class LearningandDevelopment : BaseModel<int>
    {
        public int DesignationId { get; set; }
        public int HRS { get; set; }
    }
}
