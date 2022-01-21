using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Helpers
{
    public static class PagingHelper
    {
        public static IHtmlContent Paging(this IHtmlHelper htmlHelper, int pageSize,
          int pageIndex, int pageDisplayCount, int totalRecords, string urlMethod, string divContainer = "jqGrigTable.fn_GetLoadingDiv()", string formName = "")
        {
            var htmlReader = new StringBuilder();


            var total = Convert.ToInt32(totalRecords);

            var PagerDisplayCtunt = 5;//code to display the five link for pagging and for rest we will show ... << >> < > etc
            var totalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(total) / pageSize));
            var currentPage = pageIndex;
            var seed = currentPage % PagerDisplayCtunt == 0
                ? currentPage - PagerDisplayCtunt
                : currentPage - currentPage % PagerDisplayCtunt;
            const int firstPage = 1;
            var lastPage = totalPages;
            var prevPage = currentPage - 1;
            var nextPage = currentPage + 1;

            if (currentPage > 1)
            {
                htmlReader.Append("<li>");

                if (string.IsNullOrEmpty(formName))
                {
                    htmlReader.Append(
                 "<a href='javascript:void(0)' data-ajax='true' data-ajax-mode='replace' onclick='jqGrigTable.fn_GetTablePageIndex(" +
                 firstPage + ", " + urlMethod + "," + divContainer + " )'>");
                }
                else
                {
                    htmlReader.Append(
                 "<a href='javascript:void(0)' data-ajax='true' data-ajax-mode='replace' onclick='jqGrigTable.fn_GetTablePageIndex(" +
                 firstPage + ", " + urlMethod + "," + divContainer + "," + formName + " )'>");
                }


                htmlReader.Append("<<");
                htmlReader.Append("</a></li>");

                if (string.IsNullOrEmpty(formName))
                {
                    htmlReader.Append(
                            " <li><a href='javascript:void(0)' data-ajax='true' data-ajax-mode='replace' onclick='jqGrigTable.fn_GetTablePageIndex(" +
                            prevPage + "," + urlMethod + " ," + divContainer + ")'>");
                }
                else
                {
                    htmlReader.Append(
                            " <li><a href='javascript:void(0)' data-ajax='true' data-ajax-mode='replace' onclick='jqGrigTable.fn_GetTablePageIndex(" +
                            prevPage + "," + urlMethod + " ," + divContainer + "," + formName + ")'>");
                }


                htmlReader.Append("<</a></li>");

                if (string.IsNullOrEmpty(formName))
                {
                    if (currentPage - PagerDisplayCtunt > 0)
                        htmlReader.Append(
                            "<li><a href='javascript:void(0)' data-ajax='true' data-ajax-mode='replace' onclick='jqGrigTable.fn_GetTablePageIndex((" +
                            seed + " - " + pageDisplayCount + " + 1)," + urlMethod + "," + divContainer + " )'>...</a></li>");
                }
                else
                {
                    if (currentPage - PagerDisplayCtunt > 0)
                        htmlReader.Append(
                            "<li><a href='javascript:void(0)' data-ajax='true' data-ajax-mode='replace' onclick='jqGrigTable.fn_GetTablePageIndex((" +
                            seed + " - " + pageDisplayCount + " + 1)," + urlMethod + "," + divContainer + " ," + formName + ")'>...</a></li>");
                }

            }


            for (var i = seed + 1; i <= seed + PagerDisplayCtunt && i <= totalPages; i++)
            {
                var classResult = Convert.ToInt32(pageIndex) == i ? "active" : string.Empty;

                if (string.IsNullOrEmpty(formName))
                {
                    htmlReader.Append("<li> <a href='javascript:void(0)' class=" + classResult +
                                 " data-ajax='true' data-ajax-mode='replace' onclick='jqGrigTable.fn_GetTablePageIndex(" +
                                 i + "," + urlMethod + "," + divContainer + " )'>" + i + "</a> </li>");
                }
                else
                {
                    htmlReader.Append("<li> <a href='javascript:void(0)' class=" + classResult +
                                     " data-ajax='true' data-ajax-mode='replace' onclick='jqGrigTable.fn_GetTablePageIndex(" +
                                     i + "," + urlMethod + "," + divContainer + "," + formName + " )'>" + i + "</a> </li>");

                }


            }

            if (currentPage < totalPages)
            {
                if (string.IsNullOrEmpty(formName))
                {
                    if (currentPage + pageDisplayCount <= totalPages)
                        htmlReader.Append(
                            "<li><a href='javascript:void(0)' data-ajax='true' data-ajax-mode='replace' onclick='jqGrigTable.fn_GetTablePageIndex((" +
                            seed + " + " + pageDisplayCount + " + 1)," + urlMethod + "," + divContainer + ")' >...</a></li>");

                    htmlReader.Append(
                   "<li><a href='javascript:void(0)' data-ajax='true' data-ajax-mode='replace' onclick='jqGrigTable.fn_GetTablePageIndex(" +
                   nextPage + "," + urlMethod + "," + divContainer + ")'>></a ></li >");

                    htmlReader.Append(
                        "<li><a href='javascript:void(0)' data-ajax='true' data-ajax-mode='replace' onclick='jqGrigTable.fn_GetTablePageIndex(" + lastPage +
                        "," + urlMethod + "," + divContainer + ")'>>></a></li>");
                }
                else
                {
                    if (currentPage + pageDisplayCount <= totalPages)
                        htmlReader.Append(
                            "<li><a href='javascript:void(0)' data-ajax='true' data-ajax-mode='replace' onclick='jqGrigTable.fn_GetTablePageIndex((" +
                            seed + " + " + pageDisplayCount + " + 1)," + urlMethod + "," + divContainer + "," + formName + ")' >...</a></li>");

                    htmlReader.Append(
                   "<li><a href='javascript:void(0)' data-ajax='true' data-ajax-mode='replace' onclick='jqGrigTable.fn_GetTablePageIndex(" +
                   nextPage + "," + urlMethod + "," + divContainer + "," + formName + ")'>></a ></li >");

                    htmlReader.Append(
                        "<li><a href='javascript:void(0)' data-ajax='true' data-ajax-mode='replace' onclick='jqGrigTable.fn_GetTablePageIndex(" + lastPage +
                        "," + urlMethod + "," + divContainer + "," + formName + ")'>>></a></li>");
                }
            }

            return new HtmlString(htmlReader.ToString());
        }
    }
}
