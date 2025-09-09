using CALink.Application.DTOs.Common_Dto;
using CALink.Application.Interfaces.Common_Interface;
using CALink.Domain.Common;
using CALink.Domain.Entities.Super_Admin;
using LinqKit;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Application.Services.Common_Service
{
    public class AuthService : IAuthService
    {

        private readonly IRepository<AppUser> _appUserRepository;
        private readonly ITokenService _tokenService;


        public AuthService(IRepository<AppUser> appUserRepository, ITokenService tokenService = null)
        {
            _appUserRepository = appUserRepository;
            _tokenService = tokenService;
        }

        public async Task<ApiResponse<AppUserLoginResponseDto>> LoginAdmin(AppUserLoginRequestDto appUserLoginRequestDto)
        {
            var predicate = PredicateBuilder.New<AppUser>(true);
            predicate = predicate.And(appUser => appUser.Email == appUserLoginRequestDto.Email);

            AppUser? appUser = (await _appUserRepository.FindAsync(predicate)).FirstOrDefault();


            if (appUserLoginRequestDto.Email != appUser.Email || appUserLoginRequestDto.Password != appUser.Password)
            {
                return new ApiResponse<AppUserLoginResponseDto>
                {
                    StatusCode = 404,
                    Message = Messages.IncorrectEmailPass, // Email or Password incorrect
                    Data = null
                };
            }

            if (!appUser.IsActive)
            {               
                return new ApiResponse<AppUserLoginResponseDto>
                {
                    StatusCode = 404,
                    Message = Messages.UnActive,
                    Data = null
                };
            }


            // update last login time
            appUser.LastLogin = DateTime.UtcNow;
            
            await _appUserRepository.UpdateAsync(appUser);

            AppUserLoginResponseDto user = new AppUserLoginResponseDto
            {
                Id = appUser.Id,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                Email = appUser.Email,
                IsActive = appUser.IsActive,
            };

            AdminTokenPayloadDto tokenPayload = new AdminTokenPayloadDto
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            var token = _tokenService.GenerateTokenAdmin(tokenPayload);

            var loginResponse = new AppUserLoginResponseDto
            {
                Id = appUser.Id,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                Email = appUser.Email,
                IsActive = appUser.IsActive,
                Token = token,
            };

            return new ApiResponse<AppUserLoginResponseDto>
            {
                StatusCode = 200,
                Message = Messages.LoginSuccessful,
                Data = loginResponse
            };

        }
    }
}
