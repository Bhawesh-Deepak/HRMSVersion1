﻿using HRMS.Admin.UI.AuthenticateService;
using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.UserManagement;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.Response.UserManagement;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.UserManagement
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class SubModuleController : Controller
    {
        private readonly IGenericRepository<ModuleMaster, int> _IModuleMasterRepository;
        private readonly IGenericRepository<SubModuleMaster, int> _ISubModuleRepository;

        public SubModuleController(IGenericRepository<ModuleMaster, int> moduleRepo, IGenericRepository<SubModuleMaster, int> subModuleRepo)
        {
            _IModuleMasterRepository = moduleRepo;
            _ISubModuleRepository = subModuleRepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.HeaderTitle = PageHeader.HeaderSetting["SubModuleMaster"];

                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("SubModule", "SubModuleIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(SubModuleMaster)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSubModuleDetails()
        {
            try
            {
                var subModuleResponse = await _ISubModuleRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var moduleResponse = await _IModuleMasterRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

                var response = (from sb in subModuleResponse.Entities
                                join mm in moduleResponse.Entities
                                on sb.ModuleId equals mm.Id
                                select new SubModuleVm
                                {
                                    SubModuleId = sb.Id,
                                    SubModuleName = sb.SubModuleName,
                                    //Controller = sb.ControllerName,
                                    //Action = sb.ActionName,
                                    Icon = sb.SubModuleIcon,
                                    ModuleName = mm.ModuleName,
                                    ModuleId=mm.Id

                                }).ToList();

                return PartialView(ViewHelper.GetViewPathDetails("SubModule", "SubModuleList"), response);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(SubModuleMaster)} action name {nameof(GetSubModuleDetails)} exception is {ex.Message}";

                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }

        [HttpGet]
        public async Task<IActionResult> CreateSubModule(int id)
        {
            try
            {
                await PopulateViewBag();
                if (id == 0)
                {
                    return await Task.Run(() => PartialView(ViewHelper.GetViewPathDetails("SubModule", "SubModuleCreate")));
                }
                    var response = await _ISubModuleRepository.GetAllEntities(x => x.Id == id);
                    if (response.ResponseStatus == Core.Entities.Common.ResponseStatus.Success)
                {
                    return PartialView(ViewHelper.GetViewPathDetails("SubModule", "SubModuleCreate"), response.Entities.First());
                }
                return RedirectToAction("Error", "Home");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(SubModuleMaster)} action name {nameof(CreateSubModule)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpSertSubModule(SubModuleMaster model)
        {
            try
            {
                model.CreatedDate = DateTime.Now;

                if (model.Id == 0)
                {
                    var response = await _ISubModuleRepository.CreateEntity(model);
                    return Json(new DBResponseHelper<SubModuleMaster, int>().GetDBResponseHelper(response).message);
                }

                var updateResponse = await _ISubModuleRepository.UpdateEntity(model);
                return Json(new DBResponseHelper<SubModuleMaster, int>().GetDBResponseHelper(updateResponse).message);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(SubModuleMaster)} action name {nameof(UpSertSubModule)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }

        public async Task<IActionResult> DeleteSubModule(int id)
        {
            try
            {
                var deleteModelResponse = await _ISubModuleRepository.GetAllEntityById(x => x.Id == id);
                if (deleteModelResponse.ResponseStatus == Core.Entities.Common.ResponseStatus.Success)
                {
                    deleteModelResponse.Entity.IsActive = false;
                    deleteModelResponse.Entity.IsDeleted = true;

                    var deleteResponse = await _ISubModuleRepository.DeleteEntity(deleteModelResponse.Entity);

                    return Json(new DBResponseHelper<SubModuleMaster, int>().GetDBResponseHelper(deleteResponse));
                }

            return Json($"The Role Id {id} you have passed is not valid !!!");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(SubModuleMaster)} action name {nameof(DeleteSubModule)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }
        public async Task<IActionResult> GetSubModule(int Id)
        {
            try
            {
                var subModuleResponse = await _ISubModuleRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var moduleResponse = await _IModuleMasterRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

            var response = (from sb in subModuleResponse.Entities
                            join mm in moduleResponse.Entities
                            on sb.ModuleId equals mm.Id
                            where mm.Id==Id
                            select new SubModuleVm
                            {
                                SubModuleId = sb.Id,
                                SubModuleName = sb.SubModuleName,
                                //Controller = sb.ControllerName,
                                //Action = sb.ActionName,
                                Icon = sb.SubModuleIcon,
                                ModuleName = mm.ModuleName,
                                ModuleId = mm.Id

                            }).ToList();

            return Json(response);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(SubModuleMaster)} action name {nameof(GetSubModule)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }


        #region PrivateMethod
        private async Task PopulateViewBag()
        {
            var moduleResponse = await _IModuleMasterRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            ViewBag.Module = moduleResponse.Entities;
        }
        #endregion
    }
}
