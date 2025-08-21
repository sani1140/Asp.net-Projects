using System.ComponentModel.DataAnnotations;

namespace API_FINAL_PROJECT.API.Admin.RequestObjects
{
    public class AdminLoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
