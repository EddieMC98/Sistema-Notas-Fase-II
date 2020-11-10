using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaNotas1.Models.Usuarios
{
    public class NotasXAlumno
    {
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Codigo { get; set; }
        public int Cod_Estudiante { get; set; }
        public int CodClase { get; set; }
        public int CodActividad { get; set; }
        public decimal Total { get; set; }
        public ICollection<ActividadxClase> punteoxClases { get; set; }
        public List<ActividadxClase> actividad_punteo { get; set; }
        public int contador { get; set; }
        public string Estado { get; set; }

    }
}
