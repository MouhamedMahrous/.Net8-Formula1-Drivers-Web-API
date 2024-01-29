namespace FormulaApi.Core
{
    public interface IUnitOfWork
    {
        IDriverRepository Drivers { get; }
        Task CompleteAsync();
    }
}
