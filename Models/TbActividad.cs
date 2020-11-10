using System;
using System.Collections.Generic;

namespace SistemaNotas1.Models
{
    public partial class TbActividad
    {
        public int CodActividad { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Punteo { get; set; }
        public int CodClase { get; set; }
    }
}
