using Microsoft.AspNetCore.Mvc;
using WebApi.Model.Room;
using WebApi.Model;
using Microsoft.AspNetCore.Authorization;
using Application.Interface.IDomainServices;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoomController : Controller
    {
        private readonly IBranchService _branchService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILandlordService _landlordService;
        private readonly IMapper _mapper;
        private readonly IBoundaryService _boundaryService;
        private readonly IRoomService _roomService;

        public RoomController(IBranchService branchService, UserManager<AppUser> userManager, ILandlordService landlordService, IMapper mapper, IBoundaryService boundaryService, IRoomService roomService)
        {
            _branchService=branchService;
            _userManager=userManager;
            _landlordService=landlordService;
            _mapper=mapper;
            _boundaryService=boundaryService;
            _roomService=roomService;
        }

        [HttpPost]
        [Route("add")]
        public IActionResult CreateRoom(RoomCreateModel model)
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
            var room = _mapper.Map<Room>(model);
            

            try
            {
                _roomService.CreateRoom(landlordId,model.BranchId,model.AreaId, room);
                _roomService.SaveChanges();
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "can't create room!" });
            }
        }
    }
}
