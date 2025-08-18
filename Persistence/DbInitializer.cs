using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Identity;
using Domain.Entities.OrderEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.Data;
using Persistence.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence
{
    public class DbInitializer : IDbInitializer
    {
        private readonly StoreDbContext _context;
        private readonly StoreIdentityDbContext _identityDbContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<DbInitializer> _logger;

        public DbInitializer
            (
                 StoreDbContext context
                 , StoreIdentityDbContext identityDbContext
                 , UserManager<User> userManager
                 , RoleManager<IdentityRole> roleManager
                 , ILogger<DbInitializer> logger
            )
        {
            _context = context;
            _identityDbContext = identityDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }
        public async Task InitializeAsync()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Any())
                    await _context.Database.MigrateAsync();

                if (!_context.productBrands.Any())
                {
                    var brandsData = File.ReadAllText(@"..\Persistence\Data\Seeds\brands.json");
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                    if (brands is not null && brands.Any())
                    {
                        await _context.productBrands.AddRangeAsync(brands);
                        await _context.SaveChangesAsync();
                    }
                }

                if (!_context.productTypes.Any())
                {
                    var TypesData = File.ReadAllText(@"..\Persistence\Data\Seeds\types.json");
                    var Types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);

                    if (Types is not null && Types.Any())
                    {
                        await _context.productTypes.AddRangeAsync(Types);
                        await _context.SaveChangesAsync();
                    }
                }

                if (!_context.products.Any())
                {
                    var productsData = File.ReadAllText(@"..\Persistence\Data\Seeds\products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                    if (products is not null && products.Any())
                    {
                        await _context.products.AddRangeAsync(products);
                        await _context.SaveChangesAsync();
                    }
                }

                if (!_context.deliveryMethods.Any())
                {
                    var deliveryData = File.ReadAllText(@"..\Persistence\Data\Seeds\delivery.json");
                    var delivery = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);

                    if (delivery is not null && delivery.Any())
                    {
                        await _context.deliveryMethods.AddRangeAsync(delivery);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }
        }

        public async Task InitializeIdentityAsync()
        {
            try
            {
                if (_identityDbContext.Database.GetPendingMigrations().Any())
                    await _identityDbContext.Database.MigrateAsync();

                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }

                if (!_userManager.Users.Any())
                {
                    var superAdminUser = new User
                    {
                        DisplayName = "Super Admin",
                        UserName = "SuperAdmin",
                        Email = "SuperAdmin@gmail.com",
                        PhoneNumber = "1234567890",
                    };

                    var AdminUser = new User
                    {
                        DisplayName = "Admin",
                        UserName = "Admin",
                        Email = "Admin@gmail.com",
                        PhoneNumber = "55442151410",
                    };

                    //create
                    await _userManager.CreateAsync(superAdminUser, "Ahmed123 ");
                    await _userManager.CreateAsync(AdminUser, "Ahmed123 ");
                    // add role
                    await _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                    await _userManager.AddToRoleAsync(AdminUser, "Admin");
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
            }
        }
    }
}

