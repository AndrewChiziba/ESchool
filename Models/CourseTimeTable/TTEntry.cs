using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESchool.Models.CourseTimeTable
{
    public class TTEntry
    {
        public int Id { get; set; }
        public string Activity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
