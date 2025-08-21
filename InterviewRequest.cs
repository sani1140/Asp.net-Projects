using Domain.Enums;

namespace API_FINAL_PROJECT.API.JobProvider.RequestObjects
{
    public class InterviewRequest
    {
        public DateOnly ScheduledOn { get; set; }
        
        public string Location { get; set; } 
    }
}
