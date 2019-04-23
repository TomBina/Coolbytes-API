using AutoMapper;
using CoolBytes.Core.Builders;
using CoolBytes.Data;
using CoolBytes.WebAPI.Extensions;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.SwaggerGeneration.Processors.Security;

namespace CoolBytes.WebAPI
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _environment;
        private readonly ConnectionStringFactory _connectionStringFactory;

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
            _connectionStringFactory = new ConnectionStringFactory(configuration, environment);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddHttpContextAccessor();
            services.AddTransient<BlogPostBuilder>();
            services.AddTransient<ExistingBlogPostBuilder>();
            services.AddSecurity(_configuration);
            services.AddDbContextPool<AppDbContext>(o =>
            {
                var connectionString = _connectionStringFactory.Create();
                o.UseSqlServer(connectionString);
            });
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(o => o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddFluentValidation(config => config.RegisterValidatorsFromAssembly(typeof(Startup).Assembly));
            services.AddMediatR(typeof(Startup));
            services.AddSwaggerDocument(settings =>
            {
                settings.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT"));
                settings.DocumentProcessors.Add(new SecurityDefinitionAppender("JWT", new SwaggerSecurityScheme
                {
                    Type = SwaggerSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    Description = "Bearer token",
                    In = SwaggerSecurityApiKeyLocation.Header
                }));
            });

            services.ScanServices(_environment.EnvironmentName);
            services.AddMailgun();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUi3();

            if (env.IsDevelopment())
            {
                app.UseCors("DevPolicy");
                Mapper.AssertConfigurationIsValid();
            }
            else
            {
                app.UseCors("ProductionPolicy");
            }

            app.UseMvcWithDefaultRoute();
        }
    }
}
