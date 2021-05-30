using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESchool.Models.WordExercise
{
    public class ExerciseViewModel
    {
        public string Topic { get; set; }
        public int NumberOfQuestions { get; set; }
        public int TotalScore { get; set; }

        public virtual List<Question> Questions { get; set; }

        //  
        public string questiion { get; set; }
        public string answer { get; set; }
    }
}
