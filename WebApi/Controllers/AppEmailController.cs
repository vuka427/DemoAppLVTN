using Application.Interface;
using Application.Interface.ApplicationServices;
using Application.Interface.IDomainServices;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Model.Invoice;
using WebApi.Model.JQDataTable;
using WebApi.Model;
using WebApi.Model.Email;
using AutoMapper.Execution;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppEmailController : Controller
    {
        private readonly IBranchService _branchService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILandlordService _landlordService;
        private readonly IMapper _mapper;
        private readonly IContractService _contractService;
        private readonly ITenantService _tenantService;
        private readonly IInvoiceService _invoiceService;
        private readonly IEmailService _emailService;

        public AppEmailController(IBranchService branchService, UserManager<AppUser> userManager, ILandlordService landlordService, IMapper mapper, IContractService contractService, ITenantService tenantService, IInvoiceService invoiceService, IEmailService emailService)
        {
            _branchService=branchService;
            _userManager=userManager;
            _landlordService=landlordService;
            _mapper=mapper;
            _contractService=contractService;
            _tenantService=tenantService;
            _invoiceService=invoiceService;
            _emailService=emailService;
        }


        [HttpPost]
        [Route("all")]
        public async Task<IActionResult> GetEmailForDataTable([FromBody] DatatableParam param, [FromQuery] int branchid)
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

           

                var sendEmail = _emailService.GetAllEmail(landlord.Id, branchid).OrderByDescending(i => i.CreatedDate).ToList();


                if (!string.IsNullOrEmpty(param.search.value))
                {
                sendEmail = sendEmail.Where(m => m.ReceiverName.Contains(param.search.value) ||
                                                 m.RoomName.Contains(param.search.value)).ToList();
                }

                var Dataresult = _mapper.Map<List<EmailSendModel>>(sendEmail);

                int i = 1;
                Dataresult.ForEach(m => { m.Index = i; i++; });

                totalResultsCount = Dataresult.Count();

                var result = Dataresult.Skip(param.start).Take(param.length).ToList();

                filteredResultsCount = Dataresult.Count();


            try
            {
                return Json(new
                {
                    // this is what datatables wants sending back

                    draw = param.draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = totalResultsCount,
                    data = result
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { Status = "Error", Message = "Can get invoice!" });
            }

        }




    }
}
