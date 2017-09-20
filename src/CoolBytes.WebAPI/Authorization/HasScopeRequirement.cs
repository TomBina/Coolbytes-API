using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace CoolBytes.WebAPI.Authorization
{
    public class HasScopeRequirement : AuthorizationHandler<HasScopeRequirement>, IAuthorizationRequirement
    {
        private readonly string _issuer;
        private readonly string _scope;

        public HasScopeRequirement(string scope, string issuer)
        {
            this._scope = scope;
            this._issuer = issuer;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "scope" && c.Issuer == _issuer))
                return Task.CompletedTask;

            var scopes = context.User.FindFirst(c => c.Type == "scope" && c.Issuer == _issuer).Value.Split(' ');

            if (scopes.Any(s => s == _scope))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
