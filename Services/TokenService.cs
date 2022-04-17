using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Api_Carro.Models;
using System.Collections.Generic;
using System.Linq;

namespace Api_Carro.Services
{
    public static class TokenService
    {

        public static string GenerateToken(Usuario user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor 
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(type:ClaimTypes.Name, value:user.UserName),
                    new Claim(type:ClaimTypes.Role, value:user.Role),
                }),

                Expires = DateTime.UtcNow.AddHours(8), 
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                algorithm: SecurityAlgorithms.HmacSha256Signature) 

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static string GenerateToken(IEnumerable<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor 
            {
                Subject =new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2), 
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                algorithm: SecurityAlgorithms.HmacSha256Signature) 

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static string GenerateRefreshToken(){  //gerando o refresh token através um um número random
            var randomNumber=new byte[32];
            using var rng=RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        //método responsavel por extrair os claims do token antigo
        public static ClaimsPrincipal GetPrincipalExpiredToken(string token){
            var TokenValidationParameters=new TokenValidationParameters{
                ValidateAudience=false,
                ValidateIssuer=false,
                ValidateIssuerSigningKey=true,
                IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Settings.Secret)),
                ValidateLifetime=false
            };

            //desencriptando o token e recuperando os claims
            var tokenHandler = new JwtSecurityTokenHandler();   
            var principal=tokenHandler.ValidateToken(token, TokenValidationParameters, out var securityToken);

            if(securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.
                Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase)) 
                throw new SecurityTokenException("Token Invalido"); 

            return principal;
        }

        //Lista para adicionar os refresh tokens (O mais apropiado aqui é adicionar no banco de dados.)
        private static List<(string, string)> _refreshTokens=new(); 
       
        public static void SaveRefreshToken(string UserName, string refreshToken){
             _refreshTokens.Add(new(UserName,refreshToken));
        }

         public static string GetRefreshToken(string UserName){
            return _refreshTokens.FirstOrDefault(x=>x.Item1==UserName).Item2;
        }
       
         public static void DeleteRefreshToken(string UserName, string refreshToken){
           var item= _refreshTokens.FirstOrDefault(x => x.Item1==UserName && x.Item2==refreshToken);
            _refreshTokens.Remove(item);
        }


    }
}