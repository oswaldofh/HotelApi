using HotelApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelApi.Domain.Tokens
{
    public interface ICreateToken
    {
        Task<JwtSecurityToken> GenerateToken(User user);
    }
    public class CreateToken : ICreateToken
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        public CreateToken(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<JwtSecurityToken> GenerateToken(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                   new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                   new Claim(ClaimTypes.Name, user.UserName),
                   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };


            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                _configuration["Tokens:Issuer"],
                _configuration["Tokens:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(15), // 5 SE CONTROLA LA DURACION DEL TOKEN EN DIAS,HORAS,MINUTOS ETC
                signingCredentials: credentials
            );

            return token;
        }

    }
}
