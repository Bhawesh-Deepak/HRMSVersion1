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
    [Table("AssetsCategory", Schema = "Master")]
    public class AssetsCategory : BaseModel<int>
    {
        public string Name { get; set; }
    }
}
