using System.Linq;
using NSwag;
using NSwag.AspNetCore;
using NSwag.SwaggerGeneration.Processors.Security;

namespace CoolBytes.WebAPI
{
    public class SwaggerConfiguration
    {
        public void ConfigureSwagger(SwaggerDocumentSettings settings)
        {
            settings.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT"));
            settings.DocumentProcessors.Add(new SecurityDefinitionAppender("JWT", Enumerable.Empty<string>(), new SwaggerSecurityScheme {Type = SwaggerSecuritySchemeType.ApiKey, Name = "Authorization", Description = "Bearer token", In = SwaggerSecurityApiKeyLocation.Header}));
        }
    }
}