using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Application.DTOs.Common_Dto
{
    public class TokenPayloadDto
    {
        public required Guid UserId { get; set; }
        public required Guid CompanyId { get; set; }
        public required string FirstName { get; set; }
        public required string Code { get; set; }
        public required string Email { get; set; }
    }

    public class AdminTokenPayloadDto
    {
        public required Guid UserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }

    }
}
