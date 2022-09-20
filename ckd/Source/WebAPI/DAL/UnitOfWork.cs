namespace WebAPI.DAL;

using System.Collections;
using Microsoft.EntityFrameworkCore.Storage;

public class UnitOfWork : IUnitOfWork
{
    private AppDbContext appDbContext { get; set; }
    public Dictionary<Type, object> Repositories { get; set; }
    public IDbContextTransaction? Transaction { get; set; }
    
    public UnitOfWork(AppDbContext context)
    {
        Repositories = new Dictionary<Type, object>();
        appDbContext = context;        
    }

    public virtual IRepository<T> Repository<T>() where T : class
    {
        IRepository<T>? repo;

        if (!Repositories.Keys.Contains(typeof(T)))
        {
            repo = new Repository<T>(appDbContext);
            Repositories.Add(typeof(T), repo);    
        }

        repo = Repositories[typeof(T)] as IRepository<T>;
        
        if (repo == null)
            throw new Exception("Unable to get the dbcontext or entityset for this Entity: " + typeof(T).ToString() + ";" );
            
        return repo;
        
    }

    public virtual void Commit()
    {
        if (Transaction == null)        
            throw new Exception("No transaction found or initialized in the current context or scope!");

        Transaction.Commit();
    }

    public virtual void SaveChanges()
    {
        appDbContext.SaveChanges();
    }

    public virtual void RollBack()
    {
        if (Transaction == null)        
            throw new Exception("No transaction found or initialized in the current context or scope!");

        Transaction.Rollback();
    }

    public virtual void BeginTransaction()
    {
        Transaction = appDbContext.Database.BeginTransaction();
    }
}
