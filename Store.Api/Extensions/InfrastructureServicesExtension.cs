using Domain.Contracts;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data;
using Persistence.Identity;
using Persistence.Repositories;
using Persistence.UnitOfWork;
using StackExchange.Redis;
using System.Text.Json.Serialization;

namespace Store.Api.Extensions
{
    public static class InfrastructureServicesExtension
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region DbContext 
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultSQLConnection"));
            });
            #endregion

            #region IdentityDbContext  
            services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentitySQLConnection"));
            });

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<StoreIdentityDbContext>();
            #endregion

            #region Redis 
            services.AddSingleton<IConnectionMultiplexer>
                    (
                        _ => ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis"))
                    );
            #endregion

            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            return services;
        }
    }
}
