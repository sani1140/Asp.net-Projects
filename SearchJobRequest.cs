using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.JobSeeker.DTO_s
{
    public class SearchJobRequest
    {
        public string? Title { get; set; }
        public string? Location { get; set; }
        public string? CompanyName { get; set; }
    }
}
