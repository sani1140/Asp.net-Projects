using System.ComponentModel.DataAnnotations;

namespace API_FINAL_PROJECT.API.Admin.RequestObjects
{
    public class AdminProfileRequest
    {
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        public string Phone { get; set; } = null!;
    }
}
