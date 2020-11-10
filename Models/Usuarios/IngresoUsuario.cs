using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaNotas1.Models.Usuarios
{
    public class IngresoUsuario
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

        public IEnumerable<SelectListItem> ListaGrados { get; set; }

    }
}
