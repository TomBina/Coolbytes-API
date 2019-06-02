using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace CoolBytes.WebAPI.Authorization
{
    public class HasScopeRequirement : AuthorizationHandler<HasScopeRequirement>, IAuthorizationRequirement
    {
        private readonly string _issuer;
        private readonly IHostingEnvironment _environment;
        private readonly string _scope;

        public HasScopeRequirement(string scope, string issuer, IHostingEnvironment environment)
        {
            _scope = scope;
            _issuer = issuer;
            _environment = environment;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
        {
            var hasClaim = context.User.HasClaim(c => c.Type == "scope" && c.Issuer == _issuer);

            if (!hasClaim)
            {
                if (_environment.IsDevelopment())
                    context.Succeed(requirement);

                return Task.CompletedTask;
            }

            var scopes = context.User.FindFirst(c => c.Type == "scope" && c.Issuer == _issuer).Value.Split(' ');

            if (scopes.Any(s => s == _scope) || _environment.IsDevelopment())
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
