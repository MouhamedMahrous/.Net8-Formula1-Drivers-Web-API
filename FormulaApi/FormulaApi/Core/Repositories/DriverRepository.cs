using FormulaApi.Data;
using FormulaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FormulaApi.Core.Repositories
{
    public class DriverRepository : GenericRepository<Driver>, IDriverRepository
    {
        public DriverRepository(DriversDbContext context, ILogger logger) 
            : base(context, logger)
        {
        }

        public async Task<Driver?> GetByDriverNumber(int driverNumber)
        {
            return await _context.Drivers.FirstOrDefaultAsync(d => d.DriverNumber == driverNumber);
        }
    }
}
