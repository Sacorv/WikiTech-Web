using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiTech.Entidades
{
    public class Colaborador
    {
        public int IdColaborador { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string? Email { get; set; }
    }
}
