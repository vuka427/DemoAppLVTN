using Application.Interface.ApplicationServices;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using WebApi.Model;
using WebApi.Model.User;

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
        private readonly IMapper _mapper;

        public UserController(UserManager<AppUser> userManager, ILandlordService landlordService, ITenantService tenantService, IMapper mapper)
        {
            _userManager = userManager;
            _landlordService = landlordService;
            _tenantService = tenantService;
            _mapper = mapper;
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
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { Status = "Error", Message = "Can find user profile!" });
            }

            var CurrentUser = await _userManager.FindByIdAsync(userid);
            if (CurrentUser == null) return StatusCode(
                                        StatusCodes.Status500InternalServerError,
                                        new ResponseMessage { Status = "Error", Message = "Can find user profile!" }
                                    );

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

            userProfileModel.Email = CurrentUser.Email;
            userProfileModel.UserName = CurrentUser.UserName;
            userProfileModel.AvatarUrl = String.IsNullOrEmpty(userProfileModel.AvatarUrl)?"": "/contents/avatar/" + userProfileModel.AvatarUrl;

            return Ok(userProfileModel);
        }

        [HttpPost]
        [Route("updateuserprofile")]
        public async Task<IActionResult> UpdateUserProfile(UserProfileModel model)
        {
            var Identity = HttpContext.User;
            string CurrentUserId = "";
            string userType = "";
            string UserName = "";
            if (Identity.HasClaim(c => c.Type == "userid") && Identity.HasClaim(c => c.Type == "usertype") && Identity.HasClaim(c => c.Type == "username"))
            {
                CurrentUserId = Identity.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString();
                userType = Identity.Claims.FirstOrDefault(c => c.Type == "usertype").Value.ToString();
                UserName = Identity.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            }

            if (string.IsNullOrEmpty(UserName) || CurrentUserId != model.UserId)
            {
                return StatusCode(
                            StatusCodes.Status500InternalServerError,
                            new ResponseMessage { Status = "Error", Message = "Can find user profile!" }
                   );
            }

            if (userType == "landlord")
            {
                var landlord = _landlordService.GetLandlordByUserId(CurrentUserId);
                if (landlord != null)
                {
                    landlord.FullName = model.FullName;
                    landlord.DateOfBirth = model.DateOfBirth;
                    landlord.Phone = model.Phone;
                    landlord.Ccccd = model.Ccccd;
                    landlord.Address = model.Address;
                    landlord.UpdatedDate = DateTime.Now;
                    
                    _landlordService.UpdateUser(landlord);
                    _landlordService.SaveChanges();

                    return Ok();
                }


            }
            else if (userType == "tenant")
            {
                var tenant = _tenantService.GetTenantByUserId(CurrentUserId);
                if (tenant != null)
                {

                    tenant.FullName = model.FullName;
                    tenant.DateOfBirth = model.DateOfBirth;
                    tenant.Phone = model.Phone;
                    tenant.Ccccd = model.Ccccd;
                    tenant.Address = model.Address;
                    tenant.UpdatedDate = DateTime.Now;

                   _tenantService.UpdateUser(tenant);
                   _tenantService.SaveChanges();


                    return Ok();
                }

            }

            return StatusCode(
                            StatusCodes.Status500InternalServerError,
                            new ResponseMessage { Status = "Error", Message = "Can find user profile!" }
                        );


        }

        [HttpPost]
        [Route("uploadavatar")]
        public async Task<IActionResult> UploadAvatar(string userid)
        {
            var httpRequestttt = HttpContext.Request.Form.Files;
            try 
            {
                var httpRequest = HttpContext.Request;

                var file = httpRequest.Form.Files[0];
                if (file == null) { return BadRequest(); }
                
                var fileNameRandom = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(file.FileName);//tên file random + extension file upload

                var filePath = Path.Combine("Uploads", "avatar", fileNameRandom); //đường đẫn đến file

                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(filestream); // copy file f vào filestream
                }

                var Identity = HttpContext.User;
                string CurrentUserId = "";
                string userType = "";
                if (Identity.HasClaim(c => c.Type == "userid") && Identity.HasClaim(c => c.Type == "usertype") && Identity.HasClaim(c => c.Type == "username"))
                {
                    CurrentUserId = Identity.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString();
                    userType = Identity.Claims.FirstOrDefault(c => c.Type == "usertype").Value.ToString();
                  
                }

                if (string.IsNullOrEmpty(CurrentUserId) || CurrentUserId != userid)
                {
                    return StatusCode(
                                StatusCodes.Status500InternalServerError,
                                new ResponseMessage { Status = "Error", Message = "Can find user profile!" }
                       );
                }

                if (userType == "landlord")
                {
                    var landlord = _landlordService.GetLandlordByUserId(CurrentUserId);
                    if (landlord != null)
                    {
                        landlord.AvatarUrl = fileNameRandom; 
                        _landlordService.UpdateUser(landlord);
                        _landlordService.SaveChanges();


                        return Ok();
                    }


                }
                else if (userType == "tenant")
                {
                    var tenant = _tenantService.GetTenantByUserId(CurrentUserId);
                    if (tenant != null)
                    {

                        tenant.AvatarUrl = fileNameRandom;
                        _tenantService.UpdateUser(tenant);
                        _tenantService.SaveChanges();

                        return Ok();
                    }

                }




                return Ok() ;
                
            }
            catch (Exception ex)
            {
                return StatusCode(
                                StatusCodes.Status500InternalServerError,
                                new ResponseMessage { Status = "Error", Message = "Can upload avatar !" }
                       );

            }
        }





        //----------
    }
}
