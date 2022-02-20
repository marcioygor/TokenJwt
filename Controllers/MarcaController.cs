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
        public async Task<ActionResult> GetMarcas()
        {
            var marcas = await _context.Marcas.ToListAsync();
            return Ok(marcas);
        }

        [HttpPost]
        [Route("")]
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

        [HttpPost]
        [Route("{id:int}")]
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