using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Domain.Common
{
    public class FilterAndSortingHelper
    {

        public static List<Expression<Func<T, bool>>> GetFilterExpressions<T>(
        Paginate paginate,
        Dictionary<string, Func<string, Expression<Func<T, bool>>>> filterMap)
        {
            var expressions = new List<Expression<Func<T, bool>>>();

            if (paginate.Filters != null)
            {
                foreach (var filter in paginate.Filters)
                {
                    var key = filter.Key.ToLower();

                    // Use ToString only if the value is not null
                    var stringValue = filter.Value?.ToString();

                    // ✅ Do not check IsNullOrWhiteSpace — allow "false", "0", etc.
                    if (filterMap.ContainsKey(key) && stringValue != null)
                    {
                        expressions.Add(filterMap[key](stringValue));
                    }
                }
            }
            return expressions;
        }

        public static Func<IQueryable<T>, IOrderedQueryable<T>>? GetSortExpression<T>(
                Paginate paginate,
                Dictionary<string, Expression<Func<T, object>>> sortMap)
        {
            if (paginate.Sort != null && paginate.Sort.Any())
            {
                var sort = paginate.Sort.FirstOrDefault();
                string key = sort.Key?.ToLower() ?? "";
                string direction = sort.Value?.ToLower() ?? "desc";

                if (sortMap.ContainsKey(key))
                {
                    var expression = sortMap[key];
                    return direction == "asc"
                        ? q => q.OrderBy(expression)
                        : q => q.OrderByDescending(expression);
                }
            }

            // No sort specified and no fallback
            return null;
        }
    }
}
