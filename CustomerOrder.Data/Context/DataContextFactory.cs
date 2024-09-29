using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOrder.Data.Context
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args = null)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = configurationBuilder.Build();
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

            return new DataContext(optionsBuilder.Options);
        }
    }
}
