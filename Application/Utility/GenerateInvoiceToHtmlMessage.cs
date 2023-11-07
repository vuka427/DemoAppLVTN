﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Utility
{
    internal class GenerateInvoiceToHtmlMessage
    {
        public static string ConverterCreateInvoiceToHtml(Contract contract, Invoice invoice)
        {
            StringBuilder Html = new StringBuilder();

            Html.Append(@$" <!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
                            <html xmlns=""http://www.w3.org/1999/xhtml"" xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:o=""urn:schemas-microsoft-com:office:office"">

                            <body class=""body"" style=""padding:0 !important; margin:0 !important; display:block !important; min-width:100% !important; width:100% !important; background:#f4f4f4; -webkit-text-size-adjust:none;"">
	                        <table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" bgcolor=""#f4f4f4"" class=""gwfw"">
		                    <tr>
			                <td align=""center"" valign=""top"" style=""padding: 50px 10px;"" class=""p10"">
				            <table width=""650"" border=""0"" cellspacing=""0"" cellpadding=""0"" class=""m-shell"">
					        <tr>
						    <td class=""td"" bgcolor=""#f4f4f4"" style="" width:650px; min-width:650px;  padding:0; margin:0; font-weight:normal;"">
                        ");
            Html.Append(@$"<!-- Header -->
							<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"">
								<tr>
									<td bgcolor=""#3d85c6"" style=""color:#f4f4f4; padding: 25px 50px; border-bottom: 2px solid #f4f4f4; border-radius: 6px 6px 0px 0px;"" class=""p30-20"">
										<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"">
											<tr>
												<td  class="""" style=""font-size:16pt; font-weight:bold; text-align:left;"">Hệ thống quản lý phòng trọ</td>
												<td class="""" style=""font-size:10pt;font-weight:bold; text-align:left;"">Nhà trọ XXX</td>
											</tr>
											<tr>
												<td  class="""" style=""font-size:16pt; font-weight:bold;text-align:left;""></td>
												<td class="""" style=""font-size:10pt; text-align:left;"">Địa chỉ : </td>
											</tr>
											<tr>
												<td  class="""" style=""font-size:16pt; font-weight:bold;  text-align:left;""></td>
												<td class="""" style=""font-size:10pt; text-align:left;"">Số điện thoại : </td>
											</tr>
										</table>
									</td>
								</tr>
							</table>
							<!-- END Header -->
                            ");

            Html.Append(@$"<!-- giới thiệu -->
							<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"">
								<tr>
									<td style=""padding: 10px; border-bottom: 2px solid #f4f4f4;"" class=""p30-20"">
										<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"">
											<tr>
												<td class=""h3"" style="" padding-bottom: 10px; color:#333333; font-weight:bold; font-family:'Montserrat', Arial, sans-serif; font-size:20px; line-height:25px; text-align:center;"">
												Thông báo tiền trọ tháng 10
												</td>
											</tr>
											<tr>
												<td class=""h3"" style="" padding-bottom: 20px; color:#333333;  font-family:'Montserrat', Arial, sans-serif; font-size:20px; line-height:25px; text-align:center;"">
												phòng 2.4
												</td>
											</tr>
											<tr>
												<td class=""h4"" style=""padding-bottom: 0px; color:#333333; font-family:'Montserrat', Arial, sans-serif; font-size:16px; line-height:10px; text-align:left;"">
												<multiline>
												Xin chào, tên j đó
												</multiline>
												</td>
											</tr>
											<tr>
												<td class=""text"" style=""padding-bottom: 10px; color:#555555; font-family:Arial, sans-serif; font-size:15px; line-height:30px; text-align:left; min-width:auto !important;"">
													<multiline>
													   Nhà trọ xxx , xin thông báo tiền thuê phòng tháng 10  của quý khách là <span style=""color: red"">11,000,000</span>  vnd. <br>
													   Xin quý khách vui lòng thanh toán đúng hẹn ! <br>
													   Rất mong nhận được sự hợp tác của quý đối tác khách hàng.<br>
														Xin chân thành cảm ơn!
													</multiline>
												</td>
											</tr>
											<tr>
												<td class=""text"" style="" color:#555555; font-family:Arial, sans-serif; font-size:15px; line-height:30px; text-align:left; min-width:auto !important;"">
													<multiline>
													   Chi tiết hóa đơn bên dưới:
													  
													</multiline>
												</td>
											</tr>
											
										</table>
									</td>
								</tr>
							</table>
							<!-- giới thiệu -->
                            ");
            Html.Append(@$"<!-- CTA -->
							
							<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"">
								<table   width=""100%"" style="" text-align: left; color: black);background-color: #f4f4f4;font-size: 16px; border: 1px solid black; border-collapse: collapse;"">
								<thead style=""  border: 1px solid black;"">
									<tr style=""  border: 1px solid black;"">
										<th style=""text-align: -webkit-match-parent ;border: 1px solid black; border-collapse: collapse;"">STT</th>
										<th style=""text-align: -webkit-match-parent; border: 1px solid black; border-collapse: collapse;"">Tên dịch vụ</th>
										<th style=""text-align: -webkit-match-parent; border: 1px solid black; border-collapse: collapse;"">Chỉ số đầu</th>
										<th style=""text-align: -webkit-match-parent; border: 1px solid black; border-collapse: collapse;"">Chỉ số cuối</th>
										<th style=""text-align: -webkit-match-parent; border: 1px solid black; border-collapse: collapse;"">Số lượng</th>
										<th style=""text-align: -webkit-match-parent; border: 1px solid black; border-collapse: collapse;"">Đơn giá</th>
										<th style=""text-align: -webkit-match-parent; border: 1px solid black; border-collapse: collapse;"">Thành tiền</th>
									</tr>
								</thead>
								<tbody>
									
									<tr>
										<td style=""border: 1px solid black; border-collapse: collapse;"">2</td>
										<td style=""border: 1px solid black; border-collapse: collapse;"">Ti&ecirc;̀n đi&ecirc;̣n</td>
										<td style=""border: 1px solid black; border-collapse: collapse;"">0</td>
										<td style=""border: 1px solid black; border-collapse: collapse;"">4</td>
										<td style=""border: 1px solid black; border-collapse: collapse;"">4 (Kwh)</td>
										<td style=""text-align: right !important; border: 1px solid black; border-collapse: collapse; "">7.000&nbsp;₫</td>
										<td style=""text-align: right !important; border: 1px solid black; border-collapse: collapse;"">28.000&nbsp;₫</td>
									</tr>
									
									<tr>
										
										<td colspan=""7"" style="" text-align: right !important;border: 1px solid black; border-collapse: collapse;"">Tổng : 6550.000 ₫</td>
									</tr>
								</tbody>
							</table>
										
									
						
							<!-- END CTA -->
                            ");

            Html.Append(@$"<!-- Footer -->
							<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"">
								<tr>
									<td style=""padding: 50px;"" class=""p30-20"">
										<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"">
											
											<tr>
												<td>
													<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"">
														<tr>
															<th class=""column-top"" width=""370"" style=""font-size:0pt; line-height:0pt; padding:0; margin:0; font-weight:normal; vertical-align:top;"">
																<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"">
																	<tr>
																		<td class=""text-footer m-center"" style=""color:#666666; font-family:Arial, sans-serif; font-size:13px; line-height:18px; text-align:left; min-width:auto !important;"">
																		<multiline>
																		Đây là thư từ hệ thống quản lý phòng trọ, vui lòng không trả lời thư.
																		<br />
																		<br />
																		
																		</multiline>
																		</td>
																	</tr>
																</table>
															</th>
															<th style=""padding-bottom: 25px !important; font-size:0pt; line-height:0pt; padding:0; margin:0; font-weight:normal;"" class=""column"" width=""1""></th>
															
														</tr>
													</table>
												</td>
											</tr>
										</table>
									</td>
								</tr>
							</table>
							<!-- END Footer -->
                            ");

            Html.Append(@$"						</td>
											</tr>
										</table>
									</td>
								</tr>
							</table>
						</body>
						</html>
                            ");
            Html.Append(@$"
                            ");
            Html.Append(@$"
                            ");

            return Html.ToString();

        }

        public static string ConverterPayInvoiceToHtml(Contract contract, Invoice invoice)
        {
            StringBuilder Html = new StringBuilder();

            Html.Append(@$"
                            ");
            Html.Append(@$"
                            ");
            Html.Append(@$"
                            ");
            Html.Append(@$"
                            ");
            Html.Append(@$"
                            ");
            Html.Append(@$"
                            ");


            return Html.ToString();

        }

    }
}
