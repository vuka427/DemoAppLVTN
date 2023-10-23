using Application.Interface.ApplicationServices;
using Application.Interface.IDomainServices;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Model.RoomIndex;
using WebApi.Model;
using WebApi.Model.Invoice;

namespace WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class InvoiceController : Controller
	{
		private readonly IBranchService _branchService;
		private readonly UserManager<AppUser> _userManager;
		private readonly ILandlordService _landlordService;
		private readonly IMapper _mapper;
		private readonly IBoundaryService _boundaryService;
		private readonly IRoomService _roomService;
		private readonly IInvoiceService _invoiceService;
		private readonly IContractService _contractService;

		public InvoiceController(IBranchService branchService, UserManager<AppUser> userManager, ILandlordService landlordService, IMapper mapper, IBoundaryService boundaryService, IRoomService roomService, IInvoiceService invoiceService, IContractService contractService)
		{
			_branchService=branchService;
			_userManager=userManager;
			_landlordService=landlordService;
			_mapper=mapper;
			_boundaryService=boundaryService;
			_roomService=roomService;
			_invoiceService=invoiceService;
			_contractService=contractService;
		}

		[HttpGet]
		[Route("info")]
		public async Task<IActionResult> GetInfoRoomInvoice(int roomid)
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

			
				var invoice = _invoiceService.GetInvoice(landlordId,roomid,DateTime.Now);

				var invoiceResult = _mapper.Map<InvoiceModel>(invoice);

				if (invoice != null) { return Ok(invoiceResult); }

				var contract = _contractService.GetContractByRoomId(landlordId,roomid);
				if(contract == null) { return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Lỗi không tìm thấy hợp đồng!" }); }
				var previousDate = DateTime.Now.AddMonths(-1);
				var previousInvoice = _invoiceService.GetInvoice(landlordId, roomid, previousDate);
				var branch = _branchService.GetBranchByRoomId(landlordId,roomid);

			try
			{	

				var newInvoice = new InvoiceModel() {
					Id=0,
					OldElectricNumber = previousInvoice!=null ? previousInvoice.OldElectricNumber : 0,
					OldWaterNumber = previousInvoice!= null ? previousInvoice.OldWaterNumber : 0,
					ElectricityCosts = contract.ElectricityCosts,
					WaterCosts=contract.WaterCosts,
					RentalPrice = contract.RentalPrice,
				
				};


				return Ok(newInvoice);
			}
			catch
			{
				return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Lỗi không tìm thấy nhà trọ!" });
			}
		}

	}
}
