using System.ComponentModel.DataAnnotations;

namespace StdManagment.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be 3 to 100 characters")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        // Accepts: 03001234567, +923001234567, 923001234567
        // Mobile prefixes 300-349 only (Jazz, Zong, Telenor, Ufone, SCOM)
        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^(?:\+92|92|0)3[0-4]\d{8}$",
            ErrorMessage = "Enter a valid Pakistani mobile number (e.g. 03001234567 or +923001234567)")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Student ID is required")]
        [RegularExpression(@"^[A-Za-z0-9\-]{4,20}$",
            ErrorMessage = "ID must be 4 to 20 letters, numbers or dashes")]
        [Display(Name = "Student ID")]
        public string StudentId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).+$",
            ErrorMessage = "Password must contain at least one letter and one number")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}