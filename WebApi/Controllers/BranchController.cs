using Application.Interface.ApplicationServices;
using Application.Interface.IDomainServices;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using WebApi.Model;
using WebApi.Model.Branch;
using WebApi.Model.JQDataTable;
using WebApi.Model.Room;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BranchController : Controller
    {

        private readonly IBranchService _branchService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILandlordService _landlordService;
        private readonly IMapper _mapper;
        private readonly IBoundaryService _boundaryService;

        public BranchController(IBranchService branchService, UserManager<AppUser> userManager, ILandlordService landlordService, IMapper mapper, IBoundaryService boundaryService)
        {
            _branchService=branchService;
            _userManager=userManager;
            _landlordService=landlordService;
            _mapper=mapper;
            _boundaryService=boundaryService;
        }

        [HttpPost]
        [Route("branchesfordatatable")]
        public async Task<IActionResult> GetBrachesForDataTable([FromBody] DatatableParam param)
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
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { Status = "Error", Message = "Can find user!" });
            }
            var landlord = _landlordService.GetLandlordByUserId(CurrentUserId);
            if (landlord == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { Status = "Error", Message = "Can find user!" });
            }
            try
            {
                var branches = _branchService.GetBranches(landlord.Id);
              
                totalResultsCount = branches.Count();

                var result = branches.Skip(param.start).Take(param.length).ToList();

                filteredResultsCount = result.Count();

                foreach (var branch in result)
                {
                    string ad = _boundaryService.GetAddress(branch.Province, branch.District, branch.Wards);
                    branch.Address =  branch.Address +", "+ ad;
                }

                var Dataresult = _mapper.Map<List<BranchModel>>(result);

                return Json(new
                {
                    // this is what datatables wants sending back

                    draw = param.draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    data = Dataresult
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { Status = "Error", Message = "Can get braches!" });
            }


        }


        [HttpGet]
        [Route("allroom")]
        public async Task<IActionResult> GetAllRoom(string roomStatus)
        {
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
                var branches = _branchService.GetBranchWithRoom(landlord.Id, roomStatus);
                foreach(var branch in branches)
                {
                    string ad = _boundaryService.GetAddress(branch.Province,branch.District,branch.Wards);
                    branch.Address =  branch.Address +", "+ ad;
                }

                var Dataresult = _mapper.Map<List<BranchModel>>(branches);


                return Ok(Dataresult);
            }
            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Lỗi không tìm thấy nhà trọ!" });
            }
        }

		[HttpGet]
		[Route("all")]
		public async Task<IActionResult> GetAll()
		{
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
				var branches = _branchService.GetBranches(landlord.Id);
				foreach (var branch in branches)
				{
					string ad = _boundaryService.GetAddress(branch.Province, branch.District, branch.Wards);
					branch.Address =  branch.Address +", "+ ad;
				}

				var Dataresult = _mapper.Map<List<BranchModel>>(branches);


				return Ok(Dataresult);
			}
			catch
			{
				return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Lỗi không tìm thấy nhà trọ!" });
			}


		}


		[HttpPost]
        [Route("add")]
        public IActionResult CreateBrach(BranchCreateModel model)
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

            try
            {
                var branch = _mapper.Map<Branch>(model);
                _branchService.CreateBranch(landlordId, branch);
                _branchService.SaveChanges();


                return Ok();
            }
            catch
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { Status = "Error", Message = "can't create branch!" });
            }
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult DeleteBranch([FromQuery] int branchid)
        {
            var Identity = HttpContext.User;
            string CurrentLandlordId = "";
            int landlordId = 0;
            if (Identity.HasClaim(c => c.Type == "userid"))
            {

                CurrentLandlordId = Identity.Claims.FirstOrDefault(c => c.Type == "landlordid").Value.ToString();
            }

            var result = int.TryParse(CurrentLandlordId, out landlordId);
            if (string.IsNullOrEmpty(CurrentLandlordId) && !result)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Tìm thấy nhà trọ!" });
            }
            try
            {
                _branchService.DeleteBranch(landlordId, branchid);
                _branchService.SaveChanges();
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Không thể xóa nhà trọ!" });
            }

        }

        [HttpPost]
        [Route("area/add")]
        public IActionResult CreateArea(AreaCreateModel model)
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
            try
            {
                var CreateResult = _branchService.CreateArea(landlordId, model.BranchId, new Area() { AreaName = model.AreaName, Description= model.Description });
                if (!CreateResult.Success) { return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "can't create branch!" }); }
                _branchService.SaveChanges();

                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "can't create branch!" });
            }
        }

        [HttpPut]
        [Route("area/edit")]
        public IActionResult UpdateArea(AreaEditModel model)
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


            try
            {
                var CreateResult = _branchService.UpdateArea(landlordId, model.BranchId, new Area() { Id=model.AreaId, AreaName = model.AreaName, Description= model.Description });
                if (!CreateResult.Success) { return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "can't update branch!" }); }
                _branchService.SaveChanges();

                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "can't create branch!" });
            }
        }


        [HttpGet]
        [Route("areas")]
        public async Task<IActionResult> GetAreas(int branchId)
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
                var branches = _branchService.GetBranchById(landlord.Id, branchId);

                


                var Dataresult = _mapper.Map<BranchModel>(branches);

                return Ok(Dataresult);
            }
            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Lỗi không tìm thấy nhà trọ!" });
            }

        }

        [HttpDelete]
        [Route("area/delete")]
        public IActionResult DeleteArea([FromQuery] int areaid)
        {
            var Identity = HttpContext.User;
            string CurrentLandlordId = "";
            int landlordId = 0;
            if (Identity.HasClaim(c => c.Type == "userid"))
            {

                CurrentLandlordId = Identity.Claims.FirstOrDefault(c => c.Type == "landlordid").Value.ToString();
            }

            var result = int.TryParse(CurrentLandlordId, out landlordId);
            if (string.IsNullOrEmpty(CurrentLandlordId) && !result)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Tìm thấy nhà trọ!" });
            }
            try
            {
                var deleteResult = _branchService.DeleteArea(landlordId,areaid);
                _branchService.SaveChanges();
                if(!deleteResult.Success) { return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = deleteResult.Message }); }
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Không thể xóa khu vực!" });
            }

        }

        [HttpGet]
        [Route("area")]
        public async Task<IActionResult> GetArea(int areaId)
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
                var area = _branchService.GetAreaById(landlord.Id, areaId);

                var Dataresult = _mapper.Map<AreaModel>(area);

                return Ok(Dataresult);
            }
            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Lỗi không tìm thấy khu vực!" });
            }

        }




    }

}
