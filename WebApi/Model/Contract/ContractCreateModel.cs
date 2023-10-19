


using Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Model.Contract
{
    public class ContractCreateModel
    {
        [Required(ErrorMessage = "Người cho thuê không được bỏ trống")]
        public string A_Lessor { get; set; }

        [Required(ErrorMessage = "Ngày sinh không được bỏ trống")]
        public DateTime A_DateOfBirth { get; set; }

        [Required(ErrorMessage = "Căn cước công dân không được bỏ trống")]
        public string A_Cccd { get; set; }

        [Required(ErrorMessage = "Ngày cấp không được bỏ trống")]
        public DateTime A_DateOfIssuance { get; set; }

        [Required(ErrorMessage = "Nơi cấp không được bỏ trống")]
        public string A_PlaceOfIssuance { get; set; }

        [Required(ErrorMessage = "Địa chỉ thường trú không được bỏ trống")]
        public string A_PermanentAddress { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được bỏ trống")]
        public string A_Phone { get; set; }

        [Required(ErrorMessage = "Tên người thuê không được bỏ trống")]
        public string B_Lessee { get; set; }

        [Required(ErrorMessage = "Ngày sinh không được bỏ trống")]
        public DateTime B_DateOfBirth { get; set; }

        [Required(ErrorMessage = "Căn cước công dân không được bỏ trống")]
        public string B_Cccd { get; set; }

        [Required(ErrorMessage = "Ngày cấp không được bỏ trống")]
        public DateTime B_DateOfIssuance { get; set; }

        [Required(ErrorMessage = "Nơi cấp không được bỏ trống")]
        public string B_PlaceOfIssuance { get; set; }

        [Required(ErrorMessage = "Địa chỉ thường trú không được bỏ trống")]
        public string B_PermanentAddress { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được bỏ trống")]
        public string B_Phone { get; set; }

        [Required(ErrorMessage = "Giá thuê không được bỏ trống")]
        public decimal RentalPrice { get; set; }
        [Required(ErrorMessage = "Thời gian thuê không được bỏ trống")]
        public int DurationOfHouseLease { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu không được bỏ trống")]
        public DateTime CommencingOn { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc không được bỏ trống")]
        public DateTime EndingOn { get; set; }

        [Required(ErrorMessage = "Nhà trọ không được bỏ trống")]
        public int BranchId { get; set; }
        [Required(ErrorMessage = "Địa chỉ không được bỏ trống")]
        public int AreaId { get; set; }
        [Required(ErrorMessage = "Phòng không được bỏ trống")]
        public int RoomId { get; set; }
        [Required(ErrorMessage = "Tiền cọc được bỏ trống")]
        public decimal Deposit { get; set; }
		[Required(ErrorMessage = "Giá điện được bỏ trống")]
		public decimal ElectricityCosts { get; set; }
		[Required(ErrorMessage = "Giá nước được bỏ trống")]
		public decimal WaterCosts { get; set; }


		public string TermsOfContract { get; set; }

    }
}
