using EMS.WebApi.EfCore.Models;
using System.Linq.Expressions;

namespace EMS.WebApi.EfCore.Repositories;

public interface IBaseRepository<TEntity> where TEntity : BaseModel
{
    int Create(TEntity entity);
    int CreateRange(IEnumerable<TEntity> entities);
    int Delete(TEntity entity);
    int DeleteById(int id);
    int DeleteSoft(int id);
    bool Exists(Expression<Func<TEntity, bool>> predicate);
    TEntity Find(int id);
    TEntity Find(Expression<Func<TEntity, bool>> predicate);
    IEnumerable<TEntity> List();
    IEnumerable<TEntity> Search(Expression<Func<TEntity, bool>> predicate);
    int Update(TEntity entity);
    int UpdateRange(IEnumerable<TEntity> entities);
}
