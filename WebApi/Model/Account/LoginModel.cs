using System.ComponentModel.DataAnnotations;

namespace WebApi.Model.Account
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Tên tài khoản không được bỏ trống")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được bỏ trống")]
        public string? Password { get; set; }

    }
}
