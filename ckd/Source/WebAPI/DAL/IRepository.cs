namespace WebAPI.DAL;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

public interface IRepository<TEntity> where TEntity : class
{

    TEntity GetByID(object id);
    IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>?, IOrderedQueryable<TEntity>>? orderBy, string includeProperties = "");
    IEnumerable<TEntity> GetAsNoTracking(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties = "");
    IQueryable<TEntity> GetQueryAsNoTracking(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
    IEnumerable<TEntity> GetAll();

    void Insert(TEntity entity);
    void InsertRange(TEntity[] entityList);

    void Update(TEntity entityToUpdate);

    void Delete(object Id);
    void Delete(TEntity entity);
    void DeleteRange(TEntity[] entity);

}
