using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Model.User
{
    public class UserProfileModel
    {
        public string UserId { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "Họ tên không được bỏ trống")]
        [MaxLength(256, ErrorMessage = "Họ và tên có tối đa 256 ký tự"), MinLength(3, ErrorMessage = "họ tên tối thiểu 3 ký tự")]
        public string FullName { get; set; }


        [Required(ErrorMessage = "Ngày sinh không được bỏ trống")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được bỏ trống")]
        [MaxLength(10, ErrorMessage = "Số điện thoại có tối đa 10 ký tự"), MinLength(10, ErrorMessage = "Số điện thoại tối thiểu 10 ký tự")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được bỏ trống")]
        [MaxLength(256, ErrorMessage = "Địa chỉ có tối đa 256 ký tự"), MinLength(1, ErrorMessage = "Địa chỉ tối thiểu 1 ký tự")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Cccd không được bỏ trống")]
        [MaxLength(12, ErrorMessage = "Căn cước công dân có tối đa 12 ký tự"), MinLength(12, ErrorMessage = "Căn cước công dân tối thiểu 12 ký tự")]
        public string Ccccd { get; set; }
        public string AvatarUrl { get; set; }

    }
}
