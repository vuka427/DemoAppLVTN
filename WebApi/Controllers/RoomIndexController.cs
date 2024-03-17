using Application.Interface.IDomainServices;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Model.Branch;
using WebApi.Model;
using WebApi.Model.RoomIndex;
using Application.Interface.ApplicationServices;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoomIndexController : Controller
    {
        private readonly IBranchService _branchService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILandlordService _landlordService;
        private readonly IMapper _mapper;
        private readonly IBoundaryService _boundaryService;
        private readonly IRoomService _roomService;

        public RoomIndexController(IBranchService branchService, UserManager<AppUser> userManager, ILandlordService landlordService, IMapper mapper, IBoundaryService boundaryService, IRoomService roomService)
        {
            _branchService=branchService;
            _userManager=userManager;
            _landlordService=landlordService;
            _mapper=mapper;
            _boundaryService=boundaryService;
            _roomService=roomService;
        }

        [HttpGet]
        [Route("allroomindex")]
        public async Task<IActionResult> GetAllRoomIndex(int? month, int? year)
        {
            int filteredResultsCount;
            int totalResultsCount;

            var Identity = HttpContext.User;
            string CurrentUserId = "";
            if (Identity.HasClaim(c => c.Type == "userid"))
            {
                CurrentUserId = Identity.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString();
            }

            if (string.IsNullOrEmpty(CurrentUserId))
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Không tìm thấy user id" });
            }
            var landlord = _landlordService.GetLandlordByUserId(CurrentUserId);
            if (landlord == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Không tìm thấy user" });
            }

            try
            {
                var branches = _branchService.GetBranchWithRoomIndex(landlord.Id);
                foreach (var branch in branches)
                {
                    string ad = _boundaryService.GetAddress(branch.Province, branch.District, branch.Wards);
                    branch.Address =  branch.Address +", "+ ad;

                }

                var Dataresult = _mapper.Map<List<BranchForIndexModel>>(branches);


				foreach (var branchItem in Dataresult)
				{
                    foreach (var areaItem in branchItem.Areas)
                    {
                        foreach (var room in areaItem.Rooms)
                        {
                           
                        }
                    }
				}

				return Ok(Dataresult);
            }
            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Lỗi không tìm thấy nhà trọ!" });
            }






        }
    }
}
