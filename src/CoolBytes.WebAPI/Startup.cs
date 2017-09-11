using AutoMapper;
using CoolBytes.Data;
using FluentValidation.AspNetCore;
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

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddDbContextPool<AppDbContext>(o => o.UseSqlServer(_configuration.GetConnectionString("Default")));
            services.AddMvc()
                        .AddJsonOptions(o => o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                        .AddFluentValidation(config => config.RegisterValidatorsFromAssembly(typeof(Startup).Assembly));
            services.AddAutoMapper(typeof(Startup));
            services.AddMediatR(typeof(Startup));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(o => o.AllowAnyOrigin());

                Mapper.AssertConfigurationIsValid();
            }

            app.UseMvcWithDefaultRoute();
        }
    }
}
