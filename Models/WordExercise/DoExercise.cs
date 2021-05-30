using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ESchool.Models.WordExercise
{
    public class DoExercise
    {
        public int Id { get; set; }

        public string Topic { get; set; }

        [Display(Name = "WordToTranslate")]
        [Required]
        public string questiion { get; set; }

        [Display(Name = "Translatedword")]
        [Required]
        public string answer { get; set; }
    }
}
