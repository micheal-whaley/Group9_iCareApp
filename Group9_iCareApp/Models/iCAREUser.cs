using System.ComponentModel.DataAnnotations;

namespace Group9_iCareApp.Models
{
    public class iCAREUser
    {
        public int Id { get; set; }
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; } // ? allows for null value
    }
}
