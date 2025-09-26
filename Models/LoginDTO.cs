using System.ComponentModel.DataAnnotations;

namespace ECPAPI.Models
{
    public class LoginDTO
    {
        [Required]
        public string UserNameOrEmail { get; set; }
        [Required]
        public string Password { get; set; } 
    }
}
