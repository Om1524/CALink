using CALink.Application.DTOs.Common_Dto;
using CALink.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Application.Interfaces.Common_Interface
{
    public interface IAuthService
    {
        Task<ApiResponse<AppUserLoginResponseDto>> LoginAdmin(AppUserLoginRequestDto appUserLoginRequestDto);
    }
}
