using Demo.DataAccess.Data.Contexts;
using Demo.DataAccess.Models.DepartmentModule;
using Demo.DataAccess.Models.Shared;
using Demo.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Demo.DataAccess.Repositories.Classes
{
    public class GenericRepositorie<TEntity>(ApplicationDbContext _dbContext) : IGenericRepositorie<TEntity> where TEntity : BaseEntity
    {
        // Inside Scope                                                   
        // 5 CRUD OPERATIONS                                                           
        //GET ALL                                                       
        public IEnumerable<TEntity> GetAll(bool WithTracking = false)
        {
            if (WithTracking)
                return _dbContext.Set<TEntity>().Where(entity => entity.IsDeleted == false).ToList();
            else
                return _dbContext.Set<TEntity>().Where(entity => entity.IsDeleted == false).AsNoTracking().ToList();
        }
        //GET BY ID
        public TEntity? GetById(int id) => _dbContext.Set<TEntity>().Find(id);

        //ADD
        public int Add(TEntity entity)
        {   
            _dbContext.Set<TEntity>().Add(entity); // Add Locally
            return _dbContext.SaveChanges();  //num of Rows added
        }
        //UPDATE
        public int Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity); // Update Locally
            return _dbContext.SaveChanges();  //num of Rows affected
        }
        //REMOVE
        public int Remove(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity); // Update Locally
            return _dbContext.SaveChanges();  //num of Rows Deleted
        }

        //public IEnumerable<TEntity> GetIEnumerable()
        //{
        //    return _dbContext.Set<TEntity>();
        //}

        //public IQueryable<TEntity> GetIQueryable()
        //{
        //    return _dbContext.Set<TEntity>();
        //}
    }
}
