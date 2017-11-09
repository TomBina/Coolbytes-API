using System;
using System.Net.Http;
using AutoMapper;
using CoolBytes.Core.Builders;
using CoolBytes.Core.Factories;
using CoolBytes.Core.Interfaces;
using CoolBytes.Data;
using CoolBytes.WebAPI.Authorization;
using CoolBytes.WebAPI.Extensions;
using CoolBytes.WebAPI.Services;
using CoolBytes.WebAPI.Services.Mailer;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

            services.AddTransient<BlogPostBuilder>();
            services.AddTransient<ExistingBlogPostBuilder>();
            services.AddSingleton<HttpClient, HttpClient>();
            services.AddMailgun(_configuration);
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IAuthorSearchService, AuthorService>();
            services.AddScoped<IAuthorValidator, AuthorValidator>();
            services.AddScoped<IImageFactory, ImageFactory>();
            services.AddScoped<IImageFactoryOptions>(sp => new ImageFactoryOptions(_configuration["ImagesUploadPath"]));
            services.AddScoped<IImageFactoryValidator, ImageFactoryValidator>();
            services.AddScoped<ISendValidator, ThresholdValidator>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDbContextPool<AppDbContext>(o => o.UseSqlServer(_configuration.GetConnectionString("Default")));
            services.AddMvc()
                        .AddJsonOptions(o => o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                        .AddFluentValidation(config => config.RegisterValidatorsFromAssembly(typeof(Startup).Assembly));
            services.AddAutoMapper(typeof(Startup));
            services.AddMediatR(typeof(Startup));
        }

        private void ConfigureSecurity(IServiceCollection services)
        {
            services.AddCors(o =>
            {
                o.AddPolicy("ProductionPolicy", builder => builder.WithOrigins("http://coolbytes.io", "http://www.coolbytes.io").AllowAnyMethod().AllowAnyHeader().Build());
                o.AddPolicy("DevPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().Build());
            });

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
            app.UseStaticFiles();

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
