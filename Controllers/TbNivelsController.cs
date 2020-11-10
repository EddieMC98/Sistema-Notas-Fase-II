using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaNotas1.Models;

namespace SistemaNotas1.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class TbNivelsController : Controller
    {
        private readonly sistema_notasContext _context;

        public TbNivelsController(sistema_notasContext context)
        {
            _context = context;
        }

        // GET: TbNivels
        public async Task<IActionResult> Index()
        {
            return View(await _context.TbNivel.ToListAsync());
        }

        // GET: TbNivels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbNivel = await _context.TbNivel
                .FirstOrDefaultAsync(m => m.CodNivel == id);
            if (tbNivel == null)
            {
                return NotFound();
            }

            return View(tbNivel);
        }

        // GET: TbNivels/Create
        public IActionResult Create()
        {
            ViewBag.showSuccessAlert = false;
            return View();
        }

        // POST: TbNivels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodNivel,Nombre")] TbNivel tbNivel)
        {
            if (ModelState.IsValid)
            {

                var consulta = from nivels in _context.TbNivel
                               where tbNivel.Nombre.Contains(nivels.Nombre)
                               select new
                               {
                                   nivels.Nombre
                               };

                if (consulta.Count() > 0)
                {
                    ViewBag.showSuccessAlert = true;
                    foreach (var item in consulta)
                    {
                        System.Diagnostics.Debug.WriteLine("Nivel repetido:" + item.Nombre);
                    }
                    return View();
                }
                else
                {
                    _context.Add(tbNivel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }


            }
            return View(tbNivel);
        }

        // GET: TbNivels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbNivel = await _context.TbNivel.FindAsync(id);
            if (tbNivel == null)
            {
                return NotFound();
            }
            return View(tbNivel);
        }

        // POST: TbNivels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodNivel,Nombre")] TbNivel tbNivel)
        {
            if (id != tbNivel.CodNivel)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbNivel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbNivelExists(tbNivel.CodNivel))
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
            return View(tbNivel);
        }

        // GET: TbNivels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbNivel = await _context.TbNivel
                .FirstOrDefaultAsync(m => m.CodNivel == id);
            if (tbNivel == null)
            {
                return NotFound();
            }

            return View(tbNivel);
        }

        // POST: TbNivels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbNivel = await _context.TbNivel.FindAsync(id);
            _context.TbNivel.Remove(tbNivel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbNivelExists(int id)
        {
            return _context.TbNivel.Any(e => e.CodNivel == id);
        }
    }
}
