using HRMS.Admin.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Helpers
{
    public static class PagingSortingHelper
    {
        /// <summary>
        /// Extension method for  Table Pagging and Sorting
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="modelEntity"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        public static TModel PopulateModelForPagging<TModel>(TModel modelEntity, PageSize pageSize, int pageIndex, string sortBy, string sortOrder)
        {
            var modelType = modelEntity.GetType();
            var pinfos = modelType.GetProperties();
            foreach (var prop in pinfos)
            {
                if (prop.Name == "PageSize")
                {
                    prop.SetValue(modelEntity, pageSize.ToString() == "Default" ? PageSize.Size10 : pageSize, null);
                }

                if (prop.Name == "SortBy")
                {
                    prop.SetValue(modelEntity, sortBy + " " + sortOrder ?? "ASC", null);
                }
                if (prop.Name == "PageIndex")
                {
                    prop.SetValue(modelEntity, pageIndex == 0 ? 1 : pageIndex, null);
                }
            }
            return modelEntity;
        }


        /// <summary>
        /// Extension method for Table to create link for paging like 1,2 etc
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="modelEntity"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        public static TModel PupulateModelToDisplayPagging<TModel>(TModel modelEntity, PageSize pageSize,
            int pageIndex, string sortBy, string sortOrder)
        {
            var modelType = modelEntity.GetType();
            var pinfos = modelType.GetProperties();
            foreach (var prop in pinfos)
            {
                if (prop.Name == "SortBy")
                {
                    prop.SetValue(modelEntity, sortBy, null);
                }

                if (prop.Name == "PageIndex")
                {
                    prop.SetValue(modelEntity, pageIndex == 0 ? 1 : pageIndex, null);
                }
                if (prop.Name == "PageSize")
                {
                    prop.SetValue(modelEntity, pageSize.ToString() == "Default" ? PageSize.Size10 : pageSize, null);
                }
                if (prop.Name == "OrderBy")
                {
                    prop.SetValue(modelEntity, sortOrder ?? "ASC", null);
                }
            }
            return modelEntity;
        }
    }
}
