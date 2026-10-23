using System.ComponentModel.DataAnnotations;

namespace StdManagment.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required, StringLength(20)]
        public string StudentId { get; set; } = string.Empty;   // e.g. "STD-2026-001"

        [Required, StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(15)]
        public string Phone { get; set; } = string.Empty;       // stored as 03XXXXXXXXX

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        // "Student" or "Admin"
        [Required, StringLength(20)]
        public string Role { get; set; } = "Student";

        // true while the student is logged in, false after logout
        public bool IsActive { get; set; } = false;

        public DateTime? LastLoginAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}