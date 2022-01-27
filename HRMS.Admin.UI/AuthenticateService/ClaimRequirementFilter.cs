using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HRMS.Admin.UI.AuthenticateService
{
    public  class ClaimRequirementFilter: IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var url = context.HttpContext.Request.GetDisplayUrl();

            if (context.HttpContext.Session.GetString("UserDetail") == null)
            {
                context.Result = new RedirectToActionResult("GetLoginPopUp", "Authenticate", new
                {
                    returnUrl = url
                });
            }
           
        }
    }
}