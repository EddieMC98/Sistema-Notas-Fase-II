using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaNotas1.Models.Usuarios
{
    public class CursoxDocente
    {
        public string Unidad { get; set; }

        public string Año { get; set; }
        public string Jornada { get; set; }
        public string Curso { get; set; }
        public string Seccion { get; set; }
        public string Nivel { get; set; }
        public string Grado { get; set; }
        public int CodClase { get; set; }
    }
}
