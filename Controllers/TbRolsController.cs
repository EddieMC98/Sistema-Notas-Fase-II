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
    public class TbRolsController : Controller
    {
        private readonly sistema_notasContext _context;

        public TbRolsController(sistema_notasContext context)
        {
            _context = context;
        }

        // GET: TbRols
        public async Task<IActionResult> Index()
        {
            return View(await _context.TbRol.ToListAsync());
        }

        // GET: TbRols/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbRol = await _context.TbRol
                .FirstOrDefaultAsync(m => m.CodRol == id);
            if (tbRol == null)
            {
                return NotFound();
            }

            return View(tbRol);
        }

        // GET: TbRols/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TbRols/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodRol,Rol")] TbRol tbRol)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tbRol);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tbRol);
        }

        // GET: TbRols/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbRol = await _context.TbRol.FindAsync(id);
            if (tbRol == null)
            {
                return NotFound();
            }
            return View(tbRol);
        }

        // POST: TbRols/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodRol,Rol")] TbRol tbRol)
        {
            if (id != tbRol.CodRol)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbRol);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbRolExists(tbRol.CodRol))
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
            return View(tbRol);
        }

        // GET: TbRols/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbRol = await _context.TbRol
                .FirstOrDefaultAsync(m => m.CodRol == id);
            if (tbRol == null)
            {
                return NotFound();
            }

            return View(tbRol);
        }

        // POST: TbRols/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbRol = await _context.TbRol.FindAsync(id);
            _context.TbRol.Remove(tbRol);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbRolExists(int id)
        {
            return _context.TbRol.Any(e => e.CodRol == id);
        }
    }
}
