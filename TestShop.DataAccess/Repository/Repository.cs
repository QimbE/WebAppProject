using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestShop.DataAccess.Data;
using TestShop.DataAccess.Repository.IRepository;

namespace TestShop.DataAccess.Repository
{
    public class Repository<T>: IRepository<T> where T: class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter=null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
	            query = query.Where(filter);
			}
            
			if (!string.IsNullOrEmpty(includeProperties))
            {
	            foreach (var includeProp in includeProperties
		                     .Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries))
	            {
		            query = query.Include(includeProp);
	            }
            }
            return await query.ToListAsync();
        }

        public async Task<T> Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
	        IQueryable<T> query;

			if (tracked)
	        {
				query = dbSet;
	        }
	        else
	        {
		        query = dbSet.AsNoTracking();
			}

	        query = query.Where(filter);
	        if (!string.IsNullOrEmpty(includeProperties))
	        {
		        foreach (var includeProp in includeProperties
			                 .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
		        {
			        query = query.Include(includeProp);
		        }
	        }

	        return query.FirstOrDefault();
		}

        public async Task Add(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public async Task RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}
