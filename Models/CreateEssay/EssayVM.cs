using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESchool.Models.CreateEssay
{
    public class EssayVM
    {
        public string Title { get; set; }
        public IFormFile File { get; set; }

        public string EssayDescription { get; set; }


        public string samplefileFilePath { get; set; }

        public int Score { get; set; }
    }
}
