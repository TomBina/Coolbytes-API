using AutoMapper;
using CoolBytes.Core.Builders;
using CoolBytes.Data;
using CoolBytes.WebAPI.Extensions;
using CoolBytes.WebAPI.Handlers;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoolBytes.WebAPI
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _environment;
        private readonly ConnectionStringFactory _connectionStringFactory;
        private readonly SwaggerConfiguration _swaggerConfiguration;

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
            _connectionStringFactory = new ConnectionStringFactory(configuration, environment);
            _swaggerConfiguration = new SwaggerConfiguration();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddHttpContextAccessor();
            services.AddTransient<BlogPostBuilder>();
            services.AddTransient<ExistingBlogPostBuilder>();
            services.AddSecurity(_configuration, _environment);
            services.AddDbContextPool<AppDbContext>(o =>
            {
                var connectionString = _connectionStringFactory.Create();
                o.UseSqlServer(connectionString);
            });
            services.AddConfiguredMvc();
            services.AddAutoMapper(typeof(Program));
            services.AddMediatR(typeof(Startup));
            services.AddSwaggerDocument(_swaggerConfiguration.ConfigureSwagger);
            services.ScanServices(_environment.EnvironmentName);
            services.AddMailgun();
            services.AddTransient(typeof(HandlerContext<>));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMapper mapper)
        {
            if (env.IsDevelopment())
            {
                app.UseCors("DevPolicy");
                mapper.ConfigurationProvider.AssertConfigurationIsValid();
            }
            else
            {
                app.UseCors("ProductionPolicy");
            }

            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUi3();
            app.UseMvcWithDefaultRoute();
        }
    }
}
