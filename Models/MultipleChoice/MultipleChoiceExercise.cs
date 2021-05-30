using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESchool.Models.MultipleChoice
{
    public class MultipleChoiceExercise
    {
        public int Id { get; set; }


        public string Topic { get; set; }
        public int NumberOfQuestions { get; set; }
        public int TotalScore { get; set; }

        public virtual List<MultipleChoiceQuestion> MultipleChoiceQuestions { get; set; }
    }
}
