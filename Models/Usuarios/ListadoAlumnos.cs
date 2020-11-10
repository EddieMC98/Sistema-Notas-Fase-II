using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaNotas1.Models.Usuarios
{
    public class ListadoAlumnos
    {
        public int CodGrado { get; set; }
        public string Nombre { get; set; }
        public int CodAlumno { get; set; }
        public List<ListaCursos> lista_cursos { get; set; }
    }
}
