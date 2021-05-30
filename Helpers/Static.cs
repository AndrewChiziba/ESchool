using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ESchool.Helpers
{
    public class Roles
    {
        public const string Admin = "Admin";
        public const string Student = "Student";
        public const string Teacher = "Teacher";
    }
    public class TrueOrFalse
    {
        [NotMapped]
        public bool IsAdmin { get; set; }
    }
}
