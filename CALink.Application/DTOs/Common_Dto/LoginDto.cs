using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Application.DTOs.Common_Dto
{
    public class AppUserLoginRequestDto
    {       
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }

    public class AppUserLoginResponseDto 
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
         public bool IsActive { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
