using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolBytes.WebAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoolBytes.WebAPI.Features.Resume
{
    [Route("api/[controller]")]
    public class ResumeController : Controller
    {
        private readonly IMediator _mediator;

        public ResumeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{authorid}")]
        public async Task<IActionResult> Get(GetResumeQuery message) 
            => this.OkOrNotFound(await _mediator.Send(message));
    }
}
