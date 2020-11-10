using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaNotas1.Models.Usuarios
{
    public class VerActividadesxClase
    {
        public int CodActividad { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Punteo { get; set; }
        public int CodClase { get; set; }
        public decimal Total { get; set; }
        public string Clase { get; set; }
        public string Unidad { get; set; }
        public string Nivel { get; set; }
        public string Año { get; set; }
        public string Grado { get; set; }
        public string Jornada { get; set; }
        public string NombreDoc { get; set; }
        public string Seccion { get; set; }
        public string Alumno { get; set; }
        public List<ActividadxClase> actividad_punteo { get; set; }
    }
}
