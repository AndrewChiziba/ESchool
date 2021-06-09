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
        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var mcexercise = _context.MultipleChoiceExercises.Find(id);
            if (mcexercise == null)
            {
                return NotFound();
            }
            MultipleChoiceQuestion mcquestion = new MultipleChoiceQuestion
            {
                ExerciseId = mcexercise.Id
            };
            ViewBag.returnUrl = Request.Headers["Referer"].ToString();//get url of prev page
            return View(mcquestion);
        }

        // POST: MultipleChoiceQuestions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string returnUrl, [Bind("ExerciseId,Questiion,choiceA,choiceB,choiceC,choiceD,Answer,Score")] MultipleChoiceQuestion multipleChoiceQuestion)
        {
            if (ModelState.IsValid)
            {
                var num_questions = _context.MultipleChoiceQuestions.Where(qid => qid.ExerciseId == multipleChoiceQuestion.ExerciseId).Count();

                multipleChoiceQuestion.QuestionNumber = num_questions + 1;

                _context.Add(multipleChoiceQuestion);
                await _context.SaveChangesAsync();

                var mcexercise = _context.MultipleChoiceExercises.FirstOrDefault(e => e.Id == multipleChoiceQuestion.ExerciseId);
                var totalscore = _context.MultipleChoiceQuestions.Where(q => q.ExerciseId == multipleChoiceQuestion.ExerciseId).Sum(r => r.Score);

                //Update exercise totalscore/num_questions
                mcexercise.NumberOfQuestions = multipleChoiceQuestion.QuestionNumber;
                mcexercise.TotalScore = totalscore;

                _context.Update(mcexercise);
                await _context.SaveChangesAsync();

                return Redirect(returnUrl);//return to prev page
                //return RedirectToAction(nameof(Index));
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
            ViewBag.returnUrl = Request.Headers["Referer"].ToString();//url of prev page
            return View(multipleChoiceQuestion);
        }

        // POST: MultipleChoiceQuestions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string returnUrl, [Bind("Id,ExerciseId,QuestionNumber,Questiion,choiceA,choiceB,choiceC,choiceD,Answer,Score")] MultipleChoiceQuestion multipleChoiceQuestion)
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
                    //Update exercise totalscore
                    var totalscore = _context.MultipleChoiceQuestions.Where(mcq => mcq.ExerciseId == multipleChoiceQuestion.ExerciseId).Sum(r => r.Score);

                    var mcExercise = _context.MultipleChoiceExercises.FirstOrDefault(mce => mce.Id == multipleChoiceQuestion.ExerciseId);
                    mcExercise.TotalScore = totalscore;

                    _context.Update(mcExercise);
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
                return Redirect(returnUrl);
                //return RedirectToAction(nameof(Index));
            }
            return View(multipleChoiceQuestion);
        }

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
