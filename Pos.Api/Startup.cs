﻿using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using Pos.Api.Extension;
using Pos.Api.Middleware;
using Pos.Utility;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using Pos.Api.Filters;
using Pos.Api.Infrastructure;
using Pos.Api.Options;
using Pos.BusinessLogic;
using Pos.BusinessLogic.Interface;
using Pos.Core.Enum;
using Pos.Core.Interface;
using Pos.DataAccess;
using Pos.DataAccess.Entities;

namespace Pos.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            var logConfigPath = string.Concat(Directory.GetCurrentDirectory(), "/nlog.config");
#pragma warning disable CS0618 // Type or member is obsolete
            loggerFactory.ConfigureNLog(logConfigPath);
#pragma warning restore CS0618 // Type or member is obsolete
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private const string Secretkey = "SECRETKEYGREATERTHAN128BITS";
        private const string Policyname = "ApiPolicy";

        private readonly SymmetricSecurityKey
            _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secretkey));

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors();
            services.ConfigureIISIntegration();
            services.ConfigureLoggerService();
            services.ConfigureInMemoryDatabase();
            services.ConfigureSwagger();
            services.AddSingleton(new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile(new CryptoHelper()));
            }).CreateMapper());
            services.AddSingleton<ICryptoHelper, CryptoHelper>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IProductManager, ProductManager>();
            services.AddScoped<IOrderManager, OrderManager>();
            services.AddScoped<ICampaignManager, CampaignManager>();

            var jsonSerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            JsonConvert.DefaultSettings = () => jsonSerializerSettings;

            services.AddOptions();

            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

            services.AddMvc(config =>
                    {
                        var policy = new AuthorizationPolicyBuilder()
                            .RequireAuthenticatedUser()
                            .Build();
                        config.Filters.Add(new AuthorizeFilter(policy));
                        config.Filters.Add(typeof(ValidateModelStateAttribute));
                    }
                ).AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    opt.SerializerSettings.Formatting = Formatting.Indented;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddAuthorization(options => { options.DefaultPolicy = options.GetPolicy(Policyname); });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o => o.TokenValidationParameters = tokenValidationParameters);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseCors("CorsPolicy");

            //app.UseForwardedHeaders will forward proxy headers to the current request. For Linux deployment.
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMiddleware(typeof(ErrorHandlerMiddleware));

            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pos API V1"); });

            app.UseMvc();

            SeedDatabase(app);
        }

        private static void SeedDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            using (var db = serviceScope.ServiceProvider.GetService<PosDbContext>())
            {
                var hasher = serviceScope.ServiceProvider.GetService<ICryptoHelper>();
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                db.Users.Add(new User
                {
                    Id = Guid.Parse("523755D5-E791-4FEA-B4EC-412E123E66F4"),
                    FirstName = "Gokcan",
                    LastName = "Ustun",
                    Email = "gokcan.ustun@yandex.com",
                    UserName = "admin",
                    Password = hasher.Hash("1"),
                });
                db.Products.Add(new Product
                {
                    Id = Guid.NewGuid(),
                    Code = "M001",
                    Name = "T-Shirt",
                    Price = 49.99.ToDecimal()
                });
                db.Products.Add(new Product
                {
                    Id = Guid.NewGuid(),
                    Code = "M002",
                    Name = "Jean",
                    Price = 149.99.ToDecimal()
                });
                db.Products.Add(new Product
                {
                    Id = Guid.NewGuid(),
                    Code = "M003",
                    Name = "Skirt",
                    Price = 79.99.ToDecimal()
                });

                db.Campaigns.Add(new Campaign
                {
                    Id = Guid.NewGuid(),
                    Code = "CMP01",
                    Name = "%10 Discount",
                    MaxUsageCount = 3,
                    UsageCount = 2,
                    DiscounType = EDiscountType.Ratio,
                    DiscountValue = 10
                });
                db.Campaigns.Add(new Campaign
                {
                    Id = Guid.NewGuid(),
                    Code = "CMP02",
                    Name = "5$ Discount",
                    DiscounType = EDiscountType.Amount,
                    DiscountValue = 5
                });

                db.SaveChanges();
            }
        }
    }
}
