using CALink.Application.DTOs.User_Management_Dto;
using CALink.Application.Interfaces.User_Management_Interface;
using CALink.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CALink.API.Controllers.User_Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userservice;
        public UserController(IUserService userService)
        {
            _userservice = userService;
        }
        // GET: api/Customer/getFilter
        [HttpPost("getFilter")]
        public async Task<ApiResponse<PaginationResponse<UserResponseDto>>> GetFilterAsync([FromBody] Paginate paginate)
        {
            var response = await _userservice.GetAllAsync(paginate);
            return response;
        }
        // GET: api/Customer/{id}
        [HttpGet("{id}")]
        public async Task<ApiResponse<UserResponseDto>> GetById(Guid id)
        {
            var response = await _userservice.GetById(id);
            return response;
        }
        // POST: api/Customer
        [HttpPost]
        public async Task<ApiResponse<UserResponseDto>> Create([FromBody] UserRequestDto dto)
        {
            var response = await _userservice.Create(dto);
            return response;
        }
        // PUT: api/Customer/{id}
        [HttpPut("{id}")]
        public async Task<ApiResponse<UserResponseDto>> Update(Guid id, [FromBody] UserRequestDto dto)
        {
            var response = await _userservice.Update(id, dto);
            return response;
        }
        // DELETE: api/Customer/{id}
        [HttpDelete("{id}")]
        public async Task<ApiResponse<UserResponseDto>> Delete(Guid id)
        {
            var response = await _userservice.Delete(id);
            return response;
        }
    }
}
