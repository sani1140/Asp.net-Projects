namespace API_FINAL_PROJECT.API.JobProvider.RequestObjects
{
    public class SearchRequest
    {
        public Guid? JobseekerID { get; set; }

        public string Qualifications { get; set; }
        public DateOnly? year { get; set; }
    }
}
