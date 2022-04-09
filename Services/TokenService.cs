using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Api_Carro.Models;

namespace Api_Carro.Services
{
    public static class TokenService
    {

        public static string GenerateToken(Usuario user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor //informações que o token precisa
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(type:ClaimTypes.Name, value:user.UserName),
                    new Claim(type:ClaimTypes.Role, value:user.Role),
                }),

                Expires = DateTime.UtcNow.AddHours(8), //Definindo o tempo de duração do token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                algorithm: SecurityAlgorithms.HmacSha256Signature) //iniciais para encriptar e decriptar token

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


    }
}