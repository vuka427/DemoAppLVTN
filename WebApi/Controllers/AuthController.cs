using Azure;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.AppData;
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

        public AuthController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
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
                    new Claim("fullname",  "nguyễn văn vũ"),
                    new Claim("email",  "vu@gmail.com"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
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
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { Status = "Error", Message = "User already exists!" });
            AppUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                UpdatedBy = "Admin",
                UpdatedDate = DateTime.Now
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { Status = "Error", Message = "User creation failed! Please check user details and try again."+ result.ToString() });
            return Ok(new ResponseMessage { Status = "Success", Message = "User created successfully!" });
        }
        [HttpPost]
        [Route("registerlandlord")]
        public async Task<IActionResult> RegisterLandlord([FromBody] RegisterLandlordModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { Status = "Error", Message = "User already exists!" });
            AppUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                UpdatedBy = "Admin",
                UpdatedDate = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { Status = "Error", Message = "User creation failed! Please check user details and try again." + result.ToString() });

            return Ok(new ResponseMessage { Status = "Success", Message = "User created successfully!" });
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
