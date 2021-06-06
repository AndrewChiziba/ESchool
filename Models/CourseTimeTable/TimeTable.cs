using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESchool.Models.CourseTimeTable
{
    public class TimeTable
    {
        public int Id { get; set; }

        public string CourseName { get; set; }
        public int TeacherId { get; set; }

        public List<TTEntry> TTEntries { get; set; }

    }
}
