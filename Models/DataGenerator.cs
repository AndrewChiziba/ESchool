using System;
using System.Linq;
using ESchool.AppDbContext;
using ESchool.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TestingApp
{
    class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ESchoolDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ESchoolDbContext>>()))
            {
                //Look for any Teachers
                if (context.Teachers.Any())
                    {
                        return;   // Data was already seeded
                    }

                context.Teachers.AddRange(
                    new Teacher
                    {
                     
                        Surname = "Jon",
                        Name = "Don",
                        MiddleName = "Fon",
                        FullName = "Jon Fon Don",
                        CourseId = 2,
                    },
                    new Teacher
                    {

                        Surname = "Hon",
                        Name = "Kon",
                        MiddleName = "Bon",
                        FullName = "Hon Kon Bon",
                        CourseId = 2,
                    },

                    new Teacher
                    {

                        Surname = "Ken",
                        Name = "Basa",
                        MiddleName = "Domas",
                        FullName = "Ken Basa Domas",
                        CourseId = 3,
                    },
                    new Teacher
                    {

                        Surname = "Jon",
                        Name = "Don",
                        MiddleName = "Fon",
                        FullName = "Jon Fon Don",
                        CourseId = 4,
                    },
                    new Teacher
                    {

                        Surname = "Jon",
                        Name = "Don",
                        MiddleName = "Fon",
                        FullName = "Jon Fon Don",
                        CourseId = 5,
                    }
                    );

                context.SaveChanges();
            }
        }
    }
}
