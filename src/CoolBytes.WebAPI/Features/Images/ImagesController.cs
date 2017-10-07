using System.Threading.Tasks;
using CoolBytes.WebAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoolBytes.WebAPI.Features.Images
{
    [Route("api/[controller]")]
    public class ImagesController : Controller
    {
        private readonly IMediator _mediator;

        public ImagesController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> Get(GetImagesQuery query) => this.OkOrNotFound(await _mediator.Send(query));

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm]UploadImagesCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _mediator.Send(command));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(DeleteImageCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _mediator.Send(command);

            return NoContent();
        }

    }
}
