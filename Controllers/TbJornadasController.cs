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
    public class TbJornadasController : Controller
    {
        private readonly sistema_notasContext _context;

        public TbJornadasController(sistema_notasContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Administrador")]
        // GET: TbJornadas
        public async Task<IActionResult> Index()
        {
            return View(await _context.TbJornadas.ToListAsync());
        }
        [Authorize(Roles = "Administrador")]
        // GET: TbJornadas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbJornadas = await _context.TbJornadas
                .FirstOrDefaultAsync(m => m.CodJornada == id);
            if (tbJornadas == null)
            {
                return NotFound();
            }

            return View(tbJornadas);
        }

        [Authorize(Roles = "Administrador")]
        // GET: TbJornadas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TbJornadas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodJornada,Nombre")] TbJornadas tbJornadas)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tbJornadas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tbJornadas);
        }

        [Authorize(Roles = "Administrador")]
        // GET: TbJornadas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbJornadas = await _context.TbJornadas.FindAsync(id);
            if (tbJornadas == null)
            {
                return NotFound();
            }
            return View(tbJornadas);
        }

        // POST: TbJornadas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodJornada,Nombre")] TbJornadas tbJornadas)
        {
            if (id != tbJornadas.CodJornada)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbJornadas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbJornadasExists(tbJornadas.CodJornada))
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
            return View(tbJornadas);
        }

        [Authorize(Roles = "Administrador")]
        // GET: TbJornadas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbJornadas = await _context.TbJornadas
                .FirstOrDefaultAsync(m => m.CodJornada == id);
            if (tbJornadas == null)
            {
                return NotFound();
            }

            return View(tbJornadas);
        }

        // POST: TbJornadas/Delete/5
        [Authorize(Roles = "Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbJornadas = await _context.TbJornadas.FindAsync(id);
            _context.TbJornadas.Remove(tbJornadas);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbJornadasExists(int id)
        {
            return _context.TbJornadas.Any(e => e.CodJornada == id);
        }
    }
}
