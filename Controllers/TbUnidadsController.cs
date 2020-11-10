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
    public class TbUnidadsController : Controller
    {
        private readonly sistema_notasContext _context;

        public TbUnidadsController(sistema_notasContext context)
        {
            _context = context;
        }

        // GET: TbUnidads
        public async Task<IActionResult> Index()
        {
            return View(await _context.TbUnidad.ToListAsync());
        }

        // GET: TbUnidads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbUnidad = await _context.TbUnidad
                .FirstOrDefaultAsync(m => m.CodUnidad == id);
            if (tbUnidad == null)
            {
                return NotFound();
            }

            return View(tbUnidad);
        }

        // GET: TbUnidads/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TbUnidads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodUnidad,Nombre")] TbUnidad tbUnidad)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tbUnidad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tbUnidad);
        }

        // GET: TbUnidads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbUnidad = await _context.TbUnidad.FindAsync(id);
            if (tbUnidad == null)
            {
                return NotFound();
            }
            return View(tbUnidad);
        }

        // POST: TbUnidads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodUnidad,Nombre")] TbUnidad tbUnidad)
        {
            if (id != tbUnidad.CodUnidad)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbUnidad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbUnidadExists(tbUnidad.CodUnidad))
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
            return View(tbUnidad);
        }

        // GET: TbUnidads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbUnidad = await _context.TbUnidad
                .FirstOrDefaultAsync(m => m.CodUnidad == id);
            if (tbUnidad == null)
            {
                return NotFound();
            }

            return View(tbUnidad);
        }

        // POST: TbUnidads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbUnidad = await _context.TbUnidad.FindAsync(id);
            _context.TbUnidad.Remove(tbUnidad);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbUnidadExists(int id)
        {
            return _context.TbUnidad.Any(e => e.CodUnidad == id);
        }
    }
}
