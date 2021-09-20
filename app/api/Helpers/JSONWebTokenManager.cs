using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Models.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace api.Helpers
{
    public static class JSONWebTokenManager
    {
        /// <summary>Creates the json web token that should be used for authentication.</summary>
        /// <param name="userLogged">Representation of the authenticated user.</param>
        /// <param name="configuration">IConfiguration injection for reading 'generation key'.</param>
        /// <returns>The token which should be sent within every request as identity proving.</returns>
        public static string CreateJWT(User userLogged, IConfiguration configuration)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(configuration["JWTKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userLogged.username),
                    new Claim(ClaimTypes.NameIdentifier, userLogged.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}