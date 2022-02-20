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
        public async Task<ActionResult> GetCarros()
        {
            var carros = await _context.Carros.ToListAsync();
            return Ok(carros);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Carro>> Post(
              [FromServices] DataContext context,
              [FromBody] Carro model
        )
        {
            if (ModelState.IsValid)
            {
                context.Carros.Add(model);
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
                context.Carros.Remove(context.Carros.Find(id));
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