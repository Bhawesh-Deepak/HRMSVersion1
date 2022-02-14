using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.UserManagement
{
    public class MenuSubMenuVm
    {
        public int ModuleId { get; set; }
        public int SubModuleId { get; set; }
        public int ChildNodeId { get; set; }
        public string ModuleName { get; set; }
        public string SubModuleName { get; set; }
        public string ChildNodeName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string ModuleIcon { get; set; }
        public string SubModuleIcon { get; set; }
        public string ChilNodeIcon { get; set; }
        public int DisplayOrder { get; set; }
    }
}
