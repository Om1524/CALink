using CALink.Application.DTOs.Super_Admin_Dto;
using CALink.Application.Interfaces.Super_Admin_Interface;
using CALink.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CALink.API.Controllers.Super_Admin_Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppUserController : Controller
    {
        private readonly IAppUserService _appUserService;

        public AppUserController(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        [HttpPost("getFilter")]
        public async Task<ApiResponse<PaginationResponse<AppUserResponseDto>>> GetFilterAsync([FromBody] Paginate paginate)
        {
            var response = await _appUserService.GetAllAsync(paginate);
            return response;
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<AppUserResponseDto>> GetById(Guid id)
        {
            var response = await _appUserService.GetById(id);
            return response;
        }

        [HttpPost]
        public async Task<ApiResponse<AppUserResponseDto>> Create([FromBody] AppUserRequestDto appUserRequestDto)
        {
            var response = await _appUserService.Create(appUserRequestDto);
            return response;
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<AppUserResponseDto>> Update(Guid id, [FromBody] AppUserRequestDto appUserRequestDto)
        {
            var response = await _appUserService.Update(id, appUserRequestDto);
            return response;
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse<AppUserResponseDto>> Delete(Guid id)
        {
            var response = await _appUserService.Delete(id);
            return response;
        }
    }
}
