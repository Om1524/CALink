using CALink.Application.DTOs.User_Management_Dto;
using CALink.Application.Interfaces.Common_Interface;
using CALink.Application.Interfaces.User_Management_Interface;
using CALink.Domain.Common;
using CALink.Domain.Entities.User_Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Application.Services.User_Management_Service
{
    public class RoleService : IRoleService
    {
        private readonly IRepository<Role> _roleRepository;
        private readonly ITokenService _tokenService;
        private readonly IRepository<User> _userRepository;

        public RoleService(ITokenService tokenService, IRepository<User> userRepository, IRepository<Role> roleRepository)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public Task<ApiResponse<RoleResponseDto>> Create(RoleRequestDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<RoleResponseDto>> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<PaginationResponse<RoleResponseDto>>> GetAllAsync(Paginate paginate)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<RoleResponseDto>> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<RoleResponseDto>> Update(Guid id, RoleRequestDto roleRequestDto)
        {
            throw new NotImplementedException();
        }
    }
}
