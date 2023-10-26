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
using static System.Runtime.InteropServices.JavaScript.JSType;

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

			var contract = _contractService.GetContractByRoomId(landlordId,roomid);

			if(contract == null) { return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Lỗi không tìm thấy hợp đồng!" }); }

			if (invoice != null) 
			{	
				var invoiceResult = _mapper.Map<InvoiceModel>(invoice);


				invoiceResult.Month = DateTime.Now.Month.ToString();
				invoiceResult.Year = DateTime.Now.Year.ToString();
				invoiceResult.RentalPrice = contract.RentalPrice;
				invoiceResult.ElectricityCosts = contract.ElectricityCosts;
				invoiceResult.WaterCosts=contract.WaterCosts;
				return Ok(invoiceResult); 
			}

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
					Month = DateTime.Now.Month.ToString(),
					Year = DateTime.Now.Year.ToString(),
					ContractId = contract.Id,

				};

				if (branch.Services.Count>0)
				{

					 newInvoice.ServiceItems = _mapper.Map<List<ServiceItemModel>>(branch.Services);
				}



				return Ok(newInvoice);
			}
			catch
			{
				return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Lỗi không tìm thấy nhà trọ!" });
			}
		}

		[HttpPost]
		[Route("create")]
		public async Task<IActionResult> CreateRoomInvoice(InvoiceCreateModel model)
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

			var previousDate = DateTime.Now.AddMonths(-1);
			var previousInvoice = _invoiceService.GetInvoice(landlordId, model.RoomId, previousDate);

			var invoice = new Invoice {
				IsApproved = false,
				NewElectricNumber = model.NewElectricNumber,
				NewWaterNumber = model.NewWaterNumber,
				OldElectricNumber = previousInvoice!=null ? previousInvoice.OldElectricNumber : 0,
				OldWaterNumber = previousInvoice!= null ? previousInvoice.OldWaterNumber : 0,
				ContractId = model.ContractId,
				
			};

			if (model.Services!=null)
			{
				invoice.ServiceItems = _mapper.Map<List<ServiceItem>>(model.Services);
			}

			_invoiceService.CreateInvoice(landlordId, model.RoomId, DateTime.Now, invoice);
			_invoiceService.SaveChanges();

			try
			{
				
				return Ok();
			}
			catch
			{
				return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Lỗi không lập được hóa đơn !" });
			}
		}



	}
}
