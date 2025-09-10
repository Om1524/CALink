using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Domain.Common
{
    public class Paginate
    {
        private int _pageNumber;
        private int _pageSize;

        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value > 0 ? value : 1;
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > 0 ? value : 10;
        }

        public Paginate()
        {
            _pageNumber = 1;
            _pageSize = 10;
        }

        public Dictionary<string, string>? Filters { get; set; } = null;
        public Dictionary<string, string>? Sort { get; set; } = null;
    }
}
