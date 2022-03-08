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
    [Table("PerformanceIndication", Schema = "Talent")]
    public class PerformanceIndication : BaseModel<int>
    {
        [Required(ErrorMessage = "this field is required.")]
        public int DesgignationId { get; set; }
        [Required(ErrorMessage = "this field is required.")]
        public string TechnicalCompetency { get; set; }
        [Required(ErrorMessage = "this field is required.")]
        public string OrganizationCompetency { get; set; }
    }
}
