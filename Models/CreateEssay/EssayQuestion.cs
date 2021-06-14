using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESchool.Models.CreateEssay
{
    public class EssayQuestion
    {
        public int Id { get; set; }

        public int CourseId { get; set; }
        public int QuestionNumber { get; set; }
        public string EssayDescription { get; set; }

        
        public string sampleFilePath { get; set; }

        public int Score { get; set; }
    }
}
