using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ESchool.Models.MultipleChoice
{
    public class MultipleChoiceQuestion
    {
        public int Id { get; set; }

        public string Topic { get; set; }

        public int QuestionNumber { get; set; }

        [Display(Name = "Вопрос ")]
        public string Questiion { get; set; }

        [Display(Name = "Выбор а")]
        public string choiceA { get; set; }

        [Display(Name = "Выбор б")]
        public string choiceB { get; set; }

        [Display(Name = "Выбор в")]
        public string choiceC { get; set; }

        [Display(Name = "Выбор г")]
        public string choiceD { get; set; }

        [Display(Name = "Ответы")]
        public string Answer { get; set; }

        [Display(Name = "балл")]
        public int Score { get; set; }

    }
}
