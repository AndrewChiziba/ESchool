using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ESchool.AppDbContext;
using ESchool.Models.MultipleChoice;
using ESchool.Models.ResultsRecord;

namespace ESchool.Controllers
{
    public class MultipleChoiceExercisesController : Controller
    {
        private readonly ESchoolDbContext _context;

        public MultipleChoiceExercisesController(ESchoolDbContext context)
        {
            _context = context;
        }

        // GET: MultipleChoiceExercises
        public async Task<IActionResult> Index()
        {
            return View(await _context.MultipleChoiceExercises.ToListAsync());
        }

        // GET: MultipleChoiceExercises/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var multipleChoiceExercise = await _context.MultipleChoiceExercises
                .FirstOrDefaultAsync(m => m.Id == id);
            if (multipleChoiceExercise == null)
            {
                return NotFound();
            }

            return View(multipleChoiceExercise);
        }

        // GET: MultipleChoiceExercises/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MultipleChoiceExercises/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOG([Bind("Id,Topic,NumberOfQuestions,TotalScore")] MultipleChoiceExercise multipleChoiceExercise)
        {
            if (ModelState.IsValid)
            {
                _context.Add(multipleChoiceExercise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(multipleChoiceExercise);
        }


        //Question
        public async Task<IActionResult> BlankMultipleChoiceQuestion(MultipleChoiceViewModel model, int qnumber)
        {
            var newMCQuestion = new MultipleChoiceQuestion { Topic = model.Topic, QuestionNumber = qnumber, Questiion = "", Answer = "", Score = 0, choiceA ="",choiceB="", choiceC="", choiceD="" };
            if (newMCQuestion != null)
            {
                _context.Add(newMCQuestion);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        // POST: MultipleChoiceExercises/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( MultipleChoiceViewModel model)
        {
            if (ModelState.IsValid)
            {

                for (int i = 0; i < model.NumberOfQuestions; i++)
                {
                    await BlankMultipleChoiceQuestion(model, i + 1);
                }

                var listMCQuestions = _context.MultipleChoiceQuestions.Where(MCquestion => MCquestion.Topic == model.Topic).ToList();

                var newMCExercise = new MultipleChoiceExercise { Topic = model.Topic, NumberOfQuestions = model.NumberOfQuestions, MultipleChoiceQuestions = listMCQuestions };

                _context.Add(newMCExercise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Exercises/Edit/5
        public async Task<IActionResult> EditMultipleChoiceQuestion(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var MCExercise = await _context.MultipleChoiceExercises.FindAsync(id);
            List<MultipleChoiceQuestion> MCQuestionList = _context.MultipleChoiceQuestions.Where(MCquestion => MCquestion.Topic == MCExercise.Topic).ToList();
           
            if (MCQuestionList == null)
            {
                return NotFound();
            }
            return View(MCQuestionList);
        }

        //GET
        public async Task<IActionResult> DoMultipleChoiceExercise(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var MCExercise = await _context.MultipleChoiceExercises.FindAsync(id);
            List<MultipleChoiceQuestion> MCQuestionList = _context.MultipleChoiceQuestions.Where(MCquestion => MCquestion.Topic == MCExercise.Topic).ToList();

            if (MCQuestionList == null)
            {
                return NotFound();
            }
            return View(MCQuestionList);
            // return View(await _context.MultipleChoiceQuestions.ToListAsync());
        }


        [HttpPost]

        public async Task<IActionResult> DoMultipleChoiceExercise(string topic, List<string> answers)
        {
            var st = topic;
            var ans = answers;

            int TotalScore = 0;
            int StudentScore = 0;

            var MCExercise = _context.MultipleChoiceExercises.FirstOrDefault(t => t.Topic == topic);
            TotalScore = MCExercise.TotalScore;

            List<MultipleChoiceQuestion> MCQuestionList = _context.MultipleChoiceQuestions.Where(topic => topic.Topic == MCExercise.Topic).ToList();
            if (MCQuestionList != null)
            {
                //result calculation
                for (int i = 0; i < MCQuestionList.Count; i++)
                {
                    if (answers[i] == MCQuestionList[i].Answer)
                    {
                        StudentScore += MCQuestionList[i].Score;//add score
                    }


                    //if (item.Answer == exercise.Questions.FirstOrDefault(qId => qId.Id == item.Id).Answer)
                    //{
                    //    StudentScore += 1;
                    //}
                    // _context.Entry(item).State = EntityState.Modified;
                    // _context.Entry(item).Property(x => x.Topic).IsModified = false;
                    // _context.Entry(item).Property(x => x.Questiion).IsModified = false;

                }

                //create result record
                var newResult = new Result { ExerciseId = MCExercise.Id, Topic = MCExercise.Topic, StudentId = 2, TotalScore = TotalScore, StudentScore = StudentScore };

                await CreateResult(newResult);

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateResult(Result result)
        {
            if (result != null)
            {
                _context.Add(result);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return View("Failed to create");
        }


        /*End of custom methods*/

        // GET: MultipleChoiceExercises/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var multipleChoiceExercise = await _context.MultipleChoiceExercises.FindAsync(id);
            if (multipleChoiceExercise == null)
            {
                return NotFound();
            }
            return View(multipleChoiceExercise);
        }

        // POST: MultipleChoiceExercises/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Topic,NumberOfQuestions,TotalScore")] MultipleChoiceExercise multipleChoiceExercise)
        {
            if (id != multipleChoiceExercise.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(multipleChoiceExercise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MultipleChoiceExerciseExists(multipleChoiceExercise.Id))
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
            return View(multipleChoiceExercise);
        }











        // GET: MultipleChoiceExercises/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var multipleChoiceExercise = await _context.MultipleChoiceExercises
                .FirstOrDefaultAsync(m => m.Id == id);
            if (multipleChoiceExercise == null)
            {
                return NotFound();
            }

            return View(multipleChoiceExercise);
        }

        // POST: MultipleChoiceExercises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var multipleChoiceExercise = await _context.MultipleChoiceExercises.FindAsync(id);
            _context.MultipleChoiceExercises.Remove(multipleChoiceExercise);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MultipleChoiceExerciseExists(int id)
        {
            return _context.MultipleChoiceExercises.Any(e => e.Id == id);
        }
    }
}
