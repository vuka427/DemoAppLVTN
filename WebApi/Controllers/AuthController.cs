using Application.Interface.IDomainServices;
using Azure;
using Domain.Entities;
using Domain.Enums;
using Domain.IRepositorys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.AppData;
using WebApi.Common;
using WebApi.Model;
using WebApi.Model.Account;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILandlordService _landlordService;
        private readonly ITenantService _tenantService;

        public AuthController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IConfiguration configuration, ILandlordService landlordService, ITenantService tenantService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _landlordService = landlordService;
            _tenantService = tenantService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login( LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("username",  user.UserName),
                    new Claim("userid",  user.Id),
                    new Claim("email", "vu@gmail.com"),

                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                if (user.UserType == UserType.Tenant)
                {
                    var tenant = _tenantService.GetTenantByUserId(user.Id);
                    if(tenant != null)
                    {
                        authClaims.Add(new Claim("fullname", tenant.FullName));
                        if(!String.IsNullOrEmpty( tenant.AvatarUrl))
                        authClaims.Add(new Claim("avatar", "/contents/avatar/" +tenant.AvatarUrl));
                        authClaims.Add(new Claim("tenantid", tenant.Id.ToString()));
                    }
                    authClaims.Add(new Claim("usertype", "tenant"));

                }
                else if(user.UserType == UserType.Landlord)
                {
                    var landlord = _landlordService.GetLandlordByUserId(user.Id);
                    if (landlord != null)
                    {
                        authClaims.Add(new Claim("fullname",  landlord.FullName));
                        if (!String.IsNullOrEmpty(landlord.AvatarUrl))
                            authClaims.Add(new Claim("avatar", "/contents/avatar/" +landlord.AvatarUrl));
                        authClaims.Add(new Claim("landlordid",landlord.Id.ToString()));
                    }

                    authClaims.Add(new Claim("usertype", "landlord"));

                }
                else
                {

                }

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var token = GetToken(authClaims);
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return StatusCode(StatusCodes.Status401Unauthorized, new ResponseMessage { Status = "Error", Message = "Đăng nhập thất bại!" });

        }

        [HttpPost]
        [Route("registertenant")]
        public async Task<IActionResult> RegisterTenant([FromBody] RegisterTenantModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Tên tài khoản đã được sử dụng!" });
            AppUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                UserType = UserType.Tenant,
                CreatedBy = model.Username,
                CreatedDate = DateTime.Now,
                UpdatedBy = model.Username,
                UpdatedDate = DateTime.Now
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ResponseMessage
                    {
                        Status = "Error",
                        Message = "Không thể tạo người dùng! vui lòng kiểm tra lại thông tin người dùng!" + result.ToString()
                    }
                 );

            Tenant tenant = new Tenant() {
                UserId = user.Id,
                FullName = model.FullName ?? "no name",
                DateOfBirth = DateTime.Now,
                Address = "",
                Phone = "",
                Ccccd ="",
                CreatedBy = model.Username,
                CreatedDate = DateTime.Now,
                UpdatedBy = model.Username,
                UpdatedDate = DateTime.Now
            };
            try
            {
                _tenantService.CreateNewTenant(tenant);
                _tenantService.SaveChanges();
            }
            catch
            {
                return StatusCode(
                    StatusCodes.Status400BadRequest,
                    new ResponseMessage
                    {
                        Status = "Error",
                        Message = "Không thể tạo người dùng! vui lòng kiểm tra lại thông tin người dùng!" + result.ToString()
                    }
                 );
            }



            return Ok(new ResponseMessage { Status = "Success", Message = "Tài khoản được tạo thành công!" });
        }


        [HttpPost]
        [Route("registerlandlord")]
        public async Task<IActionResult> RegisterLandlord([FromBody] RegisterLandlordModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Tên tài khoản đã được sử dụng!" });
            AppUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserType = UserType.Landlord,
                UserName = model.Username,
                CreatedBy = model.Username,
                CreatedDate = DateTime.Now,
                UpdatedBy = model.Username,
                UpdatedDate = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, model.Password); 
            if (!result.Succeeded)
                return StatusCode(
                    StatusCodes.Status400BadRequest, 
                    new ResponseMessage { 
                        Status = "Error", 
                        Message = "Không thể tạo người dùng! vui lòng kiểm tra lại thông tin người dùng!" + result.ToString() 
                    }
                );

            Landlord landlord = new Landlord() { 
                UserId = user.Id,
                FullName = model.FullName,
                DateOfBirth = model.DateOfBirth.Value,
                Address = model.Address,
                Phone = model.Phone,
                Ccccd = model.Cccd,
                CreatedBy = model.Username,
                CreatedDate = DateTime.Now,
                UpdatedBy = model.Username,
                UpdatedDate = DateTime.Now
            };

            try
            {
                _landlordService.CreateNewLandlord(landlord);
                _landlordService.SaveChanges();
            }
            catch
            {
                return StatusCode(
                    StatusCodes.Status400BadRequest, 
                    new ResponseMessage { 
                        Status = "Error", 
                        Message = "Không thể tạo người dùng! vui lòng kiểm tra lại thông tin người dùng!" + result.ToString() 
                    }
                );
            }

            return Ok(new ResponseMessage { Status = "Success", Message = "Tài khoản được tạo thành công!" });
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { Status = "Error", Message = "User already exists!" });
            AppUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                CreatedBy ="Admin",
                CreatedDate = DateTime.Now,
                UpdatedBy = "Admin",
                UpdatedDate = DateTime.Now
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            if (!await _roleManager.RoleExistsAsync(RoleName.Admin))
                await _roleManager.CreateAsync(new AppRole() { 
                    Name = RoleName.Admin,
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now,
                    UpdatedBy = "Admin",
                    UpdatedDate = DateTime.Now
                });
            if (!await _roleManager.RoleExistsAsync(RoleName.User))
                await _roleManager.CreateAsync(new AppRole() { 
                    Name = RoleName.User ,
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now,
                    UpdatedBy = "Admin",
                    UpdatedDate = DateTime.Now
                });
            if (await _roleManager.RoleExistsAsync(RoleName.Admin))
            {
                await _userManager.AddToRoleAsync(user, RoleName.Admin);
            }
            if (await _roleManager.RoleExistsAsync(RoleName.Admin))
            {
                await _userManager.AddToRoleAsync(user, RoleName.User);
            }
            return Ok(new ResponseMessage { Status = "Success", Message = "User created successfully!" });
        }


        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }



    }
}
