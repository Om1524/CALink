using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Application.DTOs.Super_Admin_Dto
{
    public class CompanyRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Tenant { get; set; }
        public string? Code { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? ZipCode { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class CompanyResponseDto
    {
        public Guid Id { get; set; }   // from BaseEntity
        public string Name { get; set; } = string.Empty;
        public string? Tenant { get; set; }
        public string? Code { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? ZipCode { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }  // from BaseEntity
        public DateTime UpdatedAt { get; set; }  // from BaseEntity
    }

}
