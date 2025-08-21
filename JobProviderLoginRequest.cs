using System.ComponentModel.DataAnnotations;

namespace API_FINAL_PROJECT.API.JobProvider.RequestObjects
{
    public class JobProviderLoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } 
        [Required]
        public string Password { get; set; } 
    }
}
