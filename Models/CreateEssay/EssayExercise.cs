using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESchool.Models.CreateEssay
{
    public class EssayExercise
    {
        public int Id { get; set; }
        public string samplefileFilePath { get; set; }

        public int TotalScore { get; set; }
        
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        
    }
}
