using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using Pesistence.EntityConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Pesistence.AppDbContext
{
    public class ApplicationDbContext : IdentityDbContext<AppUser,AppRole,string>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // tên với tiền tố AspNet như: AspNetUserRoles, AspNetUser ...
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            //new CategoryConfiguration().Configure(modelBuilder.Entity<Category>());
            new ProductConfiguration().Configure(modelBuilder.Entity<Product>());

            modelBuilder.ApplyConfiguration( new CategoryConfiguration());
        }


    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);
            return new ApplicationDbContext(builder.Options);
        }
    }
}
