using Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_FINAL_PROJECT.API.JobProvider.RequestObjects
{
    public class PostJobRequest
    {
        public string JobTitle { get; set; } = null!;
        public string JobSummary { get; set; } = null!;
        public string JobLocation { get; set; } = null!;
        public Guid JobProviderCompanyId { get; set; }
        public Guid JobCategoryId { get; set; }
        public string Qualification { get; set; } = null!;
        public string Industry { get; set; } = null!;

     
        public List<string> Skill { get; set; } = new();
        public List<string> Responsibility{ get; set; } = new();
    }
}
