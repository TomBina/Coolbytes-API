using AutoMapper;
using CoolBytes.Core.Attributes;
using CoolBytes.Core.Builders;
using CoolBytes.Core.Factories;
using CoolBytes.Core.Interfaces;
using CoolBytes.Data;
using CoolBytes.Services.Caching;
using CoolBytes.WebAPI.Authorization;
using CoolBytes.WebAPI.Extensions;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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

            services.AddHttpClient();
            services.AddHttpContextAccessor();
            services.AddTransient<BlogPostBuilder>();
            services.AddTransient<ExistingBlogPostBuilder>();

            services.Scan(s =>
                s.FromAssemblyOf<ICacheService>()
                    .AddClasses(c => c.WithAttribute<ScopedAttribute>())
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

            services.Scan(s =>
                s.FromAssemblyOf<IImageFactory>()
                    .AddClasses(c => c.WithAttribute<ScopedAttribute>())
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

            services.AddScoped<IImageFactoryOptions>(sp => new ImageFactoryOptions(_configuration["ImagesUploadPath"]));

            services.AddDbContextPool<AppDbContext>(o => o.UseSqlServer(_configuration.GetConnectionString("Default")));
            services.AddMvc()
                        .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                        .AddJsonOptions(o => o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                        .AddFluentValidation(config => config.RegisterValidatorsFromAssembly(typeof(Startup).Assembly));
            services.AddMediatR(typeof(Startup));
            services.AddSwaggerDocument();
            services.AddMailgun();
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
