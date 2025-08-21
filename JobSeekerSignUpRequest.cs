using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace API_FINAL_PROJECT.API.Jobseeker.RequestObjects
{
    public class JobSeekerSignUpRequest
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }
        

    }
}
