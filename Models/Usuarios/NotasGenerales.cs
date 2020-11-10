using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaNotas1.Models.Usuarios
{
    public class NotasGenerales
    {
        public string Alumno { get; set; }
        public string Clase { get; set; }
        public decimal Punteo { get; set; }
        public decimal Promedio { get; set; }
        public string Aprobado { get; set; }
        public int CodAlumno { get; set; }
    }
}
