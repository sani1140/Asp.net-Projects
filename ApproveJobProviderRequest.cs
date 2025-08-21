namespace API_FINAL_PROJECT.API.Admin.RequestObjects
{
    public class ApproveJobProviderRequest
    {
        public Guid Id { get; set; }
        public bool Approve { get; set; }
    }
}