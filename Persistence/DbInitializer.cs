using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.Data;
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
        private readonly ILogger<DbInitializer> _logger;

        public DbInitializer(StoreDbContext context, ILogger<DbInitializer> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task InitializeAsync()
        {
            try
            {
                //if(_context.Database.GetPendingMigrations().Any())
                //    _context.Database.Migrate();

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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }
        }
    }
}
