using CoolBytes.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Xunit;

namespace CoolBytes.Tests.Web.Extensions
{
    public class ControllerExtensionsTests
    {
        [Fact]
        public void OkOrNotFound_WithObject_ReturnsOkObjectResult()
        {
            var testController = new TestController();
            var testObject = new object();

            var result = testController.OkOrNotFound(testObject);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void OkOrNotFound_WithObjectNull_ReturnsNotFoundResult()
        {
            var testController = new TestController();

            var result = testController.OkOrNotFound(null);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void OkOrNotFound_WithIEnumberable_ReturnsOkObjectResult()
        {
            var testController = new TestController();
            var testObject = new[] { 1, 2, 3 };

            var result = testController.OkOrNotFound(testObject);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void OkOrNotFound_WithIEnumberableEmpty_ReturnsNotFoundResult()
        {
            var testController = new TestController();
            var testObject = Enumerable.Empty<string>();

            var result = testController.OkOrNotFound(testObject);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
