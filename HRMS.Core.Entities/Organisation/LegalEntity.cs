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
    public class LegalEntity : BaseModel<int>
    {
        [Required(ErrorMessage = "This field  is required.")]
        public int OrganisationId { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [Display(Prompt = "Legal Entity")]
        public string Name { get; set;  }
        [Display(Prompt = "Code")]
        [Required(ErrorMessage = "This field is required.")]
        public string Code { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string Logo { get; set; }
        [Display(Prompt = "Registered Office Address")]
        [Required(ErrorMessage = "This field is required.")]
        public string Address { get; set; }
        public string ZipCode { get; set; }

        [Display(Prompt = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "This field is required.")]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Invalid Email Id")]
       
        public string Email { get; set; }
        [Display(Prompt = "Phone")]
        [Required(ErrorMessage = "This field is required.")]
        public string Phone { get; set; }
        public string ContactPerson { get; set; }

        [Display(Prompt = "Url")]
        public string Url { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string FavIcon { get; set; }
    }
}
