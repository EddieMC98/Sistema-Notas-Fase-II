using System;
using System.Collections.Generic;

namespace SistemaNotas1.Models
{
    public partial class TbGrado
    {
        public int CodGrado { get; set; }
        public int CodGradoSiguiente { get; set; }
        public string Nombre { get; set; }
        public int? SiguienteNivel { get; set; }
        public int CodNivel { get; set; }
    }
}
