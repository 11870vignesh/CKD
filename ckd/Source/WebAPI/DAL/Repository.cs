namespace WebAPI.DAL;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private AppDbContext context;
    private DbSet<TEntity> dbSet;

    public Repository(AppDbContext context)
    {
        this.context = context;
        this.dbSet = context.Set<TEntity>();

        if (this.context == null || this.dbSet == null)
            throw new Exception("Unable to get the dbcontext or entityset for this Entity: " + typeof(TEntity).ToString() + ";");
    }

    public virtual TEntity GetByID(object id)
    {
        var entity = dbSet.Find(id);
        if (entity == null)
            throw new Exception("The entity with the given Id could not be found!");

        return entity;
    }

    public virtual void Insert(TEntity entity)
    {
        dbSet.Add(entity);
    }

    public virtual void InsertRange(TEntity[] entityList)
    {
        dbSet.AddRange(entityList);
    }

    public virtual void Update(TEntity entityToUpdate)
    {
        dbSet.Attach(entityToUpdate);
        context.Entry(entityToUpdate).State = EntityState.Modified;
    }

    public virtual void Delete(TEntity entityToDelete)
    {
        if (context.Entry(entityToDelete).State == EntityState.Detached)
        {
            dbSet.Attach(entityToDelete);
        }
        dbSet.Remove(entityToDelete);
    }

    public virtual void Delete(object id)
    {
        var entity = dbSet.Find(id);
        if (entity == null)
            throw new Exception("The entity with the given Id could not be found!");

        Delete(entity);
    }

    public virtual void DeleteRange(TEntity[] entityArrayToDelete)
    {
        dbSet.RemoveRange(entityArrayToDelete);
    }

    public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>?, IOrderedQueryable<TEntity>>? orderBy, string includeProperties = "")
    {
        IQueryable<TEntity> query = dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        if (orderBy != null)
        {
            return orderBy(query).ToList();
        }
        else
        {
            return query.ToList();
        }
    }

    public virtual IEnumerable<TEntity> GetAsNoTracking(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties = "")
    {
        IQueryable<TEntity> query = dbSet;

        if (filter != null)
        {
            query = query.Where(filter).AsNoTracking();
        }

        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        if (orderBy != null)
        {
            return orderBy(query).ToList();
        }
        else
        {
            return query.ToList();
        }
    }
    public virtual IQueryable<TEntity> GetQueryAsNoTracking(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
    {
        IQueryable<TEntity> query = dbSet;

        if (filter != null)
        {
            query = query.Where(filter).AsNoTracking();
        }

        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }


        if (orderBy != null)
        {
            return orderBy(query);
        }
        else
        {
            return query;
        }
    }

    public virtual IEnumerable<TEntity> GetAll()
    {
        return dbSet;
    }

}
