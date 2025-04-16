using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp42
{
    public class MyAppContext : DbContext
    {
        

        public DbSet<AreaEntity> Areas { get; set; }
        public DbSet<CityEntity> Cities { get; set; }
        public DbSet<WarehouseEntity> Houses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=ep-square-bar-a21rbkc4-pooler.eu-central-1.aws.neon.tech;Database=neondb;Username=neondb_owner;Password=npg_Nd9aqQl4XLiZ");
        }
    }
}
