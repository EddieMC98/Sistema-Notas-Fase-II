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
    public class TbAñoController : Controller
    {
        private readonly sistema_notasContext _context;

        public TbAñoController(sistema_notasContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Administrador")]
        // GET: TbAño
        public async Task<IActionResult> Index()
        {
            return View(await _context.TbAño.ToListAsync());
        }
        [Authorize(Roles = "Administrador")]
        // GET: TbAño/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbAño = await _context.TbAño
                .FirstOrDefaultAsync(m => m.CodAño == id);
            if (tbAño == null)
            {
                return NotFound();
            }

            return View(tbAño);
        }
        [Authorize(Roles = "Administrador")]
        // GET: TbAño/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TbAño/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodAño,Numero")] TbAño tbAño)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tbAño);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tbAño);
        }
        [Authorize(Roles = "Administrador")]
        // GET: TbAño/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbAño = await _context.TbAño.FindAsync(id);
            if (tbAño == null)
            {
                return NotFound();
            }
            return View(tbAño);
        }

        // POST: TbAño/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodAño,Numero")] TbAño tbAño)
        {
            if (id != tbAño.CodAño)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbAño);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbAñoExists(tbAño.CodAño))
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
            return View(tbAño);
        }

        [Authorize(Roles = "Administrador")]
        // GET: TbAño/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbAño = await _context.TbAño
                .FirstOrDefaultAsync(m => m.CodAño == id);
            if (tbAño == null)
            {
                return NotFound();
            }

            return View(tbAño);
        }

        [Authorize(Roles = "Administrador")]
        // POST: TbAño/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbAño = await _context.TbAño.FindAsync(id);
            _context.TbAño.Remove(tbAño);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbAñoExists(int id)
        {
            return _context.TbAño.Any(e => e.CodAño == id);
        }
    }
}
