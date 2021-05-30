using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ESchool.AppDbContext;
using ESchool.Models.MultipleChoice;

namespace ESchool.Controllers
{
    public class MultipleChoiceQuestionsController : Controller
    {
        private readonly ESchoolDbContext _context;

        public MultipleChoiceQuestionsController(ESchoolDbContext context)
        {
            _context = context;
        }

        // GET: MultipleChoiceQuestions
        public async Task<IActionResult> Index()
        {
            return View(await _context.MultipleChoiceQuestions.ToListAsync());
        }

        // GET: MultipleChoiceQuestions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var multipleChoiceQuestion = await _context.MultipleChoiceQuestions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (multipleChoiceQuestion == null)
            {
                return NotFound();
            }

            return View(multipleChoiceQuestion);
        }

        // GET: MultipleChoiceQuestions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MultipleChoiceQuestions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Topic,QuestionNumber,Questiion,choiceA,choiceB,choiceC,choiceD,Answer,Score")] MultipleChoiceQuestion multipleChoiceQuestion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(multipleChoiceQuestion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(multipleChoiceQuestion);
        }

        // GET: MultipleChoiceQuestions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var multipleChoiceQuestion = await _context.MultipleChoiceQuestions.FindAsync(id);
            if (multipleChoiceQuestion == null)
            {
                return NotFound();
            }
            return View(multipleChoiceQuestion);
        }

        // POST: MultipleChoiceQuestions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Topic,QuestionNumber,Questiion,choiceA,choiceB,choiceC,choiceD,Answer,Score")] MultipleChoiceQuestion multipleChoiceQuestion)
        {
            if (id != multipleChoiceQuestion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(multipleChoiceQuestion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MultipleChoiceQuestionExists(multipleChoiceQuestion.Id))
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
            return View(multipleChoiceQuestion);
        }
        
        /*Custom actions*/
        //GET
        public async Task<IActionResult> MCExercise()
        {
            return View(await _context.MultipleChoiceQuestions.ToListAsync());
        }
        /*Custom actions end*/


        // GET: MultipleChoiceQuestions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var multipleChoiceQuestion = await _context.MultipleChoiceQuestions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (multipleChoiceQuestion == null)
            {
                return NotFound();
            }

            return View(multipleChoiceQuestion);
        }

        // POST: MultipleChoiceQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var multipleChoiceQuestion = await _context.MultipleChoiceQuestions.FindAsync(id);
            _context.MultipleChoiceQuestions.Remove(multipleChoiceQuestion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MultipleChoiceQuestionExists(int id)
        {
            return _context.MultipleChoiceQuestions.Any(e => e.Id == id);
        }
    }
}
