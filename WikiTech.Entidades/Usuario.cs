using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiTech.Entidades
{
    public class Usuario
    {
       
        public string? nombre { get; set; }

        public string? apellido { get; set; }

        [Required(ErrorMessage = "Este dato es obligatorio")]
        public string? email { get; set; }

        [Required(ErrorMessage = "Este dato es obligatorio")]
        public string? contrasenia { get; set; }

    }
}
