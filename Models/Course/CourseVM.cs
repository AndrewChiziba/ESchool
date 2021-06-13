using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESchool.Models.Course
{
    public class CourseVM
    {
        public string CourseName { get; set; }
        public string Description { get; set; }
        public string TeacherName { get; set; }

        public DateTime CourseStart { get; set; }
        public DateTime CourseEnd { get; set; }

        public int TimeTableId { get; set; }
        public int TeacherId { get; set; }
        public virtual Course course { get; set; }
        public virtual List<Teacher> Teachers { get; set; }
    }
}
