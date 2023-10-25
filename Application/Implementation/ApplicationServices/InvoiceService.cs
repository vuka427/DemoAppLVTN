using Application.Interface.ApplicationServices;
using Domain.Common;
using Domain.Entities;
using Domain.Interface;
using Domain.IRepositorys;
using System;
using System.Collections.Generic;
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




				_invoiceRepository.Add(invoice);


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
	}
}
