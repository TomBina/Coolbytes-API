using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System;

namespace CoolBytes.Core.Attributes
{
    public class InjectAttribute : ServiceDescriptorAttribute
    {
        public string[] Environment { get; }

        public InjectAttribute(Type serviceType) : base(serviceType, ServiceLifetime.Scoped) 
            => Environment = new string[0];

        public InjectAttribute(Type serviceType, ServiceLifetime lifetime, params string[] environment) : base(serviceType, lifetime) 
            => Environment = environment;
    }
}
