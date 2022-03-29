using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.UI.AuthenticateService
{
    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var url = context.HttpContext.Request.GetDisplayUrl();

            if (context.HttpContext.Session.GetString("UserDetail") == null)
            {
                context.Result = new RedirectToActionResult("Index", "Authenticate", new
                {
                    returnUrl = url
                });
            }

        }
    }
}
