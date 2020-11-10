using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaNotas1.Models.Usuarios
{
    public class Asignaciones
    {
        public int? CodAlumno { get; set; }
        public int? CodClase { get; set; }

        public IEnumerable<SelectListItem> ListaAlumno { get; set; }
        public IEnumerable<SelectListItem> ListaClase { get; set; }
    }
}
