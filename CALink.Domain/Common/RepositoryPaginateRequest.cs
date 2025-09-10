using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Domain.Common
{
    public class RepositoryPaginateRequest<T>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public Dictionary<string, string>? Sort { get; set; } = null;
        public Expression<Func<T, bool>>? FilterExpression { get; set; } = null;
        public Func<IQueryable<T>, IOrderedQueryable<T>>? SortExpression { get; set; } = null;
    }
}
