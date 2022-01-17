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
    public class CompanyHolidays : BaseModel<int>
    {

       
        public int StateId { get; set; }
        
        [Required(ErrorMessage = "this filed is required")]
        [Display(Prompt = "Holiday Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "this filed is required")]
        [DataType(DataType.Date)]
        public DateTime HolidayDate { get; set; }
    }
}
