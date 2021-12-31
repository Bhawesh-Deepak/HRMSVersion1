using HRMS.Core.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Core.Entities.Organisation
{
    [Table("Organisation", Schema = "Organisation")]
    public class Company : BaseModel<int>
    {
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        [Required(ErrorMessage = "Company name is required.")]
        [Display(Prompt ="Company name")]
        public string Name { get; set; }
        [Display(Prompt = "Company code")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Company logo is required.")]
        public string Logo { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        [Display(Prompt = "Company email")]
        public string Email { get; set; }
        [Display(Prompt = "Company phone")]
        public string Phone { get; set; }
        public string ContactPerson { get; set; }

        [Display(Prompt = "Company Url")]
        public string Url { get; set; }
        public string FavIcon { get; set; }
    }
}
