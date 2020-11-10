using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaNotas1.Models
{
    public class AgregarActividad
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Punteo { get; set; }
        public int CodClase { get; set; }
        public int CodActividad { get; set; }
    }
}
