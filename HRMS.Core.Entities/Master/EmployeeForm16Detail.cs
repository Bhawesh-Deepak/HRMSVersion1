using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Master
{
    [Table("EmployeeForm16Detail", Schema = "Master")]
    public class EmployeeForm16Detail : BaseModel<int>
    {
        public string EmpCode { get; set; }
        public string Form16PartA { get; set; }
        public string Form16PartB { get; set; }
        public string Form16MergedFile { get; set; }
    }
}
