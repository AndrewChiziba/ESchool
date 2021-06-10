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
using System.Security.Claims;

namespace ESchool.Controllers
{
    public class MultipleChoiceExercisesController : Controller
    {
        private readonly ESchoolDbContext _context;
        string curr_userId;

        public MultipleChoiceExercisesController(ESchoolDbContext context)
        {
            _context = context;
        }

        // GET: MultipleChoiceExercises
        public async Task<IActionResult> Index()
        {
            curr_userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var curr_teacher = _context.Teachers.First(id => id.UserId == curr_userId);
            //return View(await _context.Exercises.Where(id=>id.TeacherId==curr_teacher.Id).ToListAsync());
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
            curr_userId = User.FindFirstValue(ClaimTypes.NameIdentifier);// user's
            var curr_teacher = _context.Teachers.First(id => id.UserId == curr_userId);
            //timeTable.TeacherId = curr_teacher.Id;
            MultipleChoiceExercise mcexercise = new MultipleChoiceExercise
            {
                TeacherId = curr_teacher.Id,
                CourseId = curr_teacher.CourseId
            };

            return View(mcexercise);
        }

        // POST: MultipleChoiceExercises/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Topic,TeacherId,CourseId")] MultipleChoiceExercise multipleChoiceExercise)
        {
            if (ModelState.IsValid)
            {
                _context.Add(multipleChoiceExercise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(multipleChoiceExercise);
        }

       

        //GET
        public async Task<IActionResult> MultiChoiceExercise(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mcexercise = await _context.MultipleChoiceExercises.FindAsync(id);
            List<MultipleChoiceQuestion> MCQuestionList = _context.MultipleChoiceQuestions.Where(mcq => mcq.ExerciseId == mcexercise.Id).ToList();

            MultipleChoiceViewModel mcexerciseVM = new MultipleChoiceViewModel
            {
                MultipleChoiceQuestions = MCQuestionList,
                Topic = mcexercise.Topic,
                ExerciseId = mcexercise.Id
            };

            if (MCQuestionList == null)
            {
                return NotFound();
            }
            return View(mcexerciseVM);
            // return View(await _context.MultipleChoiceQuestions.ToListAsync());
        }


        [HttpPost]
        public async Task<IActionResult> MultiChoiceExercise(int ExerciseId, List<string> answers)
        {
            int TotalScore = 0;
            int StudentScore = 0;

            curr_userId = User.FindFirstValue(ClaimTypes.NameIdentifier);// user's
            var curr_student = _context.Students.First(id => id.UserId == curr_userId);

            var mcexercise = _context.MultipleChoiceExercises.FirstOrDefault(t => t.Id == ExerciseId);
            TotalScore = mcexercise.TotalScore;

            List<MultipleChoiceQuestion> mcquestionList = _context.MultipleChoiceQuestions.Where(id => id.ExerciseId == mcexercise.Id).ToList();
            if (mcquestionList != null)
            {
                //result calculation
                for (int i = 0; i < mcquestionList.Count; i++)
                {
                    if (answers[i] == mcquestionList[i].Answer)
                    {
                        StudentScore += mcquestionList[i].Score;//add score
                    }

                }

                //get prev results
                bool didExercise = false;
                var prev_results = _context.Results.Where(id => id.StudentId == curr_student.Id).ToList();

                foreach (var result in prev_results)
                {
                    if (result.ExerciseId == ExerciseId)
                    {
                        didExercise = true;//tell user "already did exercise"
                    }

                }

                if (didExercise == false)
                {
                    //create result record
                    var newResult = new Result 
                    { 
                        ExerciseId = mcexercise.Id, 
                        Topic = mcexercise.Topic, 
                        StudentId = curr_student.Id,
                        StudentName = curr_student.Surname + " " + curr_student.Name + " " + curr_student.MiddleName,
                        TotalScore = TotalScore, 
                        StudentScore = StudentScore 
                    };

                    await CreateResult(newResult);

                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("Index");
                //return ("Ваш балл " + StudentScore + "/" + TotalScore);
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

        //
        public async Task<IActionResult> QuestionsOfExercise(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mcexercise = await _context.MultipleChoiceExercises.FindAsync(id);
            List<MultipleChoiceQuestion> mcquestionList = _context.MultipleChoiceQuestions.Where(mcq => mcq.ExerciseId == mcexercise.Id).ToList();

            if (mcquestionList == null)
            {
                return NotFound();
            }
            return View(mcquestionList);
            // return View(await _context.MultipleChoiceQuestions.ToListAsync());
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
