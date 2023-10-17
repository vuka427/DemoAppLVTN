using Domain.Entities;
using System.Text;

namespace WebApi.Services.ContractToPdf
{
	public class ContractToPdf
	{

		public string ConverterToHtml(Contract contract)
		{
			StringBuilder Html = new StringBuilder();


			Html.Append(@"<p style='margin-top:0cm;margin-right:0cm;margin-bottom:6.0pt;margin-left:0cm;font-size:11.0pt;font-family:""Calibri"",sans-serif;text-align:center;line-height:18.75pt;background:white;vertical-align:baseline;'><strong><span style='font-size:26px;font-family:""Times New Roman"",serif;color:black;border:none windowtext 1.0pt;padding:0cm;'>CỘNG XÃ HỘI CHỦ NGHĨA VIỆT NAM</span></strong><span style='font-size:19px;font-family:""Times New Roman"",serif;color:black;'><br>&nbsp;</span><strong><u><span style='font-size:22px;font-family:""Times New Roman"",serif;color:black;'>Độc lập - Tự do - Hạnh phúc</span></u></strong></p>
                            <p style='margin-top:0cm;margin-right:0cm;margin-bottom:6.0pt;margin-left:0cm;font-size:11.0pt;font-family:""Calibri"",sans-serif;text-align:center;line-height:18.75pt;background:white;vertical-align:baseline;'><strong><span style='font-size:29px;font-family:""Times New Roman"",serif;color:black;border:none windowtext 1.0pt;padding:0cm;'>&nbsp;</span></strong></p>");

			Html.Append(@$" <p style='margin-top:0cm;margin-right:0cm;margin-bottom:6.0pt;margin-left:0cm;font-size:11.0pt;font-family:""Calibri"",sans-serif;text-align:center;line-height:18.75pt;background:white;vertical-align:baseline;'><strong>
							<span style='font-size:29px;font-family:""Times New Roman"",serif;color:black;border:none windowtext 1.0pt;padding:0cm;'>HỢP ĐỒNG THUÊ NHÀ TRỌ</span></strong>
							<span style='font-size:19px;font-family:""Times New Roman"",serif;color:black;'><br>&nbsp;</span><em>
							<span style='font-size:22px;font-family:""Times New Roman"",serif;color:black;border:none windowtext 1.0pt;padding:0cm;'>(Số: {contract.Id}/HĐTNT)</span></em></p> ");

			Html.Append($@" <p style='margin-top:0cm;margin-right:0cm;margin-bottom:6.0pt;margin-left:0cm;font-size:11.0pt;font-family:""Calibri"",sans-serif;text-align:justify;line-height:18.75pt;background:white;vertical-align:baseline;'><strong><span style='font-size:27px;font-family:""Times New Roman"",serif;color:black;border:none windowtext 1.0pt;padding:0cm;'>BÊN CHO THUÊ (BÊN A):</span></strong></p>
                            <p style='margin-top:0cm;margin-right:0cm;margin-bottom:6.0pt;margin-left:0cm;font-size:11.0pt;font-family:""Calibri"",sans-serif;text-align:justify;line-height:18.75pt;background:white;vertical-align:baseline;'><span style='font-size:27px;font-family:""Times New Roman"",serif;color:black;border:none windowtext 1.0pt;padding:0cm;'>Ông/bà: {contract.A_Lessor??""}. Ngày sinh: {contract.A_DateOfBirth.ToShortTimeString()}.</span></p>
                            <p style='margin-top:0cm;margin-right:0cm;margin-bottom:6.0pt;margin-left:0cm;font-size:11.0pt;font-family:""Calibri"",sans-serif;text-align:justify;line-height:18.75pt;background:white;vertical-align:baseline;'><span style='font-size:27px;font-family:""Times New Roman"",serif;color:black;border:none windowtext 1.0pt;padding:0cm;'>CMND/CCCD số: {contract.A_Cccd} Ngày cấp {contract.A_DateOfIssuance}. Nơi cấp {contract.A_PlaceOfIssuance}</span></p>
                            <p style='margin-top:0cm;margin-right:0cm;margin-bottom:6.0pt;margin-left:0cm;font-size:11.0pt;font-family:""Calibri"",sans-serif;text-align:justify;line-height:18.75pt;background:white;vertical-align:baseline;'><span style='font-size:27px;font-family:""Times New Roman"",serif;color:black;border:none windowtext 1.0pt;padding:0cm;'>Địa chỉ thường trú:…………………………………………..………………………………</span></p>
                            <p style='margin-top:0cm;margin-right:0cm;margin-bottom:6.0pt;margin-left:0cm;font-size:11.0pt;font-family:""Calibri"",sans-serif;text-align:justify;line-height:18.75pt;background:white;vertical-align:baseline;'><span style='font-size:27px;font-family:""Times New Roman"",serif;color:black;border:none windowtext 1.0pt;padding:0cm;'>Điện thoại: …………………………………………..…………………………</span></p>
                            <p style='margin-top:0cm;margin-right:0cm;margin-bottom:6.0pt;margin-left:0cm;font-size:11.0pt;font-family:""Calibri"",sans-serif;text-align:justify;line-height:18.75pt;background:white;vertical-align:baseline;'><span style='font-size:27px;font-family:""Times New Roman"",serif;color:black;border:none windowtext 1.0pt;padding:0cm;'>Là chủ sở hữu nhà ở: …………………………………………..……………</span></p>");


			return "";
		}

	}
}
