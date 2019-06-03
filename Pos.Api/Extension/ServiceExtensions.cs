using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pos.Contracts;
using Pos.DataAccess;
using Pos.Utility;
using Swashbuckle.AspNetCore.Swagger;

namespace Pos.Api.Extension
{
    public static class ServiceExtensions
    {
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerHelper, LoggerHelper>();
        }

        public static void ConfigureInMemoryDatabase(this IServiceCollection services)
        {
            services.AddDbContext<PosDbContext>(options => options.UseInMemoryDatabase(databaseName: "PosDb"));
        }

        // ReSharper disable once InconsistentNaming
        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {

            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Pos API",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Gokcan Ustun",
                        Email = "gokcan.ustun@yandex.com"
                    }
                });

                c.AddSecurityDefinition("Bearer",
                    new ApiKeyScheme { In = "header",
                        Description = "Please get your user token from \"v1/users/login\" endpoint first. After that enter into field the word 'Bearer' following by space and JWT", 
                        Name = "Authorization", Type = "apiKey" });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                    { "Bearer", Enumerable.Empty<string>() },
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                    c.IncludeXmlComments(xmlPath);
            });
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .AllowAnyOrigin()
                        //.WithOrigins("http://www.something.com")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
        }
    }
}
