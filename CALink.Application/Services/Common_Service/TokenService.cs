using CALink.Application.DTOs.Common_Dto;
using CALink.Application.Interfaces.Common_Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text;
using System.Threading.Tasks;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;


namespace CALink.Application.Services.Common_Service
{
    public class JwtSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpirationHours { get; set; } = 24; // Default value, can be overridden by config
    }

    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TokenService(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GenerateTokenAdmin(AdminTokenPayloadDto adminTokenPayloadDto)
        {
            // Read settings from config
            var jwtSettings = new JwtSettings
            {
                Secret = _config.GetSection("JwtSettings:Secret").Value,
                Issuer = _config.GetSection("JwtSettings:Issuer").Value,
                Audience = _config.GetSection("JwtSettings:Audience").Value,
                ExpirationHours = int.TryParse(_config.GetSection("JwtSettings:ExpirationHours").Value, out var expiration) ? expiration : 24
            };

            // Validate settings
            var secret = jwtSettings.Secret ?? throw new ArgumentNullException("JwtSettings:Secret not configured.");
            var issuer = jwtSettings.Issuer ?? throw new ArgumentNullException("JwtSettings:Issuer not configured.");
            var audience = jwtSettings.Audience ?? throw new ArgumentNullException("JwtSettings:Audience not configured.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var claims = new List<Claim>
            {
                        // Standard JWT claims
                        new Claim(JwtRegisteredClaimNames.Sub, adminTokenPayloadDto.UserId.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim("uid", adminTokenPayloadDto.UserId.ToString()),
                        new Claim("email", adminTokenPayloadDto.Email ?? string.Empty),
                        new Claim("firstname", adminTokenPayloadDto.FirstName ?? string.Empty),
                        new Claim("lastname", adminTokenPayloadDto.LastName ?? string.Empty),
            };

            // Build token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(jwtSettings.ExpirationHours),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateTokenUser(TokenPayloadDto tokenPayloadDto)
        {
            var jwtSettings = new JwtSettings
            {
                Secret = _config.GetSection("JwtSettings:Secret").Value,
                Issuer = _config.GetSection("JwtSettings:Issuer").Value,
                Audience = _config.GetSection("JwtSettings:Audience").Value,
                ExpirationHours = int.TryParse(_config.GetSection("JwtSettings:ExpirationHours").Value, out var expiration) ? expiration : 24
            };

            // Validate essential settings are present
            var secret = jwtSettings.Secret ?? throw new ArgumentNullException("JwtSettings:Secret not configured.");
            var issuer = jwtSettings.Issuer ?? throw new ArgumentNullException("JwtSettings:Issuer not configured.");
            var audience = jwtSettings.Audience ?? throw new ArgumentNullException("JwtSettings:Audience not configured.");

            var key = Encoding.ASCII.GetBytes(secret);

            var claims = new List<Claim>
            {
                        // Standard JWT claims
                        new Claim(JwtRegisteredClaimNames.Sub, tokenPayloadDto.UserId.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim("uid", tokenPayloadDto.UserId.ToString()),
                        new Claim("cid", tokenPayloadDto.CompanyId.ToString()),
                        new Claim("firstname", tokenPayloadDto.FirstName ?? string.Empty),
                        new Claim("code", tokenPayloadDto.Code.ToString()),
                        new Claim("email", tokenPayloadDto.Email ?? string.Empty),                     
            };

            // Configure the token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(jwtSettings.ExpirationHours), // Token expiration time
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public AdminTokenPayloadDto GetAdminTokenPayload()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null)
                throw new UnauthorizedAccessException("User context is not available.");

            var userIdStr = user.FindFirst("uid")?.Value;
            if (!Guid.TryParse(userIdStr, out var userId))
                throw new UnauthorizedAccessException("Invalid or missing user ID in token.");

            var firstName = user.FindFirst("firstname")?.Value;
            var lastName = user.FindFirst("lastname")?.Value;
            var email = user.FindFirst("email")?.Value
                        ?? user.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                throw new UnauthorizedAccessException("Invalid or missing email in token.");

            return new AdminTokenPayloadDto
            {
                UserId = userId,
                Email = email,
                FirstName = firstName ?? string.Empty,
                LastName = lastName ?? string.Empty
            };
        }

        public TokenPayloadDto GetUserTokenPayload()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null)
            {
                throw new UnauthorizedAccessException("User context is not available.");
            }
            var companyIdStr = user.FindFirst("cid")?.Value;

            if (!Guid.TryParse(companyIdStr, out var companyId))
            {
                throw new UnauthorizedAccessException("Invalid or missing company ID in token.");
            }

            var userIdStr = user.FindFirst("uid")?.Value;
            if (!Guid.TryParse(userIdStr, out var userId))
            {
                throw new UnauthorizedAccessException("Invalid or missing user ID in token.");
            }

            var code = user.FindFirst("code")?.Value;
            if (string.IsNullOrEmpty(code))
            {
                throw new UnauthorizedAccessException("Invalid or missing code in token.");
            }

            var firstName = user.FindFirst("firstname")?.Value;
            var email = user.FindFirst("email")?.Value; // Optional: Get email if needed           

            return new TokenPayloadDto
            {
                UserId = userId,
                CompanyId = companyId,
                FirstName = firstName,
                Code = code,
                Email = email,
            };
        }
    }
}