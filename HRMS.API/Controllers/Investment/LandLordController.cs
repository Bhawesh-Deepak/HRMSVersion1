using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Investment;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.API.Controllers.Investment
{
    [Route("api/HRMS/[controller]/[action]")]
    [ApiController]
    public class LandLordController : ControllerBase
    {
        private readonly IGenericRepository<LandLordDetail, int> _ILandLordDetailRepository;
        public LandLordController(IGenericRepository<LandLordDetail, int> landLordDetailRepository)
        {
            _ILandLordDetailRepository = landLordDetailRepository;
        }
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetLandLordDetail(string EmpCode, int FinancialYear)
        {
            try
            {
            
                var models = await _ILandLordDetailRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted && x.EmpCode.Trim() == EmpCode.Trim() && x.FinancialYear == FinancialYear);
                return Ok(models.Entities.FirstOrDefault());
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
                return await Task.Run(() => BadRequest(new Helpers.ResponseEntityList<LandLordDetail>
                    (System.Net.HttpStatusCode.InternalServerError, ResponseStatus.DataBaseException.ToString(), null)
                    .GetResponseEntityList()));
            }
        }
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> PostLandLordDetail(LandLordDetail landLord)
        {
            try
            {
                var updatemodel = await _ILandLordDetailRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted && x.EmpCode.Trim() ==landLord.EmpCode.Trim() && x.FinancialYear ==landLord.FinancialYear);
                updatemodel.Entities.ToList().ForEach(data => {
                    data.IsActive = false;
                    data.IsDeleted = true;
                });
 
                var updateresponse = await _ILandLordDetailRepository.UpdateMultipleEntity(updatemodel.Entities.ToArray());

                var response = await _ILandLordDetailRepository.CreateEntity(landLord);
                return Ok(response.ResponseStatus);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
                return await Task.Run(() => BadRequest(new Helpers.ResponseEntityList<LandLordDetail>
                    (System.Net.HttpStatusCode.InternalServerError, ResponseStatus.DataBaseException.ToString(), null)
                    .GetResponseEntityList()));
            }
        }

    }
}
