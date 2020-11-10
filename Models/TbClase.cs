using System;
using System.Collections.Generic;

namespace SistemaNotas1.Models
{
    public partial class TbClase
    {
        public int CodClase { get; set; }
        public int CodUnidad { get; set; }
        public int CodAño { get; set; }
        public int CodJornada { get; set; }
        public int CodCurso { get; set; }
        public int CodDocente { get; set; }
        public int CodSeccion { get; set; }
    }
}
