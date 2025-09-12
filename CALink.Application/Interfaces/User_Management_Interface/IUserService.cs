using CALink.Application.DTOs.User_Management_Dto;
using CALink.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Application.Interfaces.User_Management_Interface
{
    public interface IUserService
    {
        Task<ApiResponse<PaginationResponse<UserResponseDto>>> GetAllAsync(Paginate paginate);
        Task<ApiResponse<UserResponseDto>> GetById(Guid id);
        Task<ApiResponse<UserResponseDto>> Create(UserRequestDto userRequestDto);
        Task<ApiResponse<UserResponseDto>> Update(Guid id, UserRequestDto userRequestDto);
        Task<ApiResponse<UserResponseDto>> Delete(Guid id);
    }
}
