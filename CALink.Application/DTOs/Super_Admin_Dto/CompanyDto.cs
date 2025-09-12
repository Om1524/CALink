using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Application.DTOs.Super_Admin_Dto
{

    public class CompanyUserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public int status { get; set; } = 1; // 1: Active, 2: Inactive, 3: Pending
    }

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
        public List<CompanyUserDto>? Users { get; set; } = new List<CompanyUserDto>();
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
