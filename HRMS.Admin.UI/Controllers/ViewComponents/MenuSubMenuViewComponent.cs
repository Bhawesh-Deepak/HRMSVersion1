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
        private readonly IGenericRepository<MenuChildNode, int> _IMenuChildNodeRepository;
        private readonly IGenericRepository<RoleAccess, int> _IRoleAccessRepository;

        public MenuSubMenuViewComponent(IGenericRepository<RoleAccess, int> iRoleAccessRepository,
            IGenericRepository<SubModuleMaster, int> iSubModuleRepository, IGenericRepository<ModuleMaster, int> iMoudleMasterRepository,
            IGenericRepository<MenuChildNode, int> iMenuChildNodeRepository)
        {
            _IRoleAccessRepository = iRoleAccessRepository;
            _ISubModuleRepository = iSubModuleRepository;
            _IModuleRepository = iMoudleMasterRepository;
            _IMenuChildNodeRepository = iMenuChildNodeRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var models = new MenuSubMenuVm();
            int roleId = Convert.ToInt32(HttpContext.Session.GetString("RoleId"));

            var roleAccess = await _IRoleAccessRepository.GetAllEntities(x => x.RoleId == roleId && x.IsActive && !x.IsDeleted);
            var moduleDetails = await _IModuleRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var subModuleDetails = await _ISubModuleRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var menuChildNodeDetails = await _IMenuChildNodeRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

            var response = (from rd in roleAccess.Entities 
                            join md in moduleDetails.Entities on rd.ModuleId equals md.Id
                            join sm in subModuleDetails.Entities on rd.SubModuleId equals sm.Id
                            join cn in menuChildNodeDetails.Entities on rd.ChildNodeId equals cn.Id
                            select new MenuSubMenuVm()
                            {
                                ModuleId= md.Id,
                                SubModuleId= sm.Id,
                                ChildNodeId= cn.Id,
                                ModuleName= md.ModuleName,
                                SubModuleName= sm.SubModuleName,
                                ChildNodeName= cn.ChildNodeName,
                                ModuleIcon= md.ModuleIcon,
                                SubModuleIcon=sm.SubModuleIcon,
                                ChilNodeIcon=cn.ChildNodeIcon,
                                ControllerName=cn.ControllerName,
                                ActionName=cn.ActionName,
                                DisplayOrder=rd.DisplayOrder
                                ,MenuLevel=sm.MenuLevel
                            }).OrderBy(x=>x.DisplayOrder).ToList();

            return await Task.FromResult((IViewComponentResult)View("_MenuSubMenu", response));
        }
    }
}
