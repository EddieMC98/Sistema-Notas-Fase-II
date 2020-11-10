using System;
using System.Collections.Generic;

namespace SistemaNotas1.Models
{
    public partial class TbCurso
    {
        public int CodCurso { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int CodGrado { get; set; }
    }
}
