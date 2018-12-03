using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoolBytes.WebAPI.Features.Categories
{
    [ApiController]
    public class CategoriesController : Controller
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public void GetAll()
        {

        }
    }
}
