using HRMS.Core.Helpers.CommonHelper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.UserManagement
{
    public class CommingSoonController : Controller
    {
        public async Task<IActionResult> Index()
        {
            try
            {
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("CommingSoon", "_CommingSoonIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CommingSoonController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

    }
}
