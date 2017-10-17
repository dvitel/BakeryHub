using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    class BakeryHubDesignTimeContextFactory : IDesignTimeDbContextFactory<BakeryHubContext>
    {
        public BakeryHubContext CreateDbContext(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("DB_ENV");
            if (String.IsNullOrEmpty(env)) throw new Exception("Set DB_ENV to Development, Production or Staging before executing migrations");
            var config =
                new ConfigurationBuilder()
                    .AddJsonFile($"appsettings.{env}.json")
                    .Build();
            var optionsBuilder = new DbContextOptionsBuilder<BakeryHubContext>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            return new BakeryHubContext(optionsBuilder.Options);
        }
    }
}
