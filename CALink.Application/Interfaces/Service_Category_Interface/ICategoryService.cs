using CALink.Application.DTOs.Service_Category_Dto;
using CALink.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Application.Interfaces.Service_Category_Interface
{
    public interface ICategoryService
    {
        Task<ApiResponse<PaginationResponse<CategoryResponseDto>>> GetAllAsync(Paginate paginate);

        Task<ApiResponse<CategoryResponseDto>> GetById(Guid id);

        Task<ApiResponse<CategoryResponseDto>> Create(CategoryRequestDto dto);

        Task<ApiResponse<CategoryResponseDto>> Update(Guid id,CategoryRequestDto dto);  

        Task<ApiResponse<CategoryResponseDto>> Delete(Guid id);
    }
}
