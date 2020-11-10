using System;
using System.Collections.Generic;

namespace SistemaNotas1.Models
{
    public partial class TbAlumnoPadreDeFamilia
    {
        public int CodAlumnoPadreDeFamilia { get; set; }
        public int CodAlumno { get; set; }
        public int CodPadreDeFamilia { get; set; }
    }
}
