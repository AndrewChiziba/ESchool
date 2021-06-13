using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ESchool.AppDbContext;
using ESchool.Models.CourseTimeTable;
using System.Security.Claims;

namespace ESchool.Controllers
{
    public class TimeTablesController : Controller
    {
        private readonly ESchoolDbContext _context;
        string curr_userId;

        public TimeTablesController(ESchoolDbContext context)
        {
            _context = context;
        }

        // GET: Timetables
        public async Task<IActionResult> Index()
        {
            return View(await _context.TimeTables.ToListAsync());
        }

        // GET: TimeTables
        //public async Task<IActionResult> Index()
        //{
        //    var timeTable = await _context.TimeTables.ToListAsync();
        //    TimeTableViewModel timeTableViewModel = new TimeTableViewModel
        //    {

        //    };




        //    TimeTables = await _context.TimeTables.ToListAsync();
        //    return View(TimeTableVM);
        //}
        //public async Task<IActionResult> Index(TimeTableViewModel TimeTableVM)
        //{
        //    //curr_userId = User.FindFirstValue(ClaimTypes.NameIdentifier);// user's userId
        //    curr_userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var curr_teacher = _context.Teachers.First(id => id.UserId == curr_userId);
        //    TimeTableVM.TeacherName = curr_teacher.Name + " " + curr_teacher.MiddleName;


        //    TimeTableVM.TimeTables = await _context.TimeTables.ToListAsync();
        //    return View(TimeTableVM);
        //}
        // GET: TimeTables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeTable = await _context.TimeTables
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timeTable == null)
            {
                return NotFound();
            }

            return View(timeTable);
        }

        // GET: TimeTables/Create
        public IActionResult Create()
        {
            curr_userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curr_teacher = _context.Teachers.First(id => id.UserId == curr_userId);

            var teachersCourses = _context.Courses.Where(d=>d.Id==curr_teacher.CourseId).ToList();
            
            if (teachersCourses == null)
            {
                return NotFound();
            }

            TimeTableViewModel timetableVM = new TimeTableViewModel
            {
                CourseList = teachersCourses
                
            };
            return View(timetableVM); ;
        }


        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CourseId")] TimeTable timeTable)
        {
            curr_userId = User.FindFirstValue(ClaimTypes.NameIdentifier);// user's

            if (ModelState.IsValid)
            {
                var curr_teacher = _context.Teachers.First(id=>id.UserId==curr_userId);
                timeTable.TeacherId = curr_teacher.Id;

                _context.Add(timeTable);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(timeTable);
        }

        /*Custom*/


        // GET: TimeTables/Edit/5
        //public async Task<IActionResult> CheckTT(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    curr_userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var curr_teacher = _context.Teachers.First(id => id.UserId == curr_userId);
            
        //    var tt = await _context.TimeTables.FindAsync(id);
        //    tt.TTEntries = _context.TTEntries.Where(t => t.TimeTableId == tt.Id).ToList();

        //    if (tt == null)
        //    {
        //        return NotFound();
        //    }
        //    TimeTableViewModel TTViewModel = new TimeTableViewModel
        //    {
        //        CourseName = tt.CourseName,
        //        TeacherName = curr_teacher.Name + " " + curr_teacher.MiddleName, 
        //        TTEntries = tt.TTEntries
        //    };


        //    return View(TTViewModel);
        //}

        // POST: TimeTables/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckTT(int id, [Bind("Id,CourseName,TeacherId")] TimeTable timeTable)
        {
            if (id != timeTable.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(timeTable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimeTableExists(timeTable.Id))
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
            return View(timeTable);
        }


        // GET: TTEntries/Create
        //public IActionResult AddEntry(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var tt =  _context.TimeTables.Find(id);
        //    if (tt == null)
        //    {
        //        return NotFound();
        //    }
        //    AddEntryEditModel addEntryEditModel = new AddEntryEditModel
        //    {
        //        CourseName = tt.CourseName,

        //        TimeTableId = tt.Id
        //    };

        //    return View(addEntryEditModel);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEntry(AddEntryEditModel addEntryEditModel)
        {
            TTEntry tTEntry = new TTEntry
            {
                Activity = addEntryEditModel.Activity,
                StartDate = addEntryEditModel.StartDate,
                EndDate = addEntryEditModel.EndDate,
                TimeTableId = addEntryEditModel.TimeTableId,
                

            };
            //if (ModelState.IsValid)
            //{
                _context.Add(tTEntry);
                await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            return View(addEntryEditModel);
        }





        /*Custom ending*/

        // GET: TimeTables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeTable = await _context.TimeTables.FindAsync(id);
            if (timeTable == null)
            {
                return NotFound();
            }
            return View(timeTable);
        }
        // POST: TimeTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseId,TeacherId")] TimeTable timeTable)
        {
            if (id != timeTable.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(timeTable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimeTableExists(timeTable.Id))
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
            return View(timeTable);
        }

        // GET: TimeTables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeTable = await _context.TimeTables
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timeTable == null)
            {
                return NotFound();
            }

            return View(timeTable);
        }

        // POST: TimeTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var timeTable = await _context.TimeTables.FindAsync(id);
            _context.TimeTables.Remove(timeTable);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TimeTableExists(int id)
        {
            return _context.TimeTables.Any(e => e.Id == id);
        }
    }
}
