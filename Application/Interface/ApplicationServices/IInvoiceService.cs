﻿using Domain.Common;
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

		AppResult CreateInvoice(int landlordId, int roomid, DateTime date, Invoice invoice);

		ICollection<Invoice> GetInvoiceOfDataTable(int landlordId, string status, int month, int year, int branchid);

		void SaveChanges();
	}
	
}