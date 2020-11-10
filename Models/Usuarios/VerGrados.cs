using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaNotas1.Models.Usuarios
{
    public class VerGrados
    {
        public int CodGrado { get; set; }
        public int CodGradoSiguiente { get; set; }
        public string Nombre { get; set; }
        public int? SiguienteNivel { get; set; }
        public int CodNivel { get; set; }
        public string GradoSiguiente { get; set; }
        public string NombreSiguienteNivel { get; set; }
        public string NombreNivel { get; set; }
    }
}
