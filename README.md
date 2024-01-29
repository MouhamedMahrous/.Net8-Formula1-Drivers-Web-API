# .Net8-Formula1-Drivers-Web-API
Building ASP.Net Core Web API by Applying the repository pattern and unit of work pattern to achieve the abstraction and the isolation of the data layer from the business layer.
</br>
# Project Overview
This project implements a RESTful API for managing Formula 1 drivers. Leveraging ASP .NET Core, Entity Framework Core, MS SQL Server, repository pattern, and the unit of work pattern, it offers seamless CRUD operations for the entities.
</br>
# What are the benefits of implementing repository pattern and unit of work pattern? 
**Implementing the repository pattern and unit of work pattern offers several benefits in software development, particularly in applications that interact with databases. Here are some key advantages:**

1.Abstraction and Decoupling: Both patterns help abstract data access logic from the rest of the application, promoting loose coupling between different components. This separation of concerns makes the codebase more modular, easier to maintain, and less prone to breaking changes when modifying data-related operations.

2.Testability: By abstracting data access behind interfaces (repositories), it becomes easier to write unit tests for the business logic without involving the actual database. Mocking the repository interfaces allows developers to isolate and test components independently, leading to more robust and reliable tests.

3.Single Responsibility Principle (SRP): The repository pattern adheres to the SRP by encapsulating all database operations related to a specific entity within a dedicated repository class. This ensures that each class has a single responsibility, making the codebase more organized and easier to understand.

4.Centralized Data Access Logic: The repository pattern centralizes data access logic within repositories, providing a consistent and uniform way to interact with data regardless of the underlying data store (e.g., SQL database, NoSQL database). This simplifies maintenance and promotes code reuse across different parts of the application.

5.Flexibility and Scalability: Implementing the unit of work pattern allows for orchestrating multiple repository operations within a single transaction. This ensures data consistency and atomicity, especially in scenarios involving multiple database changes. Additionally, the abstraction provided by these patterns makes it easier to switch between different data access technologies or to introduce caching mechanisms without affecting the rest of the application.

6.Reduced Code Duplication: By encapsulating common data access logic within repositories, developers can avoid duplicating code across multiple parts of the application. This promotes code reuse and helps maintain consistency in how data is accessed and manipulated throughout the system.

7.Enhanced Maintainability: Separating data access logic into distinct repository classes makes it easier to locate and update specific database-related functionality. This improves maintainability by reducing the risk of unintended side effects when making changes to the codebase.

Overall, the repository pattern and unit of work pattern promote cleaner, more maintainable, and testable codebases, ultimately leading to better software quality and developer productivity.
</br>
# How i implemented the repository and unit of work patterns:

- At the first i have declared my entites and here i have one entity which is the F1 Driver entity.

```c#
public class Driver
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int DriverNumber { get; set; }
    public string Team { get; set; }
}
```

- After that i have implemented the database context class using EFCore so i can connect with my database make all the database operations.

- After we have created our database now we will implement the repository and unit of work patterns.

- I have implemented the generic repository interface, this repository will have the common database operation that any other specific repositry will need. 
```c#
public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> All();
    Task<T?> GetById(int id);
    Task<bool> Add(T entity);
    Task<bool> Update(T entity);
    Task<bool> Delete(T entity);
}
```

- Then i implemented these methods in the generic repository class
```c#
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected DriversDbContext _context;
    internal DbSet<T> _dbSet;
    protected readonly ILogger _logger;

    public GenericRepository(DriversDbContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;
        _dbSet = _context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> All()
    {
        return await _dbSet.ToListAsync();
    }
    public virtual async Task<T?> GetById(int id)
    {
        return await _dbSet.FindAsync(id);
    }
    public virtual async Task<bool> Add(T entity)
    {
        await _dbSet.AddAsync(entity);
        return true;
    }
    public virtual async Task<bool> Update(T entity)
    {
        _dbSet.Update(entity);
        return true;
    }
    public virtual async Task<bool> Delete(T entity)
    {
        _dbSet.Remove(entity);
        return true;
    }
    
}
```
- Let's stop for a minute here and understand why we have implemented this generic repository instead of just implementing the repository
of my specific class which is the Driver class here? Simply because we can have more than just one entity and if we need to implement a repository for each entity we can first implement the generic repository that have all the common actions or methods among all the entities repositories and all the other repositories will inherit this generic repository and that helps us for not dublicating the code and also any entity repository can extend more functionalities for itself and also can ovveride any method from the generic repository and modify on it.

- So let's create our specific repository "Driver Repository", as you can see here the driver repository has extend a new method to itself.
```c#
public interface IDriverRepository : IGenericRepository<Driver>
{
    Task<Driver?> GetByDriverNumber(int driverNumber);
}
```  
- And here's the implementation of the repository interface:
```c#
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
```
- Now we have implemented our repositories. Let's now implement our unit of work that we will use from the business layer to manage all the repositories operations (database operations and transactions).
```c#
public interface IUnitOfWork
{
    IDriverRepository Drivers { get; }
    Task CompleteAsync();
}
```
- Here's the implementation of the IUnitOfWork:
```c#
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
```

Now, we have implemented our repositories and unit of work successfully.
</br>
# How to download and use the project
- Clone the project to your local machine.
- Attach the F1Drivers database in Database folder to your database server.
- Open the app and run it.

I hope this project can help.
</br>
Thank You.



