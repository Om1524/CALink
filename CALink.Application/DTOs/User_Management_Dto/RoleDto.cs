using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CALink.Domain.Enums.Enums;

namespace CALink.Application.DTOs.User_Management_Dto
{
    public class RoleRequestDto
    {
        public Guid CompanyId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Active;
    }

    public class RoleResponseDto
    {
        public Guid Id { get; set; }

        public Guid CompanyId { get; set; }

        public string RoleName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public UserStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
