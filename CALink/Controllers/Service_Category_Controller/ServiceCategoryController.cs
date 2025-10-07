using CALink.Application.DTOs.Service_Category_Dto;
using CALink.Application.DTOs.User_Management_Dto;
using CALink.Application.Interfaces.Service_Category_Interface;
using CALink.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CALink.API.Controllers.Service_Category_Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ServiceCategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public ServiceCategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("getFilter")]
        public async Task<ApiResponse<PaginationResponse<CategoryResponseDto>>> GetFilterAsync([FromBody] Paginate paginate)
        {
            var response = await _categoryService.GetAllAsync(paginate);
            return response;
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<CategoryResponseDto>> GetByIdAsync(Guid id)
        {
            var response = await _categoryService.GetById(id);
            return response; 
        }

        [HttpPost]
        public async Task<ApiResponse<CategoryResponseDto>> CreateAsync([FromBody] CategoryRequestDto dto)
        {

            var response = await _categoryService.Create(dto);
            return response;
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<CategoryResponseDto>> UpdateAsync(Guid id, [FromBody] CategoryRequestDto dto)
        {
            var response = await _categoryService.Update(id, dto);
            return response;
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse<CategoryResponseDto>> DeleteAsync(Guid id)
        {
            var response = await _categoryService.Delete(id);
            return response;
        }

    }
}
