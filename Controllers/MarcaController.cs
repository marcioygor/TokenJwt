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
    [Route("v1/marcas")]

    public class MarcaController : ControllerBase
    {
        private readonly DataContext _context;

        public MarcaController(DataContext context)
        {
            this._context = context;
        }

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult> GetMarcas()
        {
            var marcas = await _context.Marcas.ToListAsync();
            return Ok(marcas);
        }

        [HttpPost]
        [Route("")]
        [Authorize]
        public async Task<ActionResult<Marca>> Post(
              [FromServices] DataContext context,
              [FromBody] Marca model
        )
        {
            if (ModelState.IsValid)
            {
                context.Marcas.Add(model);
                await context.SaveChangesAsync();
                return model;
            }

            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles="ADM")] //Apenas Administradores podem fazer remoção.
        public async Task<ActionResult<Carro>> RemoveById([FromServices] DataContext context, int id)
        {
            try
            {
                context.Marcas.Remove(context.Marcas.Find(id));
                context.SaveChanges();
                return Ok("Marca apagada.");
            }
            catch
            {
                return BadRequest("Carro não encontrado");
            }

        }
    }
}