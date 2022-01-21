using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.UserManagement
{
    public class MenuSubMenuVm
    {
        public string MenuName { get; set; }
        public string MenuIcon { get; set; }
        public string SubMenuName { get; set; }
        public string Controller { get; set; }
        public string ActionName { get; set; }
        public string SubMenuIcon { get; set; }
        public int DisplayOrder { get; set; }
    }
}
