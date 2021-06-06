using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESchool.Models.CourseTimeTable
{
    [Keyless]
    public class TimeTableViewModel
    {
       public int CourseId { get; set; }
        public int TeacherId { get; set; }

        public string CourseName { get; set; }
        public string TeacherName { get; set; }

        public List<TimeTable> TimeTables { get; set; }
        public List<TTEntry> TTEntries { set; get; }
        public TimeTableViewModel()
        {
            if (TTEntries == null)
                TTEntries = new List<TTEntry>();
        }
    }
}
