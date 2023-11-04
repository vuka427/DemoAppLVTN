using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Model.Contract;
using WebApi.Model.JQDataTable;
using WebApi.Model;
using Application.Interface.ApplicationServices;
using Application.Interface.IDomainServices;
using Application.Interface;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Application.Implementation.ApplicationServices;
using WebApi.Model.Invoice;
using WebApi.Model.MemberModel;
using WebApi.Model.Member;
using AutoMapper.Execution;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly IBranchService _branchService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILandlordService _landlordService;
        private readonly IMapper _mapper;
        private readonly IContractService _contractService;
        private readonly IBoundaryService _boundaryService;
        private readonly IEmailService _mailService;

        public CustomerController(IBranchService branchService, UserManager<AppUser> userManager, ILandlordService landlordService, IMapper mapper, IContractService contractService, IBoundaryService boundaryService, IEmailService mailService)
        {
            _branchService=branchService;
            _userManager=userManager;
            _landlordService=landlordService;
            _mapper=mapper;
            _contractService=contractService;
            _boundaryService=boundaryService;
            _mailService=mailService;
        }

        [HttpPost]
        [Route("invoicefordatatable")]
        public async Task<IActionResult> GetInvoiceForDataTable([FromBody] DatatableParam param, [FromQuery] string status, [FromQuery] int month, [FromQuery] int year, [FromQuery] int branchid)
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
               
                 var members =  _mapper.Map<List<MemberForDataTableModel>>(_contractService.GetMemberOfDataTable(landlord.Id, status,branchid));

                int i = 1;
                members.ForEach(m => { m.Index = i; i++; }) ; 

                if (!string.IsNullOrEmpty(param.search.value))
                {
                    members = members.Where(m => m.FullName.Contains(param.search.value) ||
                                                 m.Cccd.Contains(param.search.value) ||
                                                 m.RoomName.Contains(param.search.value)).ToList();
                }

                totalResultsCount = members.Count();

                var result = members.Skip(param.start).Take(param.length).ToList();

                filteredResultsCount = result.Count();

                

                return Json(new
                {
                    // this is what datatables wants sending back

                    draw = param.draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    data = result
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { Status = "Error", Message = "Can get invoice!" });
            }
        }


        [HttpPost]
        [Route("create")]
        public IActionResult CreateMember(MemberCreateModel model, int roomid)
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

                var member = _mapper.Map<Domain.Entities.Member>(model);

                var createResult = _contractService.CreateMember(landlordId, roomid, member);

                _contractService.SaveChanges();

                if (!createResult.Success)
                {

                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "can't create member!" });
                }
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "can't create memberr!" });
            }
        }

        [HttpPut]
        [Route("leave")]
        public IActionResult MemberLeave( int roomid,int memberid)
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
                var leaveResult = _contractService.MemberLeave(landlordId, memberid, true);
                if(!leaveResult.Success) { return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "can't update member!" }); }
                _contractService.SaveChanges();

                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "can't update member!" });
            }
        }


    }
}
