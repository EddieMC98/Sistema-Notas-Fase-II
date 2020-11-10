using System;
using System.Collections.Generic;

namespace SistemaNotas1.Models
{
    public partial class TbClaseAlumno
    {
        public int CodClaseAlumno { get; set; }
        public int? Aprobado { get; set; }
        public int CodAlumno { get; set; }
        public int CodClase { get; set; }
    }
}
