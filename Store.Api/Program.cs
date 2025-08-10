
using Domain.Contracts;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data;
using Persistence.Identity;
using Persistence.Repositories;
using Persistence.UnitOfWork;
using Services;
using Services.Abstractions;
using Services.MappedProfiles;
using StackExchange.Redis;
using Store.Api.Factories;
using Store.Api.Middlewares;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Store.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });


            #region DbContext 
            builder.Services.AddDbContext<StoreDbContext>(options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
                }); 
            #endregion

            #region IdentityDbContext  
            builder.Services.AddDbContext<StoreIdentityDbContext>(options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentitySQLConnection"));
                });

            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<StoreIdentityDbContext>();
            #endregion

            #region Redis 
            builder.Services.AddSingleton<IConnectionMultiplexer>
                    (
                        _ => ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis"))
                    ); 
            #endregion

            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddScoped<IDbInitializer, DbInitializer>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IServiceManager, ServiceManager>();
            #region other ways for register auto mapper  
            //builder.Services.AddAutoMapper(typeof(AssemblyReference).Assembly);
            //builder.Services.AddAutoMapper(typeof(Services.ServiceManager).Assembly);
            //builder.Services.AddAutoMapper(x =>x.AddProfile(new ProductProfile()));  
            #endregion
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            builder.Services.Configure<ApiBehaviorOptions>(o =>
            {
                o.InvalidModelStateResponseFactory = ApiResponseFactory.CustomValidationErrorResponse;
            });


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            await SeedDbAsync(app);
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
        #region Injestion in an other way seeding data  
        static async Task SeedDbAsync(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();

            await dbInitializer.InitializeAsync();
        }
        #endregion
    }
}
