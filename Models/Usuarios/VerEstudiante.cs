using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaNotas1.Models.Usuarios
{
    public class VerEstudiante
    {
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public byte[] Imagen { get; set; }
        public string Correo { get; set; }
        public string Cui { get; set; }
        public int CodRol { get; set; }
        public int CodUsuario { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public string Codigo { get; set; }
        public int? CodGrado { get; set; }
        public int CodInfoPersonal { get; set; }
        public string Nivel { get; set; }
        public string Grado { get; set; }
        public int Cod_Estudiante { get; set; }
        public int CodClase { get; set; }
        public int CodActividad { get; set; }
        public IEnumerable<SelectListItem> ListaGrados { get; set; }
        public ICollection<EstudianteAsignado> verEstudiantes { get; set; }
        public ICollection<ActividadxAlumno> actividadxes { get; set; }
        public ICollection<ActividadxClase> punteo_actividad { get; set; }
        public List<ActividadxClase> actividad_punteo { get; set; }
        public string Clase { get; set; }
        public string NombreDoc { get; set; }

        public decimal PunteoActividad { get; set; }
        public int Contador { get; set; }
    }
}
