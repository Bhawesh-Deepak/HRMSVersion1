using HRMS.Admin.UI.AuthenticateService;
using HRMS.Core.Entities.UserManagement;
using HRMS.Core.ReqRespVm.Response.UserManagement;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.ViewComponents
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class MenuSubMenuViewComponent : ViewComponent
    {
        private readonly IGenericRepository<ModuleMaster, int> _IModuleRepository;
        private readonly IGenericRepository<SubModuleMaster, int> _ISubModuleRepository;
        private readonly IGenericRepository<RoleAccess, int> _IRoleAccessRepository;

        public MenuSubMenuViewComponent(IGenericRepository<RoleAccess, int> iRoleAccessRepository,
            IGenericRepository<SubModuleMaster, int> iSubModuleRepository, IGenericRepository<ModuleMaster, int> iMoudleMasterRepository)
        {
            _IRoleAccessRepository = iRoleAccessRepository;
            _ISubModuleRepository = iSubModuleRepository;
            _IModuleRepository = iMoudleMasterRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var models = new MenuSubMenuVm();
            int roleId = Convert.ToInt32(HttpContext.Session.GetString("RoleId"));

            var roleAccess = await _IRoleAccessRepository.GetAllEntities(x => x.RoleId == roleId && x.IsActive && !x.IsDeleted);
            var moduleDetails = await _IModuleRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var subModuleDetails = await _ISubModuleRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

            var response = (from rd in roleAccess.Entities
                            join md in moduleDetails.Entities
                            on rd.ModuleId equals md.Id
                            join sm in subModuleDetails.Entities
                            on rd.SubModuleId equals sm.Id
                            select new MenuSubMenuVm()
                            {
                                MenuName= md.ModuleName,
                                MenuIcon= md.ModuleIcon,
                                SubMenuName= sm.SubModuleName,
                                ActionName= sm.ActionName,
                                Controller= sm.ControllerName,
                                SubMenuIcon= sm.SubModuleIcon,
                                DisplayOrder= rd.DisplayOrder

                            }).OrderBy(x=>x.DisplayOrder).ToList();

            return await Task.FromResult((IViewComponentResult)View("_MenuSubMenu", response));
        }
    }
}
