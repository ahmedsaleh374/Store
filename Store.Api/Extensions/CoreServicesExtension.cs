using Services;
using Services.Abstractions;
using Shared.IdentityDtos;

namespace Store.Api.Extensions
{
    public static class CoreServicesExtension
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
            #region other ways for register auto mapper  
            //services.AddAutoMapper(typeof(AssemblyReference).Assembly);
            //services.AddAutoMapper(typeof(Services.ServiceManager).Assembly);
            //services.AddAutoMapper(x =>x.AddProfile(new ProductProfile()));  
            #endregion
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

            return services;
        }
    }
}
