namespace API_FINAL_PROJECT.API.Jobseeker.RequestObjects
{
    public class UploadResumeRequest
    {
        public string Title { get; set; } = null!;
        public IFormFile File { get; set; } = null!;
        public Guid JobSeekerProfileID { get; set; }
    }
}
