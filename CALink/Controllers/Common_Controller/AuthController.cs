using CALink.Application.DTOs.Common_Dto;
using CALink.Application.Interfaces.Common_Interface;
using CALink.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CALink.API.Controllers.Common_Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("LoginAdmin")]
        [AllowAnonymous]
        public async Task<ApiResponse<AppUserLoginResponseDto>> LoginAdmin([FromBody] AppUserLoginRequestDto appUserLoginRequestDto)
        {
            var response = await _authService.LoginAdmin(appUserLoginRequestDto);
            return response;
        }
    }
}
