using System.ComponentModel.DataAnnotations;

namespace Backend.Model
{
    public class Users
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        // Add more user properties as needed
    }
}