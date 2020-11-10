using System;
using System.Collections.Generic;

namespace SistemaNotas1.Models
{
    public partial class TbAlumno
    {
        public int CodAlumno { get; set; }
        public string Codigo { get; set; }
        public int CodInformacionPersonal { get; set; }
        public int CodUsuario { get; set; }
        public int CodGrado { get; set; }
    }
}
