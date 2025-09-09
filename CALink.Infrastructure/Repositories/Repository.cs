using CALink.Domain.Entities.Common;
using CALink.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using CALink.Application.Interfaces.Common_Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<Tuple<IEnumerable<T>, int>> GetFilterAsync(int pageNumber = 1, int pageSize = 10, List<Expression<Func<T, bool>>>? filterExpressions = null, Func<IQueryable<T>, IOrderedQueryable<T>>? sortExpression = null)
        {
            if (pageNumber == 1)
            {
                pageNumber = 0; // Adjusting to zero-based index for EF Core
            }
            else
            {
                pageNumber -= 1; // Adjusting to zero-based index for EF Core
            }
            var query = _dbSet.AsQueryable();
            if (filterExpressions != null && filterExpressions.Count > 0)
            {
                foreach (var filter in filterExpressions)
                {
                    query = query.Where(filter);
                }
            }

            if (sortExpression != null)
            {
                query = sortExpression(query);
            }

            int totalItems = await query.CountAsync();

            var items = await query.Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();

            return new Tuple<IEnumerable<T>, int>(items, totalItems);
        }

        public Task<Tuple<IEnumerable<T>, int>> GetFilterAsync(int pageNumber = 1, int pageSize = 10, List<Expression<Func<T, bool>>>? filterExpressions = null, Func<IQueryable<T>, IOrderedQueryable<T>>? sortExpression = null, Func<object, bool> predicate = null)
        {
            if (predicate != null)
            {
                // If a predicate is provided, apply it to the filter expressions.
                if (filterExpressions == null)
                {
                    filterExpressions = new List<Expression<Func<T, bool>>>();
                }
                // Convert the predicate to an expression and add it to the filter expressions.
                var parameter = Expression.Parameter(typeof(T), "x");
                var body = Expression.Invoke(Expression.Constant(predicate), parameter);
                var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);
                filterExpressions.Add(lambda);
            }
            return GetFilterAsync(pageNumber, pageSize, filterExpressions, sortExpression);
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
        public async Task<T> AddAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null");
            }
            _dbSet.Add(entity);

            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            if (entities == null || !entities.Any())
            {
                throw new ArgumentNullException(nameof(entities), "Entities cannot be null or empty");
            }
            _dbSet.AddRange(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null");
            }
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        //public Task<T> DeleteAsync(Guid id)
        //{
        //    var entity = _dbSet.Find(id);
        //    if (entity == null)
        //    {
        //        throw new KeyNotFoundException($"Entity with ID {id} not found.");
        //    }
        //    _dbSet.Remove(entity);
        //    _context.SaveChanges();
        //    return Task.FromResult(entity);
        //}
        public async Task<T> DeleteAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with ID {id} not found.");
            }
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<string?> GetNameByIdAsync<TEntity>(Guid id, Expression<Func<TEntity, Guid>> idSelector, Expression<Func<TEntity, string>> nameSelector) where TEntity : class
        {
            // Compile the selector to access the value for comparison
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var predicate = Expression.Lambda<Func<TEntity, bool>>(
                Expression.Equal(
                    Expression.Invoke(idSelector, parameter),
                    Expression.Constant(id)
                ),
                parameter
            );

            return await _context.Set<TEntity>()
                .Where(predicate)
                .Select(nameSelector)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<List<T>> FindAllAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = _context.Set<T>().Where(predicate);

            if (include != null)
            {
                query = include(query);
            }

            return await query.ToListAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            if (predicate != null)
            {
                // Apply the predicate to filter entities and count the filtered results
                return await _dbSet.Where(predicate).CountAsync();
            }
            // Return the total count if no predicate is provided
            return await _dbSet.CountAsync();
        }

        public async Task<List<T>> GetByCompanyIdAsync(Guid companyId, Expression<Func<T, string>>? orderBy = null)
        {
            var query = _dbSet.Where(e => EF.Property<Guid>(e, "CompanyId") == companyId);

            if (orderBy != null)
            {
                query = query.OrderBy(orderBy);
            }

            return await query.ToListAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AnyAsync(predicate);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
