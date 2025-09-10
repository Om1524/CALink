using CALink.Application.DTOs.Super_Admin_Dto;
using CALink.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Application.Interfaces.Super_Admin_Interface
{
    public interface ICompanyService
    {
        Task<ApiResponse<PaginationResponse<CompanyResponseDto>>> GetAllAsync(Paginate paginate);
        Task<ApiResponse<CompanyResponseDto>> GetById(Guid id);       
        Task<ApiResponse<CompanyResponseDto>> Create(CompanyRequestDto companyRequestDto);
        Task<ApiResponse<CompanyResponseDto>> Update(Guid id, CompanyRequestDto companyRequestDto);
        Task<ApiResponse<CompanyResponseDto>> Delete(Guid id);
    }
}
