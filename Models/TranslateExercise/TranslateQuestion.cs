using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ESchool.Models.TranslateExercise
{
    public class TranslateQuestion
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }

        public int QuestionNumber { get; set; }
        public string Questiion { get; set; }
        public string Answer { get; set; }
        public int Score { get; set; }
    }
}
