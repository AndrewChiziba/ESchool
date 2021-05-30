using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESchool.Models.WordExercise
{
    public class QuestionsEdit
    {
        public List<Question> Questions { set; get; }
        public QuestionsEdit()
        {
            if (Questions == null)
                Questions = new List<Question>();
        }
    }
}
