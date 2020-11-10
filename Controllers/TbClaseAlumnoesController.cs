using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaNotas1.Models;
using SistemaNotas1.Models.Usuarios;

namespace SistemaNotas1.Controllers
{
    public class TbClaseAlumnoesController : Controller
    {
        private readonly sistema_notasContext _context;
        Asignaciones objItemViewModel = new Asignaciones();
        public TbClaseAlumnoesController(sistema_notasContext context)
        {
            _context = context;
        }

        // GET: TbClaseAlumnoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.TbClaseAlumno.ToListAsync());
        }

        // GET: TbClaseAlumnoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbClaseAlumno = await _context.TbClaseAlumno
                .FirstOrDefaultAsync(m => m.CodClaseAlumno == id);
            if (tbClaseAlumno == null)
            {
                return NotFound();
            }

            return View(tbClaseAlumno);
        }

        // GET: TbClaseAlumnoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TbClaseAlumnoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodClaseAlumno,Aprobado,CodAlumno,CodClase")] TbClaseAlumno tbClaseAlumno)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tbClaseAlumno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tbClaseAlumno);
        }

        // GET: TbClaseAlumnoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbClaseAlumno = await _context.TbClaseAlumno.FindAsync(id);
            if (tbClaseAlumno == null)
            {
                return NotFound();
            }
            return View(tbClaseAlumno);
        }

        // POST: TbClaseAlumnoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodClaseAlumno,Aprobado,CodAlumno,CodClase")] TbClaseAlumno tbClaseAlumno)
        {
            if (id != tbClaseAlumno.CodClaseAlumno)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbClaseAlumno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbClaseAlumnoExists(tbClaseAlumno.CodClaseAlumno))
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
            return View(tbClaseAlumno);
        }

        // GET: TbClaseAlumnoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbClaseAlumno = await _context.TbClaseAlumno
                .FirstOrDefaultAsync(m => m.CodClaseAlumno == id);
            if (tbClaseAlumno == null)
            {
                return NotFound();
            }

            return View(tbClaseAlumno);
        }

        // POST: TbClaseAlumnoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbClaseAlumno = await _context.TbClaseAlumno.FindAsync(id);
            _context.TbClaseAlumno.Remove(tbClaseAlumno);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbClaseAlumnoExists(int id)
        {
            return _context.TbClaseAlumno.Any(e => e.CodClaseAlumno == id);
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult Asignacion()
        {
            ViewBag.showSuccessAlert = false;
            objItemViewModel.ListaClase = (from objCat in _context.TbClase
                                           join objCat1 in _context.TbCurso on objCat.CodCurso equals objCat1.CodCurso
                                           join objCat2 in _context.TbSeccion on objCat.CodSeccion equals objCat2.CodSeccion
                                           join objCat3 in _context.TbGrado on objCat1.CodGrado equals objCat3.CodGrado
                                           join objCat4 in _context.TbNivel on objCat3.CodNivel equals objCat4.CodNivel
                                           join objCat5 in _context.TbJornadas on objCat.CodJornada equals objCat5.CodJornada
                                           join objCat6 in _context.TbAño on objCat.CodAño equals objCat6.CodAño
                                         select new SelectListItem()
                                         {

                                             Text = String.Concat(objCat1.Nombre, " ", objCat2.Nombre, " ",objCat3.Nombre," ", objCat4.Nombre, " ", objCat5.Nombre," ", objCat6.Numero),
                                             Value = objCat.CodClase.ToString(),
                                             Selected = true
                                         });

            objItemViewModel.ListaAlumno = (from objCat in _context.TbAlumno
                                            join objCat1 in _context.TbInformacionPersonal on objCat.CodInformacionPersonal equals objCat1.CodInformacionPersonal
                                         select new SelectListItem()
                                         {

                                             Text = String.Concat(objCat1.Nombre, " ", objCat1.Apellido),
                                             Value = objCat.CodAlumno.ToString(),
                                             Selected = true
                                         });

            return View(objItemViewModel);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<ActionResult> Asignacion(Asignaciones useri)
        {
            int codigoal = useri.CodClase ?? default(int);
            int codigoal1 = useri.CodAlumno ?? default(int);


            var consulta = from pers in _context.TbClaseAlumno
                           where pers.CodAlumno.Equals(useri.CodAlumno) && pers.CodClase.Equals(useri.CodClase)
                           select new
                           {
                               pers.CodClase,
                               pers.CodAlumno
                           };
            if (consulta.Count() > 0)
            {
                ViewBag.showSuccessAlert = true;
                System.Diagnostics.Debug.WriteLine("ENTRAMOS");
                foreach (var item in consulta)
                {
                    System.Diagnostics.Debug.WriteLine("Registro Similar: " + item.CodClase);
                }
                LlenarListas();
                return View(objItemViewModel);
            }
            else {

                System.Diagnostics.Debug.WriteLine("el alumno puede asignarse ");
                TbClaseAlumno claseAlumno = new TbClaseAlumno();
                claseAlumno.CodClase = codigoal;
                claseAlumno.CodAlumno = codigoal1;
                _context.TbClaseAlumno.Add(claseAlumno);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
        }

        public void LlenarListas() {

            objItemViewModel.ListaClase = (from objCat in _context.TbClase
                                           join objCat1 in _context.TbCurso on objCat.CodCurso equals objCat1.CodCurso
                                           join objCat2 in _context.TbSeccion on objCat.CodSeccion equals objCat2.CodSeccion
                                           join objCat3 in _context.TbGrado on objCat1.CodGrado equals objCat3.CodGrado
                                           join objCat4 in _context.TbNivel on objCat3.CodNivel equals objCat4.CodNivel
                                           join objCat5 in _context.TbJornadas on objCat.CodJornada equals objCat5.CodJornada
                                           join objCat6 in _context.TbAño on objCat.CodAño equals objCat6.CodAño
                                           select new SelectListItem()
                                           {

                                               Text = String.Concat(objCat1.Nombre, " ", objCat2.Nombre, " ", objCat3.Nombre, " ", objCat4.Nombre, " ", objCat5.Nombre, " ", objCat6.Numero),
                                               Value = objCat.CodClase.ToString(),
                                               Selected = true
                                           });

            objItemViewModel.ListaAlumno = (from objCat in _context.TbAlumno
                                            join objCat1 in _context.TbInformacionPersonal on objCat.CodInformacionPersonal equals objCat1.CodInformacionPersonal
                                            select new SelectListItem()
                                            {

                                                Text = String.Concat(objCat1.Nombre, " ", objCat1.Apellido),
                                                Value = objCat.CodAlumno.ToString(),
                                                Selected = true
                                            });

        }

    }
}
