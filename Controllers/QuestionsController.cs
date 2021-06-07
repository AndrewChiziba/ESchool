using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ESchool.AppDbContext;
using ESchool.Models.WordExercise;

namespace ESchool.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly ESchoolDbContext _context;

        public QuestionsController(ESchoolDbContext context)
        {
            _context = context;
        }

        // GET: Questions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Questions.ToListAsync());
        }

        // GET: Questions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // GET: Questions/Create
        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var exercise = _context.Exercises.Find(id);
            if (exercise == null)
            {
                return NotFound();
            }
            Question question = new Question
            {
                ExerciseId = exercise.Id
            };
            ViewBag.returnUrl = Request.Headers["Referer"].ToString();//get url of prev page
            return View(question);
        }

        // POST: Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string returnUrl, [Bind("ExerciseId,Questiion,Answer,Score")] Question question)
        {
            if (ModelState.IsValid)
            {
                var num_questions = _context.Questions.Where(qid => qid.ExerciseId == question.ExerciseId).Count();

                question.QuestionNumber = num_questions + 1;

                _context.Add(question);
                await _context.SaveChangesAsync();

                var exercise = _context.Exercises.FirstOrDefault(e => e.Id == question.ExerciseId);
                var totalscore = _context.Questions.Where(q => q.ExerciseId == question.ExerciseId).Sum(r => r.Score);
               
                //Update exercise totalscore/num_questions
                exercise.NumberOfQuestions = question.QuestionNumber;
                exercise.TotalScore = totalscore;

                _context.Update(exercise);
                await _context.SaveChangesAsync();

                return Redirect(returnUrl);//return to prev page
                //return RedirectToAction(nameof(Index));
            }
            return View(question);
        }

        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            ViewBag.returnUrl = Request.Headers["Referer"].ToString();//url of prev page
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string returnUrl, [Bind("Id,QuestionNumber,Questiion,Answer,Score")] Question question)
        {
            if (id != question.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                    //Update exercise totalscore
                    var totalscore = _context.Questions.Where(q => q.ExerciseId == question.ExerciseId).Sum(r => r.Score);

                    var exercise = _context.Exercises.FirstOrDefault(e => e.Id == question.ExerciseId);
                    exercise.TotalScore = totalscore;

                    _context.Update(exercise);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect(returnUrl);
                //return RedirectToAction(nameof(Index));
            }
            return View(question);
        }

        // GET: Questions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(int id)
        {
            return _context.Questions.Any(e => e.Id == id);
        }
    }
}
