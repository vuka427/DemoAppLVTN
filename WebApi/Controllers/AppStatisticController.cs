using Application.Implementation.ApplicationServices;
using Application.Interface.ApplicationServices;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Model;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppStatisticController : Controller
    {
        
        private readonly IMapper _mapper;
        private readonly IStatisticService _statisticService;
        private readonly ITenantService _tenantService;

        public AppStatisticController(IMapper mapper, IStatisticService statisticService, ITenantService tenantService)
        {
            _mapper=mapper;
            _statisticService=statisticService;
            _tenantService=tenantService;
        }

        [HttpGet]
        [Route("general")]
        public IActionResult GetStatistic()
        {
            var Identity = HttpContext.User;
            string CurrentUserId = "";
            string CurrentLandlordId = "";
            int landlordId = 0;
            if (Identity.HasClaim(c => c.Type == "userid"))
            {
                CurrentUserId = Identity.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString();
                CurrentLandlordId = Identity.Claims.FirstOrDefault(c => c.Type == "landlordid").Value.ToString();
            }
            var result = int.TryParse(CurrentLandlordId, out landlordId);
            if (string.IsNullOrEmpty(CurrentUserId) && string.IsNullOrEmpty(CurrentLandlordId) && !result)
            {
                return Unauthorized();
            }


            var statistic = _statisticService.GetGeneralStatistic(landlordId);



            return Ok(statistic);
        }

        [HttpGet]
        [Route("branch")]
        public IActionResult GetStatisticForBranh(int year, int branchid)
        {
            try
            {
                var Identity = HttpContext.User;
                string CurrentUserId = "";
                string CurrentLandlordId = "";
                int landlordId = 0;

                if (Identity.HasClaim(c => c.Type == "userid"))
                {
                    CurrentUserId = Identity.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString();
                    CurrentLandlordId = Identity.Claims.FirstOrDefault(c => c.Type == "landlordid").Value.ToString();
                }
                var result = int.TryParse(CurrentLandlordId, out landlordId);
                if (string.IsNullOrEmpty(CurrentUserId) && string.IsNullOrEmpty(CurrentLandlordId) && !result)
                {
                    return Unauthorized();
                }


                var statistic = _statisticService.GetBranchStatistic(landlordId,year,branchid);



                return Ok(statistic);
            }catch
            {
                return Ok();
            }
            
        }

        [HttpGet]
        [Route("tenant")]
        public IActionResult GetStatisticForTenant(int year, int contractid)
        {
            try
            {
                var Identity = HttpContext.User;
                string CurrentUserId = "";
                if (Identity.HasClaim(c => c.Type == "userid"))
                {
                    CurrentUserId = Identity.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString();
                }

                if (string.IsNullOrEmpty(CurrentUserId))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Can find user!" });
                }
                var Tenant = _tenantService.GetTenantByUserId(CurrentUserId);
                if (Tenant == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Can find user!" });
                }


                var statistic = _statisticService.GetTenatStatistic(Tenant.Id, year, contractid);



                return Ok(statistic);
            }
            catch
            {
                return Ok();
            }

        }



    }
}
