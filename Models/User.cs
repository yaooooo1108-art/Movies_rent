using System.ComponentModel.DataAnnotations;

namespace _1131754.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string Role { get; set; } // "Admin" 或 "User"

    }
}
