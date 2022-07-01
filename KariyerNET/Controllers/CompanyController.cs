using MediatR;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Commands.Request;
using Service.Services.Commands.Response;
using System.Threading.Tasks;

namespace KariyerNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        IMediator _mediator;

        public CompanyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: api/Company
        [HttpPost]
        public async Task<ActionResult<AddCompanyResponse>> PostCompany(AddCompanyRequest company)
        {
            var result = await _mediator.Send(company);

            return Ok(result);
        }
    }
}
