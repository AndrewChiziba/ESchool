using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ESchool.AppDbContext;
using ESchool.Models.CourseTimeTable;

namespace ESchool.Controllers
{
    public class TTEntriesController : Controller
    {
        private readonly ESchoolDbContext _context;

        public TTEntriesController(ESchoolDbContext context)
        {
            _context = context;
        }

        // GET: TTEntries
        public async Task<IActionResult> Index()
        {
            return View(await _context.TTEntries.ToListAsync());
        }

        // GET: TTEntries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tTEntry = await _context.TTEntries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tTEntry == null)
            {
                return NotFound();
            }

            return View(tTEntry);
        }

        // GET: TTEntries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TTEntries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Activity,StartDate,EndDate,TimeTableId")] TTEntry tTEntry)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tTEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tTEntry);
        }

        // GET: TTEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tTEntry = await _context.TTEntries.FindAsync(id);
            if (tTEntry == null)
            {
                return NotFound();
            }
            return View(tTEntry);
        }

        // POST: TTEntries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Activity,StartDate,EndDate,TimeTableId")] TTEntry tTEntry)
        {
            if (id != tTEntry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tTEntry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TTEntryExists(tTEntry.Id))
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
            return View(tTEntry);
        }

        // GET: TTEntries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tTEntry = await _context.TTEntries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tTEntry == null)
            {
                return NotFound();
            }

            return View(tTEntry);
        }

        // POST: TTEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tTEntry = await _context.TTEntries.FindAsync(id);
            _context.TTEntries.Remove(tTEntry);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TTEntryExists(int id)
        {
            return _context.TTEntries.Any(e => e.Id == id);
        }
    }
}
