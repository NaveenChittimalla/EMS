using EMS.WebApi.EfCore.Data;
using EMS.WebApi.EfCore.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EMS.WebApi.EfCore.Repositories;
public class BaseRepository<TEntity>(EmsDbContext dbContext) : IBaseRepository<TEntity> where TEntity : BaseModel
{
    private readonly DbSet<TEntity> _dbset = dbContext.Set<TEntity>();

    public int Create(TEntity entity)
    {
        _dbset.Add(entity);
        return SaveChanges();
    }

    public int CreateRange(IEnumerable<TEntity> entities)
    {
        _dbset.AddRange(entities);
        return SaveChanges();
    }

    public int Delete(TEntity entity)
    {
        _dbset.Remove(entity);
        return SaveChanges();
    }

    public int DeleteById(int id)
    {
        TEntity entity = _dbset.Find(id);
        if (entity is not null)
        {
            _dbset.Remove(entity);
            return SaveChanges();
        }
        return 0;
    }

    public int DeleteSoft(int id)
    {
        TEntity entity = _dbset.Find(id);
        if (entity is not null)
        {
            entity.Deleted = true;
            _dbset.Update(entity);
            return SaveChanges();
        }
        return 0;
    }

    public bool Exists(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbset.Any(predicate);
    }

    public TEntity Find(int id)
    {
        return _dbset.Find(id);
    }

    public TEntity Find(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbset.FirstOrDefault(predicate);
    }

    public IEnumerable<TEntity> List()
    {
        return _dbset.AsNoTracking().ToList();
    }

    public IEnumerable<TEntity> Search(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbset.Where(predicate).AsNoTracking().ToList();
    }

    public int Update(TEntity entity)
    {
        _dbset.Update(entity);
        return SaveChanges();
    }

    public int UpdateRange(IEnumerable<TEntity> entities)
    {
        _dbset.UpdateRange(entities);
        return SaveChanges();
    }

    private int SaveChanges()
    {
        return dbContext.SaveChanges();
    }
}
