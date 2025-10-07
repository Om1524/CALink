using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CALink.Domain.Enums.Enums;

namespace CALink.Application.DTOs.Service_Category_Dto
{
    public class CategoryRequestDto
    {
        public string CategoryCode { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public CategoryStatus Status { get; set; }
    }

    public class CategoryResponseDto
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public CategoryStatus Status { get; set; }

    }
}
