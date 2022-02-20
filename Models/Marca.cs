using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;


namespace Api_Carro.Models
{
    public class Marca
    {

        [Key]
        public int MarcaId { get; set; }
        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(60, ErrorMessage = "Este campo deve conter no máximo 60 caracteres")]
        public string MarcaNome { get; set; }

    }
}