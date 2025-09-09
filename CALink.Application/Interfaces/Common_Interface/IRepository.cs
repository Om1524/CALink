using CALink.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Application.Interfaces.Common_Interface
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<Tuple<IEnumerable<T>, int>> GetFilterAsync(
            int pageNumber = 1,
            int pageSize = 10,
            List<Expression<Func<T, bool>>>? filterExpressions = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? sortExpression = null);

        Task<Tuple<IEnumerable<T>, int>> GetFilterAsync(
            int pageNumber = 1,
            int pageSize = 10,
            List<Expression<Func<T, bool>>>? filterExpressions = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? sortExpression = null,
            Func<object, bool> predicate = null);

        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        Task<List<T>> FindAllAsync(Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        Task<T> UpdateAsync(T entity);
        Task SaveChangesAsync();
        Task<T> DeleteAsync(Guid id);
        Task<string?> GetNameByIdAsync<TEntity>(Guid id, Expression<Func<TEntity, Guid>> idSelector,
                                                 Expression<Func<TEntity, string>> nameSelector)
                                                 where TEntity : class;
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate);

        Task<IEnumerable<T>> GetAllAsync();
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }
}
