using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Domain.Common
{
    public class PaginationResponse<T>
    {
        public IEnumerable<T>? Rows { get; set; } = null;
        public int Total { get; set; }

        public PaginationResponse()
        {
            Rows = new List<T>();
            Total = 0;
        }

        public PaginationResponse(IEnumerable<T> rows, int total)
        {
            Rows = rows;
            Total = total;
        }
    }
}

