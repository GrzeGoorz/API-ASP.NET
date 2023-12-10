

using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto
    {
        
        [Required(ErrorMessage = "Nazwa użytkownika jest wymagana.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Hasło jest wymagane.")]
        public string Password { get; set; }
    }
}