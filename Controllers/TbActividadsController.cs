using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaNotas1.Models;
using SistemaNotas1.Models.Usuarios;

namespace SistemaNotas1.Controllers
{
    public class TbActividadsController : Controller
    {
        private readonly sistema_notasContext _context;
        public List<CursoxDocente> cursoxDoc = new List<CursoxDocente>();
        public List<AgregarActividad> agregarActividads = new List<AgregarActividad>();
        public List<VerEstudiante> alumno_clase = new List<VerEstudiante>();
        public List<VerEstudiante> alumnos = new List<VerEstudiante>();
        public TbActividadsController(sistema_notasContext context)
        {
            _context = context;
        }

        // GET: TbActividads
        public async Task<IActionResult> Index()
        {
            return View(await _context.TbActividad.ToListAsync());
        }

        // GET: TbActividads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbActividad = await _context.TbActividad
                .FirstOrDefaultAsync(m => m.CodActividad == id);
            if (tbActividad == null)
            {
                return NotFound();
            }

            return View(tbActividad);
        }

        // GET: TbActividads/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TbActividads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodActividad,Nombre,Descripcion,Punteo,CodClase")] TbActividad tbActividad)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tbActividad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tbActividad);
        }

        // GET: TbActividads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbActividad = await _context.TbActividad.FindAsync(id);
            if (tbActividad == null)
            {
                return NotFound();
            }
            return View(tbActividad);
        }

        // POST: TbActividads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodActividad,Nombre,Descripcion,Punteo,CodClase")] TbActividad tbActividad)
        {
            if (id != tbActividad.CodActividad)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbActividad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbActividadExists(tbActividad.CodActividad))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tbActividad);
        }

        // GET: TbActividads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbActividad = await _context.TbActividad
                .FirstOrDefaultAsync(m => m.CodActividad == id);
            if (tbActividad == null)
            {
                return NotFound();
            }

            return View(tbActividad);
        }

        // POST: TbActividads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbActividad = await _context.TbActividad.FindAsync(id);
            _context.TbActividad.Remove(tbActividad);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbActividadExists(int id)
        {
            return _context.TbActividad.Any(e => e.CodActividad == id);
        }

        private bool TbNotaExists(int id)
        {
            return _context.TbNota.Any(e => e.CodNota == id);
        }

        [Authorize(Roles = "Docente, Administrador")]
        public ActionResult AgregarActividad()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            System.Diagnostics.Debug.WriteLine("USUARIO: " + userId);
            int CODIGODOC = 0;
            var doc = from Doc in _context.TbDocente
                      where Doc.CodUsuario == int.Parse(userId)
                      select new
                      {
                          Doc.CodDocente
                      };

            foreach (var item in doc)
            {
                CODIGODOC = item.CodDocente;
            }

            var consulta = from tb_clase in _context.TbClase
                           join tb_jornads in _context.TbJornadas on tb_clase.CodJornada equals tb_jornads.CodJornada
                           join tb_anio in _context.TbAño on tb_clase.CodAño equals tb_anio.CodAño
                           join ojbCat1 in _context.TbUnidad on tb_clase.CodUnidad equals ojbCat1.CodUnidad
                           join tb_seccion in _context.TbSeccion on tb_clase.CodSeccion equals tb_seccion.CodSeccion
                           join curso in _context.TbCurso on tb_clase.CodCurso equals curso.CodCurso
                           join grad in _context.TbGrado on curso.CodGrado equals grad.CodGrado
                           join nive in _context.TbNivel on grad.CodNivel equals nive.CodNivel
                           where tb_clase.CodDocente == CODIGODOC
                           select new
                           {
                               u1 = tb_jornads.Nombre,
                               tb_anio.Numero,
                               u2 = ojbCat1.Nombre,
                               u3 = tb_seccion.Nombre,
                               curso.Nombre,
                               nivel = nive.Nombre,
                               grado = grad.Nombre,
                               tb_clase.CodClase
                           };

            if (consulta.Count() > 0)
            {
                foreach (var item in consulta)
                {
                    CursoxDocente cursox = new CursoxDocente();
                    cursox.Año = item.Numero.ToString();
                    cursox.Unidad = item.u2;
                    cursox.Jornada = item.u1;
                    cursox.Seccion = item.u3;
                    cursox.Curso = item.Nombre;
                    cursox.CodClase = item.CodClase;
                    cursox.Grado = item.grado;
                    cursox.Nivel = item.nivel;
                    cursoxDoc.Add(cursox);
                    System.Diagnostics.Debug.WriteLine("VAMOS: " + item.CodClase);
                }
            }


            return View(cursoxDoc);
        }

        [Authorize(Roles = "Docente, Administrador")]
        [HttpPost]
        //GET:Clases
        //[Authorize(Roles = "Administrador")]
        public ActionResult AgregarActividades(string cod_clasea)
        {
            System.Diagnostics.Debug.WriteLine("CLASE SELECCIONADA: " + cod_clasea);
            //TempData.Add("Error", cod_clasea);
            SetSession24(cod_clasea);
            ViewData["nombre"] = cod_clasea;
            int cod_clase = Int32.Parse(cod_clasea);
            var consulta = from act1 in _context.TbActividad
                           where act1.CodClase == cod_clase
                           select new
                           {
                               act1.Nombre,
                               act1.Descripcion,
                               act1.CodActividad,
                               act1.Punteo,
                               act1.CodClase
                           };


            foreach (var item in consulta)
            {
                AgregarActividad agregarAct = new AgregarActividad();
                agregarAct.CodClase = cod_clase;
                agregarAct.Descripcion = item.Descripcion;
                agregarAct.Nombre = item.Nombre;
                agregarAct.Punteo = item.Punteo;
                agregarAct.CodActividad = item.CodActividad;
                agregarActividads.Add(agregarAct);
            }

            return View(agregarActividads);
        }

        [Authorize(Roles = "Docente")]
        public ActionResult AsignarNotas(int id)
        {
            LlenarListaAlumnos(id);
            ViewData["Lista_Alumnos"] = alumnos;
            System.Diagnostics.Debug.WriteLine("ACTIVIDAD SELECCIONADA: " + id);

            return View();

        }

        [Authorize(Roles = "Docente, Administrador")]
        [HttpPost]
        public async Task<IActionResult> AsignacionNotas(VerEstudiante useri)
        {
            bool punteo_verificado = false;
            int cod_actividad = useri.actividadxes.ElementAt(0).CodActividad;
            decimal punteo_act = 0;
            var punt = from act in _context.TbActividad
                       where act.CodActividad == cod_actividad
                       select new
                       {
                           act.Punteo
                       };

            foreach (var item in punt)
            {
                punteo_act = item.Punteo;
            }

            for (int i = 0; i < useri.actividadxes.Count; i++)
            {
                if (useri.actividadxes.ElementAt(i).Punteo > punteo_act)
                {
                    punteo_verificado = true;
                }
            }

            if (punteo_verificado)
            {
                System.Diagnostics.Debug.WriteLine("PUNTEO SOBREPASADO");
                return RedirectToAction(nameof(AgregarActividad));
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("PUNTEO BIEN");
                for (int i = 0; i < useri.actividadxes.Count; i++)
                {
                    var Nota = from AF1 in _context.TbNota
                               where useri.actividadxes.ElementAt(i).CodAlumno == AF1.CodAlumno && AF1.CodActividad == cod_actividad
                               select new
                               {
                                   AF1.Punteo,
                                   AF1.CodNota
                               };

                    if (Nota.Count() > 0)
                    {
                        int id = Nota.FirstOrDefault().CodNota;
                        var nota12 = from tbN in _context.TbNota
                                     where tbN.CodNota == id
                                     select new
                                     {
                                         tbN.CodNota,
                                         tbN.CodActividad,
                                         tbN.CodAlumno,
                                         tbN.Punteo
                                     };
                        var customer = _context.TbNota.Where(c => c.CodNota.Equals(id)).FirstOrDefault();
                        customer.Punteo = useri.actividadxes.ElementAt(i).Punteo;
                        _context.TbNota.Update(customer);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        TbNota nota = new TbNota();
                        nota.CodActividad = cod_actividad;
                        nota.CodAlumno = useri.actividadxes.ElementAt(i).CodAlumno;
                        nota.Punteo = useri.actividadxes.ElementAt(i).Punteo;
                        _context.TbNota.Add(nota);
                        _context.SaveChanges();
                    }

                }
                return View();
            }
        }
        [Authorize(Roles = "Docente, Administrador")]
        public ActionResult CrearActividad()
        {
            // ViewBag.showSuccessAlert1 = false;
            //ViewBag.Error = TempData["Error"];
            ViewData["nombre1"] = GetSession24();
            System.Diagnostics.Debug.WriteLine("Clase 1: " + GetSession24());
            return View();
        }
        [Authorize(Roles = "Docente, Administrador")]
        [HttpPost]
        public async Task<ActionResult> CrearActividad(AgregarActividad useri)
        {
            ViewData["nombre1"] = GetSession24();
            decimal total = 0;
            var consulta2 = from act2 in _context.TbActividad
                            where act2.CodClase == useri.CodClase
                            select new
                            {
                                act2.Punteo
                            };

            foreach (var item in consulta2)
            {
                total += item.Punteo;
            }
            total += useri.Punteo;
            if (total > 100)
            {
                System.Diagnostics.Debug.WriteLine("Se paso: " + total);

                ModelState.AddModelError("Punteo", "Se ha sobrepado el total de puntos");
                return View(useri);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Total: " + total);
                TbActividad actividad = new TbActividad();
                actividad.CodClase = useri.CodClase;
                actividad.Descripcion = useri.Descripcion;
                actividad.Nombre = useri.Nombre;
                actividad.Punteo = useri.Punteo;
                _context.TbActividad.Add(actividad);
                _context.SaveChanges();
                return RedirectToAction(nameof(AgregarActividad));
            }
            //System.Diagnostics.Debug.WriteLine("Entramos" + useri.CodClase);
            //TempData.Remove("Error");
        }

        public void SetSession24(string obj)
        {
            HttpContext.Session.SetString("direccion2", obj);
        }

        public string GetSession24()
        {
            return HttpContext.Session.GetString("direccion2");
        }

        public void LlenarListaAlumnos(int id)
        {
            int contador = 0;
            int cod_clase = 0;
            var consulta1 = from con in _context.TbActividad
                            where con.CodActividad == id
                            select new
                            {
                                con.CodClase
                            };

            foreach (var item in consulta1)
            {
                cod_clase = item.CodClase;
            }
            var consulta = from act1 in _context.TbClaseAlumno
                           join act2 in _context.TbAlumno on act1.CodAlumno equals act2.CodAlumno
                           join act3 in _context.TbInformacionPersonal on act2.CodInformacionPersonal equals act3.CodInformacionPersonal
                           join act4 in _context.TbNota on act1.CodAlumno equals act4.CodAlumno
                           where act1.CodClase == cod_clase && (act4.CodActividad == id)
                           select new
                           {
                               act1.CodAlumno,
                               act1.CodClase,
                               act2.CodInformacionPersonal,
                               act3.Nombre,
                               act3.Apellido,
                               act3.CorreoElectronico,
                               act3.Cui,
                               act3.Imagen,
                               act1.CodClaseAlumno,
                               Punteo = act4.Punteo
                           };
            int ContCons = consulta.Count();
            if (ContCons == 0)
            {
                var consulta11 = from act1 in _context.TbClaseAlumno
                                 join act2 in _context.TbAlumno on act1.CodAlumno equals act2.CodAlumno
                                 join act3 in _context.TbInformacionPersonal on act2.CodInformacionPersonal equals act3.CodInformacionPersonal
                                 where act1.CodClase == cod_clase
                                 select new
                                 {
                                     act1.CodAlumno,
                                     act1.CodClase,
                                     act2.CodInformacionPersonal,
                                     act3.Nombre,
                                     act3.Apellido,
                                     act3.CorreoElectronico,
                                     act3.Cui,
                                     act3.Imagen,
                                     act1.CodClaseAlumno
                                 };


                foreach (var item in consulta11)
                {

                    VerEstudiante verEstudiante = new VerEstudiante();
                    verEstudiante.Nombre = item.Nombre;
                    verEstudiante.Apellidos = item.Apellido;
                    verEstudiante.CodClase = item.CodClase;
                    verEstudiante.Correo = item.CorreoElectronico;
                    verEstudiante.Cui = item.Cui;
                    verEstudiante.Cod_Estudiante = item.CodAlumno;
                    verEstudiante.CodClase = item.CodClase;
                    verEstudiante.CodGrado = item.CodClaseAlumno;
                    verEstudiante.CodActividad = id;
                    verEstudiante.Contador = contador;
                    verEstudiante.PunteoActividad = 0;
                    contador++;
                    alumnos.Add(verEstudiante);
                }

            }
            else
            {

                foreach (var item in consulta)
                {

                    VerEstudiante verEstudiante = new VerEstudiante();
                    verEstudiante.Nombre = item.Nombre;
                    verEstudiante.Apellidos = item.Apellido;
                    verEstudiante.CodClase = item.CodClase;
                    verEstudiante.Correo = item.CorreoElectronico;
                    verEstudiante.Cui = item.Cui;
                    verEstudiante.Cod_Estudiante = item.CodAlumno;
                    verEstudiante.CodClase = item.CodClase;
                    verEstudiante.CodGrado = item.CodClaseAlumno;
                    verEstudiante.CodActividad = id;
                    verEstudiante.Contador = contador;
                    if (item.Punteo == null)
                    {
                        verEstudiante.PunteoActividad = 0;
                    }
                    else
                    {
                        verEstudiante.PunteoActividad = item.Punteo ?? default(decimal);
                    }
                    contador++;
                    alumnos.Add(verEstudiante);
                }
            }
        }


    }
}
