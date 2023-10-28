﻿using Application.DTOs.Invoice;
using Application.Interface.ApplicationServices;
using Domain.Common;
using Domain.Entities;
using Domain.Enum;
using Domain.Interface;
using Domain.IRepositorys;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Application.Implementation.ApplicationServices
{
	public class InvoiceService : IInvoiceService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IBranchRepository _branchRepository;
		private readonly IAreaRepository _areaRepository;
		private readonly ILandlordRepository _landlordRepository;
		private readonly IServiceRepository _serviceRepository;
		private readonly IRoomRepository _roomRepository;
		private readonly IInvoiceRepository _invoiceRepository;
		private readonly IContractRepository _contractRepository;

		public InvoiceService(IUnitOfWork unitOfWork, IBranchRepository branchRepository, IAreaRepository areaRepository, ILandlordRepository landlordRepository, IServiceRepository serviceRepository, IRoomRepository roomRepository, IInvoiceRepository invoiceRepository, IContractRepository contractRepository)
		{
			_unitOfWork=unitOfWork;
			_branchRepository=branchRepository;
			_areaRepository=areaRepository;
			_landlordRepository=landlordRepository;
			_serviceRepository=serviceRepository;
			_roomRepository=roomRepository;
			_invoiceRepository=invoiceRepository;
			_contractRepository=contractRepository;
		}

		public AppResult CreateInvoice(int landlordId, int roomid, DateTime date, Invoice invoice)
		{
			var room = _roomRepository.FindById(roomid, r => r.Contracts);
			if (room == null) { return new AppResult { Success = false, Message = "Lỗi không lập được hóa đơn !" }; }
			var contract = room.Contracts.FirstOrDefault(r => r.Status == Domain.Enum.ContractStatus.Active && r.LandlordId == landlordId);
			if (contract == null) { return new AppResult { Success = false, Message = "Lỗi không lập được hóa đơn !" }; }
			var landlord = _landlordRepository.FindById(landlordId, l => l.User);
			if (landlord == null) { return new AppResult { Success = false, Message = "Không tìm thấy người dùng !" }; }
			var currentInvoice = _invoiceRepository.FindAll(i => i.ContractId == contract.Id && i.CreatedDate.Year == date.Year && i.CreatedDate.Month == date.Month, i => i.ServiceItems).FirstOrDefault();

			if (currentInvoice == null) {

				invoice.CreatedDate = DateTime.Now;
				invoice.UpdatedDate = DateTime.Now;
				invoice.CreatedBy = landlord.User.UserName??"";
				invoice.UpdatedBy = landlord.User.UserName??"";
				invoice.IsApproved = false;
				invoice.InvoiceCode = "";
             
				decimal servicePrice = 0;
				foreach (var item in invoice.ServiceItems) 
				{ 
					servicePrice += item.Price; 
					item.CreatedDate = DateTime.Now;
					item.UpdatedDate = DateTime.Now;
					item.CreatedBy= landlord.User.UserName??"";
					item.UpdatedBy= landlord.User.UserName??"";
				}
			 
				invoice.TotalPrice = servicePrice + contract.RentalPrice + (invoice.NewElectricNumber-invoice.OldElectricNumber)*contract.ElectricityCosts + (invoice.NewWaterNumber - invoice.OldWaterNumber)*contract.WaterCosts;

				_invoiceRepository.Add(invoice);

			}
			else
			{
				currentInvoice.UpdatedDate = DateTime.Now;
				currentInvoice.UpdatedBy = landlord.User.UserName??"";
				currentInvoice.NewElectricNumber = invoice.NewElectricNumber;
				currentInvoice.NewWaterNumber = invoice.NewWaterNumber;
				currentInvoice.ServiceItems = invoice.ServiceItems;

				decimal servicePrice = 0;
				foreach (var item in currentInvoice.ServiceItems)
				{
					servicePrice += item.Price;
					item.CreatedDate = DateTime.Now;
					item.UpdatedDate = DateTime.Now;
					item.CreatedBy= landlord.User.UserName??""; 
					item.UpdatedBy= landlord.User.UserName??"";
				}

				currentInvoice.TotalPrice = servicePrice + contract.RentalPrice + (currentInvoice.NewElectricNumber-currentInvoice.OldElectricNumber)*contract.ElectricityCosts + (currentInvoice.NewWaterNumber - currentInvoice.OldWaterNumber)*contract.WaterCosts;

				_invoiceRepository.Update(currentInvoice); 

			}

			return new AppResult { Success = false, Message = "Lỗi không lập được hóa đơn !" };
		}


		public Invoice GetInvoice(int landlordId, int roomid, DateTime date )
		{
			var room = _roomRepository.FindById( roomid, r=>r.Contracts );
			if( room == null ) { return null; }
			var contract = room.Contracts.FirstOrDefault(r => r.Status == Domain.Enum.ContractStatus.Active && r.LandlordId == landlordId);
			if(contract == null) { return null; }

			var invoice = _invoiceRepository.FindAll(i=>i.ContractId == contract.Id && i.CreatedDate.Year == date.Year && i.CreatedDate.Month == date.Month ,i=>i.ServiceItems).FirstOrDefault();

			if( invoice == null ) { return null; }

			return invoice;

		}

		public Invoice GetInvoiceById(int landlordId, int invoiceId)
		{

			


			var invoice = _invoiceRepository.FindAll(i => i.Id == invoiceId, i => i.ServiceItems).FirstOrDefault();
			if (invoice == null) { return null; }

			var contracts = _contractRepository.FindAll(r => r.LandlordId == landlordId, c => c.Invoices);
			if (contracts == null) { return null; }

			var contract = contracts.Where(c => c.Id == invoice.ContractId).FirstOrDefault();
			if (contract == null) { return null; }

			invoice.Contract = contract;
			invoice.ContractId =contract.Id;

	
			return invoice;
		}

		public ICollection<Invoice> GetInvoiceOfDataTable(int landlordId, string status, int month, int year, int branchid)
		{
			bool isAppro = false;
			isAppro = (status =="unpaid")?  false : true;

		

			var  contract = _contractRepository.FindAll(c => c.LandlordId==landlordId, c => c.Invoices).ToList();

			if(contract == null)
			{
				return new List<Invoice>();
			}

			IEnumerable<Invoice> allInvoice = new List<Invoice>();
			IEnumerable<Invoice> invoice = new List<Invoice>();

			if (branchid==0) //branch
			{
				allInvoice = contract.SelectMany(c => c.Invoices);
			}
			else
			{
				allInvoice = contract.Where(c=>c.BranchId==branchid).SelectMany(c => c.Invoices );
			}

			if (status != "none" && month == 0 && year == 0 ) //status
			{
				invoice = allInvoice.Where(i => i.IsApproved == isAppro );
			}
			else if (status!= "none" && month == 0 && year != 0) //status + year 
			{
				invoice = allInvoice.Where(i => i.IsApproved == isAppro  && i.CreatedDate.Year == year );
			}
			else if (status!= "none" && month != 0 && year != 0) //status + year + month
			{
				invoice = allInvoice.Where(i => i.IsApproved == isAppro  && i.CreatedDate.Year == year && i.CreatedDate.Month == month);
			}
			else if (status== "none"  && month == 0 && year != 0) //year 
			{
				invoice = allInvoice.Where(i =>  i.CreatedDate.Year == year );
			}
			else if (status== "none" && month != 0 && year != 0) //year + month
			{
				invoice = allInvoice.Where(i => i.CreatedDate.Year == year && i.CreatedDate.Month == month);
			}
			else
			{
				invoice = allInvoice; //all
			}

			foreach ( var item in invoice)
			{
				item.Contract = contract.FirstOrDefault(c=>c.Id==item.ContractId);
			}




			return invoice.ToList();
		}

		public void SaveChanges()
		{
			_unitOfWork.Commit();
		}
	}
}
