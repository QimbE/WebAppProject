using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TestShop.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T: class
    //T - Some Db entity
    {
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter=null, string? includeProperties = null);
        Task<T> Get(Expression<Func<T,bool>> filter, string? includeProperties = null, bool tracked = false);
        Task Add(T entity);
        Task Remove(T entity);
        Task RemoveRange(IEnumerable<T> entities);
    }
}
