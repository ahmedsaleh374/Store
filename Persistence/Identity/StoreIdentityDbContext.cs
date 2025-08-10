using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Identity
{
    public class StoreIdentityDbContext(DbContextOptions options) : IdentityDbContext<User>(options)
    {
        #region old way for inject  
        //public StoreIdentityDbContext(DbContextOptions options) : base(options)
        //{
        //} 
        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Address>().ToTable("Addresses");
            base.OnModelCreating(builder);
        }
    }
}
