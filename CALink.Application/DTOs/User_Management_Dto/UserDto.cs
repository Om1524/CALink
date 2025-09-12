using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CALink.Domain.Enums.Enums;

namespace CALink.Application.DTOs.User_Management_Dto
{

    public class UserRequestDto
    {

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Guid? RoleId { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Active;
        
    }

    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Guid? RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public UserStatus Status { get; set; }
        public string MobileNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
