using System.ComponentModel.DataAnnotations;

namespace CafeVirtual.Pruebas.API.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
