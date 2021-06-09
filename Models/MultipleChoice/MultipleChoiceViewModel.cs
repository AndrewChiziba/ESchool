using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESchool.Models.MultipleChoice
{
    public class MultipleChoiceViewModel
    {
        public string Topic { get; set; }
        public int NumberOfQuestions { get; set; }
        public int TotalScore { get; set; }

        public virtual List<MultipleChoiceQuestion> MultipleChoiceQuestions { get; set; }

        public int ExerciseId { get; set; }
        public string questiion { get; set; }
        public string answer { get; set; }
    }
}
