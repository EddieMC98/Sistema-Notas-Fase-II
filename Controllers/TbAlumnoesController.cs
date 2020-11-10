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
    public class TbAlumnoesController : Controller
    {
        private readonly sistema_notasContext _context;

        public TbAlumnoesController(sistema_notasContext context)
        {
            _context = context;
        }

        // GET: TbAlumnoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.TbAlumno.ToListAsync());
        }

        // GET: TbAlumnoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbAlumno = await _context.TbAlumno
                .FirstOrDefaultAsync(m => m.CodAlumno == id);
            if (tbAlumno == null)
            {
                return NotFound();
            }

            return View(tbAlumno);
        }

        // GET: TbAlumnoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TbAlumnoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodAlumno,Codigo,CodInformacionPersonal,CodUsuario,CodGrado")] TbAlumno tbAlumno)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tbAlumno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tbAlumno);
        }

        // GET: TbAlumnoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbAlumno = await _context.TbAlumno.FindAsync(id);
            if (tbAlumno == null)
            {
                return NotFound();
            }
            return View(tbAlumno);
        }

        // POST: TbAlumnoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodAlumno,Codigo,CodInformacionPersonal,CodUsuario,CodGrado")] TbAlumno tbAlumno)
        {
            if (id != tbAlumno.CodAlumno)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbAlumno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbAlumnoExists(tbAlumno.CodAlumno))
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
            return View(tbAlumno);
        }

        // GET: TbAlumnoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbAlumno = await _context.TbAlumno
                .FirstOrDefaultAsync(m => m.CodAlumno == id);
            if (tbAlumno == null)
            {
                return NotFound();
            }

            return View(tbAlumno);
        }

        // POST: TbAlumnoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbAlumno = await _context.TbAlumno.FindAsync(id);
            _context.TbAlumno.Remove(tbAlumno);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbAlumnoExists(int id)
        {
            return _context.TbAlumno.Any(e => e.CodAlumno == id);
        }
    }
}
