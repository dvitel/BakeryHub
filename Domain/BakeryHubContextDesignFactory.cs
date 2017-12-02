using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BakeryHub.Domain
{
    class BakeryHubDesignTimeContextFactory : IDesignTimeDbContextFactory<BakeryHubContext>
    {
        public BakeryHubContext CreateDbContext(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configFile = String.Join(".", (new[] { "appsettings", env == "Production" ? "" : env, "json" }).Where(s => !String.IsNullOrEmpty(s)));
            System.Console.WriteLine($"ENVIRONMENT: {env ?? "Production"}; {configFile}");
            var config =
                new ConfigurationBuilder()
                    .AddJsonFile(configFile)
                    .Build();
            var optionsBuilder = new DbContextOptionsBuilder<BakeryHubContext>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            return new BakeryHubContext(optionsBuilder.Options);
        }
    }
}
