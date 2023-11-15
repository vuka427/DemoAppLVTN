using Application.DTOs.Invoice;
using Domain.Common;
using Domain.Entities;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interface.ApplicationServices
{
	public interface IInvoiceService
	{
		Invoice GetInvoice(int landlordId, int roomid, DateTime date);
		Invoice GetInvoiceById(int landlordId, int invoiceId);
        Invoice GetInvoiceTenantById(int tenantId, int invoiceId);
        bool SetInvoiceIsApproved(int landlordId, int invoiceId);

		AppResult CreateInvoice(int landlordId, int roomid, DateTime date, Invoice invoice);

		ICollection<Invoice> GetInvoiceOfDataTable(int landlordId, string status, int month, int year, int branchid);
		ICollection<Invoice> GetInvoiceTenantOfDataTable(int tenantId, string status, int month, int year);
        ICollection<Invoice> GetInvoiceRoom(int landlordId, int roomId);


        void SaveChanges();
	}
	
}
