using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Organisation
{

    [Table("Company", Schema = "Organisation")]
    public class Subsidiary : BaseModel<int>
    {
        [Required(ErrorMessage = "This field  is required.")]
        public int OrganisationId { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [Display(Prompt = "Subsidiary Name")]
        public string Name { get; set;  }
        [Display(Prompt = "Code")]
        public string Code { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string Logo { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }

        [Display(Prompt = "Email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Incorrect Email Format")]
       
        public string Email { get; set; }
        [Display(Prompt = "Phone")]
        public string Phone { get; set; }
        public string ContactPerson { get; set; }

        [Display(Prompt = "Url")]
        public string Url { get; set; }
        public string FavIcon { get; set; }
    }
}
