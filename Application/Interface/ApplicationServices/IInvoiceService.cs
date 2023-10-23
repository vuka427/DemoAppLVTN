﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interface.ApplicationServices
{
	public interface IInvoiceService
	{
		Invoice GetInvoice(int landlordId, int roomid, DateTime date);


	}
}
