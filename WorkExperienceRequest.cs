namespace API_FINAL_PROJECT.API.Jobseeker.RequestObjects
{
    public class WorkExperienceRequest
    {
        public Guid? Id { get; set; }
        public Guid JobSeekerProfileID { get; set; }
        public string JobTitle { get; set; } 
        public string CompanyName { get; set; } 
        public string Summary { get; set; } 
        public DateOnly? ServiceStart { get; set; }
        public DateOnly? ServiceEnd { get; set; }
    }
}
