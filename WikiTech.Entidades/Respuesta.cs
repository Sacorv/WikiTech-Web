using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiTech.Entidades
{
    public class Respuesta
    {

        public bool success { get; set; }
        public string message { get; set; }
        public string result { get; set; }

    }
}
