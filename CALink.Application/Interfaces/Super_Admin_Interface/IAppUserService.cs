using CALink.Application.DTOs.Super_Admin_Dto;
using CALink.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Application.Interfaces.Super_Admin_Interface
{
    public interface IAppUserService
    {
        Task<ApiResponse<PaginationResponse<AppUserResponseDto>>> GetAllAsync(Paginate paginate);
        Task<ApiResponse<AppUserResponseDto>> GetById(Guid id);
        Task<ApiResponse<AppUserResponseDto>> Create(AppUserRequestDto appUserRequestDto);
        Task<ApiResponse<AppUserResponseDto>> Update(Guid id, AppUserRequestDto appUserRequestDto);
        Task<ApiResponse<AppUserResponseDto>> Delete(Guid id);
    }
}
