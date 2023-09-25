using Application.Interface.IDomainServices;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using WebApi.Model;
using WebApi.Model.Branch;
using WebApi.Model.JQDataTable;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
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
        public async Task<IActionResult> GetBrachesForDataTable( [FromBody] DatatableParam param)
        {
            int filteredResultsCount ;
            int totalResultsCount;


            var Identity = HttpContext.User;
            string CurrentUserId = "";
            if (Identity.HasClaim(c => c.Type == "userid"))
            {
                CurrentUserId = Identity.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString();
            }
            
            if (string.IsNullOrEmpty(CurrentUserId) )
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



        [HttpPost]
        [Route("add")]
        public IActionResult CreateBrach(BranchCreateModel branch)
        {
            var a = User;
            var address = _boundaryService.GetAddress(branch.Province, branch.District, branch.Wards);

            return Ok(); 
        }



    }
}
