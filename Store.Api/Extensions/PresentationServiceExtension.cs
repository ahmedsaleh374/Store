using Microsoft.AspNetCore.Mvc;
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
            services.AddSwaggerGen();

            return services;
        }
    }
}
