using ePizzaHub.Core.Contracts;
using ePizzaHub.Models.ApiModels.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ePizzaHub.Core.Concrete
{
    public class TokenGeneratorService : ITokenGeneratorService
    {
        private readonly IConfiguration _configuration;

        public TokenGeneratorService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToekn(ValidateUserResponse userResponse)
        {
            string secret = _configuration["Jwt:Secret"]!;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity([
                    new Claim(ClaimTypes.Name, userResponse.Name),
                    new Claim(ClaimTypes.Email, userResponse.Email),
                    new Claim("UserId",userResponse.UserId.ToString()),
                    new Claim("IsAdmin",userResponse.Roles.Any(x=>x.Equals("Admin")).ToString()),
                    new Claim("Roles", JsonSerializer.Serialize(userResponse.Roles))
                ]),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["Jwt:TokenExpiryInMinutes"])),
                SigningCredentials = credentials,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
            };

            var TokenHandler = new JsonWebTokenHandler();
            var token = TokenHandler.CreateToken(tokenDescriptor);
            return token;
        }
    }
}
