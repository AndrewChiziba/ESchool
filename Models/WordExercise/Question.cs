using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ESchool.Models.WordExercise
{
    public class Question
    {
        public int Id { get; set; }

        public int ExerciseId { get; set; }

        public int QuestionNumber { get; set; }

        [Display(Name = "Question")]
        public string Questiion { get; set; }

        [Display(Name = "Answer")]
        public string Answer { get; set; }

        public int Score { get; set; }
    }
}
