using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Model.Branch;
using WebApi.Model.JQDataTable;
using WebApi.Model;
using Application.Interface.IDomainServices;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using WebApi.Model.Contract;
using System.Linq.Dynamic.Core;
using Application.Interface.ApplicationServices;

using Application.Interface;
using SelectPdf;

using WebApi.Services.ContractToPdf;

using WebApi.Model.Invoice;


namespace WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContractController : Controller
    {
        private readonly IBranchService _branchService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILandlordService _landlordService;
        private readonly IMapper _mapper;
        private readonly IContractService _contractService;
        private readonly IBoundaryService _boundaryService;
        private readonly ISendMailService _mailService;
        private readonly ITenantService _tenantService;
        private readonly IInvoiceService _invoiceService;

        public ContractController(IBranchService branchService, UserManager<AppUser> userManager, ILandlordService landlordService, IMapper mapper, IContractService contractService, IBoundaryService boundaryService, ISendMailService mailService, ITenantService tenantService, IInvoiceService invoiceService)
        {
            _branchService=branchService;
            _userManager=userManager;
            _landlordService=landlordService;
            _mapper=mapper;
            _contractService=contractService;
            _boundaryService=boundaryService;
            _mailService=mailService;
            _tenantService=tenantService;
            _invoiceService=invoiceService;
        }

        [HttpPost]
        [Route("contractfordatatable")]
        public async Task<IActionResult> GetContractForDataTable([FromBody] DatatableParam param)
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
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Can find user!" });
            }
            var landlord = _landlordService.GetLandlordByUserId(CurrentUserId);
            if (landlord == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Can find user!" });
            }

            var contracts = _contractService.GetContract(landlord.Id);

            var sortColumn = param.order.FirstOrDefault().column.ToString();
            var sortColumnDirection = param.order.FirstOrDefault().dir;

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection) ))
            {
                contracts = contracts.OrderBy(sortColumn + " " + sortColumnDirection);
            }
            else
            {
                contracts = contracts.OrderBy("CreatedDate asc");
            }

            if (!string.IsNullOrEmpty(param.search.value))
            {
                contracts = contracts.Where(m => m.B_Lessee.Contains(param.search.value)
                                            || m.RoomNumber.ToString().Contains(param.search.value));
            }

            contracts = contracts.OrderByDescending(m => m.CreatedDate);

            totalResultsCount = contracts.Count();

            var result = contracts.Skip(param.start).Take(param.length).ToList();

            filteredResultsCount = result.Count();


            var Dataresult = _mapper.Map<List<ContractModel>>(result);

            _contractService.Dispose();

            int i = param.start+1;
            foreach(var dataItem in Dataresult)
            {
                dataItem.Index = i;
                i++;
            }

            try
            {
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
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { Status = "Error", Message = "Can get contracts!" });
            }

        }

        [HttpPost]
        [Route("add")]
        public IActionResult CreateContract(ContractCreateModel model)
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

                
                var contract = _mapper.Map<Contract>(model);

                var branch = _branchService.GetBranchById(landlordId, model.BranchId);
                if (branch != null)
                {
                    string ad = _boundaryService.GetAddress(branch.Province, branch.District, branch.Wards);
                    contract.BranchAddress =  branch.Address +", "+ ad;
                }

                 _contractService.CreateContract(landlordId,contract);
                 _contractService.SaveChanges();

            try
            {
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "can't create contract!" });
            }
        }


        [HttpPost]
        [Route("sendemail")]
        [AllowAnonymous]
        public async Task<IActionResult> SendMail()
        {
            var result = await _mailService.SendEmailAsync("atrox427@gmail.com","thử nghiệm gửi mail","đây là email gửi từ ứng dụng quản lý phòng trọ 2");
            if (result.Success)
            {
                return Ok("send mail success !");
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = result.Message });
            }
        }


        [HttpGet]
        [Route("pdf")]
        public async Task<IActionResult> ContractToPDF( int contractid)
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

			var contract = _contractService.GetContractById(landlordId, contractid);
            if (contract == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "không tìm thấy hợp đồng " });
            }

            var d = DateTime.Now;

			string contractHtml = ContractToPdf.ConverterToHtml(contract);
            string filename = "HDTT_"+contract.Id+"_p"+contract.RoomNumber+"_"+d.Day+"_"+d.Month+"_"+d.Year+".pdf";

			// instantiate a html to pdf converter object
			HtmlToPdf converter = new HtmlToPdf();

			converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            converter.Options.MarginLeft = 70;
            converter.Options.MarginRight = 50;
            converter.Options.MarginTop = 50;
            converter.Options.MarginBottom = 50;

            try
            {
                // create a new pdf document converting the html code
			    PdfDocument doc = converter.ConvertHtmlString(contractHtml);

			    byte[] fileB = doc.Save();
                MemoryStream m = new MemoryStream(fileB);

                // close pdf document
                doc.Close();
				return File(m, "application/pdf", filename);
			}
            catch(Exception e)
            {
				return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "lỗi!. không render được file Pdf " });
			}

        }

		[HttpGet]
		[Route("detail")]
		public async Task<IActionResult> GetContract(int contractid)
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
				    CurrentLandlordId = Identity.Claims.FirstOrDefault(c => c.Type == "landlordid").Value.ToString()??"";
			    }
			    var result = int.TryParse(CurrentLandlordId, out landlordId);
			    if (string.IsNullOrEmpty(CurrentUserId) && string.IsNullOrEmpty(CurrentLandlordId) && !result)
			    {
				    return Unauthorized();
			    }

			    var contract = _contractService.GetContractById(landlordId, contractid);
			    if (contract == null)
			    {
				    return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "không tìm thấy hợp đồng " });
			    }

			

                var contractResult  = _mapper.Map<ContractDetailModel>(contract);

				return Ok(contractResult);
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "lỗi! tìm thấy chi tiết hóa đơn " });
			}

		}

        [HttpGet]
        [Route("tenant/detail")]
        public async Task<IActionResult> GetContractForTenant(int contractId)
        {
            try
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

                var contract = _contractService.GetContractByTenantId(Tenant.Id, contractId );
                if (contract == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Không tìm thấy hợp đồng" });
                }

                var contractResult = _mapper.Map<ContractDetailModel>(contract);

                return Ok(contractResult);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Lỗi! không tìm thấy chi tiết hóa đơn" });
            }
        }




        [HttpPost]
		[Route("end")]
		public async Task<IActionResult> ContractToEnd(int contractid, InvoiceCreateModel model)
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
                int day = model.LeaveDay.Day;
                int dayInMonth = DateTime.DaysInMonth(model.LeaveDay.Year,model.LeaveDay.Month) ;


                var invoice = new Invoice
                {
                    IsApproved = false,
                    NewElectricNumber = model.NewElectricNumber,
                    NewWaterNumber = model.NewWaterNumber,
                    OldElectricNumber = model.OldElectricNumber,
                    OldWaterNumber = model.OldWaterNumber,
                    ContractId = model.ContractId,
                    StayDay= day,
                    Day= dayInMonth,
                    
                };

                if (model.Services!=null)
                {
                    invoice.ServiceItems = _mapper.Map<List<ServiceItem>>(model.Services);
                }

                _invoiceService.CreateInvoice(landlordId, model.RoomId, DateTime.Now, invoice);

                var contract = _contractService.ContractToEnd(landlordId, contractid);
			    if (!contract)
			    {
				    return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "không tìm thấy hợp đồng " });
			    }
                _invoiceService.SaveChanges();
                _contractService.SaveChanges();
                return Ok();
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "lỗi!. không render được file Pdf " });
			}

		}

        [HttpGet]
        [Route("searchTenant")]
        public async Task<IActionResult> SearchTenantByPhone(string phone)
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
                var tenant = _tenantService.GetTenantByPhone(phone);
                if(tenant != null)
                {
                    return Ok(new { Success = true, Id = tenant.Id, FullName = tenant.FullName, Phone = tenant.Phone , DateOfBirth = tenant.DateOfBirth.ToShortDateString() });
                }

                return Ok(new { Success = false });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "lỗi! không render được file Pdf " });
            }

        }

        [HttpPost]
        [Route("linktotenant")]
        public async Task<IActionResult> LinkToTenant(int contractid,int tenantid)
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

            var LinkResult = _contractService.LinkToTenant(landlordId, contractid, tenantid);
            if (!LinkResult)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "không tìm thấy hợp đồng " });
            }

            try
            {

                _contractService.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "lỗi!. không render được file Pdf " });
            }

        }

        //

        [HttpPost]
        [Route("tenant/contractfordatatable")]
        public async Task<IActionResult> GetContractTenantForDataTable([FromBody] DatatableParam param)
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
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Can find user!" });
            }
            var Tenant = _tenantService.GetTenantByUserId(CurrentUserId);
            if (Tenant == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Can find user!" });
            }

            var contracts = _contractService.GetContractForTenant(Tenant.Id);

            var sortColumn = param.order.FirstOrDefault().column.ToString();
            var sortColumnDirection = param.order.FirstOrDefault().dir;

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                contracts = contracts.OrderBy(sortColumn + " " + sortColumnDirection);
            }
            else
            {
                contracts = contracts.OrderBy("CreatedDate asc");
            }

            if (!string.IsNullOrEmpty(param.search.value))
            {
                contracts = contracts.Where(m => m.B_Lessee.Contains(param.search.value)
                                            || m.RoomNumber.ToString().Contains(param.search.value));
            }

            contracts = contracts.OrderByDescending(m => m.CreatedDate);

            totalResultsCount = contracts.Count();

            var result = contracts.Skip(param.start).Take(param.length).ToList();

            filteredResultsCount = result.Count();


            var Dataresult = _mapper.Map<List<ContractModel>>(result);

            _contractService.Dispose();

            int i = param.start+1;
            foreach (var dataItem in Dataresult)
            {
                dataItem.Index = i;
                i++;
            }

            try
            {
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
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { Status = "Error", Message = "Can get contracts!" });
            }


        }

        [HttpGet]
        [Route("tenant/all")]
        public async Task<IActionResult> GetContractTenantAll()
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
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Can find user!" });
            }
            var Tenant = _tenantService.GetTenantByUserId(CurrentUserId);
            if (Tenant == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Can find user!" });
            }

            var contracts = _contractService.GetContractForTenant(Tenant.Id);


            var Dataresult = _mapper.Map<List<ContractModel>>(contracts);

         

            try
            {
                return Ok(Dataresult);
              
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { Status = "Error", Message = "Can get contracts!" });
            }


        }



        [HttpGet]
        [Route("tenant/current")]
        public async Task<IActionResult> GetContractsForTenant()
        {
            try
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

                var contract = _contractService.GetContractForTenant(Tenant.Id).Where(c=>c.Status == Domain.Enum.ContractStatus.Active);
                if (contract == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Không tìm thấy hợp đồng" });
                }

                var contractResult = _mapper.Map<List<ContractDetailModel>>(contract);

                return Ok(contractResult);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Lỗi! Không tìm thấy hợp đồng" });
            }
        }



    }
}
