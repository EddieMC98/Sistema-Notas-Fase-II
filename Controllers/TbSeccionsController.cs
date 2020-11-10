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
    public class TbSeccionsController : Controller
    {
        private readonly sistema_notasContext _context;

        public TbSeccionsController(sistema_notasContext context)
        {
            _context = context;
        }

        // GET: TbSeccions
        public async Task<IActionResult> Index()
        {
            return View(await _context.TbSeccion.ToListAsync());
        }

        // GET: TbSeccions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbSeccion = await _context.TbSeccion
                .FirstOrDefaultAsync(m => m.CodSeccion == id);
            if (tbSeccion == null)
            {
                return NotFound();
            }

            return View(tbSeccion);
        }

        // GET: TbSeccions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TbSeccions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodSeccion,Nombre")] TbSeccion tbSeccion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tbSeccion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tbSeccion);
        }

        // GET: TbSeccions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbSeccion = await _context.TbSeccion.FindAsync(id);
            if (tbSeccion == null)
            {
                return NotFound();
            }
            return View(tbSeccion);
        }

        // POST: TbSeccions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodSeccion,Nombre")] TbSeccion tbSeccion)
        {
            if (id != tbSeccion.CodSeccion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbSeccion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbSeccionExists(tbSeccion.CodSeccion))
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
            return View(tbSeccion);
        }

        // GET: TbSeccions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbSeccion = await _context.TbSeccion
                .FirstOrDefaultAsync(m => m.CodSeccion == id);
            if (tbSeccion == null)
            {
                return NotFound();
            }

            return View(tbSeccion);
        }

        // POST: TbSeccions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbSeccion = await _context.TbSeccion.FindAsync(id);
            _context.TbSeccion.Remove(tbSeccion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbSeccionExists(int id)
        {
            return _context.TbSeccion.Any(e => e.CodSeccion == id);
        }
    }
}
