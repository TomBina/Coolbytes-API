using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolBytes.WebAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoolBytes.WebAPI.Features.Authors
{
    [Authorize("admin")]
    [Route("api/[controller]")]
    public class AuthorsController : Controller
    {
        private readonly IMediator _mediator;

        public AuthorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(GetAuthorQuery query) =>
            this.OkOrNotFound(await _mediator.Send(query));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddAuthorCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                return Ok(await _mediator.Send(command));
            }
            catch (InvalidOperationException)
            {
                return BadRequest("Operation not allowed, only one author can be created per user.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAuthorCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _mediator.Send(command));
        }
    }
}