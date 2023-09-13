using System.ComponentModel.DataAnnotations;

namespace WebApi.Model.Account
{
    public class RegisterLandlordModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Cccd is required")]
        [MaxLength(12,ErrorMessage ="Căn cước công dân có tối đa 12 ký tự"),MinLength(12, ErrorMessage = "Căn cước công dân tối thiểu 12 ký tự")]
        public string? Cccd { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
