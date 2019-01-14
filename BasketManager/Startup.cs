
using System;
using System.Text;

using BasketManager.Auth;
using BasketManager.Models;
using BasketManager.Services;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace BasketManager
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        const string SecretKey = "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH"; // todo: get this from somewhere secure
        readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IJwtFactory, JwtFactory>();

            services.AddSwaggerDocument();

            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            services.Configure<JwtIssuerOptions>(
                options =>
                    {
                        options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                        options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                        options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
                    });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddJwtBearer(
                options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],
                            ValidateAudience = true,
                            ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = _signingKey,
                            RequireExpirationTime = true,
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.FromMinutes(120)
                        };
                    });

            services.AddAuthorization(
                options => options.AddPolicy("ApiUser", policy => policy.RequireClaim("rol", "api_access")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUi3();

                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            var corsSection = Configuration.GetSection("Cors");
            var angular = corsSection.GetSection("Client");
            app.UseCors(
                options => options.WithOrigins(angular.Value).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
