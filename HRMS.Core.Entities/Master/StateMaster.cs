using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Master
{
    [Table("CompanyHolidays", Schema = "Master")]
    public class StateMaster: BaseModel<int>
    {
        public int CountryId { get; set; }


        [Required(ErrorMessage = "Department name is required.")]
        [Display(Prompt = "Department Name")]
        public string StateName { get; set; }


        public string HolidayDate { get; set; }
    }
}
