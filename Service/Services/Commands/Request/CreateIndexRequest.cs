using MediatR;
using Service.Services.Commands.Response;

namespace Service.Services.Commands.Request
{
    public class CreateIndexRequest : IRequest<CreateIndexResponse>
    {
    }
}
