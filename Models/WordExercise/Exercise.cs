using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESchool.Models.WordExercise
{
    public class Exercise
    {
        public int Id { get; set; }


        public string Topic { get; set; }
        public int NumberOfQuestions { get; set; }
        public int TotalScore { get; set; }

        public int TeacherId { get; set; }
        public int CourseId { get; set; }
        public virtual List<Question> Questions { get; set; }
    }
}
