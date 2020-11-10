using System;
using System.Collections.Generic;

namespace SistemaNotas1.Models
{
    public partial class TbUsuario
    {
        public int CodUsuario { get; set; }
        public string Usuario { get; set; }
        public string Contraseña { get; set; }
        public int CodRol { get; set; }
    }
}
