using Microsoft.AspNetCore.Http;
using Moq;
using System;

namespace CoolBytes.Tests.Web
{
    public class ServiceProviderBuilder
    {
        private readonly Mock<IServiceProvider> _mock;

        public ServiceProviderBuilder()
        {
            _mock = new Mock<IServiceProvider>();
        }

        public ServiceProviderBuilder AddHttpContextAccessor(Action<HttpContext> configure = null)
        {
            var accessor = new Mock<IHttpContextAccessor>();
            var httpContext = new DefaultHttpContext();
            accessor.Setup(h => h.HttpContext).Returns(httpContext);

            configure?.Invoke(httpContext);

            Add<IHttpContextAccessor>(accessor.Object);

            return this;
        }

        public ServiceProviderBuilder Add<T>(object implementation)
        {
            _mock.Setup(sp => sp.GetService(It.Is<Type>(t => t == typeof(T)))).Returns(implementation);

            return this;
        }

        public IServiceProvider Build()
            => _mock.Object;
    }
}