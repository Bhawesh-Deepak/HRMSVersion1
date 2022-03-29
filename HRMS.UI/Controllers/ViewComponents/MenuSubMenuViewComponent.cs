using HRMS.Core.ReqRespVm.Response.UserManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HRMS.UI.Controllers.ViewComponents
{
    public class MenuSubMenuViewComponent : ViewComponent
    {
        private readonly string APIURL = string.Empty;
        public MenuSubMenuViewComponent(IConfiguration configuration)
        {
            APIURL = configuration.GetSection("APIURL").Value;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            using (var client = new HttpClient())
            {
                int roleId = Convert.ToInt32(HttpContext.Session.GetString("RoleId"));
                client.BaseAddress = new Uri(APIURL);
                var responseTask = await client.GetAsync("api/HRMS/MenuSubMenu/GetMenuSubMenu?roleId=" + roleId);
                if (responseTask.IsSuccessStatusCode)
                {
                    var responseDetails = await responseTask.Content.ReadAsStringAsync();
                    var menusubmenu = JsonConvert.DeserializeObject<List<MenuSubMenuVm>>(responseDetails);
                    return await Task.FromResult((IViewComponentResult)View("_MenuSubMenu", menusubmenu));
                }
                else
                {
                    return await Task.FromResult((IViewComponentResult)View("_MenuSubMenu"));
                }
            }
          
        }
    }
}
