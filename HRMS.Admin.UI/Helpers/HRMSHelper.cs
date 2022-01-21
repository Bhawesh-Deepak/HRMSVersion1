using HRMS.Admin.UI.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Helpers
{
    public static class HRMSHelper
    {
        public static IHtmlContent HRMSButtonHelper(this IHtmlHelper htmlHelper, string className, int insertUpdate)
        {
            var text = insertUpdate == 0 ? "Submit" : "Update";
            var htmlContent = $" <button type='submit' class='{className}'>{text}</button>";
            return new HtmlString(htmlContent);
        }

        public static IHtmlContent HRMSTableHeader(this IHtmlHelper htmlHelper, PaggerHeaderModel modelEntity)
        {

            var totalRecord = modelEntity.TotalRecordCount;
            string records = "<b>Total Record(s):</b> " + modelEntity.TotalRecordCount;
            int pageIndex = 1;
            int recordCount;

            if (modelEntity.TotalRecordCount > 0)
            {
                pageIndex = (modelEntity.PageIndex - 1) * modelEntity.PageSize + 1;
            }

            if (modelEntity.PageIndex * modelEntity.PageSize < totalRecord)
            {
                recordCount = modelEntity.PageIndex * modelEntity.PageSize;
            }
            else
            {
                recordCount = totalRecord;
            }
            var htmlContent = $"<div class='col-xs-12 marB10'>";
            htmlContent += $"<div class='pull-left recordInfo font-bold' id='_noofrecords'>" + records + "</div>";
            htmlContent += $"<div class='pull-right recordInfo' id='recordbypage'>" + pageIndex + " - " + recordCount + " of " + totalRecord + "</div>";
            htmlContent += $"</div>";
            return new HtmlString(htmlContent);
        }
    }
}
