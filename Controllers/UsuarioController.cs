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

             return new{
                 usuario=usuario,
                 token=token
             };
        }

        else
        {
             return NotFound("Usuário não Informado.");
        }      
    }
} 

}