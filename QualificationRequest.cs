namespace API_FINAL_PROJECT.API.Jobseeker.RequestObjects
{
    public class QualificationRequest
    {
        public Guid? Id { get; set; }
        public Guid JobSeekerProfileId { get; set; }
        public string CourseName { get; set; }
        public string Institution { get; set; }
        public int Year { get; set; }
    }
}
