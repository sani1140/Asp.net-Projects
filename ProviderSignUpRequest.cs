using System.ComponentModel.DataAnnotations;

namespace API_FINAL_PROJECT.API.JobProvider.RequestObjects
{
    public class ProviderSignUpRequest
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        [Required]
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }

        [Required]
        public string CompanyLegalName { get; set; }

        [Required]
        public string Industry { get; set; }

        public string? Summary { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Website { get; set; }

        public string? FullName { get; set; }


    }
}
