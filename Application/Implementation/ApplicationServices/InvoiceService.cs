using Application.DTOs.Invoice;
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
using System.Threading.Tasks;

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
		private readonly IEmailService _emailService;
		private readonly ITenantRepository _tenantRepository;

        public InvoiceService(IUnitOfWork unitOfWork, IBranchRepository branchRepository, IAreaRepository areaRepository, ILandlordRepository landlordRepository, IServiceRepository serviceRepository, IRoomRepository roomRepository, IInvoiceRepository invoiceRepository, IContractRepository contractRepository, IEmailService emailService, ITenantRepository tenantRepository)
        {
            _unitOfWork=unitOfWork;
            _branchRepository=branchRepository;
            _areaRepository=areaRepository;
            _landlordRepository=landlordRepository;
            _serviceRepository=serviceRepository;
            _roomRepository=roomRepository;
            _invoiceRepository=invoiceRepository;
            _contractRepository=contractRepository;
            _emailService=emailService;
            _tenantRepository=tenantRepository;
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
				invoice.ElectricityCosts = contract.ElectricityCosts;
				invoice.WaterCosts = contract.WaterCosts;
             
				decimal servicePrice = 0;
				foreach (var item in invoice.ServiceItems) 
				{ 
					servicePrice += item.Price; 
					item.CreatedDate = DateTime.Now;
					item.UpdatedDate = DateTime.Now;
					item.CreatedBy= landlord.User.UserName??"";
					item.UpdatedBy= landlord.User.UserName??"";
				}

				invoice.TotalPrice = servicePrice + (contract.RentalPrice/invoice.Day*invoice.StayDay) + (invoice.NewElectricNumber-invoice.OldElectricNumber) * contract.ElectricityCosts + (invoice.NewWaterNumber - invoice.OldWaterNumber) * contract.WaterCosts;



				_invoiceRepository.Add(invoice);

				if (contract.TenantId != null && contract.TenantId.Value > 0)
				{
					var tenant = _tenantRepository.FindById(contract.TenantId.Value, t=>t.User);

					if(tenant != null && tenant.User != null)
					{
                        Func<Task<AppResult>> taskSend = () => { return _emailService.SendMailCreateInvoice(tenant.User.Email, "", contract, invoice); };

                        Task<Task<AppResult>> task = new Task<Task<AppResult>>(taskSend);

                        task.Start();
                    }

					
				}


			}
			else // update
			{
				currentInvoice.UpdatedDate = DateTime.Now;
				currentInvoice.UpdatedBy = landlord.User.UserName??"";
				currentInvoice.NewElectricNumber = invoice.NewElectricNumber;
				currentInvoice.NewWaterNumber = invoice.NewWaterNumber;
                currentInvoice.OldElectricNumber = invoice.OldElectricNumber;
                currentInvoice.OldWaterNumber = invoice.OldWaterNumber;
                currentInvoice.ServiceItems = invoice.ServiceItems;
                currentInvoice.ElectricityCosts = contract.ElectricityCosts;
                currentInvoice.WaterCosts = contract.WaterCosts;

                decimal servicePrice = 0;
				foreach (var item in currentInvoice.ServiceItems)
				{
					servicePrice += item.Price * item.Quantity;
					item.CreatedDate = DateTime.Now;
					item.UpdatedDate = DateTime.Now;
					item.CreatedBy= landlord.User.UserName??""; 
					item.UpdatedBy= landlord.User.UserName??"";
				}

				currentInvoice.TotalPrice = servicePrice + (contract.RentalPrice/invoice.Day*invoice.StayDay)  + (currentInvoice.NewElectricNumber-currentInvoice.OldElectricNumber)*contract.ElectricityCosts + (currentInvoice.NewWaterNumber - currentInvoice.OldWaterNumber)*contract.WaterCosts;

				_invoiceRepository.Update(currentInvoice);

                if (contract.TenantId != null && contract.TenantId.Value > 0)
                {
                    var tenant = _tenantRepository.FindById(contract.TenantId.Value, t => t.User);

                    if (tenant != null && tenant.User != null)
                    {
                        Func<Task<AppResult>> taskSend = () => { return _emailService.SendMailCreateInvoice(tenant.User.Email, "", contract, currentInvoice); };

                        Task<Task<AppResult>> task = new Task<Task<AppResult>>(taskSend);

                        task.Start();
                    }


                }

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

			return invoice.OrderByDescending(i=>i.CreatedDate).ToList();
		}

        public ICollection<Invoice> GetInvoiceRoom(int landlordId, int roomId)
        {
            var contract = _contractRepository.FindAll(c => c.LandlordId==landlordId && c.RoomId == roomId, c => c.Invoices);


           var  allInvoice = contract.SelectMany(c => c.Invoices).Where(i=>i.IsApproved == false).ToList();


            return allInvoice;
            

        }

        public Invoice GetInvoiceTenantById(int tenantId, int invoiceId)
        {
            var invoice = _invoiceRepository.FindAll(i => i.Id == invoiceId, i => i.ServiceItems).FirstOrDefault();
            if (invoice == null) { return null; }

            var contracts = _contractRepository.FindAll(r => r.TenantId == tenantId, c => c.Invoices);
            if (contracts == null) { return null; }

            var contract = contracts.Where(c => c.Id == invoice.ContractId).FirstOrDefault();
            if (contract == null) { return null; }

            invoice.Contract = contract;
            invoice.ContractId =contract.Id;


            return invoice;
        }

        public ICollection<Invoice> GetInvoiceTenantOfDataTable(int tenantId, string status, int month, int year)
        {
            bool isAppro = false;
            isAppro = (status =="unpaid") ? false : true;



            var contract = _contractRepository.FindAll(c => c.TenantId==tenantId, c => c.Invoices).ToList();

            if (contract == null)
            {
                return new List<Invoice>();
            }

            IEnumerable<Invoice> allInvoice = new List<Invoice>();
            IEnumerable<Invoice> invoice = new List<Invoice>();

            allInvoice = contract.SelectMany(c => c.Invoices);



            if (status != "none" && month == 0 && year == 0) //status
            {
                invoice = allInvoice.Where(i => i.IsApproved == isAppro);
            }
            else if (status!= "none" && month == 0 && year != 0) //status + year 
            {
                invoice = allInvoice.Where(i => i.IsApproved == isAppro  && i.CreatedDate.Year == year);
            }
            else if (status!= "none" && month != 0 && year != 0) //status + year + month
            {
                invoice = allInvoice.Where(i => i.IsApproved == isAppro  && i.CreatedDate.Year == year && i.CreatedDate.Month == month);
            }
            else if (status== "none"  && month == 0 && year != 0) //year 
            {
                invoice = allInvoice.Where(i => i.CreatedDate.Year == year);
            }
            else if (status== "none" && month != 0 && year != 0) //year + month
            {
                invoice = allInvoice.Where(i => i.CreatedDate.Year == year && i.CreatedDate.Month == month);
            }
            else
            {
                invoice = allInvoice; //all
            }

            foreach (var item in invoice)
            {
                item.Contract = contract.FirstOrDefault(c => c.Id==item.ContractId);
            }




            return invoice.OrderByDescending(i => i.CreatedDate).ToList();
        }



        public void SaveChanges()
		{
			_unitOfWork.Commit();
		}

		public bool SetInvoiceIsApproved(int landlordId, int invoiceId)
		{
			var invoice = _invoiceRepository.FindAll(i => i.Id == invoiceId, i => i.ServiceItems).FirstOrDefault();
			if (invoice == null) { return false; }

			var contracts = _contractRepository.FindAll(r => r.LandlordId == landlordId, c => c.Invoices);
			if (contracts == null) { return false; }

			var contract = contracts.Where(c => c.Id == invoice.ContractId).FirstOrDefault();
			if (contract == null) { return false; }

			invoice.IsApproved= true;
			_invoiceRepository.Update(invoice);

         

            if (contract.TenantId != null && contract.TenantId.Value > 0)
            {
                var tenant = _tenantRepository.FindById(contract.TenantId.Value, t => t.User);

                if (tenant != null && tenant.User != null)
                {
                    Func<Task<AppResult>> taskSend = () => { return _emailService.SendMailPayInvoice(tenant.User.Email, "", contract, invoice); };

                    Task<Task<AppResult>> task = new Task<Task<AppResult>>(taskSend);

                    task.Start();
                }
            }



            return true;
		}
	}
}
