using ESchool.AppDbContext;
using ESchool.Helpers;
using ESchool.Models;
using ESchool.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESchool.Controllers
{
    public class AccountController : Controller
    {
        private readonly ESchoolDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<IdentityUser> userManager,
                                      SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, ESchoolDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var user = new IdentityUser{UserName = model.Email, Email = model.Email, MiddleName = model.MiddleName, Name = model.Name, Surname = model.Surname };
                var user = new IdentityUser { UserName = model.Email, Email = model.Email};

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //create roles if they dont exist
                    if (!await _roleManager.RoleExistsAsync(Roles.Admin))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(Roles.Admin));
                        await _userManager.AddToRoleAsync(user, Roles.Admin);//delete 1st User
                    }
                    if (!await _roleManager.RoleExistsAsync(Roles.Teacher))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(Roles.Teacher));
                    }

                    if (!await _roleManager.RoleExistsAsync(Roles.Student))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(Roles.Student));
                    }

                    //Assign user to a role as per check box selection
                    if (model.IsTeacher==true)
                    {
                        await _userManager.AddToRoleAsync(user, Roles.Teacher);
                        //add teacher
                        var newTeacher = new Teacher 
                        { 
                            Name = model.Name,
                            Surname = model.Surname, 
                            MiddleName = model.MiddleName, 
                            FullName = model.Surname + " " + model.Name + " " +model.MiddleName,
                            UserId = user.Id 
                        };
                       await AddTeacher(newTeacher);

                        
                    }
                    else if(User.IsInRole(Roles.Admin)==false) 
                    {
                        await _userManager.AddToRoleAsync(user, Roles.Student);

                        var newStudent = new Student 
                        { 
                            Name = model.Name,
                            Surname = model.Surname, 
                            MiddleName = model.MiddleName,
                            FullName = model.Surname + " " + model.Name + " " + model.MiddleName,
                            UserId = user.Id 
                        };
                        await AddStudent(newStudent);

                        
                    }


                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    await Logout();
                    return RedirectToAction("index", "Home");
                }

                
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(model);
        }

        //create student

        public async Task<IActionResult> AddStudent(Student newStudent)
        {
            if (newStudent != null)
            {
                _context.Add(newStudent);
               await _context.SaveChangesAsync();
                return Ok();
            }  
            else
                return RedirectToAction("index");
        }

        //create teacher
        public async Task<IActionResult> AddTeacher(Teacher newTeacher)
        {
            if (newTeacher != null)
            {
                _context.Add(newTeacher);
               await _context.SaveChangesAsync();
                return Ok();
            }  
            else
                return RedirectToAction("index");
        }

 
        //Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(user);
        }

        //Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }

    }
}
