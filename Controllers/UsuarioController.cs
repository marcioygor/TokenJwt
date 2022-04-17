using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Net;
using System.Resources;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Api_Carro.Models;
using Api_Carro.Data;
using Api_Carro.Services;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;


namespace Api_Carro.Controllers
{

    [ApiController]
    [Route("v1/usuario")]
    public class UsuarioController: ControllerBase
    {
        private readonly DataContext _context;

        public UsuarioController(DataContext context)
        {
            this._context = context;
        }

        [HttpPost]
        [Route("cadastro")]
        public async Task<ActionResult<Usuario>> Post(
              [FromServices] DataContext context,
              [FromBody] Usuario model
        )
        {
            if (ModelState.IsValid)
            {
                context.Usuarios.Add(model);
                await context.SaveChangesAsync(); 
                model.Password="";
                return model;
            }

            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("login")]
        
        public async Task<ActionResult <dynamic>> Login( //não retorna um tipo especifico
              [FromServices] DataContext context,
              [FromBody] Usuario model)
        {
            var usuario=model;

            if(usuario!=null){
               usuario=context.Usuarios.FirstOrDefault(x => x.UserName == usuario.UserName && x.Password==usuario.Password);

            if(usuario==null)
                return NotFound("Usuário não localizado na base de dados.");

            var token=TokenService.GenerateToken(usuario);

            var refreshToken=TokenService.GenerateRefreshToken();
            TokenService.SaveRefreshToken(usuario.UserName, refreshToken);

             return new{
                 usuario=usuario,
                 token=token,
                 refreshToken=refreshToken
             };
        }

        else
        {
             return NotFound("Usuário não Informado.");
        }      
    }

    [HttpPost]
    [Route("refresh")]

     public async Task<ActionResult <dynamic>> Refresh(string token, string refreshToken){
         var principal=TokenService.GetPrincipalExpiredToken(token);
         var UserName=principal.Identity.Name;
         var savedRefreshToken=TokenService.GetRefreshToken(UserName);

         if(savedRefreshToken!=refreshToken)
            throw new SecurityTokenException("Token Invalido"); 
         
         var newJwtToken=TokenService.GenerateToken(principal.Claims);
         var newRefreshToken=TokenService.GenerateRefreshToken();
         TokenService.DeleteRefreshToken(UserName, refreshToken);
         TokenService.SaveRefreshToken(UserName, newRefreshToken);

           return new{
                 token=newJwtToken,
                 refreshToken=newRefreshToken
             };
     }
        
} 

}