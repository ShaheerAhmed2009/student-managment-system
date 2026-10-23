using System.ComponentModel.DataAnnotations;

namespace StdManagment.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Enter your email or Student ID")]
        [Display(Name = "Email or Student ID")]
        public string Identifier { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}