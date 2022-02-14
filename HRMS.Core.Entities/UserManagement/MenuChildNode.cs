using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.UserManagement
{
    [Table("MenuChildNode", Schema = "UserManagement")]
    public class MenuChildNode : Model<int>
    {
        public int SubModuleId { get; set; }
        public string ChildNodeName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string ChildNodeIcon { get; set; }
    }
}
