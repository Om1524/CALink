using CALink.Application.DTOs.Common_Dto;
using CALink.Application.DTOs.Super_Admin_Dto;
using CALink.Application.Interfaces.Common_Interface;
using CALink.Application.Interfaces.Super_Admin_Interface;
using CALink.Domain.Common;
using CALink.Domain.Entities.Super_Admin;
using CALink.Domain.Entities.User_Management;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static CALink.Domain.Enums.Enums;

namespace CALink.Application.Services.Super_Admin_Service
{
    public class CompanyService : ICompanyService
    {
        private readonly IRepository<Company> _companyRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        //private readonly IDefaultMetadataSeeder _defaultMetadataSeeder;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;

        public CompanyService(IRepository<Company> companyRepository, ITokenService tokenService, IRepository<User> userRepository,
            IRepository<Role> roleRepository, IConfiguration config, IEmailService emailService)
        {
            _companyRepository = companyRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _tokenService = tokenService;
            //_defaultMetadataSeeder = defaultMetadataSeeder;
            _emailService = emailService;
            _config = config;
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

            //await _defaultMetadataSeeder.SeedAsync(company.Id);
            // ✅ Check if SuperAdmin role exists for this company
            var predicate = PredicateBuilder.New<Role>(true);
            predicate = predicate.And(role => role.RoleName == "SuperAdmin" && role.CompanyId == company.Id);

            Role? existingSuperAdminRole = (await _roleRepository.FindAsync(predicate)).FirstOrDefault();

            Guid superAdminRoleId;

            if (existingSuperAdminRole != null)
            {
                superAdminRoleId = existingSuperAdminRole.Id;
            }
            else
            {
                // Create SuperAdmin role for the new company
                var superAdminRole = new Role
                {
                    Id = Guid.NewGuid(),
                    CompanyId = company.Id,
                    RoleName = "SuperAdmin",
                    Description = "Super Admin Role with full permissions",
                    Status = RoleStatus.Active, // Active
                    CreatedBy = tokenPayload.UserId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedBy = tokenPayload.UserId,
                    UpdatedAt = DateTime.UtcNow
                };
                await _roleRepository.AddAsync(superAdminRole);
                superAdminRoleId = superAdminRole.Id;
            }

            if(companyRequestDto.Users != null && companyRequestDto.Users.Any())
            {
                foreach (var userDto in companyRequestDto.Users)
                {
                    //var existingUser = await _userRepository.FindAsync(u => u.Email == userDto.Email || u.MobileNumber == userDto.MobileNumber);
                    //if (existingUser.Any())
                    //{
                    //    continue; // Skip existing users
                    //}
                    string generatedPassword = PasswordHelper.PasswordGenerator();
                    var user = new User
                    {
                        Id = Guid.NewGuid(),
                        CompanyId = company.Id,
                        FirstName = userDto.FirstName,
                        LastName = userDto.LastName,
                        Email = userDto.Email,
                        MobileNumber = userDto.MobileNumber,
                        Password = generatedPassword, // Default password, should be changed on first login
                        RoleId = superAdminRoleId, // Assign SuperAdmin role
                        Status = (UserStatus)userDto.status,                        
                        CreatedBy = tokenPayload.UserId,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _userRepository.AddAsync(user);

                    // ✅ Determine LoginUrl based on connection string
                    var connectionString = _config.GetConnectionString("DefaultConnection");
                    string loginUrl = EnvironmentUrls.GetUserLoginUrl(connectionString);

                    string emailBody = EmailTemplateHelper.GetCompanyAdminAccountCreationEmailBody(
                        user.FirstName, 
                        user.Email, 
                        companyRequestDto.Code,
                        generatedPassword, 
                        loginUrl
                    );

                    // Send email to the user with their credentials
                    await _emailService.SendAsync( new EmailDto
                    {
                        To = user.Email,
                        Subject = "Welcome to CALink - Your Admin Account Has Been Created",
                        Body = emailBody,
                        IsBodyHtml = true
                    });
                }
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
