using CALink.Application.DTOs.Common_Dto;
using CALink.Application.DTOs.Service_Category_Dto;
using CALink.Application.DTOs.User_Management_Dto;
using CALink.Application.Interfaces.Common_Interface;
using CALink.Application.Interfaces.Service_Category_Interface;
using CALink.Domain.Common;
using CALink.Domain.Entities.Customer;
using CALink.Domain.Entities.Service_Category;
using CALink.Domain.Entities.User_Management;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Application.Services.Category_Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ITokenService _tokenService;
        private readonly IRepository<ServiceCategory> _categoryRepositary;


        public CategoryService(ITokenService tokenService, IRepository<ServiceCategory> categoryRepositary) { 
            _tokenService = tokenService;
            _categoryRepositary = categoryRepositary;
        }


        public async Task<ApiResponse<CategoryResponseDto>> Create(CategoryRequestDto dto)
        {
            TokenPayloadDto tokenPayloadDto = _tokenService.GetUserTokenPayload();
            
            if(tokenPayloadDto == null)
            {
                return new ApiResponse<CategoryResponseDto>
                {
                    StatusCode = 401,
                    Message = Messages.Unauthorized,
                    Data = null
                };

            }
           
            // 🔍 Check for duplicates within the same company
            var existingCategory = await _categoryRepositary.FindAsync(u =>
                u.CompanyId == tokenPayloadDto.CompanyId &&
                (
                    EF.Functions.ILike(u.CategoryName, dto.CategoryName) ||
                    EF.Functions.ILike(u.CategoryCode, dto.CategoryCode)
                )
            );

            if (existingCategory.Any())
            {
                var duplicateFields = new List<string>();
                foreach (var categories in existingCategory)
                {
                    if (categories.CategoryName.Equals(dto.CategoryName, StringComparison.OrdinalIgnoreCase))
                        duplicateFields.Add("CategoryName");                  
                    if (categories.CategoryCode.Equals(dto.CategoryCode, StringComparison.OrdinalIgnoreCase))
                        duplicateFields.Add("CategoryCode");
                }

                return new ApiResponse<CategoryResponseDto>
                {
                    StatusCode = 409,
                    Message = $"Duplicate value(s) found: {string.Join(", ", duplicateFields)}.",
                    Data = null
                };
            }

            var category = new ServiceCategory
            {
                Id = Guid.NewGuid(),
                CompanyId = tokenPayloadDto.CompanyId,
                CategoryName = dto.CategoryName,
                CategoryCode = dto.CategoryCode,
                Description = dto.Description,
                Status = dto.Status
            };

            await _categoryRepositary.AddAsync(category);

            var response = new CategoryResponseDto
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                CategoryCode = category.CategoryCode,
                Description = category.Description,
                Status = category.Status
            };

            return new ApiResponse<CategoryResponseDto>
            {
                StatusCode = 201,
                Message = Messages.CategoryCreate(category.CategoryName),
                Data = response
            };
        }

        public async Task<ApiResponse<CategoryResponseDto>> Delete(Guid id)
        {
            TokenPayloadDto tokenPayloadDto = _tokenService.GetUserTokenPayload();

            if (tokenPayloadDto == null)
            {
                return new ApiResponse<CategoryResponseDto>
                {
                    StatusCode = 401,
                    Message = Messages.Unauthorized,
                    Data = null
                };
            }

            // Find the category
            var category = await _categoryRepositary.GetByIdAsync(id);

            if (category == null)
            {
                return new ApiResponse<CategoryResponseDto>
                {
                    StatusCode = 404,
                    Message = Messages.NotFound,
                    Data = null
                };
            }

            // Delete the category
            await _categoryRepositary.DeleteAsync(id);

            return new ApiResponse<CategoryResponseDto>
            {
                StatusCode = 200,
                Message = Messages.CategoryDeleted(category.CategoryName),
                Data = null
            };
        }


        public async Task<ApiResponse<PaginationResponse<CategoryResponseDto>>> GetAllAsync(Paginate paginate)
        {
            TokenPayloadDto tokenPayload = _tokenService.GetUserTokenPayload();

            if (tokenPayload == null || tokenPayload.CompanyId == Guid.Empty)
            {
                return new ApiResponse<PaginationResponse<CategoryResponseDto>>
                {
                    StatusCode = 400,
                    Message = Messages.Unauthorized,
                    Data = null
                };
            }

            // Filtering map
            var filterMap = new Dictionary<string, Func<string, Expression<Func<ServiceCategory, bool>>>>
            {
                ["categoryname"] = value => category => EF.Functions.ILike(category.CategoryName, $"%{value}%"),
                ["categorycode"] = value => category => EF.Functions.ILike(category.CategoryCode, $"%{value}%"),
                ["description"] = value => category => EF.Functions.ILike(category.Description, $"%{value}%"),
                ["status"] = value => category => category.Status.ToString().ToLower() == value.ToLower()
            };

            // Sorting map
            var sortMap = new Dictionary<string, Expression<Func<ServiceCategory, object>>>
            {
                ["categoryname"] = category => category.CategoryName,
                ["categorycode"] = category => category.CategoryCode,
                ["description"] = category => category.Description,
                ["status"] = category => category.Status,
            };

            var filterExpressions = FilterAndSortingHelper.GetFilterExpressions(paginate, filterMap) ?? new List<Expression<Func<ServiceCategory, bool>>>();
            filterExpressions ??= new List<Expression<Func<ServiceCategory, bool>>>();
            filterExpressions.Add(category => category.CompanyId == tokenPayload.CompanyId);

            var sortExpression = FilterAndSortingHelper.GetSortExpression(paginate, sortMap);

            var filteredData = await _categoryRepositary.GetFilterAsync(
                pageNumber: paginate.PageNumber,
                pageSize: paginate.PageSize,
                filterExpressions: filterExpressions,
                sortExpression: sortExpression != null ? query => sortExpression(query) : null
            );

            var totalCount = await _categoryRepositary.CountAsync(category => category.CompanyId == tokenPayload.CompanyId);

            var categoryDto = filteredData.Item1.Select(category => new CategoryResponseDto
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                CategoryCode = category.CategoryCode,
                Description = category.Description,
                Status = category.Status,
            }).ToList();

            var paginationResponse = new PaginationResponse<CategoryResponseDto>
            {
                Rows = categoryDto,
                Total = totalCount
            };

            return new ApiResponse<PaginationResponse<CategoryResponseDto>>
            {
                StatusCode = 200,
                Message = Messages.CategoryRetrievedSuccess,
                Data = paginationResponse
            };
        }

        public async Task<ApiResponse<CategoryResponseDto>> GetById(Guid id)
        {
            // ✅ Get token details
            TokenPayloadDto tokenPayloadDto = _tokenService.GetUserTokenPayload();

            if (tokenPayloadDto == null)
            {
                return new ApiResponse<CategoryResponseDto>
                {
                    StatusCode = 401,
                    Message = Messages.Unauthorized,
                    Data = null
                };
            }

            // ✅ Fetch the category by ID and company
            var category = await _categoryRepositary.GetByIdAsync(id);

            if (category == null)
            {
                return new ApiResponse<CategoryResponseDto>
                {
                    StatusCode = 404,
                    Message = Messages.NotFound,
                    Data = null
                };
            }

            var response = new CategoryResponseDto
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                CategoryCode = category.CategoryCode,
                Description = category.Description,
                Status = category.Status
            };

            return new ApiResponse<CategoryResponseDto>
            {
                StatusCode = 200,
                Message = Messages.categoryRetriveedSuccess,
                Data = response
            };

        }

        public Task<ApiResponse<CategoryResponseDto>> Update(Guid id, CategoryRequestDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
