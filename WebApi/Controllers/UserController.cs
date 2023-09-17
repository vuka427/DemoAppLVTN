using Application.Interface.IDomainServices;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Model;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILandlordService _landlordService;
        private readonly ITenantService _tenantService;

        public UserController(UserManager<AppUser> userManager, ILandlordService landlordService, ITenantService tenantService)
        {
            _userManager = userManager;
            _landlordService = landlordService;
            _tenantService = tenantService;
        }

        [HttpGet]
        [Route("getuserprofile")]

        public async Task<IActionResult> GetUserProfile(string userid)
        {
            var Identity = HttpContext.User;
            string CurrentUserId = "";
            if (Identity.HasClaim(c => c.Type == "userid"))
            {
                CurrentUserId = Identity.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString();
            }

            if (string.IsNullOrEmpty(CurrentUserId) || userid != CurrentUserId)
            {
                return Unauthorized();
            }
            var CurrentUser = await _userManager.FindByIdAsync(userid);
            if(CurrentUser == null) return Unauthorized();

            if (CurrentUser.UserType == UserType.Tenant)
            {
                var tenant = _tenantService.GetTenantByUserId(CurrentUser.Id);
                if (tenant != null)
                {

                    return Ok();
                }
                
            }
            else if (CurrentUser.UserType == UserType.Landlord)
            {
                var landlord = _landlordService.GetLandlordByUserId(CurrentUser.Id);
                if (landlord != null)
                {

                    return Ok();
                }

            }
            else
            {

            }



            return Ok(new ResponseMessage { Status = "Success", Message = CurrentUserId });
        }
    }
}
