using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ESchool.AppDbContext;
using ESchool.Models.Course;
using System.Security.Claims;

namespace ESchool.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ESchoolDbContext _context;
        string curr_userId;

        public CoursesController(ESchoolDbContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            return View(await _context.Courses.ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curr_course = await _context.Courses
                .FirstOrDefaultAsync(m => m.Id == id);
            var teacher_Name = _context.Teachers.First(d => d.Id == curr_course.TeacherId).FullName;

            if (curr_course == null)
            {
                return NotFound();
            }

            var courseVM = new CourseVM
            { 
                course = curr_course,
                TeacherName = teacher_Name
            };

            return View(courseVM);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            var teachers = _context.Teachers.ToList();
            CourseVM courseVM = new CourseVM
            {
                Teachers=teachers,
            };
            return View(courseVM);
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CourseName,Description,TimeTableId,TeacherId,CourseStart,CourseEnd")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        /*custom start*/

        // GET: Courses/Edit/5
        public async Task<IActionResult> RegisterToCourse(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(Id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/RegisterToCourse

        [HttpPost]
        public async Task<IActionResult> RegisterToCourse(int Id)
        {
            curr_userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curr_student = _context.Students.First(id => id.UserId == curr_userId);

            var course = await _context.Courses.FindAsync(Id);
            if (course == null)
            {
                return NotFound();
            }

            else 
            {
                if(curr_student.CourseId!= 0)
                {
                    return RedirectToAction("AlreadyRegistered","Courses", new { Id = Id });
                }
                curr_student.CourseId = course.Id;
                _context.Update(curr_student);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction("Index","HomeController");
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> AlreadyRegistered(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(Id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }


            // GET: Courses/Edit/5
            public async Task<IActionResult> EditCustom(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curr_course = await _context.Courses.FindAsync(id);
            var teachers = _context.Teachers.ToList();

            if (curr_course == null)
            {
                return NotFound();
            }

            CourseVM courseVM = new CourseVM
            {
                Teachers = teachers,
                course = curr_course

            };

            return View(courseVM);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCustom(/*int id, [Bind("Id,CourseName,Description,TimeTableId,TeacherId")]*/ CourseVM courseVM)
        {
            //var course = courseVM.course;
            //if (course.Id == null)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(courseVM.course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(courseVM.course.Id))
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
            return View(courseVM.course);
        }

        /*custom end*/

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseName,Description,TimeTableId,TeacherId,CourseStart,CourseEnd")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
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
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}
