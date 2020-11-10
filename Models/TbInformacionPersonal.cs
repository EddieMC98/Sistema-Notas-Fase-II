using System;
using System.Collections.Generic;

namespace SistemaNotas1.Models
{
    public partial class TbInformacionPersonal
    {
        public int CodInformacionPersonal { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string CorreoElectronico { get; set; }
        public string Direccion { get; set; }
        public byte[] Imagen { get; set; }
        public string Cui { get; set; }
    }
}
