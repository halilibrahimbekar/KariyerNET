using System.Collections.Generic;

namespace Service.Services.Search.SearchModel
{
    public class JobSearchResponse
    {
        public long Count { get; set; }
        public IEnumerable<JobIndex> Documents { get; set; }
    }
}
