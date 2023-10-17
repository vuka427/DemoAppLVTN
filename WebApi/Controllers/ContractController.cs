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
using Application.Implementation.DomainServices;
using Application.Interface.ApplicationServices;

using Application.Interface;
using SelectPdf;
using Microsoft.AspNetCore.Html;
using System.IO;

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
        private readonly IEmailService _mailService;
       

        public ContractController(IBranchService branchService, UserManager<AppUser> userManager, ILandlordService landlordService, IMapper mapper, IContractService contractService, IBoundaryService boundaryService, IEmailService mailService)
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
                    recordsFiltered = filteredResultsCount,
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
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "can't create branch!" });
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


        [HttpPost]
        [Route("pdf")]
        [AllowAnonymous]
        public async Task<IActionResult> ContractToPDF()
        {

            DateTime dayNow = DateTime.Now;
            string day = dayNow.Day.ToString();
            string year = dayNow.Year.ToString();
            string month = dayNow.Month.ToString();

            string a = @"<p style='margin-top:0cm;margin-right:0cm;margin-bottom:6.0pt;margin-left:0cm;font-size:11.0pt;font-family:""Calibri"",sans-serif;text-align:center;line-height:18.75pt;background:white;vertical-align:baseline;'><strong><span style='font-size:26px;font-family:""Times New Roman"",serif;color:black;border:none windowtext 1.0pt;padding:0cm;'>CỘNG XÃ HỘI CHỦ NGHĨA VIỆT NAM</span></strong><span style='font-size:19px;font-family:""Times New Roman"",serif;color:black;'><br>&nbsp;</span><strong><u><span style='font-size:22px;font-family:""Times New Roman"",serif;color:black;'>Độc lập - Tự do - Hạnh phúc</span></u></strong></p>
                            <p style='margin-top:0cm;margin-right:0cm;margin-bottom:6.0pt;margin-left:0cm;font-size:11.0pt;font-family:""Calibri"",sans-serif;text-align:center;line-height:18.75pt;background:white;vertical-align:baseline;'><strong><span style='font-size:29px;font-family:""Times New Roman"",serif;color:black;border:none windowtext 1.0pt;padding:0cm;'>&nbsp;</span></strong></p>
                            <p style='margin-top:0cm;margin-right:0cm;margin-bottom:6.0pt;margin-left:0cm;font-size:11.0pt;font-family:""Calibri"",sans-serif;text-align:center;line-height:18.75pt;background:white;vertical-align:baseline;'><strong><span style='font-size:29px;font-family:""Times New Roman"",serif;color:black;border:none windowtext 1.0pt;padding:0cm;'>HỢP ĐỒNG THUÊ NHÀ TRỌ</span></strong><span style='font-size:19px;font-family:""Times New Roman"",serif;color:black;'><br>&nbsp;</span><em><span style='font-size:22px;font-family:""Times New Roman"",serif;color:black;border:none windowtext 1.0pt;padding:0cm;'>(Số: ..../HĐTNO)</span></em></p>
                            <p style='margin-top:0cm;margin-right:0cm;margin-bottom:6.0pt;margin-left:0cm;font-size:11.0pt;font-family:""Calibri"",sans-serif;text-align:justify;line-height:18.75pt;background:white;vertical-align:baseline;'><em><span style='font-size:27px;font-family:""Times New Roman"",serif;color:black;border:none windowtext 1.0pt;padding:0cm;'>Hôm nay, ngày …. tháng …. năm ….., Tại ………………………..Chúng tôi gồm có:</span></em></p>
                            <p style='margin-top:0cm;margin-right:0cm;margin-bottom:6.0pt;margin-left:0cm;font-size:11.0pt;font-family:""Calibri"",sans-serif;text-align:justify;line-height:18.75pt;background:white;vertical-align:baseline;'><strong><span style='font-size:27px;font-family:""Times New Roman"",serif;color:black;border:none windowtext 1.0pt;padding:0cm;'>BÊN CHO THUÊ (BÊN A):</span></strong></p>
                            <p style='margin-top:0cm;margin-right:0cm;margin-bottom:6.0pt;margin-left:0cm;font-size:11.0pt;font-family:""Calibri"",sans-serif;text-align:justify;line-height:18.75pt;background:white;vertical-align:baseline;'><span style='font-size:27px;font-family:""Times New Roman"",serif;color:black;border:none windowtext 1.0pt;padding:0cm;'>Ông/bà: ………………………………………. Năm sinh: …………………..</span></p>
                            <p style='margin-top:0cm;margin-right:0cm;margin-bottom:6.0pt;margin-left:0cm;font-size:11.0pt;font-family:""Calibri"",sans-serif;text-align:justify;line-height:18.75pt;background:white;vertical-align:baseline;'><span style='font-size:27px;font-family:""Times New Roman"",serif;color:black;border:none windowtext 1.0pt;padding:0cm;'>CMND/CCCD số: ………… Ngày cấp ………….. Nơi cấp ………………</span></p>
                            <p style='margin-top:0cm;margin-right:0cm;margin-bottom:6.0pt;margin-left:0cm;font-size:11.0pt;font-family:""Calibri"",sans-serif;text-align:justify;line-height:18.75pt;background:white;vertical-align:baseline;'><span style='font-size:27px;font-family:""Times New Roman"",serif;color:black;border:none windowtext 1.0pt;padding:0cm;'>Hộ khẩu: …………………………………………..……………………………</span></p>
                            <p style='margin-top:0cm;margin-right:0cm;margin-bottom:6.0pt;margin-left:0cm;font-size:11.0pt;font-family:""Calibri"",sans-serif;text-align:justify;line-height:18.75pt;background:white;vertical-align:baseline;'><span style='font-size:27px;font-family:""Times New Roman"",serif;color:black;border:none windowtext 1.0pt;padding:0cm;'>Địa chỉ:…………………………………………..………………………………</span></p>
                            <p style='margin-top:0cm;margin-right:0cm;margin-bottom:6.0pt;margin-left:0cm;font-size:11.0pt;font-family:""Calibri"",sans-serif;text-align:justify;line-height:18.75pt;background:white;vertical-align:baseline;'><span style='font-size:27px;font-family:""Times New Roman"",serif;color:black;border:none windowtext 1.0pt;padding:0cm;'>Điện thoại: …………………………………………..…………………………</span></p>
                            <p style='margin-top:0cm;margin-right:0cm;margin-bottom:6.0pt;margin-left:0cm;font-size:11.0pt;font-family:""Calibri"",sans-serif;text-align:justify;line-height:18.75pt;background:white;vertical-align:baseline;'><span style='font-size:27px;font-family:""Times New Roman"",serif;color:black;border:none windowtext 1.0pt;padding:0cm;'>Là chủ sở hữu nhà ở: …………………………………………..……………</span></p>
                            <p style='margin-top:0cm;margin-right:0cm;margin-bottom:6.0pt;margin-left:0cm;font-size:11.0pt;font-family:""Calibri"",sans-serif;line-height:18.75pt;background:white;vertical-align:baseline;'><span style='font-size:27px;font-family:""Times New Roman"",serif;color:black;border:none windowtext 1.0pt;padding:0cm;'> </span></p>
                            <p style='margin-top:0cm;margin-right:0cm;margin-bottom:6.0pt;margin-left:0cm;font-size:11.0pt;font-family:""Calibri"",sans-serif;line-height:18.75pt;background:white;vertical-align:baseline;'><span style='font-size:27px;font-family:""Times New Roman"",serif;color:black;border:none windowtext 1.0pt;padding:0cm;'> </span></p>
                            <p style='margin-top:0cm;margin-right:0cm;margin-bottom:6.0pt;margin-left:0cm;font-size:11.0pt;font-family:""Calibri"",sans-serif;text-align:justify;line-height:18.75pt;background:white;vertical-align:baseline;'><strong><span style='font-size:27px;font-family:""Times New Roman"",serif;color:black;border:none windowtext 1.0pt;padding:0cm;'>BÊN THUÊ (BÊN B):</span></strong></p>
                            <p style='margin-top:0cm;margin-right:0cm;margin-bottom:6.0pt;margin-left:0cm;font-size:11.0pt;font-family:""Calibri"",sans-serif;text-align:justify;line-height:18.75pt;background:white;vertical-align:baseline;'><span style='font-size:27px;font-family:""Times New Roman"",serif;color:black;border:none windowtext 1.0pt;padding:0cm;'>Ông/bà: ………………………………………. Năm sinh: …………………..</span></p>
                            <p style='margin-top:0cm;margin-right:0cm;margin-bottom:6.0pt;margin-left:0cm;font-size:11.0pt;font-family:""Calibri"",sans-serif;text-align:justify;line-height:18.75pt;background:white;vertical-align:baseline;'><span style='font-size:27px;font-family:""Times New Roman"",serif;color:black;border:none windowtext 1.0pt;padding:0cm;'>CMND/CCCD số: ………… Ngày cấp ………….. Nơi cấp ………………</span></p>
                            <p style='margin-top:0cm;margin-right:0cm;margin-bottom:6.0pt;margin-left:0cm;font-size:11.0pt;font-family:""Calibri"",sans-serif;text-align:justify;line-height:18.75pt;background:white;vertical-align:baseline;'><span style='font-size:27px;font-family:""Times New Roman"",serif;color:black;border:none windowtext 1.0pt;padding:0cm;'>Hộ khẩu: …………………………………………..……………………………</span></p>
                            <p style='margin-top:0cm;margin-right:0cm;margin-bottom:6.0pt;margin-left:0cm;font-size:11.0pt;font-family:""Calibri"",sans-serif;text-align:justify;line-height:18.75pt;background:white;vertical-align:baseline;'><span style='font-size:27px;font-family:""Times New Roman"",serif;color:black;border:none windowtext 1.0pt;padding:0cm;'>Địa chỉ:…………………………………………..………………………………</span></p>
                            <p style='margin-top:0cm;margin-right:0cm;margin-bottom:6.0pt;margin-left:0cm;font-size:11.0pt;font-family:""Calibri"",sans-serif;text-align:justify;line-height:18.75pt;background:white;vertical-align:baseline;'><span style='font-size:27px;font-family:""Times New Roman"",serif;color:black;border:none windowtext 1.0pt;padding:0cm;'>Điện thoại: …………………………………………..…………………………</span></p>
                            <p style='margin-top:0cm;margin-right:0cm;margin-bottom:6.0pt;margin-left:0cm;font-size:11.0pt;font-family:""Calibri"",sans-serif;text-align:justify;line-height:18.75pt;background:white;vertical-align:baseline;'><strong><em><span style='font-size:27px;font-family:""Times New Roman"",serif;color:black;border:none windowtext 1.0pt;padding:0cm;'>Hai bên cùng thỏa thuận ký hợp đồng với những nội dung sau:</span></em></strong></p>";

            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();

            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            converter.Options.MarginLeft = 70;
            converter.Options.MarginRight = 0;
            converter.Options.MarginTop = 40;
            converter.Options.MarginBottom = 30;

            // create a new pdf document converting the html code
            PdfDocument doc = converter.ConvertHtmlString(a);

            byte[] fileB = doc.Save();
            MemoryStream m = new MemoryStream(fileB);

            // save pdf document
            

            // close pdf document
           doc.Close();

           
            return File(m, "application/pdf","123.pdf");

        }




    }
}
