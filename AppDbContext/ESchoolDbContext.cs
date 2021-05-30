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
        public DbSet<Result> Results { get; set; }
        

        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }//redundant,use identity
    }
}
