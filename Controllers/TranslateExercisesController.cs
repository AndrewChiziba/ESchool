using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ESchool.AppDbContext;
using ESchool.Models.TranslateExercise;

namespace ESchool.Controllers
{
    public class TranslateExercisesController : Controller
    {
        private readonly ESchoolDbContext _context;

        public TranslateExercisesController(ESchoolDbContext context)
        {
            _context = context;
        }

        // GET: TranslateExercises
        public async Task<IActionResult> Index()
        {
            return View(await _context.TranslateExercises.ToListAsync());
        }

        // GET: TranslateExercises/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var translateExercise = await _context.TranslateExercises
                .FirstOrDefaultAsync(m => m.Id == id);
            if (translateExercise == null)
            {
                return NotFound();
            }

            return View(translateExercise);
        }

        // GET: TranslateExercises/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TranslateExercises/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Topic,NumberOfQuestions,TotalScore,TeacherId,CourseId")] TranslateExercise translateExercise)
        {
            if (ModelState.IsValid)
            {
                _context.Add(translateExercise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(translateExercise);
        }

        // GET: TranslateExercises/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var translateExercise = await _context.TranslateExercises.FindAsync(id);
            if (translateExercise == null)
            {
                return NotFound();
            }
            return View(translateExercise);
        }

        // POST: TranslateExercises/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Topic,NumberOfQuestions,TotalScore,TeacherId,CourseId")] TranslateExercise translateExercise)
        {
            if (id != translateExercise.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(translateExercise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TranslateExerciseExists(translateExercise.Id))
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
            return View(translateExercise);
        }

        // GET: TranslateExercises/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var translateExercise = await _context.TranslateExercises
                .FirstOrDefaultAsync(m => m.Id == id);
            if (translateExercise == null)
            {
                return NotFound();
            }

            return View(translateExercise);
        }

        // POST: TranslateExercises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var translateExercise = await _context.TranslateExercises.FindAsync(id);
            _context.TranslateExercises.Remove(translateExercise);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TranslateExerciseExists(int id)
        {
            return _context.TranslateExercises.Any(e => e.Id == id);
        }
    }
}
