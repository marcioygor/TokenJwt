using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Api_Carro.Models
{

    public class Carro
    {
        [Key]
        public int Id { get; set; }
        public Marca Marca { get; set; }

        [MaxLength(60, ErrorMessage = "Este campo deve conter no máximo 60 caracteres")]
        [Required(ErrorMessage = "Este campo é obrigatório")]

        public string Modelo { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public int MarcaId { get; set; }

    }


}