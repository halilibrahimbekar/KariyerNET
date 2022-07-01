using Data.Common.Enum;
using MediatR;
using System;
using System.Collections.Generic;

namespace Service.Services.Queries.Request
{
    public class GetJobListRequest : IRequest<List<GetJobResponse>>
    {
        public GetJobListRequest()
        {
            PageSize = 10;
            PageNumber = 1;
        }

        public SearchJobSortEnum Sort { get; set; } = SearchJobSortEnum.EndDateDesc;

        public Guid CompanyId { get; set; }

        public DateTime? MaxEndDate { get; set; }

        public DateTime? MinEndDate { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }
    }
}
