using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaNotas1.Models;

namespace SistemaNotas1.Controllers
{
    public class TbInformacionPersonalsController : Controller
    {
        private readonly sistema_notasContext _context;

        public TbInformacionPersonalsController(sistema_notasContext context)
        {
            _context = context;
        }

        // GET: TbInformacionPersonals
        public async Task<IActionResult> Index()
        {
            return View(await _context.TbInformacionPersonal.ToListAsync());
        }

        // GET: TbInformacionPersonals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbInformacionPersonal = await _context.TbInformacionPersonal
                .FirstOrDefaultAsync(m => m.CodInformacionPersonal == id);
            if (tbInformacionPersonal == null)
            {
                return NotFound();
            }

            return View(tbInformacionPersonal);
        }

        // GET: TbInformacionPersonals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TbInformacionPersonals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodInformacionPersonal,Nombre,Apellido,Telefono,CorreoElectronico,Direccion,Imagen,Cui")] TbInformacionPersonal tbInformacionPersonal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tbInformacionPersonal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tbInformacionPersonal);
        }

        // GET: TbInformacionPersonals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbInformacionPersonal = await _context.TbInformacionPersonal.FindAsync(id);
            if (tbInformacionPersonal == null)
            {
                return NotFound();
            }
            return View(tbInformacionPersonal);
        }

        // POST: TbInformacionPersonals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodInformacionPersonal,Nombre,Apellido,Telefono,CorreoElectronico,Direccion,Imagen,Cui")] TbInformacionPersonal tbInformacionPersonal)
        {
            if (id != tbInformacionPersonal.CodInformacionPersonal)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbInformacionPersonal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbInformacionPersonalExists(tbInformacionPersonal.CodInformacionPersonal))
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
            return View(tbInformacionPersonal);
        }

        // GET: TbInformacionPersonals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbInformacionPersonal = await _context.TbInformacionPersonal
                .FirstOrDefaultAsync(m => m.CodInformacionPersonal == id);
            if (tbInformacionPersonal == null)
            {
                return NotFound();
            }

            return View(tbInformacionPersonal);
        }

        // POST: TbInformacionPersonals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbInformacionPersonal = await _context.TbInformacionPersonal.FindAsync(id);
            _context.TbInformacionPersonal.Remove(tbInformacionPersonal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbInformacionPersonalExists(int id)
        {
            return _context.TbInformacionPersonal.Any(e => e.CodInformacionPersonal == id);
        }
    }
}
