using CALink.Application.DTOs.Super_Admin_Dto;
using CALink.Application.Interfaces.Common_Interface;
using CALink.Application.Interfaces.Super_Admin_Interface;
using CALink.Application.Services.Super_Admin_Service;
using CALink.Domain.Common;
using CALink.Domain.Entities.Super_Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CALink.API.Controllers.Super_Admin_Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyController : Controller
    {

        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpPost("getFilter")]
        public async Task<ApiResponse<PaginationResponse<CompanyResponseDto>>> GetFilterAsync([FromBody] Paginate paginate)
        {
            var response = await _companyService.GetAllAsync(paginate);
            return response;
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<CompanyResponseDto>> GetById(Guid id)
        {
            var response = await _companyService.GetById(id);
            return response;
        }

        [HttpPost]
        public async Task<ApiResponse<CompanyResponseDto>> Create([FromBody] CompanyRequestDto companyRequestDto)
        {
            var response = await _companyService.Create(companyRequestDto);
            return response;
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<CompanyResponseDto>> Update(Guid id, [FromBody] CompanyRequestDto companyRequestDto)
        {
            var response = await _companyService.Update(id, companyRequestDto);
            return response;
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse<CompanyResponseDto>> Delete(Guid id)
        {
            var response = await _companyService.Delete(id);
            return response;
        }

    }
}
