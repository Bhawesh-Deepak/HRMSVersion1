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
        [Required(ErrorMessage = "AssetsCategory is required.")]
        public int AssetsCategoryId { get; set; }

        [Required(ErrorMessage = "Employee is required.")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Branch is required.")]
        public int BranchId { get; set; }

       
        [Display(Prompt = "AssetsTag")]
        public string AssetsTag { get; set; }

    
        [Display(Prompt = "Serial Number")]
        public string SerialNumber { get; set; }

     
        [Display(Prompt = "Model Numbar")]
        public string ModelNumbar { get; set; }

    
        [Display(Prompt = "MEID")]
        public string MEID { get; set; }


      
        [Display(Prompt = "Assets Status")]
        public string AssetsStatus { get; set; }

       
        [Display(Prompt = "Category")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Employee Assets is required.")]
        [Display(Prompt = "Employee Assets Name")]
        public string Name { get; set; }

       
        [Display(Prompt = "Make")]
        public string Make { get; set; }

       
        [Display(Prompt = "Dongle Number")]
        public string DongleNumber { get; set; }
    }
}
