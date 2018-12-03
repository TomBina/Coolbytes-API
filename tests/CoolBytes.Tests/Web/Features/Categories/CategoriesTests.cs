using CoolBytes.WebAPI.Features.Categories.ViewModels;
using CoolBytes.WebAPI.Utils;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CoolBytes.Tests.Web.Features.Categories
{
    public class CategoriesTests : TestBase
    {
        public CategoriesTests(TestContext testContext) : base(testContext)
        {

        }


        [Fact]
        public void GetAllCategoriesHandler_Returns_Categories()
        {
            var message = new GetAllCategoriesQuery();
            var handler = new GetAllCategoriesHandler();
        }
    }

    public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, Result<IEnumerable<CategoryViewModel>>>
    {
        public Task<Result<IEnumerable<CategoryViewModel>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class GetAllCategoriesQuery : IRequest<Result<IEnumerable<CategoryViewModel>>>
    {
    }
}
