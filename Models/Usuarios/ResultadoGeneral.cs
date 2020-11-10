using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaNotas1.Models.Usuarios
{
    public class ResultadoGeneral
    {
        public List<PunteoXUnidad> Punteos { get; set; }
        public string Alumno { get; set; }
        public string Clase { get; set; }
        public int CodClase { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }
        public int Contador1 { get; set; }
        public int TotalClases { get; set; }
        public int CodigoAlumno { get; set; }
        public int CodGrado { get; set; }
    }
}
