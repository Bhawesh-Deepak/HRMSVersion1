using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Investment
{
    [Table("LandLordDetail", Schema = "Investment")]
    public class LandLordDetail:BaseModel<int>
    {
        public string EmpCode { get; set; }
        public string LandLordName { get; set; }
        public string LandLordPAN { get; set; }
        public string FileUrl { get; set; }
     
    }
}
