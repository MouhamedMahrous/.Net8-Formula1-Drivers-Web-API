using FormulaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FormulaApi.Data
{
    public class DriversDbContext : DbContext
    {
        public virtual DbSet<Driver> Drivers { get; set; }
        public DriversDbContext(DbContextOptions<DriversDbContext> options) 
            : base(options)
        {
        }
    }
}
