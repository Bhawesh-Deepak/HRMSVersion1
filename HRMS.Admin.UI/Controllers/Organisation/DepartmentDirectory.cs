using HRMS.Admin.UI.AuthenticateService;
using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Master;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Organisation
{
    [CustomAuthenticate]
    public class DepartmentDirectory : Controller
    {
        private readonly IGenericRepository<Department, int> _IDepartmentRepository;
        private readonly IGenericRepository<Designation, int> _IDesignationRepository;

        public DepartmentDirectory(IGenericRepository<Department, int> DepartmentRepository, IGenericRepository<Designation, int> DesignationRepository)
        {
            _IDepartmentRepository = DepartmentRepository;
            _IDesignationRepository = DesignationRepository;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.HeaderTitle = PageHeader.HeaderSetting["Department Directory"];
            try
            {
                var departmentList = new DBResponseHelper<Department, int>()
                    .GetDBResponseHelper(await _IDepartmentRepository
                    .GetAllEntities(x => x.IsActive && !x.IsDeleted));
                var designationList = new DBResponseHelper<Designation, int>()
                  .GetDBResponseHelper(await _IDesignationRepository
                  .GetAllEntities(x => x.IsActive && !x.IsDeleted));
                var response = from department in departmentList.Item2.Entities
                               join designation in designationList.Item2.Entities
                               on department.Id equals designation.DepartmentId
                               select new Designation
                               {
                                   Id = department.Id,
                                   DepartmentName =department.Name,
                                   Name = designation.Name,
                                   
                               };

                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("DepartmentDirectory", "_DepartmentDirectoryIndex"), response));

            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(DepartmentDirectory)} action name {nameof(Index)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }
    }
}
