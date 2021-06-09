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
    public class TranslateQuestionsController : Controller
    {
        private readonly ESchoolDbContext _context;

        public TranslateQuestionsController(ESchoolDbContext context)
        {
            _context = context;
        }

        // GET: TranslateQuestions
        public async Task<IActionResult> Index()
        {
            return View(await _context.TranslateQuestions.ToListAsync());
        }

        // GET: TranslateQuestions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var translateQuestion = await _context.TranslateQuestions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (translateQuestion == null)
            {
                return NotFound();
            }

            return View(translateQuestion);
        }

        // GET: TranslateQuestions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TranslateQuestions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ExerciseId,QuestionNumber,Questiion,Answer,Score")] TranslateQuestion translateQuestion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(translateQuestion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(translateQuestion);
        }

        // GET: TranslateQuestions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var translateQuestion = await _context.TranslateQuestions.FindAsync(id);
            if (translateQuestion == null)
            {
                return NotFound();
            }
            return View(translateQuestion);
        }

        // POST: TranslateQuestions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ExerciseId,QuestionNumber,Questiion,Answer,Score")] TranslateQuestion translateQuestion)
        {
            if (id != translateQuestion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(translateQuestion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TranslateQuestionExists(translateQuestion.Id))
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
            return View(translateQuestion);
        }

        // GET: TranslateQuestions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var translateQuestion = await _context.TranslateQuestions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (translateQuestion == null)
            {
                return NotFound();
            }

            return View(translateQuestion);
        }

        // POST: TranslateQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var translateQuestion = await _context.TranslateQuestions.FindAsync(id);
            _context.TranslateQuestions.Remove(translateQuestion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TranslateQuestionExists(int id)
        {
            return _context.TranslateQuestions.Any(e => e.Id == id);
        }
    }
}
