using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ESchool.AppDbContext;
using ESchool.Models.TranslateExercise;
using System.Security.Claims;
using ESchool.Models.ResultsRecord;

namespace ESchool.Controllers
{
    public class TranslateExercisesController : Controller
    {
        private readonly ESchoolDbContext _context;
        string curr_userId;

        public TranslateExercisesController(ESchoolDbContext context)
        {
            
            _context = context;
        }

        // GET: TranslateExercises
        public async Task<IActionResult> Index()
        {
            curr_userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var curr_teacher = _context.Teachers.First(id => id.UserId == curr_userId);
            //return View(await _context.Exercises.Where(id=>id.TeacherId==curr_teacher.Id).ToListAsync());
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
            curr_userId = User.FindFirstValue(ClaimTypes.NameIdentifier);// user's
            var curr_teacher = _context.Teachers.First(id => id.UserId == curr_userId);
           
            //timeTable.TeacherId = curr_teacher.Id;
            TranslateExercise texercise = new TranslateExercise
            {
                TeacherId = curr_teacher.Id,
                CourseId = curr_teacher.CourseId
            };

            return View(texercise);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Topic,TeacherId,CourseId")] TranslateExercise translateExercise)
        {
            if (ModelState.IsValid)
            {
                _context.Add(translateExercise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(translateExercise);
        }


        /*Custom Methods*/

        //GET
        public async Task<IActionResult> TransExercise(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var texercise = await _context.TranslateExercises.FindAsync(id);
            List<TranslateQuestion> TQuestionList = _context.TranslateQuestions.Where(q => q.ExerciseId == texercise.Id).ToList();
            TranslateVM translateVM = new TranslateVM
            {
                TranslateQuestions = TQuestionList,
                Topic = texercise.Topic,
                ExerciseId = texercise.Id
            };

            if (TQuestionList == null)
            {
                return NotFound();
            }
            return View(translateVM);
            // return View(await _context.MultipleChoiceQuestions.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> TransExercise(int ExerciseId, List<string> answers)
        {
            int TotalScore = 0;
            int StudentScore = 0;
            curr_userId = User.FindFirstValue(ClaimTypes.NameIdentifier);// user's
            var curr_student = _context.Students.First(id => id.UserId == curr_userId);

            var texercise = _context.TranslateExercises.FirstOrDefault(t => t.Id == ExerciseId);
            TotalScore = texercise.TotalScore;

            List<TranslateQuestion> tquestionList = _context.TranslateQuestions.Where(id => id.ExerciseId == texercise.Id).ToList();
            if (tquestionList != null)
            {
                //result calculation
                for (int i = 0; i < tquestionList.Count; i++)
                {
                    if (answers[i] == tquestionList[i].Answer)
                    {
                        StudentScore += tquestionList[i].Score;//add score
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
                    var newResult = new Result { ExerciseId = texercise.Id, Topic = texercise.Topic, StudentId = curr_student.Id, TotalScore = TotalScore, StudentScore = StudentScore };

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

            var texercise = await _context.TranslateExercises.FindAsync(id);
            List<TranslateQuestion> tquestionList = _context.TranslateQuestions.Where(mcq => mcq.ExerciseId == texercise.Id).ToList();

            if (tquestionList == null)
            {
                return NotFound();
            }
            return View(tquestionList);
            // return View(await _context.MultipleChoiceQuestions.ToListAsync());
        }

        /*Custom Methods end*/

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
