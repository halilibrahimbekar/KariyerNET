using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Queries.Request
{
    public class GetJobByIdRequest : IRequest<GetJobResponse>
    {
        public Guid Id { get; set; }
    }
}
