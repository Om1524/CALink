using CALink.Application.DTOs.Common_Dto;
using CALink.Application.Interfaces.Common_Interface;
using CALink.Domain.Common;
using CALink.Domain.Entities.Super_Admin;
using CALink.Domain.Entities.User_Management;
using LinqKit;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CALink.Domain.Enums.Enums;

namespace CALink.Application.Services.Common_Service
{
    public class AuthService : IAuthService
    {

        private readonly IRepository<AppUser> _appUserRepository;
        private readonly IRepository<Company> _companyRepository;
        private readonly IRepository<User> _userRepository;
        private readonly ITokenService _tokenService;


        public AuthService(IRepository<AppUser> appUserRepository, IRepository<Company> companyRepository , IRepository<User> userRepository, ITokenService tokenService = null)
        {
            _appUserRepository = appUserRepository;
            _tokenService = tokenService;
            _companyRepository = companyRepository;
            _userRepository = userRepository ;
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<LoginResponseDto>> Login(LoginUserDto loginUserDto)
        {

            var predicateCompany = PredicateBuilder.New<Company>(true);
            predicateCompany = predicateCompany.And(company => company.Code == loginUserDto.SecratCode);

            Company? company = (await _companyRepository.FindAsync(predicateCompany)).FirstOrDefault();

            if (company == null)
            {
                return new ApiResponse<LoginResponseDto>
                {
                    StatusCode = 404,
                    Message = Messages.IncorrectSecratCode, // Secrat code is incorrect
                    Data = null
                };
            }

            if (!company.IsActive)
            {
                return new ApiResponse<LoginResponseDto>
                {
                    StatusCode = 404,
                    Message = Messages.CompanyUnActive, // Company is not active
                    Data = null
                };
            }

            User? user = (await _userRepository.FindAsync(user => user.Email == loginUserDto.Email && user.CompanyId == company.Id)).FirstOrDefault();

            if (user == null || loginUserDto.Password != user.Password)
            {
                return new ApiResponse<LoginResponseDto>
                {
                    StatusCode = 404,
                    Message = Messages.IncorrectEmailPass, // Email or Password incorrect
                    Data = null
                };
            }

            if (user.Status != UserStatus.Active)
            {
                return new ApiResponse<LoginResponseDto>
                {
                    StatusCode = 404,
                    Message = Messages.UnActive, // User is not active
                    Data = null
                };
            }

            user.LastLogin = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            var tokenPayload = new TokenPayloadDto
            {
                UserId = user.Id,
                CompanyId = user.CompanyId,
                Email = user.Email,
                FirstName = user.FirstName,
                Code = company.Code ?? string.Empty
            };

            var token = _tokenService.GenerateTokenUser(tokenPayload);

            var loginResponse = new LoginResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                MobileNumber = user.MobileNumber,
                RoleName = user.Role != null ? user.Role.RoleName : string.Empty,
                Status = user.Status,
                Token = token,
                IsFirstLogin = user.IsFirstLogin
            };

            return new ApiResponse<LoginResponseDto>
            {
                StatusCode = 200,
                Message = user.IsFirstLogin ? "First login - password change required" : "Login successful",
                IsLogoutRequired = false,
                Data = loginResponse
            };
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
