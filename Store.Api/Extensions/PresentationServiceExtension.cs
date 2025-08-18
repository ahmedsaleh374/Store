using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Store.Api.Factories;
using System.Text.Json.Serialization;

namespace Store.Api.Extensions
{
    public static class PresentationServiceExtension
    {
        public static IServiceCollection AddPresentationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers().AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.Configure<ApiBehaviorOptions>(o =>
            {
                o.InvalidModelStateResponseFactory = ApiResponseFactory.CustomValidationErrorResponse;
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(swagger =>
            {
                #region changing Swagger default schema 
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Store.API",
                    Description = " test E-commerce App "
                });
                // To Enable authorization using Swagger (JWT)
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter'Bearer'[space]and then your valid token in the text input below.\r\n\r\nExample:\"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme {Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme,Id = "Bearer" } },
                        new string[] {}
                    }

                });
                #endregion
            });

            return services;
        }
    }
}
