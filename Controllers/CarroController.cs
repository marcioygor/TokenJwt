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
using Microsoft.AspNetCore.Authorization;

namespace Api_Carro.Controllers
{

    [ApiController]
    [Route("v1/carros")]

    public class CarroController : ControllerBase
    {
        private readonly DataContext _context;

        public CarroController(DataContext context)
        {
            this._context = context;
        }

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult> GetCarros()
        {
            var carros = await _context.Carros.ToListAsync(); //Listando todos os carros
            return Ok(carros);
        }

        [HttpPost]
        [Route("")]
        [Authorize]
        public async Task<ActionResult<Carro>> Post(
              [FromServices] DataContext context,
              [FromBody] Carro model
        )
        {
            if (ModelState.IsValid)
            {
                context.Carros.Add(model);
                await context.SaveChangesAsync();     //Criando um carro
                return model;
            }

            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles="ADM")] 
        public async Task<ActionResult<Carro>> RemoveById([FromServices] DataContext context, int id)
        {
            try
            {
                context.Carros.Remove(context.Carros.Find(id)); //dado um id, remove um carro
                context.SaveChanges();
                return Ok("Carro apagado.");
            }
            catch
            {
                return BadRequest("Carro não encontrado");
            }

        }
    }
}

//eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkpvbiIsInJvbGUiOiJBRE0iLCJuYmYiOjE2NTEzMjc0MTIsImV4cCI6MTY1MTM1NjIxMiwiaWF0IjoxNjUxMzI3NDEyfQ.CBs3F1J5y7y8O4GD38fjQ6kHEgPUoRg0V3EUdnL66Ig