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
using WebApi.Model.Branch;
using WebApi.Model.JQDataTable;
using System.Diagnostics.Contracts;
using Application.Implementation.ApplicationServices;

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
        private readonly ITenantService _tenantService;

        public InvoiceController(IBranchService branchService, UserManager<AppUser> userManager, ILandlordService landlordService, IMapper mapper, IBoundaryService boundaryService, IRoomService roomService, IInvoiceService invoiceService, IContractService contractService, ITenantService tenantService)
        {
            _branchService=branchService;
            _userManager=userManager;
            _landlordService=landlordService;
            _mapper=mapper;
            _boundaryService=boundaryService;
            _roomService=roomService;
            _invoiceService=invoiceService;
            _contractService=contractService;
            _tenantService=tenantService;
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

			if (invoice != null) //đã lập 
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
					OldElectricNumber = previousInvoice!=null ? previousInvoice.NewElectricNumber : 0,
					OldWaterNumber = previousInvoice!= null ? previousInvoice.NewWaterNumber : 0,
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

			var invoice = new Invoice {
				IsApproved = false,
				NewElectricNumber = model.NewElectricNumber,
				NewWaterNumber = model.NewWaterNumber,
				OldElectricNumber = model.OldElectricNumber,
				OldWaterNumber = model.OldWaterNumber,
				ContractId = model.ContractId
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


		[HttpPost]
		[Route("invoicefordatatable")]
		public async Task<IActionResult> GetInvoiceForDataTable([FromBody] DatatableParam param ,[FromQuery] string status, [FromQuery] int  month,[FromQuery] int year, [FromQuery] int branchid)
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
				var invoices = _invoiceService.GetInvoiceOfDataTable(landlord.Id, status,month ,year ,branchid);

				if (!string.IsNullOrEmpty(param.search.value))
				{
					invoices = invoices.Where(m => m.Contract.B_Lessee.Contains(param.search.value)
												|| m.Contract.RoomNumber.ToString().Contains(param.search.value)).ToList();
				}

				totalResultsCount = invoices.Count();

				var result = invoices.Skip(param.start).Take(param.length).ToList();

				filteredResultsCount = result.Count();

				var Dataresult = _mapper.Map<List<InvoiceDataTableModel>>(result);

				return Json(new
				{
					// this is what datatables wants sending back

					draw = param.draw,
					recordsTotal = totalResultsCount,
					recordsFiltered = totalResultsCount,
					data = Dataresult
				});
			}
			catch
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { Status = "Error", Message = "Can get invoice!" });
			}

		}

		[HttpGet]
		[Route("detail")]
		public async Task<IActionResult> GetInvoice(int invoiceid)
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
				var invoice = _invoiceService.GetInvoiceById(landlordId, invoiceid);
				if (invoice == null)
				{
					return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Lỗi không tìm thấy hóa đơn!" });
				}

				var resultData = _mapper.Map<InvoiceDetailModel>(invoice);

				return Ok(resultData);
			}
			catch
			{
				return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Lỗi không tìm thấy hóa đơn!" });
			}
		}


        [HttpGet]
        [Route("tenant/detail")]
        public async Task<IActionResult> GetInvoiceTenant(int invoiceid)
        {
            var Identity = HttpContext.User;
            string CurrentUserId = "";
            if (Identity.HasClaim(c => c.Type == "userid"))
            {
                CurrentUserId = Identity.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString();
            }

            if (string.IsNullOrEmpty(CurrentUserId))
            {
                return Unauthorized();
            }
            var Tenant = _tenantService.GetTenantByUserId(CurrentUserId);
            if (Tenant == null)
            {
                return Unauthorized();
            }

            try
            {
                var invoice = _invoiceService.GetInvoiceTenantById(Tenant.Id, invoiceid);
                if (invoice == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Lỗi không tìm thấy hóa đơn!" });
                }

                var resultData = _mapper.Map<InvoiceDetailModel>(invoice);

                return Ok(resultData);
            }
            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Lỗi không tìm thấy hóa đơn!" });
            }
        }

        [HttpGet]
        [Route("room")]
        public async Task<IActionResult> GetInvoiceRoom(int roomid)
        {
			try
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

                var invoice = _invoiceService.GetInvoiceRoom(landlordId, roomid);
                if (invoice == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Lỗi không tìm thấy hóa đơn!" });
                }

                var resultData = _mapper.Map<List<InvoiceDetailModel>>(invoice);

                return Ok(resultData);
            }
            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Lỗi không tìm thấy hóa đơn!" });
            }
        }

        [HttpPost]
		[Route("pay")]
		public async Task<IActionResult> AgreeToPayInvoice(int invoiceid)
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
				bool updateResult = _invoiceService.SetInvoiceIsApproved(landlordId, invoiceid);
				if (!updateResult)
				{
					return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Lỗi không thanh toán được hóa đơn !" });
				}
				_invoiceService.SaveChanges();

				return Ok();
			}
			catch
			{
				return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Lỗi không thanh toán được hóa đơn !" });
			}
		}


        [HttpPost]
        [Route("tenant/invoicefordatatable")]
        public async Task<IActionResult> GetInvoiceTenantForDataTable([FromBody] DatatableParam param, [FromQuery] string status, [FromQuery] int month, [FromQuery] int year, [FromQuery] int branchid)
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
                return Unauthorized();
            }
            var Tenant = _tenantService.GetTenantByUserId(CurrentUserId);
            if (Tenant == null)
            {
                return Unauthorized();
            }

            try
            {
                var invoices = _invoiceService.GetInvoiceTenantOfDataTable(Tenant.Id, status, month, year);


                if (!string.IsNullOrEmpty(param.search.value))
                {
                    invoices = invoices.Where(m => m.Contract.B_Lessee.Contains(param.search.value)
                                                || m.Contract.RoomNumber.ToString().Contains(param.search.value)).ToList();
                }

                var Dataresult = _mapper.Map<List<InvoiceDataTableModel>>(invoices);
				int index = 1;
				foreach (var invoice in Dataresult)
				{
					invoice.Index= index;
					index++;
				}

                totalResultsCount = Dataresult.Count();

                var result = Dataresult.Skip(param.start).Take(param.length).ToList();

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


    }
}
