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

namespace ESchool.Controllers
{
    public class ExercisesController : Controller
    {
        private readonly ESchoolDbContext _context;

        public ExercisesController(ESchoolDbContext context)
        {
            _context = context;
        }

        // GET: Exercises
        public async Task<IActionResult> Index()
        {
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
            return View();
        }

        // POST: Exercises/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOg([Bind("Id,Topic,NumberOfQuestions,TotalMark")] Exercise exercise)
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

        //Question
        public async Task<IActionResult> CreateBlankQuestion(ExerciseViewModel model, int qnumber)
        {
            var newQuestion = new Question { Topic = model.Topic,QuestionNumber = qnumber, Questiion = "", Answer = "", Score = 0};
            if (newQuestion != null)
            {
                _context.Add(newQuestion);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExerciseViewModel model)
        {
            if (ModelState.IsValid)
            {
                for (int i = 0; i < model.NumberOfQuestions; i++)
                {
                    await CreateBlankQuestion(model,i+1);
                }

                var listQuestions = _context.Questions.Where(question => question.Topic == model.Topic).ToList();

                var newExercise = new Exercise { Topic = model.Topic, NumberOfQuestions = model.NumberOfQuestions, Questions = listQuestions };

                _context.Add(newExercise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        //Edit Question
        //GET
        public async Task<IActionResult> EditQuestions(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises.FindAsync(id);

            List<Question> questionList = new List<Question>();
            // you can replace this manual filling with data from database
            questionList = _context.Questions.Where(question => question.Topic == exercise.Topic).ToList();
            
            return View(questionList);
        }

        //[HttpPost]
        ////public ActionResult EditQuestions
        //    public async Task<IActionResult> EditQuestions(int id, QuestionsEdit model)
        //{
        //    int TotalScore = 0;
        //    if (model.Questions != null)
        //    {
        //        foreach (var item in model.Questions)
        //        {
        //            _context.Entry(item).State = EntityState.Modified;
        //            _context.Entry(item).Property(x => x.Topic).IsModified = false;
        //            _context.Entry(item).Property(x => x.QuestionNumber).IsModified = false;
        //            TotalScore += item.Score;
        //        }
        //        var exercise = await _context.Exercises.FindAsync(id);
        //        exercise.TotalScore = TotalScore;

        //        await _context.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return View(model);
        //}

        //Do exercise
        //GET
        public async Task<IActionResult> DoExercise(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises.FindAsync(id);
            List<Question> QuestionList = _context.Questions.Where(q => q.Topic == exercise.Topic).ToList();

            if (QuestionList == null)
            {
                return NotFound();
            }
            return View(QuestionList);
            // return View(await _context.MultipleChoiceQuestions.ToListAsync());
        }

        [HttpPost]
        
        public async Task<IActionResult> DoExercise(string topic, List<string>answers)
        {
            var st = topic;
            var ans = answers;

            int TotalScore = 0;
            int StudentScore = 0;

            var exercise = _context.Exercises.FirstOrDefault(t=>t.Topic==topic);
            TotalScore = exercise.TotalScore;
            
            List<Question> questionList = _context.Questions.Where(topic => topic.Topic == exercise.Topic).ToList();
            if (questionList != null)
            {
                //result calculation
                for(int i=0;i<questionList.Count;i++)
                {
                    if (answers[i] == questionList[i].Answer)
                    {
                        StudentScore += questionList[i].Score;//add score
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
                var newResult = new Result { ExerciseID = exercise.Id, Topic = exercise.Topic, StudentID = 1, TotalScore = TotalScore, StudentScore = StudentScore };

                await CreateResult(newResult);

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return Ok();
        }


        //GET
        public async Task<IActionResult> TranslateExercise(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises.FindAsync(id);
            List<Question> QuestionList = _context.Questions.Where(q => q.Topic == exercise.Topic).ToList();

            if (QuestionList == null)
            {
                return NotFound();
            }
            return View(QuestionList);
            // return View(await _context.MultipleChoiceQuestions.ToListAsync());
        }

        [HttpPost]

        public async Task<IActionResult> TranslateExercise(string topic, List<string> answers)
        {
            var st = topic;
            var ans = answers;

            int TotalScore = 0;
            int StudentScore = 0;

            var exercise = _context.Exercises.FirstOrDefault(t => t.Topic == topic);
            TotalScore = exercise.TotalScore;

            List<Question> questionList = _context.Questions.Where(topic => topic.Topic == exercise.Topic).ToList();
            if (questionList != null)
            {
                //result calculation
                for (int i = 0; i < questionList.Count; i++)
                {
                    if (answers[i] == questionList[i].Answer)
                    {
                        StudentScore += questionList[i].Score;//add score
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
                var newResult = new Result { ExerciseID = exercise.Id, Topic = exercise.Topic, StudentID = 1, TotalScore = TotalScore, StudentScore = StudentScore };

                await CreateResult(newResult);

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return Ok();
        }
        //

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
