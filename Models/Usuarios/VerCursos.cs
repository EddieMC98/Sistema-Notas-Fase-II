using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaNotas1.Models.Usuarios
{
    public class VerCursos
    {
        public string curso { get; set; }
        public string descripcion { get; set; }
        public string grado { get; set; }
        public string nivel { get; set; }
        public int CodGrado { get; set; }
        public int CodCurso { get; set; }
    }

}
