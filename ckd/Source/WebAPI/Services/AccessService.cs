using Models.entity;
using Models.model;
using WebAPI.apiauthorization;
using WebAPI.Services;
using WebAPI.DAL;
using AutoMapper;
using Models.common;
using WebAPI.services;
using System;

namespace WebAPI.Services;

public interface IAccessService
{

    Access GetById(int Id);
    List<Access> GetAll();
    List<Access> GetAll(string search, string limit, string offset, string order, string dir, string business);

    Access Add(Access entity);
    Access Update(Access entity);

    bool Delete(int id);
    bool CheckNameExists(string accessName);
List<Access> GetAllList(string? search, string? limit, string? offset, string? order, string? dir, string? business);
}

public class AccessService : BaseService, IAccessService
{
    private IRepository<Access> accessRepo;

    public AccessService(IUnitOfWork uow ,  IMapper  mapper) : base(uow)
    {
        accessRepo = uow.Repository<Access>();
    }

    public Access GetById(int Id)
    {
        var entity = accessRepo.GetByID(Id);
        return entity;
    }

    public List<Access> GetAll()
    {
        var entityList = accessRepo.GetAll().Where(x => x.IsDelete == false);
        return entityList.ToList();
    }

    public List<Access> GetAll(string search, string limit, string offset, string order, string dir, string business)
    {
        var entityList = accessRepo.GetAll().Where( x => x.IsDelete == false);
        if (business  != "null")
            entityList = entityList.Where(x => x.Name.Contains(business, StringComparison.OrdinalIgnoreCase));
        
        if (search != "null")        
            entityList = entityList.Where(x => x.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
        
        if (dir == "null" || dir == "undefined" || dir == "asc")        
            entityList = entityList.OrderByDescending(x => x.CreatedOn);        
        else        
            entityList = entityList.OrderBy(x => x.CreatedOn);
        

        //execute the query by calling the ToList method;
        entityList = entityList.ToList();

        return entityList.ToList();

    }
     public List<Access> GetAllList(string? search, string? limit, string? offset, string? order, string? dir, string? business)
    {
        var accessData = GetAll(search, limit, offset, order, dir, business);
        var data = accessData.Skip(Convert.ToInt32(offset)).Take(Convert.ToInt32(limit)).ToList();
        return data;
    }

    public Access Add(Access entity)
    {
        entity.IsDelete  =  false;
        uow.Repository<Access>().Insert(entity);
        uow.SaveChanges();

        return entity;
    }

    public Access Update(Access entity)
    {
        entity.IsDelete  = false;
        accessRepo.Update(entity);
        uow.SaveChanges();
        return entity;
    }

    public bool CheckNameExists(string access)
    {
        var Data = accessRepo.GetAll().ToList();
        if (Data.Any(x => x.Name == access))
            return false;
        return true;
    }


    public bool Delete(int entityId)
    {
        var entity = accessRepo.GetByID(entityId);
        if (entity != null)
        {
            entity.IsActive = false;
            entity.IsDelete = true;

            accessRepo.Update(entity);
            uow.SaveChanges();

            return true;
        }
        else
            return false;

    }
}