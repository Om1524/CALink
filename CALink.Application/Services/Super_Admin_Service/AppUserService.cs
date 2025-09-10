using CALink.Application.DTOs.Common_Dto;
using CALink.Application.DTOs.Super_Admin_Dto;
using CALink.Application.Interfaces.Common_Interface;
using CALink.Application.Interfaces.Super_Admin_Interface;
using CALink.Domain.Common;
using CALink.Domain.Entities.Super_Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Application.Services.Super_Admin_Service
{
    public class AppUserService : IAppUserService
    {
        private readonly IRepository<AppUser> _appUserRepository;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;

        public AppUserService(IRepository<AppUser> appUserRepository, IEmailService emailService, IConfiguration config,ITokenService tokenService) 
        {
            _appUserRepository = appUserRepository;
            _emailService = emailService;
            _tokenService = tokenService;
            _config = config;
        }

        public async Task<ApiResponse<AppUserResponseDto>> Create(AppUserRequestDto appUserRequestDto)
        {
            //AdminTokenPayloadDto tokenPayload = _tokenService.GetAdminTokenPayload();
            AdminTokenPayloadDto tokenPayload;
            try
            {
                tokenPayload = _tokenService.GetAdminTokenPayload();
            }
            catch (UnauthorizedAccessException ex)
            {
                return new ApiResponse<AppUserResponseDto>
                {
                    StatusCode = 401, 
                    Message = Messages.Unauthorized,
                    Data = null
                };
            }
            if (appUserRequestDto == null)
            {
                return new ApiResponse<AppUserResponseDto>
                {
                    StatusCode = 400,
                    Message = Messages.InvalidData,
                    Data = null
                };
            }

            // 🔍 Check for existing email
            var existingUser = (await _appUserRepository
                .FindAsync(u => u.Email.ToLower() == appUserRequestDto.Email.ToLower()))
                .FirstOrDefault();

            if (existingUser != null)
            {
                return new ApiResponse<AppUserResponseDto>
                {
                    StatusCode = 409,
                    Message = Messages.EmailAlreadyExists,
                    Data = null
                };
            }


            string generatedPassword = PasswordHelper.PasswordGenerator();
            var appUser = new AppUser
            {
                FirstName = appUserRequestDto.FirstName,
                LastName = appUserRequestDto.LastName,
                Email = appUserRequestDto.Email,
                Password = generatedPassword,
                IsActive = appUserRequestDto.IsActive,
                CreatedBy = tokenPayload.UserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
               
            };

            await _appUserRepository.AddAsync(appUser);
            var connectionString = _config.GetConnectionString("DefaultConnection");
            string loginUrl = EnvironmentUrls.GetAdminLoginUrl(connectionString);          
            string emailBody = EmailTemplateHelper.GetSuperAdminAccountCreationEmailBody(
                         appUserRequestDto.FirstName,
                         appUserRequestDto.Email,
                         generatedPassword,
                         loginUrl
                     );

            //Send the email
           await _emailService.SendAsync(new EmailDto
           {
               To = appUserRequestDto.Email,
               Subject = "Welcome to CALink - Your Super-Admin Account",
               Body = emailBody,
               IsBodyHtml = true // ✅ Ensure this is set
           });

            var appUserResponseDto = new AppUserResponseDto
            {
                Id = appUser.Id,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                Email = appUser.Email,
                IsActive = appUser.IsActive,
                CreatedAt = appUser.CreatedAt,
                UpdatedAt = appUser.UpdatedAt,            
            };
            return new ApiResponse<AppUserResponseDto>
            {
                StatusCode = 200,
                Message = Messages.AppUserCreate,
                Data = appUserResponseDto
            };
        }
        

        public async Task<ApiResponse<AppUserResponseDto>> Delete(Guid id)
        {
            AdminTokenPayloadDto tokenPayload;
            try
            {
                tokenPayload = _tokenService.GetAdminTokenPayload();
            }
            catch (UnauthorizedAccessException ex)
            {
                return new ApiResponse<AppUserResponseDto>
                {
                    StatusCode = 401,
                    Message = Messages.Unauthorized,
                    Data = null
                };
            }
            var appUser = await _appUserRepository.GetByIdAsync(id);
            if (appUser == null)
            {
                return new ApiResponse<AppUserResponseDto>
                {
                    StatusCode = 404,
                    Message = Messages.AppUserNotFound,
                    Data = null
                };
            }
            await _appUserRepository.DeleteAsync(id);
            return new ApiResponse<AppUserResponseDto>
            {
                StatusCode = 200,
                Message = Messages.AppUserDeleted,
                Data = null
            };
        }

        public async Task<ApiResponse<PaginationResponse<AppUserResponseDto>>> GetAllAsync(Paginate paginate)
        {
            AdminTokenPayloadDto tokenPayload;
            try
            {
                tokenPayload = _tokenService.GetAdminTokenPayload();
            }
            catch (UnauthorizedAccessException ex)
            {
                return new ApiResponse<PaginationResponse<AppUserResponseDto>>
                {
                    StatusCode = 401,
                    Message = Messages.Unauthorized,
                    Data = null
                };
            }
            var filterMap = new Dictionary<string, Func<string, Expression<Func<AppUser, bool>>>>
            {
                ["firstname"] = value => user => EF.Functions.ILike(user.FirstName, $"%{value}%"),
                ["lastname"] = value => user => EF.Functions.ILike(user.LastName, $"%{value}%"),
                ["email"] = value => user => EF.Functions.ILike(user.Email, $"%{value}%"),
            };

            // Define sort mapping
            var sortMap = new Dictionary<string, Expression<Func<AppUser, object>>>
            {
                ["firstname"] = user => user.FirstName,
                ["lastname"] = user => user.LastName,
                ["email"] = user => user.Email,
            };

            var filterExpressions = FilterAndSortingHelper.GetFilterExpressions(paginate, filterMap);
            var sortExpression = FilterAndSortingHelper.GetSortExpression(paginate, sortMap);

            var appUsersResult = await _appUserRepository.GetFilterAsync(
                pageNumber: paginate.PageNumber,
                pageSize: paginate.PageSize,
                filterExpressions: filterExpressions,
                sortExpression: sortExpression
            );

            var appUserDtos = appUsersResult.Item1.Select(appUser => new AppUserResponseDto
            {
                Id = appUser.Id,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                Email = appUser.Email,
                IsActive = appUser.IsActive,
                CreatedAt = appUser.CreatedAt,
                UpdatedAt = appUser.UpdatedAt,              
            }).ToList();

            var paginationResponse = new PaginationResponse<AppUserResponseDto>
            {
                Rows = appUserDtos,
                Total = appUsersResult.Item2
            };

            return new ApiResponse<PaginationResponse<AppUserResponseDto>>
            {
                StatusCode = 200,
                Message = Messages.AppUsersRetrievedSuccess,
                Data = paginationResponse
            };
        }

        public async Task<ApiResponse<AppUserResponseDto>> GetById(Guid id)
        {
            AdminTokenPayloadDto tokenPayload;
            try
            {
                tokenPayload = _tokenService.GetAdminTokenPayload();
            }
            catch (UnauthorizedAccessException ex)
            {
                return new ApiResponse<AppUserResponseDto>
                {
                    StatusCode = 401,
                    Message = Messages.Unauthorized,
                    Data = null
                };
            }
            var appUser = await _appUserRepository.GetByIdAsync(id);
            if (appUser == null)
            {
                return new ApiResponse<AppUserResponseDto>
                {
                    StatusCode = 404,
                    Message = Messages.AppUserNotFound,
                    Data = null
                };
            }
            var appUserDto = new AppUserResponseDto
            {
                Id = appUser.Id,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                Email = appUser.Email,
                CreatedAt = appUser.CreatedAt,
                UpdatedAt = appUser.UpdatedAt,                
            };

            return new ApiResponse<AppUserResponseDto>
            {
                StatusCode = 200,
                Message = Messages.AppUserRetrievedSuccess,
                Data = appUserDto
            };

        }

        public async Task<ApiResponse<AppUserResponseDto>> Update(Guid id, AppUserRequestDto appUserRequestDto)
        {

            AdminTokenPayloadDto tokenPayload;
            try
            {
                tokenPayload = _tokenService.GetAdminTokenPayload();
            }
            catch (UnauthorizedAccessException ex)
            {
                return new ApiResponse<AppUserResponseDto>
                {
                    StatusCode = 401,
                    Message = Messages.Unauthorized,
                    Data = null
                };
            }

            var appUser = await _appUserRepository.GetByIdAsync(id);

            if (appUser == null) {
                return new ApiResponse<AppUserResponseDto>
                {
                    StatusCode = 404,
                    Message = Messages.AppUserNotFound,
                    Data = null
                };
            }

            if (appUserRequestDto == null)
            {
                return new ApiResponse<AppUserResponseDto>
                {
                    StatusCode = 400,
                    Message = Messages.InvalidData,
                    Data = null
                };
            }

            // Check for email uniqueness if the email is being updated
            bool isEmailChanged = !string.Equals(appUser.Email, appUserRequestDto.Email, StringComparison.OrdinalIgnoreCase);
            if (isEmailChanged)
            {
                var existingUser = (await _appUserRepository
                    .FindAsync(u => u.Email.ToLower() == appUserRequestDto.Email.ToLower() && u.Id != id))
                    .FirstOrDefault();
                if (existingUser != null)
                {
                    return new ApiResponse<AppUserResponseDto>
                    {
                        StatusCode = 409,
                        Message = Messages.EmailAlreadyExists,
                        Data = null
                    };
                }
            }

            appUser.FirstName = appUserRequestDto.FirstName;
            appUser.LastName = appUserRequestDto.LastName;
            appUser.Email = appUserRequestDto.Email;
            appUser.IsActive = appUserRequestDto.IsActive;
            appUser.UpdatedAt = DateTime.UtcNow;
            appUser.UpdatedBy = tokenPayload.UserId;
            await _appUserRepository.UpdateAsync(appUser);

            if(isEmailChanged)
            {
                var connectionString = _config.GetConnectionString("DefaultConnection");
                string loginUrl = EnvironmentUrls.GetAdminLoginUrl(connectionString);
                string emailBody = EmailTemplateHelper.GetSuperAdminEmailChangedNotificationBody(
                    appUser.FirstName,
                    appUser.Email,
                    loginUrl
                );
                // Send the email
                await _emailService.SendAsync(new EmailDto
                {
                    To = appUser.Email,
                    Subject = "CALink - Email Address Updated",
                    Body = emailBody,
                    IsBodyHtml = true // ✅ Ensure this is set
                });
            }

            var appUserResponseDto = new AppUserResponseDto
            {
                Id = appUser.Id,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                Email = appUser.Email,
                IsActive = appUser.IsActive,
                CreatedAt = appUser.CreatedAt,
                UpdatedAt = appUser.UpdatedAt,              
            };
            return new ApiResponse<AppUserResponseDto>
            {
                StatusCode = 200,
                Message = Messages.AppUserUpdated,
                Data = appUserResponseDto
            };
        }
    }
}
