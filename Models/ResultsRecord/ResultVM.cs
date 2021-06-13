using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESchool.Models.ResultsRecord
{
    public class ResultVM
    {
        public string Student { get; set; }
        public string Course { get; set; }
        public string Topic { get; set; }
        
        public int StudentScore { get; set; }
        public int TotalScore { get; set; }
        //public int FinalScore { get; set; }
        public virtual List<Result> Results { get; set; }

    }
}
