using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESchool.Models.ResultsRecord
{
    public class Result
    {
        public int Id { get; set; }

        public string Topic { get; set; }
        public int StudentID { get; set; }
        public int ExerciseID { get; set; }
        public int TotalScore { get; set; }
        public int StudentScore { get; set; }
    }
}
