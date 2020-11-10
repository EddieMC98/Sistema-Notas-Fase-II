using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaNotas1.Models;
using SistemaNotas1.Models.Usuarios;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace SistemaNotas1.Controllers
{
    public class TbCursoesController : Controller
    {
        private readonly sistema_notasContext _context;
        public List<VerCursos> listacursos = new List<VerCursos>();
        public AgregarCurso objItemViewModel = new AgregarCurso();

        public TbCursoesController(sistema_notasContext context)
        {
            _context = context;
        }

        // GET: TbCursoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.TbCurso.ToListAsync());
        }

        // GET: TbCursoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbCurso = await _context.TbCurso
                .FirstOrDefaultAsync(m => m.CodCurso == id);
            if (tbCurso == null)
            {
                return NotFound();
            }

            return View(tbCurso);
        }

        // GET: TbCursoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TbCursoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodCurso,Nombre,Descripcion,CodGrado")] TbCurso tbCurso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tbCurso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tbCurso);
        }

        // GET: TbCursoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbCurso = await _context.TbCurso.FindAsync(id);
            if (tbCurso == null)
            {
                return NotFound();
            }
            return View(tbCurso);
        }

        // POST: TbCursoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodCurso,Nombre,Descripcion,CodGrado")] TbCurso tbCurso)
        {
            if (id != tbCurso.CodCurso)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbCurso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbCursoExists(tbCurso.CodCurso))
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
            return View(tbCurso);
        }

        // GET: TbCursoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbCurso = await _context.TbCurso
                .FirstOrDefaultAsync(m => m.CodCurso == id);
            if (tbCurso == null)
            {
                return NotFound();
            }

            return View(tbCurso);
        }

        // POST: TbCursoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbCurso = await _context.TbCurso.FindAsync(id);
            _context.TbCurso.Remove(tbCurso);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbCursoExists(int id)
        {
            return _context.TbCurso.Any(e => e.CodCurso == id);
        }

        public void llenarListaDeCursos()
        {
            objItemViewModel.ListaGrados = (from objCat in _context.TbGrado
                                            join objCat1 in _context.TbNivel on objCat.CodNivel equals objCat1.CodNivel
                                            select new SelectListItem()
                                            {
                                                Text = String.Concat(objCat.Nombre, " ", objCat1.Nombre),
                                                Value = objCat.CodGrado.ToString(),
                                                Selected = true
                                            });
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult AgregarCursos()
        {
            ViewBag.showSuccessAlert = false;

            objItemViewModel.ListaGrados = (from objCat in _context.TbGrado
                                            join objCat1 in _context.TbNivel on objCat.CodNivel equals objCat1.CodNivel
                                            select new SelectListItem()
                                            {
                                                Text = String.Concat(objCat.Nombre, " ", objCat1.Nombre),
                                                Value = objCat.CodGrado.ToString(),
                                                Selected = true
                                            });

            return View(objItemViewModel);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<ActionResult> AgregarCursos(AgregarCurso useri)
        {


            var consulta = from curso in _context.TbCurso
                           where curso.CodGrado.Equals(useri.CodGrado) && useri.Nombre.Contains(curso.Nombre)
                           select new
                           {
                               curso.Nombre
                           };

            if (useri.Descripcion.Length > 250)
            {
                ViewBag.showSuccessAlert = true;
                ModelState.AddModelError("Descripcion", "La longitud de la Descripción debe ser menor a 250 caracteres.");
                llenarListaDeCursos();
                return View(objItemViewModel);
            }
            else
            {

                if (consulta.Count() > 0)
                {
                    ViewBag.showSuccessAlert = true;
                    foreach (var item in consulta)
                    {
                        System.Diagnostics.Debug.WriteLine("Curso repetido:" + item.Nombre);
                    }
                    llenarListaDeCursos();
                    return View(objItemViewModel);
                }
                else
                {

                    int codigoal = useri.CodGrado ?? default(int);
                    TbCurso tbCurso = new TbCurso();
                    tbCurso.Nombre = useri.Nombre;
                    tbCurso.Descripcion = useri.Descripcion;
                    tbCurso.CodGrado = codigoal;
                    _context.TbCurso.Add(tbCurso);
                    _context.SaveChanges();

                    return RedirectToAction("AgregarCursos", "TbCursoes");
                }
            }
        }

        public ActionResult VerCursos()
        {
            var consulta = from con1 in _context.TbCurso
                           join con2 in _context.TbGrado on con1.CodGrado equals con2.CodGrado
                           join con3 in _context.TbNivel on con2.CodNivel equals con3.CodNivel
                           select new { 
                           Curso = con1.Nombre,
                           Grado = con2.Nombre,
                           Nivel = con3.Nombre,
                           CodCurso = con1.CodCurso
                           };

            foreach (var item in consulta) {
                VerCursos lista2 = new VerCursos();
                lista2.curso = item.Curso;
                lista2.grado = item.Grado;
                lista2.CodCurso = item.CodCurso;
                lista2.nivel = item.Nivel;
                listacursos.Add(lista2);

            }

            return View(listacursos);
        }

    }
}
