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
        [Required(ErrorMessage = "Organisation  is required.")]
        public int OrganisationId { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        [Required(ErrorMessage = "Subsidiary name is required.")]
        [Display(Prompt = "Subsidiary name")]
        public string Name { get; set; }
        [Display(Prompt = "Subsidiary code")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Subsidiary logo is required.")]
        public string Logo { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        [Display(Prompt = "Subsidiary email")]

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Display(Prompt = "Subsidiary phone")]
        public string Phone { get; set; }
        public string ContactPerson { get; set; }

        [Display(Prompt = "Subsidiary Url")]
        public string Url { get; set; }
        public string FavIcon { get; set; }
    }
}
