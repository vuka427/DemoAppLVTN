using Application.Interface.ApplicationServices;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppStatisticController : Controller
    {
        
        private readonly IMapper _mapper;
        private readonly IStatisticService _statisticService;

        public AppStatisticController(IMapper mapper, IStatisticService statisticService)
        {
            _mapper=mapper;
            _statisticService=statisticService;
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
        }
    }
}
