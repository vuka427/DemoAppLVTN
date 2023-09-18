using Application.Interface.IDomainServices;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Model;
using WebApi.Model.User;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILandlordService _landlordService;
        private readonly ITenantService _tenantService;
        private readonly IMapper _mapper;

        public UserController(UserManager<AppUser> userManager, ILandlordService landlordService, ITenantService tenantService, IMapper mapper)
        {
            _userManager = userManager;
            _landlordService = landlordService;
            _tenantService = tenantService;
            _mapper = mapper;
        }

        [HttpGet, Authorize]
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
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { Status = "Error", Message = "Can find user profile!" });
            }

            var CurrentUser = await _userManager.FindByIdAsync(userid);
            if(CurrentUser == null) return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { Status = "Error", Message = "Can find user profile!" });

            UserProfileModel userProfileModel = null;

            if (CurrentUser.UserType == UserType.Tenant)
            {
                var tenant = _tenantService.GetTenantByUserId(CurrentUser.Id);
                if (tenant != null)
                {
                    userProfileModel = _mapper.Map<UserProfileModel>(tenant);
                }
            }
            else if (CurrentUser.UserType == UserType.Landlord)
            {
                var landlord = _landlordService.GetLandlordByUserId(CurrentUser.Id);
                if (landlord != null)
                {
                    userProfileModel = _mapper.Map<UserProfileModel>(landlord);
                }
            }
            else
            {
                //admin handle
            }

            if (userProfileModel == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { Status = "Error", Message = "Can find user profile!" });
            }

            userProfileModel.Email = CurrentUser .Email;
            userProfileModel.UserName = CurrentUser.UserName;


            return Ok(userProfileModel);
        }
    }
}
