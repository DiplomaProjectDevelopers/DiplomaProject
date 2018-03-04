using DiplomaProject.Domain.Entities;
using DiplomaProject.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomaProject.Domain.Initializer
{
    public class DbInitializer : IDbInitializer
    {


        private readonly DiplomaProjectContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public DbInitializer(
            DiplomaProjectContext context,
            UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //This example just creates an Administrator role and one Admin users
        public async Task Initialize()
        {
            try
            {
                //create database schema if none exists
                _context.Database.Migrate();
                //If there is already an Administrator role, abort
                if (!_context.Roles.Any())
                {
                    _roleManager.CreateAsync(new Role("BaseAdmin")).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new Role("ProfessionAdmin")).GetAwaiter().GetResult();
                }

                if (!_context.Faculties.Any())
                {
                    var faculties = new List<Faculty>
                    {
                        new Faculty
                        {
                            Name = "Քոմփյութերային համակարգեր և ինֆորմատիկա",
                        }
                    };
                    _context.Faculties.AddRange(faculties);
                    _context.SaveChanges();
                }

                if (!_context.Departments.Any())
                {
                    var departments = new List<Department>
                    {
                        new Department
                        {
                            Name= "ՏԱԾԱ",
                            FacultyId = 1

                        }
                    };
                    _context.Departments.AddRange(departments);
                    _context.SaveChanges();
                }

                if (!_context.Professions.Any())
                {
                    var professions = new List<Profession>
                    {
                        new Profession
                        {
                            Name = "Տեղեկատվական անվտանգություն",
                            DepartmentId = 1,
                        },
                        new Profession
                        {
                            Name = "Ինֆորմատիկա և ծրագրավորում",
                            DepartmentId = 1
                        }
                    };
                    _context.Professions.AddRange(professions);
                    _context.SaveChanges();
                }

                if (!_context.OutComeTypes.Any())
                {
                    var types = new List<OutComeType>
                    {
                        new OutComeType
                        {
                            Name= "Գիտելիք"
                        },
                        new OutComeType
                        {
                            Name ="Կարողություն"
                        },
                        new OutComeType
                        {
                            Name = "Հմտություն"
                        }
                    };
                    _context.OutComeTypes.AddRange(types);
                    _context.SaveChanges();
                }

                if (!_context.InitialSubjects.Any())
                {
                    var subjects = new List<InitialSubject>
                    {
                        new InitialSubject
                        {
                            Name = "Օբյեկտ կողմոնորշված և կոմպոնենտային ծրագրավորում",
                            ProfessionId = 1
                        },
                        new InitialSubject
                        {
                            Name = "Տվյալների բազաների նախագծման տեխնոլոգիաներ և պաշտպանություն",
                            ProfessionId = 1
                        }
                    };
                    _context.InitialSubjects.AddRange(subjects);
                    _context.SaveChanges();
                }

                if (!_context.InitialOutComes.Any())
                {
                    var outComes = new List<InitialOutCome>
                    {
                        new InitialOutCome
                        {
                            Name = "Կիրառական ծրագրերի գործնական նախագծման կարողություն",
                            SubjectId = 1,
                            TypeId = 2
                        },
                        new InitialOutCome
                        {
                            Name = "Պաշտպանված ծրագրային ապահովման նախագծման կարողություն",
                            SubjectId = 1,
                            TypeId = 2
                        },
                        new InitialOutCome
                        {
                            Name = "Խրագրային ապահովման թեստավորման հմտություն",
                            SubjectId = 1,
                            TypeId = 3
                        },
                        new InitialOutCome
                        {
                            Name ="Մասնագիտական գործնական գիտելիքներ",
                            SubjectId = 1,
                            TypeId = 1
                        },
                        new InitialOutCome
                        {
                            Name= "Մասնագիտական գործնական հմտություններ",
                            SubjectId = 1,
                            TypeId = 3
                        },
                        new InitialOutCome
                        {
                            Name = "Պաշտպանված քոմփյութերային համակարգերի նախագծման կարողություններ",
                            SubjectId = 2,
                            TypeId = 2
                        },
                        new InitialOutCome
                        {
                            Name = "Մասնագիտական գործնական գիտելիքներ",
                            SubjectId = 2,
                            TypeId = 1
                        }
                    };
                    _context.InitialOutComes.AddRange(outComes);
                    _context.SaveChanges();
                }


                if (!_context.FinalOutComes.Any())
                {
                    var outComes = new List<FinalOutCome>
                    {
                        new FinalOutCome
                        {
                            Name = "Կիրառական ծրագրերի գործնական նախագծման կարողություն",
                            TypeId = 2,
                            ProfessionId = 1
                        },
                        new FinalOutCome
                        {
                            Name = "Պաշտպանված ծրագրային ապահովման նախագծման կարողություն",
                            TypeId = 2,
                            ProfessionId = 1
                        },
                        new FinalOutCome
                        {
                            Name = "Խրագրային ապահովման թեստավորման հմտություն",
                            TypeId = 3,
                            ProfessionId = 1
                        },
                        new FinalOutCome
                        {
                            Name ="Մասնագիտական գործնական գիտելիքներ",
                            TypeId = 1,
                            ProfessionId = 1
                        },
                        new FinalOutCome
                        {
                            Name= "Մասնագիտական գործնական հմտություններ",
                            TypeId = 3,
                            ProfessionId = 1
                        },
                        new FinalOutCome
                        {
                            Name = "Պաշտպանված քոմփյութերային համակարգերի նախագծման կարողություններ",
                            TypeId = 2,
                            ProfessionId = 1
                        },
                        new FinalOutCome
                        {
                            Name = "Մասնագիտական գործնական գիտելիքներ",
                            TypeId = 1,
                            ProfessionId = 1,
                            IsNew = true
                        }
                    };
                    _context.FinalOutComes.AddRange(outComes);
                    _context.SaveChanges();
                }


                //Create the Administartor Role

                //Create the default Admin account and apply the Administrator role
                string password = "sa";
                var users = new List<User>
                {
                    new User
                    {
                        FirstName = "Hakob",
                        LastName = "Papazyan",
                        PhoneNumber = "093697343",
                        UserName = "hakob_papazyan",
                        Email = "hakobpapazyan2@gmail.com",
                        EmailConfirmed = true
                    },
                    new User
                    {
                        Email = "sa@sa.com",
                        UserName = "sa",
                        EmailConfirmed = true,
                    },
                    new User
                    {
                        FirstName = "Liana",
                        LastName = "Grigoryan",
                        UserName = "liana_grigoryan",
                        Email = "lgrigoryan25@gmail.com",
                        EmailConfirmed = true
                    },
                    new User
                    {
                        FirstName = "Qristine",
                        LastName = "Serobyan",
                        UserName = "qristine_serobyan",
                        Email = "serobyanqristine@gmail.com",
                        EmailConfirmed = true
                    },
                    new User
                    {
                        FirstName = "Kim",
                        LastName = "Sargsyan",
                        UserName = "kim_sargsyan",
                        Email = "kim.sargsian@gmail.com",
                        EmailConfirmed = true
                    }
                };
                if (!_context.Users.Any())
                {
                    foreach (var user in users)
                    {
                        await _userManager.CreateAsync(user, password);
                        await _userManager.AddToRolesAsync(user, new[] { "ProfessionAdmin" });
                    }
                    await _userManager.AddToRoleAsync(users[0], "BaseAdmin");
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
