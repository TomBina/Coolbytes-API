using CoolBytes.Core.Utils;
using CoolBytes.WebAPI.Features.Categories.CQ;
using CoolBytes.WebAPI.Features.Categories.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Internal.Account;
using NotFoundResult = CoolBytes.Core.Utils.NotFoundResult;

namespace CoolBytes.WebAPI.Features.Categories
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<CategoryViewModel>>> GetAll()
        {
            var message = new GetAllCategoriesQuery();
            var result = await _mediator.Send(message);

            if (!result)
                return NotFound();

            return result.Payload.ToList();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CategoryViewModel>> Get(int id)
        {
            var message = new GetCategoryQuery() { Id = id };
            var result = await _mediator.Send(message);

            if (!result)
                return NotFound();

            return result.Payload;
        }

        [Authorize("admin")]
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Add(AddCategoryCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound();

            return Ok();
        }

        [Authorize("admin")]
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Update(UpdateCategoryCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound();

            return Ok();
        }

        [Authorize("admin")]
        [HttpPut("sort")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Sort(SortCategoriesCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result)
                return BadRequest();

            return Ok();
        }

        [Authorize("admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400, Type = typeof(ErrorResult))]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteCategoryCommand() { Id = id };
            var result = await _mediator.Send(command);

            if (!result)
            {
                if (result.Is<NotFoundResult>())
                    return NotFound();

                var errorResult = result.As<ErrorResult>();
                return BadRequest(errorResult.ErrorMessage);
            }

            return NoContent();
        }
    }
}
