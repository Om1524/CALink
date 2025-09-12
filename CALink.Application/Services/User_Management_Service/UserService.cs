using CALink.Application.DTOs.User_Management_Dto;
using CALink.Application.Interfaces.User_Management_Interface;
using CALink.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Application.Services.User_Management_Service
{
    public class UserService : IUserService
    {
        public UserService() { }

        public Task<ApiResponse<UserResponseDto>> Create(UserRequestDto userRequestDto)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<UserResponseDto>> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<PaginationResponse<UserResponseDto>>> GetAllAsync(Paginate paginate)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<UserResponseDto>> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<UserResponseDto>> Update(Guid id, UserRequestDto userRequestDto)
        {
            throw new NotImplementedException();
        }
    }
}
