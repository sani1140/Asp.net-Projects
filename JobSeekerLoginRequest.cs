using System.ComponentModel.DataAnnotations;

namespace API_FINAL_PROJECT.API.Jobseeker.RequestObjects
{
    public class JobSeekerLoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; } 
    }
}
