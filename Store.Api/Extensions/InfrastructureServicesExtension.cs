using Domain.Contracts;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Persistence.Data;
using Persistence.Identity;
using Persistence.Repositories;
using Persistence.UnitOfWork;
using Shared.IdentityDtos;
using StackExchange.Redis;
using System.Text;
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


            #region JWT configurations 
            var jwtConfig = configuration.GetSection("JwtOptions").Get<JwtOptions>();
            //var jwtConfig = services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

            // changing schema 
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtConfig.Issuer,
                    ValidAudience = jwtConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecurityKey))
                };
            });
            #endregion


            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            


            return services;
        }
    }
}
