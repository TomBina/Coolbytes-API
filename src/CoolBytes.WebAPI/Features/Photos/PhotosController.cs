using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolBytes.WebAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace CoolBytes.WebAPI.Features.Photos
{
    [Route("api/[controller]")]
    public class PhotosController : Controller
    {
        private readonly IMediator _mediator;

        public PhotosController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> Get(GetPhotosQuery query) => this.OkOrNotFound(await _mediator.Send(query));

        [HttpPost]
        public async Task<IActionResult> UploadPhoto([FromForm]UploadPhotosCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _mediator.Send(command));
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePhoto(DeletePhotoCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _mediator.Send(command);

            return NoContent();
        }

    }
}
