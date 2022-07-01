using MediatR;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Commands.Request;
using Service.Services.Commands.Response;
using Service.Services.Queries.Request;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KariyerNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        IMediator _mediator;

        public JobController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<GetJobResponse>>> GetJob([FromQuery] GetJobListRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        // GET: api/Job/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetJobResponse>> GetJob(Guid id)
        {
            var job = await _mediator.Send(new GetJobByIdRequest { Id = id });

            if (job == null)
            {
                return NotFound();
            }

            return Ok(job);
        }

        // POST: api/Job
        [HttpPost]
        public async Task<ActionResult<AddJobResponse>> PostJob(AddJobRequest job)
        {
            var result = await _mediator.Send(job);

            return Ok(result);
        }

        // POST: api/CreateIndex
        [HttpPost("createIndex")]
        public async Task<ActionResult<CreateIndexResponse>> CreateIndex()
        {
            var result = await _mediator.Send(new CreateIndexRequest());

            return Ok(result);
        }
    }
}
