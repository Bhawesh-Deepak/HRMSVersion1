using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Talent
{
    [Table("LearningAndDevelopment", Schema = "Talent")]
    public class LearningAndDevelopment : BaseModel<int>
    {
        [Required(ErrorMessage = "this field is required.")]
        public int DesignationId { get; set; }
        [Required(ErrorMessage = "this field is required.")]
        public int LAndDHourId { get; set; }
        [NotMapped]
        public string DesignationName { get; set; }
        [NotMapped]
        public string LAndDHour { get; set; }
    }
}
