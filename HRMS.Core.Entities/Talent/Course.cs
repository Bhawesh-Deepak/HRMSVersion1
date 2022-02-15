using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Talent
{
    public class Course : BaseModel<int>
    {
        public int DesignationId { get; set; }
        public int mode { get; set; }
        public string Coursename { get; set; }
        public string CourseCode { get; set; }
        [DataType(DataType.Date)]

        public string StartDate { get; set; }
        [DataType(DataType.Date)]

        public string EndDate { get; set; }

        //  [DataType(DataType.Date)]

        public string TrainingDateTime { get; set; }

    }
}
