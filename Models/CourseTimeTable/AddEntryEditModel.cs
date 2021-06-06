using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESchool.Models.CourseTimeTable
{
    public class AddEntryEditModel
    {
        public string CourseName { get; set; }
       
        public string Activity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int TimeTableId { get; set; }
    }
}
