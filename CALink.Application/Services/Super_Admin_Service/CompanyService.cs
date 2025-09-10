using CALink.Application.DTOs.Common_Dto;
using CALink.Application.DTOs.Super_Admin_Dto;
using CALink.Application.Interfaces.Common_Interface;
using CALink.Application.Interfaces.Super_Admin_Interface;
using CALink.Domain.Common;
using CALink.Domain.Entities.Super_Admin;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Application.Services.Super_Admin_Service
{
    public class CompanyService : ICompanyService
    {
        private readonly IRepository<Company> _companyRepository;
        private readonly ITokenService _tokenService;

        public CompanyService(IRepository<Company> companyRepository, ITokenService tokenService)
        {
            _companyRepository = companyRepository;
            _tokenService = tokenService;
        }

        public async Task<ApiResponse<CompanyResponseDto>> Create(CompanyRequestDto companyRequestDto)
        {
            AdminTokenPayloadDto tokenPayload;
            try
            {
                tokenPayload = _tokenService.GetAdminTokenPayload();
            }
            catch (UnauthorizedAccessException ex)
            {
                return new ApiResponse<CompanyResponseDto>
                {
                    StatusCode = 401,
                    Message = Messages.Unauthorized,
                    Data = null
                };
            }

            if (companyRequestDto == null)
            {
                return new ApiResponse<CompanyResponseDto>
                {
                    StatusCode = 400,
                    Message = Messages.InvalidData,
                    Data = null
                };
            }

            var existingCompany = await _companyRepository.FindAsync(c => c.Name == companyRequestDto.Name 
                        || c.Email == companyRequestDto.Email
                        || c.Phone == companyRequestDto.Phone
                        || c.Code == companyRequestDto.Code
                        || c.Tenant == companyRequestDto.Tenant
            );

            if (existingCompany.Any())
            {
                var duplicates = new List<string>();
                var duplicatecompany = existingCompany.First();

                if (duplicatecompany.Name == companyRequestDto.Name) duplicates.Add("Name");
                if (duplicatecompany.Tenant == companyRequestDto.Tenant) duplicates.Add("Tenant");
                if (duplicatecompany.Code == companyRequestDto.Code) duplicates.Add("Code");
                if (duplicatecompany.Email == companyRequestDto.Email) duplicates.Add("Email");
                if (duplicatecompany.Phone == companyRequestDto.Phone) duplicates.Add("Phone");                

                return new ApiResponse<CompanyResponseDto>
                {
                    StatusCode = 409,
                    Message = $"Company with the same {string.Join(", ", duplicates)} already exists.",
                    Data = null
                };
            }

            var company = new Company
            {
                Id = Guid.NewGuid(),
                Name = companyRequestDto.Name,
                Code = companyRequestDto.Code,
                Tenant = companyRequestDto.Tenant,
                Email = companyRequestDto.Email,
                Phone = companyRequestDto.Phone,
                Address = companyRequestDto.Address,
                City = companyRequestDto.City,
                State = companyRequestDto.State,
                Country = companyRequestDto.Country,
                ZipCode = companyRequestDto.ZipCode,
                IsActive = true,
                CreatedBy = tokenPayload.UserId,
                CreatedAt = DateTime.UtcNow
            };

            await _companyRepository.AddAsync(company);

            var companyResponseDto = new CompanyResponseDto
            {
                Id = company.Id,
                Name = company.Name,
                Code = company.Code,
                Tenant = company.Tenant,
                Email = company.Email,
                Phone = company.Phone,
                Address = company.Address,
                City = company.City,
                State = company.State,
                Country = company.Country,
                ZipCode = company.ZipCode,
                IsActive = company.IsActive,
                CreatedAt = company.CreatedAt,
               UpdatedAt = company.UpdatedAt
            };

            return new ApiResponse<CompanyResponseDto>
            {
                StatusCode = 201,
                Message = Messages.CompanyCreate(company.Name),
                Data = companyResponseDto
            };           
        }

        public async Task<ApiResponse<CompanyResponseDto>> Delete(Guid id)
        {
            AdminTokenPayloadDto tokenPayload;
            try
            {
                tokenPayload = _tokenService.GetAdminTokenPayload();
            }
            catch (UnauthorizedAccessException ex)
            {
                return new ApiResponse<CompanyResponseDto>
                {
                    StatusCode = 401,
                    Message = Messages.Unauthorized,
                    Data = null
                };
            }

            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null)
            {
                return new ApiResponse<CompanyResponseDto>
                {
                    StatusCode = 404,
                    Message = Messages.CompanyNotFound,
                    Data = null
                };
            }

            //var users = await _userRepository.FindAsync(user => user.CompanyId == id);
            //if (users != null && users.Any())
            //{
            //    foreach (var user in users)
            //    {
            //        await _userRepository.DeleteAsync(user.Id);
            //    }
            //}

            await _companyRepository.DeleteAsync(id);
            return new ApiResponse<CompanyResponseDto>
            {
                StatusCode = 200,
                Message = Messages.CompanyDeleted(company.Name),
                Data = null
            };            
        }

        public async Task<ApiResponse<PaginationResponse<CompanyResponseDto>>> GetAllAsync(Paginate paginate)
        {
            AdminTokenPayloadDto tokenPayload;
            try
            {
                tokenPayload = _tokenService.GetAdminTokenPayload();
            }
            catch (UnauthorizedAccessException ex)
            {
                return new ApiResponse<PaginationResponse<CompanyResponseDto>>
                {
                    StatusCode = 401,
                    Message = Messages.Unauthorized,
                    Data = null
                };
            }

            // Define filter mapping
            var filterMap = new Dictionary<string, Func<string, Expression<Func<Company, bool>>>>
            {
                { "name", val => c => EF.Functions.ILike(c.Name, $"%{val}%") },
                { "code", val => c => EF.Functions.ILike(c.Code, $"%{val}%") },
                { "email", val => c => EF.Functions.ILike(c.Email, $"%{val}%") },
                { "phone", val => c => EF.Functions.ILike(c.Phone, $"%{val}%") },
                { "tenant", val => c => EF.Functions.ILike(c.Tenant, $"%{val}%") },              
                { "address", val => c => EF.Functions.ILike(c.Address, $"%{val}%") },
                { "city", val => c => EF.Functions.ILike(c.City, $"%{val}%") },
                { "state", val => c => EF.Functions.ILike(c.State, $"%{val}%") },
                { "country", val => c => EF.Functions.ILike(c.Country, $"%{val}%") },
                { "zipcode", val => c => EF.Functions.ILike(c.ZipCode, $"%{val}%") },
                { "isActive", val => c => c.IsActive.ToString().ToLower() == val.ToLower() }
            };

            // Define sort mapping
            var sortMap = new Dictionary<string, Expression<Func<Company, object>>>
            {
                { "name", c => c.Name },
                { "email", c => c.Email },
                { "phone", c => c.Phone },
                { "tenant", c => c.Tenant },              
                { "address", c => c.Address },
                { "city", c => c.City },
                { "state", c => c.State },
                { "country", c => c.Country },
                { "zipcode", c => c.ZipCode },
                { "isActive", c => c.IsActive }
            };

            // Use utility class
            var filterExpressions = FilterAndSortingHelper.GetFilterExpressions(paginate, filterMap);
            var sortExpression = FilterAndSortingHelper.GetSortExpression(paginate, sortMap);

            var companyResult = await _companyRepository.GetFilterAsync(
                pageNumber: paginate.PageNumber,
                pageSize: paginate.PageSize,
                filterExpressions: filterExpressions,
                sortExpression: sortExpression
            );

            var companyDtos = companyResult.Item1.Select(company => new CompanyResponseDto
            {
                Id = company.Id,
                Name = company.Name,
                Code = company.Code,
                Tenant = company.Tenant,
                Email = company.Email,
                Phone = company.Phone,
                Address = company.Address,
                City = company.City,
                State = company.State,
                Country = company.Country,
                ZipCode = company.ZipCode,
                IsActive = company.IsActive,
                CreatedAt = company.CreatedAt,
                UpdatedAt = company.UpdatedAt
            }).ToList();

            var paginationResponse = new PaginationResponse<CompanyResponseDto>
            {
                Rows = companyDtos,
                Total = companyResult.Item2
            };

            return new ApiResponse<PaginationResponse<CompanyResponseDto>>
            {
                StatusCode = 200,
                Message = Messages.CompaniesRetrievedSuccess,
                Data = paginationResponse
            };            
        }

        public async Task<ApiResponse<CompanyResponseDto>> GetById(Guid id)
        {
            AdminTokenPayloadDto tokenPayload;
            try
            {
                tokenPayload = _tokenService.GetAdminTokenPayload();
            }
            catch (UnauthorizedAccessException ex)
            {
                return new ApiResponse<CompanyResponseDto>
                {
                    StatusCode = 401,
                    Message = Messages.Unauthorized,
                    Data = null
                };
            }

            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null) {
                return new ApiResponse<CompanyResponseDto>
                {
                    StatusCode = 404,
                    Message = Messages.CompanyNotFound,
                    Data = null
                };
            }

            var companyResponseDto = new CompanyResponseDto
            {
                Id = company.Id,
                Name = company.Name,
                Code = company.Code,
                Tenant = company.Tenant,
                Email = company.Email,
                Phone = company.Phone,
                Address = company.Address,
                City = company.City,
                State = company.State,
                Country = company.Country,
                ZipCode = company.ZipCode,
                IsActive = company.IsActive,
                CreatedAt = company.CreatedAt,
               UpdatedAt = company.UpdatedAt
            };

            return new ApiResponse<CompanyResponseDto>
            {
                StatusCode = 200,
                Message = Messages.CompanyRetrievedSuccess,
                Data = companyResponseDto
            };
        }

        public async Task<ApiResponse<CompanyResponseDto>> Update(Guid id, CompanyRequestDto companyRequestDto)
        {
            AdminTokenPayloadDto tokenPayload;
            try
            {
                tokenPayload = _tokenService.GetAdminTokenPayload();
            }
            catch (UnauthorizedAccessException ex)
            {
                return new ApiResponse<CompanyResponseDto>
                {
                    StatusCode = 401,
                    Message = Messages.Unauthorized,
                    Data = null
                };
            }

            throw new NotImplementedException();
        }
    }
}
