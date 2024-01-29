using FormulaApi.Core;
using FormulaApi.Core.Repositories;

namespace FormulaApi.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DriversDbContext _context;
        
        public IDriverRepository Drivers { get; private set; }

        public UnitOfWork(DriversDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            var _logger = loggerFactory.CreateLogger("logs");
            Drivers = new DriverRepository(_context, _logger);
        }
        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
