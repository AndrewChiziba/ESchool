using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ESchool.Models
{
    public class Student
    {
        public int Id { get; set; }
        //
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }
        
        //FKs
        //[ForeignKey("AspNetUsers")]
        public string UserId { get; set; }

        public int CourseId { get; set; }

    }
}
