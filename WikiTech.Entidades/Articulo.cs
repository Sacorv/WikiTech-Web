using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiTech.Entidades
{
    public class Articulo
    {
        public int id { get; set; }

        [Required(ErrorMessage = "Este dato es obligatorio")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "Este dato es obligatorio")]
        public string Contenido { get; set; }

        [Required(ErrorMessage = "Este dato es obligatorio")]
        public DateTime Fecha { get; set; }

        public int Puntaje { get; set; }

        public string? ImagenPrincipal { get; set; }

        public int IdColaborador { get; set; }

        public int IdCategoria { get; set; }

        //public static explicit operator Articulo(Task<List<Articulo>> v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
