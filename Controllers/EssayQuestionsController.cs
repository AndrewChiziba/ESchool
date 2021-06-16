using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ESchool.AppDbContext;
using ESchool.Models.CreateEssay;
using System.IO;
using System.Security.Claims;

namespace ESchool.Controllers
{
    public class EssayQuestionsController : Controller
    {
        private readonly ESchoolDbContext _context;
        private readonly IWebHostEnvironment _hostingEnv;
        string curr_userId;

        public EssayQuestionsController(IWebHostEnvironment hostingEnv, ESchoolDbContext context)
        {
            _hostingEnv = hostingEnv;
            _context = context;
        }

        // GET: EssayQuestions
        public async Task<IActionResult> Index()
        {
            return View(await _context.EssayQuestions.ToListAsync());
        }


        /*custom start*/

        public async Task<IActionResult> ListEssayQuestions()
        {
            return View(await _context.EssayQuestions.ToListAsync());
        }
        // GET: Questions/Create
        public IActionResult AddEssayQuestion()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEssayQuestion(EssayVM essayVM)
        {
            curr_userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curr_teacher = _context.Teachers.First(id => id.UserId == curr_userId);

            if (essayVM.File != null)
            {
                //upload files to wwwroot
                var fileName = Path.GetFileName(essayVM.File.FileName);
                //judge if it is pdf file
                string ext = Path.GetExtension(essayVM.File.FileName);
                if (ext.ToLower() != ".pdf")
                {
                    return View();
                }
                var filePath = Path.Combine(_hostingEnv.WebRootPath, "materials/essays", fileName);

                using (var fileSteam = new FileStream(filePath, FileMode.Create))
                {
                    await essayVM.File.CopyToAsync(fileSteam);
                }
                //your logic to save filePath to database, for example
                var num_questions = _context.EssayQuestions.Where(d => d.CourseId == curr_teacher.CourseId).Count();

                EssayQuestion essayQuestion = new EssayQuestion
                {
                    CourseId=curr_teacher.CourseId,
                    QuestionNumber = num_questions + 1,
                    EssayDescription = essayVM.EssayDescription,
                    Score = essayVM.Score,
                    sampleFilePath = filePath,
                };

                _context.EssayQuestions.Add(essayQuestion);
                await _context.SaveChangesAsync();
            }
            else
            {

            }
            return View();
        }

        //View Essay
        public IActionResult ViewEssay(string filePath)
        {

            //byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            //string fileName = "myfile.pdf";
            // return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            //For preview pdf and the download it use below code
            var stream = new FileStream(filePath, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        //download Essay
        public IActionResult DownloadEssay(string filePath)
        {

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            //string fileName = "myfile.pdf";
            // return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            //For preview pdf and the download it use below code
            var stream = new FileStream(filePath, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }
        /*custom end*/








        // GET: EssayQuestions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var essayQuestion = await _context.EssayQuestions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (essayQuestion == null)
            {
                return NotFound();
            }

            return View(essayQuestion);
        }

        // GET: EssayQuestions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EssayQuestions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,QuestionNumber,Questiion,samplefileFilePath,Score")] EssayQuestion essayQuestion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(essayQuestion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(essayQuestion);
        }

        // GET: EssayQuestions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var essayQuestion = await _context.EssayQuestions.FindAsync(id);
            if (essayQuestion == null)
            {
                return NotFound();
            }
            return View(essayQuestion);
        }

        // POST: EssayQuestions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,QuestionNumber,Questiion,samplefileFilePath,Score")] EssayQuestion essayQuestion)
        {
            if (id != essayQuestion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(essayQuestion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EssayQuestionExists(essayQuestion.Id))
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
            return View(essayQuestion);
        }

        // GET: EssayQuestions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var essayQuestion = await _context.EssayQuestions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (essayQuestion == null)
            {
                return NotFound();
            }

            return View(essayQuestion);
        }

        // POST: EssayQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var essayQuestion = await _context.EssayQuestions.FindAsync(id);
            _context.EssayQuestions.Remove(essayQuestion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EssayQuestionExists(int id)
        {
            return _context.EssayQuestions.Any(e => e.Id == id);
        }
    }
}
