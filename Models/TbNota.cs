using System;
using System.Collections.Generic;

namespace SistemaNotas1.Models
{
    public partial class TbNota
    {
        public int CodNota { get; set; }
        public int CodActividad { get; set; }
        public int CodAlumno { get; set; }
        public decimal? Punteo { get; set; }
    }
}
