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
    [Table("EmployeeAssets", Schema = "Master")]
    public class EmployeeAssets : BaseModel<int>
    {
        public int AssetsCategoryId { get; set; }
        public int EmployeeId { get; set; }
        public int BranchId { get; set; }

        [Required(ErrorMessage = "AssetsTag is required.")]
        [Display(Prompt = "AssetsTag Name")]
        public string AssetsTag { get; set; }

        [Required(ErrorMessage = "SerialNumber is required.")]
        [Display(Prompt = "SerialNumber Name")]
        public string SerialNumber { get; set; }

        [Required(ErrorMessage = "ModelNumbar is required.")]
        [Display(Prompt = "ModelNumbar Name")]
        public string ModelNumbar { get; set; }

        [Required(ErrorMessage = "MEID is required.")]
        [Display(Prompt = "MEID Name")]
        public string MEID { get; set; }


        [Required(ErrorMessage = "AssetsStatus is required.")]
        [Display(Prompt = "AssetsStatus Name")]
        public string AssetsStatus { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [Display(Prompt = "Category Name")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Employee Assets is required.")]
        [Display(Prompt = "Employee Assets Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Make is required.")]
        [Display(Prompt = "Make Name")]
        public string Make { get; set; }

        [Required(ErrorMessage = "DongleNumber is required.")]
        [Display(Prompt = "DongleNumber Name")]
        public string DongleNumber { get; set; }
    }
}
