using System.ComponentModel.DataAnnotations;

namespace WebApi.Model.Account
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Tên tài khoản không được bỏ trống")]
        public string? Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email không được bỏ trống")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được bỏ trống")]
        public string? Password { get; set; }
    }
}
