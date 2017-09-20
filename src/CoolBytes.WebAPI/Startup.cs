using AutoMapper;
using CoolBytes.Data;
using CoolBytes.WebAPI.Authorization;
using CoolBytes.WebAPI.Services;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
            ConfigureSecurity(services);

            services.AddScoped<IUserService, UserService>();
            services.AddDbContextPool<AppDbContext>(o => o.UseSqlServer(_configuration.GetConnectionString("Default")));
            services.AddMvc()
                        .AddJsonOptions(o => o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                        .AddFluentValidation(config => config.RegisterValidatorsFromAssembly(typeof(Startup).Assembly));
            services.AddAutoMapper(typeof(Startup));
            services.AddMediatR(typeof(Startup));
        }

        private void ConfigureSecurity(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("DevPolicy", builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().Build(); }));
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.Authority = _configuration["Auth0:Domain"];
                o.Audience = _configuration["Auth0:ApiIdentifier"];
            });

            services.AddAuthorization(o =>
            {
                o.AddPolicy("admin",
                    policy => policy.Requirements.Add(new HasScopeRequirement("admin", _configuration["Auth0:Domain"])));
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("DevPolicy");

                Mapper.AssertConfigurationIsValid();
            }

            app.UseMvcWithDefaultRoute();
        }
    }
}
