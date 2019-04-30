using Microsoft.Extensions.DependencyInjection;
using System;

namespace CoolBytes.Tests.Web
{
    public class ServiceProviderBuilder
    {
        private ServiceCollection _services;

        public ServiceProviderBuilder()
        {
            _services = new ServiceCollection();
        }

        public ServiceProviderBuilder Add(Action<IServiceCollection> services)
        {
            services(_services);

            return this;
        }

        public IServiceProvider Build()
            => _services.BuildServiceProvider();
    }
}