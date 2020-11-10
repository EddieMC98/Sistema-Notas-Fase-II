using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaNotas1.Models.Usuarios
{
    public class AgregarClase
    {
        public int? CodUnidad { get; set; }
        public int? CodAño { get; set; }
        public int? CodJornada { get; set; }
        public int? CodCurso { get; set; }
        public int? CodDocente { get; set; }
        public int? CodSeccion { get; set; }

        public IEnumerable<SelectListItem> ListaUnidad { get; set; }
        public IEnumerable<SelectListItem> ListaAño { get; set; }
        public IEnumerable<SelectListItem> ListaJornada { get; set; }
        public IEnumerable<SelectListItem> ListaCurso { get; set; }
        public IEnumerable<SelectListItem> ListaDocente { get; set; }
        public IEnumerable<SelectListItem> ListaSeccion { get; set; }

    }
}

