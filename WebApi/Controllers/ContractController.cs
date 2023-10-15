﻿using Microsoft.AspNetCore.Authorization;
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

        public ContractController(IBranchService branchService, UserManager<AppUser> userManager, ILandlordService landlordService, IMapper mapper, IContractService contractService, IBoundaryService boundaryService)
        {
            _branchService=branchService;
            _userManager=userManager;
            _landlordService=landlordService;
            _mapper=mapper;
            _contractService=contractService;
            _boundaryService=boundaryService;
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



    }
}
