using System.ComponentModel.DataAnnotations;

namespace API_FINAL_PROJECT.API.Admin.RequestObjects
{
    public class ChangePasswordRequest
    {
        [Required]
        public string NewPassword { get; set; } = null!;
    }
}
