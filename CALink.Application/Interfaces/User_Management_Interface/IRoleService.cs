using CALink.Application.DTOs.User_Management_Dto;
using CALink.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Application.Interfaces.User_Management_Interface
{
    public interface IRoleService
    {
        Task<ApiResponse<PaginationResponse<RoleResponseDto>>> GetAllAsync(Paginate paginate);
        Task<ApiResponse<RoleResponseDto>> GetById(Guid id);
        Task<ApiResponse<RoleResponseDto>> Create(RoleRequestDto dto);
        Task<ApiResponse<RoleResponseDto>> Update(Guid id, RoleRequestDto roleRequestDto);
        Task<ApiResponse<RoleResponseDto>> Delete(Guid id);
    }
}
