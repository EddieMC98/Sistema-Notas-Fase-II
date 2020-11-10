using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaNotas1.Models.Usuarios
{
    public class AgregarGrados
    {
        public int? CodGradoSiguiente { get; set; }
        public string Nombre { get; set; }
        public int? SiguienteNivel { get; set; }
        public int? CodNivel { get; set; }

        public IEnumerable<SelectListItem> ListaGrados { get; set; }
        public IEnumerable<SelectListItem> ListaNiveles { get; set; }
        public IEnumerable<SelectListItem> ListaSigNiveles { get; set; }
    }
}
