using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ESchool.AppDbContext;
using ESchool.Models.WordExercise;
using ESchool.Models.ResultsRecord;
using System.Security.Claims;

namespace ESchool.Controllers
{
    public class ExercisesController : Controller
    {
        private readonly ESchoolDbContext _context;
        string curr_userId;

        public ExercisesController(ESchoolDbContext context)
        {
            _context = context;
        }

        // GET: Exercises
        public async Task<IActionResult> Index()
        {
            curr_userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var curr_teacher = _context.Teachers.First(id => id.UserId == curr_userId);
            //return View(await _context.Exercises.Where(id=>id.TeacherId==curr_teacher.Id).ToListAsync());
            return View(await _context.Exercises.ToListAsync());
        }

        // GET: Exercises
        public async Task<IActionResult> TranslateIndex()
        {
            return View(await _context.Exercises.Where(tp=>tp.Topic=="Translate").ToListAsync());
        }


        // GET: Exercises/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }

        // GET: Exercises/Create
        public IActionResult Create()
        {
            curr_userId = User.FindFirstValue(ClaimTypes.NameIdentifier);// user's
            var curr_teacher = _context.Teachers.First(id => id.UserId == curr_userId);
            //timeTable.TeacherId = curr_teacher.Id;
            Exercise exercise = new Exercise
            {
                TeacherId = curr_teacher.Id,
                CourseId = curr_teacher.CourseId
            };
            
            return View(exercise);
        }

        // POST: Exercises/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Topic,TeacherId,CourseId")] Exercise exercise)
        {
            if (ModelState.IsValid)
            {
                _context.Add(exercise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(exercise);
        }

        /*Custom Methods*/

        //GET
        public async Task<IActionResult> WordExercise(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises.FindAsync(id);
            List<Question> QuestionList = _context.Questions.Where(q => q.ExerciseId == exercise.Id).ToList();
            ExerciseViewModel exerciseVM = new ExerciseViewModel
            {
                Questions = QuestionList,
                Topic = exercise.Topic,
                ExerciseId = exercise.Id
            };

            if (QuestionList == null)
            {
                return NotFound();
            }
            return View(exerciseVM);
            // return View(await _context.MultipleChoiceQuestions.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> WordExercise(int ExerciseId, List<string>answers)
        {
            int TotalScore = 0;
            int StudentScore = 0;
            curr_userId = User.FindFirstValue(ClaimTypes.NameIdentifier);// user's
            var curr_student = _context.Students.First(id => id.UserId == curr_userId);

            var exercise = _context.Exercises.FirstOrDefault(t => t.Id == ExerciseId);
            TotalScore = exercise.TotalScore;

            List<Question> questionList = _context.Questions.Where(id => id.ExerciseId == exercise.Id).ToList();
            if (questionList != null)
            {
                //result calculation
                for (int i = 0; i < questionList.Count; i++)
                {
                    if (answers[i] == questionList[i].Answer)
                    {
                        StudentScore += questionList[i].Score;//add score
                    }

                }

                //get prev results
                bool didExercise = false;
                var prev_results = _context.Results.Where(id=>id.StudentId==curr_student.Id).ToList();

                foreach(var result in prev_results)
                {
                    if (result.ExerciseId == ExerciseId)
                    {
                        didExercise = true;//tell user "already did exercise"
                    }

                }

                if (didExercise==false)
                {            
                    var newResult = new Result { ExerciseId = exercise.Id, Topic = exercise.Topic, StudentId = curr_student.Id, TotalScore = TotalScore, StudentScore = StudentScore };

                await CreateResult(newResult);

                await _context.SaveChangesAsync();

                }
                return RedirectToAction("Index");
                //return ("Ваш балл " + StudentScore + "/" + TotalScore);
            }
            return Ok();
        }

        //[HttpPost]
        //public async Task<JsonResult> Word(int ExerciseId, List<string> answers)
        //{
        //    int TotalScore = 0;
        //    int StudentScore = 0;
        //    curr_userId = User.FindFirstValue(ClaimTypes.NameIdentifier);// user's
        //    var curr_student = _context.Students.First(id => id.UserId == curr_userId);

        //    var exercise = _context.Exercises.FirstOrDefault(t => t.Id == ExerciseId);
        //    TotalScore = exercise.TotalScore;

        //    List<Question> questionList = _context.Questions.Where(id => id.ExerciseId == exercise.Id).ToList();
        //    if (questionList != null)
        //    {
        //        //result calculation
        //        for (int i = 0; i < questionList.Count; i++)
        //        {
        //            if (answers[i] == questionList[i].Answer)
        //            {
        //                StudentScore += questionList[i].Score;//add score
        //            }

        //        }

        //        //get prev results
        //        bool didExercise = false;
        //        var prev_results = _context.Results.Where(id => id.StudentId == curr_student.Id).ToList();

        //        foreach (var result in prev_results)
        //        {
        //            if (result.ExerciseId == ExerciseId)
        //            {
        //                didExercise = true;//tell user "already did exercise"
        //            }

        //        }

        //        if (didExercise == false)
        //        {
        //            var newResult = new Result { ExerciseId = exercise.Id, Topic = exercise.Topic, StudentId = curr_student.Id, TotalScore = TotalScore, StudentScore = StudentScore };

        //            await CreateResult(newResult);

        //            await _context.SaveChangesAsync();

        //        }
        //        //return RedirectToAction("Index");
        //        return Json(new { resultID= "Ваш балл " + StudentScore + "/" + TotalScore });
        //    }
        //    //return Ok();
        //    return Json(new { resultID = "Ваш балл " + StudentScore + "/" + TotalScore });
        //}


        ////GET
        //public async Task<IActionResult> TranslateExercise(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var exercise = await _context.Exercises.FindAsync(id);
        //    List<Question> QuestionList = _context.Questions.Where(q => q.Topic == exercise.Topic).ToList();

        //    if (QuestionList == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(QuestionList);
        //    // return View(await _context.MultipleChoiceQuestions.ToListAsync());
        //}

        //[HttpPost]

        //public async Task<IActionResult> TranslateExercise(string topic, List<string> answers)
        //{
        //    var st = topic;
        //    var ans = answers;

        //    int TotalScore = 0;
        //    int StudentScore = 0;

        //    var exercise = _context.Exercises.FirstOrDefault(t => t.Topic == topic);
        //    TotalScore = exercise.TotalScore;

        //    List<Question> questionList = _context.Questions.Where(topic => topic.Topic == exercise.Topic).ToList();
        //    if (questionList != null)
        //    {
        //        //result calculation
        //        for (int i = 0; i < questionList.Count; i++)
        //        {
        //            if (answers[i] == questionList[i].Answer)
        //            {
        //                StudentScore += questionList[i].Score;//add score
        //            }


        //            //if (item.Answer == exercise.Questions.FirstOrDefault(qId => qId.Id == item.Id).Answer)
        //            //{
        //            //    StudentScore += 1;
        //            //}
        //            // _context.Entry(item).State = EntityState.Modified;
        //            // _context.Entry(item).Property(x => x.Topic).IsModified = false;
        //            // _context.Entry(item).Property(x => x.Questiion).IsModified = false;

        //        }

        //        //create result record
        //        var newResult = new Result { ExerciseId = exercise.Id, Topic = exercise.Topic, StudentId = 1, TotalScore = TotalScore, StudentScore = StudentScore };

        //        await CreateResult(newResult);

        //        await _context.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return Ok();
        //}
        ////

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateResult(Result result)
        {
            if (result!=null)
            {
                _context.Add(result);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return View("Failed to create");
        }


        /*Custom Methods end*/

        // GET: Exercises/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null)
            {
                return NotFound();
            }
            return View(exercise);
        }

        // POST: Exercises/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Topic,NumberOfQuestions,TotalMark")] Exercise exercise)
        {
            if (id != exercise.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    _context.Update(exercise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExerciseExists(exercise.Id))
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
            return View(exercise);
        }

        // GET: Exercises/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }

        // POST: Exercises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExerciseExists(int id)
        {
            return _context.Exercises.Any(e => e.Id == id);
        }
    }
}
