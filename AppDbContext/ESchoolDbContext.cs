using ESchool.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESchool.Models.WordExercise;
using ESchool.Models.ResultsRecord;
using ESchool.Models.MultipleChoice;
using ESchool.Models.EduMaterial;
using ESchool.Models.CourseTimeTable;
using ESchool.Models.TranslateExercise;
using ESchool.Models.Course;
using ESchool.Models.CreateEssay;

namespace ESchool.AppDbContext
{
    public class ESchoolDbContext : IdentityDbContext
    {
        private readonly DbContextOptions _options;

        public ESchoolDbContext(DbContextOptions options) : base(options)
        {
            _options = options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
        //DbSets below

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Exercise> Exercises { get; set; }

        public DbSet<MultipleChoiceQuestion> MultipleChoiceQuestions { get; set; }
        public DbSet<MultipleChoiceExercise> MultipleChoiceExercises { get; set; }

        public DbSet<TranslateQuestion> TranslateQuestions { get; set; }
        public DbSet<TranslateExercise> TranslateExercises { get; set; }

        public DbSet<EssayQuestion> EssayQuestions { get; set; }
        public DbSet<EssayExercise> EssayExercises { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<PdfMaterial> PdfMaterials { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<TimeTable> TimeTables { get; set; }
        public DbSet<TTEntry> TTEntries { get; set; }

        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }//redundant,use identity
    }
}
