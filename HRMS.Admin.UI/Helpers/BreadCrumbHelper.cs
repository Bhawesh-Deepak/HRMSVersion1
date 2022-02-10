using HRMS.Admin.UI.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Helpers
{
    public static class BreadCrumbHelper
    {
        private static readonly HtmlContentBuilder _emptyBuilder = new HtmlContentBuilder();
        private static List<BreadCrumbModel> breadCrumbModels = new List<BreadCrumbModel>();

        public static IHtmlContent BuildBreadcrumbNavigation(this IHtmlHelper helper)
        {
            if (helper.ViewContext.RouteData.Values["controller"].ToString() == "Home" ||
                helper.ViewContext.RouteData.Values["controller"].ToString() == "Account")
            {
                return _emptyBuilder;
            }

            string controllerName = helper.ViewContext.RouteData.Values["controller"].ToString();
            string actionName = helper.ViewContext.RouteData.Values["action"].ToString();

            breadCrumbModels.Add(new BreadCrumbModel() {
                Controller=controllerName,
                Action=actionName
            });

            var breadcrumb = new HtmlContentBuilder()
                                .AppendHtml("<ol class='breadcrumb'><li>")
                                .AppendHtml(helper.ActionLink("Home", "Index", "Home"))
                                .AppendHtml("</li><li>")
                                .AppendHtml(helper.ActionLink(controllerName,
                                                          "Index", controllerName))
                                .AppendHtml("</li>");


            if (helper.ViewContext.RouteData.Values["action"].ToString() != "Index")
            {
                breadcrumb.AppendHtml("<li>")
                          .AppendHtml(helper.ActionLink(actionName, actionName, controllerName))
                          .AppendHtml("</li>");
            }

            return breadcrumb.AppendHtml("</ol>");
        }
    }
}

