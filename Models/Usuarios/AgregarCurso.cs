using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaNotas1.Models.Usuarios
{
    public class AgregarCurso
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int? CodGrado { get; set; }
        public IEnumerable<SelectListItem> ListaGrados { get; set; }
    }
}
