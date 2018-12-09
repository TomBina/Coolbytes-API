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

        public ServiceProviderBuilder Add<T>(object implementation)
        {
            _mock.Setup(sp => sp.GetService(It.Is<Type>(t => t == typeof(T)))).Returns(implementation);

            return this;
        }

        public IServiceProvider Build()
            => _mock.Object;
    }
}