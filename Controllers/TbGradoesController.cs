using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using SistemaNotas1.Models;
using SistemaNotas1.Models.Usuarios;

namespace SistemaNotas1.Controllers
{
    public class TbGradoesController : Controller
    {
        private readonly sistema_notasContext _context;
        AgregarGrados objItemViewModel = new AgregarGrados();
        List<VerGrados> verGrados = new List<VerGrados>();
        public TbGradoesController(sistema_notasContext context)
        {
            _context = context;
        }

        // GET: TbGradoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.TbGrado.ToListAsync());
        }

        // GET: TbGradoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbGrado = await _context.TbGrado
                .FirstOrDefaultAsync(m => m.CodGrado == id);
            if (tbGrado == null)
            {
                return NotFound();
            }

            return View(tbGrado);
        }

        // GET: TbGradoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TbGradoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodGrado,CodGradoSiguiente,Nombre,SiguienteNivel,CodNivel")] TbGrado tbGrado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tbGrado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tbGrado);
        }

        // GET: TbGradoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbGrado = await _context.TbGrado.FindAsync(id);
            if (tbGrado == null)
            {
                return NotFound();
            }
            return View(tbGrado);
        }

        // POST: TbGradoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodGrado,CodGradoSiguiente,Nombre,SiguienteNivel,CodNivel")] TbGrado tbGrado)
        {
            if (id != tbGrado.CodGrado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbGrado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbGradoExists(tbGrado.CodGrado))
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
            return View(tbGrado);
        }

        // GET: TbGradoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbGrado = await _context.TbGrado
                .FirstOrDefaultAsync(m => m.CodGrado == id);
            if (tbGrado == null)
            {
                return NotFound();
            }

            return View(tbGrado);
        }

        // POST: TbGradoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbGrado = await _context.TbGrado.FindAsync(id);
            _context.TbGrado.Remove(tbGrado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbGradoExists(int id)
        {
            return _context.TbGrado.Any(e => e.CodGrado == id);
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult AgregarGrado()
        {
            ViewBag.showSuccessAlert = false;

            objItemViewModel.ListaGrados = (from ojbCat in _context.TbGrado
                                            join ojbCat1 in _context.TbNivel on ojbCat.CodNivel equals ojbCat1.CodNivel
                                            select new SelectListItem()
                                            {

                                                Text = String.Concat(ojbCat.Nombre, " ", ojbCat1.Nombre),
                                                Value = ojbCat.CodGrado.ToString(),
                                                Selected = true
                                            });

            objItemViewModel.ListaNiveles = (from ojbCat in _context.TbNivel
                                             select new SelectListItem()
                                             {

                                                 Text = String.Concat(ojbCat.Nombre),
                                                 Value = ojbCat.CodNivel.ToString(),
                                                 Selected = true
                                             });

            objItemViewModel.ListaSigNiveles = (from ojbCat in _context.TbNivel
                                                select new SelectListItem()
                                                {

                                                    Text = String.Concat(ojbCat.Nombre),
                                                    Value = ojbCat.CodNivel.ToString(),
                                                    Selected = true
                                                });


            return View(objItemViewModel);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<ActionResult> AgregarGrado(AgregarGrados useri)
        {
            int codigoal = useri.CodNivel ?? default(int);
            int codigoal1 = useri.CodGradoSiguiente ?? default(int);
            int codigoal2 = useri.SiguienteNivel ?? default(int);

            var consulta = from pers in _context.TbGrado
                           where (pers.Nombre.Equals(useri.Nombre) && pers.CodNivel.Equals(useri.CodNivel)) || pers.CodGradoSiguiente.Equals(useri.CodGradoSiguiente)
                           select new
                           {
                               pers.Nombre,
                               pers.CodNivel
                           };
            if (consulta.Count() > 0)
            {
                ViewBag.showSuccessAlert = true;
                System.Diagnostics.Debug.WriteLine("ENTRAMOS");
                foreach (var item in consulta)
                {
                    System.Diagnostics.Debug.WriteLine("Registro Similar: " + item.Nombre);
                }
                LlenarListasGrados();
                return View(objItemViewModel);
            }
            else
            {

                System.Diagnostics.Debug.WriteLine("el curso puede asignarse ");

                //TbGrado grados = new TbGrado();
                //grados.Nombre = useri.Nombre;
                //grados.CodGradoSiguiente = codigoal1;
                //grados.CodNivel = codigoal;
                //grados.SiguienteNivel = codigoal2;
                //_context.TbGrado.Add(grados);
                //_context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
        }

        public void LlenarListasGrados()
        {
            objItemViewModel.ListaGrados = (from ojbCat in _context.TbGrado
                                            join ojbCat1 in _context.TbNivel on ojbCat.CodNivel equals ojbCat1.CodNivel
                                            select new SelectListItem()
                                            {

                                                Text = String.Concat(ojbCat.Nombre, " ", ojbCat1.Nombre),
                                                Value = ojbCat.CodGrado.ToString(),
                                                Selected = true
                                            });

            objItemViewModel.ListaNiveles = (from ojbCat in _context.TbNivel
                                             select new SelectListItem()
                                             {

                                                 Text = String.Concat(ojbCat.Nombre),
                                                 Value = ojbCat.CodNivel.ToString(),
                                                 Selected = true
                                             });

            objItemViewModel.ListaSigNiveles = (from ojbCat in _context.TbNivel
                                                select new SelectListItem()
                                                {

                                                    Text = String.Concat(ojbCat.Nombre),
                                                    Value = ojbCat.CodNivel.ToString(),
                                                    Selected = true
                                                });
        }


        public ActionResult VerGrados()
        {
            var consulta = from var1 in _context.TbGrado
                           join cons1 in _context.TbNivel on var1.CodNivel equals cons1.CodNivel
                           join cons2 in _context.TbGrado on var1.CodGradoSiguiente equals cons2.CodGrado
                           join cons3 in _context.TbNivel on var1.SiguienteNivel equals cons3.CodNivel

                           select new
                           {
                               Nombre = var1.Nombre,
                               Nivel = cons1.Nombre,
                               SiguienteNombre = cons2.Nombre,
                               SiguienteNivel = cons3.Nombre,
                               CodGrado = var1.CodGrado
                           };

            foreach (var item in consulta)
            {
                VerGrados hola1 = new VerGrados();
                hola1.Nombre = item.Nombre;
                hola1.NombreNivel = item.Nivel;
                hola1.NombreSiguienteNivel = item.SiguienteNivel;
                hola1.GradoSiguiente = item.SiguienteNombre;
                hola1.CodGrado = item.CodGrado;
                verGrados.Add(hola1);
            }

            return View("VerGrados",verGrados);
            //return new ViewAsPdf("VerGrados",verGrados) 
            //{
            //    PageSize = Rotativa.AspNetCore.Options.Size.Legal,
            //    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            //};
        }

    }
}
