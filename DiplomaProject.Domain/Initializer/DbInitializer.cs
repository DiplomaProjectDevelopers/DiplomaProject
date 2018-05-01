﻿using DiplomaProject.Domain.Entities;
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




                //Create the Administartor Role
                var roles = new List<Role>
                    {
                        new Role
                        {
                            Name = "BaseAdmin",
                            DisplayName = "Գլխավոր ադմին",
                            Priority = 1
                        }, //BaseAdmin
                        new Role
                        {
                            Name = "FacultyAdmin",
                            Priority = 2,
                            DisplayName = "Ֆակուլտետի ադմին"
                        }, //FacultyAdmin
                        new Role
                        {
                            Name = "DepartmentAdmin",
                            DisplayName = "Ամբիոնի ադմին",
                            Priority = 3
                        }, //DepartmentAdmin
                        new Role
                        {
                            Name="ProfessionAdmin",
                            Priority = 4,
                            DisplayName = "Մասնագիտության ադմին"
                        }, //ProfessionAdmin
                        new Role
                        {
                            Name="RequestSender",
                            Priority = 5,
                            DisplayName = "Հարցում կատարող"
                        }, //RequestSender
                        new Role
                        {
                            Name="SubjectMaker",
                            DisplayName = "Առարկաներ ձևավորող",
                            Priority = 5
                        }, //subjectMaker
                        new Role
                        {
                            Priority = 5,
                            Name = "LaborMaker",
                            DisplayName = "Աշխատատարություն որոշող"
                        }, //LaborMaker
                        new Role
                        {
                            Priority = 5,
                            Name = "CurriculumMaker",
                            DisplayName = "Ուսումնական պլան կազմող"
                        }, //CurriculumMaker
                        new Role
                        {
                            Name = "DefaultRole",
                            Priority = 6,
                            DisplayName = "Նոր օգտատեր"
                        } //Defaultrole
                    };

                if (!_context.Roles.Any())
                {
                    foreach (var role in roles)
                    {
                        await _roleManager.CreateAsync(role);
                    }
                }

                //Create the default Admin account and apply the Administrator role
                string password = "sa";
                var users = new List<User>
                {
                    new User
                    {
                        FirstName = "Base",
                        LastName = "Admin",
                        Email = "ba@sa.com",
                        EmailConfirmed = true,
                        UserName = "ba"
                    },
                    new User
                    {
                        FirstName = "Faculty",
                        LastName = "Admin",
                        Email = "fa@sa.com",
                        EmailConfirmed = true,
                        UserName = "fa"
                    },
                    new User
                    {
                        FirstName = "Department",
                        LastName = "Admin",
                        UserName = "da",
                        Email = "da@sa.com",
                        EmailConfirmed = true
                    },
                    new User
                    {
                        FirstName = "Profession",
                        LastName = "Admin",
                        UserName = "pa",
                        Email = "pa@sa.com",
                        EmailConfirmed = true
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
                        FirstName = "Hakob",
                        LastName = "Papazyan",
                        PhoneNumber = "093697343",
                        UserName = "hakob_papazyan",
                        Email = "hakobpapazyan2@gmail.com",
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
                    },
                    new User
                    {
                        FirstName = "Simple",
                        LastName = "Admin",
                        Email = "sa@sa.com",
                        UserName = "sa",
                        EmailConfirmed = true,
                    },
                };
                if (!_context.Users.Any())
                {
                    for (int i = 0; i < users.Count; i++)
                    {
                        await _userManager.CreateAsync(users[i], password);

                    }
                }

                if (!_context.StakeHolderTypes.Any())
                {
                    var types = new List<StakeHolderType>
                    {
                        new StakeHolderType
                        {
                            TypeName = "Գործատու",
                            ProfessionName = "Տնօրեն",
                            Coefficient = 1
                        },
                        new StakeHolderType
                        {
                            TypeName = "Գործատու",
                            ProfessionName = "Թիմի ղեկավար",
                            Coefficient = 5
                        },
                        new StakeHolderType
                        {
                            TypeName = "Գործատու",
                            ProfessionName = "Ծրագրավորող",
                            Coefficient = 4
                        },
                        new StakeHolderType
                        {
                            TypeName = "Դասախոս",
                            Coefficient = 5
                        },
                        new StakeHolderType
                        {
                            TypeName = "Ուսանող",
                            Coefficient = 2
                        },
                        new StakeHolderType
                        {
                            TypeName = "Շրջանավարտ",
                            Coefficient = 3
                        }
                    };
                    _context.StakeHolderTypes.AddRange(types);
                    _context.SaveChanges();
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
                if (!_context.Branches.Any())
                {
                    var branches = new List<Branch>
                    {
                        new Branch
                        {
                            Name = "Տեղեկատվական ոլորտ"
                        },
                        new Branch
                        {
                            Name = "Տեխնոլոգիական ոլորտ"
                        }
                    };
                    _context.Branches.AddRange(branches);
                    _context.SaveChanges();
                }
                if (!_context.SubjectModules.Any())
                {
                    var modules = new List<SubjectModule>()
                    {
                        new SubjectModule
                        {
                            Name = "Լեզուների մոդուլ",
                            Group = "Ընդհանուր կրթության կառուցամաս"
                        },
                        new SubjectModule
                        {
                            Name = "Հումանիտար դասընթացների մոդուլ",
                            Group = "Ընդհանուր կրթության կառուցամաս"
                        },
                        new SubjectModule
                        {
                            Name = "Կազմակերպատնտեսագիտական դասընթացների մոդուլ",
                            Group = "Ընդհանուր կրթության կառուցամաս"
                        },
                        new SubjectModule
                        {
                            Name = "Մաթեմատիկական և բնագիտական դասընթացների մոդուլ",
                            Group = "Ընդհանուր կրթության կառուցամաս"
                        },
                        new SubjectModule
                        {
                            Name = "Ճարտարագիտական դասընթացների մոդուլ",
                            Group = "Ընդհանուր կրթության կառուցամաս"
                        },
                        new SubjectModule
                        {
                            Name = "Բնագավառի պարտադիր ուսուցման դասընթացների մոդուլներ",
                            Group = "Մասնագիտական կրթության կառուցամաս"
                        },
                        new SubjectModule
                        {
                            Name = "Մասնագիտական պարտադիր ուսուցման դասընթացների մոդուլներ",
                            Group = "Մասնագիտական կրթության կառուցամաս"
                        },
                        new SubjectModule
                        {
                            Name = "Մասնագիտացման պարտադիր ուսուցման դասընթացների մոդուլներ",
                            Group = "Մասնագիտական կրթության կառուցամաս"
                        },
                        new SubjectModule
                        {
                            Name = "Մասնագիտացման կամընտրական ուսուցման դասընթացների մոդուլ",
                            Group = "Մասնագիտական կրթության կառուցամաս"
                        }
                    };
                    _context.SubjectModules.AddRange(modules);
                    _context.SaveChanges();

                }
                if (!_context.StakeHolders.Any())
                {
                    var stakeholders = new List<StakeHolder>
                    {
                        new StakeHolder
                        {
                            FirstName = "Hakob",
                            LastName = "Papazyan",
                            CompanyName = "Factumsoft LLC",
                            BranchId = 1,
                            Email = "hakobpapazyan2@gmail.com",
                            TypeId = 5
                        },
                        new StakeHolder
                        {
                            FirstName = "Kim",
                            LastName = "Sargsyan",
                            BranchId = 1,
                            Email = "kim.sargsian@gmail.com",
                            TypeId = 2
                        },
                        new StakeHolder
                        {
                            FirstName = "Kristine",
                            LastName = "Serobyan",
                            CompanyName = "Appa",
                            BranchId = 1,
                            Email = "serobyanqristine@gmail.com",
                            TypeId = 3
                        },
                        new StakeHolder
                        {
                            FirstName = "Lian",
                            LastName = "Grigoryan",
                            CompanyName = "NPUA",
                            BranchId = 1,
                            Email = "lgrigoryan25@gmail.com",
                            TypeId = 2
                        }
                    };
                    _context.AddRange(stakeholders);
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
                            BranchId = 1
                        },
                        new Profession
                        {
                            Name = "Ինֆորմատիկա և ծրագրավորում",
                            DepartmentId = 1,
                            BranchId = 1
                        }
                    };
                    _context.Professions.AddRange(professions);
                    _context.SaveChanges();
                }

                if (!_context.UserRoles.Any())
                {
                    var userroles = new List<UserRole>();
                    for (int i = 0; i < 9; i++)
                    {
                        userroles.Add(new UserRole { ProfessionId = 1, UserId = users[i].Id, RoleId = roles[i].Id });
                    }
                    _context.UserRoles.AddRange(userroles);
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
                            InitialSubjectId = 1,
                            OutComeTypeId = 2
                        },
                        new InitialOutCome
                        {
                            Name = "Պաշտպանված ծրագրային ապահովման նախագծման կարողություն",
                            InitialSubjectId = 1,
                            OutComeTypeId = 2
                        },
                        new InitialOutCome
                        {
                            Name = "Ծրագրային ապահովման թեստավորման հմտություն",
                            InitialSubjectId = 1,
                            OutComeTypeId = 3
                        },
                        new InitialOutCome
                        {
                            Name ="Մասնագիտական գործնական գիտելիքներ",
                            InitialSubjectId = 1,
                            OutComeTypeId = 1
                        },
                        new InitialOutCome
                        {
                            Name= "Մասնագիտական գործնական հմտություններ",
                            InitialSubjectId = 1,
                            OutComeTypeId = 3
                        },
                        new InitialOutCome
                        {
                            Name = "Պաշտպանված քոմփյութերային համակարգերի նախագծման կարողություններ",
                            InitialSubjectId = 2,
                            OutComeTypeId = 2
                        },
                        new InitialOutCome
                        {
                            Name = "Մասնագիտական գործնական գիտելիքներ",
                            InitialSubjectId = 2,
                            OutComeTypeId = 1
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
                            OutComeTypeId = 2,
                            ProfessionId = 1
                        },
                        new FinalOutCome
                        {
                            Name = "Պաշտպանված ծրագրային ապահովման նախագծման կարողություն",
                            OutComeTypeId = 2,
                            ProfessionId = 1
                        },
                        new FinalOutCome
                        {
                            Name = "Ծրագրային ապահովման թեստավորման հմտություն",
                            OutComeTypeId = 3,
                            ProfessionId = 1
                        },
                        new FinalOutCome
                        {
                            Name ="Մասնագիտական գործնական գիտելիքներ",
                            OutComeTypeId = 1,
                            ProfessionId = 1
                        },
                        new FinalOutCome
                        {
                            Name= "Մասնագիտական գործնական հմտություններ",
                            OutComeTypeId = 3,
                            ProfessionId = 1
                        },
                        new FinalOutCome
                        {
                            Name = "Պաշտպանված քոմփյութերային համակարգերի նախագծման կարողություններ",
                            OutComeTypeId = 2,
                            ProfessionId = 1
                        },
                        new FinalOutCome
                        {
                            Name = "Մասնագիտական գործնական գիտելիքներ",
                            OutComeTypeId = 1,
                            ProfessionId = 1,
                            IsNew = true
                        }
                    };
                    _context.FinalOutComes.AddRange(outComes);
                    _context.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
